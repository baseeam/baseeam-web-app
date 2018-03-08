/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    /// <summary>
    /// Implement this interface in an entity class to 
    /// explicitly indicate that it stores a hierarchical
    /// structure through its ParentId field.
    /// </summary>
    public interface IHierarchy
    {
        /// <summary>
        /// Concatenation of list of entity ids, beginning from the
        /// root down to this entity.
        /// HierarchyIdPath is to enable efficient tree search.
        /// </summary>
        string HierarchyIdPath { get; set; }

        /// <summary>
        /// Concatenation of list of entity names, beginning from the
        /// root down to this entity.
        /// HierarchyNamePath is to enable efficient tree search.
        /// </summary>
        string HierarchyNamePath { get; set; }

        /// <summary>
        /// The foreign key to the parent entity.
        /// </summary>
        long? ParentId { get; set; }
    }
}
