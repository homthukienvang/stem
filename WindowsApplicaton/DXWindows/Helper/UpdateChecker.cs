using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using teboweb;

namespace DXWindows.Helper
{
    class UpdateChecker
    {
        /// <summary>Get UpdateChecker and version information from specified online file - returns a List</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="versionFile">Name of the pipe| delimited version file to download</param>
        /// <param name="resourceDownloadFolder">Folder on the local machine to download the version file to</param>
        /// <param name="startLine">Line number, of the version file, to read the version information from</param>
        /// <returns>app|version|release date|url|file</returns>
        public static List<string> GetUpdateInfo(string downloadsURL, string versionFile, string resourceDownloadFolder, int startLine)
        {

            bool updateChecked = false;

            //create download folder if it does not exist
            if (!Directory.Exists(resourceDownloadFolder))
            {

                Directory.CreateDirectory(resourceDownloadFolder);

            }

            //let's try and download UpdateChecker information from the web
            updateChecked = webdata.downloadFromWeb(downloadsURL, versionFile, resourceDownloadFolder);

            //if the download of the file was successful
            if (updateChecked)
            {

                //get information out of download info file
                return PopulateInfoFromWeb(versionFile, resourceDownloadFolder, startLine);

            }
            //there is a chance that the download of the file was not successful
            else
            {

                return null;

            }

        }

        /// <summary>Starts the UpdateChecker application passing across relevant information</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="filename">Name of the file to download</param>
        /// <param name="destinationFolder">Folder on the local machine to download the file to</param>
        /// <param name="processToEnd">Name of the process to end before applying the updates</param>
        /// <param name="postProcess">Name of the process to restart</param>
        /// <param name="startupCommand">Command line to be passed to the process to restart</param>
        /// <param name="updater"></param>
        /// <returns>Void</returns>
        public static void InstallUpdateRestart(string downloadsURL, string filename, string destinationFolder, string processToEnd, string postProcess, string startupCommand, string updater)
        {

            string cmdLn = "";

            cmdLn += "|downloadFile|" + filename;
            cmdLn += "|URL|" + downloadsURL;
            cmdLn += "|destinationFolder|" + destinationFolder;
            cmdLn += "|processToEnd|" + processToEnd;
            cmdLn += "|postProcess|" + postProcess;
            cmdLn += "|command|" + @" / " + startupCommand;
            const int ERROR_CANCELLED = 1223; //The operation was canceled by the user.
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = updater;
            startInfo.Arguments = cmdLn;

            startInfo.UseShellExecute = true;
            startInfo.Verb = "runas";
            try
            {
                Process.Start(startInfo);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == ERROR_CANCELLED)
                    MessageBox.Show("Why you no select Yes?");
                else
                    throw;
            }

        }
        
        private static List<string> PopulateInfoFromWeb(string versionFile, string resourceDownloadFolder, int line)
        {

            List<string> tempList = new List<string>();
            int ln;

            ln = 0;

            foreach (string strline in File.ReadAllLines(resourceDownloadFolder + versionFile))
            {

                if (ln == line)
                {

                    string[] parts = strline.Split('|');
                    foreach (string part in parts)
                    {

                        tempList.Add(part);

                    }

                    return tempList;

                }

                ln++;
            }


            return null;

        }
    }
}

