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
using System.Collections.Generic;
using System.Linq;

namespace BaseEAM.Services
{
    public class LocationService : BaseService, ILocationService
    {
        #region Fields

        private readonly IRepository<Location> _locationRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public LocationService(IRepository<Location> locationRepository,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._locationRepository = locationRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<Location> GetLocations(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.LocationSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Location.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.LocationSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var locations = connection.Query<Location, ValueItem, ValueItem, Site, Location>(search.RawSql, 
                    (location, locationType, locationStatus, site) => { location.LocationType = locationType; location.LocationStatus = locationStatus; location.Site = site; return location; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Location>(locations, totalCount);
            }
        }

        public virtual List<Location> GetSiteLocationList(long parentValue, string param)
        {
            var locations = _locationRepository.GetAll()
                .Where(l => l.SiteId == parentValue && l.Name.Contains(param))
                .OrderBy(l => l.Name)
                .ToList();
            return locations;
        }

        public virtual List<Location> GetAllLocationsByParentId(long? parentId)
        {
            var locations = _locationRepository.GetAll().Where(c => c.ParentId == parentId).OrderBy(c => c.Name).ToList();
            return locations;
        }

        #endregion
    }
}
