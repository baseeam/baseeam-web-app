/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Web.Mvc;

namespace BaseEAM.Web.Framework.Mvc
{
    /// <summary>
    /// BaseEam model
    /// </summary>
    [ModelBinder(typeof(BaseEamModelBinder))]
    public partial class BaseEamModel
    {
    }

    /// <summary>
    /// BaseEam entity model
    /// </summary>
    [ModelBinder(typeof(BaseEamModelBinder))]
    [Bind(Exclude= "FirstCreatedBy,LastUpdatedBy,CreatedDateTime,ModifiedDateTime,CreatedUser,ModifiedUser")]
    public class BaseEamEntityModel
    {
        public virtual long Id { get; set; }
        
        [BaseEamResourceDisplayName("Common.FirstCreatedBy")]
        public string FirstCreatedBy
        {
            get { return CreatedUser + " ON " + (CreatedDateTime.HasValue ? CreatedDateTime.Value.ToString("F") : ""); }            
        }

        [BaseEamResourceDisplayName("Common.LastUpdatedBy")]
        public string LastUpdatedBy
        {
            get { return ModifiedUser + " ON " + (ModifiedDateTime.HasValue ? ModifiedDateTime.Value.ToString("F") : ""); }
        }

        [BaseEamResourceDisplayName("Common.CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [BaseEamResourceDisplayName("Common.ModifiedDateTime")]
        public DateTime? ModifiedDateTime { get; set; }

        [BaseEamResourceDisplayName("Common.CreatedUser")]
        public string CreatedUser { get; set; }

        [BaseEamResourceDisplayName("Common.ModifiedUser")]
        public string ModifiedUser { get; set; }
        
        public bool IsNew { get; set; }
        
    }
}
