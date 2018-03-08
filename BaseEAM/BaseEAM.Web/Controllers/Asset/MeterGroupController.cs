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

namespace BaseEAM.Web.Controllers
{
    public class MeterGroupController : BaseController
    {
        #region Fields

        private readonly IRepository<MeterGroup> _meterGroupRepository;
        private readonly IRepository<Meter> _meterRepository;
        private readonly IRepository<MeterLineItem> _meterLineItemRepository;
        private readonly IMeterGroupService _meterGroupService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public MeterGroupController(IRepository<MeterGroup> meterGroupRepository,
            IRepository<Meter> meterRepository,
            IRepository<MeterLineItem> meterLineItemRepository,
            IMeterGroupService meterGroupService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._meterGroupRepository = meterGroupRepository;
            this._meterRepository = meterRepository;
            this._meterLineItemRepository = meterLineItemRepository;
            this._localizationService = localizationService;
            this._meterGroupService = meterGroupService;
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
            var meterGroupNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "MeterGroupName",
                ResourceKey = "MeterGroup.Name",
                DbColumn = "Id",
                Value = null,
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.DB,
                DbTable = "MeterGroup",
                DbTextColumn = "Name",
                DbValueColumn = "Id",
                IsRequiredField = false
            };
            model.Filters.Add(meterGroupNameFilter);

            return model;
        }

        #endregion

        #region MeterGroups

        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.MeterGroupSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.MeterGroupSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.MeterGroupSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.MeterGroupSearchModel] = model;

                PagedResult<MeterGroup> data = _meterGroupService.GetMeterGroups(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Create")]
        public ActionResult Create()
        {
            var meterGroup = new MeterGroup { IsNew = true };
            _meterGroupRepository.InsertAndCommit(meterGroup);
            return Json(new { Id = meterGroup.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<MeterGroup>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Create,Asset.MeterGroup.Read,Asset.MeterGroup.Update")]
        public ActionResult Edit(long id)
        {
            var meterGroup = _meterGroupRepository.GetById(id);
            var model = meterGroup.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Create,Asset.MeterGroup.Update")]
        public ActionResult Edit(MeterGroupModel model)
        {
            var meterGroup = _meterGroupRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                meterGroup = model.ToEntity(meterGroup);

                //always set IsNew to false when saving
                meterGroup.IsNew = false;
                _meterGroupRepository.Update(meterGroup);

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
        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var meterGroup = _meterGroupRepository.GetById(id);

            if (!_meterGroupService.IsDeactivable(meterGroup))
            {
                ModelState.AddModelError("MeterGroup", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _meterGroupRepository.DeactivateAndCommit(meterGroup);
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
        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var meterGroups = new List<MeterGroup>();
            foreach (long id in selectedIds)
            {
                var meterGroup = _meterGroupRepository.GetById(id);
                if (meterGroup != null)
                {
                    if (!_meterGroupService.IsDeactivable(meterGroup))
                    {
                        ModelState.AddModelError("MeterGroup", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        meterGroups.Add(meterGroup);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var meterGroup in meterGroups)
                    _meterGroupRepository.Deactivate(meterGroup);
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

        #region Meters

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Read")]
        public ActionResult MeterLineItemList(long meterGroupId, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var query = _meterLineItemRepository.GetAll().Where(m => m.MeterGroupId == meterGroupId);
            query = sort == null ? query.OrderBy(a => a.DisplayOrder) : query.Sort(sort);
            var meterLineItems = new PagedList<MeterLineItem>(query, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = meterLineItems.Select(x => x.ToModel()),
                Total = meterLineItems.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Create,Asset.Meter.Update")]
        public ActionResult AddMeterLineItems(long meterGroupId, long[] selectedIds)
        {
            var meterGroup = _meterGroupRepository.GetById(meterGroupId);
            var meterLineItems = meterGroup.MeterLineItems;
            int displayOrder = meterLineItems.Max(m => (int?)m.DisplayOrder) ?? 0;
            foreach (var id in selectedIds)
            {
                var existed = meterLineItems.Any(s => s.MeterId == id);
                if (!existed)
                {
                    var meterLineItem = new MeterLineItem();
                    meterLineItem.MeterId = id;
                    displayOrder = displayOrder + 1;
                    meterLineItem.DisplayOrder = displayOrder;
                    meterGroup.MeterLineItems.Add(meterLineItem);
                }
            }
            this._dbContext.SaveChanges();
            return new NullJsonResult();
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Asset.MeterGroup.Create,Asset.MeterGroup.Update,Asset.MeterGroup.Delete")]
        public ActionResult SaveChanges([Bind(Prefix = "updated")]List<MeterLineItemModel> updatedItems,
            [Bind(Prefix = "created")]List<MeterLineItemModel> createdItems,
            [Bind(Prefix = "deleted")]List<MeterLineItemModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //we only have update & delete
                    //Update MeterLineItems
                    if (updatedItems != null)
                    {
                        foreach (var model in updatedItems)
                        {
                            var meterLineItem = _meterLineItemRepository.GetById(model.Id);
                            if (meterLineItem != null)
                            {
                                meterLineItem.DisplayOrder = model.DisplayOrder;
                                _meterLineItemRepository.Update(meterLineItem);
                            }
                        }
                    }

                    //Delete ValueItems
                    if (deletedItems != null)
                    {
                        foreach (var model in deletedItems)
                        {
                            var meterLineItem = _meterLineItemRepository.GetById(model.Id);
                            if (meterLineItem != null)
                            {
                                _meterLineItemRepository.Deactivate(meterLineItem);
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
    }
}