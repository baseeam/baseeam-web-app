using System.Collections.Generic;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using Dapper;
using BaseEAM.Core;
using System.Linq;
using BaseEAM.Core.Data;

namespace BaseEAM.Services
{
    public class UnitOfMeasureService : BaseService, IUnitOfMeasureService
    {
        #region Fields

        private readonly DapperContext _dapperContext;
        private readonly IRepository<UnitOfMeasure> _unitOfMeasureRepository;

        #endregion

        #region Ctor

        public UnitOfMeasureService(IRepository<UnitOfMeasure> unitOfMeasureRepository,
            DapperContext dapperContext)
        {
            this._unitOfMeasureRepository = unitOfMeasureRepository;
            this._dapperContext = dapperContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<UnitOfMeasure> GetUnitOfMeasures(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var builder = new SqlBuilder();
            var search = builder.AddTemplate(SqlTemplate.UnitOfMeasureSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                builder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    builder.OrderBy(s.ToExpression());
            }
            else
            {
                builder.OrderBy("UnitOfMeasure.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.UnitOfMeasureSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var unitOfMeasures = connection.Query<UnitOfMeasure>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<UnitOfMeasure>(unitOfMeasures, totalCount);
            }
        }

        #endregion
    }
}
