namespace BaseEAM.Web.Framework.Mvc
{
    public class Lookup
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

        public string ParentFieldName { get; set; }

        public string LookupType { get; set; }
        public string ViewType { get; set; }
        public string SelectedEvent { get; set; }
    }
}
