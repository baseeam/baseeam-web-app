/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class AuditTrail : BaseEntity
    {
        public DateTime Date { get; set; }
        public string LogXml { get; set; }
    }
}
