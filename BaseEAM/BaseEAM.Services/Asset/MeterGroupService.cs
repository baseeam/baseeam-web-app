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
    public class MeterGroupService : BaseService, IMeterGroupService
    {
        #region Fields

        private readonly IRepository<MeterGroup> _meterGroupRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public MeterGroupService(IRepository<MeterGroup> meterGroupRepository,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._meterGroupRepository = meterGroupRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<MeterGroup> GetMeterGroups(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.MeterGroupSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("MeterGroup.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.MeterGroupSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var meterGroups = connection.Query<MeterGroup>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<MeterGroup>(meterGroups, totalCount);
            }
        }

        public virtual List<MeterGroup> GetMeterGroupList(string param)
        {
            var meterGroups = _meterGroupRepository.GetAll()
                .Where(m => m.Name.Contains(param))
                .OrderBy(l => l.Name)
                .ToList();
            return meterGroups;
        }

        #endregion
    }
}