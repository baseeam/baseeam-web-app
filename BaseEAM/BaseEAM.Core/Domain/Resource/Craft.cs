/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class Craft : BaseEntity
    {
        public string Description { get; set; }

        /// <summary>
        /// Standard hourly rate
        /// </summary>
        public decimal? StandardRate { get; set; }

        /// <summary>
        /// Overtime hourly rate
        /// </summary>
        public decimal? OvertimeRate { get; set; }
    }
}
