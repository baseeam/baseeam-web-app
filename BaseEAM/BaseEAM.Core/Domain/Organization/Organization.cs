/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public partial class Organization : BaseEntity
    {
        public string Description { get; set; }

        private ICollection<Site> _sites;
        public virtual ICollection<Site> Sites
        {
            get { return _sites ?? (_sites = new List<Site>()); }
            protected set { _sites = value; }
        }

        private ICollection<Address> _addresses;
        public virtual ICollection<Address> Addresses
        {
            get { return _addresses ?? (_addresses = new List<Address>()); }
            protected set { _addresses = value; }
        }
    }
}
