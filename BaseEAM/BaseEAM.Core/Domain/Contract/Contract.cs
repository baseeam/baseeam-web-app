/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Contract : WorkflowBaseEntity
    {
        public int? ContractType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal? Total { get; set; }

        public long? SiteId { get; set; }
        public virtual Site Site { get; set; }

        public long? WorkCategoryId { get; set; }
        public virtual ValueItem WorkCategory { get; set; }

        public long? WorkTypeId { get; set; }
        public virtual ValueItem WorkType { get; set; }

        public long? VendorId { get; set; }
        public virtual Company Vendor { get; set; }

        public long? SupervisorId { get; set; }
        public virtual User Supervisor { get; set; }

        private ICollection<Contact> _contacts;
        public virtual ICollection<Contact> Contacts
        {
            get { return _contacts ?? (_contacts = new List<Contact>()); }
            protected set { _contacts = value; }
        }

        private ICollection<ContractTerm> _contractTerms;
        public virtual ICollection<ContractTerm> ContractTerms
        {
            get { return _contractTerms ?? (_contractTerms = new List<ContractTerm>()); }
            protected set { _contractTerms = value; }
        }

        private ICollection<ContractPriceItem> _contractPriceItems;
        public virtual ICollection<ContractPriceItem> ContractPriceItems
        {
            get { return _contractPriceItems ?? (_contractPriceItems = new List<ContractPriceItem>()); }
            protected set { _contractPriceItems = value; }
        }

        private ICollection<PreventiveMaintenance> _preventiveMaintenances;
        public virtual ICollection<PreventiveMaintenance> PreventiveMaintenances
        {
            get { return _preventiveMaintenances ?? (_preventiveMaintenances = new List<PreventiveMaintenance>()); }
            protected set { _preventiveMaintenances = value; }
        }

        private ICollection<WorkOrder> _workOrders;
        public virtual ICollection<WorkOrder> WorkOrders
        {
            get { return _workOrders ?? (_workOrders = new List<WorkOrder>()); }
            protected set { _workOrders = value; }
        }
    }

    public enum ContractType
    {
        Service = 0,
        Purchase
    }
}
