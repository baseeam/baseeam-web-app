/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BaseEAM.Web.Extensions;
using BaseEAM.Core.Data;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework;

namespace BaseEAM.Web.Controllers
{
    public class LookupController : BaseController
    {
        #region Fields
        private readonly IRepository<Point> _pointRepository;
        private readonly IRepository<MeterEvent> _meterEventRepository;
        private readonly ISiteService _siteService;
        private readonly ISecurityGroupService _securityGroupService;
        private readonly IMeterService _meterService;
        private readonly IUserService _userService;
        private readonly IAttributeService _attributeService;
        private readonly ITechnicianService _technicianService;
        private readonly ITeamService _teamService;
        private readonly IItemService _itemService;
        private readonly IAssetService _assetService;
        private readonly IWorkOrderService _workOrderService;
        private readonly ILocationService _locationService;
        private readonly IStoreService _storeService;
        private readonly IServiceRequestService _serviceRequestService;
        private readonly ICompanyService _companyService;

        #endregion

        #region Constructors

        public LookupController(
            IRepository<Point> pointRepository,
            IRepository<MeterEvent> meterEventRepository,
            ISiteService siteService,
            ISecurityGroupService securityGroupService,
            IMeterService meterService,
            IUserService userService,
            IAttributeService attributeService,
            ITechnicianService technicianService,
            ITeamService teamService,
            IItemService itemService,
            IAssetService assetService,
            IWorkOrderService workOrderService,
            ILocationService locationService,
            IStoreService storeService,
            IServiceRequestService serviceRequestService,
            ICompanyService companyService)
        {
            this._pointRepository = pointRepository;
            this._meterEventRepository = meterEventRepository;
            this._siteService = siteService;
            this._securityGroupService = securityGroupService;
            this._meterService = meterService;
            this._userService = userService;
            this._attributeService = attributeService;
            this._technicianService = technicianService;
            this._teamService = teamService;
            this._itemService = itemService;
            this._assetService = assetService;
            this._workOrderService = workOrderService;
            this._locationService = locationService;
            this._storeService = storeService;
            this._serviceRequestService = serviceRequestService;
            this._companyService = companyService;
        }

        #endregion

        #region Utilities

        private SearchModel BuildTeamSearchModel()
        {
            var model = new SearchModel();
            var nameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Team.Name",
                DbColumn = "Team.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(nameFilter);

            return model;
        }

        private SearchModel BuildTechnicianSearchModel()
        {
            var model = new SearchModel();
            var userNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "UserName",
                ResourceKey = "Technician.User",
                DbColumn = "User.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(userNameFilter);

            return model;
        }

        private SearchModel BuildAttributeSearchModel()
        {
            var model = new SearchModel();
            var siteNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Attribute.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(siteNameFilter);

            return model;
        }

        private SearchModel BuildSiteSearchModel()
        {
            var model = new SearchModel();
            var siteNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Site.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(siteNameFilter);

            return model;
        }

        private SearchModel BuildSecurityGroupSearchModel()
        {
            var model = new SearchModel();
            var siteNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "SecurityGroup.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(siteNameFilter);

            return model;
        }

        private SearchModel BuildMeterSearchModel()
        {
            var model = new SearchModel();
            var siteNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Meter.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(siteNameFilter);

            return model;
        }

        private SearchModel BuildUserSearchModel()
        {
            var model = new SearchModel();
            var siteNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "User.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(siteNameFilter);

            return model;
        }

        private SearchModel BuildItemSearchModel()
        {
            var model = new SearchModel();
            var itemNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "ItemName",
                ResourceKey = "Item.Name",
                DbColumn = "Item.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(itemNameFilter);

            var itemGroupFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "ItemGroup",
                ResourceKey = "Item.ItemGroup",
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

            var itemCategoryFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "ItemCategory",
                ResourceKey = "Item.ItemCategory",
                DbColumn = "Item.ItemCategory",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Part,Tool,Asset,Other",
                CsvValueList = "0,1,2,3",
                IsRequiredField = false
            };
            model.Filters.Add(itemCategoryFilter);

            var barcodeFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "Barcode",
                ResourceKey = "Item.Barcode",
                DbColumn = "Item.Barcode",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(barcodeFilter);

            var itemStatusFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "ItemStatus",
                ResourceKey = "Item.ItemStatus",
                DbColumn = "Item.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "ValueItems",
                AdditionalField = "category",
                AdditionalValue = "Item Status",
                IsRequiredField = false
            };
            model.Filters.Add(itemStatusFilter);

