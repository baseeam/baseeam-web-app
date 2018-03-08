/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PointMap : BaseEamEntityTypeConfiguration<Point>
    {
        public PointMap()
            : base()
        {
            this.ToTable("Point");
            this.HasOptional(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationId);
            this.HasOptional(e => e.Asset)
                .WithMany()
                .HasForeignKey(e => e.AssetId);
            this.HasOptional(e => e.MeterGroup)
                .WithMany()
                .HasForeignKey(e => e.MeterGroupId);            
        }
    }
}
