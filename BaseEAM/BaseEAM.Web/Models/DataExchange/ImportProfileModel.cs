/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(ImportProfileValidator))]
    public class ImportProfileModel : BaseEamEntityModel
    {
        public int? FileTypeId { get; set; }

        [BaseEamResourceDisplayName("ImportProfile.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("EntityType")]
        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("ImportProfile.LastRunStartDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? LastRunStartDateTime { get; set; }

        [BaseEamResourceDisplayName("ImportProfile.LastRunEndDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? LastRunEndDateTime { get; set; }

        [BaseEamResourceDisplayName("ImportProfile.ImportFileName")]
        public string ImportFileName { get; set; }
        public long? ImportFileId { get; set; }

        [BaseEamResourceDisplayName("ImportProfile.LogFileName")]
        public string LogFileName { get; set; }
    }
}