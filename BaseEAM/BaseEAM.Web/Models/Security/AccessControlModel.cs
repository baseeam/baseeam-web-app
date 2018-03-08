/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Web.Models
{
    public class AccessControlModel
    {
        public long SecurityGroupId { get; set; }

        public string Name { get; set; }

        public bool HasPermission { get; set; }
    }
}