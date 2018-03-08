/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;
using System.Web.Mvc;

namespace BaseEAM.Web.Framework.CustomField
{
    public static class FieldOperatorType
    {
        public static readonly string contains = "contains";
        public static readonly string startswith = "startswith";
        public static readonly string endswith = "endswith";
        public static readonly string eq = "eq";
        public static readonly string neq = "neq";
        public static readonly string lt = "lt";
        public static readonly string lte = "lte";
        public static readonly string gt = "gt";
        public static readonly string gte = "gte";
    }

    public static class FieldOperator
    {
        public static readonly Dictionary<string, List<SelectListItem>> AvailableOperators = new Dictionary<string, List<SelectListItem>>
        {
            { "String", new List<SelectListItem>
                        {
                            new SelectListItem() { Value = FieldOperatorType.contains, Text = "%%" },
                            new SelectListItem() { Value = FieldOperatorType.startswith, Text = ".%" },
                            new SelectListItem() { Value = FieldOperatorType.endswith, Text = "%." }
                        }
            },
            { "Integer", new List<SelectListItem>
                        {
                            new SelectListItem() { Value = FieldOperatorType.eq, Text = "=" },
                            new SelectListItem() { Value = FieldOperatorType.neq, Text = "<>" },
                            new SelectListItem() { Value = FieldOperatorType.lt, Text = "<" },
                            new SelectListItem() { Value = FieldOperatorType.lte, Text = "<=" },
                            new SelectListItem() { Value = FieldOperatorType.gt, Text = ">" },
                            new SelectListItem() { Value = FieldOperatorType.gte, Text = ">=" }
                        }
            },
            { "Decimal", new List<SelectListItem>
                        {
                            new SelectListItem() { Value = FieldOperatorType.eq, Text = "=" },
                            new SelectListItem() { Value = FieldOperatorType.neq, Text = "<>" },
                            new SelectListItem() { Value = FieldOperatorType.lt, Text = "<" },
                            new SelectListItem() { Value = FieldOperatorType.lte, Text = "<=" },
                            new SelectListItem() { Value = FieldOperatorType.gt, Text = ">" },
                            new SelectListItem() { Value = FieldOperatorType.gte, Text = ">=" }
                        }
            },
            { "DateTime", new List<SelectListItem>
                        {
                            new SelectListItem() { Value = FieldOperatorType.eq, Text = "=" },
                            new SelectListItem() { Value = FieldOperatorType.neq, Text = "<>" },
                            new SelectListItem() { Value = FieldOperatorType.lt, Text = "<" },
                            new SelectListItem() { Value = FieldOperatorType.lte, Text = "<=" },
                            new SelectListItem() { Value = FieldOperatorType.gt, Text = ">" },
                            new SelectListItem() { Value = FieldOperatorType.gte, Text = ">=" }
                        }
            },
            { "DropDownList", new List<SelectListItem>
                        {
                            new SelectListItem() { Value = FieldOperatorType.eq, Text = "=" }
                        }
            },
            { "MultiSelectList", new List<SelectListItem>
                        {
                            new SelectListItem() { Value = FieldOperatorType.eq, Text = "=" }
                        }
            }
        };
    }
}
