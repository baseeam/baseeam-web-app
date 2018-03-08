/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class UserMap : BaseEamWorkflowEntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("User");
            this.Property(e => e.LoginName).HasMaxLength(128);
            this.Property(e => e.LoginPassword).HasMaxLength(128);
            this.Property(e => e.Email).HasMaxLength(128);
            this.Property(e => e.TimeZoneId).HasMaxLength(128);
            this.Property(e => e.AddressCountry).HasMaxLength(256);
            this.Property(e => e.AddressState).HasMaxLength(256);
            this.Property(e => e.AddressCity).HasMaxLength(256);
            this.Property(e => e.Address).HasMaxLength(256);
            this.Property(e => e.Phone).HasMaxLength(128);
            this.Property(e => e.Cellphone).HasMaxLength(128);
            this.Property(e => e.Fax).HasMaxLength(128);
            this.Property(u => u.ActiveDirectoryDomain).HasMaxLength(128);
            this.Property(u => u.LastIpAddress).HasMaxLength(128);
            this.Property(u => u.PublicKey).HasMaxLength(256);
            this.Property(u => u.SecretKey).HasMaxLength(256);
            this.Property(e => e.POApprovalLimit).HasPrecision(19, 4);
            this.HasOptional(e => e.Supervisor)
                .WithMany()
                .HasForeignKey(e => e.SupervisorId);
            this.HasOptional(e => e.Language)
                .WithMany()
                .HasForeignKey(e => e.LanguageId);
            this.HasOptional(e => e.DefaultSite)
                .WithMany()
                .HasForeignKey(e => e.DefaultSiteId);
        }
    }
}
