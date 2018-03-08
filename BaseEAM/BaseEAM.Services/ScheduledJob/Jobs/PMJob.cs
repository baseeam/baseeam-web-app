/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using Common.Logging;
using Quartz;
using System;
using System.Linq;

namespace BaseEAM.Services
{
    [DisallowConcurrentExecution]
    public class PMJob : IJob
    {
        private static readonly ILog s_log = LogManager.GetLogger<PMJob>();
        private readonly IRepository<PreventiveMaintenance> _pmRepository;
        private readonly IRepository<PMMeterFrequency> _pMMeterFrequencyRepository;
        private readonly IRepository<PointMeterLineItem> _pointMeterLineItemRepository;
        private readonly IRepository<MeterEventHistory> _meterEventHistoryRepository;
        private readonly IPreventiveMaintenanceService _pmService;
        private readonly IWorkOrderService _workOrderService;
        public PMJob(IRepository<PreventiveMaintenance> pmRepository,
            IRepository<PMMeterFrequency> pMMeterFrequencyRepository,
            IRepository<PointMeterLineItem> pointMeterLineItemRepository,
            IRepository<MeterEventHistory> meterEventHistoryRepository,
            IPreventiveMaintenanceService pmService,
            IWorkOrderService workOrderService)
        {
            this._pmRepository = pmRepository;
            this._pMMeterFrequencyRepository = pMMeterFrequencyRepository;
            this._pointMeterLineItemRepository = pointMeterLineItemRepository;
            this._meterEventHistoryRepository = meterEventHistoryRepository;
            this._pmService = pmService;
            this._workOrderService = workOrderService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var pmList = _pmRepository.GetAll()
                .Where(s => s.PMStatus.Name == "Active" &&
                        (s.WorkOrders.All(w => w.Assignment.Name == WorkflowStatus.Closed || w.Assignment.Name == WorkflowStatus.Cancelled)))
                .ToList();

            if (pmList.Count == 0)
                return;
            var wo = new WorkOrder();
            foreach (var pm in pmList)
            {
                #region Time-Based

                if (pm.FirstWorkExpectedStartDateTime.HasValue)
                {
                    // get the latest WO
                    var latestWO = pm.WorkOrders.OrderByDescending(w => w.ActualEndDateTime).FirstOrDefault();
                    // This is the first work
                    if (latestWO == null)
                    {
                        wo = _pmService.CreateNextWorkOrder(pm,
                            pm.FirstWorkExpectedStartDateTime.Value,
                            pm.FirstWorkDueDateTime.Value);
                        s_log.InfoFormat("Generated WO = {0} for PM = {1}", wo.Number, pm.Number);
                        // Generate WO for only one PM at a time
                        return;
                    }
                    else if ((latestWO.Assignment.Name == WorkflowStatus.Closed
                                || latestWO.Assignment.Name == WorkflowStatus.Cancelled)
                        && latestWO.DueDateTime <= pm.EndDateTime)
                    {
                        wo = _workOrderService.CreateNextWorkOrderForPM(latestWO);
                        s_log.InfoFormat("Generated WO = {0} for PM = {1}", wo.Number, pm.Number);
                        // Generate WO for only one PM at a time
                        return;
                    }
                }

                #endregion

                #region Meter-Based

                if (pm.PMMeterFrequencies.Count > 0)
                {
                    foreach (var pMMeterFrequency in pm.PMMeterFrequencies)
                    {
                        var pointMeterLineItem = _pointMeterLineItemRepository.GetAll()
                            .Where(m => pMMeterFrequency.MeterId != null && m.MeterId == pMMeterFrequency.MeterId
                                        && ((m.Point.AssetId != null && m.Point.AssetId == pMMeterFrequency.PreventiveMaintenance.AssetId)
                                        || (m.Point.LocationId != null && m.Point.LocationId == pMMeterFrequency.PreventiveMaintenance.LocationId))).FirstOrDefault();

                        var lastReadingValueRange = (int)Math.Round(pointMeterLineItem.LastReadingValue.Value / pMMeterFrequency.Frequency.Value);
                        var generatedReadingRange = (int)Math.Round(pMMeterFrequency.GeneratedReading.HasValue ? pMMeterFrequency.GeneratedReading.Value / pMMeterFrequency.Frequency.Value : 0);
                        if (pMMeterFrequency.GeneratedReading == null || (lastReadingValueRange != generatedReadingRange && pointMeterLineItem.LastReadingValue <= pMMeterFrequency.EndReading.Value))
                        {
                            wo = _pmService.CreateNextWorkOrder(pm,
                               DateTime.UtcNow,
                               DateTime.UtcNow.AddDays(1));
                            s_log.InfoFormat("Generated WO = {0} for PM = {1}", wo.Number, pm.Number);
                            pMMeterFrequency.GeneratedReading = pointMeterLineItem.LastReadingValue;
                            _pMMeterFrequencyRepository.UpdateAndCommit(pMMeterFrequency);
                            return;
                        }

                    }
                }

                #endregion

                #region Event-Based

                if (pm.MeterEvents.Count > 0)
                {
                    foreach (var meterEvent in pm.MeterEvents)
                    {
                        var meterEventHistory = _meterEventHistoryRepository.GetAll().Where(h => h.MeterEventId == meterEvent.Id && h.IsWorkOrderCreated == false).FirstOrDefault();
                        if (meterEventHistory != null)
                        {
                            wo = _pmService.CreateNextWorkOrder(pm,
                             DateTime.UtcNow,
                             DateTime.UtcNow.AddDays(1));
                            s_log.InfoFormat("Generated WO = {0} for PM = {1}", wo.Number, pm.Number);
                            meterEventHistory.IsWorkOrderCreated = true;
                            _meterEventHistoryRepository.UpdateAndCommit(meterEventHistory);
                            return;
                        }
                    }
                }

                #endregion
            }
        }
    }
}
