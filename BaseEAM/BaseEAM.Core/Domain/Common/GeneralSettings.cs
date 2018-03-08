/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Configuration;

namespace BaseEAM.Core.Domain
{
    public class GeneralSettings : ISettings
    {
        public int DefaultGridPageSize { get; set; }
        public string GridPageSizes { get; set; }
        public bool EnableMiniProfiler { get; set; }
        public bool CollapseMenu { get; set; }

        public bool EnableCssBundling { get; set; }
        public bool EnableJsBundling { get; set; }

        public bool DisplayMiniProfiler { get; set; }

        public string MessageEmailSender { get; set; }
        public string MessageSmtpServer { get; set; }
        public int MessageSmtpPort { get; set; }
        public bool MessageSmtpRequiresAuthentication { get; set; }
        public string MessageSmtpServerUserName { get; set; }
        public string MessageSmtpServerPassword { get; set; }

        public bool SMSEnabled { get; set; }
        public string ClickatellApiId { get; set; }
        public string ClickatellUsername { get; set; }
        public string ClickatellPassword { get; set; }

        public int MessageNumberOfTries { get; set; }
        public bool OnlyLogMessage { get; set; }

        //Web Api
        public int ValidMinutePeriod { get; set; }
        public bool LogUnauthorized { get; set; }
        public bool NoRequestTimestampValidation { get; set; }
        public bool AllowEmptyMd5Hash { get; set; }

        public int BarcodeType { get; set; }
    }
}
