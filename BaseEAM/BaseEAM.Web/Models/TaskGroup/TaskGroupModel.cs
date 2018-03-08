/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(TaskGroupValidator))]
    public class TaskGroupModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("TaskGroup.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("TaskGroup.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("TaskGroup.AssetType")]
        public long? AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
    }
}