/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Models
{
    public class InventorySettingsModel : BaseEamModel
    {
        [BaseEamResourceDisplayName("InventorySettings.CostingType")]
        public ItemCostingType CostingType { get; set; }
    }
}