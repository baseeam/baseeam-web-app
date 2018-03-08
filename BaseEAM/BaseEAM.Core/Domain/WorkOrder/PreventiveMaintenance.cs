/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class PreventiveMaintenance : BaseEntity
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }

        public long? PMStatusId { get; set; }
        public virtual ValueItem PMStatus { get; set; }

        public long? SiteId { get; set; }
        public virtual Site Site { get; set; }

        public long? AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public long? LocationId { get; set; }
        public virtual Location Location { get; set; }

        public long? WorkCategoryId { get; set; }
        public virtual ValueItem WorkCategory { get; set; }

        public long? WorkTypeId { get; set; }
        public virtual ValueItem WorkType { get; set; }

        public long? FailureGroupId { get; set; }
        public virtual Code FailureGroup { get; set; }

        public long? SupervisorId { get; set; }
        public virtual User Supervisor { get; set; }

        public long? TaskGroupId { get; set; }
        public virtual TaskGroup TaskGroup { get; set; }

        public long? ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        private ICollection<PMLabor> _pmLabors;
        public virtual ICollection<PMLabor> PMLabors
        {
            get { return _pmLabors ?? (_pmLabors = new List<PMLabor>()); }
            protected set { _pmLabors = value; }
        }

        private ICollection<PMTask> _pmTasks;
        public virtual ICollection<PMTask> PMTasks
        {
            get { return _pmTasks ?? (_pmTasks = new List<PMTask>()); }
            protected set { _pmTasks = value; }
        }

        private ICollection<PMItem> _pmItems;
        public virtual ICollection<PMItem> PMItems
        {
            get { return _pmItems ?? (_pmItems = new List<PMItem>()); }
            protected set { _pmItems = value; }
        }

        private ICollection<PMServiceItem> _pmServiceItems;
        public virtual ICollection<PMServiceItem> PMServiceItems
        {
            get { return _pmServiceItems ?? (_pmServiceItems = new List<PMServiceItem>()); }
            protected set { _pmServiceItems = value; }
        }

        private ICollection<PMMiscCost> _pmMiscCosts;
        public virtual ICollection<PMMiscCost> PMMiscCosts
        {
            get { return _pmMiscCosts ?? (_pmMiscCosts = new List<PMMiscCost>()); }
            protected set { _pmMiscCosts = value; }
        }

        private ICollection<WorkOrder> _workOrders;
        public virtual ICollection<WorkOrder> WorkOrders
        {
            get { return _workOrders ?? (_workOrders = new List<WorkOrder>()); }
            protected set { _workOrders = value; }
        }

        //Scheduling Time-based
        public DateTime? FirstWorkExpectedStartDateTime { get; set; }
        public DateTime? FirstWorkDueDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public DateTime? TempExpectedStartDateTime { get; set; }
        public DateTime? TempWorkDueDateTime { get; set; }

        public int? FrequencyCount { get; set; }

        /// <summary>
        /// 0 - Daily
        /// 1 - Weekly
        /// 2 - Monthly
        /// 3 - Yearly
        /// </summary>
        public int? FrequencyType { get; set; }

        //For Weekly
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }

        //For Monthly
        public bool Day1 { get; set; }
        public bool Day2 { get; set; }
        public bool Day3 { get; set; }
        public bool Day4 { get; set; }
        public bool Day5 { get; set; }
        public bool Day6 { get; set; }
        public bool Day7 { get; set; }
        public bool Day8 { get; set; }
        public bool Day9 { get; set; }
        public bool Day10 { get; set; }
        public bool Day11 { get; set; }
        public bool Day12 { get; set; }
        public bool Day13 { get; set; }
        public bool Day14 { get; set; }
        public bool Day15 { get; set; }
        public bool Day16 { get; set; }
        public bool Day17 { get; set; }
        public bool Day18 { get; set; }
        public bool Day19 { get; set; }
        public bool Day20 { get; set; }
        public bool Day21 { get; set; }
        public bool Day22 { get; set; }
        public bool Day23 { get; set; }
        public bool Day24 { get; set; }
        public bool Day25 { get; set; }
        public bool Day26 { get; set; }
        public bool Day27 { get; set; }
        public bool Day28 { get; set; }
        public bool Day29 { get; set; }
        public bool Day30 { get; set; }
        public bool Day31 { get; set; }

        //Scheduling Meter-based
        private ICollection<PMMeterFrequency> _pmMeterFrequencies;
        public virtual ICollection<PMMeterFrequency> PMMeterFrequencies
        {
            get { return _pmMeterFrequencies ?? (_pmMeterFrequencies = new List<PMMeterFrequency>()); }
            protected set { _pmMeterFrequencies = value; }
        }

        //Scheduling Event-based
        private ICollection<MeterEvent> _meterEvents;
        public virtual ICollection<MeterEvent> MeterEvents
        {
            get { return _meterEvents ?? (_meterEvents = new List<MeterEvent>()); }
            protected set { _meterEvents = value; }
        }
    }

    public enum FrequencyType
    {
        Daily = 0,
        Weekly,
        Monthly,
        Yearly
    }
}
