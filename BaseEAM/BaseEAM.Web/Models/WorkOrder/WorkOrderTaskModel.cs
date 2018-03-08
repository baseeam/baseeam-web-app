/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(WorkOrderTaskValidator))]
    public class WorkOrderTaskModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("WorkOrder")]
        public long? WorkOrderId { get; set; }
        public string WorkOrderName { get; set; }

        [BaseEamResourceDisplayName("TaskGroup")]
        public long? TaskGroupId { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.Number")]
        public string WorkOrderNumber { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.Status")]
        public string WorkOrderStatus { get; set; }

        [BaseEamResourceDisplayName("WorkOrderTask.Sequence")]
        [UIHint("Int32Nullable")]
        public int? Sequence { get; set; }

        [BaseEamResourceDisplayName("WorkOrderTask.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("AssignedUser")]
        public long? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }

        [BaseEamResourceDisplayName("WorkOrderTask.Completed")]
        public bool Completed { get; set; }

        [BaseEamResourceDisplayName("CompletedUser")]
        public long? CompletedUserId { get; set; }
        public string CompletedUserName { get; set; }

        [BaseEamResourceDisplayName("WorkOrderTask.CompletedDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? CompletedDate { get; set; }

        [BaseEamResourceDisplayName("WorkOrderTask.HoursSpent")]
        [UIHint("DecimalNullable")]
        public decimal? HoursSpent { get; set; }

        [BaseEamResourceDisplayName("WorkOrderTask.Result")]
        public TaskResult Result { get; set; }
        public string ResultText { get; set; }

        [BaseEamResourceDisplayName("WorkOrderTask.CompletionNotes")]
        public string CompletionNotes { get; set; }

    }
}