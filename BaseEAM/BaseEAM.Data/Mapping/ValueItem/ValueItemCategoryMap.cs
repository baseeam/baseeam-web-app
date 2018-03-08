/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class ValueItemCategoryMap : BaseEamEntityTypeConfiguration<ValueItemCategory>
    {
        public ValueItemCategoryMap()
        {
            this.ToTable("ValueItemCategory");
        }
    }
}
