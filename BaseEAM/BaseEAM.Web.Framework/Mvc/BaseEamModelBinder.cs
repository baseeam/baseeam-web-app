/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Infrastructure;
using BaseEAM.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace BaseEAM.Web.Framework.Mvc
{
    public class BaseEamModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            return model;
        }

        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value)
        {
            var dateTimeHelper = EngineContext.Current.Resolve<IDateTimeHelper>();
            //check if data type of value is System.String
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                //developers can mark properties to be excluded from trimming with [NoTrim] attribute
                if (propertyDescriptor.Attributes.Cast<object>().All(a => a.GetType() != typeof(NoTrimAttribute)))
                {
                    var stringValue = (string)value;
                    value = string.IsNullOrEmpty(stringValue) ? "" : stringValue.Trim();
                }
            }
            else if (propertyDescriptor.PropertyType == typeof(DateTime) && value != null)
            {
                var dateValue = (DateTime)value;
                value = dateTimeHelper.ConvertToUtcTime(dateValue, dateTimeHelper.CurrentTimeZone);
            }
            else if (propertyDescriptor.PropertyType == typeof(DateTime?) && value != null)
            {
                var dateValue = (DateTime?)value;
                if (dateValue.HasValue)
                {
                    value = dateTimeHelper.ConvertToUtcTime(dateValue.Value, dateTimeHelper.CurrentTimeZone);
                }
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}
