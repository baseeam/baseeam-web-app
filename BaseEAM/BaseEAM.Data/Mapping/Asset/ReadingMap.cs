/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ReadingMap : BaseEamEntityTypeConfiguration<Reading>
    {
        public ReadingMap()
            : base()
        {
            this.ToTable("Reading");
            this.HasOptional(e => e.PointMeterLineItem)
                .WithMany(e => e.Readings)
                .HasForeignKey(e => e.PointMeterLineItemId);
            this.HasOptional(e => e.WorkOrder)
                .WithMany()
                .HasForeignKey(e => e.WorkOrderId);
            this.Property(e => e.ReadingValue).HasPrecision(19, 4);
        }
    }
}
