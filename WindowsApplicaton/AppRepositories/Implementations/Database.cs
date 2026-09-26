using System.Configuration;
using Repositories.Interfaces;
using Extensions;

namespace Repositories.Implementations
{
    public class Database : IDatabase
    {
        public string Get()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }
    }
}