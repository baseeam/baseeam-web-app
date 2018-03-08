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
using System.Text;
using BaseEAM.Web.Framework;

namespace BaseEAM.Web.Controllers
{
    public class TaskGroupController : BaseController
    {
        #region Fields

        private readonly IRepository<TaskGroup> _taskGroupRepository;
        private readonly IRepository<ValueItem> _valueItemRepository;
        private readonly IRepository<Task> _taskRepository;
        private readonly IRepository<PreventiveMaintenance> _pmRepository;
        private readonly ITaskGroupService _taskGroupService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public TaskGroupController(IRepository<TaskGroup> taskGroupRepository,
            IRepository<ValueItem> valueItemRepository,
            IRepository<Task> taskRepository,
            IRepository<PreventiveMaintenance> pmRepository,
            ITaskGroupService taskGroupService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._taskGroupRepository = taskGroupRepository;
            this._valueItemRepository = valueItemRepository;
            this._taskRepository = taskRepository;
            this._pmRepository = pmRepository;
            this._localizationService = localizationService;
            this._taskGroupService = taskGroupService;
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
            var taskGroupNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "TaskGroupName",
                ResourceKey = "TaskGroup.Name",
                DbColumn = "Name, Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false

            };
            model.Filters.Add(taskGroupNameFilter);

            return model;
        }

        #endregion

