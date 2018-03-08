/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class EntityAttribute : BaseEntity
    {
        public long? EntityId { get; set; }
        public string EntityType { get; set; }
        public int? DisplayOrder { get; set; }
        public string Value { get; set; }
        public bool IsRequiredField { get; set; }

        public long? AttributeId { get; set; }
        public virtual Attribute Attribute { get; set; }
    }
}
