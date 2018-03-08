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
    public class ServiceRequestController : BaseController
    {
        #region Fields

        private readonly IRepository<ServiceRequest> _serviceRequestRepository;
        private readonly IRepository<Assignment> _assignmentRepository;
        private readonly IRepository<AssignmentHistory> _assignmentHistoryRepository;
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IServiceRequestService _serviceRequestService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public ServiceRequestController(IRepository<ServiceRequest> serviceRequestRepository,
            IRepository<Assignment> assignmentRepository,
            IRepository<AssignmentHistory> assignmentHistoryRepository,
            IRepository<WorkOrder> workOrderRepository,
            IServiceRequestService serviceRequestService,
            IAutoNumberService autoNumberService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._serviceRequestRepository = serviceRequestRepository;
            this._assignmentRepository = assignmentRepository;
            this._assignmentHistoryRepository = assignmentHistoryRepository;
            this._workOrderRepository = workOrderRepository;
            this._localizationService = localizationService;
            this._serviceRequestService = serviceRequestService;
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
            var numberFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Number",
                ResourceKey = "ServiceRequest.Number",
                DbColumn = "ServiceRequest.Number, ServiceRequest.Description",
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
                DbColumn = "ServiceRequest.Priority",
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
                ResourceKey = "ServiceRequest.Status",
                DbColumn = "Assignment.Name",
                Value = null,
                ControlType = FieldControlType.MultiSelectList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Open,Review,Execution,Closed,Cancelled",
                CsvValueList = "'Open','Review','Execution','Closed','Cancelled'",
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

            var requestorTypeFilter = new FieldModel
            {
                DisplayOrder = 7,
                Name = "RequestorType",
                ResourceKey = "RequestorType",
                DbColumn = "ServiceRequest.RequestorType",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int32,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "User,Anonymous",
                CsvValueList = "0,1",
                IsRequiredField = false
            };
            model.Filters.Add(requestorTypeFilter);

            return model;
        }

        #endregion

        #region ServiceRequests

        [BaseEamAuthorize(PermissionNames = "Maintenance.ServiceRequest.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ServiceRequestSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ServiceRequestSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.ServiceRequest.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ServiceRequestSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.ServiceRequestSearchModel] = model;

                PagedResult<ServiceRequest> data = _serviceRequestService.GetServiceRequests(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var item in result)
                {
                    item.PriorityText = item.Priority.ToString();
                    item.RequestorTypeText = item.RequestorType.ToString();
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
        [BaseEamAuthorize(PermissionNames = "Maintenance.ServiceRequest.Create")]
        public ActionResult Create()
        {
            var serviceRequest = new ServiceRequest
            {
                IsNew = true,
                Priority = (int?)AssignmentPriority.Medium,
                RequestorName = _workContext.CurrentUser.Name,
                RequestorEmail = _workContext.CurrentUser.Email,
                RequestorPhone = _workContext.CurrentUser.Phone,
                RequestedDateTime = DateTime.UtcNow,
                CreatedUserId = this._workContext.CurrentUser.Id
            };
            _serviceRequestRepository.InsertAndCommit(serviceRequest);

            //start workflow
            var workflowInstanceId = WorkflowServiceClient.StartWorkflow(serviceRequest.Id, EntityType.ServiceRequest, 0, this._workContext.CurrentUser.Id);
            return Json(new { Id = serviceRequest.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.ServiceRequest.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            var serviceRequest = _serviceRequestRepository.GetById(id);
            var assignment = serviceRequest.Assignment;
            var assignmentHistories = _assignmentHistoryRepository.GetAll()
                .Where(a => a.EntityId == serviceRequest.Id && a.EntityType == EntityType.ServiceRequest)
                .ToList();

            _serviceRequestRepository.Delete(serviceRequest);
            _assignmentRepository.Delete(assignment);
            foreach (var history in assignmentHistories)
                _assignmentHistoryRepository.Delete(history);

            this._dbContext.SaveChanges();

            //cancel wf
            WorkflowServiceClient.CancelWorkflow(
                serviceRequest.Id, EntityType.ServiceRequest, assignment.WorkflowDefinitionId, assignment.WorkflowInstanceId,
                assignment.WorkflowVersion.Value, this._workContext.CurrentUser.Id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Maintenance.ServiceRequest.Create,Maintenance.ServiceRequest.Read,Maintenance.ServiceRequest.Update")]
        public ActionResult Edit(long id)
        {
            var serviceRequest = _serviceRequestRepository.GetById(id);
            var model = serviceRequest.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.ServiceRequest.Create,Maintenance.ServiceRequest.Update")]
        public ActionResult Edit(ServiceRequestModel model)
        {
            var serviceRequest = _serviceRequestRepository.GetById(model.Id);
            var assignment = serviceRequest.Assignment;
            if (ModelState.IsValid)
            {
                serviceRequest = model.ToEntity(serviceRequest);

                if (serviceRequest.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), serviceRequest);
                    serviceRequest.Number = number;
                }
                //always set IsNew to false when saving
                serviceRequest.IsNew = false;
                //copy to Assignment
                if (serviceRequest.Assignment != null)
                {
                    serviceRequest.Assignment.Number = serviceRequest.Number;
                    serviceRequest.Assignment.Description = serviceRequest.Description;
                    serviceRequest.Assignment.Priority = serviceRequest.Priority;
                }
                _serviceRequestRepository.Update(serviceRequest);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //trigger workflow action
                if (!string.IsNullOrEmpty(model.ActionName))
                {
                    WorkflowServiceClient.TriggerWorkflowAction(serviceRequest.Id, EntityType.ServiceRequest, assignment.WorkflowDefinitionId, assignment.WorkflowInstanceId,
                        assignment.WorkflowVersion.Value, model.ActionName, model.Comment, this._workContext.CurrentUser.Id);
                    //Every time we query twice, because EF is caching entities so it won't get the latest value from DB
                    //We need to detach the specified entity and load it again
                    this._dbContext.Detach(serviceRequest.Assignment);
                    assignment = _assignmentRepository.GetById(serviceRequest.AssignmentId);
                }

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new
                {
                    number = serviceRequest.Number,
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
        [BaseEamAuthorize(PermissionNames = "Maintenance.ServiceRequest.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var serviceRequest = _serviceRequestRepository.GetById(id);

            if (!_serviceRequestService.IsDeactivable(serviceRequest))
            {
                ModelState.AddModelError("ServiceRequest", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                var assignment = serviceRequest.Assignment;
                var assignmentHistories = _assignmentHistoryRepository.GetAll()
                    .Where(a => a.EntityId == serviceRequest.Id && a.EntityType == EntityType.ServiceRequest)
                    .ToList();

                _serviceRequestRepository.Deactivate(serviceRequest);
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
        [BaseEamAuthorize(PermissionNames = "Maintenance.ServiceRequest.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var serviceRequests = new List<ServiceRequest>();
            foreach (long id in selectedIds)
            {
                var serviceRequest = _serviceRequestRepository.GetById(id);
                if (serviceRequest != null)
                {
                    if (!_serviceRequestService.IsDeactivable(serviceRequest))
                    {
                        ModelState.AddModelError("ServiceRequest", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        serviceRequests.Add(serviceRequest);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var serviceRequest in serviceRequests)
                {
                    var assignment = serviceRequest.Assignment;
                    var assignmentHistories = _assignmentHistoryRepository.GetAll()
                        .Where(a => a.EntityId == serviceRequest.Id && a.EntityType == EntityType.ServiceRequest)
                        .ToList();

                    _serviceRequestRepository.Deactivate(serviceRequest);
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

        #region Work Orders

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.ServiceRequest.Read")]
        public ActionResult WorkOrderList(long? serviceRequestId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderRepository.GetAll().Where(c => c.ServiceRequestId == serviceRequestId);
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
        [BaseEamAuthorize(PermissionNames = "Maintenance.ServiceRequest.Create,Maintenance.ServiceRequest.Update")]
        public ActionResult CreateWorkOrder(long serviceRequestId)
        {
            var serviceRequest = _serviceRequestRepository.GetById(serviceRequestId);

            var workOrder = new WorkOrder
            {
                IsNew = true,
                ServiceRequestId = serviceRequest.Id,
                SiteId = serviceRequest.SiteId,
                AssetId = serviceRequest.AssetId,
                LocationId = serviceRequest.LocationId,
                RequestorName = serviceRequest.RequestorName,
                RequestorEmail = serviceRequest.RequestorEmail,
                RequestorPhone = serviceRequest.RequestorPhone,
                RequestedDateTime = serviceRequest.RequestedDateTime,
                CreatedUserId = this._workContext.CurrentUser.Id
            };
            _workOrderRepository.InsertAndCommit(workOrder);

            //start workflow
            var workflowInstanceId = WorkflowServiceClient.StartWorkflow(workOrder.Id, EntityType.WorkOrder, 0, this._workContext.CurrentUser.Id);

            return Json(new { workOrderId = workOrder.Id });
        }

        #endregion

        #region Public SR



        #endregion
    }
}