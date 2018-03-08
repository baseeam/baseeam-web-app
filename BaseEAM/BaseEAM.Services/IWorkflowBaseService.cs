/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

using BaseEAM.Core.Domain;
using System;

namespace BaseEAM.Services
{
    public interface IWorkflowBaseService
    {
        /// <summary>
        /// Create an assignment to a list of users.
        /// </summary>
        void CreateAssignment(long entityId,
            string entityType,
            User currentUser,
            string workflowInstanceId,
            long workflowDefinitionId,
            string name,
            DateTime? expectedStartDateTime,
            DateTime? dueDateTime,
            string users,
            string triggeredAction,
            string comment,
            string messageTemplate);

        /// <summary>
        /// Create an approve assignment to a list of users.
        /// </summary>
        void CreateApproveAssignment(long entityId,
            string entityType,
            User currentUser,
            string workflowInstanceId,
            long workflowDefinitionId,
            string name,
            DateTime? expectedStartDateTime,
            DateTime? dueDateTime,
            string users,
            string triggeredAction,
            string comment,
            string messageTemplate);

        /// <summary>
        /// Send a message to a list of users.
        /// </summary>
        void SendMessage(long entityId,
            string entityType,
            string users,
            string messageTemplate);

        /// <summary>
        /// Used in SLA to get values for TrackingBaseField & TrackingField
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <param name="fieldName">a property on entity or has the format WorkflowStatus + 'DateTime'</param>
        /// <returns>DateTime value</returns>
        DateTime? GetDateTimeFromFieldName(long entityId, string entityType, string fieldName);
    }
}
