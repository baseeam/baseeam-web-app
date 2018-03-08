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
    public class MessageController : BaseController
    {
        #region Fields

        private readonly IRepository<Message> _messageRepository;
        private readonly IMessageService _messageService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public MessageController(IRepository<Message> messageRepository,
            IMessageService messageService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._messageRepository = messageRepository;
            this._localizationService = localizationService;
            this._messageService = messageService;
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
            var sentDateTimeFromFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "SentDateTimeFrom",
                ResourceKey = "Message.SentDateTimeFrom",
                DbColumn = "Message.SentDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(sentDateTimeFromFilter);

            var sentDateTimeToFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "SentDateTimeTo",
                ResourceKey = "Message.SentDateTimeTo",
                DbColumn = "Message.SentDateTime",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(sentDateTimeToFilter);

            return model;
        }

        #endregion

        #region Messages

        [BaseEamAuthorize(PermissionNames = "System.Message.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.MessageSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.MessageSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Message.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.MessageSearchModel] as SearchModel;
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
                _httpContext.Session[SessionKey.MessageSearchModel] = model;

                PagedResult<Message> data = _messageService.GetMessages(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);
                var result = data.Result.Select(x => x.ToModel()).ToList();
                foreach (var r in result)
                {
                    r.MessageTypeText = r.MessageType.ToString();
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

        [BaseEamAuthorize(PermissionNames = "System.Message.Create,Message.Message.Read,Message.Message.Update")]
        public ActionResult Edit(long id)
        {
            var message = _messageRepository.GetById(id);
            var model = message.ToModel();
            model.MessageTypeText = model.MessageType.ToString();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.Message.Delete")]
        public ActionResult Delete(long? parentId, long id)
        {
            var message = _messageRepository.GetById(id);

            if (!_messageService.IsDeactivable(message))
            {
                ModelState.AddModelError("Message", _localizationService.GetResource("Common.NotDeactivable"));
            }

            if (ModelState.IsValid)
            {
                _messageRepository.DeactivateAndCommit(message);
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
        [BaseEamAuthorize(PermissionNames = "System.Message.Delete")]
        public ActionResult DeleteSelected(long? parentId, ICollection<long> selectedIds)
        {
            var messages = new List<Message>();
            foreach (long id in selectedIds)
            {
                var message = _messageRepository.GetById(id);
                if (message != null)
                {
                    if (!_messageService.IsDeactivable(message))
                    {
                        ModelState.AddModelError("Message", _localizationService.GetResource("Common.NotDeactivable"));
                        break;
                    }
                    else
                    {
                        messages.Add(message);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var message in messages)
                    _messageRepository.Deactivate(message);
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