/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Models
{
    public class PointModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Location")]
        public long? LocationId { get; set; }
        public virtual LocationModel Location { get; set; }

        [BaseEamResourceDisplayName("Asset")]
        public long? AssetId { get; set; }
        public AssetModel Asset { get; set; }

        [BaseEamResourceDisplayName("MeterGroup")]
        public long? MeterGroupId { get; set; }
        public MeterGroupModel MeterGroup { get; set; }
    }
}