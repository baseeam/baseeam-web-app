/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Web.Framework.Mvc
{
    public class TreeViewLookup
    {
        public string ValueFieldId { get; set; }
        public string TextFieldId { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string DbTable { get; set; }
        public string DbTextColumn { get; set; }
        public string DbValueColumn { get; set; }
        //pass additional field, value
        public string AdditionalField { get; set; }
        public string AdditionalValue { get; set; }

        //tree type: Code, Asset, Location, ...
        public string TreeType { get; set; }
    }
}
