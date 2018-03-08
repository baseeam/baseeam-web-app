/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    [Table("Company")]
    public class Company : BaseEntity
    {
        public string Description { get; set; }
        public string Website { get; set; }

        public long? CompanyTypeId { get; set; }
        [Write(false)]
        public virtual ValueItem CompanyType { get; set; }

        public long? CurrencyId { get; set; }
        [Write(false)]
        public virtual Currency Currency { get; set; }

        private ICollection<Contact> _contacts;
        [Write(false)]
        public virtual ICollection<Contact> Contacts
        {
            get { return _contacts ?? (_contacts = new List<Contact>()); }
            protected set { _contacts = value; }
        }

        private ICollection<Address> _addresses;
        [Write(false)]
        public virtual ICollection<Address> Addresses
        {
            get { return _addresses ?? (_addresses = new List<Address>()); }
            protected set { _addresses = value; }
        }
    }
}
