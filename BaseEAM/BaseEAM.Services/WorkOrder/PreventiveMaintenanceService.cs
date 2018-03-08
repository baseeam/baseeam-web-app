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
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseEAM.Services
{
    public class PreventiveMaintenanceService : BaseService, IPreventiveMaintenanceService
    {
        #region Fields

        private readonly IRepository<PreventiveMaintenance> _preventiveMaintenanceRepository;
        private readonly IRepository<ValueItem> _valueItemRepository;
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IRepository<WorkOrderTask> _workOrderTaskRepository;
        private readonly IRepository<Assignment> _assignmentRepository;
        private readonly IRepository<Asset> _assetRepository;
        private readonly IRepository<TaskGroup> _taskGroupRepository;
        private readonly ICalendarService _calendarService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IAttachmentService _attachmentService;
        private readonly DapperContext _dapperContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public PreventiveMaintenanceService(IRepository<PreventiveMaintenance> preventiveMaintenanceRepository,
            IRepository<ValueItem> valueItemRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<WorkOrderTask> workOrderTaskRepository,
            IRepository<Assignment> assignmentRepository,
            IRepository<Asset> assetRepository,
            IRepository<TaskGroup> taskGroupRepository,
            ICalendarService calendarService,
            IAutoNumberService autoNumberService,
            IAttachmentService attachmentService,
            IWorkContext workContext,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._preventiveMaintenanceRepository = preventiveMaintenanceRepository;
            this._valueItemRepository = valueItemRepository;
            this._workOrderRepository = workOrderRepository;
            this._workOrderTaskRepository = workOrderTaskRepository;
            this._assignmentRepository = assignmentRepository;
            this._assetRepository = assetRepository;
            this._taskGroupRepository = taskGroupRepository;
            this._calendarService = calendarService;
            this._autoNumberService = autoNumberService;
            this._attachmentService = attachmentService;
            this._workContext = workContext;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<PreventiveMaintenance> GetPreventiveMaintenances(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.PreventiveMaintenanceSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("PreventiveMaintenance.Number");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.PreventiveMaintenanceSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var preventiveMaintenances = connection.Query<PreventiveMaintenance, ValueItem, Asset, Location, Site, PreventiveMaintenance>(search.RawSql,
                    (preventiveMaintenance, valueItem, asset, location, site) => { preventiveMaintenance.Site = site; preventiveMaintenance.PMStatus = valueItem; preventiveMaintenance.Asset = asset; preventiveMaintenance.Location = location; return preventiveMaintenance; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<PreventiveMaintenance>(preventiveMaintenances, totalCount);
            }
        }

        public virtual WorkOrder CreateNextWorkOrder(PreventiveMaintenance pm, 
            DateTime startDateTime,
            DateTime endDateTime)
        {
            DateTime prevDateTime = DateTime.MinValue;

            // if there's already an active WO for this PM
            // then return
            WorkOrder existingWork = _workOrderRepository.GetAll().
                   Where(w => w.PreventiveMaintenanceId == pm.Id 
                            && w.Assignment.Name != WorkflowStatus.Closed
                            && w.Assignment.Name != WorkflowStatus.Cancelled).FirstOrDefault();

            if (existingWork != null)
            {
                return null;
            }
            // copy work parameters
            WorkOrder workOrder = new WorkOrder();
            workOrder.CreatedUserId = this._workContext.CurrentUser.Id;
            workOrder.RequestedDateTime = DateTime.UtcNow;
            workOrder.ExpectedStartDateTime = startDateTime;
            workOrder.DueDateTime = endDateTime;
            workOrder.LocationId = pm.LocationId;
            workOrder.AssetId = pm.AssetId;
            workOrder.SiteId = pm.SiteId;
            workOrder.WorkType = pm.WorkType;
            workOrder.WorkCategoryId = pm.WorkCategoryId;
            workOrder.FailureGroupId = pm.FailureGroupId;
            workOrder.Description = workOrder.Name = pm.Description;
            workOrder.PreventiveMaintenanceId = pm.Id;
            workOrder.Priority = pm.Priority;
            workOrder.ContractId = pm.ContractId;
            workOrder.IsNew = false;
            foreach(var pmLabor in pm.PMLabors)
            {
                var workOrderLabor = new WorkOrderLabor();
                workOrderLabor.TeamId = pmLabor.TeamId;
                workOrderLabor.CraftId = pmLabor.CraftId;
                workOrderLabor.TechnicianId = pmLabor.TechnicianId;
                workOrderLabor.PlanHours = pmLabor.PlanHours;
                workOrderLabor.StandardRate = pmLabor.StandardRate;
                workOrderLabor.OTRate = pmLabor.OTRate;
                workOrderLabor.PlanTotal = pmLabor.PlanTotal;
                workOrder.WorkOrderLabors.Add(workOrderLabor);
            }
            foreach (var pmTask in pm.PMTasks)
            {
                var workOrderTask = new WorkOrderTask();
                workOrder.TaskGroupId = pm.TaskGroupId;
                workOrderTask.Sequence = pmTask.Sequence;
                workOrderTask.Description = pmTask.Description;
                workOrderTask.AssignedUserId = pmTask.AssignedUserId;
                workOrder.WorkOrderTasks.Add(workOrderTask);
            }

            foreach (var pmItem in pm.PMItems)
            {
                var workOrderItem = new WorkOrderItem();
                workOrderItem.StoreId = pmItem.StoreId;
                workOrderItem.ItemId = pmItem.ItemId;
                workOrderItem.UnitPrice = pmItem.UnitPrice;
                workOrderItem.PlanQuantity = pmItem.PlanQuantity;
                workOrderItem.PlanTotal = pmItem.PlanTotal;
                workOrderItem.PlanToolHours = pmItem.PlanToolHours;
                workOrderItem.ToolRate = pmItem.ToolRate;
                workOrder.WorkOrderItems.Add(workOrderItem);
            }

            foreach (var pmServiceItem in pm.PMServiceItems)
            {
                var workOrderServiceItem = new WorkOrderServiceItem();
                workOrderServiceItem.ServiceItemId = pmServiceItem.ServiceItemId;
                workOrderServiceItem.Description = pmServiceItem.Description;
                workOrderServiceItem.PlanUnitPrice = pmServiceItem.PlanUnitPrice;
                workOrderServiceItem.PlanQuantity = pmServiceItem.PlanQuantity;
                workOrderServiceItem.PlanTotal = pmServiceItem.PlanTotal;
                workOrder.WorkOrderServiceItems.Add(workOrderServiceItem);
            }

            foreach (var pmMiscCost in pm.PMMiscCosts)
            {
                var workOrderMiscCost = new WorkOrderMiscCost();
                workOrderMiscCost.Sequence = pmMiscCost.Sequence;
                workOrderMiscCost.Description = pmMiscCost.Description;
                workOrderMiscCost.PlanQuantity = pmMiscCost.PlanQuantity;
                workOrderMiscCost.PlanUnitPrice = pmMiscCost.PlanUnitPrice;
                workOrderMiscCost.PlanTotal = pmMiscCost.PlanTotal;
                workOrder.WorkOrderMiscCosts.Add(workOrderMiscCost);
            }

            if (!pm.FirstWorkExpectedStartDateTime.HasValue || workOrder.ExpectedStartDateTime.Value <= pm.EndDateTime.Value)
            {
                string number = _autoNumberService.GenerateNextAutoNumber(DateTime.Now, workOrder);
                workOrder.Number = number;
                _workOrderRepository.InsertAndCommit(workOrder);

                // copy attachments
                CopyAttachments(workOrder.PreventiveMaintenance.PMTasks.ToList(), workOrder.WorkOrderTasks.ToList());
                this._dbContext.SaveChanges();

                // start WO workflow
                var workflowInstanceId = WorkflowServiceClient.StartWorkflow(workOrder.Id, EntityType.WorkOrder, 0, this._workContext.CurrentUser.Id);
                this._dbContext.Detach(workOrder);
                workOrder = _workOrderRepository.GetById(workOrder.Id);
                this._dbContext.Detach(workOrder.Assignment);
                var assignment = _assignmentRepository.GetById(workOrder.AssignmentId);

                // trigger action
                WorkflowServiceClient.TriggerWorkflowAction(workOrder.Id, EntityType.WorkOrder, assignment.WorkflowDefinitionId, assignment.WorkflowInstanceId,
                        assignment.WorkflowVersion.Value, WorkflowActionName.Submit, "PreventiveMaintenance", this._workContext.CurrentUser.Id);
            }
            return workOrder;
        }

        public virtual void CopyAttachments(List<PMTask> from, List<WorkOrderTask> to)
        {
            for (int i = 0; i < from.Count; i++)
            {
                _attachmentService.CopyAttachments(from[i].Id, EntityType.PMTask,
                        to[i].Id, EntityType.WorkOrderTask);
            }
        }

        public void ClosePM(PreventiveMaintenance pm)
        {
            var closedStatus = _valueItemRepository.GetAll().Where(c => c.ValueItemCategory.Name == "PM Status" && c.Name == "Closed").FirstOrDefault();
            pm.PMStatusId = closedStatus.Id;
            _preventiveMaintenanceRepository.UpdateAndCommit(pm);
        }

        public virtual void GeneratePMTasks(PreventiveMaintenance pm, long? assetId)
        {
            if (assetId == null)
                return;
            if (pm.IsNew == true)
            {
                var assetType = _assetRepository.GetById(assetId).AssetType.Name;
                var taskGroup = _taskGroupRepository.GetAll()
                    .Where(t => t.AssetTypes.Contains(assetType))
                    .FirstOrDefault();
                if (taskGroup != null)
                {
                    pm.TaskGroupId = taskGroup.Id;
                    var tasks = taskGroup.Tasks.ToList();
                    foreach (var task in tasks)
                    {
                        var pmTask = new PMTask
                        {
                            Sequence = task.Sequence,
                            Description = task.Description
                        };
                        pm.PMTasks.Add(pmTask);
                    }
                    _preventiveMaintenanceRepository.Update(pm);
                    this._dbContext.SaveChanges();

                    //copy attachments, need to copy after saving PMTasks
                    //so we can have toEntityId
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        _attachmentService.CopyAttachments(tasks[i].Id, EntityType.Task,
                            pm.PMTasks.ToList()[i].Id, EntityType.PMTask);
                    }
                }
            }
        }

        #endregion
    }
}
