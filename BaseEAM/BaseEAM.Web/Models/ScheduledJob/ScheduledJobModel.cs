using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using System;

namespace BaseEAM.Web.Models
{
    public class ScheduledJobModel : BaseEamModel
    {
        [BaseEamResourceDisplayName("ScheduledJob.GroupName")]
        public string GroupName { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.JobName")]
        public string JobName { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.JobDescription")]
        public string JobDescription { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.TriggerName")]
        public string TriggerName { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.TriggerGroupName")]
        public string TriggerGroupName { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.TriggerType")]
        public string TriggerType { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.TriggerState")]
        public int TriggerState { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.IsSimpleTrigger")]
        public bool IsSimpleTrigger { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.RepeatCount")]
        public int RepeatCount { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.RepeatInterval")]
        public int RepeatInterval { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.IsCronTrigger")]
        public bool IsCronTrigger { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.CronExpression")]
        public string CronExpression { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.MisfireInstruction")]
        public int MisfireInstruction { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.MisfireInstruction")]
        public string MisfireInstructionText { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.NextFireTime")]
        public DateTime NextFireTime { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.PreviousFireTime")]
        public DateTime PreviousFireTime { get; set; }

        [BaseEamResourceDisplayName("ScheduledJob.RunTime")]
        public double RunTime { get; set; }
    }
}