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
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Filters;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Mvc;
using System;
using System.IO;
using System.Text;

namespace BaseEAM.Web.Controllers
{
    public class WorkflowDefinitionController : BaseController
    {
        #region Fields

        private readonly IRepository<WorkflowDefinition> _workflowDefinitionRepository;
        private readonly IRepository<WorkflowDefinitionVersion> _workflowDefinitionVersionRepository;
        private readonly IWorkflowDefinitionService _workflowDefinitionService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public WorkflowDefinitionController(IRepository<WorkflowDefinition> workflowDefinitionRepository,
            IRepository<WorkflowDefinitionVersion> workflowDefinitionVersionRepository,
            IWorkflowDefinitionService workflowDefinitionService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._workflowDefinitionRepository = workflowDefinitionRepository;
            this._workflowDefinitionVersionRepository = workflowDefinitionVersionRepository;
            this._localizationService = localizationService;
            this._workflowDefinitionService = workflowDefinitionService;
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
            var workflowDefinitionNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "WorkflowDefinition.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(workflowDefinitionNameFilter);

            var workflowDefinitionEntityTypeFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "EntityType",
                ResourceKey = "EntityType",
                DbColumn = "EntityType",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "WorkflowEntities",
                IsRequiredField = false
            };
            model.Filters.Add(workflowDefinitionEntityTypeFilter);

            return model;
        }

        private byte[] ReadFileBinary(HttpPostedFileBase file)
        {
            byte[] content;
            using (var reader = new BinaryReader(file.InputStream))
            {
                content = reader.ReadBytes(file.ContentLength);
            }
            return content;
        }

