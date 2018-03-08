/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using Dapper;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    public class ModuleService : BaseService, IModuleService
    {
        private readonly DapperContext _dapperContext;

        public ModuleService(DapperContext dapperContext)
        {
            this._dapperContext = dapperContext;
        }

        public virtual IEnumerable<Module> GetModules(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var builder = new SqlBuilder();
            var search = builder.AddTemplate(SqlTemplate.ModuleSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                builder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    builder.OrderBy(s.ToExpression());
            }
            else
            {
                builder.OrderBy("Module.DisplayOrder");
            }
            IEnumerable<Module> modules = null;

            using (var connection = _dapperContext.GetOpenConnection())
            {
                modules = connection.Query<Module>(search.RawSql, search.Parameters);
            }

            return modules;
        }
    }
}
