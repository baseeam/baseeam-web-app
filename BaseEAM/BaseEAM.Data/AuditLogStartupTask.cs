/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Infrastructure;
using EntityFramework.Audit;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace BaseEAM.Data
{
    /// <summary>
    /// Register AuditLog configuration when starting apps: web, api, background service
    /// </summary>
    public class AuditLogStartupTask : IStartupTask
    {
        public void Execute()
        {
            var auditConfiguration = AuditConfiguration.Default;
            //auditConfiguration.IncludeRelationships = true;
            //auditConfiguration.LoadRelationships = true;
            //auditConfiguration.DefaultAuditable = true;

            string dataConnectionString = ConfigurationManager.ConnectionStrings["BaseEAM"].ConnectionString;
            var dapperContext = new DapperContext(dataConnectionString);
            var repo = new DapperRepository<AuditEntityConfiguration>(dapperContext);
            var auditEntities = repo.GetAll().ToList();
            Assembly assembly = typeof(BaseEntity).Assembly;
            var types = assembly.GetTypes();
            foreach(var auditEntity in auditEntities)
            {
                var type = types.Where(t => t.Name == auditEntity.EntityType).FirstOrDefault();
                if(type != null)
                {
                    auditConfiguration.IsAuditable(type, true);
                    //default excluded columns
                    //auditConfiguration.IsNotAudited(type, "IsNew", true);
                    auditConfiguration.IsNotAudited(type, "CreatedUser", true);
                    auditConfiguration.IsNotAudited(type, "CreatedDateTime", true);
                    auditConfiguration.IsNotAudited(type, "ModifiedUser", true);
                    auditConfiguration.IsNotAudited(type, "ModifiedDateTime", true);
                    auditConfiguration.IsNotAudited(type, "Version", true);
                    //excluded columns from db configuration
                    if (!string.IsNullOrEmpty(auditEntity.ExcludedColumns))
                    {
                        var columns = auditEntity.ExcludedColumns.Split(',');
                        foreach(var column in columns)
                            auditConfiguration.IsNotAudited(type, column, true);
                    }
                }
            }
        }

        public int Order
        {
            get { return 100; }
        }
    }
}
