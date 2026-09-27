using System;
using System.Management;
namespace DXWindows.Helper
{
    public class MACHelper
    {
        /// <summary>
        /// Gets the mac.
        /// </summary>
        /// <returns></returns>
        //public static string GetMac()
        //{
        //    return MACAddress();
        //}

        /// <summary>
        /// Macs the address.
        /// </summary>
        /// <returns></returns>
        private static string MACAddress()
        {
            string result = "";
            var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (var o in moc)
            {
                var mo = (ManagementObject)o;
                if (mo["IPEnabled"].ToString() == "True")
                {
                    //Only get the first one
                    if (result == "")
                    {
                        try
                        {
                            result = mo["MACAddress"].ToString();
                            break;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            return result;
        }

        public static string GetHddSerialNumber()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                
                return wmi_HD.GetPropertyValue("SerialNumber").ToString();//get the serailNumber of diskdrive

            }
            return "";
        }
    }}
