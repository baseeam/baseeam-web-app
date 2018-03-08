/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Extensions;
using BaseEAM.Web.Framework.Controllers;
using BaseEAM.Web.Framework.Filters;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Models;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers
{
    public class SettingController : BaseController
    {
        #region Fields

        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructors

        public SettingController(ISettingService settingService,
            ILocalizationService localizationService)
        {
            this._settingService = settingService;
            this._localizationService = localizationService;
        }

        #endregion

        #region Methods

        [BaseEamAuthorize(PermissionNames = "Setting.GeneralSettings.Read")]
        public ActionResult General()
        {
            var generalSettings = _settingService.LoadSetting<GeneralSettings>();
            var model = generalSettings.ToModel();
            return View(model);
        }

        [HttpPost]
        [BaseEamAuthorize(PermissionNames = "Setting.GeneralSettings.Create,Setting.GeneralSettings.Update")]
        public ActionResult General(GeneralSettingsModel model)
        {
            var generalSettings = model.ToEntity();
            _settingService.SaveSetting(generalSettings);
            SuccessNotification(_localizationService.GetResource("Record.Saved"));
            return new NullJsonResult();
        }

        #endregion
    }
}