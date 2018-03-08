/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class CalendarNonWorking : BaseEntity
    {
        public long? CalendarId { get; set; }
        public virtual Calendar Calendar { get; set; }

        public DateTime? NonWorkingDate { get; set; }
    }
}
