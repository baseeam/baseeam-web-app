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
using BaseEAM.Web.Framework;

namespace BaseEAM.Web.Controllers
{
    public class PreventiveMaintenanceController : BaseController
    {
        #region Fields

        private readonly IRepository<PreventiveMaintenance> _preventiveMaintenanceRepository;
        private readonly IRepository<MeterEvent> _meterEventRepository;
        private readonly IRepository<PMLabor> _pMLaborRepository;
        private readonly IRepository<PMItem> _pMItemRepository;
        private readonly IRepository<PMTask> _pMTaskRepository;
        private readonly IRepository<PMMeterFrequency> _pMMeterFrequencyRepository;
        private readonly IRepository<PMMiscCost> _PMMiscCostRepository;
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IRepository<PMServiceItem> _pMServiceItemRepository;
        private readonly IRepository<Task> _taskRepository;
        private readonly IPreventiveMaintenanceService _preventiveMaintenanceService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IItemService _itemService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IAttachmentService _attachmentService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public PreventiveMaintenanceController(IRepository<PreventiveMaintenance> preventiveMaintenanceRepository,
            IRepository<MeterEvent> meterEventRepository,
            IRepository<PMLabor> pMLaborRepository,
            IRepository<PMItem> pMItemRepository,
            IRepository<PMTask> pMTaskRepository,
            IRepository<PMMeterFrequency> pMMeterFrequencyRepository,
            IRepository<PMMiscCost> PMMiscCostRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<PointMeterLineItem> pointMeterLineItemRepository,
            IRepository<Task> taskRepository,
            IRepository<PMServiceItem> pMServiceItemRepository,
            IPreventiveMaintenanceService preventiveMaintenanceService,
            IAutoNumberService autoNumberService,
            IItemService itemService,
            IDateTimeHelper dateTimeHelper,
            IAttachmentService attachmentService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._preventiveMaintenanceRepository = preventiveMaintenanceRepository;
            this._meterEventRepository = meterEventRepository;
            this._pMLaborRepository = pMLaborRepository;
            this._pMItemRepository = pMItemRepository;
            this._pMTaskRepository = pMTaskRepository;
            this._pMMeterFrequencyRepository = pMMeterFrequencyRepository;
            this._PMMiscCostRepository = PMMiscCostRepository;
            this._workOrderRepository = workOrderRepository;
            this._pMServiceItemRepository = pMServiceItemRepository;
            this._taskRepository = taskRepository;
            this._localizationService = localizationService;
            this._preventiveMaintenanceService = preventiveMaintenanceService;
            this._autoNumberService = autoNumberService;
            this._itemService = itemService;
            this._dateTimeHelper = dateTimeHelper;
            this._attachmentService = attachmentService;
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
            var numberFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Number",
                ResourceKey = "PreventiveMaintenance.Number",
                DbColumn = "PreventiveMaintenance.Number, PreventiveMaintenance.Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(numberFilter);

            var siteFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "Site",
                ResourceKey = "Site",
                DbColumn = "Site.Id",
                Value = this._workContext.CurrentUser.DefaultSiteId,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Site",
                MvcAction = "SiteList",
                IsRequiredField = false
            };
            model.Filters.Add(siteFilter);

            var priorityFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "Priority",
                ResourceKey = "Priority",
                DbColumn = "PreventiveMaintenance.Priority",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int32,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Urgent,High,Medium,Low",
                CsvValueList = "0,1,2,3",
                IsRequiredField = false
            };
            model.Filters.Add(priorityFilter);

