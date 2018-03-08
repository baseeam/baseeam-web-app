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
using BaseEAM.Web.Framework.Filters;

namespace BaseEAM.Web.Controllers.Asset
{
    public class AutoNumberController : BaseController
    {
        #region Fields

        private readonly IRepository<AutoNumber> _autoNumberRepository;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IRepository<UnitOfMeasure> _unitOfMeasureRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public AutoNumberController(IRepository<AutoNumber> autoNumberRepository,
            IAutoNumberService autoNumberService,
            IRepository<UnitOfMeasure> unitOfMeasureRepository,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._autoNumberRepository = autoNumberRepository;
            this._localizationService = localizationService;
            this._autoNumberService = autoNumberService;
            this._unitOfMeasureRepository = unitOfMeasureRepository;
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
            var autoNumberEntityTypeFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "EntityType",
                ResourceKey = "AutoNumber.EntityType",
                DbColumn = "EntityType",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "Entities",
                IsRequiredField = false
            };
            model.Filters.Add(autoNumberEntityTypeFilter);

            return model;
        }

        #endregion

        #region AutoNumbers

        [BaseEamAuthorize(PermissionNames = "Administration.AutoNumber.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.AutoNumberSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.AutoNumberSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.AutoNumber.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.AutoNumberSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.AutoNumberSearchModel] = model;

                PagedResult<AutoNumber> data = _autoNumberService.GetAutoNumbers(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Administration.AutoNumber.Create")]
        public ActionResult Create()
        {
            var autoNumber = new AutoNumber { IsNew = true };
            _autoNumberRepository.InsertAndCommit(autoNumber);
            return Json(new { Id = autoNumber.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.AutoNumber.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<AutoNumber>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Administration.AutoNumber.Create,Administration.AutoNumber.Read,Administration.AutoNumber.Update")]
        public ActionResult Edit(long id)
        {
            var autoNumber = _autoNumberRepository.GetById(id);
            var model = autoNumber.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.AutoNumber.Create,Administration.AutoNumber.Update")]
        public ActionResult Edit(AutoNumberModel model)
        {
            var autoNumber = _autoNumberRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                autoNumber = model.ToEntity(autoNumber);
                //always set IsNew to false when saving
                autoNumber.IsNew = false;
                _autoNumberRepository.Update(autoNumber);

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
        [BaseEamAuthorize(PermissionNames = "Administration.AutoNumber.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var autoNumber = _autoNumberRepository.GetById(id);

            if (!_autoNumberService.IsDeactivable(autoNumber))
            {
                ModelState.AddModelError("AutoNumber", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _autoNumberRepository.DeactivateAndCommit(autoNumber);
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
        [BaseEamAuthorize(PermissionNames = "Administration.AutoNumber.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var autoNumbers = new List<AutoNumber>();
            foreach (long id in selectedIds)
            {
                var autoNumber = _autoNumberRepository.GetById(id);
                if (autoNumber != null)
                {
                    if (!_autoNumberService.IsDeactivable(autoNumber))
                    {
                        ModelState.AddModelError("AutoNumber", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        autoNumbers.Add(autoNumber);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var autoNumber in autoNumbers)
                    _autoNumberRepository.Deactivate(autoNumber);
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
    }
}