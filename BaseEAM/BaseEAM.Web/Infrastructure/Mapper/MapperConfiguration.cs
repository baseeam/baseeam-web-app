/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using AutoMapper;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Infrastructure.Mapper;
using BaseEAM.Web.Models;
using System;
using System.Linq;

namespace BaseEAM.Web.Infrastructure.Mapper
{
    public class MapperConfiguration : IMapperConfiguration
    {
        /// <summary>
        /// Get configuration
        /// </summary>
        /// <returns>Mapper configuration action</returns>
        public Action<IMapperConfigurationExpression> GetConfiguration()
        {
            Action<IMapperConfigurationExpression> action = cfg =>
            {
                //ContractPriceItem
                cfg.CreateMap<ContractPriceItem, ContractPriceItemModel>();
                cfg.CreateMap<ContractPriceItemModel, ContractPriceItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //ContractTerm
                cfg.CreateMap<ContractTerm, ContractTermModel>();
                cfg.CreateMap<ContractTermModel, ContractTerm>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // Contract
                cfg.CreateMap<Contract, ContractModel>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Assignment == null ? "" : src.Assignment.Name))
                    .ForMember(dest => dest.AvailableActions, opt => opt.MapFrom(src => src.Assignment == null ? null : src.Assignment.AvailableActions))
                    .ForMember(dest => dest.AssignedUsers, opt => opt.MapFrom(src => src.Assignment == null ? null :
                        string.Join(";", src.Assignment.Users.Select(u => u.Name))));
                cfg.CreateMap<ContractModel, Contract>()
                    .ForMember(dest => dest.Number, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Message
                cfg.CreateMap<Message, MessageModel>();
                cfg.CreateMap<MessageModel, Message>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //MeterEventHistory
                cfg.CreateMap<MeterEventHistory, MeterEventHistoryModel>();
                cfg.CreateMap<MeterEventHistoryModel, MeterEventHistory>()
                    .ForMember(dest => dest.GeneratedReading, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //MeterEvent
                cfg.CreateMap<MeterEvent, MeterEventModel>();
                cfg.CreateMap<MeterEventModel, MeterEvent>()
                    .ForMember(dest => dest.DisplayOrder, src => src.Ignore())
                    .ForMember(dest => dest.MeterId, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // PMMeterFrequency
                cfg.CreateMap<PMMeterFrequency, PMMeterFrequencyModel>();
                cfg.CreateMap<PMMeterFrequencyModel, PMMeterFrequency>()
                    .ForMember(dest => dest.GeneratedReading, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());


                // AuditEntityConfiguration
                cfg.CreateMap<AuditEntityConfiguration, AuditEntityConfigurationModel>();
                cfg.CreateMap<AuditEntityConfigurationModel, AuditEntityConfiguration>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // ImportProfile
                cfg.CreateMap<ImportProfile, ImportProfileModel>();
                cfg.CreateMap<ImportProfileModel, ImportProfile>()
                    .ForMember(dest => dest.LastRunStartDateTime, src => src.Ignore())
                    .ForMember(dest => dest.LastRunEndDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ImportFileName, src => src.Ignore())
                    .ForMember(dest => dest.FileTypeId, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // Assignment
                cfg.CreateMap<Assignment, AssignmentModel>();
                cfg.CreateMap<AssignmentModel, Assignment>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // ServiceRequest
                cfg.CreateMap<ServiceRequest, ServiceRequestModel>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Assignment == null ? "" : src.Assignment.Name))
                    .ForMember(dest => dest.AvailableActions, opt => opt.MapFrom(src => src.Assignment == null ? null : src.Assignment.AvailableActions))
                    .ForMember(dest => dest.AssignedUsers, opt => opt.MapFrom(src => src.Assignment == null ? null :
                        string.Join(";", src.Assignment.Users.Select(u => u.Name))));
                cfg.CreateMap<ServiceRequestModel, ServiceRequest>()
                    .ForMember(dest => dest.Number, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());


                // PMTask
                cfg.CreateMap<PMTask, PMTaskModel>();
                cfg.CreateMap<PMTaskModel, PMTask>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // PMServiceItem
                cfg.CreateMap<PMServiceItem, PMServiceItemModel>();
                cfg.CreateMap<PMServiceItemModel, PMServiceItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // PMMiscCost
                cfg.CreateMap<PMMiscCost, PMMiscCostModel>();
                cfg.CreateMap<PMMiscCostModel, PMMiscCost>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // PMLabor
                cfg.CreateMap<PMLabor, PMLaborModel>()
                    .ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src => src.TechnicianId == null ? "" : src.Technician.User.Name));
                cfg.CreateMap<PMLaborModel, PMLabor>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // PMItem
                cfg.CreateMap<PMItem, PMItemModel>();
                cfg.CreateMap<PMItemModel, PMItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // PreventiveMaintenance
                cfg.CreateMap<PreventiveMaintenance, PreventiveMaintenanceModel>();
                cfg.CreateMap<PreventiveMaintenanceModel, PreventiveMaintenance>()
                    .ForMember(dest => dest.Number, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());
                    