        #region TaskGroups

        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.TaskGroupSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.TaskGroupSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.TaskGroupSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.TaskGroupSearchModel] = model;

                PagedResult<TaskGroup> data = _taskGroupService.GetTaskGroups(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Create")]
        public ActionResult Create()
        {
            var taskGroup = new TaskGroup { IsNew = true };
            _taskGroupRepository.InsertAndCommit(taskGroup);
            return Json(new { Id = taskGroup.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<TaskGroup>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Create,Maintenance.TaskGroup.Read,Maintenance.TaskGroup.Update")]
        public ActionResult Edit(long id)
        {
            var taskGroup = _taskGroupRepository.GetById(id);
            var model = taskGroup.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Create,Maintenance.TaskGroup.Update")]
        public ActionResult Edit(TaskGroupModel model)
        {
            var taskGroup = _taskGroupRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                taskGroup = model.ToEntity(taskGroup);

                //always set IsNew to false when saving
                taskGroup.IsNew = false;
                _taskGroupRepository.Update(taskGroup);

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
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var taskGroup = _taskGroupRepository.GetById(id);

            if (!_taskGroupService.IsDeactivable(taskGroup))
            {
                ModelState.AddModelError("TaskGroup", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _taskGroupRepository.DeactivateAndCommit(taskGroup);
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
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var taskGroups = new List<TaskGroup>();
            foreach (long id in selectedIds)
            {
                var taskGroup = _taskGroupRepository.GetById(id);
                if (taskGroup != null)
                {
                    if (!_taskGroupService.IsDeactivable(taskGroup))
                    {
                        ModelState.AddModelError("TaskGroup", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        taskGroups.Add(taskGroup);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var taskGroup in taskGroups)
                    _taskGroupRepository.Deactivate(taskGroup);
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

        #region Tasks

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Read")]
        public ActionResult TaskList(long taskGroupId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _taskRepository.GetAll().Where(c => c.TaskGroupId == taskGroupId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var tasks = new PagedList<Task>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = tasks.Select(x => x.ToModel()),
                Total = tasks.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Create,Maintenance.TaskGroup.Read,Maintenance.TaskGroup.Update")]
        public ActionResult Task(long id)
        {
            var task = _taskRepository.GetById(id);
            var model = task.ToModel();
            var html = this.TaskPanel(model);
            return Json(new { Id = task.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Create")]
        public ActionResult CreateTask(long taskGroupId)
        {
            var task = new Task
            {
                IsNew = true
            };
            _taskRepository.Insert(task);

            var taskGroup = _taskGroupRepository.GetById(taskGroupId);
            taskGroup.Tasks.Add(task);

            this._dbContext.SaveChanges();

            var model = new TaskModel();
            model = task.ToModel();
            var html = this.TaskPanel(model);

            return Json(new { Id = task.Id, Html = html });
        }

        [NonAction]
        public string TaskPanel(TaskModel model)
        {
            var html = this.RenderPartialViewToString("_TaskDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Create,Maintenance.TaskGroup.Update")]
        public ActionResult SaveTask(TaskModel model)
        {
            if (ModelState.IsValid)
            {
                var task = _taskRepository.GetById(model.Id);
                //always set IsNew to false when saving
                task.IsNew = false;
                task = model.ToEntity(task);

                _taskRepository.UpdateAndCommit(task);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Create,Maintenance.TaskGroup.Update")]
        public ActionResult CancelTask(long id)
        {
            var task = _taskRepository.GetById(id);
            if (task.IsNew == true)
            {
                _taskRepository.DeleteAndCommit(task);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Delete")]
        public ActionResult DeleteTask(long? parentId, long id)
        {
            var task = _taskRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _taskRepository.DeactivateAndCommit(task);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Delete")]
        public ActionResult DeleteSelectedTasks(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var task = _taskRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _taskRepository.Deactivate(task);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Asset Types

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Read")]
        public ActionResult AssetTypeList(long? taskGroupId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var taskGroup = _taskGroupRepository.GetById(taskGroupId);
            var assetTypes = taskGroup.AssetTypes;
            var assetTypeList = new List<string>();

            if (!string.IsNullOrEmpty(assetTypes))
            {
                assetTypeList = assetTypes.Split(',').ToList();
            }
            List<TaskGroupModel> models = new List<TaskGroupModel>();
            foreach(var assetTypeName in assetTypeList)
            {
                var newModel = taskGroup.ToModel();
                var assetType = _valueItemRepository.GetAll().Where(v => v.Name == assetTypeName).FirstOrDefault();
                newModel.AssetTypeName = assetType.Name;
                newModel.AssetTypeId = assetType.Id;
                models.Add(newModel);
            }
            var gridModel = new DataSourceResult
            {
                Data = models.PagedForCommand(command),
                Total = assetTypeList.Count()
            };

            return Json(gridModel);
        }

        /// <summary>
        /// Save an Asset Type for Task Group
        /// </summary>
        /// <param name="taskGroupId"></param>
        /// <param name="assetTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Create,Maintenance.TaskGroup.Update")]
        public ActionResult SaveAssetType(long taskGroupId, long? assetTypeId)
        {
            var taskGroup = _taskGroupRepository.GetById(taskGroupId);
            var assetTypeName = _valueItemRepository.GetById(assetTypeId).Name;
            var assetTypes = taskGroup.AssetTypes;
            
            if (!string.IsNullOrEmpty(assetTypes))
            {
                var assetTypeList = assetTypes.Split(',').ToList();
                if (assetTypeList.Contains(assetTypeName))
                {
                    ModelState.AddModelError("TaskGroup", _localizationService.GetResource("TaskGroup.AssetTypeAlreadyExists"));
                }
            }
            if (ModelState.IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!string.IsNullOrEmpty(assetTypes))
                {
                    stringBuilder.Append(assetTypes);
                    stringBuilder.Append(",");
                }
                stringBuilder.Append(assetTypeName);

                taskGroup.AssetTypes = stringBuilder.ToString();
                _taskGroupRepository.UpdateAndCommit(taskGroup);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        /// <summary>
        /// Delete an asset type(id) within the task group(parentId)
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Delete")]
        public ActionResult DeleteAssetType(long? parentId, long id)
        {
            var taskGroup = _taskGroupRepository.GetById(parentId);

            if (ModelState.IsValid)
            {
                var assetTypeName = _valueItemRepository.GetById(id).Name;
                var assetTypeList = taskGroup.AssetTypes.Split(',').ToList();
                assetTypeList.Remove(assetTypeName);
                taskGroup.AssetTypes = string.Join(",", assetTypeList);

                _taskGroupRepository.UpdateAndCommit(taskGroup);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        #endregion

        #region PM

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.TaskGroup.Read")]
        public ActionResult PMList(long? taskGroupId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _pmRepository.GetAll().Where(c => c.TaskGroupId == taskGroupId);
            query = sort == null ? query.OrderBy(a => a.CreatedDateTime) : query.Sort(sort);
            var pms = new PagedList<PreventiveMaintenance>(query, command.Page - 1, command.PageSize);

            var result = pms.Select(x => x.ToModel()).ToList();
            foreach (var r in result)
            {
                r.PriorityText = r.Priority.ToString();
            }

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = pms.TotalCount
            };

            return Json(gridModel);
        }

        #endregion
    }
}