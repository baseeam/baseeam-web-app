using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(CommentValidator))]
    public class CommentModel : BaseEamEntityModel
    {
        public long? EntityId { get; set; }

        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("Comment.Message")]
        public string Message { get; set; }
    }
}