using System;
using System.Management;
using System.Reflection;
using DeviceId;
using log4net;

namespace DXWindows.Helper
{
    public class DISKHelper
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// using Win32_DiskDrive class (không tối ưu)
        /// </summary>
        /// <returns></returns>
        public static string GetDisk()
        {
            try
            {
                string pcName = System.Environment.MachineName;
                var macIp = (Signature().Replace(" ", "") + "|" + pcName).ToUpper();
                return macIp;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// (recommended)
        /// https://github.com/MatthewKing/DeviceId
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceId()
        {
            try
            {
                var deviceId = new DeviceIdBuilder()
                .AddMachineName()
                .AddOsVersion()
                .OnWindows(windows => windows
                    .AddProcessorId()
                    .AddMotherboardSerialNumber()
                    .AddSystemDriveSerialNumber()).ToString();
                return deviceId;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return null;
        }

        static string GetCollection(string propName)
        {
            try
            {
                var mc = new ManagementClass("Win32_DiskDrive");
                var moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    var obj = mo[propName];
                    if (obj != null)
                        return obj.ToString();
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return "";
        }
        /// <summary>
        /// Models this instance.
        /// </summary>
        /// <returns></returns>
        private static string Model()
        {
            return GetCollection("Model");
        }

        /// <summary>
        /// Manufacturers this instance.
        /// </summary>
        /// <returns></returns>
        private static string Manufacturer()
        {
            return GetCollection("Manufacturer");
        }

        /// <summary>
        /// Signatures this instance.
        /// </summary>
        /// <returns></returns>
        private static string Signature()
        {
            return GetCollection("Signature");
        }

        /// <summary>
        /// Totals the heads.
        /// </summary>
        /// <returns></returns>
        private static string TotalHeads()
        {
            return GetCollection("TotalHeads");
        }
    }
}
