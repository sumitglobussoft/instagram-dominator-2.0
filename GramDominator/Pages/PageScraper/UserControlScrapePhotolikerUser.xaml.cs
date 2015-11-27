using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
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
    /// Interaction logic for UserControlScrapePhotolikerUser.xaml
    /// </summary>
    public partial class UserControlScrapePhotolikerUser : UserControl
    {
        public UserControlScrapePhotolikerUser()
        {
            InitializeComponent();

            AccountBinding();
        }

        Utils objUtils = new Utils();
        public void AccountBinding()
        {
            try
            {
                cmb_Select_To_Account.Items.Clear();
                if (IGGlobals.listAccounts.Count > 0)
                {
                    foreach (var item in IGGlobals.listAccounts)
                    {
                        cmb_Select_To_Account.Items.Add(item.Split(':')[0]);
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

        private void btnMessage_ScrapePhotoLikerUser_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
                        GlobalDeclration.objScrapeUser.isStopScrapeUser = false;
                        GlobalDeclration.objScrapeUser.lstofThreadScrapeUser.Clear();

                        Regex checkNo = new Regex("^[0-9]*$");

                        int processorCount = objUtils.GetProcessor();

                        int threads = 25;

                        if (chkBox_Scraper_ScrapeUserFromPhoto_SingleUsername.IsChecked == true)
                        {
                            GlobalDeclration.objScrapeUser.listOfUsernameForPhotouserScraper.Clear();
                            GlobalDeclration.objScrapeUser.usernmeToScrape = Txt_UsernameToScrapeFormPhoto.Text;
                            GlobalDeclration.objScrapeUser.listOfUsernameForPhotouserScraper.Add(Txt_UsernameToScrapeFormPhoto.Text);

                        }

                        int maxThread = 25 * processorCount;
                        try
                        {
                            GlobalDeclration.objScrapeUser.minDelayScrapeUser = Convert.ToInt32(txt_ScrapeUsers_DelayMin.Text);
                            GlobalDeclration.objScrapeUser.maxDelayScrapeUser = Convert.ToInt32(txt_ScrapeUsers_DelayMax.Text);
                            GlobalDeclration.objScrapeUser.NoOfThreadsScarpeUser = Convert.ToInt32(txt_Tweet_ScrapeUsers_NoOfThreads.Text);

                            GlobalDeclration.objScrapeUser.noOfPhotoToScrape = Convert.ToInt32(Txt_ScrapeUser_ScrapeUser_NoOfPhotoToScrape.Text);
                            GlobalDeclration.objScrapeUser.noOfUserToScrape = Convert.ToInt32(Txt_ScrapeUser_ScrapeFollowing_NoOfUserToScrape.Text);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Enter in Correct Formate/Fill all Field");
                            ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
                            return;
                        }
                        if (threads > maxThread)
                        {
                            threads = 25;
                        }
                        GlobalDeclration.objScrapeUser.isScrapePhotoLikeOfUser = true;
                        Thread CommentPosterThread = new Thread(GlobalDeclration.objScrapeUser.StartScrapUser);
                        CommentPosterThread.Start();
                        GlobusLogHelper.log.Info("------ ScrapeFollower Proccess Started ------");
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
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
            }
        }

        private void chkBox_Scraper_ScrapeUserFromPhoto_SingleUsername_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_ScrapeUserFromPhoto_Browse.Visibility = Visibility.Hidden;
                lblLoadUsername.Content = "Enter Username To Scrape ";
            }
            catch (Exception ex)
            {

            }
        }

        private void chkBox_Scraper_ScrapeUserFromPhoto_MultipleUsername_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_ScrapeUserFromPhoto_Browse.Visibility = Visibility.Visible;
                lblLoadUsername.Content = "Load Username To Scrape ";
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_ScrapeUserFromPhoto_Browse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    Txt_UsernameToScrapeFormPhoto.Text = dlg.FileName.ToString();
                    readcommentidFile(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
            }
        }

        public void readcommentidFile(string commentidFilePath)
        {
            try
            {
                GlobalDeclration.objScrapeUser.listOfUsernameForPhotouserScraper.Clear();
                List<string> commentUserlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentUserlist)
                {
                    GlobalDeclration.objScrapeUser.listOfUsernameForPhotouserScraper.Add(commentidlist_item);
                }
                GlobalDeclration.objScrapeUser.listOfUsernameForPhotouserScraper = GlobalDeclration.objScrapeUser.listOfUsernameForPhotouserScraper.Distinct().ToList();
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + GlobalDeclration.objScrapeUser.listOfUsernameForPhotouserScraper.Count + " Username Uploaded. ]");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
            }
        }
    }
}
