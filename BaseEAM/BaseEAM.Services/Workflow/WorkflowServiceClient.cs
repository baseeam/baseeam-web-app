/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;

namespace BaseEAM.Services
{
    public class WorkflowServiceClient
    {
        private static HttpClient client = new HttpClient();
        private static string baseAddress = ConfigurationManager.AppSettings["WorkflowServiceUrl"];

        public static string StartWorkflow(long entityId, string entityType, long workflowDefinitionId, long currentUserId)
        {
            var workflowServiceParameter = new WorkflowServiceParameter
            {
                EntityId = entityId,
                EntityType = entityType,
                WorkflowDefinitionId = workflowDefinitionId,
                CurrentUserId = currentUserId
            };
            var response = client.PostAsJsonAsync(baseAddress + "api/workflow/startworkflow", workflowServiceParameter).Result;
            if (response.IsSuccessStatusCode)
            {
                string workflowInstanceId = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                if (string.IsNullOrEmpty(workflowInstanceId))
                {
                    throw new BaseEamException("There're exceptions in workflow service");
                }
                return workflowInstanceId;
            }
            else
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                throw new BaseEamException(responseBody);
            }
        }

        public static string TriggerWorkflowAction(long entityId, string entityType, long workflowDefinitionId, string workflowInstanceId, int workflowVersion, string actionName, string comment, long currentUserId)
        {
            var workflowServiceParameter = new WorkflowServiceParameter
            {
                EntityId = entityId,
                EntityType = entityType,
                WorkflowDefinitionId = workflowDefinitionId,
                WorkflowInstanceId = workflowInstanceId,
                WorkflowVersion = workflowVersion,
                ActionName = actionName,
                Comment = comment,
                CurrentUserId = currentUserId
            };
            var response = client.PostAsJsonAsync(baseAddress + "api/workflow/triggerworkflowaction", workflowServiceParameter).Result;
            if (response.IsSuccessStatusCode)
            {
                string instanceId = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                if (string.IsNullOrEmpty(instanceId))
                {
                    throw new BaseEamException("There're exceptions in workflow service");
                }
                return instanceId;
            }
            else
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                throw new BaseEamException(responseBody);
            }
        }

        public static string CancelWorkflow(long entityId, string entityType, long workflowDefinitionId, string workflowInstanceId, int workflowVersion, long currentUserId)
        {
            var workflowServiceParameter = new WorkflowServiceParameter
            {
                EntityId = entityId,
                EntityType = entityType,
                WorkflowDefinitionId = workflowDefinitionId,
                WorkflowInstanceId = workflowInstanceId,
                WorkflowVersion = workflowVersion,
                CurrentUserId = currentUserId
            };
            var response = client.PostAsJsonAsync(baseAddress + "api/workflow/cancelworkflow", workflowServiceParameter).Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                throw new BaseEamException(responseBody);
            }
        }
    }
}
