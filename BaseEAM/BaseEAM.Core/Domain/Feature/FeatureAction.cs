/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    /// <summary>
    /// A feature action represents an action in feature 
    /// such as: Create, View, Edit, ...
    /// </summary>
    public partial class FeatureAction : BaseEntity
    {
        public string Description { get; set; }
        public int DisplayOrder { get; set; }

        public long? FeatureId { get; set; }
        public virtual Feature Feature { get; set; }
    }
}
