/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Infrastructure;
using BaseEAM.Core.Kendoui;
using BaseEAM.Core.WebApi.Security;
using BaseEAM.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BaseEAM.Services
{
    /// <summary>
    /// User service
    /// </summary>s
    public partial class UserService : BaseService, IUserService
    {
        #region Constants

        #endregion

        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly IAssignmentGroupService _assignmentGroupService;
        private readonly IDbContext _dbContext;
        private readonly DapperContext _dapperContext;

        #endregion

        #region Ctor

        public UserService(IRepository<User> userRepository,
            IAssignmentGroupService assignmentGroupService,
            IDbContext dbContext,
            DapperContext dapperContext)
        {
            this._userRepository = userRepository;
            this._assignmentGroupService = assignmentGroupService;
            this._dbContext = dbContext;
            this._dapperContext = dapperContext;
        }

        #endregion

        #region Methods

        public virtual IPagedList<User> GetAllUsers(string searchName, int pageIndex = 0, int pageSize = 2147483647, IEnumerable<Sort> sort = null)
        {
            var query = _userRepository.GetAll();
            query = query.Where(c => c.Name.Contains(searchName));
            query = sort == null ? query.OrderBy(l => l.LoginName) : query.Sort(sort);
            var users = new PagedList<User>(query, pageIndex, pageSize);
            return users;
        }

        /// <summary>
        /// Gets all users by user format (including deleted ones)
        /// </summary>
        /// <param name="passwordFormat">Password format</param>
        /// <returns>Users</returns>
        public virtual IList<User> GetAllUsersByPasswordFormat(PasswordFormat passwordFormat)
        {
            var passwordFormatId = (int)passwordFormat;

            var query = _userRepository.Table;
            query = query.Where(c => c.PasswordFormatId == passwordFormatId);
            query = query.OrderByDescending(c => c.CreatedDateTime);
            var users = query.ToList();
            return users;
        }

        /// <summary>
        /// Get users by identifiers
        /// </summary>
        /// <param name="userIds">User identifiers</param>
        /// <returns>Users</returns>
        public virtual IList<User> GetUsersByIds(long[] userIds)
        {
            if (userIds == null || userIds.Length == 0)
                return new List<User>();

            var query = from c in _userRepository.Table
                        where userIds.Contains(c.Id)
                        select c;
            var users = query.ToList();
            //sort by passed identifiers
            var sortedUsers = new List<User>();
            foreach (long id in userIds)
            {
                var user = users.Find(x => x.Id == id);
                if (user != null)
                    sortedUsers.Add(user);
            }
            return sortedUsers;
        }

        /// <summary>
        /// Gets a user by GUID
        /// </summary>
        /// <param name="userGuid">User GUID</param>
        /// <returns>A user</returns>
        public virtual User GetUserByGuid(Guid userGuid)
        {
            if (userGuid == Guid.Empty)
                return null;

            var query = from c in _userRepository.Table
                        where c.UserGuid == userGuid
                        orderby c.Id
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        public virtual User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.Email == email
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        public virtual User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.Name == username
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user by loginName
        /// </summary>
        /// <param name="loginName">loginName</param>
        /// <returns>User</returns>
        public virtual User GetUserByLoginName(string loginName)
        {
            if (string.IsNullOrWhiteSpace(loginName))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.LoginName == loginName
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        public virtual PagedResult<User> GetUsers(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.UserSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("User.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.UserSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var users = connection.Query<User>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<User>(users, totalCount);
            }
        }

        public virtual List<User> GetUsers(string users, WorkflowBaseEntity wfEntity)
        {
            var result = new List<User>();
            //If users is a list of emails
            if (users.Contains("@"))
            {
                var emails = users.Split(';');
                foreach (var email in emails)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        var user = _userRepository.GetAll().Where(u => u.Email == email).FirstOrDefault();
                        if (user != null)
                            result.Add(user);
                        //in case the email is not in BaseEAM, such as SR was created anonymously
                        else
                            result.Add(new User { Email = email, Phone = "" });
                    }
                }
            }
            // "." represents a method invocation
            // this case is not a C# method
            else if (!users.Contains("."))
            {
                //if users is an assignment group
                //check if wfEntity has SiteId => this entity is at Site Level
                object value = wfEntity.GetType().GetProperty("SiteId").GetValue(wfEntity, null);
                long? siteId = null;
                if (value != null)
                    siteId = (long?)value;

                result = _assignmentGroupService.GetUsers(users, siteId);
            }
            //if users is a C# method
            else
            {
                // already handled in Task Activity
            }

            return result;
        }

        public virtual List<User> GetUserFromExpression(string userExpression, BaseEntity entity)
        {
            var result = new List<User>();
            //If userExpression is a list of emails
            if (userExpression.Contains("@"))
            {
                var emails = userExpression.Split(';');
                foreach (var email in emails)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        var user = _userRepository.GetAll().Where(u => u.Email == email).FirstOrDefault();
                        if (user != null)
                            result.Add(user);
                        //in case the email is not in BaseEAM, such as SR was created anonymously
                        else
                            result.Add(new User { Email = email, Phone = "" });
                    }
                }
            }
            // "." represents a method invocation
            // this case is not a C# method
            else if (!userExpression.Contains("."))
            {
                //if users is an assignment group
                //check if entity has SiteId => this entity is at Site Level
                object value = entity.GetType().GetProperty("SiteId").GetValue(entity, null);
                long? siteId = null;
                if (value != null)
                    siteId = (long?)value;

                result = _assignmentGroupService.GetUsers(userExpression, siteId);
            }
            //if userExpressions is a C# method
            else
            {
                if (userExpression.Contains(".") && userExpression.Contains("Service"))
                {
                    var array = userExpression.Split('.');
                    if (array.Length != 2)
                        throw new BaseEamException("Wrong method call syntax in workflow");
                    var serviceInterface = array[0];
                    var methodName = array[1];

                    Assembly asm = typeof(WorkflowBaseService).Assembly;
                    Type type = asm.GetType("BaseEAM.Services." + serviceInterface);
                    var service = BackgroundServiceEngine.Instance.BackgroundServiceContainerManager.Resolve(type);
                    MethodInfo method = type.GetMethod(methodName);
                    result = method.Invoke(service, new object[] { entity.Id }) as List<User>;
                }
            }

            return result;
        }

        #endregion

        #region HMAC authentication

        public virtual bool CreateKeys(User user)
        {
            if (user != null)
            {
                var hmac = new HmacAuthentication();
                string key1, key2;

                for (int i = 0; i < 9999; ++i)
                {
                    if (hmac.CreateKeys(out key1, out key2) && !_userRepository.GetAll().Any(x => x.PublicKey.ToLower().Equals(key1.ToLower())))
                    {
                        user.PublicKey = key1;
                        user.SecretKey = key2;
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual void RemoveKeys(User user)
        {
            if (user != null)
            {
                user.PublicKey = user.SecretKey = null;
            }
        }

        public virtual void EnableOrDisableUser(User user, bool enable)
        {
            if (user != null)
            {
                user.WebApiEnabled = enable;
            }
        }

        #endregion
    }
}
