/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class CompanyMap : BaseEamEntityTypeConfiguration<Company>
    {
        public CompanyMap()
            : base()
        {
            this.ToTable("Company");
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.Website).HasMaxLength(128);
            this.HasOptional(e => e.CompanyType)
                .WithMany()
                .HasForeignKey(e => e.CompanyTypeId);
            this.HasOptional(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId);
            this.HasMany(e => e.Addresses)
                .WithMany()
                .Map(e =>
                {
                    e.MapLeftKey("CompanyId");
                    e.MapRightKey("AddressId");
                    e.ToTable("Company_Address");
                });
        }
    }
}
