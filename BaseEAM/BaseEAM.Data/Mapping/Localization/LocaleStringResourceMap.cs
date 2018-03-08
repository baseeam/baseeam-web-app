/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class LocaleStringResourceMap : BaseEamEntityTypeConfiguration<LocaleStringResource>
    {
        public LocaleStringResourceMap()
            :base()
        {
            this.ToTable("LocaleStringResource");
            this.Property(lsr => lsr.ResourceName).IsRequired().HasMaxLength(200);
            this.Property(lsr => lsr.ResourceValue).IsRequired();
            this.HasRequired(lsr => lsr.Language)
                .WithMany(l => l.LocaleStringResources)
                .HasForeignKey(lsr => lsr.LanguageId)
                .WillCascadeOnDelete(true);
        }
    }
}