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
    [Validator(typeof(AuditEntityConfigurationValidator))]
    public class AuditEntityConfigurationModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("EntityType")]
        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("AuditEntityConfiguration.ExcludedColumn")]
        public string ExcludedColumn { get; set; }

        [BaseEamResourceDisplayName("AuditEntityConfiguration.ExcludedColumns")]
        public string ExcludedColumns { get; set; }
    }
}