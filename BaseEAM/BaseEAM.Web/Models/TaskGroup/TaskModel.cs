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
    [Validator(typeof(TaskValidator))]
    public class TaskModel : BaseEamEntityModel
    {
        public long? TaskGroupId { get; set; }

        [BaseEamResourceDisplayName("Task.Sequence")]
        public int Sequence { get; set; }

        [BaseEamResourceDisplayName("Task.Description")]
        public string Description { get; set; }

    }
}