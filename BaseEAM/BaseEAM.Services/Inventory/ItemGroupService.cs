using System.Collections.Generic;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using Dapper;
using BaseEAM.Core;
using System.Linq;

namespace BaseEAM.Services
{
    public class ItemGroupService : BaseService, IItemGroupService
    {
        #region Fields

        private readonly DapperContext _dapperContext;

        #endregion

        #region Ctor

        public ItemGroupService(DapperContext dapperContext)
        {
            this._dapperContext = dapperContext;
        }

        #endregion

        #region Methods
        public virtual PagedResult<ItemGroup> GetItemGroups(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var builder = new SqlBuilder();
            var search = builder.AddTemplate(SqlTemplate.ItemGroupSearch(), new { skip = pageIndex * pageSize, take = pageSize });
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
            var count = countBuilder.AddTemplate(SqlTemplate.ItemGroupSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var itemGroups = connection.Query<ItemGroup>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<ItemGroup>(itemGroups, totalCount);
            }
        }
        #endregion
    }
}
