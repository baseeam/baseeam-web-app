/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(OrganizationValidator))]
    public class OrganizationModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Organization.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Organization.Description")]
        public string Description { get; set; }

        public InventorySettingsModel InventorySettings { get; set; }

        public ItemCostingType CostingType { get; set; }
    }
}