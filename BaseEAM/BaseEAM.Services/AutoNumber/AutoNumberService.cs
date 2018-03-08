using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseEAM.Services
{
    public class AutoNumberService : BaseService, IAutoNumberService
    {
        #region Fields

        private readonly IRepository<AutoNumber> _autoNumberRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AutoNumberService(IRepository<AutoNumber> autoNumberRepository,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._autoNumberRepository = autoNumberRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<AutoNumber> GetAutoNumbers(string expression,
            dynamic paraautoNumbers,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.AutoNumberSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, paraautoNumbers);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("EntityType");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.AutoNumberSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, paraautoNumbers);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var autoNumbers = connection.Query<AutoNumber>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<AutoNumber>(autoNumbers, totalCount);
            }
        }

        /// <summary>
        /// Generates the next auto number based on the date, and the specified
        /// entity.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="entity"></param>
        public string GenerateNextAutoNumber<T>(DateTime date, T entity) where T : BaseEntity
        {
            //The separators must be / or -
            char[] separators = new char[] { '/', '-' };
            // Gets the entity type of the entity.
            string entityType = typeof(T).Name;
            AutoNumber autoNumber = _autoNumberRepository.GetAll().Where(r => r.EntityType == entityType).FirstOrDefault();
            int nextNumber = GenerateNextNumber(autoNumber);
            //default the format string {0:000000}
            if (autoNumber == null)
            {
                return string.Format("{0:000000}", nextNumber);
            }

            var formatString = autoNumber.FormatString;
            //The current separator is using in this autoNumber
            var separator = formatString.Contains(separators[0]) ? separators[0] : separators[1];

            string[] formatStringSections = formatString.Split(separators);
            StringBuilder result = new StringBuilder();
            foreach (var s in formatStringSections)
            {
                //format entity
                if (s.Contains("e:"))
                {
                    string entityPropertyValue = FleeExpression.Evaluate(entity, "entity." + s.Replace(" ", "").Replace("e:", "")).ToString();
                    result.Append(entityPropertyValue);
                }
                //format date
                else if (s.Contains("d:"))
                {
                    string dateFormat = GetFormat(s, "d:");
                    result.Append(string.Format(dateFormat, date));
                }
                //format number
                else if (s.Contains("n:"))
                {
                    //update the last number for this entity
                    autoNumber.LastNumber = nextNumber;
                    _autoNumberRepository.UpdateAndCommit(autoNumber);

                    string numberFormat = GetFormat(s, "n:");
                    result.Append(string.Format(numberFormat, nextNumber));
                }
                //label 
                else
                {
                    result.Append(s);
                }
                result.Append(separator);
            }
            //remove the last character "/" from result
            return result.Remove(result.Length - 1, 1).ToString();
        }

        /// <summary>
        /// Get C# format for BaseEAM format convention
        /// </summary>
        private string GetFormat(string text, string prefix)
        {
            string format = text.Replace(" ","").Replace(prefix, "{0:");
            format = format + "}";
            return format;
        }

        private int GenerateNextNumber(AutoNumber autoNumber)
        {
            if (autoNumber == null)
                return 1;
            int nextNumber = autoNumber.LastNumber + 1;
            return nextNumber;
        }
        #endregion
    }
}
