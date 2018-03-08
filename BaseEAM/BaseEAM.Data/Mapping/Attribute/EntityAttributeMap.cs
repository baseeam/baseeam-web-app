/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class EntityAttributeMap : BaseEamEntityTypeConfiguration<EntityAttribute>
    {
        public EntityAttributeMap()
        {
            this.ToTable("EntityAttribute");
            this.Property(s => s.EntityType).HasMaxLength(64);
            this.Property(s => s.Value).HasMaxLength(256);
            this.HasOptional(e => e.Attribute)
                .WithMany()
                .HasForeignKey(e => e.AttributeId);
        }
    }
}
