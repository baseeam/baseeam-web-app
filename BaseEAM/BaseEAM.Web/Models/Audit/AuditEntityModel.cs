/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;

namespace BaseEAM.Web.Models
{
    public class AuditEntityModel
    {
        public long? AuditTrailId { get; set; }

        [BaseEamResourceDisplayName("AuditEntity.Action")]
        public string Action { get; set; }

        [BaseEamResourceDisplayName("EntityType")]
        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("AuditEntity.Key")]
        public string Key { get; set; }

        [BaseEamResourceDisplayName("AuditEntity.Value")]
        public string Value { get; set; }
    }
}