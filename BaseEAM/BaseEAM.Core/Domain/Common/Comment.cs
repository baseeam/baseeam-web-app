/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Core.Domain
{
    public class Comment : BaseEntity
    {
        public long? EntityId { get; set; }
        public string EntityType { get; set; }
        public string Message { get; set; }
    }
}
