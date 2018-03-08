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
using System;

namespace BaseEAM.Web.Controllers
{
    public class AssetController : BaseController
    {
        #region Fields

        private readonly IRepository<BaseEAM.Core.Domain.Asset> _assetRepository;
        private readonly IRepository<AssetSparePart> _assetSparePartRepository;
        private readonly IRepository<AssetStatusHistory> _assetStatusHistoryRepository;
        private readonly IRepository<AssetLocationHistory> _assetLocationHistoryRepository;
        private readonly IRepository<AssetDowntime> _assetDowntimeRepository;
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Point> _pointRepository;
        private readonly IRepository<ValueItem> _valueItemRepository;
        private readonly IAssetService _assetService;
        private readonly IEntityAttributeService _entityAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public AssetController(IRepository<BaseEAM.Core.Domain.Asset> assetRepository,
            IRepository<AssetSparePart> assetSparePartRepository,
            IRepository<AssetStatusHistory> assetStatusHistoryRepository,
            IRepository<AssetLocationHistory> assetLocationHistoryRepository,
            IRepository<AssetDowntime> assetDowntimeRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<Address> addressRepository,
            IRepository<Point> pointRepository,
            IRepository<ValueItem> valueItemRepository,
            IAssetService assetService,
            IEntityAttributeService entityAttributeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._assetRepository = assetRepository;
            this._assetSparePartRepository = assetSparePartRepository;
            this._assetStatusHistoryRepository = assetStatusHistoryRepository;
            this._assetLocationHistoryRepository = assetLocationHistoryRepository;
            this._assetDowntimeRepository = assetDowntimeRepository;
            this._workOrderRepository = workOrderRepository;
            this._addressRepository = addressRepository;
            this._pointRepository = pointRepository;
            this._valueItemRepository = valueItemRepository;
            this._localizationService = localizationService;
            this._assetService = assetService;
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
            var assetNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "AssetName",
                ResourceKey = "Asset.Name",
                DbColumn = "Asset.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(assetNameFilter);

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

            var assetTypeFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "AssetType",
                ResourceKey = "Asset.AssetType",
                DbColumn = "AssetType.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "ValueItems",
                AdditionalField = "category",
                AdditionalValue = "Asset Type",
                IsRequiredField = false
            };
            model.Filters.Add(assetTypeFilter);

            var assetStatusFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "AssetStatus",
                ResourceKey = "Asset.AssetStatus",
                DbColumn = "AssetStatus.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "ValueItems",
                AdditionalField = "category",
                AdditionalValue = "Asset Status",
                IsRequiredField = false
            };
            model.Filters.Add(assetStatusFilter);

