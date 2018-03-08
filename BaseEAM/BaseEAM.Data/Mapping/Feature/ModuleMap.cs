/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class ModuleMap : BaseEamEntityTypeConfiguration<Module>
    {
        public ModuleMap()
        {
            this.ToTable("Module");
            this.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
