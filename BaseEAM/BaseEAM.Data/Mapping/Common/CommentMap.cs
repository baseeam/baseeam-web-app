/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class CommentMap : BaseEamEntityTypeConfiguration<Comment>
    {
        public CommentMap()
            :base()
        {
            this.ToTable("Comment");
            this.Property(a => a.EntityType).HasMaxLength(64);
            this.Property(a => a.Message).HasMaxLength(512);
        }
    }
}
