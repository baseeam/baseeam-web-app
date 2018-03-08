/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    [Table("Location")]
    public class Location : BaseEntity, IHierarchy
    {
        public string HierarchyIdPath { get; set; }
        public string HierarchyNamePath { get; set; }

        public long? ParentId { get; set; }
        [Write(false)]
        public virtual Location Parent { get; set; }

        private ICollection<Location> _children;
        [Write(false)]
        public virtual ICollection<Location> Children
        {
            get { return _children ?? (_children = new List<Location>()); }
            protected set { _children = value; }
        }

        public long? SiteId { get; set; }
        [Write(false)]
        public virtual Site Site { get; set; }

        public long? LocationTypeId { get; set; }
        [Write(false)]
        public virtual ValueItem LocationType { get; set; }

        public long? LocationStatusId { get; set; }
        [Write(false)]
        public virtual ValueItem LocationStatus { get; set; }

        public long? AddressId { get; set; }
        [Write(false)]
        public virtual Address Address { get; set; }
    }
}
