/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using System;
using System.Web.Mvc;

namespace BaseEAM.Web.Models
{
    public class LogModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Log.LogLevel")]
        public string LogLevel { get; set; }

        [BaseEamResourceDisplayName("Log.ShortMessage")]
        [AllowHtml]
        public string ShortMessage { get; set; }

        [BaseEamResourceDisplayName("Log.FullMessage")]
        [AllowHtml]
        public string FullMessage { get; set; }

        [BaseEamResourceDisplayName("Log.User")]
        public long? UserId { get; set; }

        public UserModel User { get; set; }

        public string UserEmail { get; set; }

        [BaseEamResourceDisplayName("Log.CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}