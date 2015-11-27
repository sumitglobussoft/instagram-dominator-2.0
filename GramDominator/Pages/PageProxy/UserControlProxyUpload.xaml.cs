using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using ProxyChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GramDominator.Pages.PageProxy
{
    /// <summary>
    /// Interaction logic for UserControlProxyUpload.xaml
    /// </summary>
    public partial class UserControlProxyUpload : UserControl
    {
        public UserControlProxyUpload()
        {
            InitializeComponent();
        }

        private void Proxy_Upload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Proxy_progress.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_proxy.Text = dlg.FileName.ToString();
                    Loadproxy(dlg.FileName);
                }
                
                Proxy_progress.IsIndeterminate = false;
            }
            catch { };
        }

        public void Loadproxy(string filePath)
        {
            try
            {
                string ValidIpAddressRegex = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
                List<string> proxyList = GlobusFileHelper.ReadFile(filePath);
                List<string> getTheFinaleProxyList = proxyList.Where(e => e.StartsWith(System.Text.RegularExpressions.Regex.Match(e.ToString(), ValidIpAddressRegex).ToString())).ToList();
                ClGlobul.ProxyList = getTheFinaleProxyList;
                //getchekingproxy();
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.ProxyList.Count() + " Proxies Uploaded ]");
            }
            catch (Exception)
            {
            }
        } 



        Utils objUtils = new Utils();
        private void CheckProxy_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {

                        if (string.IsNullOrEmpty(txt_proxy.Text) && string.IsNullOrEmpty(Proxy_NoOfThreads.Text))
                        {
                            GlobusLogHelper.log.Info("Please Upload Comment Message");
                            ModernDialog.ShowMessage("Please Upload Comment Message", "Upload Message", MessageBoxButton.OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                    obj_ProxyManager.lstThreadsProxy.Clear();

                    Regex checkNo = new Regex("^[0-9]*$");

                    int processorCount = objUtils.GetProcessor();

                    int threads = 25;

                    int maxThread = 25 * processorCount;
                    try
                    {
                        try
                        {

                            ProxyManager.Nothread_Proxy = Convert.ToInt32(Proxy_NoOfThreads.Text);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Enter in Correct Format");
                            return;
                        }

                        

                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                    if (!string.IsNullOrEmpty(Proxy_NoOfThreads.Text) && checkNo.IsMatch(Proxy_NoOfThreads.Text))
                    {
                        threads = Convert.ToInt32(Proxy_NoOfThreads.Text);
                    }

                    if (threads > maxThread)
                    {
                        threads = 25;
                    }
                    obj_ProxyManager.NoOfThreadsProxy = threads;
                    Thread CommentPosterThread = new Thread(obj_ProxyManager.StartProxyChecker);
                    CommentPosterThread.Start();
                    GlobusLogHelper.log.Info("------ Proxy Proccess Started ------");
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Load Accounts !");
                    GlobusLogHelper.log.Debug("Please Load Accounts !");

                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        ProxyManager obj_ProxyManager = new ProxyManager();

        private void CheckProxy_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread objStopProxy = new Thread(stopMultiThreadProxy);
                objStopProxy.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void stopMultiThreadProxy()
        {
            try
            {
                // obj_ProxyManager.isStopLikePoster = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = ProxyManager.lstProxyThread.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        ProxyManager.lstProxyThread.Remove(item);
                    }
                    catch (Exception ex)
                    {
                        //Thread.ResetAbort();
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            GlobusLogHelper.log.Info("Process Stopped !");
            GlobusLogHelper.log.Debug("Process Stopped !");
        }

        private void CheckProxy_Clear_click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_proxy.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void btu_exportproxy_click(object sender, RoutedEventArgs e)
        {
            try
            {
               // obj_ProxyManager.exportproxy();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }
    }
}