            return model;
        }

        private SearchModel BuildAssetSearchModel(long? parentValue)
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
                Name = "SL_Site",
                ResourceKey = "Site",
                DbColumn = "Site.Id",
                Value = parentValue,
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

        private SearchModel BuildLocationSearchModel(long? parentValue)
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
                Value = parentValue,
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

        private SearchModel BuildStoreSearchModel(long? parentValue)
        {
            var model = new SearchModel();

            var storeNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "StoreName",
                ResourceKey = "Store",
                DbColumn = "Store.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(storeNameFilter);

            var siteFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "SL_Site",
                ResourceKey = "Site",
                DbColumn = "Site.Id",
                Value = parentValue,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Site",
                MvcAction = "SiteList",
                IsRequiredField = false
            };
            model.Filters.Add(siteFilter);

            var storeTypeFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "StoreType",
                ResourceKey = "StoreType",
                DbColumn = "StoreType.Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "ValueItems",
                AdditionalField = "category",
                AdditionalValue = "Store Type",
                IsRequiredField = false
            };
            model.Filters.Add(storeTypeFilter);

            return model;
        }

        private SearchModel BuildWorkOrderSearchModel(long? parentValue)
        {
            var model = new SearchModel();

            var numberFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "WorkOrderNumber",
                ResourceKey = "WorkOrder.Number",
                DbColumn = "WorkOrder.Number, WorkOrder.Description",
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
                Name = "SL_Site",
                ResourceKey = "Site",
                DbColumn = "Site.Id",
                Value = parentValue,
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
                Name = "WorkOrderPriority",
                ResourceKey = "Priority",
                DbColumn = "WorkOrder.Priority",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int32,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Urgent,High,Medium,Low",
                CsvValueList = "0,1,2,3",
                IsRequiredField = false
            };
            model.Filters.Add(priorityFilter);

            var statusFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "WorkOrderStatus",
                ResourceKey = "WorkOrder.Status",
                DbColumn = "Assignment.Name",
                Value = null,
                ControlType = FieldControlType.MultiSelectList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Open,Planning,Execution,WaitingForMaterial,WaitingForVendor,Review,Closed,Rejected,Cancelled",
                CsvValueList = "'Open','Planning','Execution','WaitingForMaterial','WaitingForVendor','Review','Closed','Rejected','Cancelled'",
                IsRequiredField = false
            };
            model.Filters.Add(statusFilter);

            return model;
        }

        private SearchModel BuildServiceRequestSearchModel(long? parentValue)
        {
            var model = new SearchModel();

            var numberFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "ServiceRequestNumber",
                ResourceKey = "ServiceRequest.Number",
                DbColumn = "ServiceRequest.Number, ServiceRequest.Description",
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
                Name = "SL_Site",
                ResourceKey = "Site",
                DbColumn = "Site.Id",
                Value = parentValue,
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
                Name = "ServiceRequestPriority",
                ResourceKey = "Priority",
                DbColumn = "ServiceRequest.Priority",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int32,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Urgent,High,Medium,Low",
                CsvValueList = "0,1,2,3",
                IsRequiredField = false
            };
            model.Filters.Add(priorityFilter);

            var statusFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "ServiceRequestStatus",
                ResourceKey = "ServiceRequest.Status",
                DbColumn = "Assignment.Name",
                Value = null,
                ControlType = FieldControlType.MultiSelectList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Open,Review,Execution,Closed,Cancelled",
                CsvValueList = "'Open','Review','Execution','Closed','Cancelled'",
                IsRequiredField = false
            };
            model.Filters.Add(statusFilter);

            var requestorTypeFilter = new FieldModel
            {
                DisplayOrder = 5,
                Name = "RequestorType",
                ResourceKey = "RequestorType",
                DbColumn = "ServiceRequest.RequestorType",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int32,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "User,Anonymous",
                CsvValueList = "0,1",
                IsRequiredField = false
            };
            model.Filters.Add(requestorTypeFilter);


            return model;
        }

        private SearchModel BuildVendorSearchModel()
        {
            var model = new SearchModel();
            var nameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Company.Name",
                DbColumn = "Company.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(nameFilter);

            return model;
        }

        private SearchModel BuildMeterEventSearchModel()
        {
            var model = new SearchModel();
            var siteNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Meter.Name",
                DbColumn = "Meter.Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(siteNameFilter);

            return model;
        }
        #endregion

        #region Multi Site Lookup

        [HttpGet]
        public ActionResult MLSiteView()
        {
            var model = BuildSiteSearchModel();
            return PartialView("_MLSite", model);
        }

        [HttpPost]
        public ActionResult SiteList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildSiteSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Site> data = _siteService.GetSites(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => new { Id = x.Id, Name = x.Name, Description = x.Description }),
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Multi SecurityGroup Lookup

        [HttpGet]
        public ActionResult MLSecurityGroupView()
        {
            var model = BuildSecurityGroupSearchModel();
            return PartialView("_MLSecurityGroup", model);
        }

        [HttpPost]
        public ActionResult SecurityGroupList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildSecurityGroupSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<SecurityGroup> data = _securityGroupService.GetSecurityGroups(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => new { Id = x.Id, Name = x.Name, Description = x.Description }),
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Multi Meter Lookup

        [HttpGet]
        public ActionResult MLMeterView()
        {
            var model = BuildMeterSearchModel();
            return PartialView("_MLMeter", model);
        }

        [HttpPost]
        public ActionResult MeterList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildMeterSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Meter> data = _meterService.GetMeters(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => new { Id = x.Id, Name = x.Name, Description = x.Description }),
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Multi User Lookup

        [HttpGet]
        public ActionResult MLUserView()
        {
            var model = BuildUserSearchModel();
            return PartialView("_MLUser", model);
        }

        [HttpPost]
        public ActionResult UserList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildUserSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<User> data = _userService.GetUsers(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = data.Result.Select(x => new { Id = x.Id, Name = x.Name, Email = x.Email }),
                    Total = data.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Multi Attribute Lookup

        [HttpGet]
        public ActionResult MLAttributeView()
        {
            var model = BuildAttributeSearchModel();
            return PartialView("_MLAttribute", model);
        }

        [HttpPost]
        public ActionResult AttributeList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildAttributeSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Attribute> data = _attributeService.GetAttributes(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var r in result)
                {
                    r.ControlTypeText = r.ControlType.ToString();
                    r.DataTypeText = r.DataType.ToString();
                    r.DataSourceText = r.DataSource.ToString();
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
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Multi Technician Lookup

        [HttpGet]
        public ActionResult MLTechnicianView()
        {
            var model = BuildTechnicianSearchModel();
            return PartialView("_MLTechnician", model);
        }

        [HttpPost]
        public ActionResult TechnicianList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildTechnicianSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Technician> data = _technicianService.GetTechnicians(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                
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
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Multi Team Lookup

        [HttpGet]
        public ActionResult MLTeamView()
        {
            var model = BuildTeamSearchModel();
            return PartialView("_MLTeam", model);
        }

        [HttpPost]
        public ActionResult TeamList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildTeamSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Team> data = _teamService.GetTeams(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();

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
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Multi Item Lookup

        [HttpGet]
        public ActionResult MLItemView()
        {
            var model = BuildItemSearchModel();
            return PartialView("_MLItem", model);
        }

        [HttpPost]
        public ActionResult ItemList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildItemSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Item> data = _itemService.GetItems(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var r in result)
                {
                    r.ItemCategoryText = r.ItemCategory.ToString();
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
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Single Item Lookup

        [HttpGet]
        public ActionResult SLItemView(string valueFieldId, string textFieldId)
        {
            ViewBag.ValueFieldId = valueFieldId;
            ViewBag.TextFieldId = textFieldId;

            var model = BuildItemSearchModel();
            return PartialView("_SLItem", model);
        }

        #endregion

        #region Single Asset Lookup

        [HttpGet]
        public ActionResult SLAssetView(string valueFieldId, string textFieldId, long? parentValue)
        {
            ViewBag.ValueFieldId = valueFieldId;
            ViewBag.TextFieldId = textFieldId;

            var model = BuildAssetSearchModel(parentValue);
            return PartialView("_SLAsset", model);
        }

        [HttpPost]
        public ActionResult AssetList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildAssetSearchModel(null);
            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<BaseEAM.Core.Domain.Asset> data = _assetService.GetAssets(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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

        #endregion

        #region Single Location Lookup

        [HttpGet]
        public ActionResult SLLocationView(string valueFieldId, string textFieldId, long? parentValue)
        {
            ViewBag.ValueFieldId = valueFieldId;
            ViewBag.TextFieldId = textFieldId;

            var model = BuildLocationSearchModel(parentValue);
            return PartialView("_SLLocation", model);
        }

        [HttpPost]
        public ActionResult LocationList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildLocationSearchModel(null);
            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<BaseEAM.Core.Domain.Location> data = _locationService.GetLocations(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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

        #endregion

        #region Single Store Lookup

        [HttpGet]
        public ActionResult SLStoreView(string valueFieldId, string textFieldId, long? parentValue)
        {
            ViewBag.ValueFieldId = valueFieldId;
            ViewBag.TextFieldId = textFieldId;

            var model = BuildStoreSearchModel(parentValue);
            return PartialView("_SLStore", model);
        }

        [HttpPost]
        public ActionResult StoreList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildStoreSearchModel(null);
            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<Store> data = _storeService.GetStores(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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

        #endregion

        #region Single WorkOrder Lookup

        [HttpGet]
        public ActionResult SLWorkOrderView(string valueFieldId, string textFieldId, long? parentValue)
        {
            ViewBag.ValueFieldId = valueFieldId;
            ViewBag.TextFieldId = textFieldId;

            var model = BuildWorkOrderSearchModel(parentValue);
            return PartialView("_SLWorkOrder", model);
        }

        [HttpPost]
        public ActionResult WorkOrderList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildWorkOrderSearchModel(null);
            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<WorkOrder> data = _workOrderService.GetWorkOrders(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
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

        #endregion

        #region Single Service Request Lookup

        [HttpGet]
        public ActionResult SLServiceRequestView(string valueFieldId, string textFieldId, long? parentValue)
        {
            ViewBag.ValueFieldId = valueFieldId;
            ViewBag.TextFieldId = textFieldId;

            var model = BuildServiceRequestSearchModel(parentValue);
            return PartialView("_SLServiceRequest", model);
        }

        [HttpPost]
        public ActionResult ServiceRequestList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildServiceRequestSearchModel(null);
            if (ModelState.IsValid)
            {
                model.Update(searchValues);

                PagedResult<ServiceRequest> data = _serviceRequestService.GetServiceRequests(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var item in result)
                {
                    item.PriorityText = item.Priority.ToString();
                    item.RequestorTypeText = item.RequestorType.ToString();
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

        #endregion

        #region Multi Vendor Lookup

        [HttpGet]
        public ActionResult MLVendorView()
        {
            var model = BuildVendorSearchModel();
            return PartialView("_MLVendor", model);
        }

        [HttpPost]
        public ActionResult VendorList(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = BuildVendorSearchModel();

            if (ModelState.IsValid)
            {
                model.Update(searchValues);
                var expression = model.ToExpression();
                //do a hack here to specify a Vendor
                expression = expression + " AND ValueItem.Name LIKE '%Vendor%' ";
                PagedResult<Company> data = _companyService.GetCompanies(expression, model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion

        #region Multi MeterEvent Lookup

        [HttpGet]
        public ActionResult MLMeterEventView(long? assetId, long? locationId)
        {
            var model = BuildMeterEventSearchModel();
            ViewBag.AssetId = assetId;
            ViewBag.LocationId = locationId;
            return PartialView("_MLMeterEvent", model);
        }

        [HttpPost]
        public ActionResult MeterEventList(long? assetId, long? locationId, string meterName, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            if (ModelState.IsValid)
            {
                var points = _pointRepository.GetAll()
              .Where(p => ((assetId != null && p.AssetId == assetId)
              || (locationId != null && p.LocationId == locationId)))
              .ToList();

                if (points.Count() == 0)
                {
                    return new NullJsonResult();
                }

                var meterEvents = new List<MeterEvent>();
                foreach (var point in points)
                {
                    var meterEventList = _meterEventRepository.GetAll().Where(m => m.PointId == point.Id).ToList();
                    if (meterEventList.Count() > 0)
                    {
                        meterEvents.AddRange(meterEventList);
                    }
                }

                //Search by meterName
                if (!string.IsNullOrEmpty(meterName))
                {
                    meterEvents = meterEvents.Where(m => m.Meter.Name.IndexOf(meterName, System.StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                var gridModel = new DataSourceResult
                {
                    Data = meterEvents.Select(x => x.ToModel()).PagedForCommand(command),
                    Total = meterEvents.Count
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        #endregion
    }
}