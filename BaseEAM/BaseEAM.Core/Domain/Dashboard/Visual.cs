/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Visual : BaseEntity
    {
        public string Description { get; set; }
        public int? VisualType { get; set; }
        public string Query { get; set; }
        public string SortExpression { get; set; }
        public string XAxis { get; set; }
        public string YAxis { get; set; }

        private ICollection<VisualFilter> _visualFilters;
        public virtual ICollection<VisualFilter> VisualFilters
        {
            get { return _visualFilters ?? (_visualFilters = new List<VisualFilter>()); }
            protected set { _visualFilters = value; }
        }

        private ICollection<SecurityGroup> _securityGroups;
        public virtual ICollection<SecurityGroup> SecurityGroups
        {
            get { return _securityGroups ?? (_securityGroups = new List<SecurityGroup>()); }
            protected set { _securityGroups = value; }
        }
    }

    public enum VisualType
    {
        Area = 0,
        Area_Spline,
        Area_Step,
        Bar,
        Bar_Stacked,
        Donut,
        Gauge,
        Line,
        Line_Stacked,
        Pie,
        Spline,
        Step,
        Metric,
        Table
    }
}
