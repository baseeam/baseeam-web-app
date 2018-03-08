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
    public class ReceiptController : BaseController
    {
        #region Fields

        private readonly IRepository<Receipt> _receiptRepository;
        private readonly IRepository<ReceiptItem> _receiptItemRepository;
        private readonly IRepository<Item> _itemRepository;
        private readonly IReceiptService _receiptService;
        private readonly IStoreService _storeService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IItemService _itemService;
        private readonly IUnitConversionService _unitConversionService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public ReceiptController(IRepository<Receipt> receiptRepository,
            IRepository<ReceiptItem> receiptItemRepository,
            IRepository<Item> itemRepository,
            IReceiptService receiptService,
            IStoreService storeService,
            IAutoNumberService autoNumberService,
            IItemService itemService,
            IUnitConversionService unitConversionService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._receiptRepository = receiptRepository;
            this._receiptItemRepository = receiptItemRepository;
            this._itemRepository = itemRepository;
            this._localizationService = localizationService;
            this._receiptService = receiptService;
            this._storeService = storeService;
            this._autoNumberService = autoNumberService;
            this._itemService = itemService;
            this._unitConversionService = unitConversionService;
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
            var receiptNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Number",
                ResourceKey = "Receipt.Number",
                DbColumn = "Receipt.Number, Receipt.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(receiptNameFilter);

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

            var receiptDateFromFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "ReceiptDateFrom",
                ResourceKey = "Receipt.ReceiptDateFrom",
                DbColumn = "Receipt.ReceiptDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(receiptDateFromFilter);

            var receiptDateToFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "ReceiptDateTo",
                ResourceKey = "Receipt.ReceiptDateTo",
                DbColumn = "Receipt.ReceiptDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(receiptDateToFilter);

            return model;
        }

        private SearchModel BuildCreateReceiptItemsSearchModel()
        {
            var model = new SearchModel();
            var itemNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "ItemName",
                ResourceKey = "Item",
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

        private void Validate(ReceiptModel model, Receipt receipt, string actionName)
        {
            if(!string.IsNullOrEmpty(actionName))
            {
                if(actionName == WorkflowActionName.Approve)
                {
                    if (model.ReceiptDate != null && model.ReceiptDate < DateTime.UtcNow.Date)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Receipt.ReceiptDateCannotEarlierThanToday"));
                    }
                    if(receipt.IsApproved == true)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Record.AlreadyApproved"));
                    }
                }
            }
        }

        private void PrepareReceiptItemModel(ReceiptItemModel model)
        {
            model.CurrentQuantity = _storeService.GetTotalQuantity(null, model.StoreLocatorId, model.ItemId);

            var uoms = _unitConversionService.GetFromUOMs(model.ItemUnitOfMeasureId.Value);
            model.AvailableUnitOfMeasures.Add(new SelectListItem
            {
                Value = model.ItemUnitOfMeasureId.ToString(),
                Text = model.ItemUnitOfMeasureName
            });
            foreach (var c in uoms)
            {
                model.AvailableUnitOfMeasures.Add(new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = model.ReceiptUnitOfMeasureId == c.Id
                });
            }
        }

        #endregion

        #region Receipts

        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ReceiptSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ReceiptSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ReceiptSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.ReceiptSearchModel] = model;

                PagedResult<Receipt> data = _receiptService.GetReceipts(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create")]
        public ActionResult Create()
        {
            var receipt = new Receipt { IsNew = true };
            _receiptRepository.InsertAndCommit(receipt);
            return Json(new { Id = receipt.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Receipt>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Read,Inventory.Receipt.Update")]
        public ActionResult Edit(long id)
        {
            var receipt = _receiptRepository.GetById(id);
            var model = receipt.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Update")]
        public ActionResult Edit(ReceiptModel model)
        {
            var receipt = _receiptRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                receipt = model.ToEntity(receipt);

                if(receipt.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), receipt);
                    receipt.Number = number;
                }

                //always set IsNew to false when saving
                receipt.IsNew = false;
                //update attributes
                _receiptRepository.Update(receipt);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = receipt.Number, isApproved = receipt.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Approve")]
        public ActionResult Approve(ReceiptModel model)
        {
            var receipt = _receiptRepository.GetById(model.Id);
            Validate(model, receipt, WorkflowActionName.Approve);
            if (ModelState.IsValid)
            {
                receipt = model.ToEntity(receipt);

                if (receipt.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), receipt);
                    receipt.Number = number;
                }

                //always set IsNew to false when saving
                receipt.IsNew = false;
                //update attributes
                _receiptRepository.Update(receipt);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //approve
                _receiptService.Approve(receipt);

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = receipt.Number, isApproved = receipt.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var receipt = _receiptRepository.GetById(id);

            if (!_receiptService.IsDeactivable(receipt))
            {
                ModelState.AddModelError("Receipt", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _receiptRepository.DeactivateAndCommit(receipt);
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
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var receipts = new List<Receipt>();
            foreach (long id in selectedIds)
            {
                var receipt = _receiptRepository.GetById(id);
                if (receipt != null)
                {
                    if (!_receiptService.IsDeactivable(receipt))
                    {
                        ModelState.AddModelError("Receipt", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        receipts.Add(receipt);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var receipt in receipts)
                    _receiptRepository.Deactivate(receipt);
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

        #region ReceiptItem

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Read,Inventory.Receipt.Update")]
        public ActionResult ReceiptItemList(long receiptId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _receiptItemRepository.GetAll().Where(c => c.ReceiptId == receiptId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var receiptItems = new PagedList<ReceiptItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = receiptItems.Select(x => x.ToModel()),
                Total = receiptItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Read,Inventory.Receipt.Update")]
        public ActionResult ReceiptItem(long id)
        {
            var receiptItem = _receiptItemRepository.GetById(id);
            var model = receiptItem.ToModel();
            PrepareReceiptItemModel(model);
            var html = this.ReceiptItemPanel(model);
            return Json(new { Id = receiptItem.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Update")]
        public ActionResult CreateReceiptItem(long receiptId)
        {
            //need to get receipt here to assign to new receiptItem
            //so when mapping to Model, we will have StoreId as defined
            //in AutoMapper configuration
            var receipt = _receiptRepository.GetById(receiptId);
            var receiptItem = new ReceiptItem
            {
                IsNew = true,
                Receipt = receipt
            };
            _receiptItemRepository.Insert(receiptItem);

            this._dbContext.SaveChanges();

            var model = new ReceiptItemModel();
            model = receiptItem.ToModel();
            var html = this.ReceiptItemPanel(model);

            return Json(new { Id = receiptItem.Id, Html = html });
        }

        [NonAction]
        public string ReceiptItemPanel(ReceiptItemModel model)
        {
            var html = this.RenderPartialViewToString("_ReceiptItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Update")]
        public ActionResult SaveReceiptItem(ReceiptItemModel model)
        {
            if (ModelState.IsValid)
            {
                var receiptItem = _receiptItemRepository.GetById(model.Id);
                //always set IsNew to false when saving
                receiptItem.IsNew = false;
                receiptItem = model.ToEntity(receiptItem);

                // unit conversion
                var item = _itemRepository.GetById(model.ItemId);
                if(item.UnitOfMeasureId != model.ReceiptUnitOfMeasureId)
                {
                    var uc = _unitConversionService.GetUnitConversion(model.ReceiptUnitOfMeasureId.Value, item.UnitOfMeasureId.Value);
                    receiptItem.Quantity = receiptItem.ReceiptQuantity * uc.ConversionFactor;
                    receiptItem.UnitPrice = receiptItem.ReceiptUnitPrice / uc.ConversionFactor;
                }
                else
                {
                    receiptItem.Quantity = receiptItem.ReceiptQuantity;
                    receiptItem.UnitPrice = receiptItem.ReceiptUnitPrice;
                }

                _receiptItemRepository.UpdateAndCommit(receiptItem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Update")]
        public ActionResult CancelReceiptItem(long id)
        {
            var receiptItem = _receiptItemRepository.GetById(id);
            if (receiptItem.IsNew == true)
            {
                _receiptItemRepository.DeleteAndCommit(receiptItem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Update")]
        public ActionResult DeleteReceiptItem(long? parentId, long id)
        {
            var receiptItem = _receiptItemRepository.GetById(id);
            _receiptItemRepository.DeactivateAndCommit(receiptItem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Update")]
        public ActionResult DeleteSelectedReceiptItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var receiptItem = _receiptItemRepository.GetById(id);
                _receiptItemRepository.Deactivate(receiptItem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Create Receipt Items

        [HttpGet]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Update")]
        public ActionResult CreateReceiptItemsView()
        {
            var model = BuildCreateReceiptItemsSearchModel();
            return PartialView("_CreateReceiptItems", model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Update")]
        public ActionResult CreateReceiptItemList(long receiptId, DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildCreateReceiptItemsSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Item> data = _itemService.GetItems(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => new ReceiptItemModel
                    {
                        ReceiptId = receiptId,
                        ItemId = x.Id,
                        ItemName = x.Name,
                        ReceiptUnitOfMeasureId = x.UnitOfMeasureId,
                        ReceiptUnitOfMeasureName = x.UnitOfMeasure.Name,
                        ReceiptUnitPrice = x.UnitPrice,
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
        /// The list of creating receipt items is in updatedItems 
        /// </summary>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Receipt.Create,Inventory.Receipt.Update")]
        public ActionResult CreateReceiptItems([Bind(Prefix = "updated")]List<ReceiptItemModel> updatedItems,
           [Bind(Prefix = "created")]List<ReceiptItemModel> createdItems,
           [Bind(Prefix = "deleted")]List<ReceiptItemModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create ReceiptItems
                    if (updatedItems != null)
                    {
                        //get the current receipt
                        var receiptId = updatedItems.Count > 0 ? updatedItems[0].ReceiptId : 0;
                        var receiptItems = _receiptItemRepository.GetAll().Where(r => r.ReceiptId == receiptId).ToList();
                        foreach (var model in updatedItems)
                        {
                            //we don't merge if the receipt item already existed
                            if(!receiptItems.Any(r => r.ItemId == model.ItemId))
                            {
                                //manually mapping here to set only foreign key
                                //if used AutoMapper, the reference property will also be mapped
                                //and EF will consider these properties as new and insert it
                                //so db records will be duplicated
                                //we can also ignore it in AutoMapper configuation instead of manually mapping
                                var receiptItem = new ReceiptItem
                                {
                                    ReceiptId = model.ReceiptId,
                                    StoreLocatorId = model.StoreLocator.Id,
                                    ItemId = model.ItemId,
                                    ReceiptUnitOfMeasureId = model.ReceiptUnitOfMeasureId,
                                    ReceiptQuantity = model.ReceiptQuantity,
                                    ReceiptUnitPrice = model.ReceiptUnitPrice,
                                    UnitPrice = model.ReceiptUnitPrice,
                                    Quantity = model.ReceiptQuantity,
                                    LotNumber = model.LotNumber,
                                    ExpiryDate = model.ExpiryDate
                                };
                                receiptItem.Cost = receiptItem.UnitPrice * receiptItem.Quantity;
                                _receiptItemRepository.Insert(receiptItem);
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