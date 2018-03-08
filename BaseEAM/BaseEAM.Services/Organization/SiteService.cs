/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BaseEAM.Services
{
    public class SiteService : BaseService, ISiteService
    {
        #region Fields

        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<Asset> _assetRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<Team> _teamRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Technician> _technicianRepository;
        private readonly IRepository<Point> _pointRepository;
        private readonly DapperContext _dapperContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public SiteService(IRepository<Site> siteRepository,
            IRepository<Asset> assetRepository,
            IRepository<Location> locationRepository,
            IRepository<Team> teamRepository,
            IRepository<User> userRepository,
            IRepository<Technician> technicianRepository,
            IRepository<Point> pointRepository,
            DapperContext dapperContext)
        {
            this._siteRepository = siteRepository;
            this._assetRepository = assetRepository;
            this._locationRepository = locationRepository;
            this._teamRepository = teamRepository;
            this._userRepository = userRepository;
            this._technicianRepository = technicianRepository;
            this._pointRepository = pointRepository;
            this._dapperContext = dapperContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<Site> GetSites(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var builder = new SqlBuilder();
            var search = builder.AddTemplate(SqlTemplate.SiteSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                builder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    builder.OrderBy(s.ToExpression());
            }
            else
            {
                builder.OrderBy("Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.SiteSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var sites = connection.Query<Site>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Site>(sites, totalCount);
            }
        }

        public virtual List<Site> GetSites(User user, DateTime? modifiedDateTime)
        {
            var securityGroupIds = user.SecurityGroups.Select(s => s.Id).ToList();
            var sites = _siteRepository.GetAll()
                .Where(s => s.SecurityGroups.Any(g => securityGroupIds.Contains(g.Id))
                            && s.ModifiedDateTime >= modifiedDateTime)
                .ToList();

            return sites;
        }

        public virtual List<Asset> GetAssets(User user, DateTime? modifiedDateTime)
        {
            var securityGroupIds = user.SecurityGroups.Select(s => s.Id).ToList();
            var assets = _assetRepository.GetAll()
                .Where(a => a.Site.SecurityGroups.Any(g => securityGroupIds.Contains(g.Id))
                        && a.ModifiedDateTime >= modifiedDateTime)
                .ToList();

            return assets;
        }

        public virtual List<Location> GetLocations(User user, DateTime? modifiedDateTime)
        {
            var securityGroupIds = user.SecurityGroups.Select(s => s.Id).ToList();
            var locations = _locationRepository.GetAll()
                .Where(a => a.Site.SecurityGroups.Any(g => securityGroupIds.Contains(g.Id))
                        && a.ModifiedDateTime >= modifiedDateTime)
                .ToList();

            return locations;
        }

        public virtual List<Team> GetTeams(User user, DateTime? modifiedDateTime)
        {
            var securityGroupIds = user.SecurityGroups.Select(s => s.Id).ToList();
            var teams = _teamRepository.GetAll()
                .Where(a => a.Site.SecurityGroups.Any(g => securityGroupIds.Contains(g.Id))
                        && a.ModifiedDateTime >= modifiedDateTime)
                .ToList();

            return teams;
        }

        public virtual List<User> GetUsers(User user, DateTime? modifiedDateTime)
        {
            var securityGroupIds = user.SecurityGroups.Select(s => s.Id).ToList();
            var siteIds = _siteRepository.GetAll()
                .Where(s => s.SecurityGroups.Any(g => securityGroupIds.Contains(g.Id)))
                .Select(s => s.Id)
                .ToList();
            var users = _userRepository.GetAll()
                .Where(u => u.SecurityGroups.Any(g => g.Sites.Any(s => siteIds.Contains(s.Id)))
                            && u.ModifiedDateTime >= modifiedDateTime)
                .ToList();

            return users;
        }

        public virtual List<Technician> GetTechnicians(User user, DateTime? modifiedDateTime)
        {
            var securityGroupIds = user.SecurityGroups.Select(s => s.Id).ToList();
            var siteIds = _siteRepository.GetAll()
                .Where(s => s.SecurityGroups.Any(g => securityGroupIds.Contains(g.Id)))
                .Select(s => s.Id)
                .ToList();
            var technicians = _technicianRepository.GetAll()
                .Where(t => t.User.SecurityGroups.Any(g => g.Sites.Any(s => siteIds.Contains(s.Id)))
                            && t.ModifiedDateTime >= modifiedDateTime)
                .ToList();

            return technicians;
        }

        public virtual List<Point> GetPoints(User user, DateTime? modifiedDateTime)
        {
            var securityGroupIds = user.SecurityGroups.Select(s => s.Id).ToList();
            var points = _pointRepository.GetAll()
                .Where(p => ((p.AssetId != null && p.Asset.Site.SecurityGroups.Any(g => securityGroupIds.Contains(g.Id)))
                            || (p.LocationId != null && p.Location.Site.SecurityGroups.Any(g => securityGroupIds.Contains(g.Id))))
                        && p.ModifiedDateTime >= modifiedDateTime)
                .Include(p => p.PointMeterLineItems)
                .ToList();

            return points;
        }

        #endregion
    }
}
