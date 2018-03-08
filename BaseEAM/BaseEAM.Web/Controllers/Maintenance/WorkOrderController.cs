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
    public class WorkOrderController : BaseController
    {
        #region Fields

        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IRepository<Issue> _issueRepository;
        private readonly IRepository<WorkOrderLabor> _workOrderLaborRepository;
        private readonly IRepository<WorkOrderItem> _workOrderItemRepository;
        private readonly IRepository<WorkOrderTask> _workOrderTaskRepository;
        private readonly IRepository<WorkOrderMiscCost> _workOrderMiscCostRepository;
        private readonly IRepository<Point> _pointRepository;
        private readonly IRepository<Task> _taskRepository;
        private readonly IRepository<PointMeterLineItem> _pointMeterLineItemRepository;
        private readonly IRepository<WorkOrderServiceItem> _workOrderServiceItemRepository;
        private readonly IRepository<Assignment> _assignmentRepository;
        private readonly IRepository<AssignmentHistory> _assignmentHistoryRepository;
        private readonly IWorkOrderService _workOrderService;
        private readonly IMeterService _meterService;
        private readonly IIssueService _issueService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IItemService _itemService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IAttachmentService _attachmentService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;
        private readonly IWorkflowBaseService _workflowBaseService;

        #endregion

        #region Constructors

        public WorkOrderController(IRepository<WorkOrder> workOrderRepository,
            IRepository<Issue> issueRepository,
            IRepository<WorkOrderLabor> workOrderLaborRepository,
            IRepository<WorkOrderItem> workOrderItemRepository,
            IRepository<WorkOrderTask> workOrderTaskRepository,
            IRepository<WorkOrderMiscCost> workOrderMiscCostRepository,
            IRepository<Point> pointRepository,
            IRepository<Task> taskRepository,
            IRepository<PointMeterLineItem> pointMeterLineItemRepository,
            IRepository<WorkOrderServiceItem> workOrderServiceItemRepository,
            IRepository<Assignment> assignmentRepository,
            IRepository<AssignmentHistory> assignmentHistoryRepository,
            IWorkOrderService workOrderService,
            IMeterService meterService,
            IIssueService issueService,
            IAutoNumberService autoNumberService,
            IItemService itemService,
            IDateTimeHelper dateTimeHelper,
            IAttachmentService attachmentService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext,
            IWorkflowBaseService workflowBaseService)
        {
            this._workOrderRepository = workOrderRepository;
            this._issueRepository = issueRepository;
            this._workOrderLaborRepository = workOrderLaborRepository;
            this._workOrderItemRepository = workOrderItemRepository;
            this._workOrderTaskRepository = workOrderTaskRepository;
            this._workOrderMiscCostRepository = workOrderMiscCostRepository;
            this._pointRepository = pointRepository;
            this._taskRepository = taskRepository;
            this._pointMeterLineItemRepository = pointMeterLineItemRepository;
            this._workOrderServiceItemRepository = workOrderServiceItemRepository;
            this._assignmentRepository = assignmentRepository;
            this._assignmentHistoryRepository = assignmentHistoryRepository;
            this._localizationService = localizationService;
            this._workOrderService = workOrderService;
            this._meterService = meterService;
            this._issueService = issueService;
            this._autoNumberService = autoNumberService;
            this._itemService = itemService;
            this._dateTimeHelper = dateTimeHelper;
            this._attachmentService = attachmentService;
            this._permissionService = permissionService;
            this._httpContext = httpContext;
            this._workContext = workContext;
            this._dbContext = dbContext;
            this._workflowBaseService = workflowBaseService;
        }

        #endregion

        #region Utilities

        private SearchModel BuildSearchModel()
        {
            var model = new SearchModel();
            var numberFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Number",
                ResourceKey = "WorkOrder.Number",
                DbColumn = "WorkOrder.Number, WorkOrder.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(numberFilter);

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

            var priorityFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "Priority",
                ResourceKey = "Priority",
                DbColumn = "WorkOrder.Priority",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int32,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Urgent,High,Medium,Low",
                CsvValueList = "0,1,2,3",
                IsRequiredField = false
            };
            model.Filters.Add(priorityFilter);

            var assetFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "Asset",
                ResourceKey = "Asset",
                DbColumn = "Asset.Id",
                Value = null,
                ControlType = FieldControlType.Lookup,
                IsRequiredField = false,
                LookupType = "Asset",
                LookupValueField = "AssetId",
                LookupTextField = "AssetName",
                ParentFieldName = "Site"
            };

            model.Filters.Add(assetFilter);

            var statusFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "Status",
                ResourceKey = "WorkOrder.Status",
                DbColumn = "Assignment.Name",
                Value = null,
                ControlType = FieldControlType.MultiSelectList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Open,Planning,Execution,WaitingForMaterial,WaitingForVendor,Review,Closed,Rejected,Cancelled",
                CsvValueList = "'Open','Planning','Execution','WaitingForMaterial','WaitingForVendor','Review','Closed','Rejected','Cancelled'",
                IsRequiredField = false
            };
            model.Filters.Add(statusFilter);

            var locationFilter = new FieldModel
            {
                DisplayOrder = 6,
                Name = "Location",
                ResourceKey = "Location",
                DbColumn = "Location.Id",
                Value = null,
                ControlType = FieldControlType.Lookup,
                IsRequiredField = false,
                LookupType = "Location",
                LookupValueField = "LocationId",
                LookupTextField = "LocationName",
                ParentFieldName = "Site"
            };

            model.Filters.Add(locationFilter);

            var expectedStartDateTimeFromFilter = new FieldModel
            {
                DisplayOrder = 7,
                Name = "ExpectedStartDateTimeFrom",
                ResourceKey = "WorkOrder.ExpectedStartDateTimeFrom",
                DbColumn = "WorkOrder.ExpectedStartDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(expectedStartDateTimeFromFilter);

            var expectedStartDateTimeToFilter = new FieldModel
            {
                DisplayOrder = 8,
                Name = "ExpectedStartDateTimeTo",
                ResourceKey = "WorkOrder.ExpectedStartDateTimeTo",
                DbColumn = "WorkOrder.ExpectedStartDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(expectedStartDateTimeToFilter);

            var dueDateTimeFromFilter = new FieldModel
            {
                DisplayOrder = 9,
                Name = "DueDateTimeFrom",
                ResourceKey = "WorkOrder.DueDateTimeFrom",
                DbColumn = "WorkOrder.DueDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(dueDateTimeFromFilter);

            var dueDateTimeToFilter = new FieldModel
            {
                DisplayOrder = 10,
                Name = "dueDateTimeTo",
                ResourceKey = "WorkOrder.DueDateTimeTo",
                DbColumn = "WorkOrder.DueDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(dueDateTimeToFilter);

            return model;
        }

        #endregion

        #region WorkOrders

        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.WorkOrderSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.WorkOrderSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.WorkOrderSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.WorkOrderSearchModel] = model;

                PagedResult<WorkOrder> data = _workOrderService.GetWorkOrders(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var item in result)
                {
                    item.PriorityText = item.Priority.ToString();
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

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create")]
        public ActionResult Create()
        {
            var workOrder = new WorkOrder
            {
                IsNew = true,
                Priority = (int?)AssignmentPriority.Medium,
                RequestorName = _workContext.CurrentUser.Name,
                RequestorEmail = _workContext.CurrentUser.Email,
                RequestorPhone = _workContext.CurrentUser.Phone,
                RequestedDateTime = DateTime.UtcNow,
                CreatedUserId = this._workContext.CurrentUser.Id
            };
            _workOrderRepository.InsertAndCommit(workOrder);

            //start workflow
            var workflowInstanceId = WorkflowServiceClient.StartWorkflow(workOrder.Id, EntityType.WorkOrder, 0, this._workContext.CurrentUser.Id);
            return Json(new { Id = workOrder.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            var workOrder = _workOrderRepository.GetById(id);
            var assignment = workOrder.Assignment;
            var assignmentHistories = _assignmentHistoryRepository.GetAll()
                .Where(a => a.EntityId == workOrder.Id && a.EntityType == EntityType.WorkOrder)
                .ToList();

            _workOrderRepository.Delete(workOrder);
            _assignmentRepository.Delete(assignment);
            foreach (var history in assignmentHistories)
                _assignmentHistoryRepository.Delete(history);

            this._dbContext.SaveChanges();

            //cancel wf
            WorkflowServiceClient.CancelWorkflow(
                workOrder.Id, EntityType.WorkOrder, assignment.WorkflowDefinitionId, assignment.WorkflowInstanceId,
                assignment.WorkflowVersion.Value, this._workContext.CurrentUser.Id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Read,Maintenance.WorkOrder.Update")]
        public ActionResult Edit(long id)
        {
            var workOrder = _workOrderRepository.GetById(id);
            var model = workOrder.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult Edit(WorkOrderModel model)
        {
            var workOrder = _workOrderRepository.GetById(model.Id);
            var assignment = workOrder.Assignment;
            if (ModelState.IsValid)
            {
                workOrder = model.ToEntity(workOrder);

                if (workOrder.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), workOrder);
                    workOrder.Number = number;
                    _workOrderService.GenerateWorkOrderTasks(workOrder, model.AssetId);
                }
                //always set IsNew to false when saving
                workOrder.IsNew = false;
                //copy to Assignment
                if (workOrder.Assignment != null)
                {
                    workOrder.Assignment.Number = workOrder.Number;
                    workOrder.Assignment.Description = workOrder.Description;
                    workOrder.Assignment.Priority = workOrder.Priority;
                    workOrder.Assignment.AssignmentAmount = workOrder.AssignmentAmount;
                }
                _workOrderRepository.Update(workOrder);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //trigger workflow action
                if (!string.IsNullOrEmpty(model.ActionName))
                {
                    WorkflowServiceClient.TriggerWorkflowAction(workOrder.Id, EntityType.WorkOrder, assignment.WorkflowDefinitionId, assignment.WorkflowInstanceId,
                        assignment.WorkflowVersion.Value, model.ActionName, model.Comment, this._workContext.CurrentUser.Id);
                    //Every time we query twice, because EF is caching entities so it won't get the latest value from DB
                    //We need to detach the specified entity and load it again
                    this._dbContext.Detach(workOrder.Assignment);
                    assignment = _assignmentRepository.GetById(workOrder.AssignmentId);
                }

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new
                {
                    number = workOrder.Number,
                    status = assignment.Name,
                    assignedUsers = assignment.Users.Select(u => u.Name),
                    availableActions = assignment.AvailableActions ?? ""
                });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var workOrder = _workOrderRepository.GetById(id);

            if (!_workOrderService.IsDeactivable(workOrder))
            {
                ModelState.AddModelError("WorkOrder", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                var assignment = workOrder.Assignment;
                var assignmentHistories = _assignmentHistoryRepository.GetAll()
                    .Where(a => a.EntityId == workOrder.Id && a.EntityType == EntityType.WorkOrder)
                    .ToList();

                _workOrderRepository.Deactivate(workOrder);
                _assignmentRepository.Deactivate(assignment);
                foreach (var history in assignmentHistories)
                    _assignmentHistoryRepository.Deactivate(history);

                this._dbContext.SaveChanges();

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
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var workOrders = new List<WorkOrder>();
            foreach (long id in selectedIds)
            {
                var workOrder = _workOrderRepository.GetById(id);
                if (workOrder != null)
                {
                    if (!_workOrderService.IsDeactivable(workOrder))
                    {
                        ModelState.AddModelError("WorkOrder", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        workOrders.Add(workOrder);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var workOrder in workOrders)
                {
                    var assignment = workOrder.Assignment;
                    var assignmentHistories = _assignmentHistoryRepository.GetAll()
                        .Where(a => a.EntityId == workOrder.Id && a.EntityType == EntityType.WorkOrder)
                        .ToList();

                    _workOrderRepository.Deactivate(workOrder);
                    _assignmentRepository.Deactivate(assignment);
                    foreach (var history in assignmentHistories)
                        _assignmentHistoryRepository.Deactivate(history);
                }
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

        #region WorkOrderLabors

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Read,Maintenance.WorkOrder.Update")]
        public ActionResult WorkOrderLaborList(long workOrderId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderLaborRepository.GetAll().Where(c => c.WorkOrderId == workOrderId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var workOrderLabors = new PagedList<WorkOrderLabor>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = workOrderLabors.Select(x => x.ToModel()),
                Total = workOrderLabors.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Read,Maintenance.WorkOrder.Update")]
        public ActionResult WorkOrderLabor(long id)
        {
            var workOrderLabor = _workOrderLaborRepository.GetById(id);
            var model = workOrderLabor.ToModel();
            var html = this.WorkOrderLaborPanel(model);
            return Json(new { Id = workOrderLabor.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult CreateWorkOrderLabor(long workOrderId)
        {
            var workOrderLabor = new WorkOrderLabor
            {
                IsNew = true,
                WorkOrderId = workOrderId,
                PlanHours = 1.0M,
                StandardRate = 1.0M,
                OTRate = 1.0M
            };
            _workOrderLaborRepository.InsertAndCommit(workOrderLabor);

            var model = new WorkOrderLaborModel();
            model = workOrderLabor.ToModel();
            var html = this.WorkOrderLaborPanel(model);

            return Json(new { Id = workOrderLabor.Id, Html = html });
        }

        [NonAction]
        public string WorkOrderLaborPanel(WorkOrderLaborModel model)
        {
            var html = this.RenderPartialViewToString("_WorkOrderLaborDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult SaveWorkOrderLabor(WorkOrderLaborModel model)
        {
            if (ModelState.IsValid)
            {
                var workOrderLabor = _workOrderLaborRepository.GetById(model.Id);
                //always set IsNew to false when saving
                workOrderLabor.IsNew = false;
                //update totals
                model.PlanTotal = (model.PlanHours ?? 0) * (model.StandardRate ?? 0);
                model.ActualTotal = (model.ActualNormalHours ?? 0) * (model.StandardRate ?? 0)
                    + (model.ActualOTHours ?? 0) * (model.OTRate ?? 0);

                workOrderLabor = model.ToEntity(workOrderLabor);
                _workOrderLaborRepository.UpdateAndCommit(workOrderLabor);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult CancelWorkOrderLabor(long id)
        {
            var workOrderLabor = _workOrderLaborRepository.GetById(id);
            if (workOrderLabor.IsNew == true)
            {
                _workOrderLaborRepository.DeleteAndCommit(workOrderLabor);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult DeleteWorkOrderLabor(long? parentId, long id)
        {
            var workOrderLabor = _workOrderLaborRepository.GetById(id);
            _workOrderLaborRepository.DeactivateAndCommit(workOrderLabor);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult DeleteSelectedWorkOrderLabors(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var workOrderLabor = _workOrderLaborRepository.GetById(id);
                _workOrderLaborRepository.Deactivate(workOrderLabor);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        /// <summary>
        /// Get the list of technician from Work Order Labor
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AssignedUserListFromWOLabor(long workOrderId, string param)
        {
            var technicians = _workOrderLaborRepository.GetAll()
                .Where(w => w.WorkOrderId == workOrderId && w.Technician.User.Name.Contains(param))
                .Select(w => new SelectListItem { Text = w.Technician.User.Name, Value = w.Technician.User.Id.ToString() })
                .ToList();
            if (technicians.Count > 0)
            {
                technicians.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(technicians);
        }

        [HttpPost]
        public JsonResult CompletedUserListFromWOLabor(long workOrderId, string param)
        {
            var technicians = _workOrderLaborRepository.GetAll()
                .Where(w => w.WorkOrderId == workOrderId && w.Technician.User.Name.Contains(param))
                .Select(w => new SelectListItem { Text = w.Technician.User.Name, Value = w.Technician.User.Id.ToString() })
                .ToList();

            var currentUser = this._workContext.CurrentUser;

            // in case of reassign 
            // the current user is not in the labor list
            // so we need to add it to the completed user list
            if (!technicians.Any(t => t.Value == currentUser.Id.ToString()))
            {
                technicians.Add(new SelectListItem { Text = currentUser.Name, Value = currentUser.Id.ToString() });
            }
            if (technicians.Count > 0)
            {
                technicians.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(technicians);
        }

        #endregion

        #region WorkOrderItems

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Read,Maintenance.WorkOrder.Update")]
        public ActionResult WorkOrderItemList(long workOrderId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderItemRepository.GetAll().Where(c => c.WorkOrderId == workOrderId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var workOrderItems = new PagedList<WorkOrderItem>(query, command.Page - 1, command.PageSize);
            var result = workOrderItems.Select(x => x.ToModel()).ToList();

            foreach (var r in result)
            {
                r.ItemItemCategoryText = r.ItemItemCategory.ToString();
            }
            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = workOrderItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Read,Maintenance.WorkOrder.Update")]
        public ActionResult WorkOrderItem(long id)
        {
            var workOrderItem = _workOrderItemRepository.GetById(id);
            var model = workOrderItem.ToModel();
            model.ItemItemCategoryText = model.ItemItemCategory.ToString();
            var html = this.WorkOrderItemPanel(model);
            return Json(new { Id = workOrderItem.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult CreateWorkOrderItem(long workOrderId)
        {
            var workOrderitem = new WorkOrderItem
            {
                IsNew = true,
                WorkOrderId = workOrderId
            };
            _workOrderItemRepository.InsertAndCommit(workOrderitem);

            var model = new WorkOrderItemModel();
            model = workOrderitem.ToModel();
            var html = this.WorkOrderItemPanel(model);

            return Json(new { Id = workOrderitem.Id, Html = html });
        }

        [NonAction]
        public string WorkOrderItemPanel(WorkOrderItemModel model)
        {
            var html = this.RenderPartialViewToString("_WorkOrderItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult SaveWorkOrderItem(WorkOrderItemModel model)
        {
            if (ModelState.IsValid)
            {
                var workOrderitem = _workOrderItemRepository.GetById(model.Id);
                //always set IsNew to false when saving
                workOrderitem.IsNew = false;
                //update totals
                if (model.PlanToolHours > 0)
                {
                    model.PlanTotal = (model.ToolRate ?? 0) * (model.PlanToolHours ?? 0) * (model.PlanQuantity ?? 0);
                    model.ActualTotal = (model.ActualToolHours ?? 0) * (model.ToolRate ?? 0) * (model.ActualToolHours ?? 0);
                }
                else
                {
                    model.PlanTotal = (model.PlanQuantity ?? 0) * (model.UnitPrice ?? 0);
                    model.ActualTotal = (model.ActualQuantity ?? 0) * (model.UnitPrice ?? 0);
                }

                workOrderitem = model.ToEntity(workOrderitem);
                _workOrderItemRepository.UpdateAndCommit(workOrderitem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult CancelWorkOrderItem(long id)
        {
            var workOrderitem = _workOrderItemRepository.GetById(id);
            if (workOrderitem.IsNew == true)
            {
                _workOrderItemRepository.DeleteAndCommit(workOrderitem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult DeleteWorkOrderItem(long? parentId, long id)
        {
            var workOrderitem = _workOrderItemRepository.GetById(id);
            _workOrderItemRepository.DeactivateAndCommit(workOrderitem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult DeleteSelectedWorkOrderItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var workOrderitem = _workOrderItemRepository.GetById(id);
                _workOrderItemRepository.Deactivate(workOrderitem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult IssueAction(long workOrderId, string issueActionName)
        {
            if(string.IsNullOrEmpty(issueActionName))
                return new NullJsonResult();

            var wo = _workOrderRepository.GetById(workOrderId);
            var wois = new List<WorkOrderItem>();
            if (issueActionName.Equals("Material"))
            {
                wois = _workOrderItemRepository.GetAll()
                    .Where(i => i.WorkOrderId == workOrderId && i.Item.ItemCategory == (int?)ItemCategory.Part).ToList();
            }
            else if (issueActionName.Equals("Tool"))
            {
                wois = _workOrderItemRepository.GetAll()
                    .Where(i => i.WorkOrderId == workOrderId && i.Item.ItemCategory == (int?)ItemCategory.Tool).ToList();
            }
            else if (issueActionName.Equals("All"))
            {
                wois = _workOrderItemRepository.GetAll().Where(i => i.WorkOrderId == workOrderId).ToList();
            }

            //Validate
            foreach (var woi in wois)
            {
                var existingIssue = _issueRepository.GetAll()
                    .Where(i => i.WorkOrderId == workOrderId && i.IssueItems.Any(ii => ii.ItemId == woi.ItemId)).FirstOrDefault();
                if (existingIssue != null)
                {
                    return Json(new { Errors = string.Format(_localizationService.GetResource("Issue.AlreadyCreatedForThisWO"), existingIssue.Number) });
                }
            }

            //Create issues base on storeId 
            List<Issue> issues = new List<Issue>();
            var storeIds = wois.Select(s => s.StoreId).Distinct();
            foreach(var id in storeIds)
            {
                var newIssue = new Issue();
                newIssue.WorkOrderId = workOrderId;
                newIssue.IssueTo = (int?)IssueTo.WorkOrder;
                newIssue.IssueDate = DateTime.UtcNow;
                newIssue.SiteId = wo.SiteId;
                newIssue.UserId = this._workContext.CurrentUser.Id;
                newIssue.Description = string.Format(_localizationService.GetResource("Issue.IssueForWO"), wo.Number);
                newIssue.StoreId = id;
                issues.Add(newIssue);
            }
            //Add issue items into the issues from wois
            foreach (var woi in wois)
            {
                var newIssueItem = new IssueItem();
                newIssueItem.StoreLocatorId = woi.StoreLocatorId;
                newIssueItem.ItemId = woi.ItemId;
                newIssueItem.IssueQuantity = woi.PlanQuantity;
                newIssueItem.Quantity = newIssueItem.IssueQuantity;
                newIssueItem.IssueUnitOfMeasureId = woi.Item.UnitOfMeasureId;
                foreach (var issue in issues)
                {
                    if (issue.StoreId == woi.StoreId)
                    {
                        _issueService.UpdateIssueCost(newIssueItem);
                        issue.IssueItems.Add(newIssueItem);
                    }
                }
            }
            try
            {
                foreach (var issue in issues)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), issue);
                    issue.Number = number;
                    _issueRepository.Insert(issue);
                }

                this._dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { Errors = e.Message });
            }

            SuccessNotification(_localizationService.GetResource("Issue.CreatedForWO"));
            return new NullJsonResult();
        }

        #endregion

        #region WorkOrderTasks

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Read")]
        public ActionResult WorkOrderTaskList(long WorkOrderId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderTaskRepository.GetAll().Where(c => c.WorkOrderId == WorkOrderId);
            query = sort == null ? query.OrderBy(a => a.Sequence) : query.Sort(sort);
            var workOrderTasks = new PagedList<WorkOrderTask>(query, command.Page - 1, command.PageSize);
            var result = workOrderTasks.Select(x => x.ToModel()).ToList();

            foreach (var r in result)
            {
                r.ResultText = r.Result.ToString();
            }
            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = workOrderTasks.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Read,Maintenance.WorkOrder.Update")]
        public ActionResult WorkOrderTask(long id)
        {
            var workOrderTask = _workOrderTaskRepository.GetById(id);
            var model = workOrderTask.ToModel();
            var html = this.WorkOrderTaskPanel(model);
            return Json(new { Id = workOrderTask.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create")]
        public ActionResult CreateWorkOrderTask(long workOrderId)
        {
            var workOrder = _workOrderRepository.GetById(workOrderId);
            var maxSequence = workOrder.WorkOrderTasks.Max(p => p.Sequence) ?? 0;
            var workOrderTask = new WorkOrderTask
            {
                WorkOrderId = workOrderId,
                Sequence = maxSequence + 1,
                IsNew = true
            };
            _workOrderTaskRepository.InsertAndCommit(workOrderTask);

            var model = new WorkOrderTaskModel();
            model = workOrderTask.ToModel();
            var html = this.WorkOrderTaskPanel(model);

            return Json(new { Id = workOrderTask.Id, Html = html });
        }

        [NonAction]
        public string WorkOrderTaskPanel(WorkOrderTaskModel model)
        {
            var html = this.RenderPartialViewToString("_WorkOrderTaskDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create")]
        public ActionResult CreateWorkOrderTasks(long? workOrderId, long? taskGroupId)
        {
            var workOrder = _workOrderRepository.GetById(workOrderId);
            var workOrderTasks = workOrder.WorkOrderTasks;
            if (workOrderTasks.Count() > 0)
            {
                if (workOrderTasks.Any(m => m.Completed == true))
                {
                    ModelState.AddModelError("", _localizationService.GetResource("WorkOrder.CannotChangeTaskGroup"));
                }
            }
            if (ModelState.IsValid)
            {
                workOrderTasks.Clear();

                workOrder.TaskGroupId = taskGroupId;
                var tasks = _taskRepository.GetAll().Where(m => m.TaskGroupId == taskGroupId).ToList();
                foreach (var task in tasks)
                {
                    var workOrderTask = new WorkOrderTask
                    {
                        Sequence = task.Sequence,
                        Description = task.Description
                    };
                    workOrder.WorkOrderTasks.Add(workOrderTask);
                }
                _workOrderRepository.Update(workOrder);
                this._dbContext.SaveChanges();

                //copy attachments, need to copy after saving WorkOrderTasks
                //so we can have toEntityId
                for (int i = 0; i < tasks.Count; i++)
                {
                    _attachmentService.CopyAttachments(tasks[i].Id, EntityType.Task,
                        workOrder.WorkOrderTasks.ToList()[i].Id, EntityType.WorkOrderTask);
                }
                this._dbContext.SaveChanges();

                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult SaveWorkOrderTask(WorkOrderTaskModel model)
        {
            if (ModelState.IsValid)
            {
                var workOrderTask = _workOrderTaskRepository.GetById(model.Id);
                //always set IsNew to false when saving
                workOrderTask.IsNew = false;
                workOrderTask = model.ToEntity(workOrderTask);

                _workOrderTaskRepository.UpdateAndCommit(workOrderTask);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult CancelWorkOrderTask(long id)
        {
            var workOrderTask = _workOrderTaskRepository.GetById(id);
            if (workOrderTask.IsNew == true)
            {
                _workOrderTaskRepository.DeleteAndCommit(workOrderTask);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Delete")]
        public ActionResult DeleteWorkOrderTask(long? parentId, long id)
        {
            var workOrderTask = _workOrderTaskRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _workOrderTaskRepository.DeactivateAndCommit(workOrderTask);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Delete")]
        public ActionResult DeleteSelectedWorkOrderTasks(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var workOrderTask = _workOrderTaskRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _workOrderTaskRepository.Deactivate(workOrderTask);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region WorkOrderMiscCosts

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Read")]
        public ActionResult WorkOrderMiscCostList(long WorkOrderId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderMiscCostRepository.GetAll().Where(c => c.WorkOrderId == WorkOrderId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var workOrderMiscCosts = new PagedList<WorkOrderMiscCost>(query, command.Page - 1, command.PageSize);
            var result = workOrderMiscCosts.Select(x => x.ToModel()).ToList();

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = workOrderMiscCosts.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Read,Maintenance.WorkOrder.Update")]
        public ActionResult WorkOrderMiscCost(long id)
        {
            var workOrderMiscCost = _workOrderMiscCostRepository.GetById(id);
            var model = workOrderMiscCost.ToModel();
            var html = this.WorkOrderMiscCostPanel(model);
            return Json(new { Id = workOrderMiscCost.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create")]
        public ActionResult CreateWorkOrderMiscCost(long workOrderId)
        {
            var workOrderMiscCost = new WorkOrderMiscCost
            {
                IsNew = true
            };
            _workOrderMiscCostRepository.Insert(workOrderMiscCost);

            var workOrder = _workOrderRepository.GetById(workOrderId);
            workOrder.WorkOrderMiscCosts.Add(workOrderMiscCost);

            this._dbContext.SaveChanges();

            var model = new WorkOrderMiscCostModel();
            model = workOrderMiscCost.ToModel();
            var html = this.WorkOrderMiscCostPanel(model);

            return Json(new { Id = workOrderMiscCost.Id, Html = html });
        }

        [NonAction]
        public string WorkOrderMiscCostPanel(WorkOrderMiscCostModel model)
        {
            var html = this.RenderPartialViewToString("_WorkOrderMiscCostDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult SaveWorkOrderMiscCost(WorkOrderMiscCostModel model)
        {
            if (ModelState.IsValid)
            {
                var workOrderMiscCost = _workOrderMiscCostRepository.GetById(model.Id);
                //always set IsNew to false when saving
                workOrderMiscCost.IsNew = false;
                //update totals
                model.PlanTotal = (model.PlanQuantity ?? 0) * (model.PlanUnitPrice ?? 0);
                model.ActualTotal = (model.ActualQuantity ?? 0) * (model.ActualUnitPrice ?? 0);

                workOrderMiscCost = model.ToEntity(workOrderMiscCost);

                _workOrderMiscCostRepository.UpdateAndCommit(workOrderMiscCost);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult CancelWorkOrderMiscCost(long id)
        {
            var workOrderMiscCost = _workOrderMiscCostRepository.GetById(id);
            if (workOrderMiscCost.IsNew == true)
            {
                _workOrderMiscCostRepository.DeleteAndCommit(workOrderMiscCost);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Delete")]
        public ActionResult DeleteWorkOrderMiscCost(long? parentId, long id)
        {
            var workOrderMiscCost = _workOrderMiscCostRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _workOrderMiscCostRepository.DeactivateAndCommit(workOrderMiscCost);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Delete")]
        public ActionResult DeleteSelectedWorkOrderMiscCosts(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var workOrderMiscCost = _workOrderMiscCostRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _workOrderMiscCostRepository.Deactivate(workOrderMiscCost);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region WorkOrderServiceItems

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Read")]
        public ActionResult WorkOrderServiceItemList(long WorkOrderId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderServiceItemRepository.GetAll().Where(c => c.WorkOrderId == WorkOrderId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var workOrderServiceItems = new PagedList<WorkOrderServiceItem>(query, command.Page - 1, command.PageSize);
            var result = workOrderServiceItems.Select(x => x.ToModel()).ToList();

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = workOrderServiceItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Read,Maintenance.WorkOrder.Update")]
        public ActionResult WorkOrderServiceItem(long id)
        {
            var workOrderServiceItem = _workOrderServiceItemRepository.GetById(id);
            var model = workOrderServiceItem.ToModel();
            var html = this.WorkOrderServiceItemPanel(model);
            return Json(new { Id = workOrderServiceItem.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create")]
        public ActionResult CreateWorkOrderServiceItem(long workOrderId)
        {
            var workOrderServiceItem = new WorkOrderServiceItem
            {
                IsNew = true
            };
            _workOrderServiceItemRepository.Insert(workOrderServiceItem);

            var workOrder = _workOrderRepository.GetById(workOrderId);
            workOrder.WorkOrderServiceItems.Add(workOrderServiceItem);

            this._dbContext.SaveChanges();

            var model = new WorkOrderServiceItemModel();
            model = workOrderServiceItem.ToModel();
            var html = this.WorkOrderServiceItemPanel(model);

            return Json(new { Id = workOrderServiceItem.Id, Html = html });
        }

        [NonAction]
        public string WorkOrderServiceItemPanel(WorkOrderServiceItemModel model)
        {
            var html = this.RenderPartialViewToString("_WorkOrderServiceItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult SaveWorkOrderServiceItem(WorkOrderServiceItemModel model)
        {
            if (ModelState.IsValid)
            {
                var workOrderServiceItem = _workOrderServiceItemRepository.GetById(model.Id);
                //always set IsNew to false when saving
                workOrderServiceItem.IsNew = false;
                //update totals
                model.PlanTotal = (model.PlanQuantity ?? 0) * (model.PlanUnitPrice ?? 0);
                model.ActualTotal = (model.ActualQuantity ?? 0) * (model.ActualUnitPrice ?? 0);

                workOrderServiceItem = model.ToEntity(workOrderServiceItem);

                _workOrderServiceItemRepository.UpdateAndCommit(workOrderServiceItem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create,Maintenance.WorkOrder.Update")]
        public ActionResult CancelWorkOrderServiceItem(long id)
        {
            var workOrderServiceItem = _workOrderServiceItemRepository.GetById(id);
            if (workOrderServiceItem.IsNew == true)
            {
                _workOrderServiceItemRepository.DeleteAndCommit(workOrderServiceItem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Delete")]
        public ActionResult DeleteWorkOrderServiceItem(long? parentId, long id)
        {
            var workOrderServiceItem = _workOrderServiceItemRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _workOrderServiceItemRepository.DeactivateAndCommit(workOrderServiceItem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Delete")]
        public ActionResult DeleteSelectedWorkOrderServiceItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var workOrderServiceItem = _workOrderServiceItemRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _workOrderServiceItemRepository.Deactivate(workOrderServiceItem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Point

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Read")]
        public ActionResult PointMeterLineItemList(long? assetId, long? locationId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _pointMeterLineItemRepository.GetAll()
                .Where(c => (assetId != null && c.Point.AssetId == assetId)
                            || (locationId != null && c.Point.LocationId == locationId));
            query = sort == null ? query.OrderBy(a => a.Meter.Name) : query.Sort(sort);
            var pointMeterLineItems = new PagedList<PointMeterLineItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = pointMeterLineItems.Select(x => x.ToModel()),
                Total = pointMeterLineItems.TotalCount
            };

            return Json(gridModel);

        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Update")]
        public ActionResult SaveChanges([Bind(Prefix = "updated")]List<PointMeterLineItemModel> updatedItems,
           [Bind(Prefix = "created")]List<PointMeterLineItemModel> createdItems,
           [Bind(Prefix = "deleted")]List<PointMeterLineItemModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                //Update PointMeterLineItem
                if (updatedItems != null)
                {
                    foreach (var model in updatedItems)
                    {
                        var pointMeterLineItem = _pointMeterLineItemRepository.GetById(model.Id);
                        if (model.ReadingValue.HasValue)
                        {
                            var newReading = new Reading();
                            newReading.ReadingValue = model.ReadingValue;
                            newReading.DateOfReading = model.DateOfReading;
                            newReading.ReadingSource = (int?)ReadingSource.WorkOrder;
                            newReading.WorkOrderId = model.WorkOrderId;
                            pointMeterLineItem.LastDateOfReading = model.DateOfReading;
                            pointMeterLineItem.LastReadingValue = model.ReadingValue;
                            pointMeterLineItem.Readings.Add(newReading);
                            //Check and create a new meter event history if the reading value does not in the range of the list of meter events
                            _meterService.CreateMeterEventHistory(pointMeterLineItem, newReading);
                            _pointMeterLineItemRepository.Update(pointMeterLineItem);
                        }
                    }
                }

                _dbContext.SaveChanges();
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return new NullJsonResult();

            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Child Work Orders

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Read")]
        public ActionResult ChildWorkOrderList(long? workOrderId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderRepository.GetAll().Where(c => c.ParentId == workOrderId);
            query = sort == null ? query.OrderBy(a => a.Number) : query.Sort(sort);
            var workOrders = new PagedList<WorkOrder>(query, command.Page - 1, command.PageSize);
            var result = workOrders.Select(x => x.ToModel()).ToList();
            foreach (var item in result)
            {
                item.PriorityText = item.Priority.ToString();
            }

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = workOrders.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.WorkOrder.Create")]
        public ActionResult CreateChildWO(long? workOrderId)
        {
            var workOrder = _workOrderRepository.GetById(workOrderId);
            var childWO = new WorkOrder();

            childWO.ParentId = workOrder.Id;
            childWO.CreatedUserId = this._workContext.CurrentUser.Id;
            childWO.Description = workOrder.Description;
            childWO.SiteId = workOrder.SiteId;
            childWO.AssetId = workOrder.AssetId;
            childWO.LocationId = workOrder.LocationId;
            childWO.PreventiveMaintenanceId = workOrder.PreventiveMaintenanceId;
            childWO.ServiceRequestId = workOrder.ServiceRequestId;

            childWO.Priority = workOrder.Priority;
            childWO.WorkCategoryId = workOrder.WorkCategoryId;
            childWO.WorkTypeId = workOrder.WorkTypeId;
            childWO.FailureGroupId = workOrder.FailureGroupId;

            childWO.RequestorName = workOrder.RequestorName;
            childWO.RequestorPhone = workOrder.RequestorPhone;
            childWO.RequestorEmail = workOrder.RequestorEmail;
            childWO.RequestedDateTime = workOrder.RequestedDateTime;

            string number = _autoNumberService.GenerateNextAutoNumber(DateTime.Now, childWO);
            childWO.Number = number;

            try
            {
                _workOrderRepository.InsertAndCommit(childWO);
                //start workflow
                var workflowInstanceId = WorkflowServiceClient.StartWorkflow(childWO.Id, EntityType.WorkOrder, 0, this._workContext.CurrentUser.Id);

                this._dbContext.Detach(childWO);
                childWO = _workOrderRepository.GetById(childWO.Id);
                this._dbContext.Detach(childWO.Assignment);
                var assignment = _assignmentRepository.GetById(childWO.AssignmentId);

                // trigger action
                WorkflowServiceClient.TriggerWorkflowAction(childWO.Id, EntityType.WorkOrder, assignment.WorkflowDefinitionId, assignment.WorkflowInstanceId,
                        assignment.WorkflowVersion.Value, WorkflowActionName.Submit, "WorkOrder", this._workContext.CurrentUser.Id);

                return Json(new { Id = childWO.Id });
            }
            catch (Exception e)
            {
                return Json(new { Errors = e.Message });
            }
        }

        #endregion
    }
}