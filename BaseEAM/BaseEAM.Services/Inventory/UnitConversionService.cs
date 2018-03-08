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
    public class UnitConversionService : BaseService, IUnitConversionService
    {
        #region Fields

        private readonly IRepository<UnitConversion> _unitConversionRepository;
        private readonly DapperContext _dapperContext;

        #endregion

        #region Ctor

        public UnitConversionService(DapperContext dapperContext,
            IRepository<UnitConversion> unitConversionRepository)
        {
            this._dapperContext = dapperContext;
            this._unitConversionRepository = unitConversionRepository;
        }

        #endregion

        #region Methods

        public virtual PagedResult<UnitConversion> GetUnitConversions(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var builder = new SqlBuilder();
            var search = builder.AddTemplate(SqlTemplate.UnitConversionSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                builder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    builder.OrderBy(s.ToExpression());
            }
            else
            {
                builder.OrderBy("FromUnitOfMeasure.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.UnitConversionSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var unitConversions = connection.Query<UnitConversion, UnitOfMeasure, UnitOfMeasure, UnitConversion>(search.RawSql, (unitConversion, fromUnitOfMeasure, toUnitOfMeasure) => { unitConversion.FromUnitOfMeasure = fromUnitOfMeasure; unitConversion.ToUnitOfMeasure = toUnitOfMeasure; return unitConversion; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<UnitConversion>(unitConversions, totalCount);
            }
        }

        public virtual UnitConversion GetUnitConversion(long fromUnitOfMeasureId, long toUnitOfMeasureId)
        {
            var result = _unitConversionRepository.GetAll()
                .Where(u => u.FromUnitOfMeasureId == fromUnitOfMeasureId && u.ToUnitOfMeasureId == toUnitOfMeasureId)
                .FirstOrDefault();
            return result;
        }

        public virtual List<UnitOfMeasure> GetFromUOMs(long toUnitOfMeasureId)
        {
            var result = _unitConversionRepository.GetAll()
                .Where(u => u.ToUnitOfMeasureId == toUnitOfMeasureId)
                .Select(u => u.FromUnitOfMeasure)
                .ToList();
            return result;
        }

        #endregion
    }
}
