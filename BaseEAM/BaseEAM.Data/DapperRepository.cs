/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace BaseEAM.Data
{
    public class DapperRepository<T> : IDapperRepository<T> where T : BaseEntity
    {
        #region Fields

        private readonly DapperContext _dapperContext;

        #endregion

        #region Constructor

        public DapperRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        #endregion

        #region Methods

        public IEnumerable<T> Query(string sql, object param = null)
        {
            using (var connection = _dapperContext.GetOpenConnection())
            {
                return connection.Query<T>(sql, param);
            }
        }

        public IEnumerable<T> GetAll(bool showIsDeleted = false)
        {
            using (var connection = _dapperContext.GetOpenConnection())
            {
                return connection.GetAll<T>();
            }
        }

        public T GetById(long id)
        {
            using (var connection = _dapperContext.GetOpenConnection())
            {
                return connection.Get<T>(id);
            }
        }

        public long Insert(T entity)
        {
            using (var connection = _dapperContext.GetOpenConnection())
            {
                return connection.Insert<T>(entity);
            }
        }

        public int Insert(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            using (var connection = _dapperContext.GetOpenConnection())
            {
                return connection.Update<T>(entity);
            }
        }

        public bool Update(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T entity)
        {
            using (var connection = _dapperContext.GetOpenConnection())
            {
                return connection.Delete<T>(entity);
            }
        }

        public bool Delete(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAll()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
