/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ContractTermMap : BaseEamEntityTypeConfiguration<ContractTerm>
    {
        public ContractTermMap()
            : base()
        {
            this.ToTable("ContractTerm");
            this.Property(e => e.Description).HasMaxLength(512);
            this.HasOptional(e => e.Contract)
                .WithMany(e => e.ContractTerms)
                .HasForeignKey(e => e.ContractId);
        }
    }
}
