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
using System;

namespace BaseEAM.Web.Controllers
{
    public class AdjustController : BaseController
    {
        #region Fields

        private readonly IRepository<Adjust> _adjustRepository;
        private readonly IRepository<AdjustItem> _adjustItemRepository;
        private readonly IAdjustService _adjustService;
        private readonly IStoreService _storeService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IItemService _itemService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public AdjustController(IRepository<Adjust> adjustRepository,
            IRepository<AdjustItem> adjustItemRepository,
            IAdjustService adjustService,
            IStoreService storeService,
            IAutoNumberService autoNumberService,
            IItemService itemService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._adjustRepository = adjustRepository;
            this._adjustItemRepository = adjustItemRepository;
            this._localizationService = localizationService;
            this._adjustService = adjustService;
            this._storeService = storeService;
            this._autoNumberService = autoNumberService;
            this._itemService = itemService;
            this._dateTimeHelper = dateTimeHelper;
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
            var adjustNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Number",
                ResourceKey = "Adjust.Number",
                DbColumn = "Adjust.Number, Adjust.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(adjustNameFilter);

            var isApprovedFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "IsApproved",
                ResourceKey = "Adjust.IsApproved",
                DbColumn = "Adjust.IsApproved",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Boolean,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "True,False",
                CsvValueList = "True,False",
                IsRequiredField = false
            };
            model.Filters.Add(isApprovedFilter);

            var siteFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "Site",
                ResourceKey = "Site",
                DbColumn = "Site.Id",
                Value = this._workContext.CurrentUser.DefaultSiteId,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Site",
                MvcAction = "SiteList",
                IsRequiredField = false
            };
            model.Filters.Add(siteFilter);

