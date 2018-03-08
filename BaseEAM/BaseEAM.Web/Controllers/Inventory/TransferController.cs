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
    public class TransferController : BaseController
    {
        #region Fields

        private readonly IRepository<Transfer> _transferRepository;
        private readonly IRepository<TransferItem> _transferItemRepository;
        private readonly ITransferService _transferService;
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

        public TransferController(IRepository<Transfer> transferRepository,
            IRepository<TransferItem> transferItemRepository,
            ITransferService transferService,
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
            this._transferRepository = transferRepository;
            this._transferItemRepository = transferItemRepository;
            this._localizationService = localizationService;
            this._transferService = transferService;
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
            var transferNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Number",
                ResourceKey = "Transfer.Number",
                DbColumn = "Transfer.Number, Transfer.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(transferNameFilter);

            var isApprovedFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "IsApproved",
                ResourceKey = "Transfer.IsApproved",
                DbColumn = "Transfer.IsApproved",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Boolean,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "True,False",
                CsvValueList = "True,False",
                IsRequiredField = false
            };
            model.Filters.Add(isApprovedFilter);

            var fromSiteFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "FromSite",
                ResourceKey = "FromSite",
                DbColumn = "FromSite.Id",
                Value = this._workContext.CurrentUser.DefaultSiteId,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Site",
                MvcAction = "SiteList",
                IsRequiredField = false
            };
            model.Filters.Add(fromSiteFilter);

            var fromStoreFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "FromStore",
                ResourceKey = "FromStore",
                DbColumn = "FromStore.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Site",
                MvcAction = "StoreList",
                IsRequiredField = false,
                ParentFieldName = "FromSite"
            };

            model.Filters.Add(fromStoreFilter);

            var toSiteFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "ToSite",
                ResourceKey = "ToSite",
                DbColumn = "ToSite.Id",
                Value = this._workContext.CurrentUser.DefaultSiteId,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Site",
                MvcAction = "SiteList",
                IsRequiredField = false
            };
            model.Filters.Add(toSiteFilter);

            var toStoreFilter = new FieldModel
            {
                DisplayOrder = 6,
                Name = "ToStore",
                ResourceKey = "ToStore",
                DbColumn = "ToStore.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Site",
                MvcAction = "StoreList",
                IsRequiredField = false,
                ParentFieldName = "ToSite"
            };

            model.Filters.Add(toStoreFilter);

            var transferDateFromFilter = new FieldModel
            {
                DisplayOrder = 7,
                Name = "TransferDateFrom",
                ResourceKey = "Transfer.TransferDateFrom",
                DbColumn = "Transfer.TransferDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(transferDateFromFilter);

            var transferDateToFilter = new FieldModel
            {
                DisplayOrder = 8,
                Name = "TransferDateTo",
                ResourceKey = "Transfer.TransferDateTo",
                DbColumn = "Transfer.TransferDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(transferDateToFilter);

            return model;
        }

        private SearchModel BuildCreateTransferItemsSearchModel()
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

        private void Validate(TransferModel model, Transfer transfer, string actionName)
        {
            if (!string.IsNullOrEmpty(actionName))
            {
                if (actionName == WorkflowActionName.Approve)
                {
                    if (model.TransferDate != null && model.TransferDate < DateTime.UtcNow.Date)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Transfer.TransferDateCannotEarlierThanToday"));
                    }
                    if (transfer.IsApproved == true)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Record.AlreadyApproved"));
                    }

                    var insufficientList = _transferService.CheckSufficientQuantity(transfer);
                    if (insufficientList.Count > 0)
                    {
                        foreach (var item in insufficientList)
                        {
                            ModelState.AddModelError("", string.Format(_localizationService.GetResource("Transfer.InSufficientQuantiy"), item.ItemName, item.StoreLocatorName));
                        }
                    }
                }
            }
        }

        #endregion

        #region Transfers

        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.TransferSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.TransferSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.TransferSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.TransferSearchModel] = model;

                PagedResult<Transfer> data = _transferService.GetTransfers(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create")]
        public ActionResult Create()
        {
            var transfer = new Transfer { IsNew = true };
            _transferRepository.InsertAndCommit(transfer);
            return Json(new { Id = transfer.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Transfer>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Read,Inventory.Transfer.Update")]
        public ActionResult Edit(long id)
        {
            var transfer = _transferRepository.GetById(id);
            var model = transfer.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Update")]
        public ActionResult Edit(TransferModel model)
        {
            var transfer = _transferRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                transfer = model.ToEntity(transfer);

                if (transfer.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), transfer);
                    transfer.Number = number;
                }

                //always set IsNew to false when saving
                transfer.IsNew = false;
                //update attributes
                _transferRepository.Update(transfer);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = transfer.Number, isApproved = transfer.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Approve")]
        public ActionResult Approve(TransferModel model)
        {
            var transfer = _transferRepository.GetById(model.Id);
            Validate(model, transfer, WorkflowActionName.Approve);
            if (ModelState.IsValid)
            {
                transfer = model.ToEntity(transfer);

                if (transfer.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), transfer);
                    transfer.Number = number;
                }

                //always set IsNew to false when saving
                transfer.IsNew = false;
                //update attributes
                _transferRepository.Update(transfer);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //approve
                _transferService.Approve(transfer);

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = transfer.Number, isApproved = transfer.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var transfer = _transferRepository.GetById(id);

            if (!_transferService.IsDeactivable(transfer))
            {
                ModelState.AddModelError("Transfer", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _transferRepository.DeactivateAndCommit(transfer);
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
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var transfers = new List<Transfer>();
            foreach (long id in selectedIds)
            {
                var transfer = _transferRepository.GetById(id);
                if (transfer != null)
                {
                    if (!_transferService.IsDeactivable(transfer))
                    {
                        ModelState.AddModelError("Transfer", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        transfers.Add(transfer);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var transfer in transfers)
                    _transferRepository.Deactivate(transfer);
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

        #region TransferItem

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Read,Inventory.Transfer.Update")]
        public ActionResult TransferItemList(long transferId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _transferItemRepository.GetAll().Where(c => c.TransferId == transferId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var transferItems = new PagedList<TransferItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = transferItems.Select(x => x.ToModel()),
                Total = transferItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Read,Inventory.Transfer.Update")]
        public ActionResult TransferItem(long id)
        {
            var transferItem = _transferItemRepository.GetById(id);
            var model = transferItem.ToModel();
            model.CurrentQuantity = _storeService.GetTotalQuantity(null, model.FromStoreLocatorId, model.ItemId);
            var html = this.TransferItemPanel(model);
            return Json(new { Id = transferItem.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Update")]
        public ActionResult CreateTransferItem(long transferId)
        {
            //need to get transfer here to assign to new transferItem
            //so when mapping to Model, we will have StoreId as defined
            //in AutoMapper configuration
            var transfer = _transferRepository.GetById(transferId);
            var transferItem = new TransferItem
            {
                IsNew = true,
                Transfer = transfer
            };
            _transferItemRepository.Insert(transferItem);

            this._dbContext.SaveChanges();

            var model = new TransferItemModel();
            model = transferItem.ToModel();
            var html = this.TransferItemPanel(model);

            return Json(new { Id = transferItem.Id, Html = html });
        }

        [NonAction]
        public string TransferItemPanel(TransferItemModel model)
        {
            var html = this.RenderPartialViewToString("_TransferItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Update")]
        public ActionResult SaveTransferItem(TransferItemModel model)
        {
            if (ModelState.IsValid)
            {
                var transferItem = _transferItemRepository.GetById(model.Id);
                //always set IsNew to false when saving
                transferItem.IsNew = false;
                transferItem = model.ToEntity(transferItem);
                //update transfer cost
                _transferService.UpdateTransferCost(transferItem);
                _transferItemRepository.UpdateAndCommit(transferItem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Update")]
        public ActionResult CancelTransferItem(long id)
        {
            var transferItem = _transferItemRepository.GetById(id);
            if (transferItem.IsNew == true)
            {
                _transferItemRepository.DeleteAndCommit(transferItem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Update")]
        public ActionResult DeleteTransferItem(long? parentId, long id)
        {
            var transferItem = _transferItemRepository.GetById(id);
            _transferItemRepository.DeactivateAndCommit(transferItem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Update")]
        public ActionResult DeleteSelectedTransferItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var transferItem = _transferItemRepository.GetById(id);
                _transferItemRepository.Deactivate(transferItem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Create Transfer Items

        [HttpGet]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Update")]
        public ActionResult CreateTransferItemsView()
        {
            var model = BuildCreateTransferItemsSearchModel();
            return PartialView("_CreateTransferItems", model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Update")]
        public ActionResult CreateTransferItemList(long transferId, DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildCreateTransferItemsSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Item> data = _itemService.GetItems(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => new TransferItemModel
                    {
                        TransferId = transferId,
                        ItemId = x.Id,
                        ItemName = x.Name,
                        ItemUnitOfMeasureId = x.UnitOfMeasureId,
                        ItemUnitOfMeasureName = x.UnitOfMeasure.Name,
                        FromStoreLocator = new StoreLocatorModel(),
                        ToStoreLocator = new StoreLocatorModel()
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
        /// The list of creating transfer items is in updatedItems 
        /// </summary>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Transfer.Create,Inventory.Transfer.Update")]
        public ActionResult CreateTransferItems([Bind(Prefix = "updated")]List<TransferItemModel> updatedItems,
           [Bind(Prefix = "created")]List<TransferItemModel> createdItems,
           [Bind(Prefix = "deleted")]List<TransferItemModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create TransferItems
                    if (updatedItems != null)
                    {
                        //get the current transfer
                        var transferId = updatedItems.Count > 0 ? updatedItems[0].TransferId : 0;
                        var transferItems = _transferItemRepository.GetAll().Where(r => r.TransferId == transferId).ToList();
                        foreach (var model in updatedItems)
                        {
                            //we don't merge if the transfer item already existed
                            if (!transferItems.Any(r => r.ItemId == model.ItemId))
                            {
                                //manually mapping here to set only foreign key
                                //if used AutoMapper, the reference property will also be mapped
                                //and EF will consider these properties as new and insert it
                                //so db records will be duplicated
                                //we can also ignore it in AutoMapper configuation instead of manually mapping
                                var transferItem = new TransferItem
                                {
                                    TransferId = model.TransferId,
                                    ItemId = model.ItemId,
                                    FromStoreLocatorId = model.FromStoreLocator.Id,
                                    ToStoreLocatorId = model.ToStoreLocator.Id,
                                    TransferQuantity = model.TransferQuantity,
                                    TransferUnitOfMeasureId = model.ItemUnitOfMeasureId
                                };
                                _transferService.UpdateTransferCost(transferItem);
                                _transferItemRepository.Insert(transferItem);
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