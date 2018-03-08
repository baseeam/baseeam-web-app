/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AuditEntityConfigurationMap : BaseEamEntityTypeConfiguration<AuditEntityConfiguration>
    {
        public AuditEntityConfigurationMap()
            : base()
        {
            this.ToTable("AuditEntityConfiguration");
            this.Property(s => s.EntityType).HasMaxLength(64);
            this.Property(s => s.ExcludedColumns).HasMaxLength(512);
        }
    }
}
