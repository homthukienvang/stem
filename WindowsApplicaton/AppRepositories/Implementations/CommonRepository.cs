using System;
using System.Collections.Generic;
using System.Data; 
using System.Data.SqlServerCe;
using System.Linq;
using System.Reflection;
using System.Transactions;
using  Model;
using  Repositories.Interfaces;
using Extensions; 

namespace  Repositories.Implementations
{
    public class CommonRepository : ICommonRepository
    {
        private readonly string _connectString;
        public CommonRepository(IDatabase database)
        {
            _connectString = database.Get();
        }
        public CommonRepository()
        {
             var database = new Database();
            _connectString =  database.Get();
        }
        public CommonRepository(string _connectionString)
        {
             var database = new Database();
            _connectString = _connectionString;
        }

        public List<T> GetListBySqlQuery<T>(string sql) where T : class
        {
            List<T> items;
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(sql, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.Text;
                    var reader = command.ExecuteReader();
                    items = reader.MapToList<T>();
                    reader.Dispose();
                    reader.Close();
                }
                catch (SqlCeException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return items;
        }

        public List<T> GetListBySqlQueryV2<T>(string sql, object obj) where T : class
        {
            List<T> items;
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(sql, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.Text;
                    foreach (PropertyInfo p in obj.GetType().GetProperties())
                    {
                        var val = p.GetValue(obj, null);
                        command.Parameters.AddWithValue("@" + p.Name, (val == null || val.ToString().Length == 0) ? DBNull.Value : val);
                    }
                    var reader = command.ExecuteReader();
                    items = reader.MapToList<T>();
                    reader.Dispose();
                    reader.Close();
                }
                catch (SqlCeException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return items?? new List<T>();
        }

        public T GetObjectBySqlQuery<T>(string sql) where T : class
        {
            var items = new List<T>();
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(sql, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.Text;
                    var reader = command.ExecuteReader();
                    items = reader.MapToList<T>();

                    reader.Dispose();
                    reader.Close();
                }
                catch (SqlCeException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            if (items.Any()) return items.FirstOrDefault();
            return null;
        }

        public T GetObjectBySqlQueryV2<T>(string sql, object item) where T : class
        {
            var list = GetListBySqlQueryV2<T>(sql, item);
            if (list!=null && list.Any())
            {
                return list.FirstOrDefault();
            }
            return null;
        }

        public Response ExcuteSql(string sql)
        {
            var response = new Response
            {
                Success = true,
                Message = ""
            };
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(sql, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.Text;
                    response.RowsAffected = command.ExecuteNonQuery();
                    response.Success = true;
                }
                catch (SqlCeException exception)
                {
                    throw exception;
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return response;
        }

        public bool ExcuteSqlQuery(string sql, object obj)
        {
            int rowsAffected = 0;
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(sql, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.Text;
                    foreach (PropertyInfo p in obj.GetType().GetProperties())
                    {
                        var val = p.GetValue(obj, null);
                        command.Parameters.AddWithValue("@" + p.Name, (val == null || val.ToString().Length == 0) ? DBNull.Value : val);
                    }
                    rowsAffected = command.ExecuteNonQuery();

                }
                catch (SqlCeException exception)
                {
                    throw exception;
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return rowsAffected != 0;
        }
        public object ExcuteSqlQueryGetValueV2(string sql, object obj)
        {
            object item = null;
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(sql, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.Text;
                    foreach (PropertyInfo p in obj.GetType().GetProperties())
                    {
                        var val = p.GetValue(obj, null);
                        command.Parameters.AddWithValue("@" + p.Name, (val == null || val.ToString().Length == 0) ? DBNull.Value : val);
                    }
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        item = reader[0];
                    }
                    reader.Dispose();
                    reader.Close();
                }
                catch (SqlCeException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return item;
        }
        public object ExcuteStoreGetValueV2(string storeName, object obj)
        {
            object item = null;
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (PropertyInfo p in obj.GetType().GetProperties())
                    {
                        var val = p.GetValue(obj, null);
                        command.Parameters.AddWithValue("@" + p.Name, (val == null || val.ToString().Length == 0) ? DBNull.Value : val);
                    }
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        item = reader[0];
                    }
                    reader.Dispose();
                    reader.Close();
                }
                catch (SqlCeException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return item;
        }

        public Response ExcuteStore(string storeName, params KeyValuePair<String, Object>[] parameters)
        {
            var response = new Response
            {
                Success = true,
                Message = "",
                RowsAffected = 0
            };
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var p in parameters)
                    {
                        command.Parameters.AddWithValue("@" + p.Key, (p.Value == null || p.Value.ToString().Length == 0) ? DBNull.Value : p.Value);
                    }
                    response.RowsAffected = command.ExecuteNonQuery();
                    response.Success = true;
                }
                catch (SqlCeException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return response;
        }
        public Response ExcuteStoreV2(string storeName, object obj)
        {
            var response = new Response
            {
                Success = true,
                Message = "",
                RowsAffected = 0
            };
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (PropertyInfo p in obj.GetType().GetProperties())
                    {
                        var val = p.GetValue(obj, null);
                        command.Parameters.AddWithValue("@" + p.Name, (val == null || val.ToString().Length == 0) ? DBNull.Value : val);
                    }
                    response.RowsAffected = command.ExecuteNonQuery();
                    response.Success = true;
                }
                catch (SqlCeException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return response;
        }

        public List<T> GetListByStoreV2<T>(string storeName, object obj) where T : class
        {
            var items = new List<T>();
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (PropertyInfo p in obj.GetType().GetProperties())
                    {
                        var val = p.GetValue(obj, null);
                        command.Parameters.AddWithValue("@" + p.Name, (val == null || val.ToString().Length == 0) ? DBNull.Value : val);
                    }
                    var reader = command.ExecuteReader();
                    items = reader.MapToList<T>();
                    reader.Dispose();
                    reader.Close();
                }
                catch (SqlCeException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return items;
        }

        public List<T> GetListByStore<T>(string storeName, params KeyValuePair<String, Object>[] parameters) where T : class
        {
            var items = new List<T>();
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var p in parameters)
                    {
                        command.Parameters.AddWithValue("@" + p.Key, (p.Value == null || p.Value.ToString().Length == 0) ? DBNull.Value : p.Value);
                    }
                    var reader = command.ExecuteReader();
                    items = reader.MapToList<T>();
                    reader.Dispose();
                    reader.Close();
                }
                catch (SqlCeException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return items;
        }

        public T GetObjectByStore<T>(string storeName, KeyValuePair<string, object>[] parameters) where T : class
        {
            var items = GetListByStore<T>(storeName, parameters);
            return items.Any() ? items.FirstOrDefault() : null;
        }
        public T GetObjectByStoreV2<T>(string storeName, object obj) where T : class
        {
            var items = GetListByStoreV2<T>(storeName, obj);
            return items.Any() ? items.FirstOrDefault() : null;
        }
        public object GetValueByKey(string storeName, string keyName, params KeyValuePair<string, object>[] parameters)
        {
            object item = null;
            using (var connection = new SqlCeConnection(_connectString))
            using (var command = new SqlCeCommand(storeName, connection))
            {
                connection.Open();
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var p in parameters)
                    {
                        command.Parameters.AddWithValue(p.Key,
                            (p.Value == null || p.Value.ToString().Length == 0) ? DBNull.Value : p.Value);
                    }
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        item = reader[keyName];
                    }
                    reader.Dispose();
                    reader.Close();
                }
                catch (SqlCeException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                }
            }
            return item;
        }


        /// <summary>
        /// Gets the table schema.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private string GetTableSchema(Type type)
        {
            string name = "";
            var tableattr =
                type.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableSchemaAttribute")
                    as
                    dynamic;
            if (tableattr != null) name = tableattr.Name + ".";

            return name;
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static string GetTableName(Type type)
        {
            var tableattr =
                type.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableNameAttribute") as
                    dynamic;
            string name = tableattr != null ? tableattr.Name : type.Name;

            return name;
        }

        /// <summary>
        /// Withes the transaction.
        /// </summary>
        /// <param name="func">The function.</param>
        public void WithTransaction(Func<Response> func)
        {
            using (var tran = new TransactionScope())
            {
                func();
                tran.Complete();
            }
        }
    }
}
