/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class StoreLocatorReservationMap : BaseEamEntityTypeConfiguration<StoreLocatorReservation>
    {
        public StoreLocatorReservationMap()
        {
            this.ToTable("StoreLocatorReservation");
            this.Property(e => e.QuantityReserved).HasPrecision(19, 4);
            this.HasOptional(e => e.StoreLocator)
                .WithMany()
                .HasForeignKey(e => e.StoreLocatorId);
            this.HasOptional(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId);
        }
    }
}
