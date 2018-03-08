/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Caching;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Infrastructure;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class CommonController : BaseController
    {
        #region Fields

        private readonly IRepository<Feature> _featureRepository;
        private readonly IRepository<PointMeterLineItem> _pointMeterLineItemRepository;
        private readonly IRepository<Core.Domain.Asset> _assetRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IRepository<ServiceRequest> _serviceRequestRepository;
        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<StoreLocator> _storeLocatorRepository;
        private readonly IRepository<StoreLocatorItem> _storeLocatorItemRepository;
        private readonly IRepository<AssignmentHistory> _assignmentHistoryRepository;
        private readonly IRepository<Assignment> _assignmentRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IValueItemService _valueItemService;
        private readonly ILocationService _locationService;
        private readonly IMeterGroupService _meterGroupService;
        private readonly IAssignmentService _assignmentService;
        private readonly ISiteService _siteService;
        private readonly ILogger _logger;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly DapperContext _dapperContext;
        private readonly HttpContextBase _httpContext;
        private readonly IDbContext _dbContext;
        private readonly GeneralSettings _generalSettings;

        #endregion

        #region Constructors

        public CommonController(IRepository<Feature> featureRepository,
            IRepository<PointMeterLineItem> pointMeterLineItemRepository,
            IRepository<Core.Domain.Asset> assetRepository,
            IRepository<Location> locationRepository,
            IRepository<Store> storeRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<ServiceRequest> serviceRequestRepository,
            IRepository<Site> siteRepository,
            IRepository<StoreLocator> storeLocatorRepository,
            IRepository<StoreLocatorItem> storeLocatorItemRepository,
            IRepository<AssignmentHistory> assignmentHistoryRepository,
            IRepository<Assignment> assignmentRepository,
            IRepository<User> userRepository,
            IRepository<Address> addressRepository,
            IValueItemService valueItemService,
            ILocationService locationService,
            IMeterGroupService meterGroupService,
            IAssignmentService assignmentService,
            ISiteService siteService,
            ILogger logger,
            IDateTimeHelper dateTimeHelper,
            IWorkContext workContext,
            ILocalizationService localizationService,
            DapperContext dapperContext,
            HttpContextBase httpContext,
            IDbContext dbContext,
            GeneralSettings generalSettings)
        {
            this._featureRepository = featureRepository;
            this._pointMeterLineItemRepository = pointMeterLineItemRepository;
            this._assetRepository = assetRepository;
            this._locationRepository = locationRepository;
            this._storeRepository = storeRepository;
            this._workOrderRepository = workOrderRepository;
            this._serviceRequestRepository = serviceRequestRepository;
            this._siteRepository = siteRepository;
            this._storeLocatorRepository = storeLocatorRepository;
            this._storeLocatorItemRepository = storeLocatorItemRepository;
            this._assignmentHistoryRepository = assignmentHistoryRepository;
            this._assignmentRepository = assignmentRepository;
            this._userRepository = userRepository;
            this._addressRepository = addressRepository;
            this._valueItemService = valueItemService;
            this._locationService = locationService;
            this._meterGroupService = meterGroupService;
            this._assignmentService = assignmentService;
            this._siteService = siteService;
            this._logger = logger;
            this._dateTimeHelper = dateTimeHelper;
            this._workContext = workContext;
            this._localizationService = localizationService;
            this._dapperContext = dapperContext;
            this._httpContext = httpContext;
            this._dbContext = dbContext;
            this._generalSettings = generalSettings;
        }

        #endregion

        //page not found
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;

            return View();
        }

        public ActionResult AccessDenied(string pageUrl)
        {
            var currentUser = _workContext.CurrentUser;
            if (currentUser == null)
            {
                _logger.Information(string.Format("Access denied to anonymous request on {0}", pageUrl));
                return View();
            }

            _logger.Information(string.Format("Access denied to user #{0} '{1}' on {2}", currentUser.LoginName, currentUser.Email, pageUrl));


            return View();
        }

        [HttpPost]
        public ActionResult ClearCache()
        {
            var cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("baseeam_cache_static");
            cacheManager.Clear();
            SuccessNotification(_localizationService.GetResource("Common.CacheCleared"));
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "System.SystemInfo.Read")]
        public ActionResult SystemInfo()
        {
            var model = new SystemInfoModel();
            model.BaseEamVersion = BaseEamVersion.CurrentVersion;
            model.ServerTimeZone = TimeZone.CurrentTimeZone.StandardName;
            model.ServerLocalTime = DateTime.Now;
            model.UtcTime = DateTime.UtcNow;
            model.CurrentUserTime = _dateTimeHelper.ConvertToUserTime(DateTime.Now);

            return View(model);
        }

        [HttpPost]
        public JsonResult GetChoices(string dbTable, string dbTextColumn, string dbValueColumn, string param, bool autoBind = true)
        {
            var choices = new List<SelectListItem>();
            if(!autoBind && string.IsNullOrEmpty(param))
            {
                return Json(choices);
            }

            string query = string.Format(SqlTemplate.GetAll(), dbValueColumn, dbTextColumn, dbTable, param);
            IEnumerable<dynamic> result = null;
            result = _dapperContext.Query(query);

            foreach (IDictionary<string, object> obj in result)
            {
                choices.Add(new SelectListItem
                {
                    Value = obj[dbValueColumn].ToString(),
                    Text = obj[dbTextColumn].ToString()
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        /// <summary>
        /// Get choices with default dbValueColumn = "Id", dbTextColumn = "Name"
        /// </summary>
        [HttpPost]
        public JsonResult GetDefaultChoices(string dbTable, string param, bool autoBind = true)
        {
            var choices = new List<BaseEamListItem>();
            if (!autoBind && string.IsNullOrEmpty(param))
            {
                return Json(choices);
            }

            string query = string.Format(SqlTemplate.GetAll(), "Id", "Name", dbTable, param);
            IEnumerable<dynamic> result = null;
            result = _dapperContext.Query(query);

            foreach (IDictionary<string, object> obj in result)
            {
                choices.Add(new BaseEamListItem
                {
                    Id = obj["Id"].ToString(),
                    Name = obj["Name"].ToString()
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new BaseEamListItem { Id = "", Name = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        public JsonResult GetSqlChoices(string sessionKey, string name, string param, bool autoBind = true, long parentValue = 0)
        {
            var choices = new List<SelectListItem>();

            //get field from session
            var model = _httpContext.Session[sessionKey] as SearchModel;
            var fieldModel = new FieldModel();
            if (model == null)
            {
                return Json(choices);
            }
            else
            {
                fieldModel = model.Filters.Where(f => f.Name == name).FirstOrDefault();
                if(fieldModel == null)
                {
                    return Json(choices);
                }
            }

            string query = string.Format(fieldModel.SqlQuery, parentValue, param);
            IEnumerable<dynamic> result = null;
            result = _dapperContext.Query(query);

            foreach (IDictionary<string, object> obj in result)
            {
                choices.Add(new SelectListItem
                {
                    Value = obj[fieldModel.SqlValueField].ToString(),
                    Text = obj[fieldModel.SqlTextField].ToString()
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        public JsonResult TimeZones(string param)
        {
            var timezones = TimeZoneInfo.GetSystemTimeZones().Where(z => z.DisplayName.Contains(param));
            var choices = new List<SelectListItem>();
            foreach (var timezone in timezones)
            {
                choices.Add(new SelectListItem
                {
                    Value = timezone.Id,
                    Text = timezone.DisplayName
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        public JsonResult ValueItems(string category, string param)
        {
            var items = _valueItemService.GetValueItemsByCategory(category, param);
            var choices = new List<SelectListItem>();
            foreach (var item in items)
            {
                choices.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        public JsonResult Entities(string param)
        {
            var entities = _featureRepository.GetAll()
                .Where(f => !string.IsNullOrEmpty(f.EntityType) && f.EntityType.Contains(param))
                .ToList();

            var choices = new List<SelectListItem>();
            foreach (var item in entities)
            {
                choices.Add(new SelectListItem
                {
                    Value = item.EntityType,
                    Text = item.EntityType
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        public JsonResult WorkflowEntities(string param)
        {
            var entities = _featureRepository.GetAll()
                .Where(f => !string.IsNullOrEmpty(f.EntityType) && f.WorkflowEnabled == true && f.EntityType.Contains(param))
                .ToList();

            var choices = new List<SelectListItem>();
            foreach (var item in entities)
            {
                choices.Add(new SelectListItem
                {
                    Value = item.EntityType,
                    Text = item.EntityType
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        public JsonResult ImportEntities(string param)
        {
            var entities = _featureRepository.GetAll()
                .Where(f => !string.IsNullOrEmpty(f.EntityType) && f.ImportEnabled == true && f.EntityType.Contains(param))
                .ToList();

            var choices = new List<SelectListItem>();
            foreach (var item in entities)
            {
                choices.Add(new SelectListItem
                {
                    Value = item.EntityType,
                    Text = item.EntityType
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        public JsonResult AuditTrailEntities(string param)
        {
            var entities = _featureRepository.GetAll()
                .Where(f => !string.IsNullOrEmpty(f.EntityType) && f.AuditTrailEnabled == true && f.EntityType.Contains(param))
                .ToList();

            var choices = new List<SelectListItem>();
            foreach (var item in entities)
            {
                choices.Add(new SelectListItem
                {
                    Value = item.EntityType,
                    Text = item.EntityType
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        public JsonResult SiteLocationList(long parentValue, string param)
        {
            var locations = _locationService.GetSiteLocationList(parentValue, param);
            var choices = new List<SelectListItem>();
            foreach (var location in locations)
            {
                choices.Add(new SelectListItem
                {
                    Value = location.Id.ToString(),
                    Text = location.Name
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        [HttpPost]
        public JsonResult MeterGroupList(string param)
        {
            var meterGroups = _meterGroupService.GetMeterGroupList(param);
            var choices = new List<SelectListItem>();
            foreach (var meterGroup in meterGroups)
            {
                choices.Add(new SelectListItem
                {
                    Value = meterGroup.Id.ToString(),
                    Text = meterGroup.Name
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        /// <summary>
        /// Get the list of work orders 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WorkOrderList(string param)
        {
            var workOrders = _workOrderRepository.GetAll()
                .Where(w => w.Number.Contains(param) || w.Description.Contains(param)).ToList();
            var choices = new List<SelectListItem>();
            foreach (var workOrder in workOrders)
            {
                choices.Add(new SelectListItem
                {
                    Value = workOrder.Id.ToString(),
                    Text = workOrder.Number
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        /// <summary>
        /// Get the list of Service Requests
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ServiceRequestList(string param)
        {
            var serviceRequests = _serviceRequestRepository.GetAll()
                 .Where(w => w.Number.Contains(param) || w.Description.Contains(param)).ToList();
            var choices = new List<SelectListItem>();
            foreach (var serviceRequest in serviceRequests)
            {
                choices.Add(new SelectListItem
                {
                    Value = serviceRequest.Id.ToString(),
                    Text = serviceRequest.Number
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        /// <summary>
        /// Get the list of assets based on
        /// security group of the current user
        /// </summary>
        /// <param name="param">siteId</param>
        [HttpPost]
        public JsonResult AssetList(string param, long? parentValue)
        {
            var securityGroupIds = this._workContext.CurrentUser.SecurityGroups.Select(s => s.Id).ToList();
            var siteIds = _siteRepository.GetAll()
                .Where(s => s.SecurityGroups.Any(sg => securityGroupIds.Contains(sg.Id)))
                .Select(s => s.Id)
                .ToList();

            var assets = new List<Core.Domain.Asset>();
            if (parentValue > 0)
            {
                assets = _assetRepository.GetAll()
                .Where(a => a.SiteId == parentValue && a.Name.Contains(param))
                .OrderBy(a => a.Name)
                .ToList();
            }
            else
            {
                assets = _assetRepository.GetAll()
                .Where(a => siteIds.Contains(a.SiteId.Value) && a.Name.Contains(param))
                .OrderBy(a => a.Name)
                .ToList();
            }

            var choices = new List<SelectListItem>();
            foreach (var asset in assets)
            {
                choices.Add(new SelectListItem
                {
                    Value = asset.Id.ToString(),
                    Text = asset.Name
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        /// <summary>
        /// Get the list of locations based on
        /// security group of the current user
        /// </summary>
        /// <param name="param">siteId</param>
        [HttpPost]
        public JsonResult LocationList(string param, long? parentValue)
        {
            var securityGroupIds = this._workContext.CurrentUser.SecurityGroups.Select(s => s.Id).ToList();
            var siteIds = _siteRepository.GetAll()
                .Where(s => s.SecurityGroups.Any(sg => securityGroupIds.Contains(sg.Id)))
                .Select(s => s.Id)
                .ToList();

            var locations = new List<Location>();
            if (parentValue > 0)
            {
                locations = _locationRepository.GetAll()
                .Where(a => a.SiteId == parentValue && a.Name.Contains(param))
                .OrderBy(a => a.Name)
                .ToList();
            }
            else
            {
                locations = _locationRepository.GetAll()
                .Where(a => siteIds.Contains(a.SiteId.Value) && a.Name.Contains(param))
                .OrderBy(a => a.Name)
                .ToList();
            }

            var choices = new List<SelectListItem>();
            foreach (var location in locations)
            {
                choices.Add(new SelectListItem
                {
                    Value = location.Id.ToString(),
                    Text = location.Name
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        /// <summary>
        /// Get the list of store based on
        /// security group of the current user
        /// </summary>
        /// <param name="param">siteId</param>
        [HttpPost]
        public JsonResult StoreList(string param, long? parentValue)
        {
            var securityGroupIds = this._workContext.CurrentUser.SecurityGroups.Select(s => s.Id).ToList();
            var siteIds = _siteRepository.GetAll()
                .Where(s => s.SecurityGroups.Any(sg => securityGroupIds.Contains(sg.Id)))
                .Select(s => s.Id)
                .ToList();

            var stores = new List<Store>();
            if(parentValue > 0)
            {
                stores = _storeRepository.GetAll()
                .Where(a => a.SiteId == parentValue && a.Name.Contains(param))
                .OrderBy(a => a.Name)
                .ToList();
            }
            else
            {
                stores = _storeRepository.GetAll()
                .Where(a => siteIds.Contains(a.SiteId.Value) && a.Name.Contains(param))
                .OrderBy(a => a.Name)
                .ToList();
            }

            var choices = new List<SelectListItem>();
            foreach (var store in stores)
            {
                choices.Add(new SelectListItem
                {
                    Value = store.Id.ToString(),
                    Text = store.Name
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        /// <summary>
        /// Get locator info
        /// </summary>
        /// <param name="parentValue">storeId</param>
        /// <param name="additionalValue">itemId</param>
        /// <param name="param"></param>
        [HttpPost]
        public JsonResult GetStoreLocators(long? parentValue, long? additionalValue, string param)
        {
            var choices = new List<SelectListItem>();
            if (parentValue == null || additionalValue == null)
                return Json(choices);

            var query1 = from sli in _storeLocatorItemRepository.GetAll()
                         where sli.StoreId == parentValue && sli.ItemId == additionalValue
                         group sli by new { sli.StoreId, sli.StoreLocatorId, sli.ItemId }
                         into g
                         select new
                         {
                             StoreLocatorId = g.Key.StoreLocatorId,
                             Quantity = g.Sum(e => e.Quantity)
                         };

            var data = from sl in _storeLocatorRepository.GetAll()
                       where sl.StoreId == parentValue //need to put condition here, if remove this LINQ LEFT JOIN doesn't work ???
                       join q in query1 on sl.Id equals q.StoreLocatorId
                       into r1
                       from r in r1.DefaultIfEmpty()
                       select new
                       {
                           StoreLocatorId = sl.Id,
                           StoreLocatorName = sl.Name,
                           Quantity = r.Quantity == null ? 0 : r.Quantity
                       };

            foreach (var item in data)
            {
                choices.Add(new SelectListItem
                {
                    Value = item.StoreLocatorId.ToString(),
                    Text = item.StoreLocatorName + " [" + item.Quantity.Value.ToString("N2") + "]"
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }

            return Json(choices);
        }

        /// <summary>
        /// Get the list of columns into the entity
        /// </summary>
        /// <param name="parentValue">EntityType</param>
        /// <param name="param">The text input from user</param>
        [HttpPost]
        public JsonResult GetColumnNamesFromEntityType(string parentValue, string param)
        {
            var choices = new List<SelectListItem>();
            var columns =  _dbContext.GetColumnNames(parentValue, param);
            foreach(var column in columns)
            {
                choices.Add(new SelectListItem
                {
                    Value = column,
                    Text = column
                });
            }
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(choices);
        }

        // <summary>
        /// Get the list of meters of a asset and location under site
        /// </summary>
        /// <param name="parentValue">siteId</param>
        /// <param name="additionalValue">assetId</param>
        /// <param name="optionalValue">locationId</param>
        /// <param name="param">The text input from user</param>
        [HttpPost]
        public JsonResult MeterLineItemList(long parentValue, int? additionalValue, int? optionalValue, string param)
        {
            var choices = new List<SelectListItem>();
            var pointMeterLineItems = _pointMeterLineItemRepository.GetAll()
                .Where(m => (additionalValue != null && m.Point.AssetId == additionalValue && m.Point.Asset.SiteId == parentValue)
                            || (optionalValue != null && m.Point.LocationId == optionalValue && m.Point.Location.SiteId == parentValue)).ToList();

            foreach (var pointMeterLineItem in pointMeterLineItems)
            {
                choices.Add(new SelectListItem
                {
                    Value = pointMeterLineItem.MeterId.ToString(),
                    Text = pointMeterLineItem.Meter.Name
                });
            }
            if (choices.Count > 0)
            {
                choices.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(choices);


        }

        #region AssignmentHistories

        [HttpPost]
        public ActionResult AssignmentHistoryList(long? entityId, string entityType, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _assignmentHistoryRepository.GetAll()
                .Where(c => c.EntityId == entityId && c.EntityType == entityType);
            query = sort == null ? query.OrderBy(a => a.CreatedDateTime) : query.Sort(sort);
            var histories = new PagedList<AssignmentHistory>(query, command.Page - 1, command.PageSize);
            var result = histories.Select(x => x.ToModel()).ToList();
            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = histories.TotalCount
            };

            return Json(gridModel);
        }

        #endregion

        #region Reassign

        [HttpPost]
        public ActionResult Reassign(long entityId, string entityType, long[] selectedIds)
        {
            var assignedUsers = _assignmentService.Reassign(entityId, entityType, selectedIds);
            SuccessNotification("Record.Reassigned");
            return Json(new
            {
                assignedUsers = assignedUsers
            });
        }

        #endregion

        #region Barcode

        public ActionResult BarcodeImage(string barcode)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            try
            {
                Image img = b.Encode((BarcodeLib.TYPE)_generalSettings.BarcodeType, barcode, Color.Black, Color.White, 400, 100);
                string base64String;
                using (MemoryStream m = new MemoryStream())
                {
                    img.Save(m, ImageFormat.Png);
                    byte[] imageBytes = m.ToArray();
                    // Convert byte[] to Base64 String
                    base64String = Convert.ToBase64String(imageBytes);
                }
                var imgSrc = string.Format("data:image/gif;base64,{0}", base64String);
                return View(model: imgSrc);
            }
            catch(Exception ex)
            {
                ErrorNotification(ex.Message);
            }
            return View(model: "");
        }

        #endregion

        #region Address

        [HttpPost]
        public ActionResult AddressInfo(long? addressId)
        {
            if (addressId == null || addressId == 0)
                return new NullJsonResult();

            var address = _addressRepository.GetById(addressId);

            var addressInfo = address.ToModel();
            return Json(new { addressInfo = addressInfo });
        }

        #endregion

        #region User Preferences

        [HttpPost]
        public virtual ActionResult SavePreference(string name, bool value)
        {
            var userPreferences = _httpContext.Session[SessionKey.UserPreferences] as UserPreferences;
            if (userPreferences == null)
            {
                userPreferences = new UserPreferences();
            }

            if(name == "SidebarCollapsed")
            {
                userPreferences.SidebarCollapsed = value;
            }

            _httpContext.Session[SessionKey.UserPreferences] = userPreferences;

            return Json(new
            {
                Result = true
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PaymentMethod
        ///Get the list of payment method from value item
        [HttpPost]
        public JsonResult GetPaymentMethods(string additionalValue, string param)
        {
            var items = _valueItemService.GetValueItemsByCategory(additionalValue, param);
            var choices = new List<BaseEamListItem>();
            foreach (var item in items)
            {
                choices.Add(new BaseEamListItem
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            //add empty value
            if (choices.Count > 0)
            {
                choices.Insert(0, new BaseEamListItem { Id = "", Name = "" });
            }

            return Json(choices);


        }
        #endregion
    }
}