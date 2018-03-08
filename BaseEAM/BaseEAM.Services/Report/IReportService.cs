/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using System.Collections.Generic;
using System.IO;

namespace BaseEAM.Services
{
    public interface IReportService : IBaseService
    {
        PagedResult<Report> GetReports(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        List<Report> GetReportsByUser(User user);

        IEnumerable<dynamic> GetReportData(Report report,
            string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null);

        MemoryStream ExportToCsv(long reportId, IEnumerable<dynamic> data);

        MemoryStream ExportToExcel(long reportId, IEnumerable<dynamic> data);

        MemoryStream ExportToPdf(long reportId, IEnumerable<dynamic> data);

        Stream CrystalReportExport(Report report, string searchValues, IEnumerable<dynamic> data, int exportFormatType);
    }
}
