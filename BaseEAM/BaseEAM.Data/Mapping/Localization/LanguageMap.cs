/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class LanguageMap : BaseEamEntityTypeConfiguration<Language>
    {
        public LanguageMap()
            :base()
        {
            this.ToTable("Language");
            this.Property(l => l.LanguageCulture).HasMaxLength(20);
            this.Property(l => l.FlagImageFileName).HasMaxLength(50);
        }
    }
}