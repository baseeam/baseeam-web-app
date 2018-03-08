/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class UnitConversionMap : BaseEamEntityTypeConfiguration<UnitConversion>
    {
        public UnitConversionMap()
        {
            this.ToTable("UnitConversion");
            this.HasOptional(e => e.FromUnitOfMeasure)
                .WithMany()
                .HasForeignKey(e => e.FromUnitOfMeasureId);
            this.HasOptional(e => e.ToUnitOfMeasure)
                .WithMany()
                .HasForeignKey(e => e.ToUnitOfMeasureId);
            this.Property(e => e.ConversionFactor).HasPrecision(19, 4);
        }
    }
}
