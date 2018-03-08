/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class ScheduledJob
    {
        public string GroupName { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }

        public string TriggerName { get; set; }
        public string TriggerGroupName { get; set; }
        public string TriggerType { get; set; }
        public int TriggerState { get; set; }

        public bool IsSimpleTrigger { get; set; }
        public int RepeatCount { get; set; }
        public int RepeatInterval { get; set; }

        public bool IsCronTrigger { get; set; }
        public string CronExpression { get; set; }

        public int MisfireInstruction { get; set; }
        public string MisfireInstructionText { get; set; }

        public DateTime NextFireTime { get; set; }
        public DateTime PreviousFireTime { get; set; }
        public double RunTime { get; set; }
    }
}
