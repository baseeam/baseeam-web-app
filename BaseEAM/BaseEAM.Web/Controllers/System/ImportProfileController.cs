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
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Filters;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class ImportProfileController : BaseController
    {
        #region Fields
        private readonly IRepository<ImportProfile> _importProfileRepository;
        private readonly IImportProfileService _importProfileService;
        private readonly ILocalizationService _localizationService;
        private readonly QuartzScheduler _quartzScheduler;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;
        #endregion

        #region Constructors

        public ImportProfileController(ILocalizationService localizationService,
            IRepository<ImportProfile> importProfileRepository,
            IImportProfileService importProfileService,
            QuartzScheduler quartzScheduler,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._localizationService = localizationService;
            this._quartzScheduler = quartzScheduler;
            this._importProfileRepository = importProfileRepository;
            this._importProfileService = importProfileService;
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
            var moduleNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "ImportProfile.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(moduleNameFilter);

            var importProfileNameFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "EntityType",
                ResourceKey = "EntityType",
                DbColumn = "EntityType",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "ImportEntities",
                IsRequiredField = false
            };
            model.Filters.Add(importProfileNameFilter);

            return model;
        }

        #endregion

        #region Methods

        [BaseEamAuthorize(PermissionNames = "System.ImportProfile.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ImportProfileSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ImportProfileSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.ImportProfile.Read")]
        public ActionResult List(string searchValues, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ImportProfileSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();
            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.ImportProfileSearchModel] = model;

                PagedResult<ImportProfile> importProfiles = _importProfileService.GetImportProfiles(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = importProfiles.Result.Select(x => x.ToModel()),
                    Total = importProfiles.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.ImportProfile.Create")]
        public ActionResult Create()
        {
            var importProfile = new ImportProfile { IsNew = true };
            _importProfileRepository.InsertAndCommit(importProfile);
            //Create a new Directory with this profile
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolder = string.Format("Profile{0}", importProfile.Id);
            var importProfilePath = Path.Combine(rootPath, importProfileFolder);
            if (!Directory.Exists(importProfilePath))
            {
                Directory.CreateDirectory(importProfilePath);
            }

            return Json(new { Id = importProfile.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.ImportProfile.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<ImportProfile>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "System.ImportProfile.Create,System.ImportProfile.Read,System.ImportProfile.Update")]
        public ActionResult Edit(long id)
        {
            var importProfile = _importProfileRepository.GetById(id);
            var model = importProfile.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.ImportProfile.Create,System.ImportProfile.Update")]
        public ActionResult Edit(ImportProfileModel model)
        {
            var importProfile = _importProfileRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                importProfile = model.ToEntity(importProfile);
                //always set IsNew to false when saving
                importProfile.IsNew = false;
                _importProfileRepository.Update(importProfile);

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
        [BaseEamAuthorize(PermissionNames = "System.ImportProfile.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var importProfile = _importProfileRepository.GetById(id);

            if (!_importProfileService.IsDeactivable(importProfile))
            {
                ModelState.AddModelError("ImportProfile", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _importProfileRepository.DeactivateAndCommit(importProfile);
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
        [BaseEamAuthorize(PermissionNames = "System.ImportProfile.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var importProfiles = new List<ImportProfile>();
            foreach (long id in selectedIds)
            {
                var importProfile = _importProfileRepository.GetById(id);
                if (importProfile != null)
                {
                    if (!_importProfileService.IsDeactivable(importProfile))
                    {
                        ModelState.AddModelError("ImportProfile", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        importProfiles.Add(importProfile);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var importProfile in importProfiles)
                    _importProfileRepository.Deactivate(importProfile);
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
        [BaseEamAuthorize(PermissionNames = "System.ImportProfile.Create,System.ImportProfile.Update")]
        public ActionResult Execute(int id)
        {
            var importProfile = _importProfileRepository.GetById(id);
            if (importProfile == null)
                return new NullJsonResult();
            Type importJob = typeof(ImportJob);
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("importProfileId", importProfile.Id.ToString());
            try
            {
                _quartzScheduler.ScheduleOneTimeJob(importJob, dictionary);
                return Json(new { Message = string.Format(_localizationService.GetResource("ImportProfile.ProcessingImport"), importProfile.ImportFileName, importProfile.EntityType) });
            }
            catch(Exception ex)
            {
                return Json(new { Errors = ex.Message });
            }
           
        }

        [HttpGet]
        [BaseEamAuthorize(PermissionNames = "System.ImportProfile.Read")]
        public ActionResult ViewLogFile(long? id)
        {
            var importProfile = _importProfileRepository.GetById(id);
            if (importProfile != null)
            {
                var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
                var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
                var importLogProfilePath = Path.Combine(importProfileFolderPath, importProfile.LogFileName);
                if (System.IO.File.Exists(importLogProfilePath))
                {
                    try
                    {
                        var stream = new FileStream(importLogProfilePath, FileMode.Open);
                        var result = new FileStreamResult(stream, MimeTypes.TextPlain);

                        return result;
                    }
                    catch (IOException)
                    {
                        ModelState.AddModelError("ImportProfile", _localizationService.GetResource("ImportProfile.CannotReadLogFile"));
                    }
                }
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }
        #endregion
    }
}