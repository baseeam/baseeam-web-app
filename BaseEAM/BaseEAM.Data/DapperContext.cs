/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BaseEAM.Data
{
    public class DapperContext
    {
        public DapperContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public IDbConnection GetOpenConnection()
        {
            IDbConnection connection = null;
            if (DataSettings.DataProvider == "System.Data.SqlClient")
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
            }
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
            {
                connection = new MySqlConnection(ConnectionString);
                connection.Open();
            }
            return connection;
        }

        public IEnumerable<dynamic> Query(string sql, object param = null)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.Query(sql, param);
            }
        }
    }
}
