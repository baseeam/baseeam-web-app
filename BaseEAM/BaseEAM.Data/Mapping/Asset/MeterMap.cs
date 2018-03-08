/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class MeterMap : BaseEamEntityTypeConfiguration<Meter>
    {
        public MeterMap()
        {
            this.ToTable("Meter");
            this.Property(s => s.Description).HasMaxLength(512);
            this.HasOptional(e => e.MeterType)
                .WithMany()
                .HasForeignKey(e => e.MeterTypeId);
            this.HasOptional(e => e.UnitOfMeasure)
                .WithMany()
                .HasForeignKey(e => e.UnitOfMeasureId);
        }
    }
}
