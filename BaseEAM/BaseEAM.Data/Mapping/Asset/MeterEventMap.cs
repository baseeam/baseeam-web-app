/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class MeterEventMap : BaseEamEntityTypeConfiguration<MeterEvent>
    {
        public MeterEventMap()
            : base()
        {
            this.ToTable("MeterEvent");
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.UpperLimit).HasPrecision(19, 4);
            this.Property(e => e.LowerLimit).HasPrecision(19, 4);
            this.HasOptional(e => e.Meter)
                .WithMany()
                .HasForeignKey(e => e.MeterId);
            this.HasOptional(e => e.Point)
                .WithMany()
                .HasForeignKey(e => e.PointId);
        }
    }
}