            var assetFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "Asset",
                ResourceKey = "Asset",
                DbColumn = "Asset.Id",
                Value = null,
                ControlType = FieldControlType.Lookup,
                IsRequiredField = false,
                LookupType = "Asset",
                LookupValueField = "AssetId",
                LookupTextField = "AssetName",
                ParentFieldName = "Site"
            };

            model.Filters.Add(assetFilter);

            var locationFilter = new FieldModel
            {
                DisplayOrder = 6,
                Name = "Location",
                ResourceKey = "Location",
                DbColumn = "Location.Id",
                Value = null,
                ControlType = FieldControlType.Lookup,
                IsRequiredField = false,
                LookupType = "Location",
                LookupValueField = "LocationId",
                LookupTextField = "LocationName",
                ParentFieldName = "Site"
            };

            model.Filters.Add(locationFilter);
            return model;
        }

        #endregion

        #region PreventiveMaintenances

        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.PreventiveMaintenanceSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.PreventiveMaintenanceSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.PreventiveMaintenanceSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.PreventiveMaintenanceSearchModel] = model;

                PagedResult<PreventiveMaintenance> data = _preventiveMaintenanceService.GetPreventiveMaintenances(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);
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

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create")]
        public ActionResult Create()
        {
            var preventiveMaintenance = new PreventiveMaintenance
            {
                IsNew = true,
                Priority = (int?)AssignmentPriority.Medium,

            };
            _preventiveMaintenanceRepository.InsertAndCommit(preventiveMaintenance);
            return Json(new { Id = preventiveMaintenance.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<PreventiveMaintenance>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult Edit(long id)
        {
            var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(id);
            var model = preventiveMaintenance.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult Edit(PreventiveMaintenanceModel model, string actionName = "")
        {
            var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                preventiveMaintenance = model.ToEntity(preventiveMaintenance);

                if (preventiveMaintenance.IsNew == true)
                {
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), preventiveMaintenance);
                    preventiveMaintenance.Number = number;

                    _preventiveMaintenanceService.GeneratePMTasks(preventiveMaintenance, model.AssetId);
                }

                //always set IsNew to false when saving
                preventiveMaintenance.IsNew = false;
                //update attributes
                _preventiveMaintenanceRepository.Update(preventiveMaintenance);

                //commit all changes in UI
                this._dbContext.SaveChanges();

                //notification
                SuccessNotification(_localizationService.GetResource("Record.Saved"));
                return Json(new { number = preventiveMaintenance.Number });
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(id);

            if (!_preventiveMaintenanceService.IsDeactivable(preventiveMaintenance))
            {
                ModelState.AddModelError("PreventiveMaintenance", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _preventiveMaintenanceRepository.DeactivateAndCommit(preventiveMaintenance);
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
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var preventiveMaintenances = new List<PreventiveMaintenance>();
            foreach (long id in selectedIds)
            {
                var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(id);
                if (preventiveMaintenance != null)
                {
                    if (!_preventiveMaintenanceService.IsDeactivable(preventiveMaintenance))
                    {
                        ModelState.AddModelError("PreventiveMaintenance", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        preventiveMaintenances.Add(preventiveMaintenance);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var preventiveMaintenance in preventiveMaintenances)
                    _preventiveMaintenanceRepository.Deactivate(preventiveMaintenance);
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

        #region PMLabors

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult PMLaborList(long preventiveMaintenanceId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _pMLaborRepository.GetAll().Where(c => c.PreventiveMaintenanceId == preventiveMaintenanceId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var pMLabors = new PagedList<PMLabor>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = pMLabors.Select(x => x.ToModel()),
                Total = pMLabors.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult PMLabor(long id)
        {
            var pMLabor = _pMLaborRepository.GetById(id);
            var model = pMLabor.ToModel();
            var html = this.PMLaborPanel(model);
            return Json(new { Id = pMLabor.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult CreatePMLabor(long preventiveMaintenanceId)
        {
            var pMLabor = new PMLabor
            {
                IsNew = true,
                PreventiveMaintenanceId = preventiveMaintenanceId
            };
            _pMLaborRepository.InsertAndCommit(pMLabor);

            var model = new PMLaborModel();
            model = pMLabor.ToModel();
            var html = this.PMLaborPanel(model);

            return Json(new { Id = pMLabor.Id, Html = html });
        }

        [NonAction]
        public string PMLaborPanel(PMLaborModel model)
        {
            var html = this.RenderPartialViewToString("_PMLaborDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult SavePMLabor(PMLaborModel model)
        {
            if (ModelState.IsValid)
            {
                var pMLabor = _pMLaborRepository.GetById(model.Id);
                //always set IsNew to false when saving
                pMLabor.IsNew = false;
                //update totals
                model.PlanTotal = (model.PlanHours ?? 0) * (model.StandardRate ?? 0);

                pMLabor = model.ToEntity(pMLabor);
                _pMLaborRepository.UpdateAndCommit(pMLabor);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult CancelPMLabor(long id)
        {
            var pMLabor = _pMLaborRepository.GetById(id);
            if (pMLabor.IsNew == true)
            {
                _pMLaborRepository.DeleteAndCommit(pMLabor);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult DeletePMLabor(long? parentId, long id)
        {
            var pMLabor = _pMLaborRepository.GetById(id);
            _pMLaborRepository.DeactivateAndCommit(pMLabor);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult DeleteSelectedPMLabors(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var pMLabor = _pMLaborRepository.GetById(id);
                _pMLaborRepository.Deactivate(pMLabor);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        /// <summary>
        /// Get the list of technician from Work Order Labor
        /// </summary>
        /// <param name="preventiveMaintenanceId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TechnicianListFromPMLabor(long preventiveMaintenanceId, string param)
        {
            var technicians = _pMLaborRepository.GetAll()
                .Where(w => w.PreventiveMaintenanceId == preventiveMaintenanceId && w.Technician.User.Name.Contains(param))
                .Select(w => new SelectListItem { Text = w.Technician.User.Name, Value = w.TechnicianId.ToString() })
                .ToList();
            if (technicians.Count > 0)
            {
                technicians.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(technicians);
        }

        #endregion

        #region PMItems

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult PMItemList(long preventiveMaintenanceId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _pMItemRepository.GetAll().Where(c => c.PreventiveMaintenanceId == preventiveMaintenanceId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var pMItems = new PagedList<PMItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = pMItems.Select(x => x.ToModel()),
                Total = pMItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult PMItem(long id)
        {
            var pMItem = _pMItemRepository.GetById(id);
            var model = pMItem.ToModel();
            model.ItemItemCategoryText = model.ItemItemCategory.ToString();
            var html = this.PMItemPanel(model);
            return Json(new { Id = pMItem.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult CreatePMItem(long preventiveMaintenanceId)
        {
            var preventiveMaintenanceitem = new PMItem
            {
                IsNew = true,
                PreventiveMaintenanceId = preventiveMaintenanceId
            };
            _pMItemRepository.InsertAndCommit(preventiveMaintenanceitem);

            var model = new PMItemModel();
            model = preventiveMaintenanceitem.ToModel();
            var html = this.PMItemPanel(model);

            return Json(new { Id = preventiveMaintenanceitem.Id, Html = html });
        }

        [NonAction]
        public string PMItemPanel(PMItemModel model)
        {
            var html = this.RenderPartialViewToString("_PMItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult SavePMItem(PMItemModel model)
        {
            if (ModelState.IsValid)
            {
                var preventiveMaintenanceitem = _pMItemRepository.GetById(model.Id);
                //always set IsNew to false when saving
                preventiveMaintenanceitem.IsNew = false;
                //update totals
                if (model.PlanToolHours > 0)
                {
                    model.PlanTotal = (model.ToolRate ?? 0) * (model.PlanToolHours ?? 0) * (model.PlanQuantity ?? 0);
                }
                else
                {
                    model.PlanTotal = (model.PlanQuantity ?? 0) * (model.UnitPrice ?? 0);
                }

                preventiveMaintenanceitem = model.ToEntity(preventiveMaintenanceitem);
                _pMItemRepository.UpdateAndCommit(preventiveMaintenanceitem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult CancelPMItem(long id)
        {
            var preventiveMaintenanceitem = _pMItemRepository.GetById(id);
            if (preventiveMaintenanceitem.IsNew == true)
            {
                _pMItemRepository.DeleteAndCommit(preventiveMaintenanceitem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult DeletePMItem(long? parentId, long id)
        {
            var preventiveMaintenanceitem = _pMItemRepository.GetById(id);
            _pMItemRepository.DeactivateAndCommit(preventiveMaintenanceitem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult DeleteSelectedPMItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var preventiveMaintenanceitem = _pMItemRepository.GetById(id);
                _pMItemRepository.Deactivate(preventiveMaintenanceitem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region PMTasks

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Read")]
        public ActionResult PMTaskList(long PreventiveMaintenanceId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _pMTaskRepository.GetAll().Where(c => c.PreventiveMaintenanceId == PreventiveMaintenanceId);
            query = sort == null ? query.OrderBy(a => a.Sequence) : query.Sort(sort);
            var pMTasks = new PagedList<PMTask>(query, command.Page - 1, command.PageSize);
            var result = pMTasks.Select(x => x.ToModel()).ToList();

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = pMTasks.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult PMTask(long id)
        {
            var pMTask = _pMTaskRepository.GetById(id);
            var model = pMTask.ToModel();
            var html = this.PMTaskPanel(model);
            return Json(new { Id = pMTask.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create")]
        public ActionResult CreatePMTask(long preventiveMaintenanceId)
        {
            var pMTask = new PMTask
            {
                IsNew = true
            };
            _pMTaskRepository.Insert(pMTask);

            var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(preventiveMaintenanceId);
            preventiveMaintenance.PMTasks.Add(pMTask);

            this._dbContext.SaveChanges();

            var model = new PMTaskModel();
            model = pMTask.ToModel();
            var html = this.PMTaskPanel(model);

            return Json(new { Id = pMTask.Id, Html = html });
        }

        [NonAction]
        public string PMTaskPanel(PMTaskModel model)
        {
            var html = this.RenderPartialViewToString("_PMTaskDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create")]
        public ActionResult CreatePMTasks(long? preventiveMaintenanceId, long? taskGroupId)
        {
            var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(preventiveMaintenanceId);
            var pMTasks = preventiveMaintenance.PMTasks;
            if (ModelState.IsValid)
            {
                pMTasks.Clear();

                preventiveMaintenance.TaskGroupId = taskGroupId;
                var tasks = _taskRepository.GetAll().Where(m => m.TaskGroupId == taskGroupId).ToList();
                foreach (var task in tasks)
                {
                    var pMTask = new PMTask
                    {
                        Sequence = task.Sequence,
                        Description = task.Description
                    };
                    preventiveMaintenance.PMTasks.Add(pMTask);
                }
                _preventiveMaintenanceRepository.Update(preventiveMaintenance);
                this._dbContext.SaveChanges();

                //copy attachments, need to copy after saving PMTasks
                //so we can have toEntityId
                for (int i = 0; i < tasks.Count; i++)
                {
                    _attachmentService.CopyAttachments(tasks[i].Id, EntityType.Task,
                        preventiveMaintenance.PMTasks.ToList()[i].Id, EntityType.PMTask);
                }

                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult SavePMTask(PMTaskModel model)
        {
            if (ModelState.IsValid)
            {
                var pMTask = _pMTaskRepository.GetById(model.Id);
                //always set IsNew to false when saving
                pMTask.IsNew = false;
                pMTask = model.ToEntity(pMTask);

                _pMTaskRepository.UpdateAndCommit(pMTask);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult CancelPMTask(long id)
        {
            var pMTask = _pMTaskRepository.GetById(id);
            if (pMTask.IsNew == true)
            {
                _pMTaskRepository.DeleteAndCommit(pMTask);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Delete")]
        public ActionResult DeletePMTask(long? parentId, long id)
        {
            var pMTask = _pMTaskRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _pMTaskRepository.DeactivateAndCommit(pMTask);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Delete")]
        public ActionResult DeleteSelectedPMTasks(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var pMTask = _pMTaskRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _pMTaskRepository.Deactivate(pMTask);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region PMMiscCosts

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Read")]
        public ActionResult PMMiscCostList(long PreventiveMaintenanceId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _PMMiscCostRepository.GetAll().Where(c => c.PreventiveMaintenanceId == PreventiveMaintenanceId);
            query = sort == null ? query.OrderBy(a => a.Sequence) : query.Sort(sort);
            var PMMiscCosts = new PagedList<PMMiscCost>(query, command.Page - 1, command.PageSize);
            var result = PMMiscCosts.Select(x => x.ToModel()).ToList();

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = PMMiscCosts.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult PMMiscCost(long id)
        {
            var PMMiscCost = _PMMiscCostRepository.GetById(id);
            var model = PMMiscCost.ToModel();
            var html = this.PMMiscCostPanel(model);
            return Json(new { Id = PMMiscCost.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create")]
        public ActionResult CreatePMMiscCost(long preventiveMaintenanceId)
        {
            var PMMiscCost = new PMMiscCost
            {
                IsNew = true
            };
            _PMMiscCostRepository.Insert(PMMiscCost);

            var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(preventiveMaintenanceId);
            preventiveMaintenance.PMMiscCosts.Add(PMMiscCost);

            this._dbContext.SaveChanges();

            var model = new PMMiscCostModel();
            model = PMMiscCost.ToModel();
            var html = this.PMMiscCostPanel(model);

            return Json(new { Id = PMMiscCost.Id, Html = html });
        }

        [NonAction]
        public string PMMiscCostPanel(PMMiscCostModel model)
        {
            var html = this.RenderPartialViewToString("_PMMiscCostDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult SavePMMiscCost(PMMiscCostModel model)
        {
            if (ModelState.IsValid)
            {
                var PMMiscCost = _PMMiscCostRepository.GetById(model.Id);
                //always set IsNew to false when saving
                PMMiscCost.IsNew = false;
                //update totals
                model.PlanTotal = (model.PlanQuantity ?? 0) * (model.PlanUnitPrice ?? 0);

                PMMiscCost = model.ToEntity(PMMiscCost);

                _PMMiscCostRepository.UpdateAndCommit(PMMiscCost);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult CancelPMMiscCost(long id)
        {
            var PMMiscCost = _PMMiscCostRepository.GetById(id);
            if (PMMiscCost.IsNew == true)
            {
                _PMMiscCostRepository.DeleteAndCommit(PMMiscCost);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Delete")]
        public ActionResult DeletePMMiscCost(long? parentId, long id)
        {
            var PMMiscCost = _PMMiscCostRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _PMMiscCostRepository.DeactivateAndCommit(PMMiscCost);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Delete")]
        public ActionResult DeleteSelectedPMMiscCosts(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var PMMiscCost = _PMMiscCostRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _PMMiscCostRepository.Deactivate(PMMiscCost);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region PMServiceItems

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Read")]
        public ActionResult PMServiceItemList(long PreventiveMaintenanceId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _pMServiceItemRepository.GetAll().Where(c => c.PreventiveMaintenanceId == PreventiveMaintenanceId);
            query = sort == null ? query.OrderBy(a => a.ServiceItem.Name) : query.Sort(sort);
            var pMServiceItems = new PagedList<PMServiceItem>(query, command.Page - 1, command.PageSize);
            var result = pMServiceItems.Select(x => x.ToModel()).ToList();

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = pMServiceItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult PMServiceItem(long id)
        {
            var pMServiceItem = _pMServiceItemRepository.GetById(id);
            var model = pMServiceItem.ToModel();
            var html = this.PMServiceItemPanel(model);
            return Json(new { Id = pMServiceItem.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create")]
        public ActionResult CreatePMServiceItem(long preventiveMaintenanceId)
        {
            var pMServiceItem = new PMServiceItem
            {
                IsNew = true
            };
            _pMServiceItemRepository.Insert(pMServiceItem);

            var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(preventiveMaintenanceId);
            preventiveMaintenance.PMServiceItems.Add(pMServiceItem);

            this._dbContext.SaveChanges();

            var model = new PMServiceItemModel();
            model = pMServiceItem.ToModel();
            var html = this.PMServiceItemPanel(model);

            return Json(new { Id = pMServiceItem.Id, Html = html });
        }

        [NonAction]
        public string PMServiceItemPanel(PMServiceItemModel model)
        {
            var html = this.RenderPartialViewToString("_PMServiceItemDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult SavePMServiceItem(PMServiceItemModel model)
        {
            if (ModelState.IsValid)
            {
                var pMServiceItem = _pMServiceItemRepository.GetById(model.Id);
                //always set IsNew to false when saving
                pMServiceItem.IsNew = false;
                //update totals
                model.PlanTotal = (model.PlanQuantity ?? 0) * (model.PlanUnitPrice ?? 0);

                pMServiceItem = model.ToEntity(pMServiceItem);

                _pMServiceItemRepository.UpdateAndCommit(pMServiceItem);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult CancelPMServiceItem(long id)
        {
            var pMServiceItem = _pMServiceItemRepository.GetById(id);
            if (pMServiceItem.IsNew == true)
            {
                _pMServiceItemRepository.DeleteAndCommit(pMServiceItem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Delete")]
        public ActionResult DeletePMServiceItem(long? parentId, long id)
        {
            var pMServiceItem = _pMServiceItemRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _pMServiceItemRepository.DeactivateAndCommit(pMServiceItem);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Delete")]
        public ActionResult DeleteSelectedPMServiceItems(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var pMServiceItem = _pMServiceItemRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _pMServiceItemRepository.Deactivate(pMServiceItem);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Work Orders

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Read")]
        public ActionResult WorkOrderList(long? preventiveMaintenanceId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderRepository.GetAll().Where(c => c.PreventiveMaintenanceId == preventiveMaintenanceId);
            query = sort == null ? query.OrderBy(a => a.Number) : query.Sort(sort);
            var workOrders = new PagedList<WorkOrder>(query, command.Page - 1, command.PageSize);

            var result = workOrders.Select(x => x.ToModel()).ToList();
            foreach (var r in result)
            {
                r.PriorityText = r.Priority.ToString();
            }

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = workOrders.TotalCount
            };

            return Json(gridModel);
        }

        #endregion

        #region Scheduling

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult PMMeterFrequencyList(long preventiveMaintenanceId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _pMMeterFrequencyRepository.GetAll().Where(c => c.PreventiveMaintenanceId == preventiveMaintenanceId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var pMMeterFrequencies = new PagedList<PMMeterFrequency>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = pMMeterFrequencies.Select(x => x.ToModel()),
                Total = pMMeterFrequencies.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult PMMeterFrequency(long id)
        {
            var pMMeterFrequency = _pMMeterFrequencyRepository.GetById(id);
            var model = pMMeterFrequency.ToModel();
            var html = this.PMMeterFrequencyPanel(model);
            return Json(new { Id = pMMeterFrequency.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult CreatePMMeterFrequency(long preventiveMaintenanceId)
        {
            var pMMeterFrequency = new PMMeterFrequency
            {
                IsNew = true,
                PreventiveMaintenanceId = preventiveMaintenanceId
            };
            _pMMeterFrequencyRepository.InsertAndCommit(pMMeterFrequency);

            var model = new PMMeterFrequencyModel();
            model = pMMeterFrequency.ToModel();
            var html = this.PMMeterFrequencyPanel(model);

            return Json(new { Id = pMMeterFrequency.Id, Html = html });
        }

        [NonAction]
        public string PMMeterFrequencyPanel(PMMeterFrequencyModel model)
        {
            var html = this.RenderPartialViewToString("_PMMeterFrequencyDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult SavePMMeterFrequency(PMMeterFrequencyModel model)
        {
            if (ModelState.IsValid)
            {
                var pMMeterFrequency = _pMMeterFrequencyRepository.GetById(model.Id);
                //always set IsNew to false when saving
                pMMeterFrequency.IsNew = false;

                pMMeterFrequency = model.ToEntity(pMMeterFrequency);
                _pMMeterFrequencyRepository.UpdateAndCommit(pMMeterFrequency);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult CancelPMMeterFrequency(long id)
        {
            var pMMeterFrequency = _pMMeterFrequencyRepository.GetById(id);
            if (pMMeterFrequency.IsNew == true)
            {
                _pMMeterFrequencyRepository.DeleteAndCommit(pMMeterFrequency);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult DeletePMMeterFrequency(long? parentId, long id)
        {
            var pMMeterFrequency = _pMMeterFrequencyRepository.GetById(id);
            _pMMeterFrequencyRepository.DeactivateAndCommit(pMMeterFrequency);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult DeleteSelectedPMMeterFrequencies(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var pMMeterFrequency = _pMMeterFrequencyRepository.GetById(id);
                _pMMeterFrequencyRepository.Deactivate(pMMeterFrequency);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }
        #endregion

        #region Meter Event
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Read")]
        public ActionResult MeterEventList(long? preventiveMaintenanceId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var meterEvents = _preventiveMaintenanceRepository.GetById(preventiveMaintenanceId).MeterEvents;
            if (meterEvents.Count == 0)
            {
                return Json(new DataSourceResult());
            }
            else
            {
                var queryable = meterEvents.AsQueryable<MeterEvent>();
                queryable = sort == null ? queryable.OrderBy(a => a.DisplayOrder) : queryable.Sort(sort);
                var data = queryable.ToList().Select(x => x.ToModel()).ToList();
                var gridModel = new DataSourceResult
                {
                    Data = data.PagedForCommand(command),
                    Total = meterEvents.Count()
                };

                return Json(gridModel);
            }
        }
        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Create,Maintenance.PreventiveMaintenance.Read,Maintenance.PreventiveMaintenance.Update")]
        public ActionResult AddMeterEvents(long? preventiveMaintenanceId, long[] selectedIds)
        {
            var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(preventiveMaintenanceId);

            foreach (var id in selectedIds)
            {
                var existed = preventiveMaintenance.MeterEvents.Any(s => s.Id == id);
                if (!existed)
                {
                    var meterEvent = _meterEventRepository.GetById(id);
                    preventiveMaintenance.MeterEvents.Add(meterEvent);
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Delete")]
        public ActionResult DeleteMeterEvent(long? parentId, long id)
        {
            var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(parentId);

            var meterEvent = _meterEventRepository.GetById(id);
            preventiveMaintenance.MeterEvents.Remove(meterEvent);

            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Maintenance.PreventiveMaintenance.Delete")]
        [HttpPost]
        public ActionResult DeleteSelectedMeterEvents(long? parentId, long[] selectedIds)
        {
            var preventiveMaintenance = _preventiveMaintenanceRepository.GetById(parentId);
            foreach (var meterEventId in selectedIds)
            {
                var meterEvent = _meterEventRepository.GetById(meterEventId);
                preventiveMaintenance.MeterEvents.Remove(meterEvent);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion
    }
}
