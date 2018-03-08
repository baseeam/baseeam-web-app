/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(ContactValidator))]
    public class ContactModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Contact.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Contact.Position")]
        public string Position { get; set; }

        [BaseEamResourceDisplayName("Contact.Email")]
        public string Email { get; set; }

        [BaseEamResourceDisplayName("Contact.Phone")]
        public string Phone { get; set; }

        [BaseEamResourceDisplayName("Contact.Fax")]
        public string Fax { get; set; }

        [BaseEamResourceDisplayName("Company")]
        public long? CompanyId { get; set; }
        public CompanyModel Company { get; set; }

        [BaseEamResourceDisplayName("Tenant")]
        public long? TenantId { get; set; }

        [BaseEamResourceDisplayName("Contract")]
        public long? ContractId { get; set; }
    }
}