using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace update
{
    //Procesing
    //Remove previous download file
    //Download file
    //Unzip file contents to temp folder
    //Remove files from destination folder present in temp folder
    //Move unzipped files to destination folder
    //Remove download file
    //Remove temp folder


    public partial class update : Form
    {
        bool called = true;

        private string tempDownloadFolder = "";
        private string processToEnd = "";
        private string downloadFile = "";
        private string URL = "";
        private string destinationFolder = "";
        private string updateFolder = Application.StartupPath + @"\updates\";
        private string postProcessFile = "";
        private string postProcessCommand = "";

        delegate void SetLabelCallback(Label label, string text);
        public void SetLabel(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                SetLabelCallback d = new SetLabelCallback(SetLabel);
                label.Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Text = text;
                label.Refresh();
                Invalidate();
            }
        }

        public update()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Hide();

            if (called)
            {
                WindowState = FormWindowState.Normal;
                Show();

                BackgroundWorker bw = new BackgroundWorker();

                bw.DoWork -= new DoWorkEventHandler(BackgroundWorker);
                bw.DoWork += new DoWorkEventHandler(BackgroundWorker);
                bw.WorkerSupportsCancellation = true;
                bw.RunWorkerAsync();
            }
        }

        private void BackgroundWorker(object sender, DoWorkEventArgs e)
        {
            try
            {
                PreDownload();

                if (called)
                {
                    WindowState = FormWindowState.Normal;
                    Show();
                    SetLabel(line1, "Dừng  ứng dụng " + processToEnd);
                    Thread.Sleep(1000);


                    Process[] processes = Process.GetProcesses();

                    foreach (Process process in processes)
                    {
                        if (process.ProcessName == processToEnd)
                        {
                            process.Kill();
                        }
                    }

                    WebData.BytesDownloaded += BytesDownloaded;
                    WebData.DownloadFromWeb(URL, downloadFile, tempDownloadFolder);

                    SetLabel(line1, "Giải nén gói tin...");
                    Thread.Sleep(1000);
                    UnZip(tempDownloadFolder + downloadFile, tempDownloadFolder);

                    SetLabel(line1, "Xử lý tài liệu...");
                    Thread.Sleep(1000);
                    MoveFiles();
                    WrapUp();

                    SetLabel(line1, "Khởi động lại ứng dụng ...");
                    Thread.Sleep(1000);

                    if (postProcessFile != "") PostDownload();

                }

                Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UnpackCommandline()
        {
            string cmdLn = "";
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                cmdLn += arg;
            }

            if (cmdLn.IndexOf('|') == -1)
            {
                called = false;
                Close();
            }

            string[] tmpCmd = cmdLn.Split('|');

            for (int i = 1; i < tmpCmd.GetLength(0); i++)
            {
                switch (tmpCmd[i])
                {
                    case "downloadFile":
                        downloadFile = tmpCmd[i + 1];
                        break;
                    case "URL":
                        URL = tmpCmd[i + 1];
                        break;
                    case "destinationFolder":
                        destinationFolder = tmpCmd[i + 1];
                        break;
                    case "processToEnd":
                        processToEnd = tmpCmd[i + 1];
                        break;
                    case "postProcess":
                        postProcessFile = tmpCmd[i + 1];
                        break;
                    case "command":
                        postProcessCommand += @" /" + tmpCmd[i + 1];
                        break;
                }

                i++;
            }

        }

        private void UnZip(string file, string unZipTo)
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
            }
            catch (System.Exception ex)
            {
            }
        }

        private void PreDownload()
        {
            if (!Directory.Exists(updateFolder)) Directory.CreateDirectory(updateFolder);

            tempDownloadFolder = updateFolder + DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + @"\";

            if (Directory.Exists(tempDownloadFolder))
            {
                Directory.Delete(tempDownloadFolder, true);
            }

            Directory.CreateDirectory(tempDownloadFolder);

            UnpackCommandline();

        }

        private void PostDownload()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = postProcessFile;
            startInfo.Arguments = postProcessCommand;
            Process.Start(startInfo);
        }


        private void WrapUp()
        {
            if (Directory.Exists(tempDownloadFolder))
            {
                Directory.Delete(tempDownloadFolder, true);
            }
        }

        private void MoveFiles()
        {
            DirectoryInfo source = new DirectoryInfo(tempDownloadFolder);
            DirectoryInfo target = new DirectoryInfo(destinationFolder);
            CopyAll(source, target);
        }

        private void CopyAll(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            if (!targetDir.Exists)
            {
                Directory.CreateDirectory(targetDir.Name);
                targetDir = new DirectoryInfo(targetDir.Name);
            }

            // Copy each file into the new directory.
            foreach (FileInfo fi in sourceDir.GetFiles())
            {
                if (fi.Name != downloadFile)
                    fi.CopyTo(Path.Combine(targetDir.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in sourceDir.GetDirectories())
            {
                var nextTargetSubDir = targetDir.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private void BytesDownloaded(ByteArgs e)
        {
            progressBar1.Maximum = e.Total;

            SetLabel(line1, "Đang tải gói tin nâng cấp...");
            if (progressBar1.Value + e.Downloaded <= progressBar1.Maximum)
            {
                progressBar1.Value += e.Downloaded;
            }
            else
            {
                SetLabel(line1, "Tải xong.");
            }

            progressBar1.Refresh();
            Invalidate();

        }
    }
}