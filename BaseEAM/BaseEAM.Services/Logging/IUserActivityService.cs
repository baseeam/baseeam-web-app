/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    /// <summary>
    /// User activity service interface
    /// </summary>
    public partial interface IUserActivityService : IBaseService
    {
        void InsertActivityLog(string name, string comment);
        void UpdateActivityLog(ActivityLog activityLog);
        ActivityLog GetActivityLog(string name, string comment);
        List<ActivityLog> GetActivityLogs(string name);
    }
}
