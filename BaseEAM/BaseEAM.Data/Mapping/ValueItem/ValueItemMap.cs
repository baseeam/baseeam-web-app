/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class ValueItemMap : BaseEamEntityTypeConfiguration<ValueItem>
    {
        public ValueItemMap()
        {
            this.ToTable("ValueItem");
            this.HasOptional(e => e.ValueItemCategory)
                .WithMany(e => e.ValueItems)
                .HasForeignKey(e => e.ValueItemCategoryId)
                .WillCascadeOnDelete(true);
        }
    }
}
