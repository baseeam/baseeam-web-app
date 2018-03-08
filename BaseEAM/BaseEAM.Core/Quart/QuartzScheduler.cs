/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace BaseEAM.Core
{
    public class QuartzScheduler
    {
        private ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        public string Address { get; private set; }

        public QuartzScheduler(string server, int port, string scheduler)
        {
            Address = string.Format("tcp://{0}:{1}/{2}", server, port, scheduler);
            _schedulerFactory = new StdSchedulerFactory(getProperties(Address));

            try
            {
                _scheduler = _schedulerFactory.GetScheduler();
            }
            catch (SchedulerException)
            {
                throw new BaseEamException("Unable to connect to the specified server");
            }
        }

        public IScheduler GetScheduler()
        {
            return _scheduler;
        }

        private NameValueCollection getProperties(string address)
        {
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteClient";
            properties["quartz.scheduler.proxy"] = "true";
            properties["quartz.threadPool.threadCount"] = "0";
            properties["quartz.scheduler.proxy.address"] = address;
            return properties;
        }

        public List<ScheduledJob> GetJobs()
        {
            var result = new List<ScheduledJob>();
            var jobGroups = GetScheduler().GetJobGroupNames();
            foreach (string group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = GetScheduler().GetJobKeys(groupMatcher);
                foreach (var jobKey in jobKeys)
                {
                    var detail = GetScheduler().GetJobDetail(jobKey);
                    var triggers = GetScheduler().GetTriggersOfJob(jobKey);
                    foreach (ITrigger trigger in triggers)
                    {
                        var scheduledJob = new ScheduledJob
                        {
                            GroupName = group,
                            JobName = jobKey.Name,
                            JobDescription = detail.Description,
                            TriggerName = trigger.Key.Name,
                            TriggerGroupName = trigger.Key.Group,
                            TriggerType = trigger.GetType().Name,
                            TriggerState = (int)GetScheduler().GetTriggerState(trigger.Key)
                        };

                        //simple trigger
                        if (trigger is SimpleTriggerImpl)
                        {
                            scheduledJob.IsSimpleTrigger = true;
                            scheduledJob.RepeatCount = ((SimpleTriggerImpl)trigger).RepeatCount;
                            scheduledJob.RepeatInterval = ((SimpleTriggerImpl)trigger).RepeatInterval.Milliseconds;
                        }
                        else if (trigger is CronTriggerImpl)
                        {
                            scheduledJob.IsCronTrigger = true;
                            scheduledJob.CronExpression = ((CronTriggerImpl)trigger).CronExpressionString;
                        }

                        scheduledJob.MisfireInstruction = trigger.MisfireInstruction;
                        scheduledJob.MisfireInstructionText = GetCronTriggerMisfireInstructionText(trigger.MisfireInstruction);

                        DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
                        if (nextFireTime.HasValue)
                        {
                            scheduledJob.NextFireTime = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTime.Value.DateTime);
                        }

                        DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
                        if (previousFireTime.HasValue)
                        {
                            scheduledJob.PreviousFireTime = TimeZone.CurrentTimeZone.ToLocalTime(previousFireTime.Value.DateTime);
                        }

                        result.Add(scheduledJob);
                    }
                }
            }
            return result;
        }

        public List<ScheduledJob> GetRunningJobs()
        {
            var result = new List<ScheduledJob>();
            try
            {
                var contexts = GetScheduler().GetCurrentlyExecutingJobs();
                foreach (var context in contexts)
                {
                    var scheduledJob = new ScheduledJob
                    {
                        JobName = context.JobDetail.Key.Name
                    };
                    scheduledJob.RunTime = (DateTime.Now.ToUniversalTime() - ((DateTimeOffset)context.FireTimeUtc).DateTime).TotalMinutes;
                    result.Add(scheduledJob);
                }
            }
            catch (Exception ex)
            {
                throw new BaseEamException(ex.Message);
            }

            return result;
        }

        public void ScheduleOneTimeJob(Type jobType, IDictionary<string, object> map, int hoursAfter = 0)
        {
            var dataMap = new JobDataMap(map);
            Guid id = Guid.NewGuid();
            string name = string.Format("{0}-{1}", jobType.Name, id.ToString());
            string group = id.ToString();
            IJobDetail jobDetail = JobBuilder.
                Create().
                OfType(jobType).
                WithIdentity(name, group).
                UsingJobData(dataMap).Build();
            if(hoursAfter == 0)
            {
                ITrigger trigger = TriggerBuilder.
                    Create().
                    ForJob(jobDetail).
                    WithIdentity(name, group).
                    WithSchedule(SimpleScheduleBuilder.Create().WithRepeatCount(0).WithInterval(TimeSpan.Zero)).
                    StartNow().Build();
                GetScheduler().ScheduleJob(jobDetail, trigger);
            }
            else
            {
                ITrigger trigger = TriggerBuilder.
                    Create().
                    ForJob(jobDetail).
                    WithIdentity(name, group).
                    WithSchedule(SimpleScheduleBuilder.Create().WithRepeatCount(0).WithIntervalInHours(hoursAfter)).
                    StartNow().Build();
                GetScheduler().ScheduleJob(jobDetail, trigger);
            }
        }

        private string GetMisfireInstructionText(AbstractTrigger trigger)
        {
            if (trigger is CronTriggerImpl)
            {
                return GetCronTriggerMisfireInstructionText(trigger.MisfireInstruction);
            }
            return GetSimpleTriggerMisfireInstructionText(trigger.MisfireInstruction);
        }

        private string GetSimpleTriggerMisfireInstructionText(int misfireInstruction)
        {
            switch (misfireInstruction)
            {
                case 0:
                    return "SmartPolicy";
                case 1:
                    return "FireNow";
                case 2:
                    return "RescheduleNowWithExistingRepeatCount";
                case 3:
                    return "RescheduleNowWithRemainingRepeatCount";
                case 4:
                    return "RescheduleNextWithRemainingCount";
                case 5:
                    return "RescheduleNextWithExistingCount";
                default:
                    throw new ArgumentOutOfRangeException(string.Format("{0} is not a supported misfire instruction for SimpleTrigger See Quartz.MisfireInstruction for more details.", misfireInstruction));
            }
        }

        private string GetCronTriggerMisfireInstructionText(int misfireInstruction)
        {
            switch (misfireInstruction)
            {
                case 0:
                    return "SmartPolicy";
                case 1:
                    return "FireOnceNow";
                case 2:
                    return "DoNothing";
                default:
                    throw new ArgumentOutOfRangeException(string.Format("{0} is not a supported misfire instruction for CronTrigger See Quartz.MisfireInstruction for more details.", misfireInstruction));
            }
        }
    }
}
