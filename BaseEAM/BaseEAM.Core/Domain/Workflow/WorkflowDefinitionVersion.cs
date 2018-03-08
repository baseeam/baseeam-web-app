/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Core.Domain
{
    public class WorkflowDefinitionVersion : BaseEntity
    {
        public long? WorkflowDefinitionId { get; set; }
        public virtual WorkflowDefinition WorkflowDefinition { get; set; }

        public int? WorkflowVersion { get; set; }

        public string WorkflowXamlFileName { get; set; }
        public string WorkflowXamlFileContent { get; set; }
        public string WorkflowPictureFileName { get; set; }
        public byte[] WorkflowPictureFileContent { get; set; }
    }
}
