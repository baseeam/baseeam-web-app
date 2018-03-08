/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Core.Kendoui
{
    public static class ModelStateExtensions
    {
        private static string GetErrorMessage(ModelError error, ModelState modelState)
        {
            if (!string.IsNullOrEmpty(error.ErrorMessage))
            {
                return error.ErrorMessage;
            }
            if (modelState.Value == null)
            {
                return error.ErrorMessage;
            }
            var args = new object[] { modelState.Value.AttemptedValue };
            return string.Format("ValueNotValidForProperty=The value '{0}' is invalid", args);
        }

        public static object SerializeErrors(this ModelStateDictionary modelState)
        {
            return modelState.Where(entry => entry.Value.Errors.Any())
                .ToDictionary(entry => entry.Key, entry => SerializeModelState(entry.Value));
        }

        private static Dictionary<string, object> SerializeModelState(ModelState modelState)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary["errors"] = modelState.Errors.Select(x => GetErrorMessage(x, modelState)).ToArray();
            return dictionary;
        }

        public static object ToDataSourceResult(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.SerializeErrors();
            }
            return null;
        }

        public static IHtmlString Errors(this ModelStateDictionary modelState)
        {
            if (modelState == null || modelState.Values.Count == 0)
                return new HtmlString("");

            var sb = new StringBuilder();
            sb.Append("<ul>");

            foreach (ModelState s in modelState.Values)
            {
                foreach (ModelError error in s.Errors)
                {
                    sb.Append("<li>");
                    sb.Append(error.ErrorMessage);
                    sb.Append("</li>");
                }
            }

            sb.Append("</ul>");

            return new HtmlString(sb.ToString());
        }
    }
}
