/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class MeterEventHistoryMap : BaseEamEntityTypeConfiguration<MeterEventHistory>
    {
        public MeterEventHistoryMap()
        {
            this.ToTable("MeterEventHistory");
            this.HasOptional(e => e.MeterEvent)
                .WithMany(e => e.MeterEventHistories)
                .HasForeignKey(e => e.MeterEventId)
                .WillCascadeOnDelete(true);
            this.Property(e => e.GeneratedReading).HasPrecision(19, 4);
        }
    }
}
