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
    public class CraftController : BaseController
    {
        #region Fields

        private readonly IRepository<Craft> _craftRepository;
        private readonly ICraftService _craftService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public CraftController(IRepository<Craft> craftRepository,
            ICraftService craftService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._craftRepository = craftRepository;
            this._localizationService = localizationService;
            this._craftService = craftService;
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
            var craftNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "Craft.Name",
                DbColumn = "Name, Description",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(craftNameFilter);

            var craftOvertimeRateFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "OvertimeRate",
                ResourceKey = "Craft.OvertimeRate",
                DbColumn = "OvertimeRate",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.DecimalNullable,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(craftOvertimeRateFilter);

            var craftStandardRateFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "StandardRate",
                ResourceKey = "Craft.StandardRate",
                DbColumn = "StandardRate",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.DecimalNullable,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(craftStandardRateFilter);

            return model;
        }

        #endregion

        #region Crafts

        [BaseEamAuthorize(PermissionNames = "Resource.Craft.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.CraftSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.CraftSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Craft.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.CraftSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.CraftSearchModel] = model;

                PagedResult<Craft> data = _craftService.GetCrafts(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

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
        [BaseEamAuthorize(PermissionNames = "Resource.Craft.Create")]
        public ActionResult Create()
        {
            var craft = new Craft { IsNew = true };
            _craftRepository.InsertAndCommit(craft);
            return Json(new { Id = craft.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Craft.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Craft>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Resource.Craft.Create,Craft.Craft.Read,Craft.Craft.Update")]
        public ActionResult Edit(long id)
        {
            var craft = _craftRepository.GetById(id);
            var model = craft.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Resource.Craft.Create,Craft.Craft.Update")]
        public ActionResult Edit(CraftModel model)
        {
            var craft = _craftRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                craft = model.ToEntity(craft);

                //always set IsNew to false when saving
                craft.IsNew = false;
                //update attributes
                _craftRepository.Update(craft);

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
        [BaseEamAuthorize(PermissionNames = "Resource.Craft.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var craft = _craftRepository.GetById(id);

            if (!_craftService.IsDeactivable(craft))
            {
                ModelState.AddModelError("Craft", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _craftRepository.DeactivateAndCommit(craft);
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
        [BaseEamAuthorize(PermissionNames = "Resource.Craft.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var crafts = new List<Craft>();
            foreach (long id in selectedIds)
            {
                var craft = _craftRepository.GetById(id);
                if (craft != null)
                {
                    if (!_craftService.IsDeactivable(craft))
                    {
                        ModelState.AddModelError("Craft", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        crafts.Add(craft);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var craft in crafts)
                    _craftRepository.Deactivate(craft);
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
        public ActionResult CraftInfo(long? craftId)
        {
            if (craftId == null || craftId == 0)
                return new NullJsonResult();

            var craftInfo = _craftRepository.GetById(craftId).ToModel();
            return Json(new { craftInfo = craftInfo });
        }

        #endregion
    }
}