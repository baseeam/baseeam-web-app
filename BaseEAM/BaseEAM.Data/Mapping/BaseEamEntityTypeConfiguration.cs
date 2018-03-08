/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using System.Data.Entity.ModelConfiguration;

namespace BaseEAM.Data.Mapping
{
    public abstract class BaseEamEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        protected BaseEamEntityTypeConfiguration()
        {
            this.HasKey(e => e.Id);
            this.Property(e => e.Name).HasMaxLength(256);
            this.Property(e => e.CreatedUser).HasMaxLength(128);
            this.Property(e => e.ModifiedUser).HasMaxLength(128);
            this.Property(e => e.Version).IsConcurrencyToken();
            //this.Property(e => e.RowVersion).IsRowVersion();
        }
    }
}
