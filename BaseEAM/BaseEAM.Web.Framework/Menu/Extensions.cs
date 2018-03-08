/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Linq;

namespace BaseEAM.Web.Framework.Menu
{
    public static class Extensions
    {
        /// <summary>
        /// Checks whether this node or child ones has a specified controller name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool ContainsControllerName(this SiteMapNode node, string controllerName)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (String.IsNullOrWhiteSpace(controllerName))
                return false;

            if (controllerName.Equals(node.ControllerName, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return node.ChildNodes.Any(cn => ContainsControllerName(cn, controllerName));
        }
    }
}
