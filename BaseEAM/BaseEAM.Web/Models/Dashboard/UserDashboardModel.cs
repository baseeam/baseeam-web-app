/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;

namespace BaseEAM.Web.Models
{
    public class UserDashboardModel
    {
        [BaseEamResourceDisplayName("User")]
        public string UserId { get; set; }

        [BaseEamResourceDisplayName("UserDashboard.DashboardLayoutType")]
        public DashboardLayoutType DashboardLayoutType { get; set; }

        [BaseEamResourceDisplayName("UserDashboard.RegionCount")]
        public int? RegionCount { get; set; }
    }
}