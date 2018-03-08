/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using BaseEAM.Services;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Filters;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class SecurityGroupController : BaseController
    {
        #region Fields

        private readonly IRepository<SecurityGroup> _securityGroupRepository;
        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ISecurityGroupService _securityGroupService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public SecurityGroupController(IRepository<SecurityGroup> securityGroupRepository,
            IRepository<Site> siteRepository,
            IRepository<User> userRepository,
            ISecurityGroupService securityGroupService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._securityGroupRepository = securityGroupRepository;
            this._siteRepository = siteRepository;
            this._userRepository = userRepository;
            this._localizationService = localizationService;
            this._securityGroupService = securityGroupService;
            this._permissionService = permissionService;
            this._httpContext = httpContext;
            this._workContext = workContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Utilities

        private SearchModel BuildSearchModel()
        {
            var model = new SearchModel();
            var securityGroupNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "SecurityGroupName",
                ResourceKey = "SecurityGroup.Name",
                DbColumn = "Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "SecurityGroup",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(securityGroupNameFilter);

            return model;
        }

        #endregion

        #region SecurityGroups

        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.SecurityGroupSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.SecurityGroupSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.SecurityGroupSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();
            //validate
            var errorFilters = model.Validate(searchValues);
            foreach (var filter in errorFilters)
            {
                ModelState.AddModelError(filter.Name, _localizationService.GetResource(filter.ResourceKey + ".Required"));
            }
            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.SecurityGroupSearchModel] = model;

                PagedResult<SecurityGroup> data = _securityGroupService.GetSecurityGroups(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => x.ToModel()),
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create")]
        public ActionResult Create()
        {
            var securityGroup = new SecurityGroup { IsNew = true };
            _securityGroupRepository.InsertAndCommit(securityGroup);
            return Json(new { Id = securityGroup.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<SecurityGroup>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create,Security.SecurityGroup.Read,Security.SecurityGroup.Update")]
        public ActionResult Edit(long id)
        {
            var securityGroup = _securityGroupRepository.GetById(id);
            var model = securityGroup.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create,Security.SecurityGroup.Update")]
        public ActionResult Edit(SecurityGroupModel model)
        {
            var securityGroup = _securityGroupRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                securityGroup = model.ToEntity(securityGroup);
                securityGroup.IsNew = false;
                _securityGroupRepository.UpdateAndCommit(securityGroup);

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var securityGroup = _securityGroupRepository.GetById(id);

            if (!_securityGroupService.IsDeactivable(securityGroup))
            {
                ModelState.AddModelError("SecurityGroup", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _securityGroupRepository.DeactivateAndCommit(securityGroup);
                //notification
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var securityGroups = new List<SecurityGroup>();
            foreach (long id in selectedIds)
            {
                var securityGroup = _securityGroupRepository.GetById(id);
                if (securityGroup != null)
                {
                    if (!_securityGroupService.IsDeactivable(securityGroup))
                    {
                        ModelState.AddModelError("SecurityGroup", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        securityGroups.Add(securityGroup);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var securityGroup in securityGroups)
                    _securityGroupRepository.Deactivate(securityGroup);
                this._dbContext.SaveChanges();
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Sites

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Read")]
        public ActionResult SiteList(long securityGroupId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var sites = _securityGroupRepository.GetById(securityGroupId).Sites;
            if (sites.Count == 0)
            {
                return Json(new DataSourceResult());
            }
            else
            {
                var queryable = sites.AsQueryable<Site>();
                queryable = sort == null ? queryable.OrderBy(a => a.CreatedDateTime) : queryable.Sort(sort);
                var data = queryable.ToList().Select(x => x.ToModel()).ToList();
                var gridModel = new DataSourceResult
                {
                    Data = data.PagedForCommand(command),
                    Total = sites.Count()
                };

                return Json(gridModel);
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create,Administration.Site.Update")]
        public ActionResult AddSites(long securityGroupId, long[] selectedIds)
        {
            var securityGroup = _securityGroupRepository.GetById(securityGroupId);
            foreach(var id in selectedIds)
            {
                var existed = securityGroup.Sites.Any(s => s.Id == id);
                if(!existed)
                {
                    var site = _siteRepository.GetById(id);
                    securityGroup.Sites.Add(site);
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create,Administration.Site.Update")]
        public ActionResult DeleteSite(long? parentId, long id)
        {
            var securityGroup = _securityGroupRepository.GetById(parentId);
            var site = _siteRepository.GetById(id);
            //For many-many, delete by set foreign key to null
            securityGroup.Sites.Remove(site);

            _securityGroupRepository.UpdateAndCommit(securityGroup);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create,Administration.Site.Update")]
        public ActionResult DeleteSelectedSites(long? parentId, long[] selectedIds)
        {
            var securityGroup = _securityGroupRepository.GetById(parentId);
            foreach (long id in selectedIds)
            {
                var site = _siteRepository.GetById(id);
                //For many-many, need to remove from parent
                securityGroup.Sites.Remove(site);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Users

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Read")]
        public ActionResult UserList(long securityGroupId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var users = _securityGroupRepository.GetById(securityGroupId).Users;
            if (users.Count == 0)
            {
                return Json(new DataSourceResult());
            }
            else
            {
                var queryable = users.AsQueryable<User>();
                queryable = sort == null ? queryable.OrderBy(a => a.CreatedDateTime) : queryable.Sort(sort);
                var data = queryable.ToList().Select(x => x.ToModel()).ToList();
                var gridModel = new DataSourceResult
                {
                    Data = data.PagedForCommand(command),
                    Total = users.Count()
                };

                return Json(gridModel);
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create,People.User.Update")]
        public ActionResult AddUsers(long securityGroupId, long[] selectedIds)
        {
            var securityGroup = _securityGroupRepository.GetById(securityGroupId);
            foreach (var id in selectedIds)
            {
                var existed = securityGroup.Users.Any(s => s.Id == id);
                if (!existed)
                {
                    var user = _userRepository.GetById(id);
                    securityGroup.Users.Add(user);
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create,People.User.Update")]
        public ActionResult DeleteUser(long? parentId, long id)
        {
            var securityGroup = _securityGroupRepository.GetById(parentId);
            var user = _userRepository.GetById(id);
            //For many-many, delete by set foreign key to null
            securityGroup.Users.Remove(user);

            _securityGroupRepository.UpdateAndCommit(securityGroup);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create,People.User.Update")]
        public ActionResult DeleteSelectedUsers(long? parentId, long[] selectedIds)
        {
            var securityGroup = _securityGroupRepository.GetById(parentId);
            foreach (long id in selectedIds)
            {
                var user = _userRepository.GetById(id);
                //For many-many, need to remove from parent
                securityGroup.Users.Remove(user);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Permissions

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create,Security.SecurityGroup.Update")]
        public ActionResult PermissionList(long securityGroupId, DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildPermissionSearchModel();
            if (ModelState.IsValid)
            {
                model.Update(searchValues);
                PagedResult<PermissionRecord> data = _permissionService.GetPermissionRecords(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var accessControlList = data.Result.Select(x => x.ToModel()).ToList();
                var sgPermissions = _securityGroupRepository.GetById(securityGroupId).PermissionRecords;
                foreach(var ac in accessControlList)
                {
                    ac.SecurityGroupId = securityGroupId;
                    if (sgPermissions.Any(s => s.Name == ac.Name))
                    {                        
                        ac.HasPermission = true;
                    }
                }

                var gridModel = new DataSourceResult
                {
                    Data = accessControlList.ToList(),
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        public ActionResult UpdateAccessControl(List<AccessControlModel> models)
        {
            if (models.Count > 0)
            {
                var securityGroupId = models.ElementAt(0).SecurityGroupId;
                var sgPermissions = _securityGroupRepository.GetById(securityGroupId).PermissionRecords;
                foreach (var model in models)
                {
                    var permission = _permissionService.GetPermissionRecordByName(model.Name);
                    if (model.HasPermission == false)
                    {
                        sgPermissions.Remove(permission);
                    }
                    else
                    {
                        sgPermissions.Add(permission);
                    }
                }

                this._dbContext.SaveChanges();
            }

            return new NullJsonResult();
        }

        private SearchModel BuildPermissionSearchModel()
        {
            var model = new SearchModel();
            var moduleNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "ModuleName",
                ResourceKey = "Module.Name",
                DbColumn = "ModuleId",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "Module",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(moduleNameFilter);

            var featureNameFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "FeatureName",
                ResourceKey = "Feature.Name",
                DbColumn = "FeatureId",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "Feature",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(featureNameFilter);

            return model;
        }

        #endregion
    }
}