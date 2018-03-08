/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ContractPriceItemMap : BaseEamEntityTypeConfiguration<ContractPriceItem>
    {
        public ContractPriceItemMap()
            : base()
        {
            this.ToTable("ContractPriceItem");
            this.Property(e => e.ContractedPrice).HasPrecision(19, 4);
            this.HasOptional(e => e.Contract)
                .WithMany(e => e.ContractPriceItems)
                .HasForeignKey(e => e.ContractId);
            this.HasOptional(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId);
        }
    }
}
