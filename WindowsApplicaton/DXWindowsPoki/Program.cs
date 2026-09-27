using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Model;

namespace DXWindows
{
    public static class Globals
    {
        // parameterless constructor required for static class
        static Globals() { } // default value

        // public get, and private set for strict access control
        public static Client Userlogin { get; private set; }

        // GlobalInt can be changed only via this method
        public static void SetUserlogin(Client user)
        {
            Userlogin = user;
        }
    }
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("vi-VN");
            // Ép TLS1.2/1.1/1.0 cho toàn bộ HttpClient/WebRequest của app, set trước mọi request HTTPS
            // (tránh lỗi "Could not create SSL/TLS secure channel" trên máy Windows không dùng protocol mặc định phù hợp)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            Application.Run(new frmLogin());
        }
    }
}