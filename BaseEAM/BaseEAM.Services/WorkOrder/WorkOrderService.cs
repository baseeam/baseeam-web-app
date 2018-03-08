/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BaseEAM.Services
{
    public class WorkOrderService : BaseService, IWorkOrderService
    {
        #region Fields

        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Assignment> _assignmentRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IRepository<Asset> _assetRepository;
        private readonly IRepository<TaskGroup> _taskGroupRepository;
        private readonly IServiceRequestService _serviceRequestService;
        private readonly IPreventiveMaintenanceService _pmService;
        private readonly ICalendarService _calendarService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IAttachmentService _attachmentService;
        private readonly DapperContext _dapperContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public WorkOrderService(IRepository<WorkOrder> workOrderRepository,
            IRepository<User> userRepository,
            IRepository<Assignment> assignmentRepository,
            IRepository<Attachment> attachmentRepository,
            IRepository<Asset> assetRepository,
            IRepository<TaskGroup> taskGroupRepository,
            IServiceRequestService serviceRequestService,
            IPreventiveMaintenanceService pmService,
            ICalendarService calendarService,
            IAutoNumberService autoNumberService,
            IAttachmentService attachmentService,
            DapperContext dapperContext,
            IWorkContext workContext,
            IDbContext dbContext,
            IDateTimeHelper dateTimeHelper)
        {
            this._workOrderRepository = workOrderRepository;
            this._userRepository = userRepository;
            this._assignmentRepository = assignmentRepository;
            this._attachmentRepository = attachmentRepository;
            this._assetRepository = assetRepository;
            this._taskGroupRepository = taskGroupRepository;
            this._serviceRequestService = serviceRequestService;
            this._pmService = pmService;
            this._calendarService = calendarService;
            this._autoNumberService = autoNumberService;
            this._attachmentService = attachmentService;
            this._dapperContext = dapperContext;
            this._workContext = workContext;
            this._dbContext = dbContext;
            this._dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region Methods

        public virtual PagedResult<WorkOrder> GetWorkOrders(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.WorkOrderSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("WorkOrder.Number");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.WorkOrderSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var workOrders = connection.Query<WorkOrder, Asset, Location, Assignment, Site, WorkOrder>(search.RawSql,
                    (workOrder, asset, location, assignment, site) => { workOrder.Site = site; workOrder.Asset = asset; workOrder.Location = location; workOrder.Assignment = assignment; workOrder.Site = site; return workOrder; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<WorkOrder>(workOrders, totalCount);
            }
        }

        public virtual List<User> GetCreatedUser(long id)
        {
            var result = new List<User>();
            var workOrder = _workOrderRepository.GetById(id);
            var createdUser = _userRepository.GetById(workOrder.CreatedUserId);
            result.Add(createdUser);
            return result;
        }

        public virtual List<User> GetAssignedTechnicians(long id)
        {
            var workOrder = _workOrderRepository.GetById(id);
            var users = workOrder.WorkOrderLabors.Select(w => w.Technician.User).ToList();
            return users;
        }

        public virtual void CloseWorkOrder(long entityId)
        {            
            var workOrder = _workOrderRepository.GetById(entityId);
            WorkOrder newWorkOrder = CreateNextWorkOrderForPM(workOrder);

            _serviceRequestService.AutoCloseServiceRequest(workOrder.ServiceRequestId);
        }

        public virtual WorkOrder CreateNextWorkOrderForPM(WorkOrder workOrder)
        {
            WorkOrder newWork = null;
            var pm = workOrder.PreventiveMaintenance;

            // only time-based
            if (pm != null && pm.FirstWorkExpectedStartDateTime.HasValue)
            {
                DateTime startDateTime = workOrder.ExpectedStartDateTime.Value;
                DateTime endDateTime = workOrder.DueDateTime.Value;

                var isValidDate = _calendarService.DetermineNextDate(workOrder.PreventiveMaintenance, ref startDateTime, ref endDateTime);

                if (isValidDate && startDateTime <= workOrder.PreventiveMaintenance.EndDateTime.Value)
                {
                    // create a new work order
                    newWork = new WorkOrder();
                    newWork.CreatedUserId = this._workContext.CurrentUser.Id;
                    newWork.RequestedDateTime = DateTime.UtcNow;
                    newWork.ExpectedStartDateTime = startDateTime;
                    newWork.DueDateTime = endDateTime;
                    newWork.LocationId = workOrder.PreventiveMaintenance.LocationId;
                    newWork.AssetId = workOrder.PreventiveMaintenance.AssetId;
                    newWork.SiteId = workOrder.SiteId;
                    newWork.WorkType = workOrder.PreventiveMaintenance.WorkType;
                    newWork.WorkCategoryId = workOrder.PreventiveMaintenance.WorkCategoryId;
                    newWork.FailureGroupId = workOrder.PreventiveMaintenance.FailureGroupId;
                    newWork.Description = newWork.Name = workOrder.PreventiveMaintenance.Description;
                    newWork.Priority = workOrder.PreventiveMaintenance.Priority;
                    newWork.PreventiveMaintenanceId = workOrder.PreventiveMaintenanceId;
                    newWork.ContractId = pm.ContractId;
                    newWork.IsNew = false;

                    foreach (var pmLabor in workOrder.PreventiveMaintenance.PMLabors)
                    {
                        var workOrderLabor = new WorkOrderLabor();
                        workOrderLabor.TeamId = pmLabor.TeamId;
                        workOrderLabor.CraftId = pmLabor.CraftId;
                        workOrderLabor.TechnicianId = pmLabor.TechnicianId;
                        workOrderLabor.PlanHours = pmLabor.PlanHours;
                        workOrderLabor.StandardRate = pmLabor.StandardRate;
                        workOrderLabor.OTRate = pmLabor.OTRate;
                        workOrderLabor.PlanTotal = pmLabor.PlanTotal;
                        newWork.WorkOrderLabors.Add(workOrderLabor);
                    }

                    foreach (var pmTask in workOrder.PreventiveMaintenance.PMTasks)
                    {
                        var workOrderTask = new WorkOrderTask();
                        workOrder.TaskGroupId = workOrder.PreventiveMaintenance.TaskGroupId;
                        workOrderTask.Sequence = pmTask.Sequence;
                        workOrderTask.Description = pmTask.Description;
                        workOrderTask.AssignedUserId = pmTask.AssignedUserId;
                        newWork.WorkOrderTasks.Add(workOrderTask);
                    }

                    foreach (var pmItem in workOrder.PreventiveMaintenance.PMItems)
                    {
                        var workOrderItem = new WorkOrderItem();
                        workOrderItem.StoreId = pmItem.StoreId;
                        workOrderItem.ItemId = pmItem.ItemId;
                        workOrderItem.UnitPrice = pmItem.UnitPrice;
                        workOrderItem.PlanQuantity = pmItem.PlanQuantity;
                        workOrderItem.PlanTotal = pmItem.PlanTotal;
                        workOrderItem.PlanToolHours = pmItem.PlanToolHours;
                        workOrderItem.ToolRate = pmItem.ToolRate;
                        newWork.WorkOrderItems.Add(workOrderItem);
                    }

                    foreach (var pmServiceItem in workOrder.PreventiveMaintenance.PMServiceItems)
                    {
                        var workOrderServiceItem = new WorkOrderServiceItem();
                        workOrderServiceItem.ServiceItemId = pmServiceItem.ServiceItemId;
                        workOrderServiceItem.Description = pmServiceItem.Description;
                        workOrderServiceItem.PlanUnitPrice = pmServiceItem.PlanUnitPrice;
                        workOrderServiceItem.PlanQuantity = pmServiceItem.PlanQuantity;
                        workOrderServiceItem.PlanTotal = pmServiceItem.PlanTotal;
                        newWork.WorkOrderServiceItems.Add(workOrderServiceItem);
                    }

                    foreach (var pmMiscCost in workOrder.PreventiveMaintenance.PMMiscCosts)
                    {
                        var workOrderMiscCost = new WorkOrderMiscCost();
                        workOrderMiscCost.Sequence = pmMiscCost.Sequence;
                        workOrderMiscCost.Description = pmMiscCost.Description;
                        workOrderMiscCost.PlanQuantity = pmMiscCost.PlanQuantity;
                        workOrderMiscCost.PlanUnitPrice = pmMiscCost.PlanUnitPrice;
                        workOrderMiscCost.PlanTotal = pmMiscCost.PlanTotal;
                        newWork.WorkOrderMiscCosts.Add(workOrderMiscCost);
                    }

                    string number = _autoNumberService.GenerateNextAutoNumber(DateTime.Now, newWork);
                    newWork.Number = number;
                    _workOrderRepository.InsertAndCommit(newWork);

                    // copy attachments
                    _pmService.CopyAttachments(workOrder.PreventiveMaintenance.PMTasks.ToList(), newWork.WorkOrderTasks.ToList());
                    this._dbContext.SaveChanges();

                    // Start workflow
                    var workflowInstanceId = WorkflowServiceClient.StartWorkflow(newWork.Id, EntityType.WorkOrder, 0, this._workContext.CurrentUser.Id);
                    //Nguyen Le: reload work because there's data changes from workflow, need to detach first because of EF caching
                    this._dbContext.Detach(newWork);
                    newWork = _workOrderRepository.GetById(newWork.Id);

                    this._dbContext.Detach(newWork.Assignment);
                    var assignment = _assignmentRepository.GetById(newWork.AssignmentId);
                    WorkflowServiceClient.TriggerWorkflowAction(newWork.Id, EntityType.WorkOrder, assignment.WorkflowDefinitionId, assignment.WorkflowInstanceId,
                            assignment.WorkflowVersion.Value, WorkflowActionName.Submit, "WorkOrder", this._workContext.CurrentUser.Id);
                }
                else
                {
                    _pmService.ClosePM(pm);
                }
            }
            return newWork;
        }

        public virtual void GenerateWorkOrderTasks(WorkOrder workOrder, long? assetId)
        {
            if (assetId == null)
                return;
            if(workOrder.IsNew == true)
            {
                var assetType = _assetRepository.GetById(assetId).AssetType.Name;
                var taskGroup = _taskGroupRepository.GetAll()
                    .Where(t => t.AssetTypes.Contains(assetType))
                    .FirstOrDefault();
                if(taskGroup != null)
                {
                    workOrder.TaskGroupId = taskGroup.Id;
                    foreach (var task in taskGroup.Tasks)
                    {
                        var workOrderTask = new WorkOrderTask
                        {
                            Sequence = task.Sequence,
                            Description = task.Description
                        };
                        workOrder.WorkOrderTasks.Add(workOrderTask);
                    }
                }
            }
        }

        #endregion
    }
}
