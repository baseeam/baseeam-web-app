/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;

namespace BaseEAM.Web.Models
{
    public class AuditPropertyModel
    {
        [BaseEamResourceDisplayName("AuditProperty.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("AuditProperty.Type")]
        public string Type { get; set; }

        [BaseEamResourceDisplayName("AuditProperty.Current")]
        public string Current { get; set; }

        [BaseEamResourceDisplayName("AuditProperty.Original")]
        public string Original { get; set; }
    }
}