/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;

namespace BaseEAM.Web.Models
{
    public class MeterEventHistoryModel
    {
        public long Id { get; set; }

        [BaseEamResourceDisplayName("MeterEvent")]
        public long? MeterEventId { get; set; }
        public string MeterEventName { get; set; }

        //Cache AssetId, Location in PM
        [BaseEamResourceDisplayName("Asset")]
        public long? AssetId { get; set; }

        [BaseEamResourceDisplayName("Location")]
        public long? LocationId { get; set; }

    }
}