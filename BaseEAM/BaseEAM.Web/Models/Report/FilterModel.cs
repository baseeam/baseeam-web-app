/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(FilterValidator))]
    public class FilterModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Filter.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Filter.ResourceKey")]
        public string ResourceKey { get; set; }

        [BaseEamResourceDisplayName("Filter.ControlType")]
        public FieldControlType ControlType { get; set; }
        public string ControlTypeText { get; set; }

        [BaseEamResourceDisplayName("Filter.DataType")]
        public FieldDataType DataType { get; set; }
        public string DataTypeText { get; set; }

        [BaseEamResourceDisplayName("Filter.DataSource")]
        public FieldDataSource DataSource { get; set; }
        public string DataSourceText { get; set; }

        /// <summary>
        /// Specify values when data source = CSV
        /// not including empty value
        /// </summary>
        [BaseEamResourceDisplayName("Filter.CsvTextList")]
        public string CsvTextList { get; set; }

        [BaseEamResourceDisplayName("Filter.CsvValueList")]
        public string CsvValueList { get; set; }

        /// <summary>
        /// Specify values when data source = DB
        /// </summary>
        [BaseEamResourceDisplayName("Filter.DbTable")]
        public string DbTable { get; set; }

        [BaseEamResourceDisplayName("Filter.DbTextColumn")]
        public string DbTextColumn { get; set; }

        [BaseEamResourceDisplayName("Filter.DbValueColumn")]
        public string DbValueColumn { get; set; }

        /// <summary>
        /// Specify values when data source = SQL
        /// </summary>
        [BaseEamResourceDisplayName("Filter.SqlQuery")]
        public string SqlQuery { get; set; }

        [BaseEamResourceDisplayName("Filter.SqlTextField")]
        public string SqlTextField { get; set; }

        [BaseEamResourceDisplayName("Filter.SqlValueField")]
        public string SqlValueField { get; set; }

        /// <summary>
        /// Specify values when data source = MVC
        /// </summary>
        [BaseEamResourceDisplayName("Filter.MvcController")]
        public string MvcController { get; set; }

        [BaseEamResourceDisplayName("Filter.MvcAction")]
        public string MvcAction { get; set; }

        [BaseEamResourceDisplayName("Filter.AdditionalField")]
        public string AdditionalField { get; set; }

        [BaseEamResourceDisplayName("Filter.AdditionalValue")]
        public string AdditionalValue { get; set; }

        [BaseEamResourceDisplayName("Filter.LookupType")]
        public string LookupType { get; set; }

        [BaseEamResourceDisplayName("Filter.LookupTextField")]
        public string LookupTextField { get; set; }

        [BaseEamResourceDisplayName("Filter.LookupValueField")]
        public string LookupValueField { get; set; }

        /// <summary>
        /// when data source = DB || MVC, data for the 
        /// list will be load lazily if AutoBind = false
        /// </summary>
        [BaseEamResourceDisplayName("Filter.AutoBind")]
        public bool AutoBind { get; set; }
    }
}