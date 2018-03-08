/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ShiftPatternMap : BaseEamEntityTypeConfiguration<ShiftPattern>
    {
        public ShiftPatternMap()
            : base()
        {
            this.ToTable("ShiftPattern");
            this.HasOptional(e => e.Shift)
                .WithMany(e => e.ShiftPatterns)
                .HasForeignKey(e => e.ShiftId);
        }
    }
}
