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
using System;

namespace BaseEAM.Services
{
    public class CalendarService : BaseService, ICalendarService
    {
        #region Fields

        private readonly IRepository<Calendar> _calendarRepository;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public CalendarService(IRepository<Calendar> calendarRepository,
            IDateTimeHelper dateTimeHelper,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._calendarRepository = calendarRepository;
            this._dateTimeHelper = dateTimeHelper;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<Calendar> GetCalendars(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.CalendarSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
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
            var count = countBuilder.AddTemplate(SqlTemplate.CalendarSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var calendars = connection.Query<Calendar>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Calendar>(calendars, totalCount);
            }
        }

        public bool DetermineNextDate(PreventiveMaintenance pm, ref DateTime start, ref DateTime end)
        {
            if (pm.FrequencyType == (int?)FrequencyType.Daily)
            {
                // daily
                start = start.Add(new TimeSpan(pm.FrequencyCount.Value, 0, 0, 0));
                end = end.Add(new TimeSpan(pm.FrequencyCount.Value, 0, 0, 0));
                return true;
            }
            else if (pm.FrequencyType == (int?)FrequencyType.Weekly)
            {
                // based on specific days of the week
                //
                int[] dayOfWeekSelected = new int[] {
                        pm.Sunday == true ? 1 : 0,
                        pm.Monday == true ? 1 : 0,
                        pm.Tuesday == true ? 1 : 0,
                        pm.Wednesday == true ? 1 : 0,
                        pm.Thursday == true ? 1 : 0,
                        pm.Friday == true ? 1 : 0,
                        pm.Saturday == true ? 1 : 0 };

                DateTime tempStart = start;
                DateTime tempEnd = end;

                for (int i = 0; i < 7; i++)
                {
                    tempStart = tempStart.Add(new TimeSpan(1, 0, 0, 0));
                    tempEnd = tempEnd.Add(new TimeSpan(1, 0, 0, 0));
                    //Convert tempStart and tempEnd from UTC to user time
                    var tempStartUserTime = _dateTimeHelper.ConvertToUserTime(tempStart, DateTimeKind.Utc);
                    var tempEndUserTime = _dateTimeHelper.ConvertToUserTime(tempEnd, DateTimeKind.Utc);
                    if (dayOfWeekSelected[(int)tempStartUserTime.DayOfWeek] == 1)
                    {
                        start = tempStart;
                        end = tempEnd;
                        return true;
                    }
                    if (tempStartUserTime.DayOfWeek == DayOfWeek.Saturday)
                    {
                        tempStart = tempStart.Add(new TimeSpan(7 * (pm.FrequencyCount.Value - 1), 0, 0, 0));
                        tempEnd = tempEnd.Add(new TimeSpan(7 * (pm.FrequencyCount.Value - 1), 0, 0, 0));
                    }
                }
                return false;
            }
            else if (pm.FrequencyType == (int?)FrequencyType.Monthly)
            {
                // based on specific days of the week
                //
                int[] dayOfMonthSelected = new int[] {
                        pm.Day1 == true ? 1 : 0,
                        pm.Day2 == true ? 1 : 0,
                        pm.Day3 == true ? 1 : 0,
                        pm.Day4 == true ? 1 : 0,
                        pm.Day5 == true ? 1 : 0,
                        pm.Day6 == true ? 1 : 0,
                        pm.Day7 == true ? 1 : 0,
                        pm.Day8 == true ? 1 : 0,
                        pm.Day9 == true ? 1 : 0,
                        pm.Day10 == true ? 1 : 0,
                        pm.Day11 == true ? 1 : 0,
                        pm.Day12 == true ? 1 : 0,
                        pm.Day13 == true ? 1 : 0,
                        pm.Day14 == true ? 1 : 0,
                        pm.Day15 == true ? 1 : 0,
                        pm.Day16 == true ? 1 : 0,
                        pm.Day17 == true ? 1 : 0,
                        pm.Day18 == true ? 1 : 0,
                        pm.Day19 == true ? 1 : 0,
                        pm.Day20 == true ? 1 : 0,
                        pm.Day21 == true ? 1 : 0,
                        pm.Day22== true ? 1 : 0,
                        pm.Day23 == true ? 1 : 0,
                        pm.Day24 == true ? 1 : 0,
                        pm.Day25 == true ? 1 : 0,
                        pm.Day26 == true ? 1 : 0,
                        pm.Day27 == true ? 1 : 0,
                        pm.Day28 == true ? 1 : 0,
                        pm.Day29 == true ? 1 : 0,
                        pm.Day30 == true ? 1 : 0,
                        pm.Day31 == true ? 1 : 0 };

                DateTime tempStart = start;
                DateTime tempEnd = end;
                //Convert tempStart and tempEnd from UTC to user time
                var tempStartUserTime = _dateTimeHelper.ConvertToUserTime(tempStart, DateTimeKind.Utc);
                while (tempStart <= pm.EndDateTime)
                {
                    tempStart = tempStart.Add(new TimeSpan(1, 0, 0, 0));
                    tempEnd = tempEnd.Add(new TimeSpan(1, 0, 0, 0));
                    //Convert tempStart and tempEnd from UTC to user time
                    tempStartUserTime = _dateTimeHelper.ConvertToUserTime(tempStart, DateTimeKind.Utc);

                    if (dayOfMonthSelected[(int)tempStartUserTime.Day - 1] == 1)
                    {
                        start = tempStart;
                        end = tempEnd.Add(tempStart.Subtract(start));
                        return true;
                    }
                    if (tempStart.Day == DateTime.DaysInMonth(tempStartUserTime.Year, tempStartUserTime.Month) - 1)
                    {
                        DateTime nextMonth = start.AddMonths(pm.FrequencyCount.Value);
                        tempStart = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                        tempEnd = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                    }
                }
                return false;
            }
            else if (pm.FrequencyType == (int?)FrequencyType.Yearly)
            {
                start = start.AddMonths(12 * pm.FrequencyCount.Value);
                end = end.AddMonths(12 * pm.FrequencyCount.Value);
                return true;
            }
            return false;
        }

        #endregion
    }
}
