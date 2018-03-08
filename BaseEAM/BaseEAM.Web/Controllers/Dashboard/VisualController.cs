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
using BaseEAM.Web.Framework;
using System;

namespace BaseEAM.Web.Controllers
{
    public class VisualController : BaseController
    {
        #region Fields

        private readonly IRepository<Visual> _visualRepository;
        private readonly IRepository<VisualFilter> _visualFilterRepository;
        private readonly IRepository<SecurityGroup> _securityGroupRepository;
        private readonly IRepository<Core.Domain.Filter> _filterRepository;
        private readonly IVisualService _visualService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public VisualController(IRepository<Visual> visualRepository,
            IRepository<VisualFilter> visualFilterRepository,
            IRepository<SecurityGroup> securityGroupRepository,
            IRepository<Core.Domain.Filter> filterRepository,
            IVisualService visualService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._visualRepository = visualRepository;
            this._visualFilterRepository = visualFilterRepository;
            this._securityGroupRepository = securityGroupRepository;
            this._filterRepository = filterRepository;
            this._localizationService = localizationService;
            this._visualService = visualService;
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

        #endregion

        #region Visuals

        [BaseEamAuthorize(PermissionNames = "Report.Visual.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.VisualSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.VisualSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.VisualSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.VisualSearchModel] = model;

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

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Create")]
        public ActionResult Create()
        {
            var visual = new Visual { IsNew = true };
            _visualRepository.InsertAndCommit(visual);
            return Json(new { Id = visual.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Visual>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Report.Visual.Create,Report.Visual.Read,Report.Visual.Update")]
        public ActionResult Edit(long id)
        {
            var visual = _visualRepository.GetById(id);
            var model = visual.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Create,Report.Visual.Update")]
        public ActionResult Edit(VisualModel model)
        {
            var visual = _visualRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                visual = model.ToEntity(visual);
                //always set IsNew to false when saving
                visual.IsNew = false;
                _visualRepository.Update(visual);

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
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var visual = _visualRepository.GetById(id);

            if (!_visualService.IsDeactivable(visual))
            {
                ModelState.AddModelError("Visual", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _visualRepository.DeactivateAndCommit(visual);
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
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var visuals = new List<Visual>();
            foreach (long id in selectedIds)
            {
                var visual = _visualRepository.GetById(id);
                if (visual != null)
                {
                    if (!_visualService.IsDeactivable(visual))
                    {
                        ModelState.AddModelError("Visual", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        visuals.Add(visual);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var visual in visuals)
                    _visualRepository.Deactivate(visual);
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
        public ActionResult VisualInfo(long? visualId)
        {
            if (visualId == null || visualId == 0)
                return new NullJsonResult();

            var visualInfo = _visualRepository.GetById(visualId).ToModel();
            return Json(new { visualInfo = visualInfo });
        }

        #endregion

        #region VisualFilter

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Read,Report.VisualFilter.Read")]
        public ActionResult VisualFilterList(long visualId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _visualFilterRepository.GetAll().Where(c => c.VisualId == visualId);
            query = sort == null ? query.OrderBy(a => a.DisplayOrder) : query.Sort(sort);
            var visualFilters = new PagedList<VisualFilter>(query, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = visualFilters.Select(x => x.ToModel()),
                Total = visualFilters.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Read,Report.Visual.Create,Report.Visual.Update")]
        public ActionResult VisualFilter(long id)
        {
            var visualFilter = _visualFilterRepository.GetById(id);
            var model = visualFilter.ToModel();
            var html = this.VisualFilterPanel(model);
            return Json(new { Id = visualFilter.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Create")]
        public ActionResult CreateVisualFilter(long visualId)
        {
            var visual = _visualRepository.GetById(visualId);
            var maxDisplayOrder = visual.VisualFilters.Max(p => p.DisplayOrder) ?? 0;
            var visualFilter = new VisualFilter
            {
                VisualId = visualId,
                IsNew = true,
                DisplayOrder = maxDisplayOrder + 1
            };
            _visualFilterRepository.InsertAndCommit(visualFilter);

            var model = new VisualFilterModel();
            model = visualFilter.ToModel();
            var html = this.VisualFilterPanel(model);

            return Json(new { Id = visualFilter.Id, Html = html });
        }

        [NonAction]
        public string VisualFilterPanel(VisualFilterModel model)
        {
            var html = this.RenderPartialViewToString("_VisualFilterDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Create,Report.Visual.Update")]
        public ActionResult SaveVisualFilter(VisualFilterModel model)
        {
            if (ModelState.IsValid)
            {
                var visualFilter = _visualFilterRepository.GetById(model.Id);
                if (string.IsNullOrEmpty(visualFilter.Name))
                {
                    var filter = _filterRepository.GetById(model.FilterId);
                    model.Name = filter.Name + "_" + Guid.NewGuid();
                }
                //always set IsNew to false when saving
                visualFilter.IsNew = false;
                visualFilter = model.ToEntity(visualFilter);

                _visualFilterRepository.UpdateAndCommit(visualFilter);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Create,Report.Visual.Update")]
        public ActionResult CancelVisualFilter(long id)
        {
            var visualFilter = _visualFilterRepository.GetById(id);
            if (visualFilter.IsNew == true)
            {
                _visualFilterRepository.DeleteAndCommit(visualFilter);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Delete")]
        public ActionResult DeleteVisualFilter(long? parentId, long id)
        {
            var visualFilter = _visualFilterRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _visualFilterRepository.DeactivateAndCommit(visualFilter);
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Delete")]
        public ActionResult DeleteSelectedVisualFilters(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var visualFilter = _visualFilterRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _visualFilterRepository.Deactivate(visualFilter);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        /// <summary>
        /// Get the filters of the current visual 
        /// </summary>
        /// <param name="visualId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ParentVisualFilterList(long visualId, string param)
        {
            var filters = _visualFilterRepository.GetAll()
                .Where(f => f.VisualId == visualId && f.Filter.Name.Contains(param))
                .Select(f => new SelectListItem { Text = f.Filter.Name, Value = f.Id.ToString() })
                .ToList();
            if (filters.Count > 0)
            {
                filters.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(filters);
        }

        #endregion

        #region SecurityGroups

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Read")]
        public ActionResult SecurityGroupList(long visualId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var sites = _visualRepository.GetById(visualId).SecurityGroups;
            if (sites.Count == 0)
            {
                return Json(new DataSourceResult());
            }
            else
            {
                var queryable = sites.AsQueryable<SecurityGroup>();
                queryable = sort == null ? queryable.OrderBy(a => a.CreatedDateTime) : queryable.Sort(sort);
                var data = queryable.ToList().Select(x => x.ToModel()).ToList();
                var gridModel = new DataSourceResult
                {
                    Data = data.PagedForCommand(command),
                    Total = sites.Count()
                };

                return Json(gridModel);
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Create,Report.Visual.Update")]
        public ActionResult AddSecurityGroups(long visualId, long[] selectedIds)
        {
            var visual = _visualRepository.GetById(visualId);
            foreach (var id in selectedIds)
            {
                var existed = visual.SecurityGroups.Any(s => s.Id == id);
                if (!existed)
                {
                    var securityGroup = _securityGroupRepository.GetById(id);
                    visual.SecurityGroups.Add(securityGroup);
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Create,Report.Visual.Update")]
        public ActionResult DeleteSecurityGroup(long? parentId, long id)
        {
            var visual = _visualRepository.GetById(parentId);
            var securityGroup = _securityGroupRepository.GetById(id);
            //For many-many, delete by set foreign key to null
            visual.SecurityGroups.Remove(securityGroup);

            _visualRepository.UpdateAndCommit(visual);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Report.Visual.Create,Report.Visual.Update")]
        public ActionResult DeleteSelectedSecurityGroups(long? parentId, long[] selectedIds)
        {
            var visual = _visualRepository.GetById(parentId);
            foreach (long id in selectedIds)
            {
                var securityGroup = _securityGroupRepository.GetById(id);
                //For many-many, need to remove from parent
                visual.SecurityGroups.Remove(securityGroup);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

    }
}