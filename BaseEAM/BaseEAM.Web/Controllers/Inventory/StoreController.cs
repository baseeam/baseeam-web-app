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
    public class StoreController : BaseController
    {
        #region Fields

        private readonly IRepository<Store> _storeRepository;
        private readonly IRepository<StoreItem> _storeItemRepository;
        private readonly IRepository<StoreLocator> _storeLocatorRepository;
        private readonly IRepository<StoreLocatorItem> _storeLocatorItemRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IStoreService _storeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public StoreController(IRepository<Store> storeRepository,
            IRepository<StoreItem> storeItemRepository,
            IRepository<StoreLocator> storeLocatorRepository,
            IRepository<StoreLocatorItem> storeLocatorItemRepository,
            IRepository<Address> addressRepository,
            IStoreService storeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._storeRepository = storeRepository;
            this._storeItemRepository = storeItemRepository;
            this._storeLocatorRepository = storeLocatorRepository;
            this._storeLocatorItemRepository = storeLocatorItemRepository;
            this._addressRepository = addressRepository;
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
            var storeNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "StoreName",
                ResourceKey = "Store.Name",
                DbColumn = "Store.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(storeNameFilter);

            var storeTypeFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "StoreType",
                ResourceKey = "Store.StoreType",
                DbColumn = "StoreType.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "ValueItems",
                AdditionalField = "category",
                AdditionalValue = "Store Type",
                IsRequiredField = false
            };
            model.Filters.Add(storeTypeFilter);

            var siteFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "Site",
                ResourceKey = "Store.Site",
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

            var locationNameFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "Location",
                ResourceKey = "Location.Name",
                DbColumn = "Location.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Site",
                MvcAction = "LocationList",
                IsRequiredField = false,
                ParentFieldName = "Site"
            };

            model.Filters.Add(locationNameFilter);

            return model;
        }

        #endregion

        #region Stores

        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.StoreSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.StoreSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.StoreSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.StoreSearchModel] = model;

                PagedResult<Store> data = _storeService.GetStores(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create")]
        public ActionResult Create()
        {
            var store = new Store { IsNew = true };
            _storeRepository.InsertAndCommit(store);

            return Json(new { Id = store.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Store>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Store.Store.Read,Store.Store.Update")]
        public ActionResult Edit(long id)
        {
            var store = _storeRepository.GetById(id);
            var model = store.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Store.Store.Update")]
        public ActionResult Edit(StoreModel model)
        {
            var store = _storeRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                store = model.ToEntity(store);

                //add a default store locator when add new
                if(store.IsNew == true)
                {
                    var storeLocator = new StoreLocator
                    {
                        Name = "DEF",
                        StoreId = model.Id,
                        IsDefault = true
                    };
                    _storeLocatorRepository.Insert(storeLocator);
                }

                //always set IsNew to false when saving
                store.IsNew = false;

                //update attributes
                _storeRepository.Update(store);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var store = _storeRepository.GetById(id);

            if (!_storeService.IsDeactivable(store))
            {
                ModelState.AddModelError("Store", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _storeRepository.DeactivateAndCommit(store);
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
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var stores = new List<Store>();
            foreach (long id in selectedIds)
            {
                var store = _storeRepository.GetById(id);
                if (store != null)
                {
                    if (!_storeService.IsDeactivable(store))
                    {
                        ModelState.AddModelError("Store", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        stores.Add(store);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var store in stores)
                    _storeRepository.Deactivate(store);
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
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Inventory.Store.Read,Inventory.Store.Update")]
        public ActionResult StoreLocatorList(long storeId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _storeLocatorRepository.GetAll().Where(c => c.StoreId == storeId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var storeLocators = new PagedList<StoreLocator>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = storeLocators.Select(x => x.ToModel()),
                Total = storeLocators.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        public JsonResult StoreLocators(long? storeId, string param)
        {
            var storeLocators = _storeLocatorRepository.GetAll().Where(s => s.StoreId == storeId).ToList();
            var choices = new List<SelectListItem>();
            foreach (var locator in storeLocators)
            {
                choices.Add(new SelectListItem
                {
                    Value = locator.Id.ToString(),
                    Text = locator.Name
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        /// <summary>
        /// Get locators for storeLocatorEditor
        /// </summary>
        /// <param name="parentValue">storeId</param>
        /// <param name="additionalValue">itemId</param>
        /// <param name="param"></param>
        /// <param name="autoBind"></param>
        [HttpPost]
        public JsonResult GetStoreLocators(long? parentValue, long? additionalValue, string param, bool autoBind = true)
        {
            var choices = new List<BaseEamListItem>();
            if (parentValue == null || additionalValue == null)
                return Json(choices);

            if (!autoBind && string.IsNullOrEmpty(param))
                return Json(choices);

            var query1 = from sli in _storeLocatorItemRepository.GetAll()
                         where sli.StoreId == parentValue && sli.ItemId == additionalValue
                         group sli by new { sli.StoreId, sli.StoreLocatorId, sli.ItemId }
                         into g
                         select new
                         {
                             StoreLocatorId = g.Key.StoreLocatorId,
                             Quantity = g.Sum(e => e.Quantity)
                         };

            var data = from sl in _storeLocatorRepository.GetAll()
                       where sl.StoreId == parentValue //need to put condition here, if remove this LINQ LEFT JOIN doesn't work ???
                       join q in query1 on sl.Id equals q.StoreLocatorId
                       into r1
                       from r in r1.DefaultIfEmpty()
                       select new
                       {
                           StoreLocatorId = sl.Id,
                           StoreLocatorName = sl.Name,
                           Quantity = r.Quantity == null ? 0 : r.Quantity
                       };

            foreach (var item in data)
            {
                choices.Add(new BaseEamListItem
                {
                    Id = item.StoreLocatorId.ToString(),
                    Name = item.StoreLocatorName + " [" + item.Quantity.Value.ToString("N2") + "]"
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new BaseEamListItem { Id = "", Name = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Inventory.Store.Update,Inventory.Store.Delete")]
        public ActionResult SaveChanges([Bind(Prefix = "updated")]List<StoreLocatorModel> updatedItems,
            [Bind(Prefix = "created")]List<StoreLocatorModel> createdItems,
            [Bind(Prefix = "deleted")]List<StoreLocatorModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create StoreLocators
                    if (createdItems != null)
                    {
                        foreach (var model in createdItems)
                        {
                            var storeLocator = _storeLocatorRepository.GetAll().Where(c => c.Id == model.Id).FirstOrDefault();
                            if (storeLocator == null)
                            {
                                storeLocator = new StoreLocator
                                {
                                    Name = model.Name,
                                    StoreId = model.StoreId
                                };
                                _storeLocatorRepository.Insert(storeLocator);
                            }

                        }
                    }

                    //Update StoreLocators
                    if (updatedItems != null)
                    {
                        foreach (var model in updatedItems)
                        {
                            var storeLocator = _storeLocatorRepository.GetById(model.Id);
                            storeLocator.Name = model.Name;
                            _storeLocatorRepository.Update(storeLocator);
                        }
                    }

                    //Delete StoreLocators
                    if (deletedItems != null)
                    {
                        foreach (var model in deletedItems)
                        {
                            var storeLocator = _storeLocatorRepository.GetById(model.Id);
                            if (storeLocator != null)
                            {
                                _storeLocatorRepository.Deactivate(storeLocator);
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

        //Get Current Quantity from StoreLocator, Item 
        [HttpPost]
        public ActionResult GetCurrentQuantity(long? storeLocatorId, long? itemId)
        {
            if (storeLocatorId == null || storeLocatorId == 0 || itemId == null || itemId == 0)
                return new NullJsonResult();

            var currentQuantity = _storeService.GetTotalQuantity(null, storeLocatorId, itemId);
            return Json(new { currentQuantity = currentQuantity });
        }
        #endregion

        #region Items

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Inventory.Store.Read,Inventory.Store.Update")]
        public ActionResult ItemList(long storeId, DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildItemSearchModel();
            if (ModelState.IsValid)
            {
                model.Update(searchValues);
                var expression = model.ToExpression(this._workContext.CurrentUser.Id);
                //do a hack here to add storeId filter
                expression = expression + " AND Store.Id = " + storeId;
                PagedResult<StoreItemBalance> data = _storeService.GetStoreItemBalances(expression, model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.ToList();
                foreach (var r in result)
                {
                    r.StoreItemStockTypeText = ((ItemStockType)r.StoreItemStockType).ToString();
                }
                var gridModel = new DataSourceResult
                {
                    Data = result,
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

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Inventory.Store.Update")]
        public ActionResult AddItems(long storeId, long[] selectedIds)
        {
            var store = _storeRepository.GetById(storeId);
            foreach (var id in selectedIds)
            {
                var existed = store.StoreItems.Any(s => s.ItemId == id);
                if (!existed)
                {
                    var storeItem = _storeService.AddStoreItem(storeId, id);
                    store.StoreItems.Add(storeItem);
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Store.Create,Inventory.Store.Update,Inventory.Store.Delete")]
        public ActionResult DeleteItem(long? parentId, long id)
        {
            var storeItem = _storeItemRepository.GetById(id);
            _storeItemRepository.DeactivateAndCommit(storeItem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Security.SecurityGroup.Create,Administration.Item.Update")]
        public ActionResult DeleteSelectedItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var storeItem = _storeItemRepository.GetById(id);
                _storeItemRepository.Deactivate(storeItem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        private SearchModel BuildItemSearchModel()
        {
            var model = new SearchModel();
            var itemNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "ItemName",
                ResourceKey = "Item",
                DbColumn = "ItemId",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "Item",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(itemNameFilter);

            return model;
        }

        #endregion
    }
}