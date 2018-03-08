using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(AssetValidator))]
    public class AssetModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Asset.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Parent")]
        public long? ParentId { get; set; }
        [BaseEamResourceDisplayName("Parent")]
        public string ParentName { get; set; }
        public AssetModel Parent { get; set; }

        [BaseEamResourceDisplayName("Common.HierarchyNamePath")]
        public string HierarchyNamePath { get; set; }

        [BaseEamResourceDisplayName("Site")]
        public long? SiteId { get; set; }
        public SiteModel Site { get; set; }

        [BaseEamResourceDisplayName("AssetType")]
        public long? AssetTypeId { get; set; }
        public ValueItemModel AssetType { get; set; }

        [BaseEamResourceDisplayName("AssetStatus")]
        public long? AssetStatusId { get; set; }
        public ValueItemModel AssetStatus { get; set; }

        [BaseEamResourceDisplayName("Location")]
        public long? LocationId { get; set; }
        public LocationModel Location { get; set; }

        [BaseEamResourceDisplayName("Asset.SerialNumber")]
        public string SerialNumber { get; set; }

        [BaseEamResourceDisplayName("Asset.Barcode")]
        public string Barcode { get; set; }

        [BaseEamResourceDisplayName("Manufacturer")]
        public long? ManufacturerId { get; set; }
        public CompanyModel Manufacturer { get; set; }

        [BaseEamResourceDisplayName("Vendor")]
        public long? VendorId { get; set; }
        public CompanyModel Vendor { get; set; }

        [BaseEamResourceDisplayName("Asset.InstallationDate")]
        [UIHint("DateNullable")]
        public DateTime? InstallationDate { get; set; }

        [BaseEamResourceDisplayName("Asset.InstallationCost")]
        [UIHint("DecimalNullable")]
        public decimal? InstallationCost { get; set; }

        [BaseEamResourceDisplayName("Asset.PurchasePrice")]
        [UIHint("DecimalNullable")]
        public decimal? PurchasePrice { get; set; }

        [BaseEamResourceDisplayName("Asset.Period")]
        [UIHint("Int32Nullable")]
        public int? Period { get; set; }

        [BaseEamResourceDisplayName("Asset.WarrantyStartDate")]
        [UIHint("DateNullable")]
        public DateTime? WarrantyStartDate { get; set; }

        [BaseEamResourceDisplayName("Asset.WarrantyEndDate")]
        [UIHint("DateNullable")]
        public DateTime? WarrantyEndDate { get; set; }

        [BaseEamResourceDisplayName("Common.Picture")]
        public long? PictureId { get; set; }

		//Cache MeterGroupId from Asset if existing
        [BaseEamResourceDisplayName("MeterGroup")]
        public long? MeterGroupId { get; set; }

        //Cache PointId from Asset if existing
        [BaseEamResourceDisplayName("Point")]
        public long PointId { get; set; }
    }
}