/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PictureMap : BaseEamEntityTypeConfiguration<Picture>
    {
        public PictureMap()
            :base()
        {
            this.ToTable("Picture");
            this.Property(a => a.Extension).HasMaxLength(64);
            this.Property(a => a.EntityType).HasMaxLength(64);
            this.Property(a => a.ContentType).HasMaxLength(64);
        }
    }
}
