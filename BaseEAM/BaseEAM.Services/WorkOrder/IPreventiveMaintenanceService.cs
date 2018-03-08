/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using System;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    public interface IPreventiveMaintenanceService : IBaseService
    {
        PagedResult<PreventiveMaintenance> GetPreventiveMaintenances(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        /// <summary>
        /// Creates a next WO for PM. This method
        /// is called by the PMJob.
        /// </summary>
        WorkOrder CreateNextWorkOrder(PreventiveMaintenance preventiveMaintenance,
            DateTime startDateTime,
            DateTime endDateTime);

        void ClosePM(PreventiveMaintenance pm);

        /// <summary>
        /// Generate PM tasks if the Asset belongs to an Asset Type
        /// that is configured with a Task Group
        /// </summary>
        void GeneratePMTasks(PreventiveMaintenance pm, long? assetId);

        void CopyAttachments(List<PMTask> from, List<WorkOrderTask> to);
    }
}
