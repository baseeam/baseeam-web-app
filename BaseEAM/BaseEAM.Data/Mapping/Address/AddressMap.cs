/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AddressMap : BaseEamEntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            this.ToTable("Address");
            this.Property(e => e.Country).HasMaxLength(256);
            this.Property(e => e.StateProvince).HasMaxLength(256);
            this.Property(e => e.City).HasMaxLength(256);
            this.Property(e => e.Address1).HasMaxLength(256);
            this.Property(e => e.Address2).HasMaxLength(256);
            this.Property(e => e.ZipPostalCode).HasMaxLength(256);
            this.Property(e => e.PhoneNumber).HasMaxLength(256);
            this.Property(e => e.FaxNumber).HasMaxLength(256);
            this.Property(e => e.Email).HasMaxLength(256);
        }
    }
}
