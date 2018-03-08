/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PreventiveMaintenanceMap : BaseEamEntityTypeConfiguration<PreventiveMaintenance>
    {
        public PreventiveMaintenanceMap()
        {
            this.ToTable("PreventiveMaintenance");
            this.Property(e => e.Number).HasMaxLength(64);
            this.Property(e => e.Description).HasMaxLength(512);
            this.HasOptional(e => e.PMStatus)
                .WithMany()
                .HasForeignKey(e => e.PMStatusId);
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
            this.HasOptional(e => e.Supervisor)
                .WithMany()
                .HasForeignKey(e => e.SupervisorId);
            this.HasOptional(e => e.TaskGroup)
                .WithMany()
                .HasForeignKey(e => e.TaskGroupId);
            this.HasOptional(e => e.Contract)
                .WithMany(e => e.PreventiveMaintenances)
                .HasForeignKey(e => e.ContractId);
            this.HasMany(e => e.MeterEvents)
                .WithMany()
                .Map(e =>
                {
                    e.MapLeftKey("PreventiveMaintenanceId");
                    e.MapRightKey("MeterEventId");
                    e.ToTable("PM_MeterEvent");
                });
        }
    }
}
