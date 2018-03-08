/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;

namespace BaseEAM.Data
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields

        private readonly IDbContext _context;
        private IDbSet<T> _entities;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(IDbContext context)
        {
            this._context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <param name="showIsDeleted"></param>
        /// <returns></returns>
        //public virtual IQueryable<T> GetAll(bool showIsDeleted = false)
        //{
        //    //If suffer performance change to use this for better SQL generated.
        //    if (showIsDeleted == true)
        //    {
        //        //show all entities with IsDeleted = true or false
        //        return this.Entities;
        //    }

        //    //only show entites with IsDeleted = false
        //    return this.Entities.Where(e => e.IsDeleted == false && e.IsNew == false);
        //}

        public virtual IQueryable<T> GetAll()
        {
            //Already filter IsDeleted & IsNew by EF SoftDeleteInterceptor
            //We do a hack here, see in TFS-180
            return this.Entities.Where(e => e.Id > 0);
        }

        public virtual T Attach(T entity)
        {
            return this.Entities.Attach(entity);
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(object id)
        {
            //go through soft delete interceptor
            var entity = this.Entities.Find(id);
            if (entity == null)
            {
                //get by raw sql to by pass soft delete interceptor
                entity = this._context.GetById<T>(id, true);
                //if (entity != null)
                //{
                //    //then check if not attached => then attached to DbContext to track changes
                //    entity = this._context.AttachEntityToContext<T>(entity);
                //}
            }

            return entity;

            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            //return this.Entities.Find(id);
        }

        public virtual List<T> GetByColumn(string column, object value)
        {
            var entities = this._context.GetByColumn<T>(column, value);
            foreach(var entity in entities)
            {
                //then check if not attached => then attached to DbContext to track changes
                this._context.AttachEntityToContext<T>(entity);
            }
            return entities;
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Add(entity);

                //put this after SaveChanges to get the newly generated identity
                if (entity is IHierarchy)
                {
                    this._context.SaveChanges();

                    this.UpdateHierarchyPath(entity);

                    this._context.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Insert entity and commit
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void InsertAndCommit(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Add(entity);

                this._context.SaveChanges();

                //put this after SaveChanges to get the newly generated identity
                if (entity is IHierarchy)
                {
                    this.UpdateHierarchyPath(entity);
                    this._context.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Insert entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void InsertAndCommit(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                {
                    this.Entities.Add(entity);
                }

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                if (entity is IHierarchy)
                {
                    this.UpdateHierarchyPath(entity);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Update entity and commit
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void UpdateAndCommit(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                if (entity is IHierarchy)
                {
                    this.UpdateHierarchyPath(entity);
                }

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Update entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void UpdateAndCommit(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Insert or Update
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Save(T entity)
        {
            if (entity.Id > 0)
            {
                Update(entity);
            }
            else
            {
                Insert(entity);
            }
        }

        /// <summary>
        /// Insert or Update entity and commit
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void SaveAndCommit(T entity)
        {
            if (entity.Id > 0)
            {
                UpdateAndCommit(entity);
            }
            else
            {
                InsertAndCommit(entity);
            }
        }

        /// <summary>
        /// Insert or Update entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void SaveAndCommit(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    this.Entities.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Delete entity and commit
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void DeleteAndCommit(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);
                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                //in case we can't delete an entity because of references
                //first we will deactivate it
                //
                entity.IsDeleted = true;
                this._context.SaveChanges();

                //then throw the exception
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Delete entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void DeleteAndCommit(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                {
                    this.Entities.Remove(entity);
                }

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }


        /// <summary>
        /// Recursively deactivates all child entities.
        /// Note: This does not deactivate the entities linked via the 
        /// one-to-one, one-to-many, or many-to-many joins.
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Deactivate(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (entity is IHierarchy)
                {
                    DeactivateRecursive(entity);
                }
                else
                {
                    entity.IsDeleted = true;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Recursively deactivates all child entities.
        /// Note: This does not deactivate the entities linked via the 
        /// one-to-one, one-to-many, or many-to-many joins.
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void DeactivateAndCommit(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (entity is IHierarchy)
                {
                    DeactivateRecursive(entity);
                }
                else
                {
                    entity.IsDeleted = true;
                }

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Deactivate entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void DeactivateAndCommit(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                {
                    entity.IsDeleted = true;
                }

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Recursively activates all child entities.
        /// Note: This does not activate the entities linked via the 
        /// one-to-one, one-to-many, or many-to-many joins.
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Activate(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (entity is IHierarchy)
                {
                    ActivateRecursive(entity);
                }
                else
                {
                    entity.IsDeleted = false;
                }

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Recursively activates all child entities.
        /// Note: This does not activate the entities linked via the 
        /// one-to-one, one-to-many, or many-to-many joins.
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void ActivateAndCommit(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (entity is IHierarchy)
                {
                    ActivateRecursive(entity);
                }
                else
                {
                    entity.IsDeleted = false;
                }

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Activate entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void ActivateAndCommit(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                {
                    entity.IsDeleted = false;
                }

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual void SaveChanges()
        {
            this._context.SaveChanges();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        #endregion

        #region Utilities

        private void UpdateHierarchyPath(T entity)
        {
            if (entity is IHierarchy)
            {
                if (((IHierarchy)entity).ParentId == null)
                {
                    ((IHierarchy)entity).HierarchyIdPath = entity.Id.ToString();
                    ((IHierarchy)entity).HierarchyNamePath = entity.Name;
                }
                else
                {
                    //get parent entity
                    T parentEntity = this.GetById(((IHierarchy)entity).ParentId);

                    //update entity path
                    ((IHierarchy)entity).HierarchyIdPath = ((IHierarchy)parentEntity).HierarchyIdPath + " > " + entity.Id.ToString();
                    ((IHierarchy)entity).HierarchyNamePath = ((IHierarchy)parentEntity).HierarchyNamePath + " > " + entity.Name;

                    //update child entities recursively
                    List<T> entities = this.Entities.ToList();
                    List<T> childEntities = entities.Where(e => ((IHierarchy)e).ParentId == entity.Id).ToList();
                    foreach (T ce in childEntities)
                    {
                        UpdateHierarchyPath(ce);
                    }
                }
            }
        }

        private void DeactivateRecursive(T entity)
        {
            entity.IsDeleted = true;

            //deactive child entities recursively
            List<T> entities = this.Entities.ToList();
            List<T> childEntities = entities.Where(e => ((IHierarchy)e).ParentId == entity.Id).ToList();
            foreach (T ce in childEntities)
            {
                DeactivateRecursive(ce);
            }
        }

        private void ActivateRecursive(T entity)
        {
            entity.IsDeleted = false;

            //deactive child entities recursively
            List<T> entities = this.Entities.ToList();
            List<T> childEntities = entities.Where(e => ((IHierarchy)e).ParentId == entity.Id).ToList();
            foreach (T ce in childEntities)
            {
                ActivateRecursive(ce);
            }
        }

        #endregion
    }
}
