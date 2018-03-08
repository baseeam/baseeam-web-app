/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Core.Domain
{
    public class AssetSparePart : BaseEntity
    {
        public long? AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public long? ItemId { get; set; }
        public virtual Item Item { get; set; }

        /// <summary>
        /// The quantity of parts required for this asset
        /// </summary>
        public decimal? Quantity { get; set; }
    }
}