                //UserDashboardVisual
                cfg.CreateMap<UserDashboardVisual, UserDashboardVisualModel>();
                cfg.CreateMap<UserDashboardVisualModel, UserDashboardVisual>();

                //UserDashboard
                cfg.CreateMap<UserDashboard, UserDashboardModel>();
                cfg.CreateMap<UserDashboardModel, UserDashboard>();

                //VisualFilter
                cfg.CreateMap<VisualFilter, VisualFilterModel>();
                cfg.CreateMap<VisualFilterModel, VisualFilter>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Visual
                cfg.CreateMap<Visual, VisualModel>();
                cfg.CreateMap<VisualModel, Visual>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // AssignmentHistory
                cfg.CreateMap<AssignmentHistory, AssignmentHistoryModel>();
                cfg.CreateMap<AssignmentHistoryModel, AssignmentHistory>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // Comment
                cfg.CreateMap<Comment, CommentModel>();
                cfg.CreateMap<CommentModel, Comment>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // AssetDowntime
                cfg.CreateMap<AssetDowntime, AssetDowntimeModel>();
                cfg.CreateMap<AssetDowntimeModel, AssetDowntime>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // Reading
                cfg.CreateMap<Reading, ReadingModel>();
                cfg.CreateMap<ReadingModel, Reading>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                // WorkOrderMiscCost
                cfg.CreateMap<WorkOrderMiscCost, WorkOrderMiscCostModel>();
                cfg.CreateMap<WorkOrderMiscCostModel, WorkOrderMiscCost>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore())
                    .ForMember(dest => dest.SyncId, src => src.Ignore());

