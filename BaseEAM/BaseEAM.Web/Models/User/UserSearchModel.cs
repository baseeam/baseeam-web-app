/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Models
{
    public partial class UserSearchModel : BaseEamModel
    {
        [BaseEamResourceDisplayName("User.Search.Name")]
        public string Name { get; set; }
    }
}