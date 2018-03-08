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
    public class AttributeController : BaseController
    {
        #region Fields

        private readonly IRepository<Attribute> _attributeRepository;
        private readonly IAttributeService _attributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public AttributeController(IRepository<Attribute> attributeRepository,
            IAttributeService attributeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._attributeRepository = attributeRepository;
            this._localizationService = localizationService;
            this._attributeService = attributeService;
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
            var attributeNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "AttributeName",
                ResourceKey = "Attribute.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(attributeNameFilter);

            return model;
        }

        #endregion

        #region Attributes

        [BaseEamAuthorize(PermissionNames = "System.Attribute.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.AttributeSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.AttributeSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Attribute.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.AttributeSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.AttributeSearchModel] = model;

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

            return Json(new { Errors = ModelState.SerializeErrors() });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Attribute.Create")]
        public ActionResult Create()
        {
            var attribute = new Attribute { IsNew = true };
            _attributeRepository.InsertAndCommit(attribute);
            return Json(new { Id = attribute.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Attribute.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            this._dbContext.DeleteById<Attribute>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "System.Attribute.Create,System.Attribute.Read,System.Attribute.Update")]
        public ActionResult Edit(long id)
        {
            var attribute = _attributeRepository.GetById(id);
            var model = attribute.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Attribute.Create,System.Attribute.Update")]
        public ActionResult Edit(AttributeModel model)
        {
            var attribute = _attributeRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                if ((model.ControlType == AttributeControlType.DropDownList
                   || model.ControlType == AttributeControlType.MultiSelectList)
                   && (model.DataSource == AttributeDataSource.CSV))
                {
                    //No need to update for this case.
                }
                else
                {
                    model.CsvTextList = null;
                    model.CsvValueList = null;
                }
                attribute = model.ToEntity(attribute);
                //always set IsNew to false when saving
                attribute.IsNew = false;
               
                _attributeRepository.Update(attribute);

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
        [BaseEamAuthorize(PermissionNames = "System.Attribute.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var attribute = _attributeRepository.GetById(id);

            if (!_attributeService.IsDeactivable(attribute))
            {
                ModelState.AddModelError("Attribute", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _attributeRepository.DeactivateAndCommit(attribute);
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
        [BaseEamAuthorize(PermissionNames = "System.Attribute.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var attributes = new List<Attribute>();
            foreach (long id in selectedIds)
            {
                var attribute = _attributeRepository.GetById(id);
                if (attribute != null)
                {
                    if (!_attributeService.IsDeactivable(attribute))
                    {
                        ModelState.AddModelError("Attribute", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        attributes.Add(attribute);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var attribute in attributes)
                    _attributeRepository.Deactivate(attribute);
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
    }
}