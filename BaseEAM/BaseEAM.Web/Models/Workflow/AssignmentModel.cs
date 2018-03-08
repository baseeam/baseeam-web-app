/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using System;

namespace BaseEAM.Web.Models
{
    public class AssignmentModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Assignment.Name")]
        public string Name { get; set; }

        public long? EntityId { get; set; }
        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("Number")]
        public string Number { get; set; }

        [BaseEamResourceDisplayName("Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Priority")]
        public int? Priority { get; set; }
        public string PriorityText { get; set; }

        [BaseEamResourceDisplayName("Assignment.AssignmentType")]
        public string AssignmentType { get; set; }

        [BaseEamResourceDisplayName("Assignment.AssignmentAmount")]
        public decimal? AssignmentAmount { get; set; }

        [BaseEamResourceDisplayName("Assignment.ExpectedStartDateTime")]
        public DateTime? ExpectedStartDateTime { get; set; }

        [BaseEamResourceDisplayName("Assignment.DueDateTime")]
        public DateTime? DueDateTime { get; set; }
    }
}