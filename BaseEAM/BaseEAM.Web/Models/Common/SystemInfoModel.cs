/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using System;

namespace BaseEAM.Web.Models
{
    public class SystemInfoModel
    {
        [BaseEamResourceDisplayName("SystemInfo.BaseEamVersion")]
        public string BaseEamVersion { get; set; }

        [BaseEamResourceDisplayName("SystemInfo.ServerLocalTime")]
        public DateTime ServerLocalTime { get; set; }

        [BaseEamResourceDisplayName("SystemInfo.ServerTimeZone")]
        public string ServerTimeZone { get; set; }

        [BaseEamResourceDisplayName("SystemInfo.UTCTime")]
        public DateTime UtcTime { get; set; }

        [BaseEamResourceDisplayName("SystemInfo.CurrentUserTime")]
        public DateTime CurrentUserTime { get; set; }
    }
}