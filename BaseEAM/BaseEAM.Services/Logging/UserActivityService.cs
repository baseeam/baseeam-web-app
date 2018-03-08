/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Linq;
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Data;
using System.Collections.Generic;
using System;

namespace BaseEAM.Services
{
    /// <summary>
    /// User activity service
    /// </summary>
    public class UserActivityService : BaseService, IUserActivityService
    {
        #region Fields

        private const int _deleteNumberOfEntries = 1000;

        private readonly IRepository<ActivityLog> _activityLogRepository;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        public UserActivityService(
            IRepository<ActivityLog> activityLogRepository,
            IRepository<ActivityLogType> activityLogTypeRepository,
            IRepository<User> userRepository,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._activityLogRepository = activityLogRepository;
            this._activityLogTypeRepository = activityLogTypeRepository;
            this._userRepository = userRepository;
            this._workContext = workContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual void InsertActivityLog(string name, string comment)
        {
            var user = _workContext.CurrentUser;
            if (user == null)
                return;
            var activityLogType = _activityLogTypeRepository.GetAll().SingleOrDefault(a => a.Name == name);
            if (activityLogType == null)
                return;
            var activityLog = new ActivityLog
            {
                ActivityLogTypeId = activityLogType.Id,
                UserId = user.Id,
                Comment = comment
            };

            _activityLogRepository.InsertAndCommit(activityLog);
        }

        public virtual void UpdateActivityLog(ActivityLog activityLog)
        {
            activityLog.ModifiedDateTime = DateTime.UtcNow;
            _activityLogRepository.UpdateAndCommit(activityLog);
        }

        public virtual ActivityLog GetActivityLog(string name, string comment)
        {
            var user = _workContext.CurrentUser;
            if (user == null)
                return null;
            var activityLog = _activityLogRepository.GetAll()
                .Where(a => a.UserId == user.Id && a.ActivityLogType.Name == name && a.Comment == comment)
                .FirstOrDefault();

            return activityLog;
        }

        public virtual List<ActivityLog> GetActivityLogs(string name)
        {
            var user = _workContext.CurrentUser;
            if (user == null)
                return null;
            return _activityLogRepository.GetAll()
                .Where(a => a.UserId == user.Id && a.ActivityLogType.Name == name)
                .OrderByDescending(a => a.ModifiedDateTime)
                .ToList();
        }

        #endregion
    }
}
