/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class StoreLocatorMap : BaseEamEntityTypeConfiguration<StoreLocator>
    {
        public StoreLocatorMap()
        {
            this.ToTable("StoreLocator");
            this.HasOptional(e => e.Store)
                .WithMany(e => e.StoreLocators)
                .HasForeignKey(e => e.StoreId);
        }
    }
}
