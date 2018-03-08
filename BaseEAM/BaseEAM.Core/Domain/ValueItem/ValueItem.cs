/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseEAM.Core.Domain
{
    [Table("ValueItem")]
    public partial class ValueItem : BaseEntity
    {
        public long? ValueItemCategoryId { get; set; }
        [Dapper.Contrib.Extensions.Write(false)]
        public virtual ValueItemCategory ValueItemCategory { get; set; }
    }
}
    