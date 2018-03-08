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
    public class AssetService : BaseService, IAssetService
    {
        #region Fields

        private readonly IRepository<Asset> _assetRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AssetService(IRepository<Asset> assetRepository,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._assetRepository = assetRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<Asset> GetAssets(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.AssetSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Asset.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.AssetSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var assets = connection.Query<Asset, ValueItem, ValueItem, Location, Site, Asset>(search.RawSql,
                    (asset, assetType, assetStatus, location, site) => { asset.AssetType = assetType; asset.AssetStatus = assetStatus; asset.Location = location;  asset.Site = site; return asset; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Asset>(assets, totalCount);
            }
        }

        public virtual List<Asset> GetAllAssetsByParentId(long? parentId)
        {
            var assets = _assetRepository.GetAll().Where(c => c.ParentId == parentId).OrderBy(c => c.Name).ToList();
            return assets;
        }

        #endregion
    }
}
