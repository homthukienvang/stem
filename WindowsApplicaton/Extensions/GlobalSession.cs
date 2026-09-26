using System.Configuration;
using System.Linq;
namespace Extensions
{
    public class GlobalSession
    {
        private static int _fileCount = 5;
        private static long _fileSize = 10000000;
        public static string BaseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
        public static string UpdateUrl = ConfigurationManager.AppSettings["UpdateUrl"];
        public static string Updater = ConfigurationManager.AppSettings["Updater"]??"updater";

        public static int FileCount
        {
            get
            {
                return _fileCount;
            }
        }

        public static long FileSize
        {
            get
            {
                return _fileSize;
            }
        }
    }
}