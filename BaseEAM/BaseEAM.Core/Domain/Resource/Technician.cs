/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Technician : BaseEntity
    {
        public long? UserId { get; set; }
        public virtual User User { get; set; }

        public long? CalendarId { get; set; }
        public virtual Calendar Calendar { get; set; }

        public long? ShiftId { get; set; }
        public virtual Shift Shift { get; set; }

        public long? CraftId { get; set; }
        public virtual Craft Craft { get; set; }

        private ICollection<Team> _teams;
        public virtual ICollection<Team> Teams
        {
            get { return _teams ?? (_teams = new List<Team>()); }
            protected set { _teams = value; }
        }
    }
}
