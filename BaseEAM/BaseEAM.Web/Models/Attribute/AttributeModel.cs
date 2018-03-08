using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(AttributeValidator))]
    public class AttributeModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Attribute.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Attribute.ResourceKey")]
        public string ResourceKey { get; set; }

        [BaseEamResourceDisplayName("Attribute.ControlType")]
        public AttributeControlType ControlType { get; set; }
        public string ControlTypeText { get; set; }

        [BaseEamResourceDisplayName("Attribute.DataType")]
        public AttributeDataType DataType { get; set; }
        public string DataTypeText { get; set; }

        [BaseEamResourceDisplayName("Attribute.DataSource")]
        public AttributeDataSource DataSource { get; set; }
        public string DataSourceText { get; set; }

        [BaseEamResourceDisplayName("Attribute.CsvTextList")]
        public string CsvTextList { get; set; }

        [BaseEamResourceDisplayName("Attribute.CsvValueList")]
        public string CsvValueList { get; set; }
    }
}