/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public partial class PermissionRecord : BaseEntity
    {
        public long? ModuleId { get; set; }
        public virtual Module Module { get; set; }

        public long? FeatureId { get; set; }
        public virtual Feature Feature { get; set; }

        public long? FeatureActionId { get; set; }
        public virtual FeatureAction FeatureAction { get; set; }

        private ICollection<SecurityGroup> _securityGroups;
        public virtual ICollection<SecurityGroup> SecurityGroups
        {
            get { return _securityGroups ?? (_securityGroups = new List<SecurityGroup>()); }
            protected set { _securityGroups = value; }
        }
    }
}
