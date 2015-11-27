using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
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
    /// Interaction logic for UserControlScrapefollower.xaml
    /// </summary>
    public partial class UserControlScrapefollower : UserControl
    {

        public  static bool ifchangeRequired =false;
        public UserControlScrapefollower()
        {
            InitializeComponent();
            BindAccount();
            ScrapingManager.objaddComboDelegate = new addComboDelegate(AddtoComboboc);
            AddtoComboboc();
            ifchangeRequired = true;
        }


        private void BindAccount()
        {
            try
            {
                Select_To_Account.Items.Clear();
                if (IGGlobals.listAccounts.Count > 0)
                {
                    foreach (var item in IGGlobals.listAccounts)
                    {
                        Select_To_Account.Items.Add(item.Split(':')[0]);
                    }

                }
                else
                {
                    //
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

        }

        Utils objUtils = new Utils();
        private void btnMessage_Scrapefollower_Start_Click(object sender, RoutedEventArgs e)
        {
            if (IGGlobals.listAccounts.Count > 0)
            {
                try
                {
                    if (string.IsNullOrEmpty(Txt_ScrapeFolower.Text))
                    {
                        GlobusLogHelper.log.Info("Please Upload Comment Message");
                        ModernDialog.ShowMessage("Please Upload Comment Message", "Upload Message", MessageBoxButton.OK);
                        return;
                    }
                    Obj_Scrapingg.isStopScrapeUser = false;
                    Obj_Scrapingg.lstThreadsScrapeUser.Clear();
                    Obj_Scrapingg.ScrapFollower = true;
                    Regex checkNo = new Regex("^[0-9]*$");

                    int processorCount = objUtils.GetProcessor();

                    int threads = 25;

                    int maxThread = 25 * processorCount;
                    try
                    {
                        ScrapingManager.Mindelay_ScarpeFollower = Convert.ToInt32(txtMessage_Scrapefollower_DelayMin.Text);
                        ScrapingManager.Maxdelay_ScarpeFollower = Convert.ToInt32(txtMessage_Scrapefollower_Delaymax.Text);
                        ScrapingManager.No_ThreadFollower = Convert.ToInt32(txtMessage_Scrapefollower_NoOfThreads.Text);
                        ScrapingManager.Username_ScrapFollower = Txt_ScrapeFolower.Text;
                        ScrapingManager.selected_Account = Select_To_Account.Text.ToString();
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtMessage_Scrapefollower_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_Scrapefollower_NoOfThreads.Text))
                    {
                        threads = Convert.ToInt32(txtMessage_Scrapefollower_NoOfThreads.Text);
                    }

                    if (threads > maxThread)
                    {
                        threads = 25;
                    }
                    Obj_Scrapingg.NoOfThreadsLikePosterScarpeUser = threads;



                    Thread CommentPosterThread = new Thread(Obj_Scrapingg.StartScrapFollower);
                    CommentPosterThread.Start();
                }

                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
            else
            {
                GlobusLogHelper.log.Info("Please Load Accounts !");
                GlobusLogHelper.log.Debug("Please Load Accounts !");

            }
        }

        ScrapingManager Obj_Scrapingg = new ScrapingManager();


        private void btnMessage_Scrapefollower_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Obj_Scrapingg.isStopScrapeFollower = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = Obj_Scrapingg.lstThreadsScrapeFollower.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        Obj_Scrapingg.lstThreadsScrapeFollower.Remove(item);
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

        private void Scrapefollower_detail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ifchangeRequired)
            {
                NewMethod();
            }
        }

        private void NewMethod()
        {
            try
            {
                //DataSet ds = DataBaseHandler.SelectQuery("select * from tb_scrape_follower where username = '" + item + "'", "tb_scrape_follower");
                //   DataTable dt = ds.Tables[0];
                try
                {

                    Globals.selectedUsername = Scrapefollower_detail.SelectedItem.ToString();
                        var window = new ModernDialog
                        {
                            Content = new UserControl_FollowerScrape()
                        };
                        window.MinHeight = 300;
                        window.MinWidth = 800;
                        window.ShowDialog();
                    
                }
                catch { };
            }
            catch { }
        }


        public void AddtoComboboc()
        {

          DataSet ds =   DataBaseHandler.SelectQuery("select * from tb_scrape_follower", "tb_scrape_follower");
          DataTable dt = ds.Tables[0];
          List<string> lstitem = new List<string>();
          foreach (DataRow item in dt.Rows)
          {

              if (!lstitem.Contains(item["username"].ToString()))
              {

                  lstitem.Add((item["username"].ToString()));
 
              }

          }

          this.Dispatcher.Invoke(new Action(delegate
          {

              Scrapefollower_detail.Items.Clear();

              foreach (string item in lstitem)
              {
                  try
                  {
                      Scrapefollower_detail.Items.Add(item);
                  }
                  catch { };
              }

          }));





        }

        private void btnMessage_Scrapefollower_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Txt_ScrapeFolower.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }



    }
}
