/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    [Table("Asset")]
    public class Asset : BaseEntity, IHierarchy
    {
        public string HierarchyIdPath { get; set; }
        public string HierarchyNamePath { get; set; }

        public long? ParentId { get; set; }
        [Write(false)]
        public virtual Asset Parent { get; set; }

        private ICollection<Asset> _children;
        [Write(false)]
        public virtual ICollection<Asset> Children
        {
            get { return _children ?? (_children = new List<Asset>()); }
            protected set { _children = value; }
        }

        public long? SiteId { get; set; }
        [Write(false)]
        public virtual Site Site { get; set; }

        public long? AssetTypeId { get; set; }
        [Write(false)]
        public virtual ValueItem AssetType { get; set; }

        public long? AssetStatusId { get; set; }
        [Write(false)]
        public virtual ValueItem AssetStatus { get; set; }

        public long? LocationId { get; set; }
        [Write(false)]
        public virtual Location Location { get; set; }

        public string Barcode { get; set; }
        public string SerialNumber { get; set; }
        public long? ManufacturerId { get; set; }
        [Write(false)]
        public virtual Company Manufacturer { get; set; }

        public long? VendorId { get; set; }
        [Write(false)]
        public virtual Company Vendor { get; set; }

        public DateTime? InstallationDate { get; set; }
        public decimal? InstallationCost { get; set; }
        public decimal? PurchasePrice { get; set; }

        public int? Period { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }

        private ICollection<AssetSparePart> _assetSpareParts;
        [Write(false)]
        public virtual ICollection<AssetSparePart> AssetSpareParts
        {
            get { return _assetSpareParts ?? (_assetSpareParts = new List<AssetSparePart>()); }
            protected set { _assetSpareParts = value; }
        }

        private ICollection<AssetStatusHistory> _assetStatusHistories;
        [Write(false)]
        public virtual ICollection<AssetStatusHistory> AssetStatusHistories
        {
            get { return _assetStatusHistories ?? (_assetStatusHistories = new List<AssetStatusHistory>()); }
            protected set { _assetStatusHistories = value; }
        }

        private ICollection<AssetLocationHistory> _assetLocationHistories;
        [Write(false)]
        public virtual ICollection<AssetLocationHistory> AssetLocationHistories
        {
            get { return _assetLocationHistories ?? (_assetLocationHistories = new List<AssetLocationHistory>()); }
            protected set { _assetLocationHistories = value; }
        }

        private ICollection<AssetDowntime> _assetDowntimes;
        [Write(false)]
        public virtual ICollection<AssetDowntime> AssetDowntimes
        {
            get { return _assetDowntimes ?? (_assetDowntimes = new List<AssetDowntime>()); }
            protected set { _assetDowntimes = value; }
        }
    }
}
