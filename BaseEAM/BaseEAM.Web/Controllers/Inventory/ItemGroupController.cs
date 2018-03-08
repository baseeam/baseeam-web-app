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

namespace BaseEAM.Web.Controllers
{
    public class ItemGroupController : BaseController
    {
        #region Fields
        private readonly IRepository<ItemGroup> _itemGroupRepository;
        private readonly IItemGroupService _itemGroupService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;
        #endregion

        #region Constructors

        public ItemGroupController(ILocalizationService localizationService,
            IRepository<ItemGroup> itemGroupRepository,
            IItemGroupService itemGroupService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._localizationService = localizationService;
            this._itemGroupRepository = itemGroupRepository;
            this._itemGroupService = itemGroupService;
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
            var itemGroupNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "ItemGroup.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(itemGroupNameFilter);

            return model;
        }

        #endregion

        #region Methods

        [BaseEamAuthorize(PermissionNames = "Inventory.ItemGroup.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ItemGroupSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ItemGroupSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.ItemGroup.Read")]
        public ActionResult List(string searchValues, DataSourceRequest command, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ItemGroupSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();
            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.ItemGroupSearchModel] = model;

                PagedResult<ItemGroup> itemGroups = _itemGroupService.GetItemGroups(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

                var gridModel = new DataSourceResult
                {
                    Data = itemGroups.Result.Select(x => x.ToModel()),
                    Total = itemGroups.TotalCount
                };
                return new JsonResult
                {
                    Data = gridModel
                };
            }

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Inventory.ItemGroup.Create,Inventory.ItemGroup.Update,Inventory.ItemGroup.Delete")]
        public ActionResult SaveChanges([Bind(Prefix = "updated")]List<ItemGroupModel> updatedItems,
            [Bind(Prefix = "created")]List<ItemGroupModel> createdItems,
            [Bind(Prefix = "deleted")]List<ItemGroupModel> deletedItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Create ItemGroups
                    if (createdItems != null)
                    {
                        foreach (var model in createdItems)
                        {
                            var itemGroup = new ItemGroup
                            {
                                Name = model.Name,
                                Description = model.Description,
                                IsNew = false
                            };
                            _itemGroupRepository.Insert(itemGroup);
                        }
                    }

                    //Update ItemGroups
                    if (updatedItems != null)
                    {
                        foreach (var model in updatedItems)
                        {
                            var itemGroup = _itemGroupRepository.GetById(model.Id);
                            if(itemGroup != null)
                            {
                                itemGroup.Name = model.Name;
                                itemGroup.Description = model.Description;

                                _itemGroupRepository.Update(itemGroup);
                            }
                        }
                    }

                    //Delete ItemGroups
                    if (deletedItems != null)
                    {
                        foreach (var model in deletedItems)
                        {
                            var itemGroup = _itemGroupRepository.GetById(model.Id);
                            if (itemGroup != null)
                            {
                                _itemGroupRepository.Deactivate(itemGroup);
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