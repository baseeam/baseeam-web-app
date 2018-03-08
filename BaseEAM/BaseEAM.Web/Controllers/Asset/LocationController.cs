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
using System.IO;

namespace BaseEAM.Web.Controllers
{
    public class LocationController : BaseController
    {
        #region Fields

        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<Point> _pointRepository;
        private readonly IRepository<BaseEAM.Core.Domain.Asset> _assetRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly ILocationService _locationService;
        private readonly IEntityAttributeService _entityAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public LocationController(IRepository<Location> locationRepository,
            IRepository<Point> pointRepository,
            IRepository<BaseEAM.Core.Domain.Asset> assetRepository,
            IRepository<Address> addressRepository,
            IRepository<WorkOrder> workOrderRepository,
            ILocationService locationService,
            IEntityAttributeService entityAttributeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._locationRepository = locationRepository;
            this._pointRepository = pointRepository;
            this._assetRepository = assetRepository;
            this._addressRepository = addressRepository;
            this._workOrderRepository = workOrderRepository;
            this._localizationService = localizationService;
            this._locationService = locationService;
            this._entityAttributeService = entityAttributeService;
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

            var locationNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "LocationName",
                ResourceKey = "Location",
                DbColumn = "Location.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(locationNameFilter);

            var siteFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "SL_Site",
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

            var locationTypeFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "LocationType",
                ResourceKey = "LocationType",
                DbColumn = "LocationType.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "ValueItems",
                AdditionalField = "category",
                AdditionalValue = "Location Type",
                IsRequiredField = false
            };
            model.Filters.Add(locationTypeFilter);

            var locationStatusFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "LocationStatus",
                ResourceKey = "LocationStatus",
                DbColumn = "LocationStatus.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "ValueItems",
                AdditionalField = "category",
                AdditionalValue = "Location Status",
                IsRequiredField = false
            };
            model.Filters.Add(locationStatusFilter);

            return model;
        }

        #endregion

        #region Locations

        [BaseEamAuthorize(PermissionNames = "Asset.Location.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.LocationSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.LocationSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Location.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.LocationSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.LocationSearchModel] = model;

                PagedResult<Location> data = _locationService.GetLocations(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Asset.Location.Create")]
        public ActionResult Create()
        {
            var location = new Location { IsNew = true };
            _locationRepository.InsertAndCommit(location);
            return Json(new { Id = location.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Location.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Location>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Asset.Location.Create,Asset.Location.Read,Asset.Location.Update")]
        public ActionResult Edit(long id)
        {
            var location = _locationRepository.GetById(id);
            var model = location.ToModel();
            if (model.Address == null)
                model.Address = new AddressModel();

            var point = _pointRepository.GetAll().Where(c => c.LocationId == id).FirstOrDefault();
            model.MeterGroupId = point != null ? point.MeterGroupId : null;
            model.PointId = point != null ? point.Id : 0;
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Location.Create,Asset.Location.Update")]
        public ActionResult Edit(LocationModel model)
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();

            var location = _locationRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                location = model.ToEntity(location);
                //set id for Address
                if(location.AddressId > 0)
                {
                    location.Address.Id = location.AddressId.Value;
                }

                //always set IsNew to false when saving
                location.IsNew = false;
                //update attributes
                _entityAttributeService.UpdateEntityAttributes(model.Id, EntityType.Location, json);
                _locationRepository.Update(location);

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
        [BaseEamAuthorize(PermissionNames = "Asset.Location.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var location = _locationRepository.GetById(id);

            if (!_locationService.IsDeactivable(location))
            {
                ModelState.AddModelError("Location", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _locationRepository.DeactivateAndCommit(location);
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
        [BaseEamAuthorize(PermissionNames = "Asset.Location.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var locations = new List<Location>();
            foreach (long id in selectedIds)
            {
                var location = _locationRepository.GetById(id);
                if (location != null)
                {
                    if (!_locationService.IsDeactivable(location))
                    {
                        ModelState.AddModelError("Location", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        locations.Add(location);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var location in locations)
                    _locationRepository.Deactivate(location);
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

        #region TreeView

        [HttpGet]
        public ActionResult TreeView(string valueFieldId, string textFieldId)
        {
            var model = new TreeViewLookup
            {
                TreeType = "Location",
                ValueFieldId = valueFieldId,
                TextFieldId = textFieldId
            };
            return PartialView("_TreeView", model);
        }

        [HttpPost,]
        public ActionResult TreeLoadChildren(int? id = null, string searchName = "")
        {
            var locations = _locationService.GetAllLocationsByParentId(id)
                .Select(x => new
                {
                    id = x.Id,
                    Name = x.Name,
                    hasChildren = _locationService.GetAllLocationsByParentId(x.Id).Count > 0,
                    imageUrl = Url.Content("~/Content/images/ico-cube.png")
                });

            return Json(locations);
        }

        #endregion

        #region Asset

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Location.Read")]
        public ActionResult AssetList(long locationId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _assetRepository.GetAll().Where(c => c.LocationId == locationId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var assets = new PagedList<Core.Domain.Asset>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = assets.Select(x => x.ToModel()),
                Total = assets.TotalCount
            };

            return Json(gridModel);
        }
        #endregion

        #region Work Order History

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Location.Read")]
        public ActionResult WorkOrderList(long? locationId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderRepository.GetAll().Where(c => c.LocationId == locationId);
            query = sort == null ? query.OrderBy(a => a.Number) : query.Sort(sort);
            var workOrders = new PagedList<WorkOrder>(query, command.Page - 1, command.PageSize);
            var result = workOrders.Select(x => x.ToModel()).ToList();
            foreach (var item in result)
            {
                item.PriorityText = item.Priority.ToString();
            }

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = workOrders.TotalCount
            };

            return Json(gridModel);
        }

        #endregion
    }
}