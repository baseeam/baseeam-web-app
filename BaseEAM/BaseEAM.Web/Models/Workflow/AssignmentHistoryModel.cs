using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Models
{
    public class AssignmentHistoryModel : BaseEamEntityModel
    {
        public long? EntityId { get; set; }
        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("AssignmentHistory.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("AssignmentHistory.Comment")]
        public string Comment { get; set; }

        [BaseEamResourceDisplayName("AssignmentHistory.TriggeredAction")]
        public string TriggeredAction { get; set; }

        [BaseEamResourceDisplayName("AssignmentHistory.AssignedUsers")]
        public string AssignedUsers { get; set; }

        public long WorkflowDefinitionId { get; set; }
        [BaseEamResourceDisplayName("WorkflowDefinition")]
        public string WorkflowDefinitionName { get; set; }
        [BaseEamResourceDisplayName("WorkflowDefinitionVersion.WorkflowVersion")]
        public int? WorkflowVersion { get; set; }
    }
}