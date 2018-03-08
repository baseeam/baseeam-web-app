/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Core.Domain
{
    public class WorkflowServiceParameter
    {
        public long CurrentUserId { get; set; }
        public long EntityId { get; set; }
        public string EntityType { get; set; }
        public string WorkflowInstanceId { get; set; }
        public long WorkflowDefinitionId { get; set; }
        public int WorkflowVersion { get; set; }
        public string ActionName { get; set; }
        public string Comment { get; set; }
    }
}
