/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BaseEAM.Web.Framework.Filters;
using BaseEAM.Core.Data;
using BaseEAM.Web.Extensions;
using System.Linq;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Controllers
{
    /// <summary>
    /// Inventory = StoreLocatorItem
    /// </summary>
    public class InventoryController : BaseController
    {
        #region Fields

        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<StoreItem> _storeItemRepository;
        private readonly IStoreService _storeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public InventoryController(IRepository<Site> siteRepository,
            IRepository<Store> storeRepository,
            IRepository<Item> itemRepository,
            IRepository<StoreItem> storeItemRepository,
            IStoreService storeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._siteRepository = siteRepository;
            this._storeRepository = storeRepository;
            this._itemRepository = itemRepository;
            this._storeItemRepository = storeItemRepository;
            this._localizationService = localizationService;
            this._storeService = storeService;
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

            var siteFilter = new FieldModel
            {
                DisplayOrder = 1,
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
                DisplayOrder = 1,
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

            return model;
        }

        private SearchModel BuildTransactionsSearchModel()
        {
            var model = new SearchModel();

            var transactionTypeFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "TransactionType",
                ResourceKey = "StoreLocatorItemLog.TransactionType",
                DbColumn = "TransactionType.TransactionType",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Receipt,Issue,Transfer,Adjust,Return,PhysicalCount",
                CsvValueList = "Receipt,Issue,Transfer,Adjust,Return,PhysicalCount",
                IsRequiredField = false
            };
            model.Filters.Add(transactionTypeFilter);

            return model;
        }

        #endregion

        #region Inventory

        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.StoreItemSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.StoreItemSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.StoreItemSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.StoreItemSearchModel] = model;

                PagedResult<StoreItemBalance> data = _storeService.GetStoreItemBalances(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result,
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Inventory.Store.Read,Inventory.Store.Update")]
        public ActionResult Edit(long id)
        {
            var storeItem = _storeItemRepository.GetById(id);
            var model = storeItem.ToModel();
            var site = storeItem.Store.Site;
            model.SiteId = site.Id;
            model.Site = site.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Inventory.Store.Update")]
        public ActionResult Edit(StoreItemModel model)
        {
            var storeItem = _storeItemRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                storeItem = model.ToEntity(storeItem);

                //always set IsNew to false when saving
                storeItem.IsNew = false;
                //update attributes
                _storeItemRepository.Update(storeItem);

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

        #endregion

        #region Balances

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Inventory.Store.Read,Inventory.Store.Update")]
        public ActionResult BalanceList(long storeId, long itemId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var model = BuildBalanceSearchModel();
            if (ModelState.IsValid)
            {
                var expression = model.ToExpression(this._workContext.CurrentUser.Id);
                //do a hack here to add storeId & itemId filter
                expression = expression + " AND Store.Id = " + storeId;
                expression = expression + " AND Item.Id = " + itemId;
                PagedResult<StoreLocatorItemBalance> data = _storeService.GetStoreLocatorItemBalances(expression, model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result,
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

        private SearchModel BuildBalanceSearchModel()
        {
            var model = new SearchModel();
            return model;
        }

        #endregion

        #region Transactions

        [HttpGet]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Inventory.Store.Update,Inventory.Store.Read")]
        public ActionResult TransactionsView(string id)
        {
            var ids = id.Split('_');
            //user ViewData to pass params to View
            ViewData["StoreLocatorId"] = ids[2];
            ViewData["ItemId"] = ids[3];

            var model = BuildTransactionsSearchModel();
            return PartialView("_Transactions", model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Inventory.Store.Update,Inventory.Store.Read")]
        public ActionResult TransactionList(long storeLocatorId, long itemId, DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildTransactionsSearchModel();
            if (ModelState.IsValid)
            {
                model.Update(searchValues);
                var expression = model.ToExpression();
                //do a hack here to add storeId filter
                expression = expression + " AND StoreLocator.Id = " + storeLocatorId;
                expression = expression + " AND Item.Id = " + itemId;
                PagedResult<StoreLocatorItemLog> data = _storeService.GetStoreLocatorItemLogs(expression, model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion
    }
}