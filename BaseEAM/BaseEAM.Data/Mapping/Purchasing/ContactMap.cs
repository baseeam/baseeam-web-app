/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ContactMap : BaseEamEntityTypeConfiguration<Contact>
    {
        public ContactMap()
            : base()
        {
            this.ToTable("Contact");
            this.Property(e => e.Position).HasMaxLength(64);
            this.Property(e => e.Email).HasMaxLength(64);
            this.Property(e => e.Phone).HasMaxLength(64);
            this.Property(e => e.Fax).HasMaxLength(64);
            this.HasOptional(e => e.Company)
                .WithMany(e => e.Contacts)
                .HasForeignKey(e => e.CompanyId);
            this.HasOptional(e => e.Contract)
                .WithMany(e => e.Contacts)
                .HasForeignKey(e => e.ContractId);
        }
    }
}
