using System.Configuration;
using System.Linq;
namespace Extensions
{
    public class GlobalSession
    {
        private static string _dbServer;
        private static string _dbName;
        private static string _dbUser;
        private static string _dbPassword;
        private static int _pageSize;
        public static string DbServer
        {
            get
            {
                if (string.IsNullOrEmpty(_dbServer))
                {
                    _dbServer = ConfigurationManager.AppSettings.AllKeys.Contains("DBServer")
                        ? ConfigurationManager.AppSettings["DBServer"]
                        : "";
                }
                return _dbServer;
            }
        }
        public static string DbName
        {
            get
            {
                if (string.IsNullOrEmpty(_dbName))
                {
                    _dbName = ConfigurationManager.AppSettings.AllKeys.Contains("DBName")
                        ? ConfigurationManager.AppSettings["DBName"]
                        : "";
                }
                return _dbName;
            }
        }
        public static string DbUser
        {
            get
            {
                if (string.IsNullOrEmpty(_dbUser))
                {
                    _dbUser = ConfigurationManager.AppSettings.AllKeys.Contains("DBUser")
                        ? ConfigurationManager.AppSettings["DBUser"]
                        : "";
                }
                return _dbUser;
            }
        }
        public static string DbPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_dbPassword))
                {
                    _dbPassword = ConfigurationManager.AppSettings.AllKeys.Contains("DBPassword")
                        ? ConfigurationManager.AppSettings["DBPassword"]
                        : "";
                }
                return _dbPassword;
            }
        }

        public static string FileFolder = ConfigurationManager.AppSettings["FileFolder"];
        public static string ImageFolder = ConfigurationManager.AppSettings["ImageFolder"];

        public static int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        public static int DefaultLanguageId = int.Parse(ConfigurationManager.AppSettings["DefaultLanguageId"]);
        public static string RootUrl = ConfigurationManager.AppSettings["RootUrl"];
        public static string BaseApiUrl = ConfigurationManager.AppSettings["ApiDomain"];

        public static int RetailCategoryPortalId = 16;

    }
}