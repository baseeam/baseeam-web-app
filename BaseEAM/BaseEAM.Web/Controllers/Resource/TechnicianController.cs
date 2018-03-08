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

namespace BaseEAM.Web.Controllers
{
    public class TechnicianController : BaseController
    {
        #region Fields
        private readonly IRepository<Technician> _technicianRepository;
        private readonly IRepository<Team> _teamRepository;
        private readonly ITechnicianService _technicianService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public TechnicianController(IRepository<Technician> technicianRepository,
            IRepository<Team> teamRepository,
            ITechnicianService technicianService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._teamRepository = teamRepository;
            this._technicianRepository = technicianRepository;
            this._localizationService = localizationService;
            this._technicianService = technicianService;
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
            var userNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "UserName",
                ResourceKey = "User",
                DbColumn = "User.Name, User.Email, User.Phone",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(userNameFilter);

            var teamNameFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "TeamName",
                ResourceKey = "Team",
                DbColumn = "Team.Id",
                Value = null,
                ControlType = FieldControlType.MultiSelectList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.DB,
                DbTable = "Team",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(teamNameFilter);

            return model;
        }

        #endregion

        #region Technicians

        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.TechnicianSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.TechnicianSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.TechnicianSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.TechnicianSearchModel] = model;

                PagedResult<Technician> data = _technicianService.GetTechnicians(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Create")]
        public ActionResult Create()
        {
            var technician = new Technician { IsNew = true };
            _technicianRepository.InsertAndCommit(technician);
            return Json(new { Id = technician.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Technician>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Create,Resource.Technician.Read,Resource.Technician.Update")]
        public ActionResult Edit(int id)
        {
            var technician = _technicianRepository.GetById(id);
            var model = technician.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Create,Resource.Technician.Update")]
        public ActionResult Edit(TechnicianModel model)
        {
            var technician = _technicianRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                technician = model.ToEntity(technician);
                //always set IsNew to false when saving
                technician.IsNew = false;
                _technicianRepository.Update(technician);

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
        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Delete")]
        public ActionResult Delete(long? parentId, int id)
        {
            var technician = _technicianRepository.GetById(id);

            if (!_technicianService.IsDeactivable(technician))
            {
                ModelState.AddModelError("Technician", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _technicianRepository.DeactivateAndCommit(technician);
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
        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var technicians = new List<Technician>();
            foreach (long id in selectedIds)
            {
                var technician = _technicianRepository.GetById(id);
                if (technician != null)
                {
                    if (!_technicianService.IsDeactivable(technician))
                    {
                        ModelState.AddModelError("Technician", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        technicians.Add(technician);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var technician in technicians)
                    _technicianRepository.Deactivate(technician);
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

        #region Teams

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Read")]
        public ActionResult TeamList(long technicianId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var teams = _technicianRepository.GetById(technicianId).Teams;
            var queryable = teams.AsQueryable();
            queryable = sort == null ? queryable.OrderBy(a => a.Name) : queryable.Sort(sort);
            var data = queryable.ToList().Select(x => x.ToModel()).ToList();
            var gridModel = new DataSourceResult
            {
                Data = data.PagedForCommand(command),
                Total = teams.Count()
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Delete")]
        public ActionResult DeleteTeam(long? parentId, int id)
        {
            var technician = _technicianRepository.GetById(parentId);
            var teams = technician.Teams;
            var existed = teams.Any(s => s.Id == id);
            if (existed)
            {
                var team = _teamRepository.GetById(id);
                if (team != null)
                {
                    technician.Teams.Remove(team);
                }
                this._dbContext.SaveChanges();
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Delete")]
        public ActionResult DeleteSelectedTeams(long? parentId, long[] selectedIds)
        {
            var technician = _technicianRepository.GetById(parentId);
            var teams = technician.Teams;
            foreach (var id in selectedIds)
            {
                var existed = teams.Any(s => s.Id == id);
                if (existed)
                {
                    var team = _teamRepository.GetById(id);
                    if (teams != null)
                    {
                        technician.Teams.Remove(team);
                    }
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Technician.Create,Resource.Technician.Update")]
        public ActionResult AddTeams(long technicianId, long[] selectedIds)
        {
            var technician = _technicianRepository.GetById(technicianId);
            var teams = technician.Teams;
            foreach (var id in selectedIds)
            {
                var existed = teams.Any(s => s.Id == id);
                if (!existed)
                {
                    var team = _teamRepository.GetById(id);
                    technician.Teams.Add(team);
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }
        #endregion
    }
}