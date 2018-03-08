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
    public class PhysicalCountController : BaseController
    {
        #region Fields

        private readonly IRepository<PhysicalCount> _physicalCountRepository;
        private readonly IRepository<PhysicalCountItem> _physicalCountItemRepository;
        private readonly IPhysicalCountService _physicalCountService;
        private readonly IStoreService _storeService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public PhysicalCountController(IRepository<PhysicalCount> physicalCountRepository,
            IRepository<PhysicalCountItem> physicalCountItemRepository,
            IPhysicalCountService physicalCountService,
            IStoreService storeService,
            IAutoNumberService autoNumberService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._physicalCountRepository = physicalCountRepository;
            this._physicalCountItemRepository = physicalCountItemRepository;
            this._localizationService = localizationService;
            this._physicalCountService = physicalCountService;
            this._storeService = storeService;
            this._autoNumberService = autoNumberService;
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
            var physicalCountNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Number",
                ResourceKey = "PhysicalCount.Number",
                DbColumn = "PhysicalCount.Number, PhysicalCount.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(physicalCountNameFilter);

            var isApprovedFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "IsApproved",
                ResourceKey = "PhysicalCount.IsApproved",
                DbColumn = "PhysicalCount.IsApproved",
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

            var physicalCountDateFromFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "PhysicalCountDateFrom",
                ResourceKey = "PhysicalCount.PhysicalCountDateFrom",
                DbColumn = "PhysicalCount.PhysicalCountDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(physicalCountDateFromFilter);

            var physicalCountDateToFilter = new FieldModel
            {
                DisplayOrder = 6,
                Name = "PhysicalCountDateTo",
                ResourceKey = "PhysicalCount.PhysicalCountDateTo",
                DbColumn = "PhysicalCount.PhysicalCountDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(physicalCountDateToFilter);

            return model;
        }

        private SearchModel BuildCreatePhysicalCountItemsSearchModel()
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

            return model;
        }

        private void Validate(PhysicalCountModel model, PhysicalCount physicalCount, string actionName)
        {
            if (!string.IsNullOrEmpty(actionName))
            {
                if (actionName == WorkflowActionName.Approve)
                {
                    if (model.PhysicalCountDate != null && model.PhysicalCountDate < DateTime.UtcNow.Date)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("PhysicalCount.PhysicalCountDateCannotEarlierThanToday"));
                    }
                    if (physicalCount.IsApproved == true)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Record.AlreadyApproved"));
                    }

                }
            }
        }

        #endregion

        #region PhysicalCounts

        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.PhysicalCountSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.PhysicalCountSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.PhysicalCountSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.PhysicalCountSearchModel] = model;

                PagedResult<PhysicalCount> data = _physicalCountService.GetPhysicalCounts(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create")]
        public ActionResult Create()
        {
            var physicalCount = new PhysicalCount { IsNew = true };
            _physicalCountRepository.InsertAndCommit(physicalCount);
            return Json(new { Id = physicalCount.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<PhysicalCount>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Read,Inventory.PhysicalCount.Update")]
        public ActionResult Edit(long id)
        {
            var physicalCount = _physicalCountRepository.GetById(id);
            var model = physicalCount.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Update")]
        public ActionResult Edit(PhysicalCountModel model)
        {
            var physicalCount = _physicalCountRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                physicalCount = model.ToEntity(physicalCount);

                if (physicalCount.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), physicalCount);
                    physicalCount.Number = number;
                }

                //always set IsNew to false when saving
                physicalCount.IsNew = false;
                //update attributes
                _physicalCountRepository.Update(physicalCount);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = physicalCount.Number, isApproved = physicalCount.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Approve")]
        public ActionResult Approve(PhysicalCountModel model)
        {
            var physicalCount = _physicalCountRepository.GetById(model.Id);
            Validate(model, physicalCount, WorkflowActionName.Approve);
            if (ModelState.IsValid)
            {
                physicalCount = model.ToEntity(physicalCount);

                if (physicalCount.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), physicalCount);
                    physicalCount.Number = number;
                }

                //always set IsNew to false when saving
                physicalCount.IsNew = false;
                //update attributes
                _physicalCountRepository.Update(physicalCount);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //approve
                _physicalCountService.Approve(physicalCount);

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = physicalCount.Number, isApproved = physicalCount.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var physicalCount = _physicalCountRepository.GetById(id);

            if (!_physicalCountService.IsDeactivable(physicalCount))
            {
                ModelState.AddModelError("PhysicalCount", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _physicalCountRepository.DeactivateAndCommit(physicalCount);
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
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var physicalCounts = new List<PhysicalCount>();
            foreach (long id in selectedIds)
            {
                var physicalCount = _physicalCountRepository.GetById(id);
                if (physicalCount != null)
                {
                    if (!_physicalCountService.IsDeactivable(physicalCount))
                    {
                        ModelState.AddModelError("PhysicalCount", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        physicalCounts.Add(physicalCount);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var physicalCount in physicalCounts)
                    _physicalCountRepository.Deactivate(physicalCount);
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

        #region PhysicalCountItem

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Read,Inventory.PhysicalCount.Update")]
        public ActionResult PhysicalCountItemList(long physicalCountId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _physicalCountItemRepository.GetAll().Where(c => c.PhysicalCountId == physicalCountId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var physicalCountItems = new PagedList<PhysicalCountItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = physicalCountItems.Select(x => x.ToModel()),
                Total = physicalCountItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Read,Inventory.PhysicalCount.Update")]
        public ActionResult PhysicalCountItem(long id)
        {
            var physicalCountItem = _physicalCountItemRepository.GetById(id);
            var model = physicalCountItem.ToModel();
            //model.CurrentQuantity = _storeService.GetTotalQuantity(null, model.StoreLocatorId, model.ItemId);
            var html = this.PhysicalCountItemPanel(model);
            return Json(new { Id = physicalCountItem.Id, Html = html });
        }

        [NonAction]
        public string PhysicalCountItemPanel(PhysicalCountItemModel model)
        {
            var html = this.RenderPartialViewToString("_PhysicalCountItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Update")]
        public ActionResult SavePhysicalCountItem(PhysicalCountItemModel model)
        {
            if (ModelState.IsValid)
            {
                var physicalCountItem = _physicalCountItemRepository.GetById(model.Id);
                //always set IsNew to false when saving
                physicalCountItem.IsNew = false;
                physicalCountItem = model.ToEntity(physicalCountItem);
                _physicalCountItemRepository.UpdateAndCommit(physicalCountItem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Update")]
        public ActionResult CancelPhysicalCountItem(long id)
        {
            var physicalCountItem = _physicalCountItemRepository.GetById(id);
            if (physicalCountItem.IsNew == true)
            {
                _physicalCountItemRepository.DeleteAndCommit(physicalCountItem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Update")]
        public ActionResult DeletePhysicalCountItem(long? parentId, long id)
        {
            var physicalCountItem = _physicalCountItemRepository.GetById(id);
            _physicalCountItemRepository.DeactivateAndCommit(physicalCountItem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Update")]
        public ActionResult DeleteSelectedPhysicalCountItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var physicalCountItem = _physicalCountItemRepository.GetById(id);
                _physicalCountItemRepository.Deactivate(physicalCountItem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Create PhysicalCount Items

        [HttpGet]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Update")]
        public ActionResult CreatePhysicalCountItemsView()
        {
            var model = BuildCreatePhysicalCountItemsSearchModel();
            return PartialView("_CreatePhysicalCountItems", model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Update")]
        public ActionResult CreatePhysicalCountItemList(long storeId, long physicalCountId, DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildCreatePhysicalCountItemsSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                var expression = model.ToExpression(this._workContext.CurrentUser.Id);
                //do a hack here to add storeId & itemId filter
                expression = expression + " AND Store.Id = " + storeId;
                PagedResult<StoreLocatorItemBalance> data = _storeService.GetStoreLocatorItemBalances(expression, model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => new PhysicalCountItemModel { PhysicalCountId = physicalCountId, StoreLocatorItemBalance = x }),
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
        /// The list of creating physicalCount items is in updatedItems 
        /// </summary>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.PhysicalCount.Create,Inventory.PhysicalCount.Update")]
        public ActionResult CreatePhysicalCountItems([Bind(Prefix = "updated")]List<PhysicalCountItemModel> updatedItems,
           [Bind(Prefix = "created")]List<PhysicalCountItemModel> createdItems,
           [Bind(Prefix = "deleted")]List<PhysicalCountItemModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create PhysicalCountItems
                    if (updatedItems != null)
                    {
                        //get the current physicalCount
                        var physicalCountId = updatedItems.Count > 0 ? updatedItems[0].PhysicalCountId : 0;
                        var physicalCountItems = _physicalCountItemRepository.GetAll().Where(r => r.PhysicalCountId == physicalCountId).ToList();
                        foreach (var model in updatedItems)
                        {
                            //we don't merge if the physicalCount item already existed
                            //if (!physicalCountItems.Any(r => r.ItemId == model.Item.Id))
                            {
                                //manually mapping here to set only foreign key
                                //if used AutoMapper, the reference property will also be mapped
                                //and EF will consider these properties as new and insert it
                                //so db records will be duplicated
                                //we can also ignore it in AutoMapper configuation instead of manually mapping
                                var physicalCountItem = new PhysicalCountItem
                                {
                                    PhysicalCountId = model.PhysicalCountId,
                                    ItemId = model.StoreLocatorItemBalance.ItemId,
                                    StoreLocatorId = model.StoreLocatorItemBalance.StoreLocatorId,
                                    CurrentQuantity = model.StoreLocatorItemBalance.TotalQuantity,
                                    Count = model.Count
                                };
                                _physicalCountItemRepository.Insert(physicalCountItem);
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