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
using System.Web;
using System.Web.Mvc;
using BaseEAM.Web.Extensions;
using System.Linq;
using System;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class UnitConversionController : BaseController
    {
        #region Fields

        private readonly IRepository<UnitConversion> _unitConversionRepository;
        private readonly IRepository<UnitOfMeasure> _unitOfMeasureRepository;
        private readonly IUnitConversionService _unitConversionService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public UnitConversionController(IRepository<UnitConversion> unitConversionRepository,
                    IRepository<UnitOfMeasure> unitOfMeasureRepository,
                    IUnitConversionService unitConversionService,
                    ILocalizationService localizationService,
                    IPermissionService permissionService,
                    HttpContextBase httpContext,
                    IWorkContext workContext,
                    IDbContext dbContext)
        {
            this._unitConversionRepository = unitConversionRepository;
            this._unitOfMeasureRepository = unitOfMeasureRepository;
            this._unitConversionService = unitConversionService;
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

            var unitConversionNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "FromUnitOfMeasure",
                ResourceKey = "UnitConversion.FromUnitOfMeasure",
                DbColumn = "FromUnitOfMeasure.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "UnitOfMeasure",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(unitConversionNameFilter);

            unitConversionNameFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "ToUnitOfMeasure",
                ResourceKey = "UnitConversion.ToUnitOfMeasure",
                DbColumn = "ToUnitOfMeasure.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "UnitOfMeasure",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(unitConversionNameFilter);

            return model;
        }

        #endregion

        #region UnitConversion

        [BaseEamAuthorize(PermissionNames = "Inventory.UnitConversion.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.UnitConversionSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.UnitConversionSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.UnitConversion.Read")]
        public ActionResult List(string searchValues, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.UnitConversionSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();
            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.UnitConversionSearchModel] = model;

                PagedResult<UnitConversion> unitConversions = _unitConversionService.GetUnitConversions(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = unitConversions.Result.Select(x => x.ToModel()),
                    Total = unitConversions.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.UnitConversion.Create,Inventory.UnitConversion.Update,Inventory.UnitConversion.Delete")]
        public ActionResult SaveChanges([Bind(Prefix = "updated")]List<UnitConversionModel> updatedItems,
           [Bind(Prefix = "created")]List<UnitConversionModel> createdItems,
           [Bind(Prefix = "deleted")]List<UnitConversionModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create UnitConversions
                    if (createdItems != null)
                    {
                        foreach (var model in createdItems)
                        {
                            var unitConversion = _unitConversionRepository.GetById(model.Id);
                            if (unitConversion == null)
                            {
                                unitConversion = new UnitConversion
                                {
                                    FromUnitOfMeasureId = model.FromUnitOfMeasure.Id,
                                    ToUnitOfMeasureId = model.ToUnitOfMeasure.Id,
                                    ConversionFactor = model.ConversionFactor,
                                    IsNew = false,
                                };
                                _unitConversionRepository.Insert(unitConversion);
                            }
                        }
                    }

                    //Update UnitConversions
                    if (updatedItems != null)
                    {
                        foreach (var model in updatedItems)
                        {
                            var unitConversion = _unitConversionRepository.GetById(model.Id);
                            unitConversion.FromUnitOfMeasureId = model.FromUnitOfMeasure.Id;
                            unitConversion.ToUnitOfMeasureId = model.ToUnitOfMeasure.Id;
                            unitConversion.ConversionFactor = model.ConversionFactor;
                            _unitConversionRepository.Update(unitConversion);
                        }
                    }

                    //Delete UnitConversions
                    if (deletedItems != null)
                    {
                        foreach (var model in deletedItems)
                        {
                            var unitConversion = _unitConversionRepository.GetById(model.Id);
                            _unitConversionRepository.Deactivate(unitConversion);
                        }
                    }
                    _dbContext.SaveChanges();
                    SuccessNotification(_localizationService.GetResource("Record.Saved"));
                    return new NullJsonResult();
                }
                catch (Exception e)
                {
                    return Json(new { Errors = e.Message });
                }
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region UnitOfMeasureList

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.UnitOfMeasure.Read")]
        public ActionResult UnitOfMeasureList(string content)
        {
            var unitOfMeasures = _unitOfMeasureRepository.GetAll().Where(u => u.Name.Contains(content));
            var result = (from r in unitOfMeasures
                          select new { Id = r.Id, Name = r.Name }).ToList();
            return new JsonResult { Data = result };
        }

        #endregion
    }
}