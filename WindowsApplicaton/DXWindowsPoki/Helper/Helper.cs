using Extensions;
using Ionic.Zip;
using log4net;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Principal;

namespace DXWindows.Helper
{
    public class InternetHelper
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                var p = new Ping();
                var options = new PingOptions { DontFragment = true };
                PingReply reply = p.Send("google.com", 5000);
                return reply.Status == IPStatus.Success;
            }
            catch
            {

            }
            return false;
        }
    }
    public class FileHeplper
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Tạo hash theo từng tài khoản WINDOW khác nhau
        /// </summary>
        /// <param name="originFileName"></param>
        /// <returns></returns>
        public static string GetHashByCurrentUser(string originFileName)
        {
            var sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
            return $"{sid}_{originFileName}".ToLower().MD5Hash();
        }

        public static void LockFolder(string directoryPath)
        {
            //            try
            //            {
            //                _log.Debug(directoryPath);
            //                var sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);    //using SID replacement by UserName to avoid error mapping
            //                var info = new DirectoryInfo(directoryPath);
            //                var security = info.GetAccessControl(AccessControlSections.Access);
            //                var fsa = new FileSystemAccessRule(sid, FileSystemRights.FullControl, AccessControlType.Deny);
            //                security.AddAccessRule(fsa);
            //                info.SetAccessControl(security);
            //            }
            //            catch (Exception ex)
            //            {
            //                _log.Error(directoryPath, ex);
            //            }
        }
        public static void UnlockFolder(string directoryPath)
        {
            //            try
            //            {
            //                _log.Debug(directoryPath);
            //                var sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);    //using SID replacement by UserName to avoid error mapping
            //                var info = new DirectoryInfo(directoryPath);
            //                var security = info.GetAccessControl(AccessControlSections.Access);
            //                var fsa = new FileSystemAccessRule(sid, FileSystemRights.FullControl, AccessControlType.Deny);
            //                security.RemoveAccessRule(fsa);
            //                info.SetAccessControl(security);
            //            }
            //            catch (Exception ex)
            //            {
            //                _log.Error(directoryPath, ex);
            //            }
        }

        public static void Lock(string filePath)
        {
            //            if (!string.IsNullOrWhiteSpace(filePath))
            //            {
            //                try
            //                {
            //                    //                    string adminUserName = Environment.UserName;// getting your adminUserName
            //                    var sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);    //using SID replacement by UserName to avoid error mapping
            //                    File.SetAttributes(filePath, FileAttributes.Hidden);
            //
            //                    FileSecurity ds = File.GetAccessControl(filePath);
            //                    FileSystemAccessRule fsa = new FileSystemAccessRule(sid, FileSystemRights.FullControl, AccessControlType.Deny);
            //                    ds.AddAccessRule(fsa);
            //                    File.SetAccessControl(filePath, ds);
            //                }
            //
            //                catch (Exception ex)
            //                {
            //                    _log.Error(filePath, ex);
            //                }
            //            }
        }

        public static void Unlock(string filePath)
        {
            //            if (!string.IsNullOrWhiteSpace(filePath))
            //            {
            //                try
            //                {
            //                    //                    string adminUserName = Environment.UserName;// getting your adminUserName
            //                    var sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);    //using SID replacement by UserName to avoid error mapping
            //                    FileSecurity ds = File.GetAccessControl(filePath);
            //                    FileSystemAccessRule fsa = new FileSystemAccessRule(sid, FileSystemRights.FullControl, AccessControlType.Deny);
            //                    ds.RemoveAccessRule(fsa);
            //                    File.SetAccessControl(filePath, ds);
            //                }
            //                catch (Exception ex)
            //                {
            //                    _log.Error(filePath, ex);
            //                }
            //            }
        }

        /// <summary>
        /// Using before DELETE folder
        /// </summary>
        /// <param name="dir"></param>
        public static void SetFolderNormal(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
                SetFolderNormal(subDir);

            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
        }

        public static void SetFileNormal(FileInfo f)
        {
            f.Attributes = FileAttributes.Normal;
        }

        public static bool UnZip(string file, string unZipTo)//, bool deleteZipOnCompletion)
        {
            try
            {

                // Specifying Console.Out here causes diagnostic msgs to be sent to the Console
                // In a WinForms or WPF or Web app, you could specify nothing, or an alternate
                // TextWriter to capture diagnostic messages. 

                using (ZipFile zip = ZipFile.Read(file))
                {
                    // This call to ExtractAll() assumes:
                    //   - none of the entries are password-protected.
                    //   - want to extract all entries to current working directory
                    //   - none of the files in the zip already exist in the directory;
                    //     if they do, the method will throw.
                    zip.ExtractAll(unZipTo);
                }

                //if (deleteZipOnCompletion) File.Delete(unZipTo + file);

            }
            catch (System.Exception ex)
            {
                _log.Error("unzip " + file + " to " + unZipTo, ex);
                return false;
            }

            return true;
        }
    }
}