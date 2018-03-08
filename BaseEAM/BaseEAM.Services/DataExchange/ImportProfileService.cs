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
    public class ImportProfileService : BaseService, IImportProfileService
    {
        private readonly IRepository<ImportProfile> _importProfileRepository;
        private readonly DapperContext _dapperContext;

        public ImportProfileService(DapperContext dapperContext,
            IRepository<ImportProfile> importProfileRepository)
        {
            this._dapperContext = dapperContext;
            this._importProfileRepository = importProfileRepository;
        }

        public virtual PagedResult<ImportProfile> GetImportProfiles(string expression,
           dynamic parameters,
           int pageIndex = 0,
           int pageSize = 2147483647,
           IEnumerable<Sort> sort = null)
        {
            var builder = new SqlBuilder();
            var search = builder.AddTemplate(SqlTemplate.ImportProfileSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                builder.Where(expression, parameters);

            if (sort != null)
            {
                foreach (var s in sort)
                {
                    builder.OrderBy(s.ToExpression());
                }

            }
            else
            {
                builder.OrderBy("ImportFileName");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.ImportProfileSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var importProfiles = connection.Query<ImportProfile>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<ImportProfile>(importProfiles, totalCount);
            }
        }
    }
}
