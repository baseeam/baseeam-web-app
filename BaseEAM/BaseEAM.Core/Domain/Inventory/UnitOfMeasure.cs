/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

using Dapper.Contrib.Extensions;

namespace BaseEAM.Core.Domain
{
    [Table("UnitOfMeasure")]
    public class UnitOfMeasure : BaseEntity
    {
        public string Abbreviation { get; set; }
        public string Description { get; set; }
    }
}
