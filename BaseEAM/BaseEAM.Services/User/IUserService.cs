/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using System;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    /// <summary>
    /// User service interface
    /// </summary>
    public partial interface IUserService : IBaseService
    {
        IPagedList<User> GetAllUsers(string searchName, int pageIndex = 0, int pageSize = 2147483647, IEnumerable<Sort> sort = null); //Int32.MaxValue

        /// <summary>
        /// This will be called from Task activity, or Send Message action
        /// </summary>
        /// <param name="users">user expression</param>
        /// <param name="wfEntity"></param>
        List<User> GetUsers(string users, WorkflowBaseEntity wfEntity);

        /// <summary>
        /// This will be used to get users for AutomatedAction, SLA in background job
        /// </summary>
        /// <param name="userExpression">A string concatenation of emails; an AssignmentGroup; A C# method to be called with wfEntity param</param>
        /// <param name="entity"></param>
        List<User> GetUserFromExpression(string userExpression, BaseEntity entity);

        /// <summary>
        /// Gets all users by user format (including deleted ones)
        /// </summary>
        /// <param name="passwordFormat">Password format</param>
        /// <returns>Users</returns>
        IList<User> GetAllUsersByPasswordFormat(PasswordFormat passwordFormat);

        /// <summary>
        /// Get users by identifiers
        /// </summary>
        /// <param name="userIds">User identifiers</param>
        /// <returns>Users</returns>
        IList<User> GetUsersByIds(long[] userIds);

        /// <summary>
        /// Gets a user by GUID
        /// </summary>
        /// <param name="userGuid">User GUID</param>
        /// <returns>A user</returns>
        User GetUserByGuid(Guid userGuid);

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        User GetUserByUsername(string username);

        /// <summary>
        /// Get user by loginName
        /// </summary>
        /// <param name="loginname">loginName</param>
        /// <returns>User</returns>
        User GetUserByLoginName(string loginName);

        PagedResult<User> GetUsers(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        #region HMAC authentication

        bool CreateKeys(User user);

        void RemoveKeys(User user);

        void EnableOrDisableUser(User user, bool enable);

        #endregion
    }
}
