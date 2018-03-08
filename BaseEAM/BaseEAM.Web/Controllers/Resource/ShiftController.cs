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
using System;

namespace BaseEAM.Web.Controllers
{
    public class ShiftController : BaseController
    {
        #region Fields

        private readonly IRepository<Shift> _shiftRepository;
        private readonly IRepository<ShiftPattern> _shiftPatternRepository;
        private readonly IShiftService _shiftService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public ShiftController(IRepository<Shift> shiftRepository,
            IRepository<ShiftPattern> shiftPatternRepository,
            IShiftService shiftService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._shiftRepository = shiftRepository;
            this._shiftPatternRepository = shiftPatternRepository;
            this._localizationService = localizationService;
            this._shiftService = shiftService;
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
            var shiftNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Shift.Name",
                DbColumn = "Shift.Name, Shift.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(shiftNameFilter);

            var calendarNameFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "CalendarName",
                ResourceKey = "Shift.Calendar",
                DbColumn = "Calendar.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "Calendar",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(calendarNameFilter);

            return model;
        }

        private void ResetOldShiftPatterns(Shift shift)
        {
            var shiftPatterns = _shiftPatternRepository.GetAll().Where(s => s.ShiftId == shift.Id);
            foreach(var shiftPattern in shiftPatterns)
            {
                _shiftPatternRepository.Delete(shiftPattern);
            }
            this._dbContext.SaveChanges();
        }

        private void CreateNewShiftPatterns(ShiftModel model)
        {
            int daysInPattern = model.DaysInPattern.Value;
            //Default Start Time: 00:00:00 and EndTime: 23:59:59
            TimeSpan startTime = new TimeSpan(0, 0, 0);
            TimeSpan endTime = new TimeSpan(23, 59, 59);
            for (int i = 0; i < daysInPattern; i++)
            {
                var shiftPattern = new ShiftPattern
                {
                    Sequence = i + 1,
                    ShiftId = model.Id,
                    StartTime = DateTime.Now + startTime,
                    EndTime = DateTime.Now + endTime
                };
                this._shiftPatternRepository.Insert(shiftPattern);
            }
            this._dbContext.SaveChanges();
        }
        #endregion

        #region Shifts

        [BaseEamAuthorize(PermissionNames = "Resource.Shift.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ShiftSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ShiftSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Shift.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ShiftSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.ShiftSearchModel] = model;

                PagedResult<Shift> data = _shiftService.GetShifts(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Resource.Shift.Create")]
        public ActionResult Create()
        {
            var shift = new Shift { IsNew = true };
            _shiftRepository.InsertAndCommit(shift);
            return Json(new { Id = shift.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Shift.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Shift>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Resource.Shift.Create,Resource.Shift.Read,Resource.Shift.Update")]
        public ActionResult Edit(long id)
        {
            var shift = _shiftRepository.GetById(id);
            var model = shift.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Shift.Create,Resource.Shift.Update")]
        public ActionResult Edit(ShiftModel model)
        {
            var shift = _shiftRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                if (shift.DaysInPattern != model.DaysInPattern)
                {
                    //Reset the old Shift Patterns
                    ResetOldShiftPatterns(shift);
                    //create the new Shift Patterns Model
                    CreateNewShiftPatterns(model);
                }
                shift = model.ToEntity(shift);
                //always set IsNew to false when saving
                shift.IsNew = false;
                _shiftRepository.Update(shift);

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
        [BaseEamAuthorize(PermissionNames = "Resource.Shift.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var shift = _shiftRepository.GetById(id);

            if (!_shiftService.IsDeactivable(shift))
            {
                ModelState.AddModelError("Shift", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _shiftRepository.DeactivateAndCommit(shift);
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
        [BaseEamAuthorize(PermissionNames = "Resource.Shift.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var shifts = new List<Shift>();
            foreach (long id in selectedIds)
            {
                var shift = _shiftRepository.GetById(id);
                if (shift != null)
                {
                    if (!_shiftService.IsDeactivable(shift))
                    {
                        ModelState.AddModelError("Shift", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        shifts.Add(shift);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var shift in shifts)
                    _shiftRepository.Deactivate(shift);
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

        #region ShiftPatterns

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Shift.Create,Resource.Shift.Read,Resource.Shift.Update")]
        public ActionResult ShiftPatternList(long shiftId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _shiftPatternRepository.GetAll().Where( c => c.ShiftId == shiftId) ;
            query = sort == null ? query.OrderBy(a => a.Sequence) : query.Sort(sort);
            var shiftPatterns = new PagedList<ShiftPattern>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = shiftPatterns.Select(x => x.ToModel()),
                Total = shiftPatterns.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Shift.Create,Resource.Shift.Update,Resource.Shift.Delete")]
        public ActionResult SaveChanges([Bind(Prefix = "updated")]List<ShiftPatternModel> updatedItems,
           [Bind(Prefix = "created")]List<ShiftPatternModel> createdItems,
           [Bind(Prefix = "deleted")]List<ShiftPatternModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Update ShiftPatterns
                    if (updatedItems != null)
                    {
                        foreach (var model in updatedItems)
                        {
                            var shiftPattern = _shiftPatternRepository.GetById(model.Id);
                            shiftPattern.StartTime = model.StartTime;
                            shiftPattern.EndTime = model.EndTime;
                            _shiftPatternRepository.Update(shiftPattern);
                        }
                    }

                    //Delete ShiftPatterns
                    if (deletedItems != null)
                    {
                        foreach (var model in deletedItems)
                        {
                            var shiftPattern = _shiftPatternRepository.GetById(model.Id);
                            if (shiftPattern != null)
                            {
                                _shiftPatternRepository.Deactivate(shiftPattern);
                            }
                        }
                    }

                    _dbContext.SaveChanges();
                    SuccessNotification(_localizationService.GetResource("Record.Saved"));
                    return new NullJsonResult();
                }
                catch (Exception e)
                {
                    return Json(new { Errors = e.Message });
                }
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }
        #endregion
    }
}