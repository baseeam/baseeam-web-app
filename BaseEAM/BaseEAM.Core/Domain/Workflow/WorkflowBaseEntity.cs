/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Core.Domain
{
    public class WorkflowBaseEntity : BaseEntity
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }

        /// <summary>
        /// The current workflow assignment
        /// </summary>
        public long? AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }

        public virtual string AssignmentType { get { return ""; } }

        /// <summary>
        /// This money amount is used in the approval process, such as a PO
        /// </summary>
        public virtual decimal? AssignmentAmount { get { return 0; } }

        public long? CreatedUserId { get; set; }
    }

    public enum AssignmentPriority
    {
        Urgent = 0,
        High,
        Medium,
        Low
    }
}
