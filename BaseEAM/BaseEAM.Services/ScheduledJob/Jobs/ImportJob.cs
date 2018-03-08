/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using Common.Logging;
using Quartz;

namespace BaseEAM.Services
{
    [DisallowConcurrentExecution]
    public class ImportJob : IJob
    {
        private readonly IImportManager _importManager;
        private static readonly ILog s_log = LogManager.GetLogger<ImportJob>();

        public ImportJob(IImportManager importManager)
        {
            _importManager = importManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            long importProfileId = dataMap.GetLong("importProfileId");
            _importManager.Execute(importProfileId);
        }
    }
}
