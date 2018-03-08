/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    public class StoreItemModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Site")]
        public long? SiteId { get; set; }
        public SiteModel Site { get; set; }

        [BaseEamResourceDisplayName("Store")]
        public long? StoreId { get; set; }
        public StoreModel Store { get; set; }

        [BaseEamResourceDisplayName("Item")]
        public long? ItemId { get; set; }
        public ItemModel Item { get; set; }

        [BaseEamResourceDisplayName("StoreItem.StockType")]
        public ItemStockType StockType { get; set; }
        public string StockTypeText { get; set; }

        [BaseEamResourceDisplayName("StoreItem.LotType")]
        public ItemLotType LotType { get; set; }
        public string LotTypeText { get; set; }

        [BaseEamResourceDisplayName("StoreItem.CostingType")]
        public ItemCostingType CostingType { get; set; }
        public string CostingTypeText { get; set; }

        [BaseEamResourceDisplayName("StoreItem.StandardCostingUnitPrice")]
        [UIHint("DecimalNullable")]
        public decimal? StandardCostingUnitPrice { get; set; }

        [BaseEamResourceDisplayName("StoreItem.SafetyStock")]
        [UIHint("DecimalNullable")]
        public decimal? SafetyStock { get; set; }

        [BaseEamResourceDisplayName("StoreItem.ReorderPoint")]
        [UIHint("DecimalNullable")]
        public decimal? ReorderPoint { get; set; }

        [BaseEamResourceDisplayName("StoreItem.EconomicOrderQuantity")]
        [UIHint("DecimalNullable")]
        public decimal? EconomicOrderQuantity { get; set; }
    }
}