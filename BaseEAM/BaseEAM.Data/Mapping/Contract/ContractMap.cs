/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ContractMap : BaseEamWorkflowEntityTypeConfiguration<Contract>
    {
        public ContractMap()
            : base()
        {
            this.ToTable("Contract");
            this.Property(e => e.Total).HasPrecision(19, 4);
            this.HasOptional(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId);
            this.HasOptional(e => e.WorkCategory)
                .WithMany()
                .HasForeignKey(e => e.WorkCategoryId);
            this.HasOptional(e => e.WorkType)
                .WithMany()
                .HasForeignKey(e => e.WorkTypeId);
            this.HasOptional(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId);
            this.HasOptional(e => e.Supervisor)
                .WithMany()
                .HasForeignKey(e => e.SupervisorId);
        }
    }
}
