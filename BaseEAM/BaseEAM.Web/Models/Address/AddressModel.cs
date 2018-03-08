/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.Text;
using System.Web;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(AddressValidator))]
    public class AddressModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Common.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Address.Country")]
        public string Country { get; set; }

        [BaseEamResourceDisplayName("Address.StateProvince")]
        public string StateProvince { get; set; }

        [BaseEamResourceDisplayName("Address.City")]
        public string City { get; set; }

        [BaseEamResourceDisplayName("Address.Address1")]
        public string Address1 { get; set; }

        [BaseEamResourceDisplayName("Address.Address2")]
        public string Address2 { get; set; }

        [BaseEamResourceDisplayName("Address.ZipPostalCode")]
        public string ZipPostalCode { get; set; }

        [BaseEamResourceDisplayName("Address.PhoneNumber")]
        public string PhoneNumber { get; set; }

        [BaseEamResourceDisplayName("Address.FaxNumber")]
        public string FaxNumber { get; set; }

        [BaseEamResourceDisplayName("Address.Email")]
        public string Email { get; set; }

        [BaseEamResourceDisplayName("Address")]
        public string AddressHtml
        {
            get
            {
                var addressHtmlSb = new StringBuilder("<div>");
                if (!String.IsNullOrEmpty(this.Address1))
                    addressHtmlSb.AppendFormat("{0}<br />", HttpContext.Current.Server.HtmlEncode(this.Address1));
                if (!String.IsNullOrEmpty(this.Address2))
                    addressHtmlSb.AppendFormat("{0}<br />", HttpContext.Current.Server.HtmlEncode(this.Address2));
                if (!String.IsNullOrEmpty(this.City))
                    addressHtmlSb.AppendFormat("{0},", HttpContext.Current.Server.HtmlEncode(this.City));
                if (!String.IsNullOrEmpty(this.StateProvince))
                    addressHtmlSb.AppendFormat("{0},", HttpContext.Current.Server.HtmlEncode(this.StateProvince));
                if (!String.IsNullOrEmpty(this.ZipPostalCode))
                    addressHtmlSb.AppendFormat("{0}<br />", HttpContext.Current.Server.HtmlEncode(this.ZipPostalCode));
                if (!String.IsNullOrEmpty(this.Country))
                    addressHtmlSb.AppendFormat("{0}", HttpContext.Current.Server.HtmlEncode(this.Country));
                return addressHtmlSb.ToString();
            }
        }
    }
}