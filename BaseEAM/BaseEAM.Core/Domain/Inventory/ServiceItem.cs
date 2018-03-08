/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class ServiceItem : BaseEntity
    {
        public string Description { get; set; }
        public decimal? UnitPrice { get; set; }

        public long? ItemGroupId { get; set; }
        public virtual ItemGroup ItemGroup { get; set; }

        public bool IsActive { get; set; }
    }
}
