using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using BaseEAM.Services;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class DashboardController : BaseController
    {
        #region Fields

        private readonly IRepository<UserDashboard> _userDashboardRepository;
        private readonly IRepository<UserDashboardVisual> _userDashboardVisualRepository;
        private readonly IVisualService _visualService;
        private readonly IAssignmentService _assignmentService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public DashboardController(IRepository<UserDashboard> userDashboardRepository,
            IRepository<UserDashboardVisual> userDashboardVisualRepository,
            IVisualService visualService,
            IAssignmentService assignmentService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._userDashboardRepository = userDashboardRepository;
            this._userDashboardVisualRepository = userDashboardVisualRepository;
            this._visualService = visualService;
            this._assignmentService = assignmentService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._httpContext = httpContext;
            this._workContext = workContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Utilities

        private SearchModel BuildSearchModel(List<VisualFilter> visualFilters)
        {
            var model = new SearchModel();
            if (visualFilters.Count > 0)
            {
                visualFilters = visualFilters.OrderBy(r => r.DisplayOrder).ToList();
                foreach (var visualFilter in visualFilters)
                {
                    var field = new FieldModel
                    {
                        DisplayOrder = visualFilter.DisplayOrder,
                        Name = visualFilter.Name,
                        ResourceKey = visualFilter.ResourceKey,
                        DbColumn = visualFilter.DbColumn,
                        Value = null,
                        ControlType = (FieldControlType)visualFilter.Filter.ControlType,
                        DataType = (FieldDataType)visualFilter.Filter.DataType,
                        DataSource = (FieldDataSource)visualFilter.Filter.DataSource,
                        IsRequiredField = visualFilter.IsRequired,
                        CsvTextList = visualFilter.Filter.CsvTextList,
                        CsvValueList = visualFilter.Filter.CsvValueList,
                        DbTable = visualFilter.Filter.DbTable,
                        DbTextColumn = visualFilter.Filter.DbTextColumn,
                        DbValueColumn = visualFilter.Filter.DbValueColumn,
                        SqlQuery = visualFilter.Filter.SqlQuery,
                        SqlTextField = visualFilter.Filter.SqlTextField,
                        SqlValueField = visualFilter.Filter.SqlValueField,
                        SessionKey = visualFilter.Visual.Name,
                        MvcController = visualFilter.Filter.MvcController,
                        MvcAction = visualFilter.Filter.MvcAction,
                        AdditionalField = visualFilter.Filter.AdditionalField,
                        AdditionalValue = visualFilter.Filter.AdditionalValue,
                        AutoBind = visualFilter.Filter.AutoBind,
                        ParentFieldName = visualFilter.ParentVisualFilter == null ? "" : visualFilter.ParentVisualFilter.Name,
                        LookupType = visualFilter.Filter.LookupType,
                        LookupTextField = visualFilter.Filter.LookupTextField,
                        LookupValueField = visualFilter.Filter.LookupValueField
                    };
                    model.Filters.Add(field);
                }
            }
            return model;
        }

        private SearchModel BuildVisualSearchModel()
        {
            var model = new SearchModel();
            var visualNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "VisualName",
                ResourceKey = "Visual.Name",
                DbColumn = "Name, Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(visualNameFilter);

            return model;
        }

        private SearchModel BuildMyAssignmentSearchModel()
        {
            var model = new SearchModel();
            var nameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "AssignmentName",
                ResourceKey = "Assignment.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.MultiSelectList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Open,Planning,Execution,WaitingForMaterial,WaitingForVendor,Review,Closed,Rejected,Cancelled",
                CsvValueList = "'Open','Planning','Execution','WaitingForMaterial','WaitingForVendor','Review','Closed','Rejected','Cancelled'",
                IsRequiredField = false
            };
            model.Filters.Add(nameFilter);

            var expectedStartDateTimeFromFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "ExpectedStartDateTimeFrom",
                ResourceKey = "Assignment.ExpectedStartDateTimeFrom",
                DbColumn = "Assignment.ExpectedStartDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(expectedStartDateTimeFromFilter);

            var expectedStartDateTimeToFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "ExpectedStartDateTimeTo",
                ResourceKey = "Assignment.ExpectedStartDateTimeTo",
                DbColumn = "Assignment.ExpectedStartDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(expectedStartDateTimeToFilter);

            var dueDateTimeFromFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "DueDateTimeFrom",
                ResourceKey = "Assignment.DueDateTimeFrom",
                DbColumn = "Assignment.DueDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(dueDateTimeFromFilter);

            var dueDateTimeToFilter = new FieldModel
            {
                DisplayOrder = 6,
                Name = "dueDateTimeTo",
                ResourceKey = "Assignment.DueDateTimeTo",
                DbColumn = "Assignment.DueDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(dueDateTimeToFilter);

            return model;
        }

        #endregion

        #region Dashboard

        public ActionResult Index()
        {
            // HandleRedirection

            if (_workContext.CurrentUser.UserType == (int)UserType.Tenant)
            {
                return RedirectToAction("Portal", "Tenant");
            }

            /////

            var userDashboard = _userDashboardRepository.GetAll()
                .Where(d => d.UserId == this._workContext.CurrentUser.Id)
                .FirstOrDefault();
            if(userDashboard == null)
            {
                //create a default dashboard for user if not have one
                userDashboard = new UserDashboard
                {
                    UserId = this._workContext.CurrentUser.Id,
                    DashboardLayoutType = (int?)DashboardLayoutType.ThirdsGrid,
                    RegionCount = 1
                };
                _userDashboardRepository.InsertAndCommit(userDashboard);
            }

            var model = userDashboard.ToModel();

            // My assignments
            //
            var myAssignmentSearchModel = _httpContext.Session[SessionKey.MyAssignmentSearchModel] as SearchModel;
            //If not exist, build search model
            if (myAssignmentSearchModel == null)
            {
                myAssignmentSearchModel = BuildMyAssignmentSearchModel();
                //session save
                _httpContext.Session[SessionKey.MyAssignmentSearchModel] = myAssignmentSearchModel;
            }
            ViewBag.MyAssignmentSearchModel = myAssignmentSearchModel;

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateLayout(UserDashboardModel model)
        {
            var userDashboard = _userDashboardRepository.GetAll()
                .Where(d => d.UserId == this._workContext.CurrentUser.Id)
                .FirstOrDefault();
            userDashboard.DashboardLayoutType = (int?)model.DashboardLayoutType;
            userDashboard.RegionCount = model.RegionCount;
            userDashboard.UserDashboardVisuals.Clear();
            _userDashboardRepository.UpdateAndCommit(userDashboard);

            string html = "";
            html = this.RenderPartialViewToString("_Dashboard", model);
            return Json(new { Html = html });
        }

        [HttpPost]
        public ActionResult GetUserDashboardVisual(int cellId, string searchValues = "")
        {
            var visual = _userDashboardVisualRepository.GetAll()
                .Where(d => d.CellId == cellId && d.UserDashboard.UserId == this._workContext.CurrentUser.Id)
                .FirstOrDefault();
            if(visual == null)
            {
                return new NullJsonResult();
            }
            else
            {
                //get filter html
                var model = _httpContext.Session[visual.Visual.Name + "_" + cellId] as SearchModel;
                if (model == null)
                {
                    model = BuildSearchModel(visual.Visual.VisualFilters.ToList());
                    _httpContext.Session[visual.Visual.Name + "_" + cellId] = model;
                }
                else
                    model.ClearValues();
                
                //session update
                if (!string.IsNullOrEmpty(searchValues))
                    model.Update(searchValues);

                string html = this.RenderPartialViewToString("VisualFilter", model);

                //get Json data
                IEnumerable<dynamic> result = _visualService.GetVisualData(visual.Visual, model.ToExpression(), model.ToParameters());
                //transform to Keen DataSet format
                var data = new JArray();
                if(visual.Visual.VisualType == (int?)VisualType.Metric)
                {
                    //add header row
                    var headerRow = new JArray("Index", "Value");
                    data.Add(headerRow);
                    //add data row
                    foreach (IDictionary<string, object> obj in result)
                    {
                        var dataRow = new JArray("Result", Convert.ToInt64(obj["Value"]));
                        data.Add(dataRow);
                    }
                }
                else
                {
                    //add header row
                    var headerRow = new JArray("Index", visual.Visual.YAxis);
                    data.Add(headerRow);
                    //add data row
                    foreach (IDictionary<string, object> obj in result)
                    {
                        var dataRow = new JArray(obj[visual.Visual.XAxis].ToString(), obj[visual.Visual.YAxis].ToString());
                        data.Add(dataRow);
                    }
                }                

                var chartModel = new { Name = visual.Visual.Name, Type = visual.Visual.VisualType, VisualFilterHtml = html, VisualJsonData = data };
                string json = JsonConvert.SerializeObject(chartModel);
                return Content(json, "application/json");
            }
        }

        [HttpPost]
        public ActionResult AddUserDashboardVisual(int cellId, long visualId)
        {
            var userDashboard = _userDashboardRepository.GetAll()
                .Where(d => d.UserId == this._workContext.CurrentUser.Id)
                .FirstOrDefault();
            var visual = userDashboard.UserDashboardVisuals
                .Where(d => d.CellId == cellId && d.UserDashboard.UserId == this._workContext.CurrentUser.Id)
                .FirstOrDefault();
            if(visual == null)
            {
                visual = new UserDashboardVisual
                {
                    UserDashboardId = userDashboard.Id,
                    CellId = cellId,
                    VisualId = visualId
                };
                _userDashboardVisualRepository.InsertAndCommit(visual);
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult DeleteUserDashboardVisual(int cellId)
        {
            var visual = _userDashboardVisualRepository.GetAll()
                .Where(d => d.CellId == cellId && d.UserDashboard.UserId == this._workContext.CurrentUser.Id)
                .FirstOrDefault();
            if(visual != null)
            {
                _userDashboardVisualRepository.DeleteAndCommit(visual);
            }

            return new NullJsonResult();
        }

        [HttpGet]
        public ActionResult DashboardSettingView()
        {
            var userDashboard = _userDashboardRepository.GetAll()
                .Where(d => d.UserId == this._workContext.CurrentUser.Id)
                .FirstOrDefault();
            return PartialView("_DashboardSetting", userDashboard.ToModel());
        }

        #endregion

        #region Visual Lookup

        [HttpGet]
        public ActionResult SLVisualView(int cellId)
        {
            ViewBag.CellId = cellId;
            var model = BuildVisualSearchModel();
            return PartialView("_SLVisual", model);
        }

        [HttpPost]
        public ActionResult VisualList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildVisualSearchModel();
            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Visual> data = _visualService.GetVisuals(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var r in result)
                {
                    r.VisualTypeText = r.VisualType.ToString();
                }
                var gridModel = new DataSourceResult
                {
                    Data = result,
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        #endregion

        #region My Assignments

        [HttpPost]
        public ActionResult MyAssignmentList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.MyAssignmentSearchModel] as SearchModel;
            if (model == null)
                model = BuildMyAssignmentSearchModel();
            else
                model.ClearValues();
            //validate
            var errorFilters = model.Validate(searchValues);
            foreach (var filter in errorFilters)
            {
                ModelState.AddModelError(filter.Name, _localizationService.GetResource(filter.ResourceKey + ".Required"));
            }
            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.MyAssignmentSearchModel] = model;

                PagedResult<Assignment> data = _assignmentService.GetMyAssignments(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var item in result)
                {
                    item.PriorityText = item.Priority.ToString();
                }
                var gridModel = new DataSourceResult
                {
                    Data = result,
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        #endregion
    }
}