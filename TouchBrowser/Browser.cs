using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Timers;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace TouchBrowser
{
    public partial class Browser : Form
    {
        public bool debug { get; set; }
        public ChromiumWebBrowser browser { get; set; }
        public string url { get; set; }
        public bool autoRefresh { get; set; }
        public System.Timers.Timer RefreshTimer { get; set; }
        public string AppPath { get; set; }
        public string LoadingPagePath { get; set; }
        public string LoadingPageURL { get; set; }

        public Browser()
        {
            InitializeComponent();
            AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LoadingPagePath = AppPath +"\\Loading\\index.html";
            LoadingPageURL = @"file:///" + LoadingPagePath.Replace("\\", "//");

            InitBrowser();
            this.DoubleBuffered = true;
            debug = false;

            RefreshTimer = new System.Timers.Timer();
            RefreshTimer.Enabled = false;
            RefreshTimer.Elapsed += new ElapsedEventHandler(BrowserCtrl_Refresh);
        }
        
        private async void Browser_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            if (debug)
            {

            }
            else
            {
                this.TopMost = true;
                this.BringToFront();
            }

            autoRefresh = true;
            url = "https://55.195.98.135";

            List<string> users = new List<string>();
            users = "svc.itrk.ngpe,svc.miltv.ngpe,svc.pectv.ngpe,svc.tsaero.ngpe,svc.tsale.ngpe,svc.tscol.ngpe,svc.tsfre1.ngpe,svc.tsfre2.ngpe,svc.tsgen2.ngpe,svc.tsgrt.ngpe,svc.tsind.ngpe,svc.tsjac.ngpe,svc.tslaf.ngpe,svc.tslex.ngpe,svc.tslib.ngpe,svc.tslin.ngpe,svc.tsmil.ngpe,svc.tsmnt.ngpe,svc.tsmoh.ngpe,svc.tspat1.ngpe,svc.tspat2.ngpe,svc.tspri.ngpe,svc.tssar.ngpe,svc.tssgt.ngpe,svc.tstre.ngpe,svc.tstru.ngpe,svc.tsval.ngpe,svc.tswas.ngpe,svc.tsyor.ngpe,svc.vitv1.ngpe,britton.scritchfield,ash.beckett,svc.tsgen2.ngpe,svc.ldrmil.ngpe".Split(',').ToList();

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            bool userAuth = false;

            try
            {
                userName = userName.Split('\\')[1];
            }
            catch (Exception) { }


            foreach (string user in users)
            {
                if (user == userName)
                {
                    switch (user)
                    {
                        case "svc.itrk.ngpe":
                            url = "https://pecnet/apps/maint/";
                            break;
                        case "svc.miltv.ngpe":
                            url = "https://55.195.98.135/?dt=tv2";
                            pictureBoxRefresh.Visible = false;
                            break;
                        case "svc.pectv.ngpe":
                            url = "https://55.195.98.135/?dt=tv";
                            pictureBoxRefresh.Visible = false;
                            break;
                        case "svc.ldrmil.ngpe":
                            url = "https://55.195.98.135/?dt=ldermil";
                            pictureBoxRefresh.Visible = false;
                            break;
                        case "svc.tsaero.ngpe":
                            autoRefresh = false;
                            url = "https://55.195.98.135/GymMedia/";
                            pictureBoxRefresh.Visible = false;
                            break;
                        case "svc.tsfre1.ngpe":
                            autoRefresh = false;
                            //url = "https://pecnet/apps/gymcheckin/";
                            //url = "https://localhost:53341/GymMgmt/GymKiosk/#_fitnesscentersignin";
                            //url = "https://pecportal/GymMgmt/GymKiosk/#_fitnesscentersignin";
                            url = "https://pecportal/T3/GymMgmt/GymKiosk";
                            pictureBoxRefresh.Visible = false;
                            break;
                        case "svc.tsfre2.ngpe":
                            autoRefresh = true;
                            url = "https://55.195.98.135/?dt=g";

                            break;
                        case "britton.scritchfield":
                            autoRefresh = true;
                            //CloseRunningApps("explorer|cmd|taskmgr|regedit|notepad|powershell|powershell_ise");

                            //url = "https://55.195.98.135/";
                            //url = "https://55.195.98.135/MoHNew/";
                            //url = "https://55.195.98.135/GymMedia/";

                            //url = "https://localhost:44341/?dt=tv#";
                            url = "https://pecportal/T3/GymMgmt/GymKiosk";
                            //url = "https://55.195.98.135/?dt=tv";
                            //url = "https://55.195.98.135/?dt=tv2";
                            //url = "https://localhost:57078/?dt=tv2";
                            pictureBoxRefresh.Visible = false;

                            //url = "https://localhost:53341/GymMgmt/GymKiosk/#_fitnesscentersignin";
                            //url = "https://pecportal/GymMgmt/GymKiosk/#_fitnesscentersignin";

                            //url = "https://55.195.98.135/?dt=ldermil";
                            //url = "https://pecnet/apps/maint/";
                            //url = "https://www.w3schools.com/html/tryit.asp?filename=tryhtml5_video";
                            //url = "file:///U:/SVN/TouchScreens/HTML5Video/test.html";

                            url = "https://pecportal/T3/GymMgmt/GymKiosk";

                            break;
                        case "svc.tsmoh.ngpe":
                            autoRefresh = true;
                            url = "https://55.195.98.135/MoHNew/";
                            break;
                        case "svc.tsgen2.ngpe":
                            autoRefresh = true;
                            url = "https://55.195.98.135/MoHNew/";
                            break;
                        default:
                            if (!user.Contains("ts"))
                            {
                                userAuth = false;
                            }
                            url = "https://55.195.98.135";
                            break;
                    }
                    userAuth = true;
                }
            }

            if (userAuth == false)
            {
            }
            if (debug == false)
            {
                CloseRunningApps("explorer|iexplore|edge|microsoftedge|firefox|chrome|cmd|taskmgr|regedit|notepad|powershell|powershell_ise");
            }
            await FinishLoadingBrowser(url);
        }
        private void BrowserCtrl_Refresh(object sender, ElapsedEventArgs e)
        {
            //Check process resource usage 
            //browser.Enabled = false;
            browser.Stop();

            foreach (Control item in browser.Controls)
            {
                item.Dispose();
            }

            browser.SetZoomLevel(0.0);
            //browser.Update();
            //browser.Enabled = true;

            //Refresh
            Navigate(url);
            //browser.Reload();
        }
        async Task FinishLoadingBrowser(string url)
        {
            await Task.Delay(5000);
            Navigate(url);
        }
        public void Navigate(string url)
        {
            browser.Load(url);
        }
        public void RenderHtml(string html)
        {
            browser.LoadHtml(html);
        }
        public void InitBrowser()
        {

            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.WebSecurity = CefState.Disabled;

            CefSettings s = new CefSettings();
            s.LogSeverity = LogSeverity.Disable;
            s.CommandLineArgsDisabled = false;
            s.CefCommandLineArgs["touch-events"] = "enabled";
            s.DisableGpuAcceleration();
            s.CachePath = "cache"; //Without this all caching is done in memory
            s.IgnoreCertificateErrors = true;
            s.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.80 Safari/537.36";

            //Cef.Initialize(new CefSettings());
            Cef.Initialize(s);
            browser = new ChromiumWebBrowser("");
            browser.Dock = DockStyle.Fill;
            browser.BackColor = Color.Black;
            browser.ForeColor = Color.White;
            browser.LoadError += Browser_PageError;
            

            browser.ConsoleMessage += browser_ConsoleMessage;

            browser.LoadingStateChanged += browser_LoadChanged;

            this.Controls.Add(browser);
            browser.SendToBack();

            string html = "<style>html, body { background-color: #000; color: #fff; }</style><H1>Loading...</H1>";
            RenderHtml(html);
        }

        private void browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        public void CloseRunningApps(string apps)
        {
            //string apps = "explorer|cmd|Taskmgr";  TouchScreen

            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    foreach (string app in apps.Split('|'))
                    {
                        try
                        {
                            if (p.ProcessName.ToLower() == app)
                            {
                                p.Kill();
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }










































        #region Actions
        private void browser_LoadChanged(object sender, LoadingStateChangedEventArgs e)
        { 
            var html = "";
            if (!e.IsLoading)
            {
                try
                {
                    //If load status changed and has finished then get html
                    browser.GetSourceAsync().ContinueWith(taskHtml =>
                    {
                        //Look for errors
                        html = taskHtml.Result;
                        if (html.Contains("<title>The resource cannot be found.</title>") || html.Contains("Runtime Error") || html.Contains("<!--Loading-->") || browser.Address.Contains("<!--Loading-->"))
                        {
                            Navigate(url);
                            if (html.Contains("<title>The resource cannot be found.</title>") || html.Contains("Runtime Error") || html.Contains("<!--Loading-->") || browser.Address.Contains("<!--Loading-->"))
                            {
                                RenderHtml("<style>html, body { background-color: #000; color: #fff; }</style><H1>Loading...</H1><!--Loading-->");
                                //Navigate(LoadingPageURL);
                            }

                                RefreshTimer.Stop();
                            RefreshTimer.Interval = 10000;
                            RefreshTimer.Start();

                        }
                        else
                        {
                            //Simulate User Interaction
                            browser.GetBrowser().GetHost().SendMouseClickEvent(0, 0, MouseButtonType.Left, false, 1, CefEventFlags.None);
                            System.Threading.Thread.Sleep(100);
                            browser.GetBrowser().GetHost().SendMouseClickEvent(0, 0, MouseButtonType.Left, true, 1, CefEventFlags.None);

                            if (autoRefresh)
                            {
                                RefreshTimer.Stop();
                                RefreshTimer.Interval = 1200000;
                                //RefreshTimer.Interval = 10000;
                                RefreshTimer.Start();
                            }
                            else
                            {
                                RefreshTimer.Stop();
                            }
                        }
                    });
                }
                catch (Exception)
                {
                    //Browser Failed
                }
            }
        }
        private void Browser_PageError(object sender, EventArgs e)
        {
            //What error does this capture
            //MessageBox.Show(e.ToString());
        }
        private void Browser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
            browser.Dispose();
        }
        private void browser_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(e.Button.ToString());
        }
        private void pictureBoxRefresh_MouseClick(object sender, MouseEventArgs e)
        {
            browser.Reload();
        }
        private void pictureBoxRefresh_DoubleClick(object sender, EventArgs e)
        {
            if (debug)
            {
                this.Close();
            }
        }
        #endregion
    }
}
