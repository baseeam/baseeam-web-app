using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using System;

namespace BaseEAM.Web.Models
{
    public class AssetStatusHistoryModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Asset")]
        public long? AssetId { get; set; }
        public AssetModel Asset { get; set; }

        [BaseEamResourceDisplayName("AssetStatusHistory.FromStatus")]
        public string FromStatus { get; set; }

        [BaseEamResourceDisplayName("AssetStatusHistory.ToStatus")]
        public string ToStatus { get; set; }

        [BaseEamResourceDisplayName("ChangedUser")]
        public long? ChangedUserId { get; set; }
        public UserModel ChangedUser { get; set; }

        [BaseEamResourceDisplayName("AssetStatusHistory.ChangedDateTime")]
        public DateTime? ChangedDateTime { get; set; }
    }
}