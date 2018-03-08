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
    [Validator(typeof(CompanyValidator))]
    public class CompanyModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Company.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Company.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Company.Website")]
        public string Website { get; set; }

        [BaseEamResourceDisplayName("Company.CompanyType")]
        public long? CompanyTypeId { get; set; }
        public ValueItemModel CompanyType { get; set; }

        [BaseEamResourceDisplayName("Company.Currency")]
        public long? CurrencyId { get; set; }
        public CurrencyModel Currency { get; set; }

    }
}