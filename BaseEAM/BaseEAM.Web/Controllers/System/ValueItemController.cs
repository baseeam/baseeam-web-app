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
using BaseEAM.Web.Framework.Filters;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class ValueItemController : BaseController
    {
        #region Fields
        private readonly IRepository<ValueItem> _valueItemRepository;
        private readonly IRepository<ValueItemCategory> _valueItemCategoryRepository;
        private readonly IValueItemService _valueItemService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;
        #endregion

        #region Constructors

        public ValueItemController(ILocalizationService localizationService,
            IRepository<ValueItem> valueItemRepository,
            IRepository<ValueItemCategory> valueItemCategoryRepository,
            IValueItemService valueItemService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._localizationService = localizationService;
            this._valueItemCategoryRepository = valueItemCategoryRepository;
            this._valueItemRepository = valueItemRepository;
            this._valueItemService = valueItemService;
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
                Name = "ValueItemCategory",
                ResourceKey = "ValueItemCategory",
                DbColumn = "ValueItemCategory.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "ValueItemCategory",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(moduleNameFilter);

            var valueItemNameFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "ValueItemName",
                ResourceKey = "ValueItem.Name",
                DbColumn = "ValueItem.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "ValueItem",
                MvcAction = "ValueItems",
                IsRequiredField = false,
                ParentFieldName = "ValueItemCategory"
            };
            model.Filters.Add(valueItemNameFilter);

            return model;
        }

        #endregion

        #region Methods

        [BaseEamAuthorize(PermissionNames = "System.ValueItem.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ValueItemSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ValueItemSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.ValueItem.Read")]
        public ActionResult List(string searchValues, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ValueItemSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();
            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.ValueItemSearchModel] = model;

                PagedResult<ValueItem> valueItems = _valueItemService.GetValueItems(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = valueItems.Result.Select(x => x.ToModel()),
                    Total = valueItems.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.ValueItem.Create,System.ValueItem.Update,System.ValueItem.Delete")]
        public ActionResult SaveChanges([Bind(Prefix = "updated")]List<ValueItemModel> updatedItems,
            [Bind(Prefix = "created")]List<ValueItemModel> createdItems,
            [Bind(Prefix = "deleted")]List<ValueItemModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create ValueItems
                    if (createdItems != null)
                    {
                        foreach (var model in createdItems)
                        {

                            var valueItemCategory = _valueItemCategoryRepository.GetAll().Where(c => c.Name == model.ValueItemCategory.Name).FirstOrDefault();
                            if (valueItemCategory == null)
                            {
                                valueItemCategory = new ValueItemCategory
                                {
                                    Name = model.ValueItemCategory.Name
                                };
                                _valueItemCategoryRepository.InsertAndCommit(valueItemCategory);
                            }

                            var valueItem = _valueItemRepository.GetAll().Where(c => c.Name == model.Name && c.ValueItemCategory.Name == model.ValueItemCategory.Name).FirstOrDefault();
                            if (valueItem == null)
                            {
                                valueItem = new ValueItem
                                {
                                    Name = model.Name,
                                    ValueItemCategoryId = valueItemCategory.Id
                                };
                                _valueItemRepository.Insert(valueItem);
                            }

                        }
                    }

                    //Update ValueItems
                    if (updatedItems != null)
                    {
                        foreach (var model in updatedItems)
                        {
                            var valueItemCategory = _valueItemCategoryRepository.GetById(model.ValueItemCategory.Id);
                            valueItemCategory.Name = model.ValueItemCategory.Name;
                            _valueItemCategoryRepository.Update(valueItemCategory);

                            var valueItem = _valueItemRepository.GetById(model.Id);
                            valueItem.Name = model.Name;
                            _valueItemRepository.Update(valueItem);
                        }
                    }

                    //Delete ValueItems
                    if (deletedItems != null)
                    {
                        foreach (var model in deletedItems)
                        {
                            var valueItem = _valueItemRepository.GetById(model.Id);
                            if (valueItem != null)
                            {
                                _valueItemRepository.Deactivate(valueItem);
                            }
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

        [HttpPost]
        public JsonResult ValueItems(long parentValue, string param)
        {
            var items = _valueItemRepository.GetAll().Where(v => v.ValueItemCategoryId == parentValue && v.Name.Contains(param)).ToList();
            var choices = new List<SelectListItem>();
            foreach (var item in items)
            {
                choices.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        #endregion
    }
}