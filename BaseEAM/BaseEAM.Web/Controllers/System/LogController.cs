/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Framework.CustomField;
using BaseEAM.Web.Framework.Session;
using BaseEAM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class LogController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly HttpContextBase _httpContext;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IPermissionService _permissionService;

        public LogController(ILogger logger, IWorkContext workContext,
            HttpContextBase httpContext,
            ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
            IPermissionService permissionService)
        {
            this._logger = logger;
            this._workContext = workContext;
            this._httpContext = httpContext;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._permissionService = permissionService;
        }

        private SearchModel BuildSearchModel()
        {
            var model = new SearchModel();
            var createdOnFromFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "CreatedOnFrom",
                ResourceKey = "Log.CreatedOnFrom",
                DbColumn = "CreatedOnUtc",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(createdOnFromFilter);

            var createdOnToFilter = new FieldModel
            {
                DisplayOrder = 2,
                Name = "CreatedOnTo",
                ResourceKey = "Log.CreatedOnTo",
                DbColumn = "CreatedOnUtc",
                Value = null,
                ControlType = FieldControlType.DateTime,
                DataType = FieldDataType.DateTime,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(createdOnToFilter);

            var messageFilter = new FieldModel
            {
                DisplayOrder = 3,
                Name = "Message",
                ResourceKey = "Log.Message",
                DbColumn = "FullMessage",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(messageFilter);

            var logLevelFilter = new FieldModel
            {
                DisplayOrder = 4,
                Name = "LogLevel",
                ResourceKey = "Log.LogLevel",
                DbColumn = "LogLevelId",
                ControlType = FieldControlType.DropDownList,
                DataType = FieldDataType.Int64,
                DataSource = FieldDataSource.CSV,
                CsvTextList = "Debug,Information,Warning,Error,Fatal",
                CsvValueList = "10,20,30,40,50",
                IsRequiredField = false
            };
            model.Filters.Add(logLevelFilter);
            return model;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.LogSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.LogSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.LogSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();

            //session update
            model.Update(searchValues);
            _httpContext.Session[SessionKey.LogSearchModel] = model;

            PagedResult<Log> data = _logger.GetLogs(model.ToExpression(), model.ToParameters(), command.Page - 1, command.PageSize, sort);

            var gridModel = new DataSourceResult
            {
                Data = data.Result.Select(x => new LogModel
                {
                    Id = x.Id,
                    LogLevel = x.LogLevel.GetLocalizedEnum(_localizationService, _workContext),
                    ShortMessage = x.ShortMessage,
                    //little performance optimization: ensure that "FullMessage" is not returned
                    FullMessage = "",
                    UserId = x.UserId,
                    User = new UserModel { Email = x.User == null ? "" : x.User.Email },
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                }),
                Total = data.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult ClearAll()
        {
            _logger.ClearLog();
            SuccessNotification(_localizationService.GetResource("System.Log.Cleared"));
            return new NullJsonResult();
        }

        public ActionResult View(long id)
        {
            var log = _logger.GetLogById(id);

            var model = new LogModel
            {
                Id = log.Id,
                LogLevel = log.LogLevel.GetLocalizedEnum(_localizationService, _workContext),
                ShortMessage = log.ShortMessage,
                FullMessage = log.FullMessage,
                UserId = log.UserId,
                UserEmail = log.User == null ? "" : log.User.Email,
                CreatedOn = _dateTimeHelper.ConvertToUserTime(log.CreatedOnUtc, DateTimeKind.Utc)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(long? parentId, long id)
        {
            var log = _logger.GetLogById(id);
            _logger.DeleteLog(log);
            SuccessNotification(_localizationService.GetResource("Record.Deleted"));
            return new NullJsonResult();
        }
    }
}