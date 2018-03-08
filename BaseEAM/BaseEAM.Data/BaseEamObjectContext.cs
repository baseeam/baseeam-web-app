/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Timing;
using BaseEAM.Data.Mapping;
using EntityFramework.Audit;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

namespace BaseEAM.Data
{
    /// <summary>
    /// Object context
    /// </summary>
    public class BaseEamObjectContext : DbContext, IDbContext
    {
        #region Ctor

        //This is used for EF migrations
        //TODO - get connection strings dynamically and setup for deploying on production
        public BaseEamObjectContext()
            : base("BaseEAM")

        {
        }

        public BaseEamObjectContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        #endregion

        #region Utilities

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //dynamically load all configuration
            //System.Type configType = typeof(LanguageMap);   //any of your configuration classes here
            //var typesToRegister = Assembly.GetAssembly(configType).GetTypes()

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.IsAbstract != true && type.BaseType != null && type.BaseType.IsGenericType &&
                (type.BaseType.GetGenericTypeDefinition() == typeof(BaseEamEntityTypeConfiguration<>)
                    || type.BaseType.GetGenericTypeDefinition() == typeof(BaseEamWorkflowEntityTypeConfiguration<>)));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new LanguageMap());

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        public virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }

            //entity is already loaded
            return alreadyAttached;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create database script
        /// </summary>
        /// <returns>SQL to generate database</returns>
        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public override int SaveChanges()
        {
            User currentUser;
            var audit = this.BeginAudit();

            //in case of Workflow Context
            if (WorkContext == null || WorkContext.CurrentUser == null)
                currentUser = WorkflowContext.CurrentUser;
            else
                currentUser = WorkContext.CurrentUser;

            audit.CurrentUser = currentUser;
            int result;
            try
            {
                result = base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                var fail = new BaseEamException("Unable to save changes. The record you attempted to edit was modified by another user after you got the original value.");
                throw fail;
            }

            var auditEntities = CreateAuditTrail(audit, currentUser);

            return result;
        }

        /// <summary>
        /// Load reference navigation property
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="navigationProperty"></param>
        public void LoadReference<TEntity>(TEntity entity, string navigationProperty) where TEntity : BaseEntity
        {
            base.Entry<TEntity>(entity).Reference(navigationProperty).Load();
        }

        public void SetModified<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            base.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Get by Id by executing raw SQL query
        /// This can be used to by pass SoftDeleteInterceptor
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="useRawSql"></param>
        /// <returns></returns>
        public TEntity GetById<TEntity>(object id, bool useRawSql) where TEntity : BaseEntity
        {
            var result = default(TEntity);
            if (useRawSql == true)
            {
                var type = typeof(TEntity).Name;
                var sql = string.Format(SqlTemplate.GetById(), type, id.ToString());
                result = this.SqlQuery<TEntity>(sql).FirstOrDefault();
                if (result != null)
                {
                    //then check if not attached => then attached to DbContext to track changes
                    result = this.AttachEntityToContext<TEntity>(result);
                }
            }
            else
            {
                result = this.Set<TEntity>().Find(id);
            }
            return result;
        }

        public object GetByEntityIdAndType(long entityId, string entityType)
        {
            //get Type of entity
            Assembly asm = typeof(WorkflowBaseEntity).Assembly;
            Type type = asm.GetType("BaseEAM.Core.Domain." + entityType);

            //user reflection to call generic method
            MethodInfo method = typeof(BaseEamObjectContext).GetMethod("GetById");
            MethodInfo genericMethod = method.MakeGenericMethod(type);
            object entity = genericMethod.Invoke(this, new object[] { entityId, true });

            return entity;
        }

        /// <summary>
        /// Get by column by executing raw SQL query
        /// This can be used to by pass SoftDeleteInterceptor
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<TEntity> GetByColumn<TEntity>(string column, object value) where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            var sql = string.Format(SqlTemplate.GetByColumn(), type, column, value.ToString());
            var result = this.SqlQuery<List<TEntity>>(sql).FirstOrDefault();
            return result;
        }

        public void DeleteById<TEntity>(object id) where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            var sql = string.Format(SqlTemplate.DeleteById(), type, id.ToString());
            this.ExecuteSqlCommand(sql);
            //Also delete ActivityLog
            sql = string.Format(SqlTemplate.DeleteActivityLog(), type, id.ToString());
            this.ExecuteSqlCommand(sql);
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            //performance hack applied 
            bool acd = this.Configuration.AutoDetectChangesEnabled;
            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }

        /// <summary>
        /// Detach an entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        #endregion

        #region Properties

        public IWorkContext WorkContext
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether proxy creation setting is enabled (used in EF)
        /// </summary>
        public virtual bool ProxyCreationEnabled
        {
            get
            {
                return this.Configuration.ProxyCreationEnabled;
            }
            set
            {
                this.Configuration.ProxyCreationEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether auto detect changes setting is enabled (used in EF)
        /// </summary>
        public virtual bool AutoDetectChangesEnabled
        {
            get
            {
                return this.Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                this.Configuration.AutoDetectChangesEnabled = value;
            }
        }

        #endregion

        #region Helpers

        private List<AuditEntity> CreateAuditTrail(AuditLogger audit, User currentUser)
        {
            var log = audit.LastLog;
            //remove entities don't have properties changed
            log.Entities = log.Entities.Where(e => e.Properties.Count > 0).ToList();
            if (log.Entities.Count > 0)
            {
                //performance hack applied 
                bool acd = this.Configuration.AutoDetectChangesEnabled;
                bool vos = this.Configuration.ValidateOnSaveEnabled;
                this.Configuration.AutoDetectChangesEnabled = false;
                this.Configuration.ValidateOnSaveEnabled = false;

                this.Set<AuditTrail>().Add(new AuditTrail
                {
                    CreatedDateTime = Clock.Now,
                    ModifiedDateTime = Clock.Now,
                    CreatedUser = currentUser.Name,
                    ModifiedUser = currentUser.Name,
                    Date = log.Date,
                    LogXml = log.ToXml()
                });
                base.SaveChanges();

                this.Configuration.AutoDetectChangesEnabled = acd;
                this.Configuration.ValidateOnSaveEnabled = vos;
            }
            return log.Entities;
        }

        private long ValidateACExpression(AuditEntity auditEntity, string expression)
        {
            long result = 0;
            // get key
            long id = (long)auditEntity.Keys.SingleOrDefault(k => k.Name == "Id").Value;
            BaseEntity entity = this.GetByEntityIdAndType(id, auditEntity.EntityType.Name) as BaseEntity;
            bool isMatch = FleeExpression.Evaluate<bool>(entity, expression);
            if (isMatch == true)
                result = id;
            return result;
        }

        #endregion
    }
}
