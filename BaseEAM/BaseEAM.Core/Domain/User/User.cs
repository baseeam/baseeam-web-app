/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    /// <summary>
    /// Represents a user
    /// </summary>
    public partial class User : WorkflowBaseEntity
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public User()
        {
            this.UserGuid = Guid.NewGuid();
            this.PasswordFormat = PasswordFormat.Clear;
        }

        /// <summary>
        /// The user Guid is used to generate authentication token
        /// </summary>
        public Guid UserGuid { get; set; }

        public string LoginName { get; set; }
        public string LoginPassword { get; set; }

        public string Email { get; set; }

        public long PasswordFormatId { get; set; }
        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }

        public string PasswordSalt { get; set; }

        public string TimeZoneId { get; set; }

        /// <summary>
        /// A value indicating whether the user is active
        /// </summary>
        public bool Active { get; set; }

        public int UserType { get; set; }

        /// <summary>
        /// A value indicating whether the user account is system, 
        /// like 'BACKGROUND_TASK'
        /// </summary>
        public bool IsSystemAccount { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string AddressCountry { get; set; }
        public string AddressState { get; set; }
        public string AddressCity { get; set; }
        public string Address { get; set; }        
        public string Phone { get; set; }
        public string Cellphone { get; set; }
        public string Fax { get; set; }

        public bool IsPasswordChangeRequired { get; set; }
        public DateTime? PasswordLastChanged { get; set; }

        public int? FailedLoginAttempts { get; set; }

        /// <summary>
        /// A flag that indicates whether
        /// this user has been banned from the system.
        /// This can be set manually by the administrator, or
        /// it can be set when this user's failed login attempts
        /// exceeds the configured maximum.
        /// </summary>
        public bool IsBanned { get; set; }
        public DateTime? BannedDateFrom { get; set; }

        public bool IsActiveDirectoryUser { get; set; }
        public string ActiveDirectoryDomain { get; set; }

        public DateTime? LastLoginTime { get; set; }
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets the flag indicates that user can access the WebApi
        /// </summary>
        public bool WebApiEnabled { get; set; }

        /// <summary>
        /// HMAC public key
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// HMAC secret key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Time of the last WebApi request
        /// </summary>
        public DateTime? LastApiRequest { get; set; }

        public decimal? POApprovalLimit { get; set; }

        /// <summary>
        /// The supervisor of this user.
        /// </summary>
        public long? SupervisorId { get; set; }
        public virtual User Supervisor { get; set; }

        public long? LanguageId { get; set; }
        public virtual Language Language { get; set; }

        public long? DefaultSiteId { get; set; }
        public virtual Site DefaultSite { get; set; }

        private ICollection<UserPasswordHistory> _passwordHistory;
        public virtual ICollection<UserPasswordHistory> PasswordHistory
        {
            get { return _passwordHistory ?? (_passwordHistory = new List<UserPasswordHistory>()); }
            protected set { _passwordHistory = value; }
        }

        private ICollection<SecurityGroup> _securityGroups;
        public virtual ICollection<SecurityGroup> SecurityGroups
        {
            get { return _securityGroups ?? (_securityGroups = new List<SecurityGroup>()); }
            protected set { _securityGroups = value; }
        }

        private ICollection<UserDevice> _userDevices;
        public virtual ICollection<UserDevice> UserDevices
        {
            get { return _userDevices ?? (_userDevices = new List<UserDevice>()); }
            protected set { _userDevices = value; }
        }
    }

    public enum UserType
    {
        Staff = 0,
        Tenant
    }
}
