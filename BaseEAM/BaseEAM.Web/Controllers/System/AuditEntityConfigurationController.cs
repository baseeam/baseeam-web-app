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
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Filters;
using System.Text;

namespace BaseEAM.Web.Controllers
{
    public class AuditEntityConfigurationController : BaseController
    {
        #region Fields

        private readonly IRepository<AuditEntityConfiguration> _auditEntityConfigurationRepository;
        private readonly IAuditEntityConfigurationService _auditEntityConfigurationService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public AuditEntityConfigurationController(IRepository<AuditEntityConfiguration> auditEntityConfigurationRepository,
            IAuditEntityConfigurationService auditEntityConfigurationService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._auditEntityConfigurationRepository = auditEntityConfigurationRepository;
            this._localizationService = localizationService;
            this._auditEntityConfigurationService = auditEntityConfigurationService;
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
            var auditEntityConfigurationNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "EntityType",
                ResourceKey = "EntityType",
                DbColumn = "EntityType",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "Entities",
                AutoBind = false
            };
            model.Filters.Add(auditEntityConfigurationNameFilter);

            return model;
        }

        #endregion

        #region AuditEntityConfigurations

        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.AuditEntityConfigurationSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.AuditEntityConfigurationSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.AuditEntityConfigurationSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.AuditEntityConfigurationSearchModel] = model;

                PagedResult<AuditEntityConfiguration> data = _auditEntityConfigurationService.GetAuditEntityConfigurations(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Create")]
        public ActionResult Create()
        {
            var auditEntityConfiguration = new AuditEntityConfiguration { IsNew = true };
            _auditEntityConfigurationRepository.InsertAndCommit(auditEntityConfiguration);
            return Json(new { Id = auditEntityConfiguration.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<AuditEntityConfiguration>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Create,System.AuditEntityConfiguration.Read,System.AuditEntityConfiguration.Update")]
        public ActionResult Edit(long id)
        {
            var auditEntityConfiguration = _auditEntityConfigurationRepository.GetById(id);
            var model = auditEntityConfiguration.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Create,System.AuditEntityConfiguration.Update")]
        public ActionResult Edit(AuditEntityConfigurationModel model)
        {
            var auditEntityConfiguration = _auditEntityConfigurationRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                auditEntityConfiguration = model.ToEntity(auditEntityConfiguration);
                //always set IsNew to false when saving
                auditEntityConfiguration.IsNew = false;
                _auditEntityConfigurationRepository.Update(auditEntityConfiguration);

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
        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var auditEntityConfiguration = _auditEntityConfigurationRepository.GetById(id);

            if (!_auditEntityConfigurationService.IsDeactivable(auditEntityConfiguration))
            {
                ModelState.AddModelError("AuditEntityConfiguration", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _auditEntityConfigurationRepository.DeactivateAndCommit(auditEntityConfiguration);
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
        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var auditEntityConfigurations = new List<AuditEntityConfiguration>();
            foreach (long id in selectedIds)
            {
                var auditEntityConfiguration = _auditEntityConfigurationRepository.GetById(id);
                if (auditEntityConfiguration != null)
                {
                    if (!_auditEntityConfigurationService.IsDeactivable(auditEntityConfiguration))
                    {
                        ModelState.AddModelError("AuditEntityConfiguration", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        auditEntityConfigurations.Add(auditEntityConfiguration);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var auditEntityConfiguration in auditEntityConfigurations)
                    _auditEntityConfigurationRepository.Deactivate(auditEntityConfiguration);
                this._dbContext.SaveChanges();
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Read")]
        public ActionResult ExcludedColumnList(long? auditEntityConfigurationId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var auditEntityConfiguration = _auditEntityConfigurationRepository.GetById(auditEntityConfigurationId);
            var excludedColumns = auditEntityConfiguration.ExcludedColumns;
            var excludedColumnList = new List<string>();

            if (!string.IsNullOrEmpty(excludedColumns))
            {
                excludedColumnList = excludedColumns.Split(',').ToList();
            }
            var models = new List<AuditEntityConfigurationModel>();
            foreach (var excludedColumn in excludedColumnList)
            {
                var column = _dbContext.GetColumnNames(auditEntityConfiguration.EntityType, excludedColumn).FirstOrDefault();
                if(column != null)
                {
                    var model = auditEntityConfiguration.ToModel();
                    model.ExcludedColumn = column;
                    models.Add(model);
                }
               
            }
            var gridModel = new DataSourceResult
            {
                Data = models.PagedForCommand(command),
                Total = models.Count()
            };

            return Json(gridModel);
        }

        // <summary>
        /// Save an Exclude column for AuditEntityConfiguration
        /// </summary>
        /// <param name="auditEntityConfigurationId"></param>
        /// <param name="excludedColumn"></param>
        /// <returns></returns>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Create,System.AuditEntityConfiguration.Update")]
        public ActionResult SaveExcludedColumn(long? auditEntityConfigurationId, string excludedColumn)
        {
            var auditEntityConfiguration = _auditEntityConfigurationRepository.GetById(auditEntityConfigurationId);
            var column = _dbContext.GetColumnNames(auditEntityConfiguration.EntityType, excludedColumn).FirstOrDefault();
            var excludedColumns = auditEntityConfiguration.ExcludedColumns;
            if (string.IsNullOrEmpty(column))
            {
                ModelState.AddModelError("AuditEntityConfiguration", string.Format(_localizationService.GetResource("AuditEntityConfiguration.CannotFindColumn"),column,auditEntityConfiguration.EntityType));
            }
            if (!string.IsNullOrEmpty(excludedColumns))
            {
                var assetTypeList = excludedColumns.Split(',').ToList();
                if (assetTypeList.Contains(column))
                {
                    ModelState.AddModelError("AuditEntityConfiguration", string.Format(_localizationService.GetResource("AuditEntityConfiguration.ExcludedColumnAlreadyExists"), column));
                }
            }
            if (ModelState.IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!string.IsNullOrEmpty(excludedColumns))
                {
                    stringBuilder.Append(excludedColumns);
                    stringBuilder.Append(",");
                }
                stringBuilder.Append(column);

                auditEntityConfiguration.ExcludedColumns = stringBuilder.ToString();
                _auditEntityConfigurationRepository.UpdateAndCommit(auditEntityConfiguration);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        /// <summary>
        /// Delete an exclude column within the AuditEntityConfiguration(parentId)
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="id">column name</param>
        /// <returns></returns>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Delete")]
        public ActionResult DeleteExcludedColumn(long? parentId, string id)
        {
            var auditEntityConfiguration = _auditEntityConfigurationRepository.GetById(parentId);
            var column = _dbContext.GetColumnNames(auditEntityConfiguration.EntityType, id).FirstOrDefault();
            var excludedColumns = auditEntityConfiguration.ExcludedColumns;
            if (ModelState.IsValid)
            {
                var excludedColumnList = excludedColumns.Split(',').ToList();
                excludedColumnList.Remove(column);
                auditEntityConfiguration.ExcludedColumns = string.Join(",", excludedColumnList);

                _auditEntityConfigurationRepository.UpdateAndCommit(auditEntityConfiguration);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        /// <summary>
        /// Clear all exclude columns within the AuditEntityConfiguration
        /// </summary>
        /// <param name="auditEntityConfigurationId"></param>
        /// <returns></returns>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.AuditEntityConfiguration.Delete")]
        public ActionResult ClearAllExcludedColumns(AuditEntityConfigurationModel model)
        {
            var auditEntityConfiguration = _auditEntityConfigurationRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                auditEntityConfiguration.EntityType = model.EntityType;
                auditEntityConfiguration.ExcludedColumns = "";
                _auditEntityConfigurationRepository.UpdateAndCommit(auditEntityConfiguration);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }
        #endregion
    }
}