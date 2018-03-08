/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using BaseEAM.Services;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseEAM.Web.Framework.Filters;

namespace BaseEAM.Web.Controllers
{
    public class CalendarController : BaseController
    {
        #region Fields

        private readonly IRepository<Calendar> _calendarRepository;
        private readonly IRepository<CalendarNonWorking> _calendarNonWorkingRepository;
        private readonly ICalendarService _calendarService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public CalendarController(IRepository<Calendar> calendarRepository,
            IRepository<CalendarNonWorking> calendarNonWorkingRepository,
            ICalendarService calendarService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._calendarRepository = calendarRepository;
            this._calendarNonWorkingRepository = calendarNonWorkingRepository;
            this._localizationService = localizationService;
            this._calendarService = calendarService;
            this._permissionService = permissionService;
            this._httpContext = httpContext;
            this._workContext = workContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Utilities

        private SearchModel BuildSearchModel()
        {
            var model = new SearchModel();
            var calendarNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Calendar.Name",
                DbColumn = "Name, Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(calendarNameFilter);

            return model;
        }

        #endregion

        #region Calendars

        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.CalendarSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.CalendarSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.CalendarSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
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
                _httpContext.Session[SessionKey.CalendarSearchModel] = model;

                PagedResult<Calendar> data = _calendarService.GetCalendars(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => x.ToModel()),
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create")]
        public ActionResult Create()
        {
            var calendar = new Calendar { IsNew = true };
            _calendarRepository.InsertAndCommit(calendar);
            return Json(new { Id = calendar.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Calendar>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create,Calendar.Calendar.Read,Calendar.Calendar.Update")]
        public ActionResult Edit(long id)
        {
            var calendar = _calendarRepository.GetById(id);
            var model = calendar.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create,Calendar.Calendar.Update")]
        public ActionResult Edit(CalendarModel model)
        {
            var calendar = _calendarRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                calendar = model.ToEntity(calendar);

                //always set IsNew to false when saving
                calendar.IsNew = false;
                //update attributes
                _calendarRepository.Update(calendar);

                //commit all changes
                this._dbContext.SaveChanges();

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var calendar = _calendarRepository.GetById(id);

            if (!_calendarService.IsDeactivable(calendar))
            {
                ModelState.AddModelError("Calendar", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _calendarRepository.DeactivateAndCommit(calendar);
                //notification
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var calendars = new List<Calendar>();
            foreach (long id in selectedIds)
            {
                var calendar = _calendarRepository.GetById(id);
                if (calendar != null)
                {
                    if (!_calendarService.IsDeactivable(calendar))
                    {
                        ModelState.AddModelError("Calendar", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        calendars.Add(calendar);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var calendar in calendars)
                    _calendarRepository.Deactivate(calendar);
                this._dbContext.SaveChanges();
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region CalendarNonWorking

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create,Resource.Calendar.Read,Resource.Calendar.Update")]
        public ActionResult CalendarNonWorkingList(long calendarId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _calendarNonWorkingRepository.GetAll().Where(c => c.CalendarId == calendarId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var calendarNonWorkings = new PagedList<CalendarNonWorking>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = calendarNonWorkings.Select(x => x.ToModel()),
                Total = calendarNonWorkings.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create,Resource.Calendar.Read,Resource.Calendar.Update")]
        public ActionResult CalendarNonWorking(long id)
        {
            var calendarNonWorking = _calendarNonWorkingRepository.GetById(id);
            var model = calendarNonWorking.ToModel();
            var html = this.CalendarNonWorkingPanel(model);
            return Json(new { Id = calendarNonWorking.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create,Resource.Calendar.Update")]
        public ActionResult CreateCalendarNonWorking(long calendarId)
        {
            var calendarNonWorking = new CalendarNonWorking
            {
                IsNew = true
            };
            _calendarNonWorkingRepository.Insert(calendarNonWorking);

            var calendar = _calendarRepository.GetById(calendarId);
            calendar.CalendarNonWorkings.Add(calendarNonWorking);

            this._dbContext.SaveChanges();

            var model = new CalendarNonWorkingModel();
            model = calendarNonWorking.ToModel();
            var html = this.CalendarNonWorkingPanel(model);

            return Json(new { Id = calendarNonWorking.Id, Html = html });
        }

        [NonAction]
        public string CalendarNonWorkingPanel(CalendarNonWorkingModel model)
        {
            var html = this.RenderPartialViewToString("_CalendarNonWorkingDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create,Resource.Calendar.Update")]
        public ActionResult SaveCalendarNonWorking(CalendarNonWorkingModel model)
        {
            if (ModelState.IsValid)
            {
                var calendarNonWorking = _calendarNonWorkingRepository.GetById(model.Id);
                //always set IsNew to false when saving
                calendarNonWorking.IsNew = false;
                calendarNonWorking = model.ToEntity(calendarNonWorking);

                _calendarNonWorkingRepository.UpdateAndCommit(calendarNonWorking);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create,Resource.Calendar.Update")]
        public ActionResult CancelCalendarNonWorking(long id)
        {
            var calendarNonWorking = _calendarNonWorkingRepository.GetById(id);
            if (calendarNonWorking.IsNew == true)
            {
                _calendarNonWorkingRepository.DeleteAndCommit(calendarNonWorking);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create,Resource.Calendar.Update")]
        public ActionResult DeleteCalendarNonWorking(long? parentId, long id)
        {
            var calendarNonWorking = _calendarNonWorkingRepository.GetById(id);
            _calendarNonWorkingRepository.DeactivateAndCommit(calendarNonWorking);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Calendar.Create,Resource.Calendar.Update")]
        public ActionResult DeleteSelectedCalendarNonWorkings(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var calendarNonWorking = _calendarNonWorkingRepository.GetById(id);
                _calendarNonWorkingRepository.Deactivate(calendarNonWorking);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion
    }
}