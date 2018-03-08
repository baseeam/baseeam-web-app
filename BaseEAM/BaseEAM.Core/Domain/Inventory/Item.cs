/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using Dapper.Contrib.Extensions;

namespace BaseEAM.Core.Domain
{
    [Table("Item")]
    public class Item : BaseEntity
    {
        public string Description { get; set; }
        public string Barcode { get; set; }
        public decimal? UnitPrice { get; set; }

        public long? ManufacturerId { get; set; }

        [Write(false)]
        public virtual Company Manufacturer { get; set; }

        public long? ItemGroupId { get; set; }

        [Write(false)]
        public virtual ItemGroup ItemGroup { get; set; }

        public long? UnitOfMeasureId { get; set; }

        [Write(false)]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        public long? ItemStatusId { get; set; }

        [Write(false)]
        public virtual ValueItem ItemStatus { get; set; }

        public int? ItemCategory { get; set; }
    }
}