            return model;
        }

        #endregion

        #region Assets

        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.AssetSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.AssetSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.AssetSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.AssetSearchModel] = model;

                PagedResult<BaseEAM.Core.Domain.Asset> data = _assetService.GetAssets(model.ToExpression(this._workContext.CurrentUser.Id), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Create")]
        public ActionResult Create()
        {
            var asset = new BaseEAM.Core.Domain.Asset { IsNew = true };
            _assetRepository.InsertAndCommit(asset);
            return Json(new { Id = asset.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<BaseEAM.Core.Domain.Asset>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Create,Asset.Asset.Read,Asset.Asset.Update")]
        public ActionResult Edit(long id)
        {
            var asset = _assetRepository.GetById(id);
            var model = asset.ToModel();
            var point = _pointRepository.GetAll().Where(c => c.AssetId == id).FirstOrDefault();
            model.MeterGroupId = point != null ? point.MeterGroupId : null;
            model.PointId = point != null ? point.Id : 0;
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Create,Asset.Asset.Update")]
        public ActionResult Edit(AssetModel model)
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();

            var asset = _assetRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                //update status history
                if(model.AssetStatusId != asset.AssetStatusId)
                {
                    asset.AssetStatusHistories.Add(new AssetStatusHistory
                    {
                        FromStatus = asset.AssetStatus.Name,
                        ToStatus = _valueItemRepository.GetById(model.AssetStatusId).Name,
                        ChangedUserId = this._workContext.CurrentUser.Id,
                        ChangedDateTime = DateTime.UtcNow
                    });
                }

                //update location history
                if(model.LocationId != asset.LocationId)
                {
                    asset.AssetLocationHistories.Add(new AssetLocationHistory
                    {
                        FromLocationId = asset.LocationId,
                        ToLocationId = model.LocationId,
                        ChangedUserId = this._workContext.CurrentUser.Id,
                        ChangedDateTime = DateTime.UtcNow
                    });
                }

                asset = model.ToEntity(asset);

                //always set IsNew to false when saving
                asset.IsNew = false;
                //update attributes
                _entityAttributeService.UpdateEntityAttributes(model.Id, EntityType.Asset, json);
                _assetRepository.Update(asset);

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
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var asset = _assetRepository.GetById(id);

            if (!_assetService.IsDeactivable(asset))
            {
                ModelState.AddModelError("Asset", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _assetRepository.DeactivateAndCommit(asset);
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
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var assets = new List<BaseEAM.Core.Domain.Asset>();
            foreach (long id in selectedIds)
            {
                var asset = _assetRepository.GetById(id);
                if (asset != null)
                {
                    if (!_assetService.IsDeactivable(asset))
                    {
                        ModelState.AddModelError("Asset", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        assets.Add(asset);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var asset in assets)
                    _assetRepository.Deactivate(asset);
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

        #region AssetSparePart

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Create,Asset.Asset.Read,Asset.Asset.Update")]
        public ActionResult AssetSparePartList(long assetId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _assetSparePartRepository.GetAll().Where(c => c.AssetId == assetId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var assetSpareParts = new PagedList<AssetSparePart>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = assetSpareParts.Select(x => x.ToModel()),
                Total = assetSpareParts.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Create,Asset.Asset.Update,Asset.Asset.Delete")]
        public ActionResult SaveChanges([Bind(Prefix = "updated")]List<AssetSparePartModel> updatedItems,
            [Bind(Prefix = "created")]List<AssetSparePartModel> createdItems,
            [Bind(Prefix = "deleted")]List<AssetSparePartModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create AssetSparePart
                    if (createdItems != null)
                    {
                        foreach (var model in createdItems)
                        {
                            var assetSparePart = _assetSparePartRepository.GetById(model.Id);
                            if (assetSparePart == null)
                            {
                                assetSparePart = new AssetSparePart
                                {
                                    AssetId = model.AssetId,
                                    ItemId = model.ItemId,
                                    Quantity = model.Quantity
                                };
                                _assetSparePartRepository.Insert(assetSparePart);
                            }

                        }
                    }

                    //Update AssetSparePart
                    if (updatedItems != null)
                    {
                        foreach (var model in updatedItems)
                        {
                            var assetSparePart = _assetSparePartRepository.GetById(model.Id);
                            assetSparePart.ItemId = model.ItemId;
                            assetSparePart.Quantity = model.Quantity;
                            _assetSparePartRepository.Update(assetSparePart);
                        }
                    }

                    //Delete AssetSparePart
                    if (deletedItems != null)
                    {
                        foreach (var model in deletedItems)
                        {
                            var assetSparePart = _assetSparePartRepository.GetById(model.Id);
                            if (assetSparePart != null)
                            {
                                _assetSparePartRepository.Deactivate(assetSparePart);
                            }
                        }
                    }

                    _dbContext.SaveChanges();
                    SuccessNotification(_localizationService.GetResource("Record.Saved"));
                    return new NullJsonResult();
                }
                catch (Exception e)
                {
                    return Json(new { Errors = e.Message });
                }
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }
        #endregion

        #region AssetStatusHistory

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Read")]
        public ActionResult AssetStatusHistoryList(long assetId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _assetStatusHistoryRepository.GetAll().Where(c => c.AssetId == assetId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var assetStatusHistories = new PagedList<AssetStatusHistory>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = assetStatusHistories.Select(x => x.ToModel()),
                Total = assetStatusHistories.TotalCount
            };

            return Json(gridModel);
        }
        #endregion

        #region AssetLocationHistory

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Read")]
        public ActionResult AssetLocationHistoryList(long assetId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _assetLocationHistoryRepository.GetAll().Where(c => c.AssetId == assetId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var assetLocationHistories = new PagedList<AssetLocationHistory>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = assetLocationHistories.Select(x => x.ToModel()),
                Total = assetLocationHistories.TotalCount
            };

            return Json(gridModel);
        }
        #endregion

        #region AssetDowntimes

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Read")]
        public ActionResult AssetDowntimeList(long assetId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _assetDowntimeRepository.GetAll().Where(c => c.AssetId == assetId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var assetDowntimes = new PagedList<AssetDowntime>(query, command.Page - 1, command.PageSize);
            var result = assetDowntimes.Select(x => x.ToModel()).ToList();

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = assetDowntimes.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Create,Asset.Asset.Read,Asset.Asset.Update")]
        public ActionResult AssetDowntime(long id)
        {
            var assetDowntime = _assetDowntimeRepository.GetById(id);
            var model = assetDowntime.ToModel();
            var html = this.AssetDowntimePanel(model);
            return Json(new { Id = assetDowntime.Id, Html = html });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Create")]
        public ActionResult CreateAssetDowntime(long assetId)
        {
            var assetDowntime = new AssetDowntime
            {
                IsNew = true,
                ReportedDateTime = DateTime.UtcNow,
                ReportedUserId =  this._workContext.CurrentUser.Id
            };
            _assetDowntimeRepository.Insert(assetDowntime);

            var asset = _assetRepository.GetById(assetId);
            asset.AssetDowntimes.Add(assetDowntime);

            this._dbContext.SaveChanges();

            var model = new AssetDowntimeModel();
            model = assetDowntime.ToModel();
            var html = this.AssetDowntimePanel(model);

            return Json(new { Id = assetDowntime.Id, Html = html });
        }

        [NonAction]
        public string AssetDowntimePanel(AssetDowntimeModel model)
        {
            var html = this.RenderPartialViewToString("_AssetDowntimeDetails", model);
            return html;
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Create,Asset.Asset.Update")]
        public ActionResult SaveAssetDowntime(AssetDowntimeModel model)
        {
            if (ModelState.IsValid)
            {
                var assetDowntime = _assetDowntimeRepository.GetById(model.Id);
                //always set IsNew to false when saving
                assetDowntime.IsNew = false;
              
                assetDowntime = model.ToEntity(assetDowntime);

                _assetDowntimeRepository.UpdateAndCommit(assetDowntime);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Create,Asset.Asset.Update")]
        public ActionResult CancelAssetDowntime(long id)
        {
            var assetDowntime = _assetDowntimeRepository.GetById(id);
            if (assetDowntime.IsNew == true)
            {
                _assetDowntimeRepository.DeleteAndCommit(assetDowntime);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Delete")]
        public ActionResult DeleteAssetDowntime(long? parentId, long id)
        {
            var assetDowntime = _assetDowntimeRepository.GetById(id);
            //For parent-child, we can mark deleted to child
            _assetDowntimeRepository.DeactivateAndCommit(assetDowntime);
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Delete")]
        public ActionResult DeleteSelectedAssetDowntimes(long? parentId, long[] selectedIds)
        {
            foreach (long id in selectedIds)
            {
                var assetDowntime = _assetDowntimeRepository.GetById(id);
                //For parent-child, we can mark deleted to child
                _assetDowntimeRepository.Deactivate(assetDowntime);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        #endregion

        #region Work Order History

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.Asset.Read")]
        public ActionResult WorkOrderList(long? assetId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _workOrderRepository.GetAll().Where(c => c.AssetId == assetId);
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

        #region TreeView

        [HttpGet]
        public ActionResult TreeView(string valueFieldId, string textFieldId)
        {
            var model = new TreeViewLookup
            {
                TreeType = "Asset",
                ValueFieldId = valueFieldId,
                TextFieldId = textFieldId
            };
            return PartialView("_TreeView", model);
        }

        [HttpPost,]
        public ActionResult TreeLoadChildren(int? id = null, string searchName = "")
        {
            var assets = _assetService.GetAllAssetsByParentId(id)
                .Select(x => new
                {
                    id = x.Id,
                    Name = x.Name,
                    hasChildren = _assetService.GetAllAssetsByParentId(x.Id).Count > 0,
                    imageUrl = Url.Content("~/Content/images/ico-cube.png")
                });

            return Json(assets);
        }

        #endregion
    }
}