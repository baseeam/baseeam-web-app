/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using System;

namespace BaseEAM.Web.Framework.CustomField
{
    [Serializable]
    public partial class FieldModel
    {
        public FieldModel()
        {
            AutoBind = true;
        }

        /// <summary>
        /// DisplayOrder is used to layout the filter on UI
        /// odd = left side, even = right side
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Name is used as the id of the UI control and must be unique.
        /// </summary>
        public string Name { get; set; }

        public string ResourceKey { get; set; }

        /// <summary>
        /// The database column that this filter map to.
        /// </summary>
        public string DbColumn { get; set; }

        public string Operator { get; set; }

        public object Value { get; set; }
        public string Text { get; set; }

        public FieldControlType ControlType { get; set; }

        public FieldDataType DataType { get; set; }

        public FieldDataSource DataSource { get; set; }

        public bool IsRequiredField { get; set; }

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
        /// Used internally when data source = SQL
        /// The purpose is we don't want to post SQL query from ajax calls
        /// to prevent security leak
        /// Used this SessionKey to lookup to associated FieldModel
        /// and then get it's SqlQuery
        /// </summary>
        public string SessionKey { get; set; }

        /// <summary>
        /// Specify values when data source = MVC
        /// </summary>
        public string MvcController { get; set; }
        public string MvcAction { get; set; }
        public string AdditionalField { get; set; }
        public string AdditionalValue { get; set; }

        /// <summary>
        /// when data source = DB || MVC, data for the 
        /// list will be load lazily if AutoBind = false
        /// </summary>
        public bool AutoBind { get; set; }

        /// <summary>
        /// The values of this field will depend
        /// on the current value of the parent field
        /// </summary>
        public string ParentFieldName { get; set; }

        public string LookupType { get; set; }
        public string LookupTextField { get; set; }
        public string LookupValueField { get; set; }

        public string ToExpression(int index)
        {
            string expression = "";
            if (this.Operator == FieldOperatorType.contains
                    || this.Operator == FieldOperatorType.startswith
                    || this.Operator == FieldOperatorType.endswith)
            {
                if (this.DbColumn.Contains(","))
                {
                    var columns = this.DbColumn.Split(',');
                    string exp = "(";
                    foreach(var column in columns)
                    {
                        exp = exp + column + " LIKE @i{0} OR ";
                    }
                    if (exp.EndsWith(" OR "))
                    {
                        exp = exp.Substring(0, exp.LastIndexOf(" OR "));
                    }
                    exp = exp + ")";
                    expression = string.Format(exp, index);
                }
                else
                {
                    expression = string.Format("{0} LIKE @i{1}", this.DbColumn, index);
                }
            }
            else if (this.Operator == FieldOperatorType.eq)
            {
                if (this.ControlType == FieldControlType.MultiSelectList)
                {
                    if(this.Value.ToString().Contains("'"))
                    {
                        expression = string.Format("{0} IN (" + this.Value + ")", this.DbColumn);
                    }
                    else
                    {
                        expression = string.Format("{0} IN ('" + this.Value + "')", this.DbColumn);
                    }
                }
                else
                {
                    expression = string.Format("{0} = @i{1}", this.DbColumn, index);
                }
            }
            else if (this.Operator == FieldOperatorType.neq)
                expression = string.Format("{0} != @i{1}", this.DbColumn, index);
            else if (this.Operator == FieldOperatorType.lt)
                expression = string.Format("{0} < @i{1}", this.DbColumn, index);
            else if (this.Operator == FieldOperatorType.lte)
                expression = string.Format("{0} <= @i{1}", this.DbColumn, index);
            else if (this.Operator == FieldOperatorType.gt)
                expression = string.Format("{0} > @i{1}", this.DbColumn, index);
            else if (this.Operator == FieldOperatorType.gte)
                expression = string.Format("{0} >= @i{1}", this.DbColumn, index);

            return expression;
        }
    }
}
