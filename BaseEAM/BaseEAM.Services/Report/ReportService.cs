/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Dapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace BaseEAM.Services
{
    public class ReportService : BaseService, IReportService
    {
        #region Fields

        private readonly IRepository<Report> _reportRepository;
        private readonly IRepository<ReportColumn> _reportColumnRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public ReportService(IRepository<Report> reportRepository,
            IRepository<ReportColumn> reportColumnRepository,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._reportRepository = reportRepository;
            this._reportColumnRepository = reportColumnRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<Report> GetReports(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.ReportSearch(), new { skip = pageIndex * pageSize, take = pageSize });
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
            var count = countBuilder.AddTemplate(SqlTemplate.ReportSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var reports = connection.Query<Report>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Report>(reports, totalCount);
            }
        }

        public virtual List<Report> GetReportsByUser(User user)
        {
            var result = new List<Report>();
            var securityGroupIds = user.SecurityGroups.Select(g => g.Id).ToList();
            result = _reportRepository.GetAll()
                .Where(r => r.SecurityGroups.Any(g => securityGroupIds.Contains(g.Id)))
                .OrderBy(r => r.Name)
                .ToList();
            return result;
        }

        public virtual IEnumerable<dynamic> GetReportData(Report report,
            string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(report.Query, new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy(report.SortExpression);
            }

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var result = connection.Query(search.RawSql, search.Parameters);
                return result;
            }
        }

        public virtual MemoryStream ExportToCsv(long reportId, IEnumerable<dynamic> data)
        {
            var output = new MemoryStream();
            var writer = new StreamWriter(output, Encoding.UTF8);

            var columns = _reportColumnRepository.GetAll().Where(r => r.ReportId == reportId).ToList();
            for (int index = 0; index < columns.Count; index++)
            {
                if (index == columns.Count - 1)
                    writer.Write(columns[index].ColumnName);
                else
                    writer.Write(columns[index].ColumnName + ",");
            }

            writer.WriteLine();

            foreach (var item in data)
            {
                var itemData = (IDictionary<string, object>)item;

                for (int index = 0; index < columns.Count; index++)
                {
                    var value = itemData[columns[index].ColumnName] == null ? string.Empty : itemData[columns[index].ColumnName].ToString();
                    writer.Write(value);
                    if (index != columns.Count - 1)
                        writer.Write(",");
                }

                writer.WriteLine();
            }

            writer.Flush();
            output.Position = 0;

            return output;
        }

        public virtual MemoryStream ExportToExcel(long reportId, IEnumerable<dynamic> data)
        {
            //Create new Excel workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel sheet
            var sheet = workbook.CreateSheet();

            //Create a header row
            var headerRow = sheet.CreateRow(0);

            //Set the column names in the header row
            var columns = _reportColumnRepository.GetAll().Where(r => r.ReportId == reportId).ToList();
            for (int index = 0; index < columns.Count; index++)
            {
                headerRow.CreateCell(index).SetCellValue(columns[index].ColumnName);
            }

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;

            foreach (var item in data)
            {
                var itemData = (IDictionary<string, object>)item;

                //Create a new row
                var row = sheet.CreateRow(rowNumber++);

                //Set values for the cells
                for (int index = 0; index < columns.Count; index++)
                {
                    var value = itemData[columns[index].ColumnName] == null ? string.Empty : itemData[columns[index].ColumnName].ToString();
                    row.CreateCell(index).SetCellValue(value);
                }
            }

            //Write the workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            return output;
        }

        public virtual MemoryStream ExportToPdf(long reportId, IEnumerable<dynamic> data)
        {
            // step 1: creation of a document-object
            var document = new Document(PageSize.A4, 10, 10, 10, 10);

            //step 2: we create a memory stream that listens to the document
            var output = new MemoryStream();
            PdfWriter.GetInstance(document, output);

            //step 3: we open the document
            document.Open();

            //step 4: we add content to the document
            var columns = _reportColumnRepository.GetAll().Where(r => r.ReportId == reportId).ToList();
            var numOfColumns = columns.Count;
            var dataTable = new PdfPTable(numOfColumns);

            dataTable.DefaultCell.Padding = 3;

            dataTable.DefaultCell.BorderWidth = 2;
            dataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            // Adding headers
            for (int index = 0; index < columns.Count; index++)
            {
                dataTable.AddCell(columns[index].ColumnName);
            }

            dataTable.HeaderRows = 1;
            dataTable.DefaultCell.BorderWidth = 1;

            // Add values
            foreach (var item in data)
            {
                var itemData = (IDictionary<string, object>)item;

                for (int index = 0; index < columns.Count; index++)
                {
                    var value = itemData[columns[index].ColumnName] == null ? string.Empty : itemData[columns[index].ColumnName].ToString();
                    dataTable.AddCell(value);
                }
            }

            // Add table to the document
            document.Add(dataTable);

            //This is important don't forget to close the document
            document.Close();

            return output;
        }

        public virtual Stream CrystalReportExport(Report report, string searchValues, IEnumerable<dynamic> data, int exportFormatType)
        {
            var type = (ExportFormatType)exportFormatType;
            ReportDocument rd = new ReportDocument();

            string reportTempFolder = ConfigurationManager.AppSettings["ReportTempFolder"].ToString();
            rd.Load(reportTempFolder + report.TemplateFileName);

            rd.SetDataSource(ToDataTable(data));

            rd.SummaryInfo.ReportTitle = report.Name;

            var dict = ParseSearchValues(searchValues);
            StringBuilder reportComments = new StringBuilder();
            var key = "";
            foreach (var d in dict)
            {
                //we just care of these filter fields which is shown on UI 
                if (d.Key.Contains("_Operator"))
                {
                    key = d.Key.Replace("_Operator", "");
                    reportComments.Append(key);
                    //Get the field value into field_input instead of getting Id field.
                    //Except for this case which has not _input field.(contains: textbox)
                    if (!d.Value.Contains("contains"))
                    {
                        key = d.Key.Replace("_Operator", "_input");
                    }
                    reportComments.Append(" = ");
                    reportComments.Append(String.IsNullOrEmpty(dict[key]) ? "ALL" : dict[key]);
                    reportComments.Append("\r\n");
                }
            }

            rd.SummaryInfo.ReportComments = reportComments.ToString();

            try
            {
                Stream stream = rd.ExportToStream(type);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Extension method to convert dynamic data to a DataTable. Useful for databinding.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>A DataTable with the copied dynamic data.</returns>
        private DataTable ToDataTable(IEnumerable<dynamic> items)
        {
            var data = items.ToArray();
            if (data.Count() == 0) return null;

            var dt = new DataTable();
            foreach (var key in ((IDictionary<string, object>)data[0]).Keys)
            {
                dt.Columns.Add(key);
            }
            foreach (var d in data)
            {
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }

        private Dictionary<string, string> ParseSearchValues(string searchValues)
        {
            var searchValuesDictionary = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(searchValues))
                return searchValuesDictionary;
            var filters = searchValues.Split('&');
            if (filters.Count() > 0)
            {
                foreach (var filter in filters)
                {
                    var filterKeyValue = filter.Split('=');
                    if (filterKeyValue.Count() != 2)
                    {
                        throw new BaseEamException("Not valid form.");
                    }
                    else
                    {
                        //This check is for multiselect values
                        if (searchValuesDictionary.ContainsKey(filterKeyValue[0]))
                        {
                            //build a value has format '1','2','3' so it can be replaced in an IN clause
                            searchValuesDictionary[filterKeyValue[0]] = "'" + searchValuesDictionary[filterKeyValue[0]] + "','" + filterKeyValue[1] + "'";
                            searchValuesDictionary[filterKeyValue[0]] = searchValuesDictionary[filterKeyValue[0]].Replace("''", "'");
                        }
                        else
                        {
                            searchValuesDictionary.Add(filterKeyValue[0], HttpUtility.UrlDecode(filterKeyValue[1]));
                        }
                    }
                }
            }

            return searchValuesDictionary;
        }


        #endregion
    }
}
