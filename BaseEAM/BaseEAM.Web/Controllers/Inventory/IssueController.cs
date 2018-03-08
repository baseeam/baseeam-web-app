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
    public class IssueController : BaseController
    {
        #region Fields

        private readonly IRepository<Issue> _issueRepository;
        private readonly IRepository<IssueItem> _issueItemRepository;
        private readonly IRepository<Item> _itemRepository;
        private readonly IIssueService _issueService;
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

        public IssueController(IRepository<Issue> issueRepository,
            IRepository<IssueItem> issueItemRepository,
            IRepository<Item> itemRepository,
            IIssueService issueService,
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
            this._issueRepository = issueRepository;
            this._issueItemRepository = issueItemRepository;
            this._itemRepository = itemRepository;
            this._localizationService = localizationService;
            this._issueService = issueService;
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
            var issueNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Number",
                ResourceKey = "Issue.Number",
                DbColumn = "Issue.Number, Issue.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(issueNameFilter);

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

            var issueDateFromFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "IssueDateFrom",
                ResourceKey = "Issue.IssueDateFrom",
                DbColumn = "Issue.IssueDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(issueDateFromFilter);

            var issueDateToFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "IssueDateTo",
                ResourceKey = "Issue.IssueDateTo",
                DbColumn = "Issue.IssueDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(issueDateToFilter);

            return model;
        }

        private SearchModel BuildCreateIssueItemsSearchModel()
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

        private void Validate(IssueModel model, Issue issue, string actionName)
        {
            if (!string.IsNullOrEmpty(actionName))
            {
                if (actionName == WorkflowActionName.Approve)
                {
                    if (model.IssueDate != null && model.IssueDate < DateTime.UtcNow.Date)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Issue.IssueDateCannotEarlierThanToday"));
                    }
                    if (issue.IsApproved == true)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Record.AlreadyApproved"));
                    }

                    var insufficientList = _issueService.CheckSufficientQuantity(issue);
                    if(insufficientList.Count > 0)
                    {
                        foreach(var item in insufficientList)
                        {
                            ModelState.AddModelError("", string.Format(_localizationService.GetResource("Issue.InSufficientQuantiy"), item.ItemName, item.StoreLocatorName));
                        }
                    }
                }
            }
        }

        private void PrepareIssueItemModel(IssueItemModel model)
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
                    Selected = model.IssueUnitOfMeasureId == c.Id
                });
            }
        }

        #endregion

        #region Issues

        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.IssueSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.IssueSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.IssueSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.IssueSearchModel] = model;

                PagedResult<Issue> data = _issueService.GetIssues(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create")]
        public ActionResult Create()
        {
            var issue = new Issue { IsNew = true };
            _issueRepository.InsertAndCommit(issue);
            return Json(new { Id = issue.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Issue>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Read,Inventory.Issue.Update")]
        public ActionResult Edit(long id)
        {
            var issue = _issueRepository.GetById(id);
            var model = issue.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Update")]
        public ActionResult Edit(IssueModel model)
        {
            var issue = _issueRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                issue = model.ToEntity(issue);

                if (issue.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), issue);
                    issue.Number = number;
                }

                //always set IsNew to false when saving
                issue.IsNew = false;
                //update attributes
                _issueRepository.Update(issue);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = issue.Number, isApproved = issue.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Approve")]
        public ActionResult Approve(IssueModel model)
        {
            var issue = _issueRepository.GetById(model.Id);
            Validate(model, issue, WorkflowActionName.Approve);
            if (ModelState.IsValid)
            {
                issue = model.ToEntity(issue);

                if (issue.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), issue);
                    issue.Number = number;
                }

                //always set IsNew to false when saving
                issue.IsNew = false;
                //update attributes
                _issueRepository.Update(issue);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //approve
                _issueService.Approve(issue);

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = issue.Number, isApproved = issue.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var issue = _issueRepository.GetById(id);

            if (!_issueService.IsDeactivable(issue))
            {
                ModelState.AddModelError("Issue", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _issueRepository.DeactivateAndCommit(issue);
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
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var issues = new List<Issue>();
            foreach (long id in selectedIds)
            {
                var issue = _issueRepository.GetById(id);
                if (issue != null)
                {
                    if (!_issueService.IsDeactivable(issue))
                    {
                        ModelState.AddModelError("Issue", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        issues.Add(issue);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var issue in issues)
                    _issueRepository.Deactivate(issue);
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

        #region IssueItem

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Read,Inventory.Issue.Update")]
        public ActionResult IssueItemList(long issueId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _issueItemRepository.GetAll().Where(c => c.IssueId == issueId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var issueItems = new PagedList<IssueItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = issueItems.Select(x => x.ToModel()),
                Total = issueItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Read,Inventory.Issue.Update")]
        public ActionResult IssueItem(long id)
        {
            var issueItem = _issueItemRepository.GetById(id);
            var model = issueItem.ToModel();
            PrepareIssueItemModel(model);
            var html = this.IssueItemPanel(model);
            return Json(new { Id = issueItem.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Update")]
        public ActionResult CreateIssueItem(long issueId)
        {
            //need to get issue here to assign to new issueItem
            //so when mapping to Model, we will have StoreId as defined
            //in AutoMapper configuration
            var issue = _issueRepository.GetById(issueId);
            var issueItem = new IssueItem
            {
                IsNew = true,
                Issue = issue
            };
            _issueItemRepository.Insert(issueItem);

            this._dbContext.SaveChanges();

            var model = new IssueItemModel();
            model = issueItem.ToModel();
            var html = this.IssueItemPanel(model);

            return Json(new { Id = issueItem.Id, Html = html });
        }

        [NonAction]
        public string IssueItemPanel(IssueItemModel model)
        {
            var html = this.RenderPartialViewToString("_IssueItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Update")]
        public ActionResult SaveIssueItem(IssueItemModel model)
        {
            if (ModelState.IsValid)
            {
                var issueItem = _issueItemRepository.GetById(model.Id);

                //always set IsNew to false when saving
                issueItem.IsNew = false;
                issueItem = model.ToEntity(issueItem);

                // unit conversion
                var item = _itemRepository.GetById(model.ItemId);
                if (item.UnitOfMeasureId != model.IssueUnitOfMeasureId)
                {
                    var uc = _unitConversionService.GetUnitConversion(model.IssueUnitOfMeasureId.Value, item.UnitOfMeasureId.Value);
                    issueItem.Quantity = issueItem.IssueQuantity * uc.ConversionFactor;
                }
                else
                {
                    issueItem.Quantity = issueItem.IssueQuantity;
                }

                //update issue cost
                _issueService.UpdateIssueCost(issueItem);
                _issueItemRepository.UpdateAndCommit(issueItem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Update")]
        public ActionResult CancelIssueItem(long id)
        {
            var issueItem = _issueItemRepository.GetById(id);
            if (issueItem.IsNew == true)
            {
                _issueItemRepository.DeleteAndCommit(issueItem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Update")]
        public ActionResult DeleteIssueItem(long? parentId, long id)
        {
            var issueItem = _issueItemRepository.GetById(id);
            _issueItemRepository.DeactivateAndCommit(issueItem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Update")]
        public ActionResult DeleteSelectedIssueItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var issueItem = _issueItemRepository.GetById(id);
                _issueItemRepository.Deactivate(issueItem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Create Issue Items

        [HttpGet]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Update")]
        public ActionResult CreateIssueItemsView()
        {
            var model = BuildCreateIssueItemsSearchModel();
            return PartialView("_CreateIssueItems", model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Update")]
        public ActionResult CreateIssueItemList(long issueId, DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildCreateIssueItemsSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Item> data = _itemService.GetItems(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => new IssueItemModel
                    {
                        IssueId = issueId,
                        ItemId = x.Id,
                        ItemName = x.Name,
                        IssueUnitOfMeasureId = x.UnitOfMeasureId,
                        IssueUnitOfMeasureName = x.UnitOfMeasure.Name,
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
        /// The list of creating issue items is in updatedItems 
        /// </summary>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Issue.Create,Inventory.Issue.Update")]
        public ActionResult CreateIssueItems([Bind(Prefix = "updated")]List<IssueItemModel> updatedItems,
           [Bind(Prefix = "created")]List<IssueItemModel> createdItems,
           [Bind(Prefix = "deleted")]List<IssueItemModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create IssueItems
                    if (updatedItems != null)
                    {
                        //get the current issue
                        var issueId = updatedItems.Count > 0 ? updatedItems[0].IssueId : 0;
                        var issueItems = _issueItemRepository.GetAll().Where(r => r.IssueId == issueId).ToList();
                        foreach (var model in updatedItems)
                        {
                            //we don't merge if the issue item already existed
                            if (!issueItems.Any(r => r.ItemId == model.ItemId))
                            {
                                //manually mapping here to set only foreign key
                                //if used AutoMapper, the reference property will also be mapped
                                //and EF will consider these properties as new and insert it
                                //so db records will be duplicated
                                //we can also ignore it in AutoMapper configuation instead of manually mapping
                                var issueItem = new IssueItem
                                {
                                    IssueId = model.IssueId,
                                    StoreLocatorId = model.StoreLocator.Id,
                                    ItemId = model.ItemId,
                                    IssueQuantity = model.IssueQuantity,
                                    IssueUnitOfMeasureId = model.IssueUnitOfMeasureId,
                                    Quantity = model.IssueQuantity
                                };
                                _issueService.UpdateIssueCost(issueItem);
                                _issueItemRepository.Insert(issueItem);
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