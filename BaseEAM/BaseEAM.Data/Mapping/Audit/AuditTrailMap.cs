/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AuditTrailMap : BaseEamEntityTypeConfiguration<AuditTrail>
    {
        public AuditTrailMap()
            : base()
        {
            this.ToTable("AuditTrail");
        }
    }
}
