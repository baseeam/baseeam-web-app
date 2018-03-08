/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class CodeMap : BaseEamEntityTypeConfiguration<Code>
    {
        public CodeMap()
            : base()
        {
            this.ToTable("Code");
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.HierarchyIdPath).HasMaxLength(64);
            this.Property(e => e.CodeType).HasMaxLength(64);
            this.Property(e => e.HierarchyNamePath).HasMaxLength(512);
            this.HasOptional(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId);
        }
    }
}
