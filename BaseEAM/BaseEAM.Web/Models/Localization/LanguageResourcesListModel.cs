/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Models
{
    public class LanguageResourcesListModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("LanguageResourcesList.SearchResourceName")]
        public string SearchResourceName { get; set; }
        [BaseEamResourceDisplayName("LanguageResourcesList.SearchResourceValue")]
        public string SearchResourceValue { get; set; }
    }
}