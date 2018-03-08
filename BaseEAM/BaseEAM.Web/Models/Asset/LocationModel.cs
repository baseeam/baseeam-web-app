/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(LocationValidator))]
    public class LocationModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Location.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Location.Parent")]
        public long? ParentId { get; set; }
        [BaseEamResourceDisplayName("Location.Parent")]
        public string ParentName { get; set; }

        public string HierarchyIdPath { get; set; }

        [BaseEamResourceDisplayName("Common.HierarchyNamePath")]
        public string HierarchyNamePath { get; set; }

        [BaseEamResourceDisplayName("Location.Site")]
        public long? SiteId { get; set; }
        public SiteModel Site { get; set; }

        [BaseEamResourceDisplayName("Location.LocationType")]
        public long? LocationTypeId { get; set; }
        public ValueItemModel LocationType { get; set; }

        [BaseEamResourceDisplayName("Location.LocationStatus")]
        public long? LocationStatusId { get; set; }
        public ValueItemModel LocationStatus { get; set; }

        [BaseEamResourceDisplayName("Location.Address")]
        public long? AddressId { get; set; }
        public AddressModel Address { get; set; }

        [BaseEamResourceDisplayName("Common.Picture")]
        public long? PictureId { get; set; }

        //Cache MeterGroupId from Location if existing
        [BaseEamResourceDisplayName("MeterGroup")]
        public long? MeterGroupId { get; set; }

        //Cache PointId from Location if existing
        [BaseEamResourceDisplayName("Point")]
        public long PointId { get; set; }
    }
}