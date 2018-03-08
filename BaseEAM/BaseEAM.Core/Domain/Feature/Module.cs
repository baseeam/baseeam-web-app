/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    /// <summary>
    /// A module represents a top level menu item.
    /// It comprises many features.
    /// </summary>
    public partial class Module : BaseEntity
    {
        public string Description { get; set; }
        public int DisplayOrder { get; set; }

        private ICollection<Feature> _features;
        public virtual ICollection<Feature> Features
        {
            get { return _features ?? (_features = new List<Feature>()); }
            protected set { _features = value; }
        }
    }
}
