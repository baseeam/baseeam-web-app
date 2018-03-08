/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using BaseEAM.Services;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Controllers;
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
    public class ScheduledJobController : BaseController
    {
        #region Fields
        
        private readonly QuartzScheduler _quartzScheduler;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public ScheduledJobController(QuartzScheduler quartzScheduler,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            HttpContextBase httpContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._quartzScheduler = quartzScheduler;
            this._localizationService = localizationService;
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

            var nameFilter = new FieldModel
            {
                DisplayOrder = 1,
                Name = "Name",
                ResourceKey = "ScheduledJob.JobName",
                DbColumn = "JobName, JobDescription",
                Value = null,
                ControlType = FieldControlType.TextBox,
                DataType = FieldDataType.String,
                DataSource = FieldDataSource.None,
                IsRequiredField = false
            };
            model.Filters.Add(nameFilter);

            return model;
        }

        #endregion

        #region ScheduledJobs

        [BaseEamAuthorize(PermissionNames = "System.ScheduledJob.Read")]
        public ActionResult List()
        {
            var model = _httpContext.Session[SessionKey.ScheduledJobSearchModel] as SearchModel;
            //If not exist, build search model
            if (model == null)
            {
                model = BuildSearchModel();
                //session save
                _httpContext.Session[SessionKey.ScheduledJobSearchModel] = model;
            }
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "System.ScheduledJob.Read")]
        public ActionResult List(DataSourceRequest command, string searchValues, IEnumerable<Sort> sort = null)
        {
            var model = _httpContext.Session[SessionKey.ScheduledJobSearchModel] as SearchModel;
            if (model == null)
                model = BuildSearchModel();
            else
                model.ClearValues();

            if (ModelState.IsValid)
            {
                //session update
                model.Update(searchValues);
                _httpContext.Session[SessionKey.ScheduledJobSearchModel] = model;

                List<ScheduledJob> jobs = _quartzScheduler.GetJobs().OrderBy(j => j.JobName).ToList();

                PagedResult<ScheduledJob> data = new PagedResult<ScheduledJob>(jobs, jobs.Count);

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

        /// <param name="id">JobName</param>
        [BaseEamAuthorize(PermissionNames = "System.ScheduledJob.Create,System.ScheduledJob.Read,System.ScheduledJob.Update")]
        public ActionResult Edit(string id)
        {
            List<ScheduledJob> jobs = _quartzScheduler.GetJobs();
            var scheduledJob = jobs.Where(j => j.JobName == id).SingleOrDefault();
            var model = scheduledJob.ToModel();
            return View(model);
        }

        //[HttpPost]
        //[BaseEamAuthorize(PermissionNames = "System.ScheduledJob.Create,System.ScheduledJob.Update")]
        //public ActionResult Edit(ScheduledJobModel model)
        //{
        //    var scheduledJob = _scheduledJobRepository.GetById(model.Id);
        //    if (ModelState.IsValid)
        //    {
        //        scheduledJob = model.ToEntity(scheduledJob);

        //        //always set IsNew to false when saving
        //        scheduledJob.IsNew = false;
        //        //update attributes
        //        _scheduledJobRepository.Update(scheduledJob);

        //        //commit all changes
        //        this._dbContext.SaveChanges();

        //        //notification
        //        SuccessNotification(_localizationService.GetResource("Record.Saved"));
        //        return new NullJsonResult();
        //    }
        //    else
        //    {
        //        return Json(new { Errors = ModelState.SerializeErrors() });
        //    }
        //}

        #endregion
    }
}