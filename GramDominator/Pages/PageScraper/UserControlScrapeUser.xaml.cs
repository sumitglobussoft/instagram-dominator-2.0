using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
using Scraping;
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

namespace GramDominator.Pages.PageScraper
{
    /// <summary>
    /// Interaction logic for UserControlScrapeUser.xaml
    /// </summary>
    public partial class UserControlScrapeUser : UserControl
    {
        public UserControlScrapeUser()
        {
            InitializeComponent();
        }


        Utils objUtils = new Utils();
        private void btnMessage_ScrapeUSer_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Obj_Scrapingg.isStopScrapeUser = false;
                Obj_Scrapingg.lstThreadsScrapeUser.Clear();

                Regex checkNo = new Regex("^[0-9]*$");

                int processorCount = objUtils.GetProcessor();

                int threads = 25;

                int maxThread = 25 * processorCount;
                try
                {
                    ScrapingManager.minDelayScrapeUser = Convert.ToInt32(txtMessage_ScrapeUser_DelayMin.Text);
                    ScrapingManager.maxDelayScrapeUser = Convert.ToInt32(txtMessage_ScrapeUser_DelayMax.Text);
                    ScrapingManager.Nothread_ScrapeUser = Convert.ToInt32(txtMessage_ScrapeUser_NoOfThreads.Text);
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    return;
                }

                if (!string.IsNullOrEmpty(txtMessage_ScrapeUser_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_ScrapeUser_NoOfThreads.Text))
                {
                    threads = Convert.ToInt32(txtMessage_ScrapeUser_NoOfThreads.Text);
                }

                if (threads > maxThread)
                {
                    threads = 25;
                }
                Obj_Scrapingg.NoOfThreadsLikePosterScarpeUser = threads;



                Thread CommentPosterThread = new Thread(Obj_Scrapingg.StartLikePoster);
                CommentPosterThread.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        ScrapingManager Obj_Scrapingg = new ScrapingManager();


        private void btnMessage_ScrapeUser_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Obj_Scrapingg.isStopScrapeUser = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = Obj_Scrapingg.lstThreadsScrapeUser.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        Obj_Scrapingg.lstThreadsScrapeUser.Remove(item);
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

        private void Select_To_ScrapeUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Select_To_Scrapeuser.SelectedIndex == 1)
                {
                    var window = new ModernDialog
                    {
                        Content = new UserControlScrapeuserbyUsername()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
                if (Select_To_Scrapeuser.SelectedIndex == 2)
                {
                    var window = new ModernDialog
                    {
                        Content = new UserControlScarpeUser_Keyword()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }




    }
}
