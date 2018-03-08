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
    public class FilterController : BaseController
    {
        #region Fields

        private readonly IRepository<Core.Domain.Filter> _filterRepository;
        private readonly IFilterService _filterService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public FilterController(IRepository<Core.Domain.Filter> filterRepository,
            IFilterService filterService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._filterRepository = filterRepository;
            this._localizationService = localizationService;
            this._filterService = filterService;
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
            var filterNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "FilterName",
                ResourceKey = "Filter.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(filterNameFilter);

            return model;
        }

        #endregion

        #region Filters

        [BaseEamAuthorize(PermissionNames = "Report.Filter.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.FilterSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.FilterSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Filter.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.FilterSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.FilterSearchModel] = model;

                PagedResult<Core.Domain.Filter> data = _filterService.GetFilters(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var r in result)
                {
                    r.ControlTypeText = r.ControlType.ToString();
                    r.DataTypeText = r.DataType.ToString();
                    r.DataSourceText = r.DataSource.ToString();
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

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Filter.Create")]
        public ActionResult Create()
        {
            var filter = new Core.Domain.Filter { IsNew = true, AutoBind = true };
            _filterRepository.InsertAndCommit(filter);
            return Json(new { Id = filter.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Filter.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Core.Domain.Filter>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Report.Filter.Create,Report.Filter.Read,Report.Filter.Update")]
        public ActionResult Edit(long id)
        {
            var filter = _filterRepository.GetById(id);
            var model = filter.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Filter.Create,Report.Filter.Update")]
        public ActionResult Edit(FilterModel model)
        {
            var filter = _filterRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                if (model.ControlType == FieldControlType.DropDownList
                   || model.ControlType == FieldControlType.MultiSelectList)
                {
                    if(model.DataSource != FieldDataSource.CSV)
                    {
                        model.CsvTextList = null;
                        model.CsvValueList = null;
                    }
                    if (model.DataSource != FieldDataSource.DB)
                    {
                        model.DbTable = null;
                        model.DbTextColumn = null;
                        model.DbValueColumn = null;
                    }
                    if (model.DataSource != FieldDataSource.SQL)
                    {
                        model.SqlQuery = null;
                        model.SqlTextField = null;
                        model.SqlValueField = null;
                    }
                    if (model.DataSource != FieldDataSource.MVC)
                    {
                        model.MvcController = null;
                        model.MvcAction = null;
                        model.AdditionalField = null;
                        model.AdditionalValue = null;
                    }
                }

                if(model.ControlType != FieldControlType.Lookup)
                {
                    model.LookupType = null;
                    model.LookupTextField = null;
                    model.LookupValueField = null;
                }

                filter = model.ToEntity(filter);
                //always set IsNew to false when saving
                filter.IsNew = false;
                filter.AutoBind = true;
                _filterRepository.Update(filter);

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
        [BaseEamAuthorize(PermissionNames = "Report.Filter.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var filter = _filterRepository.GetById(id);

            if (!_filterService.IsDeactivable(filter))
            {
                ModelState.AddModelError("Filter", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _filterRepository.DeactivateAndCommit(filter);
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
        [BaseEamAuthorize(PermissionNames = "Report.Filter.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var filters = new List<Core.Domain.Filter>();
            foreach (long id in selectedIds)
            {
                var filter = _filterRepository.GetById(id);
                if (filter != null)
                {
                    if (!_filterService.IsDeactivable(filter))
                    {
                        ModelState.AddModelError("Filter", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        filters.Add(filter);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var filter in filters)
                    _filterRepository.Deactivate(filter);
                this._dbContext.SaveChanges();
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        public ActionResult FilterInfo(long? filterId)
        {
            if (filterId == null || filterId == 0)
                return new NullJsonResult();

            var filterInfo = _filterRepository.GetById(filterId).ToModel();
            return Json(new { filterInfo = filterInfo });
        }

        #endregion
    }
}