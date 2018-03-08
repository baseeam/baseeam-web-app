/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class CalendarMap : BaseEamEntityTypeConfiguration<Calendar>
    {
        public CalendarMap()
            : base()
        {
            this.ToTable("Calendar");
            this.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
