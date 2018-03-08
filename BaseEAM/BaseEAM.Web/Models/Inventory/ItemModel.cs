using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(ItemValidator))]
    public class ItemModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Item.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Item.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Item.Barcode")]
        public string Barcode { get; set; }

        [BaseEamResourceDisplayName("Item.UnitPrice")]
        [UIHint("DecimalNullable")]
        public decimal? UnitPrice { get; set; }

        [BaseEamResourceDisplayName("Item.Manufacturer")]
        public long? ManufacturerId { get; set; }
        public CompanyModel Manufacturer { get; set; }

        [BaseEamResourceDisplayName("Item.ItemGroup")]
        public long? ItemGroupId { get; set; }
        public ItemGroupModel ItemGroup { get; set; }

        [BaseEamResourceDisplayName("Item.UnitOfMeasure")]
        public long? UnitOfMeasureId { get; set; }
        public UnitOfMeasureModel UnitOfMeasure { get; set; }

        [BaseEamResourceDisplayName("Item.ItemStatus")]
        public long? ItemStatusId { get; set; }
        public ValueItemModel ItemStatus { get; set; }

        [BaseEamResourceDisplayName("Item.ItemCategory")]
        public ItemCategory ItemCategory { get; set; }
        public string ItemCategoryText { get; set; }

        [BaseEamResourceDisplayName("Common.Picture")]
        public long? PictureId { get; set; }
    }
}