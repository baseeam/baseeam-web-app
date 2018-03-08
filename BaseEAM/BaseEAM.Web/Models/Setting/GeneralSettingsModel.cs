/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    public class GeneralSettingsModel : BaseEamModel
    {
        [BaseEamResourceDisplayName("GeneralSettings.DefaultGridPageSize")]
        public int DefaultGridPageSize { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.GridPageSizes")]
        public string GridPageSizes { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.EnableMiniProfiler")]
        public bool EnableMiniProfiler { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.CollapseMenu")]
        public bool CollapseMenu { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.MessageEmailSender")]
        public string MessageEmailSender { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.MessageSmtpServer")]
        public string MessageSmtpServer { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.MessageSmtpPort")]
        public int MessageSmtpPort { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.MessageSmtpRequiresAuthentication")]
        public bool MessageSmtpRequiresAuthentication { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.MessageSmtpServerUserName")]
        public string MessageSmtpServerUserName { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.MessageSmtpServerPassword")]
        public string MessageSmtpServerPassword { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.SMSEnabled")]
        public bool SMSEnabled { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.ClickatellApiId")]
        public string ClickatellApiId { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.ClickatellUsername")]
        public string ClickatellUsername { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.ClickatellPassword")]
        public string ClickatellPassword { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.MessageNumberOfTries")]
        [UIHint("Int32")]
        public int MessageNumberOfTries { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.OnlyLogMessage")]
        public bool OnlyLogMessage { get; set; }

        //Web Api
        [BaseEamResourceDisplayName("GeneralSettings.ValidMinutePeriod")]
        [UIHint("Int32")]
        public int ValidMinutePeriod { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.LogUnauthorized")]
        public bool LogUnauthorized { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.NoRequestTimestampValidation")]
        public bool NoRequestTimestampValidation { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.AllowEmptyMd5Hash")]
        public bool AllowEmptyMd5Hash { get; set; }

        [BaseEamResourceDisplayName("GeneralSettings.BarcodeType")]
        public BarcodeLib.TYPE BarcodeType { get; set; }
    }
}