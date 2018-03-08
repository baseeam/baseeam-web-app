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
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Filters;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class UnitOfMeasureController : BaseController
    {
        #region Fields
        private readonly IRepository<UnitOfMeasure> _unitOfMeasureRepository;
        private readonly IUnitOfMeasureService _unitOfMeasureService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;
        #endregion

        #region Constructors
        public UnitOfMeasureController(IRepository<UnitOfMeasure> unitOfMeasureRepository,
            IUnitOfMeasureService unitOfMeasureService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._unitOfMeasureRepository = unitOfMeasureRepository;
            this._unitOfMeasureService = unitOfMeasureService;
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
            var unitOfMeasureNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "UnitOfMeasure",
                ResourceKey = "UnitOfMeasure.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(unitOfMeasureNameFilter);

            return model;
        }
        #endregion

        #region UnitOfMeasure

        [BaseEamAuthorize(PermissionNames = "Inventory.UnitOfMeasure.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.UnitOfMeasureSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.UnitOfMeasureSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.UnitOfMeasure.Read")]
        public ActionResult List(string searchValues, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.UnitOfMeasureSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();
            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.UnitOfMeasureSearchModel] = model;

                PagedResult<UnitOfMeasure> unitOfMeasures = _unitOfMeasureService.GetUnitOfMeasures(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = unitOfMeasures.Result.Select(x => x.ToModel()),
                    Total = unitOfMeasures.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.UnitOfMeasure.Create,Inventory.UnitOfMeasure.Update,Inventory.UnitOfMeasure.Delete")]
        public ActionResult SaveChanges([Bind(Prefix = "updated")]List<UnitOfMeasureModel> updatedItems,
            [Bind(Prefix = "created")]List<UnitOfMeasureModel> createdItems,
            [Bind(Prefix = "deleted")]List<UnitOfMeasureModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create UnitOfMeasures
                    if (createdItems != null)
                    {
                        foreach (var model in createdItems)
                        {
                            var unitOfMeasure = _unitOfMeasureRepository.GetAll().Where(c => c.Name == model.Name).FirstOrDefault();
                            if (unitOfMeasure == null)
                            {
                                unitOfMeasure = new UnitOfMeasure
                                {
                                    Name = model.Name,
                                    IsNew = false,
                                    Description = model.Description,
                                    Abbreviation = model.Abbreviation

                                };
                                _unitOfMeasureRepository.Insert(unitOfMeasure);
                            }
                        }
                    }

                    //Update UnitOfMeasures
                    if (updatedItems != null)
                    {
                        foreach (var model in updatedItems)
                        {
                            var unitOfMeasure = _unitOfMeasureRepository.GetById(model.Id);
                            unitOfMeasure.Name = model.Name;
                            unitOfMeasure.Description = model.Description;
                            unitOfMeasure.Abbreviation = model.Abbreviation;
                            _unitOfMeasureRepository.Update(unitOfMeasure);
                        }
                    }

                    //Delete UnitOfMeasures
                    if (deletedItems != null)
                    {
                        foreach (var model in deletedItems)
                        {
                            var unitOfMeasure = _unitOfMeasureRepository.GetById(model.Id);
                            _unitOfMeasureRepository.Deactivate(unitOfMeasure);
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
    }
}