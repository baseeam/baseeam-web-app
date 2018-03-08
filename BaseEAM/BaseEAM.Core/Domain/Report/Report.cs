/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Report : BaseEntity
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public int? TemplateType { get; set; }
        public string TemplateFileName { get; set; }
        public byte[] TemplateFileBytes { get; set; }
        public string Query { get; set; }
        public string SortExpression { get; set; }
        public bool IncludeCurrentUserInQuery { get; set; }

        private ICollection<ReportFilter> _reportFilters;
        public virtual ICollection<ReportFilter> ReportFilters
        {
            get { return _reportFilters ?? (_reportFilters = new List<ReportFilter>()); }
            protected set { _reportFilters = value; }
        }

        private ICollection<ReportColumn> _reportColumns;
        public virtual ICollection<ReportColumn> ReportColumns
        {
            get { return _reportColumns ?? (_reportColumns = new List<ReportColumn>()); }
            protected set { _reportColumns = value; }
        }

        private ICollection<SecurityGroup> _securityGroups;
        public virtual ICollection<SecurityGroup> SecurityGroups
        {
            get { return _securityGroups ?? (_securityGroups = new List<SecurityGroup>()); }
            protected set { _securityGroups = value; }
        }
    }

    public enum ReportTemplateType
    {
        Grid = 0,
        CrystalReport
    }
}
