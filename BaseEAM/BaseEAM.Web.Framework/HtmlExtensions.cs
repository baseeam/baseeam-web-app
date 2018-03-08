/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Infrastructure;
using BaseEAM.Core.Timing;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Localization;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.CustomField;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;

namespace BaseEAM.Web.Framework
{
    public static class HtmlExtensions
    {
        #region Admin area extensions

        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string buttonsSelector) where T : BaseEamEntityModel
        {
            return DeleteConfirmation(helper, "", buttonsSelector);
        }

        public static MvcHtmlString DeleteConfirmation<T>(this HtmlHelper<T> helper, string actionName,
            string buttonsSelector) where T : BaseEamEntityModel
        {
            if (String.IsNullOrEmpty(actionName))
                actionName = "Delete";

            var modalId = MvcHtmlString.Create(helper.ViewData.ModelMetadata.ModelType.Name.ToLower() + "-delete-confirmation")
                .ToHtmlString();

            var deleteConfirmationModel = new DeleteConfirmationModel
            {
                Id = helper.ViewData.Model.Id,
                ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
                ActionName = actionName,
                WindowId = modalId
            };

            var window = new StringBuilder();
            window.AppendLine(string.Format("<div id='{0}' class=\"modal fade\"  tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"{0}-title\">", modalId));
            window.AppendLine(helper.Partial("Delete", deleteConfirmationModel).ToHtmlString());
            window.AppendLine("</div>");

            window.AppendLine("<script>");
            window.AppendLine("$(document).ready(function() {");
            window.AppendLine(string.Format("$('#{0}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{1}\")", buttonsSelector, modalId));
            window.AppendLine("});");
            window.AppendLine("</script>");

            return MvcHtmlString.Create(window.ToString());
        }

        public static MvcHtmlString ActionConfirmation(this HtmlHelper helper, string buttonId, string actionName = "")
        {
            if (string.IsNullOrEmpty(actionName))
                actionName = helper.ViewContext.RouteData.GetRequiredString("action");

            var modalId = MvcHtmlString.Create(buttonId + "-action-confirmation").ToHtmlString();

            var actionConfirmationModel = new ActionConfirmationModel()
            {
                ControllerName = helper.ViewContext.RouteData.GetRequiredString("controller"),
                ActionName = actionName,
                WindowId = modalId
            };

            var window = new StringBuilder();
            window.AppendLine(string.Format("<div id='{0}' class=\"modal fade\"  tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"{0}-title\">", modalId));
            window.AppendLine(helper.Partial("Confirm", actionConfirmationModel).ToHtmlString());
            window.AppendLine("</div>");

            window.AppendLine("<script>");
            window.AppendLine("$(document).ready(function() {");
            window.AppendLine(string.Format("$('#{0}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{1}\");", buttonId, modalId));
            window.AppendLine(string.Format("$('#{0}-submit-button').attr(\"name\", $(\"#{1}\").attr(\"name\"));", modalId, buttonId));
            window.AppendLine(string.Format("$(\"#{0}\").attr(\"name\", \"\")", buttonId));
            window.AppendLine(string.Format("if($(\"#{0}\").attr(\"type\") == \"submit\")$(\"#{0}\").attr(\"type\", \"button\")", buttonId));
            window.AppendLine("});");
            window.AppendLine("</script>");

            return MvcHtmlString.Create(window.ToString());
        }

