/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class MeterLineItemMap : BaseEamEntityTypeConfiguration<MeterLineItem>
    {
        public MeterLineItemMap()
        {
            this.ToTable("MeterLineItem");
            this.HasOptional(e => e.Meter)
                .WithMany()
                .HasForeignKey(e => e.MeterId);
            this.HasOptional(e => e.MeterGroup)
                .WithMany(e => e.MeterLineItems)
                .HasForeignKey(e => e.MeterGroupId)
                .WillCascadeOnDelete(true);
        }
    }
}