            var storeFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "Store",
                ResourceKey = "Store",
                DbColumn = "Store.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Site",
                MvcAction = "StoreList",
                IsRequiredField = false,
                ParentFieldName = "Site"
            };

            model.Filters.Add(storeFilter);

            var adjustDateFromFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "AdjustDateFrom",
                ResourceKey = "Adjust.AdjustDateFrom",
                DbColumn = "Adjust.AdjustDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(adjustDateFromFilter);

            var adjustDateToFilter = new FieldModel
            {
                DisplayOrder = 6,
                Name = "AdjustDateTo",
                ResourceKey = "Adjust.AdjustDateTo",
                DbColumn = "Adjust.AdjustDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(adjustDateToFilter);

            return model;
        }

        private SearchModel BuildCreateAdjustItemsSearchModel()
        {
            var model = new SearchModel();
            var itemNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "ItemName",
                ResourceKey = "Item.Name",
                DbColumn = "Item.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(itemNameFilter);

            var itemGroupFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "ItemGroup",
                ResourceKey = "ItemGroup",
                DbColumn = "ItemGroup.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "ItemGroup",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(itemGroupFilter);

            var itemCategoryFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "ItemCategory",
                ResourceKey = "Item.ItemCategory",
                DbColumn = "Item.ItemCategory",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Part,Tool,Asset,Other",
                CsvValueList = "0,1,2,3",
                IsRequiredField = false
            };
            model.Filters.Add(itemCategoryFilter);

            var barcodeFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "Barcode",
                ResourceKey = "Item.Barcode",
                DbColumn = "Item.Barcode",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(barcodeFilter);

            var itemStatusFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "ItemStatus",
                ResourceKey = "Item.ItemStatus",
                DbColumn = "Item.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "ValueItems",
                AdditionalField = "category",
                AdditionalValue = "Item Status",
                IsRequiredField = false
            };
            model.Filters.Add(itemStatusFilter);

            return model;
        }

        private void Validate(AdjustModel model, Adjust adjust, string actionName)
        {
            if (!string.IsNullOrEmpty(actionName))
            {
                if (actionName == WorkflowActionName.Approve)
                {
                    if (model.AdjustDate != null && model.AdjustDate < DateTime.UtcNow.Date)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Adjust.AdjustDateCannotEarlierThanToday"));
                    }
                    if (adjust.IsApproved == true)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Record.AlreadyApproved"));
                    }

                    var insufficientList = _adjustService.CheckSufficientQuantity(adjust);
                    if (insufficientList.Count > 0)
                    {
                        foreach (var item in insufficientList)
                        {
                            ModelState.AddModelError("", string.Format(_localizationService.GetResource("Adjust.InSufficientQuantiy"), item.ItemName, item.StoreLocatorName));
                        }
                    }
                }
            }
        }

        #endregion

        #region Adjusts

        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.AdjustSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.AdjustSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.AdjustSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.AdjustSearchModel] = model;

                PagedResult<Adjust> data = _adjustService.GetAdjusts(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create")]
        public ActionResult Create()
        {
            var adjust = new Adjust { IsNew = true };
            _adjustRepository.InsertAndCommit(adjust);
            return Json(new { Id = adjust.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Adjust>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Read,Inventory.Adjust.Update")]
        public ActionResult Edit(long id)
        {
            var adjust = _adjustRepository.GetById(id);
            var model = adjust.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Update")]
        public ActionResult Edit(AdjustModel model)
        {
            var adjust = _adjustRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                adjust = model.ToEntity(adjust);

                if (adjust.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), adjust);
                    adjust.Number = number;
                }

                //always set IsNew to false when saving
                adjust.IsNew = false;
                //update attributes
                _adjustRepository.Update(adjust);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = adjust.Number, isApproved = adjust.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Approve")]
        public ActionResult Approve(AdjustModel model)
        {
            var adjust = _adjustRepository.GetById(model.Id);
            Validate(model, adjust, WorkflowActionName.Approve);
            if (ModelState.IsValid)
            {
                adjust = model.ToEntity(adjust);

                if (adjust.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), adjust);
                    adjust.Number = number;
                }

                //always set IsNew to false when saving
                adjust.IsNew = false;
                //update attributes
                _adjustRepository.Update(adjust);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //approve
                _adjustService.Approve(adjust);

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = adjust.Number, isApproved = adjust.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var adjust = _adjustRepository.GetById(id);

            if (!_adjustService.IsDeactivable(adjust))
            {
                ModelState.AddModelError("Adjust", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _adjustRepository.DeactivateAndCommit(adjust);
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
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var adjusts = new List<Adjust>();
            foreach (long id in selectedIds)
            {
                var adjust = _adjustRepository.GetById(id);
                if (adjust != null)
                {
                    if (!_adjustService.IsDeactivable(adjust))
                    {
                        ModelState.AddModelError("Adjust", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        adjusts.Add(adjust);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var adjust in adjusts)
                    _adjustRepository.Deactivate(adjust);
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

        #region AdjustItem

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Read,Inventory.Adjust.Update")]
        public ActionResult AdjustItemList(long adjustId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _adjustItemRepository.GetAll().Where(c => c.AdjustId == adjustId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var adjustItems = new PagedList<AdjustItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = adjustItems.Select(x => x.ToModel()),
                Total = adjustItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Read,Inventory.Adjust.Update")]
        public ActionResult AdjustItem(long id)
        {
            var adjustItem = _adjustItemRepository.GetById(id);
            var model = adjustItem.ToModel();
            model.CurrentQuantity = _storeService.GetTotalQuantity(null, model.StoreLocatorId, model.ItemId);
            var html = this.AdjustItemPanel(model);
            return Json(new { Id = adjustItem.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Update")]
        public ActionResult CreateAdjustItem(long adjustId)
        {
            //need to get adjust here to assign to new adjustItem
            //so when mapping to Model, we will have StoreId as defined
            //in AutoMapper configuration
            var adjust = _adjustRepository.GetById(adjustId);
            var adjustItem = new AdjustItem
            {
                IsNew = true,
                Adjust = adjust
            };
            _adjustItemRepository.Insert(adjustItem);

            this._dbContext.SaveChanges();

            var model = new AdjustItemModel();
            model = adjustItem.ToModel();
            var html = this.AdjustItemPanel(model);

            return Json(new { Id = adjustItem.Id, Html = html });
        }

        [NonAction]
        public string AdjustItemPanel(AdjustItemModel model)
        {
            var html = this.RenderPartialViewToString("_AdjustItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Update")]
        public ActionResult SaveAdjustItem(AdjustItemModel model)
        {
            if (ModelState.IsValid)
            {
                var adjustItem = _adjustItemRepository.GetById(model.Id);
                //always set IsNew to false when saving
                adjustItem.IsNew = false;
                adjustItem = model.ToEntity(adjustItem);
                //update adjust cost
                _adjustService.UpdateAdjustCost(adjustItem);
                _adjustItemRepository.UpdateAndCommit(adjustItem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Update")]
        public ActionResult CancelAdjustItem(long id)
        {
            var adjustItem = _adjustItemRepository.GetById(id);
            if (adjustItem.IsNew == true)
            {
                _adjustItemRepository.DeleteAndCommit(adjustItem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Update")]
        public ActionResult DeleteAdjustItem(long? parentId, long id)
        {
            var adjustItem = _adjustItemRepository.GetById(id);
            _adjustItemRepository.DeactivateAndCommit(adjustItem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Update")]
        public ActionResult DeleteSelectedAdjustItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var adjustItem = _adjustItemRepository.GetById(id);
                _adjustItemRepository.Deactivate(adjustItem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Create Adjust Items

        [HttpGet]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Update")]
        public ActionResult CreateAdjustItemsView()
        {
            var model = BuildCreateAdjustItemsSearchModel();
            return PartialView("_CreateAdjustItems", model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Update")]
        public ActionResult CreateAdjustItemList(long adjustId, DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildCreateAdjustItemsSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Item> data = _itemService.GetItems(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => new AdjustItemModel
                    {
                        AdjustId = adjustId,
                        ItemId = x.Id,
                        ItemName = x.Name,
                        ItemUnitOfMeasureId = x.UnitOfMeasureId,
                        ItemUnitOfMeasureName = x.UnitOfMeasure.Name,
                        StoreLocator = new StoreLocatorModel()
                    }),
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

        /// <summary>
        /// The list of creating adjust items is in updatedItems 
        /// </summary>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Adjust.Create,Inventory.Adjust.Update")]
        public ActionResult CreateAdjustItems([Bind(Prefix = "updated")]List<AdjustItemModel> updatedItems,
           [Bind(Prefix = "created")]List<AdjustItemModel> createdItems,
           [Bind(Prefix = "deleted")]List<AdjustItemModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create AdjustItems
                    if (updatedItems != null)
                    {
                        //get the current adjust
                        var adjustId = updatedItems.Count > 0 ? updatedItems[0].AdjustId : 0;
                        var adjustItems = _adjustItemRepository.GetAll().Where(r => r.AdjustId == adjustId).ToList();
                        foreach (var model in updatedItems)
                        {
                            //we don't merge if the adjust item already existed
                            if (!adjustItems.Any(r => r.ItemId == model.ItemId))
                            {
                                //manually mapping here to set only foreign key
                                //if used AutoMapper, the reference property will also be mapped
                                //and EF will consider these properties as new and insert it
                                //so db records will be duplicated
                                //we can also ignore it in AutoMapper configuation instead of manually mapping
                                var adjustItem = new AdjustItem
                                {
                                    AdjustId = model.AdjustId,
                                    ItemId = model.ItemId,
                                    StoreLocatorId = model.StoreLocator.Id,
                                    AdjustQuantity = model.AdjustQuantity,
                                    AdjustUnitPrice = model.AdjustUnitPrice
                                };
                                _adjustService.UpdateAdjustCost(adjustItem);
                                _adjustItemRepository.Insert(adjustItem);
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

        #endregion
    }
}