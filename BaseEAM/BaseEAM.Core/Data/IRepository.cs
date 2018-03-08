/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;
using System.Linq;

namespace BaseEAM.Core.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        /// <param name="showIsDeleted"></param>
        /// <returns></returns>
        //IQueryable<T> GetAll(bool showIsDeleted = false);
        IQueryable<T> GetAll();

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        List<T> GetByColumn(string column, object value);

        /// <summary>
		/// Attaches an entity to the context
		/// </summary>
		/// <param name="entity">The entity to attach</param>
		/// <returns>The entity</returns>
		T Attach(T entity);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(T entity);

        /// <summary>
        /// Insert entity and commit
        /// </summary>
        /// <param name="entity">Entity</param>
        void InsertAndCommit(T entity);

        /// <summary>
        /// Insert entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        void InsertAndCommit(IEnumerable<T> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// Update entity and commit
        /// </summary>
        /// <param name="entity">Entity</param>
        void UpdateAndCommit(T entity);

        /// <summary>
        /// Update entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        void UpdateAndCommit(IEnumerable<T> entities);

        /// <summary>
        /// Insert or Update
        /// </summary>
        /// <param name="entity">Entity</param>
        void Save(T entity);

        /// <summary>
        /// Insert or Update entity and commit
        /// </summary>
        /// <param name="entity">Entity</param>
        void SaveAndCommit(T entity);

        /// <summary>
        /// Insert or Update entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        void SaveAndCommit(IEnumerable<T> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Delete entity and commit
        /// </summary>
        /// <param name="entity">Entity</param>
        void DeleteAndCommit(T entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void DeleteAndCommit(IEnumerable<T> entities);

        /// <summary>
        /// Deactivate entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Deactivate(T entity);

        /// <summary>
        /// Recursively deactivates all child entities.
        /// Note: This does not deactivate the entities linked via the 
        /// one-to-one, one-to-many, or many-to-many joins.
        /// </summary>
        /// <param name="entity">Entity</param>
        void DeactivateAndCommit(T entity);

        /// <summary>
        /// Deactivate entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        void DeactivateAndCommit(IEnumerable<T> entities);

        /// <summary>
        /// Activate entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Activate(T entity);

        /// <summary>
        /// Recursively activates all child entities.
        /// Note: This does not activate the entities linked via the 
        /// one-to-one, one-to-many, or many-to-many joins.
        /// </summary>
        /// <param name="entity">Entity</param>
        void ActivateAndCommit(T entity);

        /// <summary>
        /// Activate entities and commit
        /// </summary>
        /// <param name="entities">Entities</param>
        void ActivateAndCommit(IEnumerable<T> entities);

        void SaveChanges();

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}