                //WorkOrderServiceItem
                cfg.CreateMap<WorkOrderServiceItem, WorkOrderServiceItemModel>();
                cfg.CreateMap<WorkOrderServiceItemModel, WorkOrderServiceItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore())
                    .ForMember(dest => dest.SyncId, src => src.Ignore());

                //WorkOrderItem
                cfg.CreateMap<WorkOrderItem, WorkOrderItemModel>();
                cfg.CreateMap<WorkOrderItemModel, WorkOrderItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore())
                    .ForMember(dest => dest.SyncId, src => src.Ignore());

                // WorkOrderTask
                cfg.CreateMap<WorkOrderTask, WorkOrderTaskModel>();
                cfg.CreateMap<WorkOrderTaskModel, WorkOrderTask>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore())
                    .ForMember(dest => dest.SyncId, src => src.Ignore());

                //Task
                cfg.CreateMap<Task, TaskModel>();
                cfg.CreateMap<TaskModel, Task>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //TaskGroup
                cfg.CreateMap<TaskGroup, TaskGroupModel>();
                cfg.CreateMap<TaskGroupModel, TaskGroup>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //ReportColumn
                cfg.CreateMap<ReportColumn, ReportColumnModel>();
                cfg.CreateMap<ReportColumnModel, ReportColumn>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //ReportFilter
                cfg.CreateMap<ReportFilter, ReportFilterModel>();
                cfg.CreateMap<ReportFilterModel, ReportFilter>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Report
                cfg.CreateMap<Report, ReportModel>();
                cfg.CreateMap<ReportModel, Report>()
                    .ForMember(dest => dest.TemplateFileName, src => src.Ignore())
                    .ForMember(dest => dest.TemplateFileBytes, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //WorkOrderLabor
                cfg.CreateMap<WorkOrderLabor, WorkOrderLaborModel>()
                    .ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src => src.TechnicianId == null ? "" : src.Technician.User.Name));
                cfg.CreateMap<WorkOrderLaborModel, WorkOrderLabor>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore())
                    .ForMember(dest => dest.SyncId, src => src.Ignore());

                // AuditTrail
                cfg.CreateMap<AuditTrail, AuditTrailModel>();
                cfg.CreateMap<AuditTrailModel, AuditTrail>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //WorkOrder
                cfg.CreateMap<WorkOrder, WorkOrderModel>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Assignment == null ? "" : src.Assignment.Name))
                    .ForMember(dest => dest.AvailableActions, opt => opt.MapFrom(src => src.Assignment == null ? null : src.Assignment.AvailableActions))
                    .ForMember(dest => dest.AssignedUsers, opt => opt.MapFrom(src => src.Assignment == null ? null :
                        string.Join(";", src.Assignment.Users.Select(u => u.Name))));
                cfg.CreateMap<WorkOrderModel, WorkOrder>()
                    .ForMember(dest => dest.Number, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUserId, src => src.Ignore())
                    .ForMember(dest => dest.SyncId, src => src.Ignore());

                //AssignmentGroup
                cfg.CreateMap<AssignmentGroup, AssignmentGroupModel>();
                cfg.CreateMap<AssignmentGroupModel, AssignmentGroup>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //AssignmentGroupUser
                cfg.CreateMap<AssignmentGroupUser, AssignmentGroupUserModel>();
                cfg.CreateMap<AssignmentGroupUserModel, AssignmentGroupUser>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //AssetLocationHistory
                cfg.CreateMap<AssetLocationHistory, AssetLocationHistoryModel>();
                cfg.CreateMap<AssetLocationHistoryModel, AssetLocationHistory>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //AssetStatusHistory
                cfg.CreateMap<AssetStatusHistory, AssetStatusHistoryModel>();
                cfg.CreateMap<AssetStatusHistoryModel, AssetStatusHistory>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //AssetSparePart
                cfg.CreateMap<AssetSparePart, AssetSparePartModel>();
                cfg.CreateMap<AssetSparePartModel, AssetSparePart>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //PointMeterLineItem
                cfg.CreateMap<PointMeterLineItem, PointMeterLineItemModel>();
                cfg.CreateMap<PointMeterLineItemModel, PointMeterLineItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Point
                cfg.CreateMap<Point, PointModel>();
                cfg.CreateMap<PointModel, Point>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //ScheduledJob
                cfg.CreateMap<ScheduledJob, ScheduledJobModel>();
                cfg.CreateMap<ScheduledJobModel, ScheduledJob>();

                //Code
                cfg.CreateMap<Code, CodeModel>();
                cfg.CreateMap<CodeModel, Code>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Filter
                cfg.CreateMap<Filter, FilterModel>();
                cfg.CreateMap<FilterModel, Filter>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //StoreItem
                cfg.CreateMap<StoreItem, StoreItemModel>();
                cfg.CreateMap<StoreItemModel, StoreItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //PhysicalCount
                cfg.CreateMap<PhysicalCount, PhysicalCountModel>();
                cfg.CreateMap<PhysicalCountModel, PhysicalCount>()
                    .ForMember(dest => dest.Number, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //PhysicalCountItem
                cfg.CreateMap<PhysicalCountItem, PhysicalCountItemModel>();
                cfg.CreateMap<PhysicalCountItemModel, PhysicalCountItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Return
                cfg.CreateMap<Return, ReturnModel>();
                cfg.CreateMap<ReturnModel, Return>()
                    .ForMember(dest => dest.Number, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //ReturnItem
                cfg.CreateMap<ReturnItem, ReturnItemModel>();
                cfg.CreateMap<ReturnItemModel, ReturnItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //StoreLocatorItemLog
                cfg.CreateMap<StoreLocatorItemLog, StoreLocatorItemLogModel>();
                cfg.CreateMap<StoreLocatorItemLogModel, StoreLocatorItemLog>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Adjust
                cfg.CreateMap<Adjust, AdjustModel>();
                cfg.CreateMap<AdjustModel, Adjust>()
                    .ForMember(dest => dest.Number, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //AdjustItem
                cfg.CreateMap<AdjustItem, AdjustItemModel>()
                 .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.Adjust.StoreId));
                cfg.CreateMap<AdjustItemModel, AdjustItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Transfer
                cfg.CreateMap<Transfer, TransferModel>();
                cfg.CreateMap<TransferModel, Transfer>()
                    .ForMember(dest => dest.Number, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //TransferItem
                cfg.CreateMap<TransferItem, TransferItemModel>()
                 .ForMember(dest => dest.FromStoreId, opt => opt.MapFrom(src => src.Transfer.FromStoreId))
                 .ForMember(dest => dest.ToStoreId, opt => opt.MapFrom(src => src.Transfer.ToStoreId));
                cfg.CreateMap<TransferItemModel, TransferItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //ServiceItem
                cfg.CreateMap<ServiceItem, ServiceItemModel>();
                cfg.CreateMap<ServiceItemModel, ServiceItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Issue
                cfg.CreateMap<Issue, IssueModel>();
                cfg.CreateMap<IssueModel, Issue>()
                    .ForMember(dest => dest.Number, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //IssueItem
                cfg.CreateMap<IssueItem, IssueItemModel>()
                    .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.Issue.StoreId))
                    .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.Issue.IssueDate));
                cfg.CreateMap<IssueItemModel, IssueItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //InventorySettings
                cfg.CreateMap<InventorySettings, InventorySettingsModel>();
                cfg.CreateMap<InventorySettingsModel, InventorySettings>();

                //Team
                cfg.CreateMap<Team, TeamModel>();
                cfg.CreateMap<TeamModel, Team>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Technician
                cfg.CreateMap<Technician, TechnicianModel>();
                cfg.CreateMap<TechnicianModel, Technician>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Receipt
                cfg.CreateMap<Receipt, ReceiptModel>();
                cfg.CreateMap<ReceiptModel, Receipt>()
                    .ForMember(dest => dest.Number, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //ReceiptItem
                cfg.CreateMap<ReceiptItem, ReceiptItemModel>()
                    .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.Receipt.StoreId));
                cfg.CreateMap<ReceiptItemModel, ReceiptItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Craft
                cfg.CreateMap<Craft, CraftModel>();
                cfg.CreateMap<CraftModel, Craft>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //ShiftPattern
                cfg.CreateMap<ShiftPattern, ShiftPatternModel>();
                cfg.CreateMap<ShiftPatternModel, ShiftPattern>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Shift
                cfg.CreateMap<Shift, ShiftModel>();
                cfg.CreateMap<ShiftModel, Shift>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //CalendarNonWorking
                cfg.CreateMap<CalendarNonWorking, CalendarNonWorkingModel>();
                cfg.CreateMap<CalendarNonWorkingModel, CalendarNonWorking>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Calendar
                cfg.CreateMap<Calendar, CalendarModel>();
                cfg.CreateMap<CalendarModel, Calendar>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //AutoNumber
                cfg.CreateMap<AutoNumber, AutoNumberModel>();
                cfg.CreateMap<AutoNumberModel, AutoNumber>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Store
                cfg.CreateMap<Store, StoreModel>();
                cfg.CreateMap<StoreModel, Store>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //StoreLocator
                cfg.CreateMap<StoreLocator, StoreLocatorModel>();
                cfg.CreateMap<StoreLocatorModel, StoreLocator>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Asset
                cfg.CreateMap<Asset, AssetModel>();
                cfg.CreateMap<AssetModel, Asset>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Contact
                cfg.CreateMap<Contact, ContactModel>();
                cfg.CreateMap<ContactModel, Contact>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Company
                cfg.CreateMap<Company, CompanyModel>();
                cfg.CreateMap<CompanyModel, Company>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Item
                cfg.CreateMap<Item, ItemModel>();
                cfg.CreateMap<ItemModel, Item>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //ItemGroup
                cfg.CreateMap<ItemGroup, ItemGroupModel>();
                cfg.CreateMap<ItemGroupModel, ItemGroup>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //MessageTemplate
                cfg.CreateMap<MessageTemplate, MessageTemplateModel>();
                cfg.CreateMap<MessageTemplateModel, MessageTemplate>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //WorkflowDefinitionVersion
                cfg.CreateMap<WorkflowDefinitionVersion, WorkflowDefinitionVersionModel>();
                cfg.CreateMap<WorkflowDefinitionVersionModel, WorkflowDefinitionVersion>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //WorkflowDefinition
                cfg.CreateMap<WorkflowDefinition, WorkflowDefinitionModel>();
                cfg.CreateMap<WorkflowDefinitionModel, WorkflowDefinition>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //EntityAttribute
                cfg.CreateMap<EntityAttribute, EntityAttributeModel>();
                cfg.CreateMap<EntityAttributeModel, EntityAttribute>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Attribute
                cfg.CreateMap<Core.Domain.Attribute, AttributeModel>();
                cfg.CreateMap<AttributeModel, Core.Domain.Attribute>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //MeterLineItem
                cfg.CreateMap<MeterLineItem, MeterLineItemModel>();
                cfg.CreateMap<MeterLineItemModel, MeterLineItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //MeterGroup
                cfg.CreateMap<MeterGroup, MeterGroupModel>();
                cfg.CreateMap<MeterGroupModel, MeterGroup>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());


                //Attachment
                cfg.CreateMap<Attachment, AttachmentModel>();
                cfg.CreateMap<AttachmentModel, Attachment>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Meter
                cfg.CreateMap<Meter, MeterModel>();
                cfg.CreateMap<MeterModel, Meter>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Location
                cfg.CreateMap<Location, LocationModel>();
                cfg.CreateMap<LocationModel, Location>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //PermissionRecord
                cfg.CreateMap<PermissionRecord, AccessControlModel>();
                cfg.CreateMap<AccessControlModel, PermissionRecord>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //SecurityGroup
                cfg.CreateMap<SecurityGroup, SecurityGroupModel>();
                cfg.CreateMap<SecurityGroupModel, SecurityGroup>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Address
                cfg.CreateMap<Address, AddressModel>();
                cfg.CreateMap<AddressModel, Address>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Organization
                cfg.CreateMap<Organization, OrganizationModel>();
                cfg.CreateMap<OrganizationModel, Organization>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Site
                cfg.CreateMap<Site, SiteModel>();
                cfg.CreateMap<SiteModel, Site>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Language
                cfg.CreateMap<Language, LanguageModel>()
                    .ForMember(dest => dest.Search, mo => mo.Ignore());
                cfg.CreateMap<LanguageModel, Language>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore())
                    .ForMember(dest => dest.LocaleStringResources, mo => mo.Ignore());

                //Currency
                cfg.CreateMap<Currency, CurrencyModel>();
                cfg.CreateMap<CurrencyModel, Currency>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Module
                cfg.CreateMap<Module, ModuleModel>();
                cfg.CreateMap<ModuleModel, Module>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Feature
                cfg.CreateMap<Feature, FeatureModel>();
                cfg.CreateMap<FeatureModel, Feature>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //FeatureAction
                cfg.CreateMap<FeatureAction, FeatureActionModel>();
                cfg.CreateMap<FeatureActionModel, FeatureAction>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //Log
                cfg.CreateMap<Log, LogModel>()
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());
                cfg.CreateMap<LogModel, Log>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore())
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
                    .ForMember(dest => dest.LogLevelId, mo => mo.Ignore())
                    .ForMember(dest => dest.User, mo => mo.Ignore());

                //User
                cfg.CreateMap<User, UserModel>();
                cfg.CreateMap<UserModel, User>()
                    .ForMember(dest => dest.LoginPassword, src => src.Ignore())
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //GeneralSettings
                cfg.CreateMap<GeneralSettings, GeneralSettingsModel>();
                cfg.CreateMap<GeneralSettingsModel, GeneralSettings>();

                //ValueItem
                cfg.CreateMap<ValueItem, ValueItemModel>();
                cfg.CreateMap<ValueItemModel, ValueItem>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore())
                    .ForMember(dest => dest.ValueItemCategory, src => src.Ignore());

                //ValueItem
                cfg.CreateMap<ValueItemCategory, ValueItemCategoryModel>();
                cfg.CreateMap<ValueItemCategoryModel, ValueItemCategory>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //UnitOfMeasure
                cfg.CreateMap<UnitOfMeasure, UnitOfMeasureModel>();
                cfg.CreateMap<UnitOfMeasureModel, UnitOfMeasure>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());

                //UnitConversion
                cfg.CreateMap<UnitConversion, UnitConversionModel>();
                cfg.CreateMap<UnitConversionModel, UnitConversion>()
                    .ForMember(dest => dest.CreatedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedDateTime, src => src.Ignore())
                    .ForMember(dest => dest.CreatedUser, src => src.Ignore())
                    .ForMember(dest => dest.ModifiedUser, src => src.Ignore());
            };

            return action;
        }

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }
}