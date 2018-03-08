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
    public class TeamController : BaseController
    {
        #region Fields

        private readonly IRepository<Team> _teamRepository;
        private readonly IRepository<Technician> _technicianRepository;
        private readonly ITeamService _teamService;
        private readonly ITechnicianService _technicianService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public TeamController(IRepository<Team> teamRepository,
            IRepository<Technician> technicianRepository,
            ITeamService teamService,
            ITechnicianService technicianService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IDateTimeHelper dateTimeHelper,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._teamRepository = teamRepository;
            this._technicianRepository = technicianRepository;
            this._localizationService = localizationService;
            this._teamService = teamService;
            this._technicianService = technicianService;
            this._permissionService = permissionService;
            this._dateTimeHelper = dateTimeHelper;
            this._httpContext = httpContext;
            this._workContext = workContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Utilities

        private SearchModel BuildSearchModel()
        {
            var model = new SearchModel();
            var teamNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Team.Name",
                DbColumn = "Team.Name, Team.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(teamNameFilter);

            var siteNameFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "SiteName",
                ResourceKey = "Team.Site",
                DbColumn = "Site.Id",
                Value = null,
                ControlType = FieldControlType.MultiSelectList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.DB,
                DbTable = "Site",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(siteNameFilter);

            return model;
        }

        #endregion

        #region Teams

        [BaseEamAuthorize(PermissionNames = "Resource.Team.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.TeamSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.TeamSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Team.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.TeamSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.TeamSearchModel] = model;

                PagedResult<Team> data = _teamService.GetTeams(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Resource.Team.Create")]
        public ActionResult Create()
        {
            var team = new Team { IsNew = true };
            _teamRepository.InsertAndCommit(team);
            return Json(new { Id = team.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Team.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Team>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Resource.Team.Create,Resource.Team.Read,Resource.Team.Update")]
        public ActionResult Edit(int id)
        {
            var team = _teamRepository.GetById(id);
            var model = team.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Team.Create,Resource.Team.Update")]
        public ActionResult Edit(TeamModel model)
        {
            var team = _teamRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                team = model.ToEntity(team);
                //always set IsNew to false when saving
                team.IsNew = false;
                _teamRepository.Update(team);

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
        [BaseEamAuthorize(PermissionNames = "Resource.Team.Delete")]
        public ActionResult Delete(long? parentId, int id)
        {
            var team = _teamRepository.GetById(id);

            if (!_teamService.IsDeactivable(team))
            {
                ModelState.AddModelError("Team", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _teamRepository.DeactivateAndCommit(team);
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
        [BaseEamAuthorize(PermissionNames = "Resource.Team.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var teams = new List<Team>();
            foreach (long id in selectedIds)
            {
                var team = _teamRepository.GetById(id);
                if (team != null)
                {
                    if (!_teamService.IsDeactivable(team))
                    {
                        ModelState.AddModelError("Team", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        teams.Add(team);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var team in teams)
                    _teamRepository.Deactivate(team);
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

        #region Technicians

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Team.Read")]
        public ActionResult TechnicianList(long teamId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var technicians = _teamRepository.GetById(teamId).Technicians;
            var queryable = technicians.AsQueryable();
            queryable = sort == null ? queryable.OrderBy(a => a.Name) : queryable.Sort(sort);
            var data = queryable.ToList().Select(x => x.ToModel()).ToList();
            var gridModel = new DataSourceResult
            {
                Data = data.PagedForCommand(command),
                Total = technicians.Count()
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Team.Delete")]
        public ActionResult DeleteTechnician(long? parentId, int id)
        {
            var team = _teamRepository.GetById(parentId);
            var technicians = team.Technicians;
            var existed = technicians.Any(s => s.Id == id);
            if (existed)
            {
                var technician = _technicianRepository.GetById(id);
                if (technician != null)
                {
                    team.Technicians.Remove(technician);
                }
                this._dbContext.SaveChanges();
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Team.Delete")]
        public ActionResult DeleteSelectedTechnicians(long? parentId, long[] selectedIds)
        {
            var team = _teamRepository.GetById(parentId);
            var technicians = team.Technicians;
            foreach (var id in selectedIds)
            {
                var existed = technicians.Any(s => s.Id == id);
                if (existed)
                {
                    var technician = _technicianRepository.GetById(id);
                    if (technician != null)
                    {
                        team.Technicians.Remove(technician);
                    }
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Team.Create,Resource.Team.Update")]
        public ActionResult AddTechnicians(long teamId, long[] selectedIds)
        {
            var team = _teamRepository.GetById(teamId);
            var technicians = team.Technicians;
            foreach (var id in selectedIds)
            {
                var existed = technicians.Any(s => s.Id == id);
                if (!existed)
                {
                    var technician = _technicianRepository.GetById(id);
                    team.Technicians.Add(technician);
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        /// <summary>
        /// Get the list of technicians of a team
        /// </summary>
        /// <param name="parentValue">teamId</param>
        /// <param name="additionalValue">TechnicianMatching</param>
        /// <param name="optionalValue">ExpectedStartDateTime</param>
        /// <param name="param">The text input from user</param>
        [HttpPost]
        public JsonResult Technicians(long parentValue, int? additionalValue, DateTime? optionalValue, string param)
        {
            if(optionalValue.HasValue)
            {
                // we need to convert to UTC
                // because the BaseEamModelBinder not handle for single param: DateTime? optionalValue
                optionalValue = _dateTimeHelper.ConvertToUtcTime(optionalValue.Value, _dateTimeHelper.CurrentTimeZone);
            }

            var technicians = _technicianService.GetTechnicians(parentValue, additionalValue, optionalValue, param)
                .Select(t => new SelectListItem { Text = t.User.Name, Value = t.Id.ToString() })
                .ToList();
            if (technicians.Count > 0)
            {
                technicians.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(technicians);
        }
        #endregion
    }
}