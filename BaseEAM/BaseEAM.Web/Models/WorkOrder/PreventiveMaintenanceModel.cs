/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(PreventiveMaintenanceValidator))]
    public class PreventiveMaintenanceModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("PreventiveMaintenance.Number")]
        public string Number { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Status")]
        public long? PMStatusId { get; set; }
        public string PMStatusName { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Priority")]
        public AssignmentPriority Priority { get; set; }

        [BaseEamResourceDisplayName("Priority")]
        public string PriorityText { get; set; }

		[BaseEamResourceDisplayName("Site")]
        public long? SiteId { get; set; }
        public string SiteName { get; set; }

        [BaseEamResourceDisplayName("Asset")]
        public long? AssetId { get; set; }
        public string AssetName { get; set; }

        [BaseEamResourceDisplayName("Location")]
        public long? LocationId { get; set; }
        public string LocationName { get; set; }

        [BaseEamResourceDisplayName("WorkCategory")]
        public long? WorkCategoryId { get; set; }
        public string WorkCategoryName { get; set; }

        [BaseEamResourceDisplayName("WorkType")]
        public long? WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }

        [BaseEamResourceDisplayName("FailureGroup")]
        public long? FailureGroupId { get; set; }
        public string FailureGroupName { get; set; }

        [BaseEamResourceDisplayName("Supervisor")]
        public long? SupervisorId { get; set; }
        public string SupervisorName { get; set; }

        [BaseEamResourceDisplayName("TaskGroup")]
        public long? TaskGroupId { get; set; }

        [BaseEamResourceDisplayName("Contract")]
        public long? ContractId { get; set; }

        //Scheduling
        [BaseEamResourceDisplayName("PreventiveMaintenance.FirstWorkExpectedStartDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? FirstWorkExpectedStartDateTime { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.FirstWorkDueDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? FirstWorkDueDateTime { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.EndDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDateTime { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.FrequencyCount")]
        [UIHint("Int32Nullable")]
        public int? FrequencyCount { get; set; }

        /// <summary>
        /// 0 - Daily
        /// 1 - Weekly
        /// 2 - Monthly
        /// 3 - Yearly
        /// </summary>
        [BaseEamResourceDisplayName("PreventiveMaintenance.FrequencyType")]
        public FrequencyType FrequencyType { get; set; }

        //For Weekly
        [BaseEamResourceDisplayName("PreventiveMaintenance.Sunday")]
        public bool Sunday { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Monday")]
        public bool Monday { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Tuesday")]
        public bool Tuesday { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Wednesday")]
        public bool Wednesday { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Thursday")]
        public bool Thursday { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Friday")]
        public bool Friday { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Saturday")]
        public bool Saturday { get; set; }

        //For Monthly
        [BaseEamResourceDisplayName("PreventiveMaintenance.Day1")]
        public bool Day1 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day2")]
        public bool Day2 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day3")]
        public bool Day3 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day4")]
        public bool Day4 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day5")]
        public bool Day5 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day6")]
        public bool Day6 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day7")]
        public bool Day7 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day8")]
        public bool Day8 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day9")]
        public bool Day9 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day10")]
        public bool Day10 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day11")]
        public bool Day11 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day12")]
        public bool Day12 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day13")]
        public bool Day13 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day14")]
        public bool Day14 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day15")]
        public bool Day15 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day16")]
        public bool Day16 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day17")]
        public bool Day17 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day18")]
        public bool Day18 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day19")]
        public bool Day19 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day20")]
        public bool Day20 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day21")]
        public bool Day21 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day22")]
        public bool Day22 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day23")]
        public bool Day23 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day24")]
        public bool Day24 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day25")]
        public bool Day25 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day26")]
        public bool Day26 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day27")]
        public bool Day27 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day28")]
        public bool Day28 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day29")]
        public bool Day29 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day30")]
        public bool Day30 { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance.Day31")]
        public bool Day31 { get; set; }

    }
}