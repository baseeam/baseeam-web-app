/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public partial class SecurityGroup : BaseEntity
    {
        public string Description { get; set; }

        private ICollection<Site> _sites;
        public virtual ICollection<Site> Sites
        {
            get { return _sites ?? (_sites = new List<Site>()); }
            protected set { _sites = value; }
        }

        private ICollection<PermissionRecord> _permissionRecords;
        public virtual ICollection<PermissionRecord> PermissionRecords
        {
            get { return _permissionRecords ?? (_permissionRecords = new List<PermissionRecord>()); }
            protected set { _permissionRecords = value; }
        }

        private ICollection<User> _users;
        public virtual ICollection<User> Users
        {
            get { return _users ?? (_users = new List<User>()); }
            protected set { _users = value; }
        }

        private ICollection<Report> _reports;
        public virtual ICollection<Report> Reports
        {
            get { return _reports ?? (_reports = new List<Report>()); }
            protected set { _reports = value; }
        }

        private ICollection<Visual> _visuals;
        public virtual ICollection<Visual> Visuals
        {
            get { return _visuals ?? (_visuals = new List<Visual>()); }
            protected set { _visuals = value; }
        }
    }
}
