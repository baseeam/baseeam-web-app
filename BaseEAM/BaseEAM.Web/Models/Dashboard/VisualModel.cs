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
    [Validator(typeof(VisualValidator))]
    public class VisualModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Visual.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Visual.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Visual.VisualType")]
        public VisualType VisualType { get; set; }
        public string VisualTypeText { get; set; }

        [BaseEamResourceDisplayName("Visual.Query")]
        public string Query { get; set; }

        [BaseEamResourceDisplayName("Visual.SortExpression")]
        public string SortExpression { get; set; }

        [BaseEamResourceDisplayName("Visual.XAxis")]
        public string XAxis { get; set; }

        [BaseEamResourceDisplayName("Visual.YAxis")]
        public string YAxis { get; set; }
    }
}