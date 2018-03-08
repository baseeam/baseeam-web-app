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
    public class MeterController : BaseController
    {
        #region Fields

        private readonly IRepository<Meter> _meterRepository;
        private readonly IMeterService _meterService;
        private readonly IRepository<UnitOfMeasure> _unitOfMeasureRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public MeterController(IRepository<Meter> meterRepository,
            IMeterService meterService,
            IRepository<UnitOfMeasure> unitOfMeasureRepository,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._meterRepository = meterRepository;
            this._localizationService = localizationService;
            this._meterService = meterService;
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
            var meterNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "MeterName",
                ResourceKey = "Meter.Name",
                DbColumn = "Meter.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "Meter",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(meterNameFilter);

            return model;
        }

        #endregion

        #region Meters

        [BaseEamAuthorize(PermissionNames = "Asset.Meter.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.MeterSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.MeterSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Meter.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.MeterSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.MeterSearchModel] = model;

                PagedResult<Meter> data = _meterService.GetMeters(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Asset.Meter.Create")]
        public ActionResult Create()
        {
            var meter = new Meter { IsNew = true };
            _meterRepository.InsertAndCommit(meter);
            return Json(new { Id = meter.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Meter.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Meter>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Asset.Meter.Create,Asset.Meter.Read,Asset.Meter.Update")]
        public ActionResult Edit(long id)
        {
            var meter = _meterRepository.GetById(id);
            var model = meter.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Meter.Create,Asset.Meter.Update")]
        public ActionResult Edit(MeterModel model)
        {
            var meter = _meterRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                meter = model.ToEntity(meter);
                //always set IsNew to false when saving
                meter.IsNew = false;
                _meterRepository.Update(meter);

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
        [BaseEamAuthorize(PermissionNames = "Asset.Meter.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var meter = _meterRepository.GetById(id);

            if (!_meterService.IsDeactivable(meter))
            {
                ModelState.AddModelError("Meter", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _meterRepository.DeactivateAndCommit(meter);
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
        [BaseEamAuthorize(PermissionNames = "Asset.Meter.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var meters = new List<Meter>();
            foreach (long id in selectedIds)
            {
                var meter = _meterRepository.GetById(id);
                if (meter != null)
                {
                    if (!_meterService.IsDeactivable(meter))
                    {
                        ModelState.AddModelError("Meter", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        meters.Add(meter);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var meter in meters)
                    _meterRepository.Deactivate(meter);
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
        public ActionResult MeterInfo(long? meterId)
        {
            if (meterId == null || meterId == 0)
                return new NullJsonResult();

            var meterInfo = _meterRepository.GetById(meterId).ToModel();
            return Json(new { meterInfo = meterInfo });
        }

        #endregion
    }
}