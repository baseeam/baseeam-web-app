/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class AutoNumber : BaseEntity
    {
        public string EntityType { get; set; }
        public string FormatString { get; set; }
        public int LastNumber { get; set; }
    }
}
