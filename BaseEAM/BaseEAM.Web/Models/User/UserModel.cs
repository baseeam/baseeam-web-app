/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(UserValidator))]
    public partial class UserModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("User.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("User.LoginName")]
        public string LoginName { get; set; }

        [BaseEamResourceDisplayName("User.LoginPassword")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }

        [BaseEamResourceDisplayName("User.LoginResetPassword")]
        public string LoginResetPassword { get; set; }

        [BaseEamResourceDisplayName("User.AddressCountry")]
        public string AddressCountry { get; set; }

        [BaseEamResourceDisplayName("User.AddressState")]
        public string AddressState { get; set; }

        [BaseEamResourceDisplayName("User.AddressCity")]
        public string AddressCity { get; set; }

        [BaseEamResourceDisplayName("User.Address")]
        public string Address { get; set; }

        [BaseEamResourceDisplayName("User.Phone")]
        public string Phone { get; set; }

        [BaseEamResourceDisplayName("User.Cellphone")]
        public string Cellphone { get; set; }

        [BaseEamResourceDisplayName("User.Email")]
        public string Email { get; set; }

        [BaseEamResourceDisplayName("User.Fax")]
        public string Fax { get; set; }

        [BaseEamResourceDisplayName("User.Active")]
        public bool Active { get; set; }

        [BaseEamResourceDisplayName("User.UserType")]
        public UserType UserType { get; set; }

        [BaseEamResourceDisplayName("User.DefaultSite")]
        public long? DefaultSiteId { get; set; }

        [BaseEamResourceDisplayName("User.TimeZone")]
        public string TimeZoneId { get; set; }

        [BaseEamResourceDisplayName("User.Language")]
        public long? LanguageId { get; set; }

        [BaseEamResourceDisplayName("User.Supervisor")]
        public long? SupervisorId { get; set; }

        [BaseEamResourceDisplayName("User.WebApiEnabled")]
        public bool WebApiEnabled { get; set; }

        [BaseEamResourceDisplayName("User.PublicKey")]
        public string PublicKey { get; set; }

        [BaseEamResourceDisplayName("User.SecretKey")]
        public string SecretKey { get; set; }

        [BaseEamResourceDisplayName("User.POApprovalLimit")]
        [UIHint("DecimalNullable")]
        public decimal? POApprovalLimit { get; set; }
    }
}