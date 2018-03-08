/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ShiftMap : BaseEamEntityTypeConfiguration<Shift>
    {
        public ShiftMap()
            : base()
        {
            this.ToTable("Shift");
            this.Property(e => e.Description).HasMaxLength(512);
            this.HasOptional(e => e.Calendar)
                .WithMany()
                .HasForeignKey(e => e.CalendarId);
        }
    }
}
