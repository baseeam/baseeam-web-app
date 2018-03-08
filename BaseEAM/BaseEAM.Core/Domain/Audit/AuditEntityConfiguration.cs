/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

using Dapper.Contrib.Extensions;

namespace BaseEAM.Core.Domain
{
    [Table("AuditEntityConfiguration")]
    public class AuditEntityConfiguration : BaseEntity
    {
        public string EntityType { get; set; }

        /// <summary>
        /// The list of columns seperated by ','
        /// that will not be audited
        /// </summary>
        public string ExcludedColumns { get; set; }
    }
}
