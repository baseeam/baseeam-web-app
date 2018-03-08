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
using BaseEAM.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class PointController : BaseController
    {
        #region Fields

        private readonly IRepository<Point> _pointRepository;
        private readonly IRepository<PointMeterLineItem> _pointMeterLineItemRepository;
        private readonly IRepository<MeterLineItem> _meterLineItemRepository;
        private readonly IRepository<Reading> _readingRepository;
        private readonly IRepository<MeterEvent> _meterEventRepository;
        private readonly IRepository<MeterEventHistory> _meterEventHistoryRepository;
        private readonly IMeterService _meterService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public PointController(IRepository<Point> pointRepository,
            IRepository<PointMeterLineItem> pointMeterLineItemRepository,
            IRepository<MeterLineItem> meterLineItemRepository,
            IRepository<Reading> readingRepository,
            IRepository<MeterEvent> meterEventRepository,
            IRepository<MeterEventHistory> meterEventHistoryRepository,
            IMeterService meterService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._pointRepository = pointRepository;
            this._pointMeterLineItemRepository = pointMeterLineItemRepository;
            this._meterLineItemRepository = meterLineItemRepository;
            this._readingRepository = readingRepository;
            this._meterEventRepository = meterEventRepository;
            this._meterEventHistoryRepository = meterEventHistoryRepository;
            this._meterService = meterService;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._permissionService = permissionService;
            this._httpContext = httpContext;
            this._workContext = workContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Utilities
        #endregion

        #region Points

        #endregion

        #region PointMeterLineItem

        [HttpPost]
        public ActionResult PointMeterLineItemList(long? pointId, long? assetId, long? locationId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _pointMeterLineItemRepository.GetAll()
                .Where(c => c.PointId == pointId &&
                 ((assetId != null && c.Point.AssetId == assetId) || (locationId != null && c.Point.LocationId == locationId)));
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var pointMeterLineItems = new PagedList<PointMeterLineItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = pointMeterLineItems.Select(x => x.ToModel()),
                Total = pointMeterLineItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult PointMeterLineItem(long id)
        {
            var pointMeterLineItem = _pointMeterLineItemRepository.GetById(id);
            var model = pointMeterLineItem.ToModel();
            var html = this.PointMeterLineItemPanel(model);
            return Json(new { Id = pointMeterLineItem.Id, Html = html });
        }


        /// <summary>
        /// Create PointMeterLineItem for Asset or Location
        /// ONLY assetId != null OR locationId != null
        /// </summary>
        public ActionResult CreatePointMeterLineItem(long pointId, long? assetId, long? locationId)
        {
            var point = _pointRepository.GetAll()
                .Where(p => (assetId != null && p.AssetId == assetId)
                || (locationId != null && p.LocationId == locationId))
                .FirstOrDefault();
            if (point == null)
            {
                point = new Point
                {
                    AssetId = assetId,
                    LocationId = locationId
                };
                _pointRepository.Insert(point);
            }

            var pointMeterLineItem = new PointMeterLineItem
            {
                IsNew = true,
                Point = point
            };

            var maxDisplayOrder = _pointMeterLineItemRepository.GetAll().Where(p => p.PointId == pointId).Max(p => (int?)p.DisplayOrder) ?? 0;
            pointMeterLineItem.DisplayOrder = maxDisplayOrder + 1;
            _pointMeterLineItemRepository.Insert(pointMeterLineItem);

            this._dbContext.SaveChanges();

            var model = new PointMeterLineItemModel();
            model = pointMeterLineItem.ToModel();
            var html = this.PointMeterLineItemPanel(model);

            return Json(new { Id = pointMeterLineItem.Id, pointId = point.Id, Html = html });
        }

        /// <summary>
        /// Create PointMeterLineItems for Asset or Location
        /// ONLY assetId != null OR locationId != null
        /// </summary>
        [HttpPost]
        public ActionResult CreatePointMeterLineItems(long meterGroupId, long? assetId, long? locationId)
        {
            var point = _pointRepository.GetAll()
                .Where(p => ((assetId != null && p.AssetId == assetId)
                || (locationId != null && p.LocationId == locationId)))
                .FirstOrDefault();
            if (point != null)
            {
                if (point.PointMeterLineItems.Any(m => m.LastReadingValue != null))
                {
                    ModelState.AddModelError("", _localizationService.GetResource("Point.CannotChangeMeterGroup"));
                }
            }
            if (ModelState.IsValid)
            {
                if (point != null)
                {
                    point.PointMeterLineItems.Clear();
                }
                else
                {
                    point = new Point
                    {
                        AssetId = assetId,
                        LocationId = locationId
                    };
                    _pointRepository.Insert(point);
                }

                point.MeterGroupId = meterGroupId;
                var meterLineItems = _meterLineItemRepository.GetAll().Where(m => m.MeterGroupId == meterGroupId).ToList();
                var displayOrder = 0;
                foreach (var meterLineItem in meterLineItems)
                {
                    displayOrder += 1;
                    var pointMeterLineItem = new PointMeterLineItem
                    {
                        Point = point,
                        Meter = meterLineItem.Meter,
                        MeterId = meterLineItem.MeterId,
                        DisplayOrder = displayOrder
                    };
                    point.PointMeterLineItems.Add(pointMeterLineItem);
                }
                _pointRepository.Update(point);
                this._dbContext.SaveChanges();
                return Json(new { pointId = point.Id });
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [NonAction]
        public string PointMeterLineItemPanel(PointMeterLineItemModel model)
        {
            var html = this.RenderPartialViewToString("_PointMeterLineItemDetails", model);
            return html;
        }

        [HttpPost]
        public ActionResult SavePointMeterLineItem(PointMeterLineItemModel model)
        {
            if (ModelState.IsValid)
            {
                var pointMeterLineItem = _pointMeterLineItemRepository.GetById(model.Id);
                pointMeterLineItem.IsNew = false;
                if (model.ReadingValue.HasValue)
                {
                    var newReading = new Reading();
                    newReading.ReadingValue = model.ReadingValue;
                    newReading.DateOfReading = model.DateOfReading;
                    newReading.ReadingSource = (int?)ReadingSource.Directly;
                    pointMeterLineItem.LastDateOfReading = model.DateOfReading;
                    pointMeterLineItem.LastReadingValue = model.ReadingValue;
                    pointMeterLineItem.Readings.Add(newReading);
                    //Check and create a new meter event history if the reading value does not in the range of the list of meter events
                    _meterService.CreateMeterEventHistory(pointMeterLineItem, newReading);
                }
                pointMeterLineItem.MeterId = model.MeterId;
                pointMeterLineItem.LastReadingUser = this._workContext.CurrentUser.Name;
                _pointMeterLineItemRepository.Update(pointMeterLineItem);
                this._dbContext.SaveChanges();
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        public ActionResult CancelPointMeterLineItem(long id)
        {
            var pointMeterLineItem = _pointMeterLineItemRepository.GetById(id);
            if (pointMeterLineItem.IsNew == true)
            {
                _pointMeterLineItemRepository.DeleteAndCommit(pointMeterLineItem);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult DeletePointMeterLineItem(long? parentId, long id)
        {
            var pointMeterLineItem = _pointMeterLineItemRepository.GetById(id);
            if (pointMeterLineItem.LastReadingValue != null)
            {
                ModelState.AddModelError("", _localizationService.GetResource("Point.CannotDeleteMeterLineItem"));
            }

            if (ModelState.IsValid)
            {
                var meterEvents = _meterEventRepository.GetAll().Where(m => m.MeterId == pointMeterLineItem.MeterId && m.PointId == pointMeterLineItem.PointId).ToList();

                foreach(var meterEvent in meterEvents)
                {
                    _meterEventRepository.Delete(meterEvent);
                }

                _pointMeterLineItemRepository.Delete(pointMeterLineItem);

                this._dbContext.SaveChanges();
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        public ActionResult DeleteSelectedPointMeterLineItems(long? parentId, long[] selectedIds)
        {
            var pointMeterLineItems = new List<PointMeterLineItem>();
            foreach (long id in selectedIds)
            {
                var pointMeterLineItem = _pointMeterLineItemRepository.GetById(id);
                pointMeterLineItems.Add(pointMeterLineItem);
                if (pointMeterLineItem.LastReadingValue != null)
                {
                    ModelState.AddModelError("", _localizationService.GetResource("Point.CannotDeleteMeterLineItem"));
                    break;
                }
            }

            if (ModelState.IsValid)
            {
                foreach (PointMeterLineItem pointMeterLineItem in pointMeterLineItems)
                {
                    var meterEvents = _meterEventRepository.GetAll().Where(m => m.MeterId == pointMeterLineItem.MeterId && m.PointId == pointMeterLineItem.PointId).ToList();

                    foreach (var meterEvent in meterEvents)
                    {
                        _meterEventRepository.Delete(meterEvent);
                    }

                    _pointMeterLineItemRepository.Delete(pointMeterLineItem);
                }
                this._dbContext.SaveChanges();
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        #endregion

        #region Reading Histories

        [HttpGet]
        public ActionResult ReadingHistoriesView(long? id)
        {
            var pointMeterLineItemModel = _pointMeterLineItemRepository.GetById(id);
            var model = pointMeterLineItemModel.ToModel();

            return PartialView("_ReadingHistories", model);
        }

        [HttpPost]
        public ActionResult ReadingHistoryList(long pointMeterLineItemId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {

            var query = _readingRepository.GetAll().Where(c => c.PointMeterLineItemId == pointMeterLineItemId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var readingList = new PagedList<Reading>(query, command.Page - 1, command.PageSize);

            var result = readingList.ToList().Select(x => x.ToModel()).ToList();
            foreach (var r in result)
            {
                r.ReadingSourceText = r.ReadingSource.ToString();
            }

            var gridModel = new DataSourceResult
            {
                Data = result,
                Total = readingList.TotalCount
            };

            return Json(gridModel);

        }

        #endregion

        #region Meter Event
        [HttpPost]
        public ActionResult MeterEventList(long? pointMeterLineItemId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var pointMeterLineItem = _pointMeterLineItemRepository.GetById(pointMeterLineItemId);
            var pointId = pointMeterLineItem.PointId;
            var meterId = pointMeterLineItem.MeterId;

            var query = _meterEventRepository.GetAll().Where(c => c.PointId == pointId && c.MeterId == meterId);
            query = sort == null ? query.OrderBy(a => a.Name) : query.Sort(sort);
            var meterEvents = new PagedList<MeterEvent>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = meterEvents.Select(x => x.ToModel()),
                Total = meterEvents.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult MeterEvent(long? pointMeterLineItemId)
        {
            var meterEvent = _meterEventRepository.GetById(pointMeterLineItemId);
            var model = meterEvent.ToModel();
            var html = this.MeterEventPanel(model);
            return Json(new { Id = meterEvent.Id, Html = html });
        }

        public ActionResult CreateMeterEvent(long? pointMeterLineItemId)
        {
            var pointMeterLineItem = _pointMeterLineItemRepository.GetById(pointMeterLineItemId);
            var meterEvent = new MeterEvent();
            var meterEvents = _meterEventRepository.GetAll().Where(c => c.PointId == pointMeterLineItem.PointId && c.MeterId == pointMeterLineItem.MeterId);
            int displayOrder = meterEvents.Max(m => (int?)m.DisplayOrder) ?? 0;

            meterEvent.DisplayOrder = displayOrder + 1;
            meterEvent.PointId = pointMeterLineItem.PointId;
            meterEvent.MeterId = pointMeterLineItem.MeterId;
            meterEvent.Meter = pointMeterLineItem.Meter;
            meterEvent.IsNew = true;
            _meterEventRepository.InsertAndCommit(meterEvent);

            var model = new MeterEventModel();
            
            model = meterEvent.ToModel();
            var html = this.MeterEventPanel(model);

            return Json(new { Id = meterEvent.Id, Html = html });
        }

        [NonAction]
        public string MeterEventPanel(MeterEventModel model)
        {
            var html = this.RenderPartialViewToString("_MeterEventDetails", model);
            return html;
        }

        [HttpPost]
        public ActionResult SaveMeterEvent(MeterEventModel model)
        {
            if (ModelState.IsValid)
            {
                var meterEvent = _meterEventRepository.GetById(model.Id);
                meterEvent.IsNew = false;
                meterEvent.Description = model.Description;
                meterEvent.LowerLimit = model.LowerLimit;
                meterEvent.UpperLimit = model.UpperLimit;
                _meterEventRepository.UpdateAndCommit(meterEvent);
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.Errors().ToHtmlString() });
            }
        }

        [HttpPost]
        public ActionResult CancelMeterEvent(long id)
        {
            var meterEvent = _meterEventRepository.GetById(id);
            if (meterEvent.IsNew == true)
            {
                _meterEventRepository.DeleteAndCommit(meterEvent);
            }
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult DeleteMeterEvent(long? parentId, long id)
        {
            var meterEvent = _meterEventRepository.GetById(id);
            _meterEventRepository.DeleteAndCommit(meterEvent);
            return new NullJsonResult();
        } 

        [HttpPost]
        public ActionResult DeleteSelectedMeterEvents(long? parentId, long[] selectedIds)
        {
            foreach (var meterEventId in selectedIds)
            {
                var meterEvent = _meterEventRepository.GetById(meterEventId);
                _meterEventRepository.Delete(meterEvent);
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }
        #endregion
    }
}