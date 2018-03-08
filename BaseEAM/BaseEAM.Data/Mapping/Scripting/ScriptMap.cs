/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ScriptMap : BaseEamEntityTypeConfiguration<Script>
    {
        public ScriptMap()
            : base()
        {
            this.ToTable("Script");
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.Type).HasMaxLength(64);
        }
    }
}
