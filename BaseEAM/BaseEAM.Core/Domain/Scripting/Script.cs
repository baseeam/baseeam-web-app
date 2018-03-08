/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class Script : BaseEntity
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public string SourceCode { get; set; }
    }
}
