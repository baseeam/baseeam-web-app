/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(LoginValidator))]
    public partial class LoginModel : BaseEamModel
    {
        [BaseEamResourceDisplayName("User.LoginName")]
        public string LoginName { get; set; }

        [DataType(DataType.Password)]
        [BaseEamResourceDisplayName("User.LoginPassword")]
        public string LoginPassword { get; set; }
    }
}