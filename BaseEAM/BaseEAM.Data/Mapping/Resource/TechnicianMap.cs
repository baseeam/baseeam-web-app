/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class TechnicianMap : BaseEamEntityTypeConfiguration<Technician>
    {
        public TechnicianMap()
            : base()
        {
            this.ToTable("Technician");
            this.HasOptional(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
            this.HasOptional(e => e.Calendar)
                .WithMany()
                .HasForeignKey(e => e.CalendarId);
            this.HasOptional(e => e.Shift)
                .WithMany()
                .HasForeignKey(e => e.ShiftId);
            this.HasOptional(e => e.Craft)
                .WithMany()
                .HasForeignKey(e => e.CraftId);
        }
    }
}