        /// <summary>
        /// Render CSS styles of selected index 
        /// </summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="currentTabName">Current tab name (where appropriate CSS style should be rendred)</param>
        /// <param name="content">Tab content</param>
        /// <param name="isDefaultTab">Indicates that the tab is default</param>
        /// <param name="tabNameToSelect">Tab name to select</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString RenderBootstrapTabContent(this HtmlHelper helper, string currentTabName,
            HelperResult content, bool isDefaultTab = false, string tabNameToSelect = "", string dataBind = "")
        {
            if (helper == null)
                throw new ArgumentNullException("helper");

            if (string.IsNullOrEmpty(tabNameToSelect))
                tabNameToSelect = helper.GetSelectedTabName();

            if (string.IsNullOrEmpty(tabNameToSelect) && isDefaultTab)
                tabNameToSelect = currentTabName;

            var tag = new TagBuilder("div")
            {
                InnerHtml = content.ToHtmlString(),
                Attributes =
                {
                    new KeyValuePair<string, string>("class", string.Format("tab-pane{0}", tabNameToSelect == currentTabName ? " active" : "")),
                    new KeyValuePair<string, string>("id", string.Format("{0}", currentTabName)),
                    new KeyValuePair<string, string>("data-bind", string.Format("{0}", dataBind))
                }
            };

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Render CSS styles of selected index 
        /// </summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="currentTabName">Current tab name (where appropriate CSS style should be rendred)</param>
        /// <param name="title">Tab title</param>
        /// <param name="isDefaultTab">Indicates that the tab is default</param>
        /// <param name="tabNameToSelect">Tab name to select</param>
        /// <param name="customCssClass">Tab name to select</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString RenderBootstrapTabHeader(this HtmlHelper helper, string currentTabName,
            LocalizedString title, bool isDefaultTab = false, string tabNameToSelect = "", string customCssClass = "")
        {
            if (helper == null)
                throw new ArgumentNullException("helper");

            if (string.IsNullOrEmpty(tabNameToSelect))
                tabNameToSelect = helper.GetSelectedTabName();

            if (string.IsNullOrEmpty(tabNameToSelect) && isDefaultTab)
                tabNameToSelect = currentTabName;

            var a = new TagBuilder("a")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("data-tab-name", currentTabName),
                    new KeyValuePair<string, string>("href", string.Format("#{0}", currentTabName)),
                    new KeyValuePair<string, string>("data-toggle", "tab"),
                },
                InnerHtml = title.Text
            };
            var liClassValue = "";
            if (tabNameToSelect == currentTabName)
            {
                liClassValue = "active";
            }
            if (!String.IsNullOrEmpty(customCssClass))
            {
                if (!String.IsNullOrEmpty(liClassValue))
                    liClassValue += " ";
                liClassValue += customCssClass;
            }

            var li = new TagBuilder("li")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("class", liClassValue),
                    new KeyValuePair<string, string>("id", string.Format("{0}-header", currentTabName))
                },
                InnerHtml = a.ToString(TagRenderMode.Normal)
            };

            return MvcHtmlString.Create(li.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Gets a selected tab name (used to store selected tab name)
        /// </summary>
        /// <returns>Name</returns>
        public static string GetSelectedTabName(this HtmlHelper helper)
        {
            //keep this method synchornized with
            //"SaveSelectedTab" method of \Controllers\BaseController.cs
            var tabName = string.Empty;
            const string dataKey = "baseeam.selected-tab-name";

            if (helper.ViewData.ContainsKey(dataKey))
                tabName = helper.ViewData[dataKey].ToString();

            if (helper.ViewContext.Controller.TempData.ContainsKey(dataKey))
                tabName = helper.ViewContext.Controller.TempData[dataKey].ToString();

            return tabName;
        }

        #region Form fields

        public static MvcHtmlString Hint(this HtmlHelper helper, string value)
        {
            //create tag builder
            var builder = new TagBuilder("div");
            builder.MergeAttribute("title", value);
            builder.MergeAttribute("class", "ico-help");
            var icon = new StringBuilder();
            icon.Append("<i class='fa fa-question-circle'></i>");
            builder.InnerHtml = icon.ToString();
            //render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString BaseEamLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper,
                Expression<Func<TModel, TValue>> expression, bool displayHint = false, bool required = false)
        {
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var hintResource = string.Empty;
            object value;

            result.Append(helper.LabelFor(expression, new { title = hintResource, @class = "control-label" }));
            if (required == true)
            {
                result.Append("<span class=\"required\">*</span>");
            }
            if (metadata.AdditionalValues.TryGetValue("BaseEamResourceDisplayName", out value))
            {
                var resourceDisplayName = value as BaseEamResourceDisplayName;
                if (resourceDisplayName != null && displayHint)
                {
                    var langId = EngineContext.Current.Resolve<IWorkContext>().WorkingLanguage.Id;
                    hintResource = EngineContext.Current.Resolve<ILocalizationService>()
                        .GetResource(resourceDisplayName.ResourceKey + ".Hint", langId, returnEmptyIfNotFound: true, logIfNotFound: false);
                    if (!String.IsNullOrEmpty(hintResource))
                    {
                        result.Append(helper.Hint(hintResource).ToHtmlString());
                    }
                }
            }

            var laberWrapper = new TagBuilder("div");
            laberWrapper.Attributes.Add("class", "label-wrapper");
            laberWrapper.InnerHtml = result.ToString();

            return MvcHtmlString.Create(laberWrapper.ToString());
        }

        public static MvcHtmlString BaseEamEditorFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string postfix = "",
            bool? renderFormControlClass = null, bool required = false, string dataBind = "")
        {
            var result = new StringBuilder();
            object htmlAttributes = null;
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            if ((!renderFormControlClass.HasValue && metadata.ModelType.Name.Equals("String")) ||
                (renderFormControlClass.HasValue && renderFormControlClass.Value))
                htmlAttributes = new {@class = "form-control"};

            var editorHtml = helper.EditorFor(expression, new { htmlAttributes, postfix, dataBind });
            if (required)
            {
                result.AppendFormat(
                    "<div class=\"input-group input-group-required\">{0}<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>",
                    editorHtml);
            }
            else
            {
                result.Append(editorHtml);
            }

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString BaseEamCheckBoxFor<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression)
        {
            var result = new StringBuilder();
            result.Append(helper.CheckBoxFor(expression));

            //replace the hidden field comes with checkbox
            //so jquery.serilaizeJSON can work correctly
            Regex regex = new Regex(@"<input name=.*hidden.*>");
            Match match = regex.Match(result.ToString());
            if (match.Success)
            {
                result.Replace(match.Value, "");
            }
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString BaseEamDropDownList<TModel>(this HtmlHelper<TModel> helper, string name,
            IEnumerable<SelectListItem> itemList, object htmlAttributes = null, bool renderFormControlClass = true, bool required = false)
        {
            var result = new StringBuilder();

            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (renderFormControlClass)
                attrs = AddFormControlClassToHtmlAttributes(attrs);

            if (required)
                result.AppendFormat(
                    "<div class=\"input-group input-group-required\">{0}<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>",
                    helper.DropDownList(name, itemList, attrs));
            else
                result.Append(helper.DropDownList(name, itemList, attrs));

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString BaseEamDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> itemList,
            object htmlAttributes = null, bool renderFormControlClass = true, bool required = false)
        {
            var result = new StringBuilder();

            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (renderFormControlClass)
                attrs = AddFormControlClassToHtmlAttributes(attrs);

            if (required)
                result.AppendFormat(
                    "<div class=\"input-group input-group-required\">{0}<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>",
                    helper.DropDownListFor(expression, itemList, attrs));
            else
                result.Append(helper.DropDownListFor(expression, itemList, attrs));

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString BaseEamComboBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper, 
            Expression<Func<TModel, TValue>> expression, 
            string action, string controller, RouteValueDictionary routeValues = null, bool required = false, string dataBind = "", bool aysIgnore = false)
        {
            var result = new StringBuilder();
            if(aysIgnore == true)
                result.Append(helper.TextBoxFor(expression, new { data_bind = dataBind, style = "display: none;", @class = "ays-ignore" }));
            else
                result.Append(helper.TextBoxFor(expression, new { data_bind = dataBind, style = "display: none;" }));
            var id = helper.FieldIdFor(expression);
            var dropdown = helper.Partial("ComboBox", new BaseEamComboBox
            {
                HtmlId = id,
                Controller = controller,
                Action = action,
                IsRequired = required,
                DbTable = routeValues != null && routeValues.ContainsKey("dbTable") ? routeValues["dbTable"].ToString() : "",
                DbTextColumn = routeValues != null && routeValues.ContainsKey("dbTextColumn") ? routeValues["dbTextColumn"].ToString() : "",
                DbValueColumn = routeValues != null && routeValues.ContainsKey("dbValueColumn") ? routeValues["dbValueColumn"].ToString() : "",
                AdditionalField = routeValues != null && routeValues.ContainsKey("additionalField") ? routeValues["additionalField"].ToString() : "",
                AdditionalValue = routeValues != null && routeValues.ContainsKey("additionalValue") ? routeValues["additionalValue"].ToString() : "",
                OptionalField = routeValues != null && routeValues.ContainsKey("optionalField") ? routeValues["optionalField"].ToString() : "",
                ParentFieldName = routeValues != null && routeValues.ContainsKey("parentFieldName") ? routeValues["parentFieldName"].ToString() : ""
            });

            result.Append(dropdown);
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString TreeViewLookupFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression,
            string action, string controller, RouteValueDictionary routeValues = null, bool required = false)
        {
            var result = new StringBuilder();
            var id = helper.FieldIdFor(expression);
            var model = new TreeViewLookup
            {
                TextFieldId = id,
                ValueFieldId = routeValues != null && routeValues.ContainsKey("valueFieldId") ? routeValues["valueFieldId"].ToString() : "",
                Controller = controller,
                Action = action,
                DbTable = routeValues != null && routeValues.ContainsKey("dbTable") ? routeValues["dbTable"].ToString() : "",
                DbTextColumn = routeValues != null && routeValues.ContainsKey("dbTextColumn") ? routeValues["dbTextColumn"].ToString() : "",
                DbValueColumn = routeValues != null && routeValues.ContainsKey("dbValueColumn") ? routeValues["dbValueColumn"].ToString() : "",
                AdditionalField = routeValues != null && routeValues.ContainsKey("additionalField") ? routeValues["additionalField"].ToString() : "",
                AdditionalValue = routeValues != null && routeValues.ContainsKey("additionalValue") ? routeValues["additionalValue"].ToString() : "",
                TreeType = routeValues != null && routeValues.ContainsKey("treeType") ? routeValues["treeType"].ToString() : ""
            };
            var template =
                "<div class=\"input-group\">{0}" +
                    "<span class=\"input-group-btn\">" +
                        "<button type=\"button\" id=\"{1}\" class=\"btn btn-primary btn-master\" title=\"Tree\">" +
                            "<i class=\"fa fa-sitemap\"></i>" +
                        "</button>" +
                    "</span>" +
                "</div>";
            result.AppendFormat(template, helper.BaseEamEditorFor(expression, renderFormControlClass: false), model.TreeType + "-" + model.TextFieldId);
            
            var treeViewLookup = helper.Partial("TreeViewLookup", model);

            result.Append(treeViewLookup);
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString LookupFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression,
            string action, string controller, RouteValueDictionary routeValues = null, bool required = false, string dataBind = "")
        {
            var result = new StringBuilder();
            var id = helper.FieldIdFor(expression);
            var model = new Lookup
            {
                TextFieldId = id,
                ValueFieldId = routeValues != null && routeValues.ContainsKey("valueFieldId") ? routeValues["valueFieldId"].ToString() : "",
                Controller = controller,
                Action = action,
                DbTable = routeValues != null && routeValues.ContainsKey("dbTable") ? routeValues["dbTable"].ToString() : "",
                DbTextColumn = routeValues != null && routeValues.ContainsKey("dbTextColumn") ? routeValues["dbTextColumn"].ToString() : "",
                DbValueColumn = routeValues != null && routeValues.ContainsKey("dbValueColumn") ? routeValues["dbValueColumn"].ToString() : "",
                AdditionalField = routeValues != null && routeValues.ContainsKey("additionalField") ? routeValues["additionalField"].ToString() : "",
                AdditionalValue = routeValues != null && routeValues.ContainsKey("additionalValue") ? routeValues["additionalValue"].ToString() : "",
                ParentFieldName = routeValues != null && routeValues.ContainsKey("parentFieldName") ? routeValues["parentFieldName"].ToString() : "",
                LookupType = routeValues != null && routeValues.ContainsKey("lookupType") ? routeValues["lookupType"].ToString() : "",
                ViewType = routeValues != null && routeValues.ContainsKey("viewType") ? routeValues["viewType"].ToString() : "",
                SelectedEvent = routeValues != null && routeValues.ContainsKey("selectedEvent") ? routeValues["selectedEvent"].ToString() : ""
            };
            var template =
                "<div class=\"input-group\" data-bind=\"{0}\">{1}" +
                    "<div class=\"input-group-btn\">" +
                        "<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\">" +
                            "<span class=\"caret\"></span><span class=\"sr-only\">&nbsp;</span>" +
                        "</button>" +
                        "<ul class=\"dropdown-menu\" role=\"menu\">" +
                            "<li>" +
                                "<button type=\"button\" id=\"{2}\" class=\"btn btn-primary btn-master\" title=\"Details\">" +
                                    "Details" +
                                "</button>" +
                            "</li>" +
                            "<li>" +
                                "<button type=\"button\" id=\"{3}\" class=\"btn btn-primary btn-master\" title=\"Lookup\">" +
                                    "Lookup" +
                                "</button>" +
                            "</li>" +
                        "</ul>" +
                    "</div>" +
                "</div>";
            result.AppendFormat(template, dataBind, helper.BaseEamEditorFor(expression, renderFormControlClass: false), 
                model.LookupType + "-" + model.TextFieldId + "-Details",
                model.LookupType + "-" + model.TextFieldId + "-Lookup");

            var lookup = helper.Partial("Lookup", model);

            result.Append(lookup);
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString BaseEamTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null,
            bool renderFormControlClass = true, int rows = 4, int columns = 20, bool required = false)
        {
            var result = new StringBuilder();

            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (renderFormControlClass)
                attrs = AddFormControlClassToHtmlAttributes(attrs);

            if (required)
                result.AppendFormat(
                    "<div class=\"input-group input-group-required\">{0}<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>",
                    helper.TextAreaFor(expression, rows, columns, attrs));
            else
                result.Append(helper.TextAreaFor(expression, rows, columns, attrs));

            return MvcHtmlString.Create(result.ToString());
        }


        public static MvcHtmlString BaseEamDisplayFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            var result = new TagBuilder("div");
            result.Attributes.Add("class", "form-text-row");
            result.InnerHtml = helper.DisplayFor(expression).ToString();

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString BaseEamDisplay<TModel>(this HtmlHelper<TModel> helper, string expression)
        {
            var result = new TagBuilder("div");
            result.Attributes.Add("class", "form-text-row");
            result.InnerHtml = expression;

            return MvcHtmlString.Create(result.ToString());
        }

        public static RouteValueDictionary AddFormControlClassToHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes["class"] == null || string.IsNullOrEmpty(htmlAttributes["class"].ToString()))
                htmlAttributes["class"] = "form-control";
            else
                if (!htmlAttributes["class"].ToString().Contains("form-control"))
                htmlAttributes["class"] += " form-control";

            return htmlAttributes as RouteValueDictionary;
        }

        #endregion

        #endregion

        #region Common extensions

        public static MvcHtmlString RequiredHint(this HtmlHelper helper, string additionalText = null)
        {
            // Create tag builder
            var builder = new TagBuilder("span");
            builder.AddCssClass("required");
            var innerText = "*";
            //add additional text if specified
            if (!String.IsNullOrEmpty(additionalText))
                innerText += " " + additionalText;
            builder.SetInnerText(innerText);
            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            return html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            // because "[" and "]" aren't replaced with "_" in GetFullHtmlFieldId
            return id.Replace('[', '_').Replace(']', '_');
        }

        /// <summary>
        /// Creates a days, months, years drop down list using an HTML select control. 
        /// The parameters represent the value of the "name" attribute on the select control.
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="dayName">"Name" attribute of the day drop down list.</param>
        /// <param name="monthName">"Name" attribute of the month drop down list.</param>
        /// <param name="yearName">"Name" attribute of the year drop down list.</param>
        /// <param name="beginYear">Begin year</param>
        /// <param name="endYear">End year</param>
        /// <param name="selectedDay">Selected day</param>
        /// <param name="selectedMonth">Selected month</param>
        /// <param name="selectedYear">Selected year</param>
        /// <param name="localizeLabels">Localize labels</param>
        /// <param name="htmlAttributes">HTML attributes</param>
		/// <param name="wrapTags">Wrap HTML select controls with span tags for styling/layout</param>
        /// <returns></returns>
        public static MvcHtmlString DatePickerDropDowns(this HtmlHelper html,
            string dayName, string monthName, string yearName,
            int? beginYear = null, int? endYear = null,
            int? selectedDay = null, int? selectedMonth = null, int? selectedYear = null,
            bool localizeLabels = true, object htmlAttributes = null, bool wrapTags = false)
        {
            var daysList = new TagBuilder("select");
            var monthsList = new TagBuilder("select");
            var yearsList = new TagBuilder("select");

            daysList.Attributes.Add("name", dayName);
            monthsList.Attributes.Add("name", monthName);
            yearsList.Attributes.Add("name", yearName);

            var htmlAttributesDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            daysList.MergeAttributes(htmlAttributesDictionary, true);
            monthsList.MergeAttributes(htmlAttributesDictionary, true);
            yearsList.MergeAttributes(htmlAttributesDictionary, true);

            var days = new StringBuilder();
            var months = new StringBuilder();
            var years = new StringBuilder();

            string dayLocale, monthLocale, yearLocale;
            if (localizeLabels)
            {
                var locService = EngineContext.Current.Resolve<ILocalizationService>();
                dayLocale = locService.GetResource("Common.Day");
                monthLocale = locService.GetResource("Common.Month");
                yearLocale = locService.GetResource("Common.Year");
            }
            else
            {
                dayLocale = "Day";
                monthLocale = "Month";
                yearLocale = "Year";
            }

            days.AppendFormat("<option value='{0}'>{1}</option>", "0", dayLocale);
            for (int i = 1; i <= 31; i++)
                days.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                    (selectedDay.HasValue && selectedDay.Value == i) ? " selected=\"selected\"" : null);


            months.AppendFormat("<option value='{0}'>{1}</option>", "0", monthLocale);
            for (int i = 1; i <= 12; i++)
            {
                months.AppendFormat("<option value='{0}'{1}>{2}</option>",
                                    i,
                                    (selectedMonth.HasValue && selectedMonth.Value == i) ? " selected=\"selected\"" : null,
                                    CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i));
            }


            years.AppendFormat("<option value='{0}'>{1}</option>", "0", yearLocale);

            if (beginYear == null)
                beginYear = Clock.Now.Year - 100;
            if (endYear == null)
                endYear = Clock.Now.Year;

            if (endYear > beginYear)
            {
                for (int i = beginYear.Value; i <= endYear.Value; i++)
                    years.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                        (selectedYear.HasValue && selectedYear.Value == i) ? " selected=\"selected\"" : null);
            }
            else
            {
                for (int i = beginYear.Value; i >= endYear.Value; i--)
                    years.AppendFormat("<option value='{0}'{1}>{0}</option>", i,
                        (selectedYear.HasValue && selectedYear.Value == i) ? " selected=\"selected\"" : null);
            }

            daysList.InnerHtml = days.ToString();
            monthsList.InnerHtml = months.ToString();
            yearsList.InnerHtml = years.ToString();

            if (wrapTags)
            {
                string wrapDaysList = "<span class=\"days-list select-wrapper\">" + daysList + "</span>";
                string wrapMonthsList = "<span class=\"months-list select-wrapper\">" + monthsList + "</span>";
                string wrapYearsList = "<span class=\"years-list select-wrapper\">" + yearsList + "</span>";

                return MvcHtmlString.Create(string.Concat(wrapDaysList, wrapMonthsList, wrapYearsList));
            }
            else
            {
                return MvcHtmlString.Create(string.Concat(daysList, monthsList, yearsList));
            }

        }

        public static MvcHtmlString Widget(this HtmlHelper helper, string widgetZone, object additionalData = null, string area = null)
        {
            return helper.Action("WidgetsByZone", "Widget", new { widgetZone = widgetZone, additionalData = additionalData, area = area });
        }

        /// <summary>
        /// Renders the standard label with a specified suffix added to label text
        /// </summary>
        /// <typeparam name="TModel">Model</typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <param name="html">HTML helper</param>
        /// <param name="expression">Expression</param>
        /// <param name="htmlAttributes">HTML attributes</param>
        /// <param name="suffix">Suffix</param>
        /// <returns>Label</returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes, string suffix)
        {
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string resolvedLabelText = metadata.DisplayName ?? (metadata.PropertyName ?? htmlFieldName.Split(new[] { '.' }).Last());
            if (string.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }
            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName)));
            if (!String.IsNullOrEmpty(suffix))
            {
                resolvedLabelText = String.Concat(resolvedLabelText, suffix);
            }
            tag.SetInnerText(resolvedLabelText);

            var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            tag.MergeAttributes(dictionary, true);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static string ControllerName(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];

            return string.Empty;
        }

        public static string ActionName(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
                return (string)routeValues["action"];

            return string.Empty;
        }

        #endregion

        #region Custom field extensions

        public static MvcHtmlString BaseEamFieldLabel(this HtmlHelper helper, FieldModel model, bool displayHint = false)
        {
            var result = new StringBuilder();
            var resource = string.Empty;
            var hintResource = string.Empty;
            var langId = EngineContext.Current.Resolve<IWorkContext>().WorkingLanguage.Id;
            resource = EngineContext.Current.Resolve<ILocalizationService>()
                        .GetResource(model.ResourceKey, langId, returnEmptyIfNotFound: true, logIfNotFound: false);            
            result.Append(helper.Label(model.Name, resource, new { title = hintResource, @class = "control-label" }));
            if (model.IsRequiredField == true)
            {
                result.Append("<span class=\"required\">*</span>");
            }
            if (displayHint == true)
            {
                hintResource = EngineContext.Current.Resolve<ILocalizationService>()
                        .GetResource(model.ResourceKey + ".Hint", langId, returnEmptyIfNotFound: true, logIfNotFound: false);
                if (!String.IsNullOrEmpty(hintResource))
                {
                    result.Append(helper.Hint(hintResource).ToHtmlString());
                }
            }

            var laberWrapper = new TagBuilder("div");
            laberWrapper.Attributes.Add("class", "label-wrapper");
            laberWrapper.InnerHtml = result.ToString();

            return MvcHtmlString.Create(laberWrapper.ToString());
        }

        public static MvcHtmlString BaseEamFieldOperator(this HtmlHelper helper, FieldModel model)
        {
            var result = new StringBuilder();
            var operators = new List<SelectListItem>();
            if(model.ControlType != FieldControlType.DropDownList 
                && model.ControlType != FieldControlType.MultiSelectList)
            {
                if (model.DataType == FieldDataType.String)
                {
                    operators = FieldOperator.AvailableOperators["String"];
                }
                else if (model.DataType == FieldDataType.Int32 || model.DataType == FieldDataType.Int32Nullable
                    || model.DataType == FieldDataType.Int64 || model.DataType == FieldDataType.Int64Nullable)
                {
                    operators = FieldOperator.AvailableOperators["Integer"];
                }
                else if (model.DataType == FieldDataType.Decimal || model.DataType == FieldDataType.DecimalNullable)
                {
                    operators = FieldOperator.AvailableOperators["Decimal"];
                }
                else if (model.DataType == FieldDataType.DateTime || model.DataType == FieldDataType.DateTimeNullable)
                {
                    operators = FieldOperator.AvailableOperators["DateTime"];
                }
            }            
            else
            {
                operators = FieldOperator.AvailableOperators["DropDownList"];
            }

            if (!string.IsNullOrEmpty(model.Operator))
            {
                var selected = operators.First(o => o.Value == model.Operator);
                selected.Selected = true;
            }

            result.Append(helper.DropDownList(model.Name + "_Operator", operators, new { @class = "form-control" }));

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString BaseEamHiddenFieldOperator(this HtmlHelper helper, FieldModel model)
        {
            var result = new StringBuilder();
            var operators = new List<SelectListItem>();
            if (model.ControlType != FieldControlType.DropDownList
                && model.ControlType != FieldControlType.MultiSelectList)
            {
                if (model.DataType == FieldDataType.String)
                {
                    operators = FieldOperator.AvailableOperators["String"];
                }
                else if (model.DataType == FieldDataType.Int32 || model.DataType == FieldDataType.Int32Nullable
                    || model.DataType == FieldDataType.Int64 || model.DataType == FieldDataType.Int64Nullable)
                {
                    operators = FieldOperator.AvailableOperators["Integer"];
                }
                else if (model.DataType == FieldDataType.Decimal || model.DataType == FieldDataType.DecimalNullable)
                {
                    operators = FieldOperator.AvailableOperators["Decimal"];
                }
                else if (model.DataType == FieldDataType.DateTime || model.DataType == FieldDataType.DateTimeNullable)
                {
                    operators = FieldOperator.AvailableOperators["DateTime"];                    
                }
            }
            else
            {
                operators = FieldOperator.AvailableOperators["DropDownList"];
            }

            if (!string.IsNullOrEmpty(model.Operator))
            {
                var selected = operators.First(o => o.Value == model.Operator);
                selected.Selected = true;
            }

            string operatorValue = ((SelectListItem)operators[0]).Value;
            // do a hack here for DateFrom, DateTo filters
            if (model.Name.Contains("DateFrom"))
            {
                operatorValue = FieldOperatorType.gte;
            }
            if (model.Name.Contains("DateTo"))
            {
                operatorValue = FieldOperatorType.lte;
            }
            result.Append(helper.Hidden(model.Name + "_Operator", operatorValue));

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString BaseEamFieldValue(this HtmlHelper helper, FieldModel model)
        {
            var result = new StringBuilder();
            if (model.ControlType == FieldControlType.TextBox && model.DataType == FieldDataType.String)
            {
                result.Append(helper.Partial("FieldString", model));
            }
            else if(model.ControlType == FieldControlType.TextBox && model.DataType == FieldDataType.Int32)
            {
                result.Append(helper.Partial("FieldInteger", model));
            }
            else if (model.ControlType == FieldControlType.TextBox && model.DataType == FieldDataType.Int32Nullable)
            {
                result.Append(helper.Partial("FieldInteger", model));
            }
            else if (model.ControlType == FieldControlType.TextBox && model.DataType == FieldDataType.Int64)
            {
                result.Append(helper.Partial("FieldInteger", model));
            }
            else if (model.ControlType == FieldControlType.TextBox && model.DataType == FieldDataType.Int64Nullable)
            {
                result.Append(helper.Partial("FieldInteger", model));
            }
            else if (model.ControlType == FieldControlType.TextBox && model.DataType == FieldDataType.Decimal)
            {
                result.Append(helper.Partial("FieldDecimal", model));
            }
            else if (model.ControlType == FieldControlType.TextBox && model.DataType == FieldDataType.DecimalNullable)
            {
                result.Append(helper.Partial("FieldDecimal", model));
            }
            else if (model.ControlType == FieldControlType.Date && model.DataType == FieldDataType.DateTime)
            {
                result.Append(helper.Partial("FieldDate", model));
            }
            else if (model.ControlType == FieldControlType.Date && model.DataType == FieldDataType.DateTimeNullable)
            {
                result.Append(helper.Partial("FieldDate", model));
            }
            else if (model.ControlType == FieldControlType.DateTime && model.DataType == FieldDataType.DateTime)
            {
                result.Append(helper.Partial("FieldDateTime", model));
            }
            else if (model.ControlType == FieldControlType.DateTime && model.DataType == FieldDataType.DateTimeNullable)
            {
                result.Append(helper.Partial("FieldDateTime", model));
            }
            else if (model.ControlType == FieldControlType.DropDownList)
            {
                if (model.DataSource == FieldDataSource.CSV)
                    result.Append(helper.Partial("FieldDropDownList", model));
                else if (model.DataSource == FieldDataSource.MVC)
                    result.Append(helper.Partial("FieldComboBoxMVC", model));
                else if (model.DataSource == FieldDataSource.DB)
                    result.Append(helper.Partial("FieldComboBoxDB", model));
                else if (model.DataSource == FieldDataSource.SQL)
                    result.Append(helper.Partial("FieldComboBoxSQL", model));
            }
            else if (model.ControlType == FieldControlType.MultiSelectList)
            {
                if (model.DataSource == FieldDataSource.CSV)
                    result.Append(helper.Partial("FieldMultiSelect", model));
                else if (model.DataSource == FieldDataSource.MVC)
                    result.Append(helper.Partial("FieldMultiSelectMVC", model));
                else if (model.DataSource == FieldDataSource.DB)
                    result.Append(helper.Partial("FieldMultiSelectDB", model));
            }
            else if (model.ControlType == FieldControlType.Lookup)
            {
                result.Append(helper.Partial("FieldLookup", model));
            }
            return MvcHtmlString.Create(result.ToString());
        }

        #endregion

        #region Lookup

        /// <summary>
        /// Wrapper of Lookup for Item
        /// </summary>
        public static MvcHtmlString ItemLookupFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string dataBind = "")
        {
            return LookupFor(helper, expression, "GetChoices", "Common",
                new RouteValueDictionary {
                    { "dbTable", "Item" }, { "dbTextColumn", "Name" }, { "dbValueColumn", "Id" },
                    { "lookupType", "Item" }, { "viewType", "SLItemView" },{ "selectedEvent", "SLItemSelected" }, { "valueFieldId", "ItemId" }
                }, true, dataBind);
        }

        /// <summary>
        /// Wrapper of Lookup for Asset
        /// </summary>
        public static MvcHtmlString AssetLookupFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string dataBind = "")
        {
            return LookupFor(helper, expression, "AssetList", "Common",
                new RouteValueDictionary { { "lookupType", "Asset" }, { "viewType", "SLAssetView" }, { "selectedEvent", "SLAssetSelected" },
                    { "valueFieldId", "AssetId" }, { "parentFieldName", "SiteId" }
                }, true, dataBind);
        }

        /// <summary>
        /// Wrapper of Lookup for Location
        /// </summary>
        public static MvcHtmlString LocationLookupFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string dataBind = "")
        {
            return LookupFor(helper, expression, "LocationList", "Common",
                new RouteValueDictionary { { "lookupType", "Location" }, { "viewType", "SLLocationView" }, { "selectedEvent", "SLLocationSelected" },
                    { "valueFieldId", "LocationId" }, { "parentFieldName", "SiteId" }
                }, true, dataBind);
        }

        /// <summary>
        /// Wrapper of Lookup for Store
        /// </summary>
        public static MvcHtmlString StoreLookupFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string dataBind = "")
        {
            return LookupFor(helper, expression, "StoreList", "Common",
                new RouteValueDictionary { { "lookupType", "Store" }, { "viewType", "SLStoreView" }, { "selectedEvent", "SLStoreSelected" },
                    { "valueFieldId", "StoreId" }, { "parentFieldName", "SiteId" }
                }, true, dataBind);
        }

        /// <summary>
        /// Wrapper of Lookup for WorkOrder
        /// </summary>
        public static MvcHtmlString WorkOrderLookupFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string valueFieldId = "", string dataBind = "")
        {
            if (string.IsNullOrEmpty(valueFieldId))
                valueFieldId = "WorkOrderId";
            return LookupFor(helper, expression, "WorkOrderList", "Common",
                new RouteValueDictionary { { "lookupType", "WorkOrder" }, { "viewType", "SLWorkOrderView" }, { "selectedEvent", "SLWorkOrderSelected" },
                    { "valueFieldId", valueFieldId }, { "parentFieldName", "SiteId" }
                }, true, dataBind);
        }

        /// <summary>
        /// Wrapper of Lookup for Service Request
        /// </summary>
        public static MvcHtmlString ServiceRequestLookupFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string dataBind = "")
        {
            return LookupFor(helper, expression, "ServiceRequestList", "Common",
                new RouteValueDictionary { { "lookupType", "ServiceRequest" }, { "viewType", "SLServiceRequestView" }, { "selectedEvent", "SLServiceRequestSelected" },
                    { "valueFieldId", "ServiceRequestId" }, { "parentFieldName", "SiteId" }
                }, true, dataBind);
        }
        #endregion
    }
}
