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
    public class ReturnController : BaseController
    {
        #region Fields

        private readonly IRepository<Return> _returnRepository;
        private readonly IRepository<ReturnItem> _returnItemRepository;
        private readonly IReturnService _returnService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IIssueService _issueService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public ReturnController(IRepository<Return> returnRepository,
            IRepository<ReturnItem> returnItemRepository,
            IReturnService returnService,
            IAutoNumberService autoNumberService,
            IIssueService issueService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._returnRepository = returnRepository;
            this._returnItemRepository = returnItemRepository;
            this._localizationService = localizationService;
            this._returnService = returnService;
            this._autoNumberService = autoNumberService;
            this._issueService = issueService;
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
            var returnEntityNumberFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Number",
                ResourceKey = "Return.Number",
                DbColumn = "Return.Number, Return.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(returnEntityNumberFilter);

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

            var returnDateFromFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "ReturnDateFrom",
                ResourceKey = "Return.ReturnDateFrom",
                DbColumn = "Return.ReturnDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(returnDateFromFilter);

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

            var returnDateToFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "ReturnDateTo",
                ResourceKey = "Return.ReturnDateTo",
                DbColumn = "Return.ReturnDate",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(returnDateToFilter);

            var isApprovedFilter = new FieldModel
            {
                DisplayOrder = 6,
                Name = "IsApproved",
                ResourceKey = "Return.IsApproved",
                DbColumn = "Return.IsApproved",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Boolean,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "True,False",
                CsvValueList = "True,False",
                IsRequiredField = false
            };
            model.Filters.Add(isApprovedFilter);

            return model;
        }

        private SearchModel BuildCreateReturnItemsSearchModel()
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

        private void Validate(ReturnModel model, Return returnEntity, string actionName)
        {
            if (!string.IsNullOrEmpty(actionName))
            {
                if (actionName == WorkflowActionName.Approve)
                {
                    if (model.ReturnDate != null && model.ReturnDate < DateTime.UtcNow.Date)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Return.ReturnDateCannotEarlierThanToday"));
                    }
                    if (returnEntity.IsApproved == true)
                    {
                        ModelState.AddModelError("", _localizationService.GetResource("Record.AlreadyApproved"));
                    }

                    var insufficientList = _returnService.CheckSufficientQuantity(returnEntity);
                    if (insufficientList.Count > 0)
                    {
                        foreach (var item in insufficientList)
                        {
                            ModelState.AddModelError("", string.Format(_localizationService.GetResource("Return.InSufficientQuantiy"), item.ItemName, item.StoreLocatorName));
                        }
                    }
                }
            }
        }

        #endregion

        #region Returns

        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ReturnSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ReturnSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ReturnSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.ReturnSearchModel] = model;

                PagedResult<Return> data = _returnService.GetReturns(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create")]
        public ActionResult Create()
        {
            var returnEntity = new Return { IsNew = true };
            _returnRepository.InsertAndCommit(returnEntity);
            return Json(new { Id = returnEntity.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Return>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Read,Inventory.Return.Update")]
        public ActionResult Edit(long id)
        {
            var returnEntity = _returnRepository.GetById(id);
            var model = returnEntity.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Update")]
        public ActionResult Edit(ReturnModel model)
        {
            var returnEntity = _returnRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                returnEntity = model.ToEntity(returnEntity);

                if (returnEntity.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), returnEntity);
                    returnEntity.Number = number;
                }

                //always set IsNew to false when saving
                returnEntity.IsNew = false;
                //update attributes
                _returnRepository.Update(returnEntity);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = returnEntity.Number, isApproved = returnEntity.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Approve")]
        public ActionResult Approve(ReturnModel model)
        {
            var returnEntity = _returnRepository.GetById(model.Id);
            Validate(model, returnEntity, WorkflowActionName.Approve);
            if (ModelState.IsValid)
            {
                returnEntity = model.ToEntity(returnEntity);

                if (returnEntity.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), returnEntity);
                    returnEntity.Number = number;
                }

                //always set IsNew to false when saving
                returnEntity.IsNew = false;
                //update attributes
                _returnRepository.Update(returnEntity);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //approve
                _returnService.Approve(returnEntity);

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = returnEntity.Number, isApproved = returnEntity.IsApproved });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var returnEntity = _returnRepository.GetById(id);

            if (!_returnService.IsDeactivable(returnEntity))
            {
                ModelState.AddModelError("Return", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _returnRepository.DeactivateAndCommit(returnEntity);
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
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var returnEntities = new List<Return>();
            foreach (long id in selectedIds)
            {
                var returnEntity = _returnRepository.GetById(id);
                if (returnEntity != null)
                {
                    if (!_returnService.IsDeactivable(returnEntity))
                    {
                        ModelState.AddModelError("Return", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        returnEntities.Add(returnEntity);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var returnEntity in returnEntities)
                    _returnRepository.Deactivate(returnEntity);
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

        #region ReturnItem

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Read,Inventory.Return.Update")]
        public ActionResult ReturnItemList(long returnId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _returnItemRepository.GetAll().Where(c => c.ReturnId == returnId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var returnItems = new PagedList<ReturnItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = returnItems.Select(x => x.ToModel()),
                Total = returnItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Read,Inventory.Return.Update")]
        public ActionResult ReturnItem(long id)
        {
            var returnItem = _returnItemRepository.GetById(id);
            var model = returnItem.ToModel();
            var html = this.ReturnItemPanel(model);
            return Json(new { Id = returnItem.Id, Html = html });
        }

        [NonAction]
        public string ReturnItemPanel(ReturnItemModel model)
        {
            var html = this.RenderPartialViewToString("_ReturnItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Update")]
        public ActionResult SaveReturnItem(ReturnItemModel model)
        {
            if (ModelState.IsValid)
            {
                var returnItem = _returnItemRepository.GetById(model.Id);
                //always set IsNew to false when saving
                returnItem.IsNew = false;
                returnItem.IssueItemId = model.IssueItemId;
                returnItem.ReturnQuantity = model.ReturnQuantity;
                _returnItemRepository.UpdateAndCommit(returnItem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Update")]
        public ActionResult CancelReturnItem(long id)
        {
            var returnItem = _returnItemRepository.GetById(id);
            if (returnItem.IsNew == true)
            {
                _returnItemRepository.DeleteAndCommit(returnItem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Update")]
        public ActionResult DeleteReturnItem(long? parentId, long id)
        {
            var returnItem = _returnItemRepository.GetById(id);
            _returnItemRepository.DeactivateAndCommit(returnItem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Update")]
        public ActionResult DeleteSelectedReturnItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var returnItem = _returnItemRepository.GetById(id);
                _returnItemRepository.Deactivate(returnItem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Create Return Items

        [HttpGet]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Update")]
        public ActionResult CreateReturnItemsView()
        {
            var model = BuildCreateReturnItemsSearchModel();
            return PartialView("_CreateReturnItems", model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Update")]
        public ActionResult CreateReturnItemList(long returnId, long storeId, DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildCreateReturnItemsSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                var expression = model.ToExpression();
                //do a hack here to add storeId filter
                expression = expression + " AND Store.Id = " + storeId;
                PagedResult<IssueItem> data = _issueService.GetApprovedIssueItems(expression, model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => new ReturnItemModel { ReturnId = returnId, IssueItemId = x.Id, IssueItem = x.ToModel() }),
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
        /// The list of creating return items is in updatedItems 
        /// </summary>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Return.Create,Inventory.Return.Update")]
        public ActionResult CreateReturnItems([Bind(Prefix = "updated")]List<ReturnItemModel> updatedItems,
           [Bind(Prefix = "created")]List<ReturnItemModel> createdItems,
           [Bind(Prefix = "deleted")]List<ReturnItemModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create ReturnItems
                    if (updatedItems != null)
                    {
                        //get the current return entity
                        var returnEntityId = updatedItems.Count > 0 ? updatedItems[0].ReturnId : 0;
                        var returnItems = _returnItemRepository.GetAll().Where(r => r.ReturnId == returnEntityId).ToList();
                        foreach (var model in updatedItems)
                        {
                            //we don't merge if the return item already existed
                            if (!returnItems.Any(r => r.IssueItem.Id == model.IssueItemId))
                            {
                                //manually mapping here to set only foreign key
                                //if used AutoMapper, the reference property will also be mapped
                                //and EF will consider these properties as new and insert it
                                //so db records will be duplicated
                                //we can also ignore it in AutoMapper configuation instead of manually mapping
                                var returnItem = new ReturnItem
                                {
                                    ReturnId = model.ReturnId,
                                    IssueItemId = model.IssueItemId,
                                    ReturnQuantity = model.ReturnQuantity
                                };
                                _returnItemRepository.Insert(returnItem);
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