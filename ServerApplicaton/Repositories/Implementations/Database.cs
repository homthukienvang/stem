
using System.Configuration;
using Extensions;
using Repositories.Interfaces;

namespace Repositories.Implementations
{
    public class Database : IDatabase
    {
        public string Get()
        {
            //return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            var server = string.Format("Server={0};", GlobalSession.DbServer);
            var database = string.Format("Database={0};", GlobalSession.DbName);
            var userId = string.Format("User Id={0};", GlobalSession.DbUser);
            var password = string.Format("Password={0};", GlobalSession.DbPassword);
            return string.Format("{0}{1}{2}{3}", server, database, userId, password);
        }
    }
}