/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

//code from Telerik MVC Extensions

using System;
using System.IO;
using System.Linq;
using System.Xml;
using BaseEAM.Core;
using BaseEAM.Core.Infrastructure;
using BaseEAM.Services;

namespace BaseEAM.Web.Framework.Menu
{
    public class XmlSiteMap
    {
        public XmlSiteMap()
        {
            RootNode = new SiteMapNode();
        }

        public SiteMapNode RootNode { get; set; }

        public virtual void LoadFrom(string physicalPath)
        {
            string filePath = CommonHelper.MapPath(physicalPath);
            string content = File.ReadAllText(filePath);

            if (!string.IsNullOrEmpty(content))
            {
                using (var sr = new StringReader(content))
                {
                    using (var xr = XmlReader.Create(sr,
                            new XmlReaderSettings
                            {
                                CloseInput = true,
                                IgnoreWhitespace = true,
                                IgnoreComments = true,
                                IgnoreProcessingInstructions = true
                            }))
                    {
                        var doc = new XmlDocument();
                        doc.Load(xr);

                        if ((doc.DocumentElement != null) && doc.HasChildNodes)
                        {
                            XmlNode xmlRootNode = doc.DocumentElement.FirstChild;
                            Iterate(RootNode, xmlRootNode);
                            PopulateAnalyticsNode(RootNode);
                            //If all child nodes are invisible => make the node invisible too
                            MakeInvisible(RootNode);
                        }
                    }
                }
            }
        }

        private static void Iterate(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            PopulateNode(siteMapNode, xmlNode);

            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.LocalName.Equals("siteMapNode", StringComparison.InvariantCultureIgnoreCase))
                {
                    var siteMapChildNode = new SiteMapNode();
                    siteMapNode.ChildNodes.Add(siteMapChildNode);

                    Iterate(siteMapChildNode, xmlChildNode);
                }
            }
        }

        private static void PopulateNode(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            //systemName
            var systemName = GetStringValueFromAttribute(xmlNode, "systemName");
            siteMapNode.SystemName = systemName;

            //title
            var resourceKey = GetStringValueFromAttribute(xmlNode, "resourceKey");
            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            siteMapNode.Title = localizationService.GetResource(resourceKey);

            //routes, url
            string controllerName = GetStringValueFromAttribute(xmlNode, "controller");
            string actionName = GetStringValueFromAttribute(xmlNode, "action");
            string url = GetStringValueFromAttribute(xmlNode, "url");
            if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
            {
                siteMapNode.ControllerName = controllerName;
                siteMapNode.ActionName = actionName;
            }
            else if (!string.IsNullOrEmpty(url))
            {
                siteMapNode.Url = url;
            }

            //image URL
            siteMapNode.IconClass = GetStringValueFromAttribute(xmlNode, "IconClass");

            //permission name
            var permissionNames = GetStringValueFromAttribute(xmlNode, "PermissionNames");
            if (!string.IsNullOrEmpty(permissionNames))
            {
                var permissionService = EngineContext.Current.Resolve<IPermissionService>();
                siteMapNode.Visible = permissionNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Any(permissionName => permissionService.Authorize(permissionName.Trim()));
            }
            else
            {
                siteMapNode.Visible = true;
            }

            // Open URL in new tab
            var openUrlInNewTabValue = GetStringValueFromAttribute(xmlNode, "OpenUrlInNewTab");
            bool booleanResult;
            if (!string.IsNullOrWhiteSpace(openUrlInNewTabValue) && bool.TryParse(openUrlInNewTabValue, out booleanResult))
            {
                siteMapNode.OpenUrlInNewTab = booleanResult;
            }
        }

        private static void PopulateAnalyticsNode(SiteMapNode rootNode)
        {
            var reportService = EngineContext.Current.Resolve<IReportService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var reports = reportService.GetReportsByUser(workContext.CurrentUser);
            var reportTypes = reports.Select(r => r.Type).Distinct();

            var analyticsNode = rootNode.ChildNodes.Where(c => c.SystemName == "Analytics").FirstOrDefault();
            foreach (var type in reportTypes)
            {
                var typeNode = new SiteMapNode { Title = type, Visible = true };
                analyticsNode.ChildNodes.Add(typeNode);
            }

            foreach (var report in reports)
            {
                var parentNode = analyticsNode.ChildNodes.Where(c => c.Title == report.Type).FirstOrDefault();
                var node = new SiteMapNode();
                node.Title = report.Name;
                node.ControllerName = "ReportViewer";
                node.ActionName = "View";
                node.RouteValues = new System.Web.Routing.RouteValueDictionary { { "Id", report.Id } };
                node.Visible = true;

                parentNode.ChildNodes.Add(node);
            }
        }

        private static void MakeInvisible(SiteMapNode node)
        {
            if(node.ChildNodes.Count > 0)
            {
                if (!node.ChildNodes.Any(n => n.Visible == true))
                {
                    node.Visible = false;
                }
                else
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        MakeInvisible(childNode);
                    }
                }                
            }

            //special handling for Analytics node
            if (node.SystemName == "Analytics" && node.ChildNodes.Count == 0)
                node.Visible = false;        
        }

        private static string GetStringValueFromAttribute(XmlNode node, string attributeName)
        {
            string value = null;

            if (node.Attributes != null && node.Attributes.Count > 0)
            {
                XmlAttribute attribute = node.Attributes[attributeName];

                if (attribute != null)
                {
                    value = attribute.Value;
                }
            }

            return value;
        }
    }
}
