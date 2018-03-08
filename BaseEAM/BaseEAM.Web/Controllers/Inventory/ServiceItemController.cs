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
    public class ServiceItemController : BaseController
    {
        #region Fields

        private readonly IRepository<ServiceItem> _serviceItemRepository;
        private readonly IServiceItemService _serviceItemService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public ServiceItemController(IRepository<ServiceItem> serviceItemRepository,
            IServiceItemService serviceItemService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._serviceItemRepository = serviceItemRepository;
            this._localizationService = localizationService;
            this._serviceItemService = serviceItemService;
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
            var serviceItemNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "ServiceItem.Name",
                DbColumn = "ServiceItem.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(serviceItemNameFilter);

            var itemGroupFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "ItemGroup",
                ResourceKey = "ServiceItem.ItemGroup",
                DbColumn = "ItemGroup.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "ItemGroup",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(itemGroupFilter);

            return model;
        }

        #endregion

        #region ServiceItems

        [BaseEamAuthorize(PermissionNames = "Inventory.ServiceItem.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ServiceItemSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ServiceItemSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.ServiceItem.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ServiceItemSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.ServiceItemSearchModel] = model;

                PagedResult<ServiceItem> data = _serviceItemService.GetServiceItems(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.ServiceItem.Create")]
        public ActionResult Create()
        {
            var serviceItem = new ServiceItem { IsNew = true };
            _serviceItemRepository.InsertAndCommit(serviceItem);
            return Json(new { Id = serviceItem.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.ServiceItem.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<ServiceItem>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Inventory.ServiceItem.Create,Inventory.ServiceItem.Read,Inventory.ServiceItem.Update")]
        public ActionResult Edit(long id)
        {
            var serviceItem = _serviceItemRepository.GetById(id);
            var model = serviceItem.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.ServiceItem.Create,Inventory.ServiceItem.Update")]
        public ActionResult Edit(ServiceItemModel model)
        {
            var serviceItem = _serviceItemRepository.GetById(model.Id);

            if (ModelState.IsValid)
            {
                serviceItem = model.ToEntity(serviceItem);

                //always set IsNew to false when saving
                serviceItem.IsNew = false;
                _serviceItemRepository.Update(serviceItem);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.ServiceItem.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var serviceItem = _serviceItemRepository.GetById(id);

            if (!_serviceItemService.IsDeactivable(serviceItem))
            {
                ModelState.AddModelError("ServiceItem", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _serviceItemRepository.DeactivateAndCommit(serviceItem);
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
        [BaseEamAuthorize(PermissionNames = "Inventory.ServiceItem.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var serviceItems = new List<ServiceItem>();
            foreach (long id in selectedIds)
            {
                var serviceItem = _serviceItemRepository.GetById(id);
                if (serviceItem != null)
                {
                    if (!_serviceItemService.IsDeactivable(serviceItem))
                    {
                        ModelState.AddModelError("ServiceItem", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        serviceItems.Add(serviceItem);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var serviceItem in serviceItems)
                    _serviceItemRepository.Deactivate(serviceItem);
                this._dbContext.SaveChanges();
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        /// <summary>
        /// Get the list of service item active in our system
        /// <param name="param">The text input from user</param>
        /// </summary>
        [HttpPost]
        public JsonResult GetServiceItemActiveList(string param)
        {
            var serviceItems = _serviceItemRepository.GetAll().Where(s => s.Name.Contains(param) && s.IsActive == true)
                .Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() })
                .ToList();

            if (serviceItems.Count > 0)
            {
                serviceItems.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(serviceItems);
        }

        [HttpPost]
        public ActionResult ServiceItemInfo(long? serviceItemId)
        {
            if (serviceItemId == null || serviceItemId == 0)
                return new NullJsonResult();

            var serviceItemInfo = _serviceItemRepository.GetById(serviceItemId).ToModel();
            return Json(new { serviceItemInfo = serviceItemInfo });
        }
        #endregion
    }
}