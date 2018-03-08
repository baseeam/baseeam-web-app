/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    /// <summary>
    /// A feature represents a sub level menu item.
    /// It comprises many feature actions.
    /// </summary>
    public partial class Feature : BaseEntity
    {
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public string EntityType { get; set; }
        public bool WorkflowEnabled { get; set; }
        public bool ImportEnabled { get; set; }
        public bool AuditTrailEnabled { get; set; }

        public long? ModuleId { get; set; }
        public virtual Module Module { get; set; }

        private ICollection<FeatureAction> _featureActions;
        public virtual ICollection<FeatureAction> FeatureActions
        {
            get { return _featureActions ?? (_featureActions = new List<FeatureAction>()); }
            protected set { _featureActions = value; }
        }
    }
}
