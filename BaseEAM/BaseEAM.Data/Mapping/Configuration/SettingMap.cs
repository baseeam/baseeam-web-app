/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class SettingMap : BaseEamEntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            this.ToTable("Setting");
            this.Property(s => s.Name).IsRequired().HasMaxLength(256);
            this.Property(s => s.Value).IsRequired().HasMaxLength(2048);
        }
    }
}