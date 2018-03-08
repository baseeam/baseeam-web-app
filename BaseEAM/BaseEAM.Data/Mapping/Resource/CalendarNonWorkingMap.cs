/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class CalendarNonWorkingMap : BaseEamEntityTypeConfiguration<CalendarNonWorking>
    {
        public CalendarNonWorkingMap()
            : base()
        {
            this.ToTable("CalendarNonWorking");
            this.HasOptional(e => e.Calendar)
                .WithMany(e => e.CalendarNonWorkings)
                .HasForeignKey(e => e.CalendarId)
                .WillCascadeOnDelete(true);
        }
    }
}
