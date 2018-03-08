/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class WorkOrderMap : BaseEamWorkflowEntityTypeConfiguration<WorkOrder>
    {
        public WorkOrderMap()
            : base()
        {
            this.ToTable("WorkOrder");
            this.Property(e => e.SyncId).HasMaxLength(64);
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.HierarchyIdPath).HasMaxLength(64);
            this.Property(e => e.HierarchyNamePath).HasMaxLength(512);
            this.HasOptional(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId);
            this.HasOptional(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId);
            this.HasOptional(e => e.Asset)
                .WithMany()
                .HasForeignKey(e => e.AssetId);
            this.HasOptional(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationId);
            this.HasOptional(e => e.WorkCategory)
                .WithMany()
                .HasForeignKey(e => e.WorkCategoryId);
            this.HasOptional(e => e.WorkType)
                .WithMany()
                .HasForeignKey(e => e.WorkTypeId);
            this.HasOptional(e => e.FailureGroup)
                .WithMany()
                .HasForeignKey(e => e.FailureGroupId);
            this.HasOptional(e => e.ServiceRequest)
                .WithMany(e => e.WorkOrders)
                .HasForeignKey(e => e.ServiceRequestId);
            this.HasOptional(e => e.PreventiveMaintenance)
                .WithMany(e => e.WorkOrders)
                .HasForeignKey(e => e.PreventiveMaintenanceId);
            this.HasOptional(e => e.Contract)
                .WithMany(e => e.WorkOrders)
                .HasForeignKey(e => e.ContractId);

            this.Property(e => e.RequestorName).HasMaxLength(64);
            this.Property(e => e.RequestorEmail).HasMaxLength(64);
            this.Property(e => e.RequestorPhone).HasMaxLength(64);

            this.HasOptional(e => e.Supervisor)
                .WithMany()
                .HasForeignKey(e => e.SupervisorId);
            this.HasOptional(e => e.TaskGroup)
                .WithMany()
                .HasForeignKey(e => e.TaskGroupId);

            this.HasOptional(e => e.ActualFailureGroup)
                .WithMany()
                .HasForeignKey(e => e.ActualFailureGroupId);
            this.HasOptional(e => e.ActualProblem)
                .WithMany()
                .HasForeignKey(e => e.ActualProblemId);
            this.HasOptional(e => e.ActualCause)
                .WithMany()
                .HasForeignKey(e => e.ActualCauseId);
            this.HasOptional(e => e.Resolution)
                .WithMany()
                .HasForeignKey(e => e.ResolutionId);
            this.Property(e => e.ResolutionNotes).HasMaxLength(512);
        }
    }
}
