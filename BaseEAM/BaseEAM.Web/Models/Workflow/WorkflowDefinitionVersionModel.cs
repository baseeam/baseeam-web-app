using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Models
{
    public class WorkflowDefinitionVersionModel : BaseEamEntityModel
    {
        public long? WorkflowDefinitionId { get; set; }

        [BaseEamResourceDisplayName("WorkflowDefinitionVersion.WorkflowVersion")]
        public int? WorkflowVersion { get; set; }

        [BaseEamResourceDisplayName("WorkflowDefinitionVersion.WorkflowXamlFileName")]
        public string WorkflowXamlFileName { get; set; }

        [BaseEamResourceDisplayName("WorkflowDefinitionVersion.WorkflowXamlFileContent")]
        public string WorkflowXamlFileContent { get; set; }

        [BaseEamResourceDisplayName("WorkflowDefinitionVersion.WorkflowPictureFileName")]
        public string WorkflowPictureFileName { get; set; }

        [BaseEamResourceDisplayName("WorkflowDefinitionVersion.WorkflowPictureFileContent")]
        public byte[] WorkflowPictureFileContent { get; set; }
    }
}