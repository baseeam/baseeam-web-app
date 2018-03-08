/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class ReportColumn : BaseEntity
    {
        public long? ReportId { get; set; }
        public virtual Report Report { get; set; }

        public int? DisplayOrder { get; set; }

        /// <summary>
        /// The column name return from the query's result
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// These are the types for model field types of Kendo UI Grid
        /// string
        /// number
        /// boolean
        /// date
        /// </summary>
        public string DataType { get; set; }
        public string FormatString { get; set; }
        public string ResourceKey { get; set; }
    }
}
