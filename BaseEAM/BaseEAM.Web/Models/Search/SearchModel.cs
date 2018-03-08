/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Infrastructure;
using BaseEAM.Services;
using BaseEAM.Web.Framework.CustomField;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace BaseEAM.Web.Models
{
    [Serializable]
    public class SearchModel
    {
        public SearchModel()
        {
            Filters = new List<FieldModel>();
        }

        public List<FieldModel> Filters { get; set; }

        public List<FieldModel> NoEmptyFilters
        {
            get { return Filters.Where(f => f.Value != null).OrderBy(f => f.DisplayOrder).ToList(); }
        }

        public object[] Values
        {
            get { return this.NoEmptyFilters.Select(f => f.Value).ToArray(); }
        }

        public string ToExpression(long userId = 0)
        {
            string expression = "";
            int count = this.NoEmptyFilters.Count;
            if(count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    expression = expression + this.NoEmptyFilters[i].ToExpression(i) + " AND ";
                }
            }
            if (expression.EndsWith(" AND "))
            {
                expression = expression.Substring(0, expression.LastIndexOf(" AND "));
            }
            if (string.IsNullOrEmpty(expression))
            {
                expression = "1 = 1";
            }
            if (userId > 0)
            {
                expression = expression + " AND User.Id = " + userId.ToString();
            }
            return expression;
        }

        public dynamic ToParameters()
        {
            var parameters = new ExpandoObject() as IDictionary<string, Object>;
            int count = this.NoEmptyFilters.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var filter = this.NoEmptyFilters[i];
                    if (filter.Operator == FieldOperatorType.contains)
                        parameters.Add("i" + i, "%" + this.NoEmptyFilters[i].Value + "%");
                    else if (filter.Operator == FieldOperatorType.startswith)
                        parameters.Add("i" + i, this.NoEmptyFilters[i].Value + "%");
                    else if (filter.Operator == FieldOperatorType.endswith)
                        parameters.Add("i" + i, "%" + this.NoEmptyFilters[i].Value);
                    else
                        parameters.Add("i" + i, this.NoEmptyFilters[i].Value);
                }
            }
            return parameters;
        }

        public List<FieldModel> Validate(string searchValues)
        {
            var result = new List<FieldModel>();
            var dict = ParseSearchValues(searchValues);
            foreach(var filter in this.Filters)
            {
                if(filter.IsRequiredField == true && string.IsNullOrEmpty(dict[filter.Name]))
                {
                    result.Add(filter);
                }
            }
            return result;
        }

        public void Update(string searchValues)
        {
            var dateTimeHelper = EngineContext.Current.Resolve<IDateTimeHelper>();
            var dict = ParseSearchValues(searchValues);
            foreach (var filter in this.Filters)
            {
                filter.Operator = dict[filter.Name + "_Operator"];
                if (dict.ContainsKey(filter.Name) && !string.IsNullOrEmpty(dict[filter.Name]))
                {
                    if (filter.DataType == FieldDataType.String)
                        filter.Value = dict[filter.Name];

                    if (filter.DataType == FieldDataType.Int32)
                        filter.Value = Convert.ToInt32(dict[filter.Name]);

                    else if (filter.DataType == FieldDataType.Int32Nullable)
                        filter.Value = (Int32?)Convert.ToInt32(dict[filter.Name]);

                    else if (filter.DataType == FieldDataType.Int64)
                        filter.Value = Convert.ToInt64(dict[filter.Name]);

                    else if (filter.DataType == FieldDataType.Int64Nullable)
                        filter.Value = (Int64?)Convert.ToInt64(dict[filter.Name]);

                    else if (filter.DataType == FieldDataType.Decimal)
                        filter.Value = Convert.ToDecimal(dict[filter.Name]);

                    else if (filter.DataType == FieldDataType.DecimalNullable)
                        filter.Value = (Decimal?)Convert.ToDecimal(dict[filter.Name]);

                    else if (filter.DataType == FieldDataType.DateTime)
                        filter.Value = dateTimeHelper.ConvertToUtcTime(Convert.ToDateTime(dict[filter.Name]), dateTimeHelper.CurrentTimeZone);

                    else if (filter.DataType == FieldDataType.DateTimeNullable)
                        filter.Value = (DateTime?)dateTimeHelper.ConvertToUtcTime(Convert.ToDateTime(dict[filter.Name]), dateTimeHelper.CurrentTimeZone);

                    else if (filter.DataType == FieldDataType.Boolean)
                        filter.Value = Convert.ToBoolean(dict[filter.Name]);
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.LookupValueField) && dict.ContainsKey(filter.LookupValueField) && !string.IsNullOrEmpty(dict[filter.LookupValueField]))
                    {
                        filter.Value = Convert.ToInt64(dict[filter.LookupValueField]);
                        filter.Text = dict[filter.LookupTextField];
                    }
                    else
                    {
                        filter.Value = null;
                        filter.Text = null;
                    }                        
                }
            }
        }

        public void ClearValues()
        {
            foreach(var filter in this.Filters)
            {
                filter.Value = null;
            }
        }

        private Dictionary<string, string> ParseSearchValues(string searchValues)
        {
            var searchValuesDictionary = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(searchValues))
                return searchValuesDictionary;
            var filters = searchValues.Split('&');
            if (filters.Count() > 0)
            {
                foreach (var filter in filters)
                {
                    var filterKeyValue = filter.Split('=');
                    if (filterKeyValue.Count() != 2)
                    {
                        throw new BaseEamException("Not valid form.");
                    }
                    else
                    {
                        //This check is for multiselect values
                        if (searchValuesDictionary.ContainsKey(filterKeyValue[0]))
                        {
                            //build a value has format '1','2','3' so it can be replaced in an IN clause
                            searchValuesDictionary[filterKeyValue[0]] = "'" + searchValuesDictionary[filterKeyValue[0]] + "','" + filterKeyValue[1] + "'";
                            searchValuesDictionary[filterKeyValue[0]] = searchValuesDictionary[filterKeyValue[0]].Replace("''", "'");
                        }
                        else
                        {
                            searchValuesDictionary.Add(filterKeyValue[0], HttpUtility.UrlDecode(filterKeyValue[1]));
                        }
                    }
                }
            }

            return searchValuesDictionary;
        }
    }
}