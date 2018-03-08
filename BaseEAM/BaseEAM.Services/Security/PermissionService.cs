/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Caching;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseEAM.Services
{
    /// <summary>
    /// Permission service
    /// </summary>
    public partial class PermissionService : IPermissionService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : security group Id
        /// {1} : permission system name
        /// </remarks>
        private const string PERMISSIONS_ALLOWED_KEY = "baseeam.permission.allowed-{0}-{1}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PERMISSIONS_PATTERN_KEY = "baseeam.permission.";
        #endregion

        #region Fields

        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        private readonly IRepository<Module> _moduleRepository;
        private readonly IRepository<Feature> _featureRepository;
        private readonly IRepository<FeatureAction> _featureActionRepository;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly ICacheManager _cacheManager;
        private readonly DapperContext _dapperContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="permissionRecordRepository">Permission repository</param>
        /// <param name="workContext">Work context</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="languageService">Language service</param>
        /// <param name="cacheManager">Cache manager</param>
        public PermissionService(IRepository<PermissionRecord> permissionRecordRepository,
            IRepository<Module> moduleRepository,
            IRepository<Feature> featureRepository,
            IRepository<FeatureAction> featureActionRepository,
            IWorkContext workContext,
            ILocalizationService localizationService,
            ILanguageService languageService,
            ICacheManager cacheManager,
            DapperContext dapperContext)
        {
            this._permissionRecordRepository = permissionRecordRepository;
            this._moduleRepository = moduleRepository;
            this._featureRepository = featureRepository;
            this._featureActionRepository = featureActionRepository;
            this._workContext = workContext;
            this._localizationService = localizationService;
            this._languageService = languageService;
            this._cacheManager = cacheManager;
            this._dapperContext = dapperContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordName">Permission record name</param>
        /// <param name="securityGroup">Security Group</param>
        /// <returns>true - authorized; otherwise, false</returns>
        protected virtual bool Authorize(string permissionRecordName, SecurityGroup securityGroup)
        {
            if (String.IsNullOrEmpty(permissionRecordName))
                return false;

            string key = string.Format(PERMISSIONS_ALLOWED_KEY, securityGroup.Id, permissionRecordName);
            return _cacheManager.Get(key, () =>
            {
                foreach (var permission in securityGroup.PermissionRecords)
                    if (permission.Name.Equals(permissionRecordName, StringComparison.InvariantCultureIgnoreCase))
                        return true;

                return false;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual void DeletePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionRecordRepository.DeleteAndCommit(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        /// <returns>Permission</returns>
        public virtual PermissionRecord GetPermissionRecordById(long permissionId)
        {
            if (permissionId == 0)
                return null;

            return _permissionRecordRepository.GetById(permissionId);
        }

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="name">Permission name</param>
        /// <returns>Permission</returns>
        public virtual PermissionRecord GetPermissionRecordByName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            var query = from pr in _permissionRecordRepository.Table
                        where pr.Name == name
                        orderby pr.Id
                        select pr;

            var permissionRecord = query.FirstOrDefault();
            return permissionRecord;
        }

        /// <summary>
        /// Gets all permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IList<PermissionRecord> GetAllPermissionRecords()
        {
            var query = from pr in _permissionRecordRepository.Table
                        orderby pr.Name
                        select pr;
            var permissions = query.ToList();
            return permissions;
        }

        /// <summary>
        /// Inserts a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual void InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionRecordRepository.InsertAndCommit(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual void UpdatePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionRecordRepository.UpdateAndCommit(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(PermissionRecord permission)
        {
            return Authorize(permission, _workContext.CurrentUser);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(PermissionRecord permission, User user)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;

            return Authorize(permission.Name, user);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordName">Permission system name</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(string permissionRecordName)
        {
            return Authorize(permissionRecordName, _workContext.CurrentUser);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordName">Permission record name</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(string permissionRecordName, User user)
        {
            if (String.IsNullOrEmpty(permissionRecordName))
                return false;

            var securityGroups = user.SecurityGroups;
            foreach (var group in securityGroups)
                if (Authorize(permissionRecordName, group))
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        public virtual void InsertPermissionRecord(string moduleName, string featureName, string featureActionName)
        {
            int displayOrder = 0;
            var module = _moduleRepository.GetAll().FirstOrDefault(m => m.Name == moduleName);
            if(module == null)
            {
                displayOrder = _moduleRepository.GetAll().Max(m => m.DisplayOrder);
                module = new Module { Name = moduleName, Description = moduleName + " Module", DisplayOrder = displayOrder + 1 };
                _moduleRepository.InsertAndCommit(module);
            }

            var feature = _featureRepository.GetAll().FirstOrDefault(f => f.Name == featureName);
            if (feature == null)
            {
                displayOrder = _featureRepository.GetAll().Where(f => f.ModuleId == module.Id).Max(m => m.DisplayOrder);
                feature = new Feature { Name = moduleName, Description = featureName + " Feature", DisplayOrder = displayOrder + 1 };
                _featureRepository.InsertAndCommit(feature);
            }

            var featureAction = _featureActionRepository.GetAll().FirstOrDefault(f => f.Name == featureActionName);
            if (module == null)
            {
                displayOrder = _featureActionRepository.GetAll().Where(f => f.FeatureId == feature.Id).Max(m => m.DisplayOrder);
                featureAction = new FeatureAction { Name = featureActionName, Description = featureActionName, DisplayOrder = displayOrder + 1 };
                _featureActionRepository.InsertAndCommit(featureAction);
            }

            var permissionRecord = _permissionRecordRepository.GetAll()
                .FirstOrDefault(p => p.ModuleId == module.Id && p.FeatureId == feature.Id && p.FeatureActionId == featureAction.Id);
            if(permissionRecord == null)
            {
                permissionRecord = new PermissionRecord
                {
                    ModuleId = module.Id,
                    FeatureId = feature.Id,
                    FeatureActionId = featureAction.Id,
                    Name = string.Format("{0}.{1}.{2}", module.Name.Replace(" ", ""), feature.Name.Replace(" ", ""), featureAction.Name.Replace(" ", ""))                    
                };

                _permissionRecordRepository.InsertAndCommit(permissionRecord);
            }
        }

        public virtual PagedResult<PermissionRecord> GetPermissionRecords(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.PermissionRecordSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.PermissionRecordSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var permissionRecords = connection.Query<PermissionRecord>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<PermissionRecord>(permissionRecords, totalCount);
            }
        }

        #endregion
    }
}
