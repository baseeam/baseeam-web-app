/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PMMeterFrequencyMap : BaseEamEntityTypeConfiguration<PMMeterFrequency>
    {
        public PMMeterFrequencyMap()
            : base()
        {
            this.ToTable("PMMeterFrequency");
            this.HasOptional(e => e.PreventiveMaintenance)
                .WithMany(e => e.PMMeterFrequencies)
                .HasForeignKey(e => e.PreventiveMaintenanceId);
            this.HasOptional(e => e.Meter)
                .WithMany()
                .HasForeignKey(e => e.MeterId);
            this.Property(e => e.Frequency).HasPrecision(19, 4);
            this.Property(e => e.EndReading).HasPrecision(19, 4);
            this.Property(e => e.GeneratedReading).HasPrecision(19, 4);
        }
    }
}
