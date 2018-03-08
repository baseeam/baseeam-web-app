/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class AssignmentGroup : BaseEntity
    {
        public string Description { get; set; }

        private ICollection<AssignmentGroupUser> _assignmentGroupUsers;
        public virtual ICollection<AssignmentGroupUser> AssignmentGroupUsers
        {
            get { return _assignmentGroupUsers ?? (_assignmentGroupUsers = new List<AssignmentGroupUser>()); }
            protected set { _assignmentGroupUsers = value; }
        }
    }
}
