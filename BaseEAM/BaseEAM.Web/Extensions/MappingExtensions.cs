/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Infrastructure;
using BaseEAM.Core.Infrastructure.Mapper;
using BaseEAM.Core.Reflection;
using BaseEAM.Core.Timing.Utils;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Models;
using System;

namespace BaseEAM.Web.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            var destination = AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
            if (typeof(BaseEamModel).IsAssignableFrom(typeof(TDestination))
                || typeof(BaseEamEntityModel).IsAssignableFrom(typeof(TDestination)))
            {
                //If we are mapping from Entity to Model
                //then we will convert all DateTime values from UTC to User timezone.
                //We need to mark complex properties with attribute ComplexTypeAttribute
                var dateTimeHelper = EngineContext.Current.Resolve<IDateTimeHelper>();
                var dateTimePropertyInfos = DateTimePropertyInfoHelper.GetDatePropertyInfos(typeof(TDestination));
                dateTimePropertyInfos.DateTimePropertyInfos.ForEach(property =>
                {
                    var dateTime = (DateTime?)property.GetValue(destination);
                    if (dateTime.HasValue)
                    {
                        property.SetValue(destination, dateTimeHelper.ConvertToUserTime(dateTime.Value, DateTimeKind.Utc));
                    }
                });

                dateTimePropertyInfos.ComplexTypePropertyPaths.ForEach(propertPath =>
                {
                    var dateTime = (DateTime?)ReflectionHelper.GetValueByPath(destination, typeof(TDestination), propertPath);
                    if (dateTime.HasValue)
                    {
                        ReflectionHelper.SetValueByPath(destination, typeof(TDestination), propertPath, dateTimeHelper.ConvertToUserTime(dateTime.Value, DateTimeKind.Utc));
                    }
                });

                //then set default site if have
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var siteIdProperty = destination.GetType().GetProperty("SiteId");
                if(siteIdProperty != null)
                {
                    object value = siteIdProperty.GetValue(destination, null);
                    if (value == null)
                    {
                        siteIdProperty.SetValue(destination, workContext.CurrentUser.DefaultSiteId);
                    }
                }
            }
            return destination;
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region ContractPriceItem

        public static ContractPriceItemModel ToModel(this ContractPriceItem entity)
        {
            return entity.MapTo<ContractPriceItem, ContractPriceItemModel>();
        }

        public static ContractPriceItem ToEntity(this ContractPriceItemModel model)
        {
            return model.MapTo<ContractPriceItemModel, ContractPriceItem>();
        }

        public static ContractPriceItem ToEntity(this ContractPriceItemModel model, ContractPriceItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ContractTerm

        public static ContractTermModel ToModel(this ContractTerm entity)
        {
            return entity.MapTo<ContractTerm, ContractTermModel>();
        }

        public static ContractTerm ToEntity(this ContractTermModel model)
        {
            return model.MapTo<ContractTermModel, ContractTerm>();
        }

        public static ContractTerm ToEntity(this ContractTermModel model, ContractTerm destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Contract

        public static ContractModel ToModel(this Contract entity)
        {
            return entity.MapTo<Contract, ContractModel>();
        }

        public static Contract ToEntity(this ContractModel model)
        {
            return model.MapTo<ContractModel, Contract>();
        }

        public static Contract ToEntity(this ContractModel model, Contract destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Message

        public static MessageModel ToModel(this Message entity)
        {
            return entity.MapTo<Message, MessageModel>();
        }

        public static Message ToEntity(this MessageModel model)
        {
            return model.MapTo<MessageModel, Message>();
        }

        public static Message ToEntity(this MessageModel model, Message destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region MeterEventHistory

        public static MeterEventHistoryModel ToModel(this MeterEventHistory entity)
        {
            return entity.MapTo<MeterEventHistory, MeterEventHistoryModel>();
        }

        public static MeterEventHistory ToEntity(this MeterEventHistoryModel model)
        {
            return model.MapTo<MeterEventHistoryModel, MeterEventHistory>();
        }

        public static MeterEventHistory ToEntity(this MeterEventHistoryModel model, MeterEventHistory destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region MeterEvent

        public static MeterEventModel ToModel(this MeterEvent entity)
        {
            return entity.MapTo<MeterEvent, MeterEventModel>();
        }

        public static MeterEvent ToEntity(this MeterEventModel model)
        {
            return model.MapTo<MeterEventModel, MeterEvent>();
        }

        public static MeterEvent ToEntity(this MeterEventModel model, MeterEvent destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PMMeterFrequency

        public static PMMeterFrequencyModel ToModel(this PMMeterFrequency entity)
        {
            return entity.MapTo<PMMeterFrequency, PMMeterFrequencyModel>();
        }

        public static PMMeterFrequency ToEntity(this PMMeterFrequencyModel model)
        {
            return model.MapTo<PMMeterFrequencyModel, PMMeterFrequency>();
        }

        public static PMMeterFrequency ToEntity(this PMMeterFrequencyModel model, PMMeterFrequency destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AuditEntityConfiguration

        public static AuditEntityConfigurationModel ToModel(this AuditEntityConfiguration entity)
        {
            return entity.MapTo<AuditEntityConfiguration, AuditEntityConfigurationModel>();
        }

        public static AuditEntityConfiguration ToEntity(this AuditEntityConfigurationModel model)
        {
            return model.MapTo<AuditEntityConfigurationModel, AuditEntityConfiguration>();
        }

        public static AuditEntityConfiguration ToEntity(this AuditEntityConfigurationModel model, AuditEntityConfiguration destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ImportProfile

        public static ImportProfileModel ToModel(this ImportProfile entity)
        {
            return entity.MapTo<ImportProfile, ImportProfileModel>();
        }

        public static ImportProfile ToEntity(this ImportProfileModel model)
        {
            return model.MapTo<ImportProfileModel, ImportProfile>();
        }

        public static ImportProfile ToEntity(this ImportProfileModel model, ImportProfile destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Assignment

        public static AssignmentModel ToModel(this Assignment entity)
        {
            return entity.MapTo<Assignment, AssignmentModel>();
        }

        public static Assignment ToEntity(this AssignmentModel model)
        {
            return model.MapTo<AssignmentModel, Assignment>();
        }

        public static Assignment ToEntity(this AssignmentModel model, Assignment destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ServiceRequest

        public static ServiceRequestModel ToModel(this ServiceRequest entity)
        {
            return entity.MapTo<ServiceRequest, ServiceRequestModel>();
        }

        public static ServiceRequest ToEntity(this ServiceRequestModel model)
        {
            return model.MapTo<ServiceRequestModel, ServiceRequest>();
        }

        public static ServiceRequest ToEntity(this ServiceRequestModel model, ServiceRequest destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PreventiveMaintenance

        public static PreventiveMaintenanceModel ToModel(this PreventiveMaintenance entity)
        {
            return entity.MapTo<PreventiveMaintenance, PreventiveMaintenanceModel>();
        }

        public static PreventiveMaintenance ToEntity(this PreventiveMaintenanceModel model)
        {
            return model.MapTo<PreventiveMaintenanceModel, PreventiveMaintenance>();
        }

        public static PreventiveMaintenance ToEntity(this PreventiveMaintenanceModel model, PreventiveMaintenance destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PMTask

        public static PMTaskModel ToModel(this PMTask entity)
        {
            return entity.MapTo<PMTask, PMTaskModel>();
        }

        public static PMTask ToEntity(this PMTaskModel model)
        {
            return model.MapTo<PMTaskModel, PMTask>();
        }

        public static PMTask ToEntity(this PMTaskModel model, PMTask destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PMServiceItem

        public static PMServiceItemModel ToModel(this PMServiceItem entity)
        {
            return entity.MapTo<PMServiceItem, PMServiceItemModel>();
        }

        public static PMServiceItem ToEntity(this PMServiceItemModel model)
        {
            return model.MapTo<PMServiceItemModel, PMServiceItem>();
        }

        public static PMServiceItem ToEntity(this PMServiceItemModel model, PMServiceItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PMMiscCost

        public static PMMiscCostModel ToModel(this PMMiscCost entity)
        {
            return entity.MapTo<PMMiscCost, PMMiscCostModel>();
        }

        public static PMMiscCost ToEntity(this PMMiscCostModel model)
        {
            return model.MapTo<PMMiscCostModel, PMMiscCost>();
        }

        public static PMMiscCost ToEntity(this PMMiscCostModel model, PMMiscCost destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PMLabor

        public static PMLaborModel ToModel(this PMLabor entity)
        {
            return entity.MapTo<PMLabor, PMLaborModel>();
        }

        public static PMLabor ToEntity(this PMLaborModel model)
        {
            return model.MapTo<PMLaborModel, PMLabor>();
        }

        public static PMLabor ToEntity(this PMLaborModel model, PMLabor destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PMItem

        public static PMItemModel ToModel(this PMItem entity)
        {
            return entity.MapTo<PMItem, PMItemModel>();
        }

        public static PMItem ToEntity(this PMItemModel model)
        {
            return model.MapTo<PMItemModel, PMItem>();
        }

        public static PMItem ToEntity(this PMItemModel model, PMItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region UserDashboardVisual

        public static UserDashboardVisualModel ToModel(this UserDashboardVisual entity)
        {
            return entity.MapTo<UserDashboardVisual, UserDashboardVisualModel>();
        }

        public static UserDashboardVisual ToEntity(this UserDashboardVisualModel model)
        {
            return model.MapTo<UserDashboardVisualModel, UserDashboardVisual>();
        }

        public static UserDashboardVisual ToEntity(this UserDashboardVisualModel model, UserDashboardVisual destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region UserDashboard

        public static UserDashboardModel ToModel(this UserDashboard entity)
        {
            return entity.MapTo<UserDashboard, UserDashboardModel>();
        }

        public static UserDashboard ToEntity(this UserDashboardModel model)
        {
            return model.MapTo<UserDashboardModel, UserDashboard>();
        }

        public static UserDashboard ToEntity(this UserDashboardModel model, UserDashboard destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AssignmentHistory

        public static AssignmentHistoryModel ToModel(this AssignmentHistory entity)
        {
            return entity.MapTo<AssignmentHistory, AssignmentHistoryModel>();
        }

        public static AssignmentHistory ToEntity(this AssignmentHistoryModel model)
        {
            return model.MapTo<AssignmentHistoryModel, AssignmentHistory>();
        }

        public static AssignmentHistory ToEntity(this AssignmentHistoryModel model, AssignmentHistory destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region VisualFilter

        public static VisualFilterModel ToModel(this VisualFilter entity)
        {
            return entity.MapTo<VisualFilter, VisualFilterModel>();
        }

        public static VisualFilter ToEntity(this VisualFilterModel model)
        {
            return model.MapTo<VisualFilterModel, VisualFilter>();
        }

        public static VisualFilter ToEntity(this VisualFilterModel model, VisualFilter destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Visual

        public static VisualModel ToModel(this Visual entity)
        {
            return entity.MapTo<Visual, VisualModel>();
        }

        public static Visual ToEntity(this VisualModel model)
        {
            return model.MapTo<VisualModel, Visual>();
        }

        public static Visual ToEntity(this VisualModel model, Visual destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Comment

        public static CommentModel ToModel(this Comment entity)
        {
            return entity.MapTo<Comment, CommentModel>();
        }

        public static Comment ToEntity(this CommentModel model)
        {
            return model.MapTo<CommentModel, Comment>();
        }

        public static Comment ToEntity(this CommentModel model, Comment destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AssetDowntime

        public static AssetDowntimeModel ToModel(this AssetDowntime entity)
        {
            return entity.MapTo<AssetDowntime, AssetDowntimeModel>();
        }

        public static AssetDowntime ToEntity(this AssetDowntimeModel model)
        {
            return model.MapTo<AssetDowntimeModel, AssetDowntime>();
        }

        public static AssetDowntime ToEntity(this AssetDowntimeModel model, AssetDowntime destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Reading

        public static ReadingModel ToModel(this Reading entity)
        {
            return entity.MapTo<Reading, ReadingModel>();
        }

        public static Reading ToEntity(this ReadingModel model)
        {
            return model.MapTo<ReadingModel, Reading>();
        }

        public static Reading ToEntity(this ReadingModel model, Reading destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region WorkOrderMiscCost

        public static WorkOrderMiscCostModel ToModel(this WorkOrderMiscCost entity)
        {
            return entity.MapTo<WorkOrderMiscCost, WorkOrderMiscCostModel>();
        }

        public static WorkOrderMiscCost ToEntity(this WorkOrderMiscCostModel model)
        {
            return model.MapTo<WorkOrderMiscCostModel, WorkOrderMiscCost>();
        }

        public static WorkOrderMiscCost ToEntity(this WorkOrderMiscCostModel model, WorkOrderMiscCost destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region WorkOrderServiceItem

        public static WorkOrderServiceItemModel ToModel(this WorkOrderServiceItem entity)
        {
            return entity.MapTo<WorkOrderServiceItem, WorkOrderServiceItemModel>();
        }

        public static WorkOrderServiceItem ToEntity(this WorkOrderServiceItemModel model)
        {
            return model.MapTo<WorkOrderServiceItemModel, WorkOrderServiceItem>();
        }

        public static WorkOrderServiceItem ToEntity(this WorkOrderServiceItemModel model, WorkOrderServiceItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region WorkOrderItem

        public static WorkOrderItemModel ToModel(this WorkOrderItem entity)
        {
            return entity.MapTo<WorkOrderItem, WorkOrderItemModel>();
        }

        public static WorkOrderItem ToEntity(this WorkOrderItemModel model)
        {
            return model.MapTo<WorkOrderItemModel, WorkOrderItem>();
        }

        public static WorkOrderItem ToEntity(this WorkOrderItemModel model, WorkOrderItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region WorkOrderTask

        public static WorkOrderTaskModel ToModel(this WorkOrderTask entity)
        {
            return entity.MapTo<WorkOrderTask, WorkOrderTaskModel>();
        }

        public static WorkOrderTask ToEntity(this WorkOrderTaskModel model)
        {
            return model.MapTo<WorkOrderTaskModel, WorkOrderTask>();
        }

        public static WorkOrderTask ToEntity(this WorkOrderTaskModel model, WorkOrderTask destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Task

        public static TaskModel ToModel(this Task entity)
        {
            return entity.MapTo<Task, TaskModel>();
        }

        public static Task ToEntity(this TaskModel model)
        {
            return model.MapTo<TaskModel, Task>();
        }

        public static Task ToEntity(this TaskModel model, Task destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region TaskGroup

        public static TaskGroupModel ToModel(this TaskGroup entity)
        {
            return entity.MapTo<TaskGroup, TaskGroupModel>();
        }

        public static TaskGroup ToEntity(this TaskGroupModel model)
        {
            return model.MapTo<TaskGroupModel, TaskGroup>();
        }

        public static TaskGroup ToEntity(this TaskGroupModel model, TaskGroup destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ReportColumn

        public static ReportColumnModel ToModel(this ReportColumn entity)
        {
            return entity.MapTo<ReportColumn, ReportColumnModel>();
        }

        public static ReportColumn ToEntity(this ReportColumnModel model)
        {
            return model.MapTo<ReportColumnModel, ReportColumn>();
        }

        public static ReportColumn ToEntity(this ReportColumnModel model, ReportColumn destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ReportFilter

        public static ReportFilterModel ToModel(this ReportFilter entity)
        {
            return entity.MapTo<ReportFilter, ReportFilterModel>();
        }

        public static ReportFilter ToEntity(this ReportFilterModel model)
        {
            return model.MapTo<ReportFilterModel, ReportFilter>();
        }

        public static ReportFilter ToEntity(this ReportFilterModel model, ReportFilter destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Report

        public static ReportModel ToModel(this Report entity)
        {
            return entity.MapTo<Report, ReportModel>();
        }

        public static Report ToEntity(this ReportModel model)
        {
            return model.MapTo<ReportModel, Report>();
        }

        public static Report ToEntity(this ReportModel model, Report destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region WorkOrderLabor

        public static WorkOrderLaborModel ToModel(this WorkOrderLabor entity)
        {
            return entity.MapTo<WorkOrderLabor, WorkOrderLaborModel>();
        }

        public static WorkOrderLabor ToEntity(this WorkOrderLaborModel model)
        {
            return model.MapTo<WorkOrderLaborModel, WorkOrderLabor>();
        }

        public static WorkOrderLabor ToEntity(this WorkOrderLaborModel model, WorkOrderLabor destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AuditTrail

        public static AuditTrailModel ToModel(this AuditTrail entity)
        {
            return entity.MapTo<AuditTrail, AuditTrailModel>();
        }

        public static AuditTrail ToEntity(this AuditTrailModel model)
        {
            return model.MapTo<AuditTrailModel, AuditTrail>();
        }

        public static AuditTrail ToEntity(this AuditTrailModel model, AuditTrail destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region WorkOrder

        public static WorkOrderModel ToModel(this WorkOrder entity)
        {
            return entity.MapTo<WorkOrder, WorkOrderModel>();
        }

        public static WorkOrder ToEntity(this WorkOrderModel model)
        {
            return model.MapTo<WorkOrderModel, WorkOrder>();
        }

        public static WorkOrder ToEntity(this WorkOrderModel model, WorkOrder destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AssignmentGroup

        public static AssignmentGroupModel ToModel(this AssignmentGroup entity)
        {
            return entity.MapTo<AssignmentGroup, AssignmentGroupModel>();
        }

        public static AssignmentGroup ToEntity(this AssignmentGroupModel model)
        {
            return model.MapTo<AssignmentGroupModel, AssignmentGroup>();
        }

        public static AssignmentGroup ToEntity(this AssignmentGroupModel model, AssignmentGroup destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AssignmentGroupUser

        public static AssignmentGroupUserModel ToModel(this AssignmentGroupUser entity)
        {
            return entity.MapTo<AssignmentGroupUser, AssignmentGroupUserModel>();
        }

        public static AssignmentGroupUser ToEntity(this AssignmentGroupUserModel model)
        {
            return model.MapTo<AssignmentGroupUserModel, AssignmentGroupUser>();
        }

        public static AssignmentGroupUser ToEntity(this AssignmentGroupUserModel model, AssignmentGroupUser destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AssetLocationHistory

        public static AssetLocationHistoryModel ToModel(this AssetLocationHistory entity)
        {
            return entity.MapTo<AssetLocationHistory, AssetLocationHistoryModel>();
        }

        public static AssetLocationHistory ToEntity(this AssetLocationHistoryModel model)
        {
            return model.MapTo<AssetLocationHistoryModel, AssetLocationHistory>();
        }

        public static AssetLocationHistory ToEntity(this AssetLocationHistoryModel model, AssetLocationHistory destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AssetStatusHistory

        public static AssetStatusHistoryModel ToModel(this AssetStatusHistory entity)
        {
            return entity.MapTo<AssetStatusHistory, AssetStatusHistoryModel>();
        }

        public static AssetStatusHistory ToEntity(this AssetStatusHistoryModel model)
        {
            return model.MapTo<AssetStatusHistoryModel, AssetStatusHistory>();
        }

        public static AssetStatusHistory ToEntity(this AssetStatusHistoryModel model, AssetStatusHistory destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AssetSparePart

        public static AssetSparePartModel ToModel(this AssetSparePart entity)
        {
            return entity.MapTo<AssetSparePart, AssetSparePartModel>();
        }

        public static AssetSparePart ToEntity(this AssetSparePartModel model)
        {
            return model.MapTo<AssetSparePartModel, AssetSparePart>();
        }

        public static AssetSparePart ToEntity(this AssetSparePartModel model, AssetSparePart destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PointMeterLineItem

        public static PointMeterLineItemModel ToModel(this PointMeterLineItem entity)
        {
            return entity.MapTo<PointMeterLineItem, PointMeterLineItemModel>();
        }

        public static PointMeterLineItem ToEntity(this PointMeterLineItemModel model)
        {
            return model.MapTo<PointMeterLineItemModel, PointMeterLineItem>();
        }

        public static PointMeterLineItem ToEntity(this PointMeterLineItemModel model, PointMeterLineItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Point

        public static PointModel ToModel(this Point entity)
        {
            return entity.MapTo<Point, PointModel>();
        }

        public static Point ToEntity(this PointModel model)
        {
            return model.MapTo<PointModel, Point>();
        }

        public static Point ToEntity(this PointModel model, Point destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ScheduledJob

        public static ScheduledJobModel ToModel(this ScheduledJob entity)
        {
            return entity.MapTo<ScheduledJob, ScheduledJobModel>();
        }

        public static ScheduledJob ToEntity(this ScheduledJobModel model)
        {
            return model.MapTo<ScheduledJobModel, ScheduledJob>();
        }

        public static ScheduledJob ToEntity(this ScheduledJobModel model, ScheduledJob destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Code

        public static CodeModel ToModel(this Code entity)
        {
            return entity.MapTo<Code, CodeModel>();
        }

        public static Code ToEntity(this CodeModel model)
        {
            return model.MapTo<CodeModel, Code>();
        }

        public static Code ToEntity(this CodeModel model, Code destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Filter

        public static FilterModel ToModel(this Filter entity)
        {
            return entity.MapTo<Filter, FilterModel>();
        }

        public static Filter ToEntity(this FilterModel model)
        {
            return model.MapTo<FilterModel, Filter>();
        }

        public static Filter ToEntity(this FilterModel model, Filter destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region StoreItem

        public static StoreItemModel ToModel(this StoreItem entity)
        {
            return entity.MapTo<StoreItem, StoreItemModel>();
        }

        public static StoreItem ToEntity(this StoreItemModel model)
        {
            return model.MapTo<StoreItemModel, StoreItem>();
        }

        public static StoreItem ToEntity(this StoreItemModel model, StoreItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PhysicalCount

        public static PhysicalCountModel ToModel(this PhysicalCount entity)
        {
            return entity.MapTo<PhysicalCount, PhysicalCountModel>();
        }

        public static PhysicalCount ToEntity(this PhysicalCountModel model)
        {
            return model.MapTo<PhysicalCountModel, PhysicalCount>();
        }

        public static PhysicalCount ToEntity(this PhysicalCountModel model, PhysicalCount destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PhysicalCountItem

        public static PhysicalCountItemModel ToModel(this PhysicalCountItem entity)
        {
            return entity.MapTo<PhysicalCountItem, PhysicalCountItemModel>();
        }

        public static PhysicalCountItem ToEntity(this PhysicalCountItemModel model)
        {
            return model.MapTo<PhysicalCountItemModel, PhysicalCountItem>();
        }

        public static PhysicalCountItem ToEntity(this PhysicalCountItemModel model, PhysicalCountItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Return

        public static ReturnModel ToModel(this Return entity)
        {
            return entity.MapTo<Return, ReturnModel>();
        }

        public static Return ToEntity(this ReturnModel model)
        {
            return model.MapTo<ReturnModel, Return>();
        }

        public static Return ToEntity(this ReturnModel model, Return destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ReturnItem

        public static ReturnItemModel ToModel(this ReturnItem entity)
        {
            return entity.MapTo<ReturnItem, ReturnItemModel>();
        }

        public static ReturnItem ToEntity(this ReturnItemModel model)
        {
            return model.MapTo<ReturnItemModel, ReturnItem>();
        }

        public static ReturnItem ToEntity(this ReturnItemModel model, ReturnItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region StoreLocatorItemLog

        public static StoreLocatorItemLogModel ToModel(this StoreLocatorItemLog entity)
        {
            return entity.MapTo<StoreLocatorItemLog, StoreLocatorItemLogModel>();
        }

        public static StoreLocatorItemLog ToEntity(this StoreLocatorItemLogModel model)
        {
            return model.MapTo<StoreLocatorItemLogModel, StoreLocatorItemLog>();
        }

        public static StoreLocatorItemLog ToEntity(this StoreLocatorItemLogModel model, StoreLocatorItemLog destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Adjust

        public static AdjustModel ToModel(this Adjust entity)
        {
            return entity.MapTo<Adjust, AdjustModel>();
        }

        public static Adjust ToEntity(this AdjustModel model)
        {
            return model.MapTo<AdjustModel, Adjust>();
        }

        public static Adjust ToEntity(this AdjustModel model, Adjust destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AdjustItem

        public static AdjustItemModel ToModel(this AdjustItem entity)
        {
            return entity.MapTo<AdjustItem, AdjustItemModel>();
        }

        public static AdjustItem ToEntity(this AdjustItemModel model)
        {
            return model.MapTo<AdjustItemModel, AdjustItem>();
        }

        public static AdjustItem ToEntity(this AdjustItemModel model, AdjustItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Transfer

        public static TransferModel ToModel(this Transfer entity)
        {
            return entity.MapTo<Transfer, TransferModel>();
        }

        public static Transfer ToEntity(this TransferModel model)
        {
            return model.MapTo<TransferModel, Transfer>();
        }

        public static Transfer ToEntity(this TransferModel model, Transfer destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region TransferItem

        public static TransferItemModel ToModel(this TransferItem entity)
        {
            return entity.MapTo<TransferItem, TransferItemModel>();
        }

        public static TransferItem ToEntity(this TransferItemModel model)
        {
            return model.MapTo<TransferItemModel, TransferItem>();
        }

        public static TransferItem ToEntity(this TransferItemModel model, TransferItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ServiceItem

        public static ServiceItemModel ToModel(this ServiceItem entity)
        {
            return entity.MapTo<ServiceItem, ServiceItemModel>();
        }

        public static ServiceItem ToEntity(this ServiceItemModel model)
        {
            return model.MapTo<ServiceItemModel, ServiceItem>();
        }

        public static ServiceItem ToEntity(this ServiceItemModel model, ServiceItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Issue

        public static IssueModel ToModel(this Issue entity)
        {
            return entity.MapTo<Issue, IssueModel>();
        }

        public static Issue ToEntity(this IssueModel model)
        {
            return model.MapTo<IssueModel, Issue>();
        }

        public static Issue ToEntity(this IssueModel model, Issue destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region IssueItem

        public static IssueItemModel ToModel(this IssueItem entity)
        {
            return entity.MapTo<IssueItem, IssueItemModel>();
        }

        public static IssueItem ToEntity(this IssueItemModel model)
        {
            return model.MapTo<IssueItemModel, IssueItem>();
        }

        public static IssueItem ToEntity(this IssueItemModel model, IssueItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region InventorySettings

        public static InventorySettingsModel ToModel(this InventorySettings entity)
        {
            return entity.MapTo<InventorySettings, InventorySettingsModel>();
        }

        public static InventorySettings ToEntity(this InventorySettingsModel model)
        {
            return model.MapTo<InventorySettingsModel, InventorySettings>();
        }

        public static InventorySettings ToEntity(this InventorySettingsModel model, InventorySettings destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Technician

        public static TechnicianModel ToModel(this Technician entity)
        {
            return entity.MapTo<Technician, TechnicianModel>();
        }

        public static Technician ToEntity(this TechnicianModel model)
        {
            return model.MapTo<TechnicianModel, Technician>();
        }

        public static Technician ToEntity(this TechnicianModel model, Technician destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Receipt

        public static ReceiptModel ToModel(this Receipt entity)
        {
            return entity.MapTo<Receipt, ReceiptModel>();
        }

        public static Receipt ToEntity(this ReceiptModel model)
        {
            return model.MapTo<ReceiptModel, Receipt>();
        }

        public static Receipt ToEntity(this ReceiptModel model, Receipt destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ReceiptItem

        public static ReceiptItemModel ToModel(this ReceiptItem entity)
        {
            return entity.MapTo<ReceiptItem, ReceiptItemModel>();
        }

        public static ReceiptItem ToEntity(this ReceiptItemModel model)
        {
            return model.MapTo<ReceiptItemModel, ReceiptItem>();
        }

        public static ReceiptItem ToEntity(this ReceiptItemModel model, ReceiptItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Craft

        public static CraftModel ToModel(this Craft entity)
        {
            return entity.MapTo<Craft, CraftModel>();
        }

        public static Craft ToEntity(this CraftModel model)
        {
            return model.MapTo<CraftModel, Craft>();
        }

        public static Craft ToEntity(this CraftModel model, Craft destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Team

        public static TeamModel ToModel(this Team entity)
        {
            return entity.MapTo<Team, TeamModel>();
        }

        public static Team ToEntity(this TeamModel model)
        {
            return model.MapTo<TeamModel, Team>();
        }

        public static Team ToEntity(this TeamModel model, Team destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ShiftPattern

        public static ShiftPatternModel ToModel(this ShiftPattern entity)
        {
            return entity.MapTo<ShiftPattern, ShiftPatternModel>();
        }

        public static ShiftPattern ToEntity(this ShiftPatternModel model)
        {
            return model.MapTo<ShiftPatternModel, ShiftPattern>();
        }

        public static ShiftPattern ToEntity(this ShiftPatternModel model, ShiftPattern destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Shift

        public static ShiftModel ToModel(this Shift entity)
        {
            return entity.MapTo<Shift, ShiftModel>();
        }

        public static Shift ToEntity(this ShiftModel model)
        {
            return model.MapTo<ShiftModel, Shift>();
        }

        public static Shift ToEntity(this ShiftModel model, Shift destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region CalendarNonWorking

        public static CalendarNonWorkingModel ToModel(this CalendarNonWorking entity)
        {
            return entity.MapTo<CalendarNonWorking, CalendarNonWorkingModel>();
        }

        public static CalendarNonWorking ToEntity(this CalendarNonWorkingModel model)
        {
            return model.MapTo<CalendarNonWorkingModel, CalendarNonWorking>();
        }

        public static CalendarNonWorking ToEntity(this CalendarNonWorkingModel model, CalendarNonWorking destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Calendar

        public static CalendarModel ToModel(this Calendar entity)
        {
            return entity.MapTo<Calendar, CalendarModel>();
        }

        public static Calendar ToEntity(this CalendarModel model)
        {
            return model.MapTo<CalendarModel, Calendar>();
        }

        public static Calendar ToEntity(this CalendarModel model, Calendar destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region AutoNumber

        public static AutoNumberModel ToModel(this AutoNumber entity)
        {
            return entity.MapTo<AutoNumber, AutoNumberModel>();
        }

        public static AutoNumber ToEntity(this AutoNumberModel model)
        {
            return model.MapTo<AutoNumberModel, AutoNumber>();
        }

        public static AutoNumber ToEntity(this AutoNumberModel model, AutoNumber destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Store

        public static StoreModel ToModel(this Store entity)
        {
            return entity.MapTo<Store, StoreModel>();
        }

        public static Store ToEntity(this StoreModel model)
        {
            return model.MapTo<StoreModel, Store>();
        }

        public static Store ToEntity(this StoreModel model, Store destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region StoreLocator

        public static StoreLocatorModel ToModel(this StoreLocator entity)
        {
            return entity.MapTo<StoreLocator, StoreLocatorModel>();
        }

        public static StoreLocator ToEntity(this StoreLocatorModel model)
        {
            return model.MapTo<StoreLocatorModel, StoreLocator>();
        }

        public static StoreLocator ToEntity(this StoreLocatorModel model, StoreLocator destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Asset

        public static AssetModel ToModel(this Asset entity)
        {
            return entity.MapTo<Asset, AssetModel>();
        }

        public static Asset ToEntity(this AssetModel model)
        {
            return model.MapTo<AssetModel, Asset>();
        }

        public static Asset ToEntity(this AssetModel model, Asset destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Contact

        public static ContactModel ToModel(this Contact entity)
        {
            return entity.MapTo<Contact, ContactModel>();
        }

        public static Contact ToEntity(this ContactModel model)
        {
            return model.MapTo<ContactModel, Contact>();
        }

        public static Contact ToEntity(this ContactModel model, Contact destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Company

        public static CompanyModel ToModel(this Company entity)
        {
            return entity.MapTo<Company, CompanyModel>();
        }

        public static Company ToEntity(this CompanyModel model)
        {
            return model.MapTo<CompanyModel, Company>();
        }

        public static Company ToEntity(this CompanyModel model, Company destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Item

        public static ItemModel ToModel(this Item entity)
        {
            return entity.MapTo<Item, ItemModel>();
        }

        public static Item ToEntity(this ItemModel model)
        {
            return model.MapTo<ItemModel, Item>();
        }

        public static Item ToEntity(this ItemModel model, Item destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ItemGroup

        public static ItemGroupModel ToModel(this ItemGroup entity)
        {
            return entity.MapTo<ItemGroup, ItemGroupModel>();
        }

        public static ItemGroup ToEntity(this ItemGroupModel model)
        {
            return model.MapTo<ItemGroupModel, ItemGroup>();
        }

        public static ItemGroup ToEntity(this ItemGroupModel model, ItemGroup destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region MessageTemplate

        public static MessageTemplateModel ToModel(this MessageTemplate entity)
        {
            return entity.MapTo<MessageTemplate, MessageTemplateModel>();
        }

        public static MessageTemplate ToEntity(this MessageTemplateModel model)
        {
            return model.MapTo<MessageTemplateModel, MessageTemplate>();
        }

        public static MessageTemplate ToEntity(this MessageTemplateModel model, MessageTemplate destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region WorkflowDefinitionVersion

        public static WorkflowDefinitionVersionModel ToModel(this WorkflowDefinitionVersion entity)
        {
            return entity.MapTo<WorkflowDefinitionVersion, WorkflowDefinitionVersionModel>();
        }

        public static WorkflowDefinitionVersion ToEntity(this WorkflowDefinitionVersionModel model)
        {
            return model.MapTo<WorkflowDefinitionVersionModel, WorkflowDefinitionVersion>();
        }

        public static WorkflowDefinitionVersion ToEntity(this WorkflowDefinitionVersionModel model, WorkflowDefinitionVersion destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region WorkflowDefinition

        public static WorkflowDefinitionModel ToModel(this WorkflowDefinition entity)
        {
            return entity.MapTo<WorkflowDefinition, WorkflowDefinitionModel>();
        }

        public static WorkflowDefinition ToEntity(this WorkflowDefinitionModel model)
        {
            return model.MapTo<WorkflowDefinitionModel, WorkflowDefinition>();
        }

        public static WorkflowDefinition ToEntity(this WorkflowDefinitionModel model, WorkflowDefinition destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region EntityAttribute

        public static EntityAttributeModel ToModel(this EntityAttribute entity)
        {
            return entity.MapTo<EntityAttribute, EntityAttributeModel>();
        }

        public static EntityAttribute ToEntity(this EntityAttributeModel model)
        {
            return model.MapTo<EntityAttributeModel, EntityAttribute>();
        }

        public static EntityAttribute ToEntity(this EntityAttributeModel model, EntityAttribute destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Attribute

        public static AttributeModel ToModel(this Core.Domain.Attribute entity)
        {
            return entity.MapTo<Core.Domain.Attribute, AttributeModel>();
        }

        public static Core.Domain.Attribute ToEntity(this AttributeModel model)
        {
            return model.MapTo<AttributeModel, Core.Domain.Attribute>();
        }

        public static Core.Domain.Attribute ToEntity(this AttributeModel model, Core.Domain.Attribute destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region MeterLineItem

        public static MeterLineItemModel ToModel(this MeterLineItem entity)
        {
            return entity.MapTo<MeterLineItem, MeterLineItemModel>();
        }

        public static MeterLineItem ToEntity(this MeterLineItemModel model)
        {
            return model.MapTo<MeterLineItemModel, MeterLineItem>();
        }

        public static MeterLineItem ToEntity(this MeterLineItemModel model, MeterLineItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region MeterGroup

        public static MeterGroupModel ToModel(this MeterGroup entity)
        {
            return entity.MapTo<MeterGroup, MeterGroupModel>();
        }

        public static MeterGroup ToEntity(this MeterGroupModel model)
        {
            return model.MapTo<MeterGroupModel, MeterGroup>();
        }

        public static MeterGroup ToEntity(this MeterGroupModel model, MeterGroup destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Attachment

        public static AttachmentModel ToModel(this Attachment entity)
        {
            return entity.MapTo<Attachment, AttachmentModel>();
        }

        public static Attachment ToEntity(this AttachmentModel model)
        {
            return model.MapTo<AttachmentModel, Attachment>();
        }

        public static Attachment ToEntity(this AttachmentModel model, Attachment destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Meter

        public static MeterModel ToModel(this Meter entity)
        {
            return entity.MapTo<Meter, MeterModel>();
        }

        public static Meter ToEntity(this MeterModel model)
        {
            return model.MapTo<MeterModel, Meter>();
        }

        public static Meter ToEntity(this MeterModel model, Meter destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Location

        public static LocationModel ToModel(this Location entity)
        {
            return entity.MapTo<Location, LocationModel>();
        }

        public static Location ToEntity(this LocationModel model)
        {
            return model.MapTo<LocationModel, Location>();
        }

        public static Location ToEntity(this LocationModel model, Location destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region PermissionRecord

        public static AccessControlModel ToModel(this PermissionRecord entity)
        {
            return entity.MapTo<PermissionRecord, AccessControlModel>();
        }

        public static PermissionRecord ToEntity(this AccessControlModel model)
        {
            return model.MapTo<AccessControlModel, PermissionRecord>();
        }

        public static PermissionRecord ToEntity(this AccessControlModel model, PermissionRecord destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region SecurityGroup

        public static SecurityGroupModel ToModel(this SecurityGroup entity)
        {
            return entity.MapTo<SecurityGroup, SecurityGroupModel>();
        }

        public static SecurityGroup ToEntity(this SecurityGroupModel model)
        {
            return model.MapTo<SecurityGroupModel, SecurityGroup>();
        }

        public static SecurityGroup ToEntity(this SecurityGroupModel model, SecurityGroup destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Address

        public static AddressModel ToModel(this Address entity)
        {
            return entity.MapTo<Address, AddressModel>();
        }

        public static Address ToEntity(this AddressModel model)
        {
            return model.MapTo<AddressModel, Address>();
        }

        public static Address ToEntity(this AddressModel model, Address destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Organization

        public static OrganizationModel ToModel(this Organization entity)
        {
            return entity.MapTo<Organization, OrganizationModel>();
        }

        public static Organization ToEntity(this OrganizationModel model)
        {
            return model.MapTo<OrganizationModel, Organization>();
        }

        public static Organization ToEntity(this OrganizationModel model, Organization destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Site

        public static SiteModel ToModel(this Site entity)
        {
            return entity.MapTo<Site, SiteModel>();
        }

        public static Site ToEntity(this SiteModel model)
        {
            return model.MapTo<SiteModel, Site>();
        }

        public static Site ToEntity(this SiteModel model, Site destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Language

        public static LanguageModel ToModel(this Language entity)
        {
            return entity.MapTo<Language, LanguageModel>();
        }

        public static Language ToEntity(this LanguageModel model)
        {
            return model.MapTo<LanguageModel, Language>();
        }

        public static Language ToEntity(this LanguageModel model, Language destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Currency

        public static CurrencyModel ToModel(this Currency entity)
        {
            return entity.MapTo<Currency, CurrencyModel>();
        }

        public static Currency ToEntity(this CurrencyModel model)
        {
            return model.MapTo<CurrencyModel, Currency>();
        }

        public static Currency ToEntity(this CurrencyModel model, Currency destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Module

        public static ModuleModel ToModel(this Module entity)
        {
            return entity.MapTo<Module, ModuleModel>();
        }

        public static Module ToEntity(this ModuleModel model)
        {
            return model.MapTo<ModuleModel, Module>();
        }

        public static Module ToEntity(this ModuleModel model, Module destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Feature

        public static FeatureModel ToModel(this Feature entity)
        {
            return entity.MapTo<Feature, FeatureModel>();
        }

        public static Feature ToEntity(this FeatureModel model)
        {
            return model.MapTo<FeatureModel, Feature>();
        }

        public static Feature ToEntity(this FeatureModel model, Feature destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region FeatureAction

        public static FeatureActionModel ToModel(this FeatureAction entity)
        {
            return entity.MapTo<FeatureAction, FeatureActionModel>();
        }

        public static FeatureAction ToEntity(this FeatureActionModel model)
        {
            return model.MapTo<FeatureActionModel, FeatureAction>();
        }

        public static FeatureAction ToEntity(this FeatureActionModel model, FeatureAction destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Log

        public static LogModel ToModel(this Log entity)
        {
            return entity.MapTo<Log, LogModel>();
        }

        public static Log ToEntity(this LogModel model)
        {
            return model.MapTo<LogModel, Log>();
        }

        public static Log ToEntity(this LogModel model, Log destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region User

        public static UserModel ToModel(this User entity)
        {
            return entity.MapTo<User, UserModel>();
        }

        public static User ToEntity(this UserModel model)
        {
            return model.MapTo<UserModel, User>();
        }

        public static User ToEntity(this UserModel model, User destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region GeneralSettings

        public static GeneralSettingsModel ToModel(this GeneralSettings entity)
        {
            return entity.MapTo<GeneralSettings, GeneralSettingsModel>();
        }

        public static GeneralSettings ToEntity(this GeneralSettingsModel model)
        {
            return model.MapTo<GeneralSettingsModel, GeneralSettings>();
        }

        public static GeneralSettings ToEntity(this GeneralSettingsModel model, GeneralSettings destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ValueItem

        public static ValueItemModel ToModel(this ValueItem entity)
        {
            return entity.MapTo<ValueItem, ValueItemModel>();
        }

        public static ValueItem ToEntity(this ValueItemModel model)
        {
            return model.MapTo<ValueItemModel, ValueItem>();
        }

        public static ValueItem ToEntity(this ValueItemModel model, ValueItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region ValueItemCategory

        public static ValueItemCategoryModel ToModel(this ValueItemCategory entity)
        {
            return entity.MapTo<ValueItemCategory, ValueItemCategoryModel>();
        }

        public static ValueItemCategory ToEntity(this ValueItemCategoryModel model)
        {
            return model.MapTo<ValueItemCategoryModel, ValueItemCategory>();
        }

        public static ValueItemCategory ToEntity(this ValueItemCategoryModel model, ValueItemCategory destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region UnitOfMeasure

        public static UnitOfMeasureModel ToModel(this UnitOfMeasure entity)
        {
            return entity.MapTo<UnitOfMeasure, UnitOfMeasureModel>();
        }

        public static UnitOfMeasure ToEntity(this UnitOfMeasureModel model)
        {
            return model.MapTo<UnitOfMeasureModel, UnitOfMeasure>();
        }

        public static UnitOfMeasure ToEntity(this UnitOfMeasureModel model, UnitOfMeasure destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region UnitConversion

        public static UnitConversionModel ToModel(this UnitConversion entity)
        {
            return entity.MapTo<UnitConversion, UnitConversionModel>();
        }

        public static UnitConversion ToEntity(this UnitConversionModel model)
        {
            return model.MapTo<UnitConversionModel, UnitConversion>();
        }

        public static UnitConversion ToEntity(this UnitConversionModel model, UnitConversion destination)
        {
            return model.MapTo(destination);
        }

        #endregion
    }
}