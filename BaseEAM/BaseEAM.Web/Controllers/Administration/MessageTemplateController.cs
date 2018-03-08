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
    public class MessageTemplateController : BaseController
    {
        #region Fields

        private readonly IRepository<MessageTemplate> _messageTemplateRepository;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public MessageTemplateController(IRepository<MessageTemplate> messageTemplateRepository,
            IRepository<Address> addressRepository,
            IRepository<Site> siteRepository,
            IMessageTemplateService messageTemplateService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._messageTemplateRepository = messageTemplateRepository;
            this._localizationService = localizationService;
            this._messageTemplateService = messageTemplateService;
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
            var messageTemplateNameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "MessageTemplate.Name",
                DbColumn = "Name",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(messageTemplateNameFilter);

            var messageTemplateEntityTypeFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "EntityType",
                ResourceKey = "EntityType",
                DbColumn = "EntityType",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.MVC,
                MvcController = "Common",
                MvcAction = "Entities",
                AutoBind = false
            };
            model.Filters.Add(messageTemplateEntityTypeFilter);

            var messageTemplateWhereUsedFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "WhereUsed",
                ResourceKey = "MessageTemplate.WhereUsed",
                DbColumn = "WhereUsed",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "All,WorkOrder",
                CsvValueList = "0,1",
                IsRequiredField = false
            };
            model.Filters.Add(messageTemplateWhereUsedFilter);

            return model;
        }

        #endregion

        #region MessageTemplates

        [BaseEamAuthorize(PermissionNames = "Administration.MessageTemplate.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.MessageTemplateSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.MessageTemplateSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.MessageTemplate.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.MessageTemplateSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.MessageTemplateSearchModel] = model;

                PagedResult<MessageTemplate> data = _messageTemplateService.GetMessageTemplates(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var r in result)
                {
                    r.WhereUsedText = r.WhereUsed.ToString();
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
        [BaseEamAuthorize(PermissionNames = "Administration.MessageTemplate.Create")]
        public ActionResult Create()
        {
            var messageTemplate = new MessageTemplate { IsNew = true };
            _messageTemplateRepository.InsertAndCommit(messageTemplate);
            return Json(new { Id = messageTemplate.Id });
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.MessageTemplate.Create")]
        public ActionResult Cancel(long? parentId, long id)
        {
            //hard delete
            this._dbContext.DeleteById<MessageTemplate>(id);
            return new NullJsonResult();
        }

        [BaseEamAuthorize(PermissionNames = "Administration.MessageTemplate.Create,Administration.MessageTemplate.Read,Administration.MessageTemplate.Update")]
        public ActionResult Edit(long id)
        {
            var messageTemplate = _messageTemplateRepository.GetById(id);
            var model = messageTemplate.ToModel();

            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Administration.MessageTemplate.Create,Administration.MessageTemplate.Update")]
        public ActionResult Edit(MessageTemplateModel model)
        {
            var messageTemplate = _messageTemplateRepository.GetById(model.Id);
            if (ModelState.IsValid)
            {
                messageTemplate = model.ToEntity(messageTemplate);
                //always set IsNew to false when saving
                messageTemplate.IsNew = false;
                _messageTemplateRepository.Update(messageTemplate);

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
        [BaseEamAuthorize(PermissionNames = "Administration.MessageTemplate.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var messageTemplate = _messageTemplateRepository.GetById(id);

            if (!_messageTemplateService.IsDeactivable(messageTemplate))
            {
                ModelState.AddModelError("MessageTemplate", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                //soft delete
                _messageTemplateRepository.DeactivateAndCommit(messageTemplate);
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
        [BaseEamAuthorize(PermissionNames = "Administration.MessageTemplate.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var messageTemplates = new List<MessageTemplate>();
            foreach (long id in selectedIds)
            {
                var messageTemplate = _messageTemplateRepository.GetById(id);
                if (messageTemplate != null)
                {
                    if (!_messageTemplateService.IsDeactivable(messageTemplate))
                    {
                        ModelState.AddModelError("MessageTemplate", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        messageTemplates.Add(messageTemplate);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var messageTemplate in messageTemplates)
                    _messageTemplateRepository.Deactivate(messageTemplate);
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