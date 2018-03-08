/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PointMeterLineItemMap : BaseEamEntityTypeConfiguration<PointMeterLineItem>
    {
        public PointMeterLineItemMap()
        {
            this.ToTable("PointMeterLineItem");
            this.HasOptional(e => e.Meter)
                .WithMany()
                .HasForeignKey(e => e.MeterId);
            this.HasOptional(e => e.Point)
                .WithMany(e => e.PointMeterLineItems)
                .HasForeignKey(e => e.PointId)
                .WillCascadeOnDelete(true);
            this.Property(e => e.LastReadingValue).HasPrecision(19, 4);
            this.Property(e => e.LastReadingUser).HasMaxLength(64);
        }
    }
}
