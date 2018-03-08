/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Code : BaseEntity, IHierarchy
    {
        public string Description { get; set; }
        public string HierarchyIdPath { get; set; }
        public string HierarchyNamePath { get; set; }

        public long? ParentId { get; set; }
        public virtual Code Parent { get; set; }

        private ICollection<Code> _children;
        public virtual ICollection<Code> Children
        {
            get { return _children ?? (_children = new List<Code>()); }
            protected set { _children = value; }
        }

        public string CodeType { get; set; }
    }
}
