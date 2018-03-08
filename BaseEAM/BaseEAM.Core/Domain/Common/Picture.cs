/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Core.Domain
{
    public class Picture : BaseEntity
    {
        public byte[] ImageBytes { get; set; }
        public string Extension { get; set; }        
        public string ContentType { get; set; }
        public long? EntityId { get; set; }
        public string EntityType { get; set; }
    }
}
