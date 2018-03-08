using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using System;

namespace BaseEAM.Web.Models
{
    public class AssetLocationHistoryModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Asset")]
        public long? AssetId { get; set; }
        public AssetModel Asset { get; set; }

        [BaseEamResourceDisplayName("FromLocation")]
        public long? FromLocationId { get; set; }
        public LocationModel FromLocation { get; set; }

        [BaseEamResourceDisplayName("ToLocation")]
        public long? ToLocationId { get; set; }
        public LocationModel ToLocation { get; set; }

        [BaseEamResourceDisplayName("ChangedUser")]
        public long? ChangedUserId { get; set; }
        public UserModel ChangedUser { get; set; }

        [BaseEamResourceDisplayName("AssetLocationHistory.ChangedDateTime")]
        public DateTime? ChangedDateTime { get; set; }
    }
}