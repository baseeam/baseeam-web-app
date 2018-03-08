/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class CurrencyMap : BaseEamEntityTypeConfiguration<Currency>
    {
        public CurrencyMap()
            : base()
        {
            this.ToTable("Currency");
            this.Property(e => e.CurrencyCode).HasMaxLength(64);
            this.Property(e => e.DisplayLocale).HasMaxLength(64);
            this.Property(e => e.CustomFormatting).HasMaxLength(64);
            this.Property(e => e.Description).HasMaxLength(128);
            this.Ignore(e => e.CurrencySymbol);
            this.Property(e => e.Rate).HasPrecision(19, 4);
        }
    }
}
