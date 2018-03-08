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
    public class ItemController : BaseController
    {
        #region Fields

        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<UnitConversion> _unitConversionRepository;
        private readonly IUnitConversionService _unitConversionService;
        private readonly IItemService _itemService;
        private readonly IEntityAttributeService _entityAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public ItemController(IRepository<Item> itemRepository,
            IRepository<UnitConversion> unitConversionRepository,
            IUnitConversionService unitConversionService,
            IItemService itemService,
            IEntityAttributeService entityAttributeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._itemRepository = itemRepository;
            this._unitConversionRepository = unitConversionRepository;
            this._unitConversionService = unitConversionService;
            this._localizationService = localizationService;
            this._itemService = itemService;
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

        #endregion

        #region Items

        [BaseEamAuthorize(PermissionNames = "Inventory.Item.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ItemSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ItemSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Item.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ItemSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.ItemSearchModel] = model;

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

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Item.Create")]
        public ActionResult Create()
        {
            var item = new Item { IsNew = true };
            _itemRepository.InsertAndCommit(item);
            return Json(new { Id = item.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Item.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Item>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Inventory.Item.Create,Inventory.Item.Read,Inventory.Item.Update")]
        public ActionResult Edit(long id)
        {
            var item = _itemRepository.GetById(id);
            var model = item.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.Item.Create,Inventory.Item.Update")]
        public ActionResult Edit(ItemModel model)
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();

            var item = _itemRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                item = model.ToEntity(item);

                //always set IsNew to false when saving
                item.IsNew = false;
                //update attributes
                _entityAttributeService.UpdateEntityAttributes(model.Id, EntityType.Item, json);
               _itemRepository.Update(item);

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
        [BaseEamAuthorize(PermissionNames = "Inventory.Item.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var item = _itemRepository.GetById(id);

            if (!_itemService.IsDeactivable(item))
            {
                ModelState.AddModelError("Item", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _itemRepository.DeactivateAndCommit(item);
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
        [BaseEamAuthorize(PermissionNames = "Inventory.Item.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var items = new List<Item>();
            foreach (long id in selectedIds)
            {
                var item = _itemRepository.GetById(id);
                if (item != null)
                {
                    if (!_itemService.IsDeactivable(item))
                    {
                        ModelState.AddModelError("Item", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        items.Add(item);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var item in items)
                    _itemRepository.Deactivate(item);
                this._dbContext.SaveChanges();
                SuccessNotification(_localizationService.GetResource("Record.Deleted"));
                return new NullJsonResult();
            }
            else
            {
                return Json(new { Errors = ModelState.SerializeErrors() });
            }
        }

        [HttpPost]
        public ActionResult ItemInfo(long? itemId)
        {
            if (itemId == null || itemId == 0)
                return new NullJsonResult();

            var item = _itemRepository.GetById(itemId);

            var itemInfo = item.ToModel();
            itemInfo.ItemCategoryText = itemInfo.ItemCategory.ToString();

            var uoms = _unitConversionService.GetFromUOMs(item.UnitOfMeasureId.Value)
                .Select(u => new { id = u.Id.ToString(), name = u.Name })
                .ToList();
            uoms.Insert(0, new { id = item.UnitOfMeasureId.ToString(), name = item.UnitOfMeasure.Name });
            return Json(new { itemInfo = itemInfo, uoms = uoms });
        }

        #endregion
    }
}