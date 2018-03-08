/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using Common.Logging;
using Quartz;
using System;

namespace BaseEAM.Services
{
    public class HeartbeatJob : IJob
    {
        private readonly IHeartbeatService _hearbeat;
        private static readonly ILog s_log = LogManager.GetLogger<HeartbeatJob>();

        public HeartbeatJob(IHeartbeatService hearbeat)
        {
            if (hearbeat == null) throw new ArgumentNullException(nameof(hearbeat));
            _hearbeat = hearbeat;
        }

        public void Execute(IJobExecutionContext context)
        {
            _hearbeat.UpdateServiceState("alive");
        }
    }

    public class HeartbeatService : IHeartbeatService
    {
        private static readonly ILog s_log = LogManager.GetLogger<HeartbeatService>();

        public void UpdateServiceState(string state)
        {
            s_log.InfoFormat("Service state: {0}.", state);
        }
    }

    public interface IHeartbeatService
    {
        void UpdateServiceState(string state);
    }
}
