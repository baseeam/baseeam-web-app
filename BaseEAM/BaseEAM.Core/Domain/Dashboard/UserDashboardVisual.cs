/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class UserDashboardVisual : BaseEntity
    {
        public long? UserDashboardId { get; set; }
        public virtual UserDashboard UserDashboard { get; set; }

        public int? CellId { get; set; }

        public long? VisualId { get; set; }
        public virtual Visual Visual { get; set; }
    }
}
