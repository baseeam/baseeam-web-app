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
    public class TaskGroupService : BaseService, ITaskGroupService
    {
        #region Fields

        private readonly IRepository<TaskGroup> _taskGroupRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public TaskGroupService(IRepository<TaskGroup> taskGroupRepository,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._taskGroupRepository = taskGroupRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<TaskGroup> GetTaskGroups(string expression,
            dynamic parataskGroups,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.TaskGroupSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parataskGroups);
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
            var count = countBuilder.AddTemplate(SqlTemplate.TaskGroupSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parataskGroups);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var taskGroups = connection.Query<TaskGroup>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<TaskGroup>(taskGroups, totalCount);
            }
        }

        #endregion
    }
}
