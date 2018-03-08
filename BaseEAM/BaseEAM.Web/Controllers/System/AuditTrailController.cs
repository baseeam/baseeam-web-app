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
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseEAM.Web.Framework.Filters;
using BaseEAM.Web.Framework;
using EntityFramework.Audit;

namespace BaseEAM.Web.Controllers
{
    public class AuditTrailController : BaseController
    {
        #region Fields

        private readonly IRepository<AuditTrail> _auditTrailRepository;
        private readonly IAuditTrailService _auditTrailService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public AuditTrailController(IRepository<AuditTrail> auditTrailRepository,
            IAuditTrailService auditTrailService,
            IStoreService storeService,
            IAutoNumberService autoNumberService,
            IItemService itemService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._auditTrailRepository = auditTrailRepository;
            this._localizationService = localizationService;
            this._auditTrailService = auditTrailService;
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

            var auditTrailDateFromFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "AuditTrailDateFrom",
                ResourceKey = "AuditTrail.DateFrom",
                DbColumn = "AuditTrail.Date",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(auditTrailDateFromFilter);

            var auditTrailDateToFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "AuditTrailDateTo",
                ResourceKey = "AuditTrail.DateTo",
                DbColumn = "AuditTrail.Date",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(auditTrailDateToFilter);

            return model;
        }

        #endregion

        #region AuditTrails

        [BaseEamAuthorize(PermissionNames = "System.AuditTrail.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.AuditTrailSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.AuditTrailSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.AuditTrail.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.AuditTrailSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.AuditTrailSearchModel] = model;

                PagedResult<AuditTrail> auditTrails = _auditTrailService.GetAuditTrails(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = auditTrails.Result.ToList();

                var auditTrailModels = new List<AuditTrailModel>();
                foreach (var auditrail in result)
                {
                    var auditTrailModel = auditrail.ToModel();
                    string logXml = auditrail.LogXml;
                    var auditLog = AuditLog.FromXml(logXml);
                    auditTrailModel.UserName = auditLog.Username;
                    auditTrailModel.Date = auditLog.Date;
                    auditTrailModels.Add(auditTrailModel);
                }

                var gridModel = new DataSourceResult
                {
                    Data = auditTrailModels,
                    Total = auditTrails.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }
            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.AuditTrail.Read")]
        public ActionResult AuditEntities(long auditTrailId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var audiTrail = _auditTrailRepository.GetById(auditTrailId);

            string logXml = audiTrail.LogXml;
            var auditLog = AuditLog.FromXml(logXml);
            var auditEntities = auditLog.Entities;
            var models = new List<AuditEntityModel>();
            foreach (var auditEntity in auditEntities)
            {
                var model = new AuditEntityModel();
                model.AuditTrailId = audiTrail.Id;
                model.Action = auditEntity.Action.ToString();
                model.EntityType = auditEntity.Type.ToString();
                model.Key = auditEntity.Keys[0].Name;
                model.Value = auditEntity.Keys[0].Value.ToString();
                models.Add(model);
            }
            var gridModel = new DataSourceResult
            {
                Data = models.PagedForCommand(command),
                Total = models.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.AuditTrail.Read")]
        public ActionResult AuditProperties(long auditTrailId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var audiTrail = _auditTrailRepository.GetById(auditTrailId);
            var auditTrailModel = audiTrail.ToModel();

            string logXml = audiTrail.LogXml;
            var auditLog = AuditLog.FromXml(logXml);

            var auditEntities = auditLog.Entities;
            var models = new List<AuditPropertyModel>();
            foreach (var auditEntity in auditEntities)
            {
                var properties = auditEntity.Properties.ToList();
                foreach (var property in properties)
                {
                    var model = new AuditPropertyModel();
                    model.Name = property.Name;
                    model.Type = property.Type;
                    if (property.Type.Equals(PropertyType.SystemByte))
                    {
                        model.Original = property.Original != null ? System.Text.Encoding.UTF8.GetString((byte[])property.Original) : "";
                        model.Current = property.Current != null ? System.Text.Encoding.UTF8.GetString((byte[])property.Current) : "";
                    }
                    else
                    {
                        model.Original = property.Original != null ? property.Original.ToString() : "";
                        model.Current = property.Current != null ? property.Current.ToString() : "";
                    }
                    models.Add(model);
                }
            }

            var gridModel = new DataSourceResult
            {
                Data = models.PagedForCommand(command),
                Total = models.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

    }
}