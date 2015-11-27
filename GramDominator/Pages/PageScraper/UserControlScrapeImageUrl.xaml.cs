using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using Scraping;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace GramDominator.Pages.PageScraper
{
    /// <summary>
    /// Interaction logic for UserControlScrapeImageUrl.xaml
    /// </summary>
    public partial class UserControlScrapeImageUrl : UserControl
    {
        public UserControlScrapeImageUrl()
        {
            InitializeComponent();
            AccountReport_ScrapeImageUrl();
        }

        private void rdo_singleUser_ImageurlScrape(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_scrapeImage_Url.Clear();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
            try
            {
                ClGlobul.ImageTagForScrap.Clear();
            }
            catch { };
            try
            {
                Browse_Username_ImageUrlScrape.Visibility = Visibility.Hidden;
            }
            catch { };
            try
            {
                txt_scrapeImage_Url.IsReadOnly = false;
            }
            catch { };
        }

        private void rdo_MultipleUser_ImageScrapeUrl(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_scrapeImage_Url.Clear();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
            try
            {
                Browse_Username_ImageUrlScrape.Visibility = Visibility.Visible;
            }
            catch { };
            try
            {
                txt_scrapeImage_Url.IsReadOnly = true;
            }
            catch { };
        }

        private void Browse_Username_ImageUrlScrape_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Scrap_imageurl_progress.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_scrapeImage_Url.Text = dlg.FileName.ToString();
                    readScrape_ImageUrl(dlg.FileName);

                }

                Scrap_imageurl_progress.IsIndeterminate = false;
            }
            catch { };
        }

        public void readScrape_ImageUrl(string commentidFilePath)
        {
            try
            {
                ClGlobul.ImageTagForScrap.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {

                    ClGlobul.ImageTagForScrap.Add(commentidlist_item);
                }
                ClGlobul.ImageTagForScrap = ClGlobul.ImageTagForScrap.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.ImageTagForScrap.Count + " UserName  Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        Utils objUtils = new Utils();
        QueryManager Qm = new QueryManager();

        private void btnMessage_ScrapeImageUrl_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {

                        if (string.IsNullOrEmpty(txt_scrapeImage_Url.Text))
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
                    obj_ScrapingManager.isStop_ScrapeImage = false;
                    obj_ScrapingManager.lstThreads_ScarpeImage.Clear();

                    Regex checkNo = new Regex("^[0-9]*$");

                    int processorCount = objUtils.GetProcessor();

                    int threads = 25;

                    int maxThread = 25 * processorCount;
                    try
                    {
                        obj_ScrapingManager.ImageScarpe = true;
                        try
                        {
                            ScrapingManager.minDelay_ScrapeImage = Convert.ToInt32(txtMessage_ScrapeImageUrl_DelayMin.Text);
                            ScrapingManager.maxDelay_ScrapeImage = Convert.ToInt32(txtMessage_ScrapeImageUrl_DelayMax.Text);
                            ScrapingManager.Number_ScrapeImage = Convert.ToInt32(txt_scrapeImageUrl_nophoto.Text);
                            ScrapingManager.Nothread_ScarpeImage = Convert.ToInt32(txtMessage_ScrapeImageUrl_NoOfThreads.Text);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Enter in Correct Formate/Fill all Field");
                            ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
                            return;
                        }

                        if (rdo_ImageUrl_SingleUser.IsChecked == true)
                        {
                            ScrapingManager.ScrapeImage_single = txt_scrapeImage_Url.Text;

                        }
                        if (rdo_ImageUrl_MultipleUser.IsChecked == true)
                        {
                            ScrapingManager.ScrapeImage_Multiple = txt_scrapeImage_Url.Text;

                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                    if (!string.IsNullOrEmpty(txtMessage_ScrapeImageUrl_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_ScrapeImageUrl_NoOfThreads.Text))
                    {
                        threads = Convert.ToInt32(txtMessage_ScrapeImageUrl_NoOfThreads.Text);
                    }

                    if (threads > maxThread)
                    {
                        threads = 25;
                    }
                    obj_ScrapingManager.NoOfThreadsLikePosterScarpeUser = threads;
                    Thread CommentPosterThread = new Thread(obj_ScrapingManager.StartLikePoster);
                    CommentPosterThread.Start();
                    GlobusLogHelper.log.Info("------ ScrapeImageUrl Proccess Started ------");
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
        ScrapingManager obj_ScrapingManager = new ScrapingManager();

        private void btnMessage_ScrapeImageUrl_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread objStopScrapeImageUrl = new Thread(stopMultiThreadScrapeImageUrl);
                objStopScrapeImageUrl.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void stopMultiThreadScrapeImageUrl()
        {
            try
            {
                obj_ScrapingManager.isStop_ScrapeImage = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = obj_ScrapingManager.lstThreadsScrapeUser.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        obj_ScrapingManager.lstThreadsScrapeUser.Remove(item);
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

        private void btnMessage_ScrapeImageUrl_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_scrapeImage_Url.Text = string.Empty;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        public void AccountReport_ScrapeImageUrl()
        {
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Account_User");
                dt.Columns.Add("UserName");
                dt.Columns.Add("DateTime");
                dt.Columns.Add("Status");

                int counter = 0;
                DataSet ds = null;
                try
                {
                    ds = Qm.SelectAccountreport("ScrapeImageUrl");
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                }
                foreach (DataRow ds_item in ds.Tables[0].Rows)
                {
                    try
                    {

                        string Account_User = ds_item.ItemArray[2].ToString();
                        string UserName = ds_item[6].ToString();
                        string DateTime = ds_item[12].ToString();
                        string Status = ds_item[7].ToString();
                        dt.Rows.Add(Account_User, UserName,DateTime,Status);


                    }
                    catch { };


                }
                DataView dv;
                this.Dispatcher.Invoke(new Action(delegate
                {
                    dtGrdScrape_ScrapeImageUrl_AccountsReport.ItemsSource = dt.DefaultView;

                }));
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void RefreshAccountreport_ScrapeImageUrl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountReport_ScrapeImageUrl();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void DeleteAccountModule_ScrapeImageUrl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = ModernDialog.ShowMessage("Are You Sure Delete Data ?? ", "Delete Detail", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        QueryExecuter.deleteQueryforAccountReport("ScrapeImageUrl");
                        IGGlobals.listAccounts.Clear();
                        AccountReport_ScrapeImageUrl();
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void ExportScrapeImageUrl_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

    }
}
