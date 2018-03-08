using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(WorkflowDefinitionValidator))]
    public class WorkflowDefinitionModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("WorkflowDefinition.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("WorkflowDefinition.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("EntityType")]
        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("WorkflowDefinition.IsDefault")]
        public bool IsDefault { get; set; }
    }
}