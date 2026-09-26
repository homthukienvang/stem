using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Repositories.Interfaces
{
    public interface ICommonRepository
    {
        List<T> GetListBySqlQuery<T>(string sql) where T : class;
        T GetObjectBySqlQuery<T>(string sql) where T : class;
        Response ExcuteSql(string sql);
        bool ExcuteSqlQuery(string sql, object item);
        object ExcuteSqlQueryGetValueV2(string sql, object obj);
        object ExcuteStoreGetValueV2(string storeName, object obj);

        Response ExcuteStore(string storeName, params KeyValuePair<String, Object>[] parameters);
        Response ExcuteStoreV2(string storeName, object obj);
        List<T> GetListByStore<T>(string storeName, params KeyValuePair<String, Object>[] parameters) where T : class;
        List<T> GetListByStoreV2<T>(string storeName, object obj) where T : class;
        T GetObjectByStore<T>(string storeName, KeyValuePair<string, object>[] parameters) where T : class;
        T GetObjectByStoreV2<T>(string storeName, object obj) where T : class;

        void WithTransaction(Func<Response> func);
        object GetValueByKey(string storeName, string keyName, params KeyValuePair<string, object>[] parameters);
    }
}
