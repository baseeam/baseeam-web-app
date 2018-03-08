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
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Filters;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework;

namespace BaseEAM.Web.Controllers
{
    public class AssignmentGroupController : BaseController
    {
        #region Fields

        private readonly IRepository<AssignmentGroup> _assignmentGroupRepository;
        private readonly IRepository<AssignmentGroupUser> _assignmentGroupUserRepository;
        private readonly IAssignmentGroupService _assignmentGroupService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public AssignmentGroupController(IRepository<AssignmentGroup> assignmentGroupRepository,
            IRepository<AssignmentGroupUser> assignmentGroupUserRepository,
            IAssignmentGroupService assignmentGroupService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._assignmentGroupRepository = assignmentGroupRepository;
            this._assignmentGroupUserRepository = assignmentGroupUserRepository;
            this._localizationService = localizationService;
            this._assignmentGroupService = assignmentGroupService;
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
            var assignmentGroupNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "AssignmentGroupName",
                ResourceKey = "AssignmentGroup.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(assignmentGroupNameFilter);

            return model;
        }

        #endregion

        #region AssignmentGroups

        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.AssignmentGroupSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.AssignmentGroupSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.AssignmentGroupSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.AssignmentGroupSearchModel] = model;

                PagedResult<AssignmentGroup> data = _assignmentGroupService.GetAssignmentGroups(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Create")]
        public ActionResult Create()
        {
            var assignmentGroup = new AssignmentGroup { IsNew = true };
            _assignmentGroupRepository.InsertAndCommit(assignmentGroup);
            return Json(new { Id = assignmentGroup.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<AssignmentGroup>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Create,Workflow.AssignmentGroup.Read,Workflow.AssignmentGroup.Update")]
        public ActionResult Edit(long id)
        {
            var assignmentGroup = _assignmentGroupRepository.GetById(id);
            var model = assignmentGroup.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Create,Workflow.AssignmentGroup.Update")]
        public ActionResult Edit(AssignmentGroupModel model)
        {
            var assignmentGroup = _assignmentGroupRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                assignmentGroup = model.ToEntity(assignmentGroup);

                //always set IsNew to false when saving
                assignmentGroup.IsNew = false;
                _assignmentGroupRepository.Update(assignmentGroup);

                //commit all changes
                this._dbContext.SaveChanges();

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
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var assignmentGroup = _assignmentGroupRepository.GetById(id);

            if (!_assignmentGroupService.IsDeactivable(assignmentGroup))
            {
                ModelState.AddModelError("AssignmentGroup", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _assignmentGroupRepository.DeactivateAndCommit(assignmentGroup);
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
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var assignmentGroups = new List<AssignmentGroup>();
            foreach (long id in selectedIds)
            {
                var assignmentGroup = _assignmentGroupRepository.GetById(id);
                if (assignmentGroup != null)
                {
                    if (!_assignmentGroupService.IsDeactivable(assignmentGroup))
                    {
                        ModelState.AddModelError("AssignmentGroup", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        assignmentGroups.Add(assignmentGroup);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var assignmentGroup in assignmentGroups)
                    _assignmentGroupRepository.Deactivate(assignmentGroup);
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

        #region AssignmentGroupUsers

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Read")]
        public ActionResult AssignmentGroupUserList(long assignmentGroupId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var assignmentGroupUsers = _assignmentGroupRepository.GetById(assignmentGroupId).AssignmentGroupUsers;
            var queryable = assignmentGroupUsers.AsQueryable();
            queryable = sort == null ? queryable.OrderBy(a => a.Name) : queryable.Sort(sort);
            var data = queryable.ToList().Select(x => x.ToModel()).ToList();
            var gridModel = new DataSourceResult
            {
                Data = data.PagedForCommand(command),
                Total = assignmentGroupUsers.Count()
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Read,Workflow.AssignmentGroup.Create,Workflow.AssignmentGroup.Read,Workflow.AssignmentGroup.Update")]
        public ActionResult AssignmentGroupUser(long id)
        {
            var assignmentGroupUser = _assignmentGroupUserRepository.GetById(id);
            var model = assignmentGroupUser.ToModel();
            var html = this.AssignmentGroupUserPanel(model);
            return Json(new { Id = assignmentGroupUser.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Create")]
        public ActionResult CreateAssignmentGroupUser(long assignmentGroupId)
        {
            var assignmentGroupUser = new AssignmentGroupUser
            {
                IsNew = true,
                AssignmentGroupId = assignmentGroupId
            };
            _assignmentGroupUserRepository.InsertAndCommit(assignmentGroupUser);

            var model = new AssignmentGroupUserModel();
            model = assignmentGroupUser.ToModel();
            var html = this.AssignmentGroupUserPanel(model);

            return Json(new { Id = assignmentGroupUser.Id, Html = html });
        }

        [NonAction]
        public string AssignmentGroupUserPanel(AssignmentGroupUserModel model)
        {
            var html = this.RenderPartialViewToString("_AssignmentGroupUserDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Create,Workflow.AssignmentGroup.Update")]
        public ActionResult SaveAssignmentGroupUser(AssignmentGroupUserModel model)
        {
            if (ModelState.IsValid)
            {
                var assignmentGroupUser = _assignmentGroupUserRepository.GetById(model.Id);
                //always set IsNew to false when saving
                assignmentGroupUser.IsNew = false;
                assignmentGroupUser.SiteId = model.SiteId;
                assignmentGroupUser.UserId = model.UserId;
                assignmentGroupUser.IsDefaultUser = model.IsDefaultUser;
                _assignmentGroupUserRepository.UpdateAndCommit(assignmentGroupUser);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Create,Workflow.AssignmentGroup.Update")]
        public ActionResult CancelAssignmentGroupUser(long id)
        {
            var assignmentGroupUser = _assignmentGroupUserRepository.GetById(id);
            if (assignmentGroupUser.IsNew == true)
            {
                _assignmentGroupUserRepository.DeleteAndCommit(assignmentGroupUser);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Delete")]
        public ActionResult DeleteAssignmentGroupUser(long? parentId, long id)
        {
            var assignmentGroupUser = _assignmentGroupUserRepository.GetById(id);
            _assignmentGroupUserRepository.DeactivateAndCommit(assignmentGroupUser);
            return new NullJsonResult();

        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.AssignmentGroup.Delete")]
        public ActionResult DeleteSelectedAssignmentGroupUsers(long? parentId, long[] selectedIds)
        {
            foreach (var id in selectedIds)
            {
                var assignmentGroupUser = _assignmentGroupUserRepository.GetById(id);
                _assignmentGroupUserRepository.Deactivate(assignmentGroupUser);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion
    }
}