        private void ValidateFiles(HttpFileCollectionBase uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.Count < 2 || uploadFiles.Get(0) == null || uploadFiles.Get(1) == null)
            {
                ModelState.AddModelError("", _localizationService.GetResource("WorkflowDefinition.File.Required"));
            }
            else
            {
                var xamlFile = uploadFiles.Get(0);
                var pictureFile = uploadFiles.Get(1);

                if (Path.GetExtension(xamlFile.FileName).ToLower() != ".xaml")
                {
                    ModelState.AddModelError("", _localizationService.GetResource("WorkflowDefinition.WorkflowXamlFileName.NotValidExtension"));
                }
                if (Path.GetExtension(pictureFile.FileName).ToLower() != ".png"
                    && Path.GetExtension(pictureFile.FileName).ToLower() != ".jpg")
                {
                    ModelState.AddModelError("", _localizationService.GetResource("WorkflowDefinition.WorkflowPictureFileName.NotValidExtension"));
                }
            }
        }

        private string splitFileNameInIEBrowser(string fileName)
        {
            string[] fileNames = fileName.Split(new char[] { '\\' });
            return fileNames[fileNames.Length - 1];
        }
        #endregion

        #region WorkflowDefinitions

        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.WorkflowDefinitionSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.WorkflowDefinitionSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.WorkflowDefinitionSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.WorkflowDefinitionSearchModel] = model;

                PagedResult<WorkflowDefinition> data = _workflowDefinitionService.GetWorkflowDefinitions(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create")]
        public ActionResult Create()
        {
            var workflowDefinition = new WorkflowDefinition { IsNew = true };
            _workflowDefinitionRepository.InsertAndCommit(workflowDefinition);
            return Json(new { Id = workflowDefinition.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<WorkflowDefinition>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create,Workflow.WorkflowDefinition.Read,Workflow.WorkflowDefinition.Update")]
        public ActionResult Edit(long id)
        {
            var workflowDefinition = _workflowDefinitionRepository.GetById(id);
            var model = workflowDefinition.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create,Workflow.WorkflowDefinition.Update")]
        public ActionResult Edit(WorkflowDefinitionModel model)
        {
            var workflowDefinition = _workflowDefinitionRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                workflowDefinition = model.ToEntity(workflowDefinition);

                //always set IsNew to false when saving
                workflowDefinition.IsNew = false;
                _workflowDefinitionRepository.Update(workflowDefinition);

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
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var workflowDefinition = _workflowDefinitionRepository.GetById(id);

            if (!_workflowDefinitionService.IsDeactivable(workflowDefinition))
            {
                ModelState.AddModelError("WorkflowDefinition", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _workflowDefinitionRepository.DeactivateAndCommit(workflowDefinition);
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
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var workflowDefinitions = new List<WorkflowDefinition>();
            foreach (long id in selectedIds)
            {
                var workflowDefinition = _workflowDefinitionRepository.GetById(id);
                if (workflowDefinition != null)
                {
                    if (!_workflowDefinitionService.IsDeactivable(workflowDefinition))
                    {
                        ModelState.AddModelError("WorkflowDefinition", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        workflowDefinitions.Add(workflowDefinition);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var workflowDefinition in workflowDefinitions)
                    _workflowDefinitionRepository.Deactivate(workflowDefinition);
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

        #region WorkflowDefinitionVersions

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create,Workflow.WorkflowDefinition.Read,Workflow.WorkflowDefinition.Update")]
        public ActionResult WorkflowDefinitionVersionList(long workflowDefinitionId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workflowDefinitionVersionRepository.GetAll().Where(w => w.WorkflowDefinitionId == workflowDefinitionId);
            query = sort == null ? query.OrderBy(a => a.WorkflowVersion) : query.Sort(sort);
            var workflowDefinitionVersions = new PagedList<WorkflowDefinitionVersion>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = workflowDefinitionVersions.Select(x => x.ToModel()),
                Total = workflowDefinitionVersions.TotalCount
            };
            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create,Workflow.WorkflowDefinition.Read,Workflow.WorkflowDefinition.Update")]
        public ActionResult WorkflowDefinitionVersion(long id)
        {
            var workflowDefinitionVersion = _workflowDefinitionVersionRepository.GetById(id);
            var model = workflowDefinitionVersion.ToModel();
            var html = this.WorkflowDefinitionVersionPanel(model);
            return Json(new { Id = workflowDefinitionVersion.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create,Workflow.WorkflowDefinition.Update")]
        public ActionResult CreateWorkflowDefinitionVersion(long workflowDefinitionId)
        {
            var workflowDefinitionVersion = new WorkflowDefinitionVersion
            {
                IsNew = true
            };
            _workflowDefinitionVersionRepository.Insert(workflowDefinitionVersion);

            var workflowDefinition = _workflowDefinitionRepository.GetById(workflowDefinitionId);
            workflowDefinition.WorkflowDefinitionVersions.Add(workflowDefinitionVersion);

            this._dbContext.SaveChanges();

            var model = new WorkflowDefinitionVersionModel();
            model = workflowDefinitionVersion.ToModel();
            var html = this.WorkflowDefinitionVersionPanel(model);

            return Json(new { Id = workflowDefinitionVersion.Id, Html = html });
        }

        [NonAction]
        public string WorkflowDefinitionVersionPanel(WorkflowDefinitionVersionModel model)
        {
            var html = this.RenderPartialViewToString("_WorkflowDefinitionVersionDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create")]
        public ActionResult SaveWorkflowDefinitionVersion()
        {
            HttpFileCollectionBase files = Request.Files;
            //Here, we have 2 files to validate xaml and picture file
            ValidateFiles(files);

            if (ModelState.IsValid)
            {
                try
                {
                    var xamlFileName = files.Get(0).FileName;
                    var pictureFileName = files.Get(1).FileName;

                    var xamlFileContent = Encoding.UTF8.GetString(ReadFileBinary(files.Get(0)));
                    var pictureFileContent = ReadFileBinary(files.Get(1));

                    HttpPostedFileBase file = files.Get(0);
                    // Checking for Internet Explorer Browser
                    if (Request.Browser.Browser.ToLower() == "ie" || Request.Browser.Browser.ToLower() == "internetexplorer")
                    {
                        xamlFileName = splitFileNameInIEBrowser(xamlFileName);
                        pictureFileName = splitFileNameInIEBrowser(pictureFileName);
                    }
                    var workflowDefinitionId = Convert.ToInt64(Request.Form["WorkflowDefinitionId"]);
                    var workflowDefinitionVersionId = Convert.ToInt64(Request.Form["WorkflowDefinitionVersionId"]);
                    var lastWorkflowDefinitionVersion = 
                        _workflowDefinitionVersionRepository.GetAll()
                        .Where(w => w.WorkflowDefinitionId == workflowDefinitionId)
                        .Max(w => w.WorkflowVersion) ?? 0;

                    var workflowDefintionVersion = _workflowDefinitionVersionRepository.GetById(workflowDefinitionVersionId);
                    if(workflowDefintionVersion !=null)
                    {
                        workflowDefintionVersion.WorkflowDefinitionId = workflowDefinitionId;
                        workflowDefintionVersion.WorkflowVersion = lastWorkflowDefinitionVersion + 1;
                        workflowDefintionVersion.WorkflowXamlFileName = xamlFileName;
                        workflowDefintionVersion.WorkflowXamlFileContent = xamlFileContent;
                        workflowDefintionVersion.IsNew = false;
                        workflowDefintionVersion.WorkflowPictureFileName = pictureFileName;
                        workflowDefintionVersion.WorkflowPictureFileContent = pictureFileContent;

                        _workflowDefinitionVersionRepository.UpdateAndCommit(workflowDefintionVersion);
                    }
                    
                    return new NullJsonResult();
                }
                catch (Exception ex)
                {
                    return Json(new { Errors = ex.Message });
                }
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create,Workflow.WorkflowDefinition.Update")]
        public ActionResult CancelWorkflowDefinitionVersion(long id)
        {
            var workflowDefinitionVersion = _workflowDefinitionVersionRepository.GetById(id);
            if (workflowDefinitionVersion.IsNew == true)
            {
                _workflowDefinitionVersionRepository.DeleteAndCommit(workflowDefinitionVersion);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create,Workflow.WorkflowDefinition.Update")]
        public ActionResult DeleteWorkflowDefinitionVersion(long? parentId, long id)
        {
            var workflowDefinitionVersion = _workflowDefinitionVersionRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _workflowDefinitionVersionRepository.DeactivateAndCommit(workflowDefinitionVersion);

            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Create,Workflow.WorkflowDefinition.Update")]
        public ActionResult DeleteSelectedWorkflowDefinitionVersions(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var workflowDefinitionVersion = _workflowDefinitionVersionRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _workflowDefinitionVersionRepository.Deactivate(workflowDefinitionVersion);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        /// <summary>
        /// Get the workflow image file
        /// </summary>

        [BaseEamAuthorize(PermissionNames = "Workflow.WorkflowDefinition.Read")]
        public ActionResult Image(long id)
        {
            var workflowDefinitionVersion = _workflowDefinitionVersionRepository.GetById(id);
            var base64 = Convert.ToBase64String(workflowDefinitionVersion.WorkflowPictureFileContent);
            var imgSrc = string.Format("data:image/gif;base64,{0}", base64);
            return View(model: imgSrc);
        }
        #endregion
    }
}