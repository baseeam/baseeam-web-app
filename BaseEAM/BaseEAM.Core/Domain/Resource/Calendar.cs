/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Calendar : BaseEntity
    {
        public string Description { get; set; }

        /// <summary>
        /// Sunday is a work day.
        /// </summary>
        public bool IsSunday { get; set; }
        /// <summary>
        /// Monday is a work day.
        /// </summary>
        public bool IsMonday { get; set; }
        /// <summary>
        /// Tuesday is a work day.
        /// </summary>
        public bool IsTuesday { get; set; }
        /// <summary>
        /// Wednesday is a work day.
        /// </summary>
        public bool IsWednesday { get; set; }
        /// <summary>
        /// Thursday is a work day.
        /// </summary>
        public bool IsThursday { get; set; }
        /// <summary>
        /// Friday is a work day.
        /// </summary>
        public bool IsFriday { get; set; }
        /// <summary>
        /// Saturday is a work day.
        /// </summary>
        public bool IsSaturday { get; set; }

        private ICollection<CalendarNonWorking> _calendarNonWorkings;
        public virtual ICollection<CalendarNonWorking> CalendarNonWorkings
        {
            get { return _calendarNonWorkings ?? (_calendarNonWorkings = new List<CalendarNonWorking>()); }
            protected set { _calendarNonWorkings = value; }
        }
    }
}
