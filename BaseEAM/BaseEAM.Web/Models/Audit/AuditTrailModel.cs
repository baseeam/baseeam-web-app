/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using System;

namespace BaseEAM.Web.Models
{
    public class AuditTrailModel
    {
        public long Id { get; set; }

        [BaseEamResourceDisplayName("AuditLog.UserName")]
        public string UserName { get; set; }

        [BaseEamResourceDisplayName("AuditTrail.Date")]
        public DateTime Date { get; set; }
    }
}