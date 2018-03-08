/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class WorkOrderItem : BaseEntity, ISyncEntity
    {
        public WorkOrderItem()
        {
            SyncId = Guid.NewGuid().ToString();
        }
        public string SyncId { get; set; }

        public long? WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        public long? StoreId { get; set; }
        public virtual Store Store { get; set; }

        public long? ItemId { get; set; }
        public virtual Item Item { get; set; }

        public long? StoreLocatorId { get; set; }
        public virtual StoreLocator StoreLocator { get; set; }

        public decimal? UnitPrice { get; set; }
        public decimal? PlanQuantity { get; set; }
        public decimal? PlanTotal { get; set; }
        public decimal? ActualQuantity { get; set; }
        public decimal? ActualTotal { get; set; }

        public decimal? PlanToolHours { get; set; }
        public decimal? ToolRate { get; set; }
        public decimal? ActualToolHours { get; set; }
    }
}
