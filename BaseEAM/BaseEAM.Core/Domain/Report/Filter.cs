/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    /// <summary>
    /// General definition for a filter.
    /// </summary>
    public class Filter : BaseEntity
    {
        public string ResourceKey { get; set; }
        public int? ControlType { get; set; }
        public int? DataType { get; set; }
        public int? DataSource { get; set; }
        /// <summary>
        /// Specify values when data source = CSV
        /// not including empty value
        /// </summary>
        public string CsvTextList { get; set; }
        public string CsvValueList { get; set; }

        /// <summary>
        /// Specify values when data source = DB
        /// </summary>
        public string DbTable { get; set; }
        public string DbTextColumn { get; set; }
        public string DbValueColumn { get; set; }

        /// <summary>
        /// Specify values when data source = SQL
        /// </summary>
        public string SqlQuery { get; set; }
        public string SqlTextField { get; set; }
        public string SqlValueField { get; set; }

        /// <summary>
        /// Specify values when data source = MVC
        /// </summary>
        public string MvcController { get; set; }
        public string MvcAction { get; set; }
        public string AdditionalField { get; set; }
        public string AdditionalValue { get; set; }

        /// <summary>
        /// Lookup type
        /// </summary>
        public string LookupType { get; set; }
        public string LookupTextField { get; set; }
        public string LookupValueField { get; set; }

        /// <summary>
        /// when data source = DB || MVC, data for the 
        /// list will be load lazily if AutoBind = false
        /// </summary>
        public bool AutoBind { get; set; }
    }
}
