/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Core.Domain
{
    public class Task : BaseEntity
    {
        public int Sequence { get; set; }
        public string Description { get; set; }

        public long? TaskGroupId { get; set; }
        public virtual TaskGroup TaskGroup { get; set; }
    }
}
