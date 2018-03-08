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

namespace BaseEAM.Web.Controllers
{
    public class ModuleController : BaseController
    {
        #region Fields

        private readonly IRepository<Module> _moduleRepository;
        private readonly IRepository<Feature> _featureRepository;
        private readonly IRepository<FeatureAction> _featureActionRepository;
        private readonly IModuleService _moduleService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public ModuleController(IRepository<Module> moduleRepository,
            IRepository<Feature> featureRepository,
            IRepository<FeatureAction> featureActionRepository,
            IModuleService moduleService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._moduleRepository = moduleRepository;
            this._featureRepository = featureRepository;
            this._featureActionRepository = featureActionRepository;
            this._moduleService = moduleService;
            this._localizationService = localizationService;
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
            var moduleNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "ModuleName",
                ResourceKey = "Module.Name",
                DbColumn = "Module.Id",
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
                DbColumn = "Feature.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Module",
                MvcAction = "FeatureList",
                IsRequiredField = false,
                ParentFieldName = "ModuleName"
            };
            model.Filters.Add(featureNameFilter);

            return model;
        }

        #endregion

        #region Methods

        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ModuleSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ModuleSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, string searchValues)
        {
            var model = _httpContext.Session[SessionKey.ModuleSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();
            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.ModuleSearchModel] = model;

                IEnumerable<Module> modules = _moduleService.GetModules(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, null);

                var gridModel = new DataSourceResult
                {
                    Data = modules.Select(x => x.ToModel()),
                    Total = modules.Count()
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        public ActionResult Features(long moduleId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var features = _featureRepository.GetAll().Where(f => f.ModuleId == moduleId).OrderBy(f => f.DisplayOrder).ToList();
            var gridModel = new DataSourceResult
            {
                Data = features.Select(x => x.ToModel()),
                Total = features.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost]
        public ActionResult FeatureActions(long featureId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var featureActions = _featureActionRepository.GetAll().Where(f => f.FeatureId == featureId).OrderBy(f => f.DisplayOrder).ToList();
            var gridModel = new DataSourceResult
            {
                Data = featureActions.Select(x => x.ToModel()),
                Total = featureActions.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpGet]
        public ActionResult CreateModuleView()
        {
            return PartialView("_CreateModule", new ModuleModel());
        }

        [HttpPost]
        public ActionResult CreateModule(ModuleModel model)
        {
            if (ModelState.IsValid)
            {
                var module = model.ToEntity();
                _moduleRepository.InsertAndCommit(module);
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpGet]
        public ActionResult CreateFeatureView()
        {
            return PartialView("_CreateFeature", new FeatureModel());
        }

        [HttpPost]
        public ActionResult CreateFeature(FeatureModel model)
        {
            if (ModelState.IsValid)
            {
                var feature = model.ToEntity();
                _featureRepository.InsertAndCommit(feature);
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpGet]
        public ActionResult CreateFeatureActionView()
        {
            return PartialView("_CreateFeatureAction", new FeatureActionModel());
        }

        [HttpPost]
        public ActionResult CreateFeatureAction(FeatureActionModel model)
        {
            if (ModelState.IsValid)
            {
                var featureAction = model.ToEntity();
                _featureActionRepository.InsertAndCommit(featureAction);

                var feature = _featureRepository.GetById(featureAction.FeatureId);
                //then insert a new permission record
                var permissionRecord = new PermissionRecord
                {
                    ModuleId = feature.ModuleId,
                    FeatureId = feature.Id,
                    FeatureActionId = featureAction.Id,
                    Name = feature.Module.Name.Replace(" ", "") + "."
                            + feature.Name.Replace(" ", "") + "."
                            + featureAction.Name.Replace(" ", "")
                };
                _permissionService.InsertPermissionRecord(permissionRecord);

                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        public JsonResult FeatureList(long parentValue)
        {
            var features = _featureRepository.GetAll()
                .Where(f => f.ModuleId == parentValue)
                .Select(f => new SelectListItem { Text = f.Name, Value = f.Id.ToString() })
                .ToList();
            if (features.Count > 0)
            {
                features.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(features);
        }

        #endregion
    }
}