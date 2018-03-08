/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

using System.ComponentModel.DataAnnotations.Schema;

namespace BaseEAM.Core.Domain
{
    [Table("ItemGroup")]
    public class ItemGroup : BaseEntity
    {
        public string Description { get; set; }
    }
}
