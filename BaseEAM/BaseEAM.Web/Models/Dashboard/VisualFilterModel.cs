/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    public class VisualFilterModel : BaseEamEntityModel
    {
        public long? VisualId { get; set; }

        public string Name { get; set; }

        [BaseEamResourceDisplayName("Filter")]
        public long? FilterId { get; set; }
        public string FilterName { get; set; }

        [BaseEamResourceDisplayName("ParentVisualFilter")]
        public long? ParentVisualFilterId { get; set; }
        public string ParentVisualFilterFilterName { get; set; }

        [BaseEamResourceDisplayName("VisualFilter.DisplayOrder")]
        [UIHint("Int32Nullable")]
        public int? DisplayOrder { get; set; }

        [BaseEamResourceDisplayName("VisualFilter.DbColumn")]
        public string DbColumn { get; set; }

        [BaseEamResourceDisplayName("VisualFilter.ResourceKey")]
        public string ResourceKey { get; set; }

        [BaseEamResourceDisplayName("VisualFilter.IsRequired")]
        public bool IsRequired { get; set; }
    }
}