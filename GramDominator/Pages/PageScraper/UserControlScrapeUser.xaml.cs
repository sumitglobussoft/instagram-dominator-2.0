using BaseLib;
<<<<<<< HEAD
using BaseLibID;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
using Scraping;
using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Data;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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
<<<<<<< HEAD
            AccountReport_ScrapeUser();
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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
<<<<<<< HEAD
                    GlobusLogHelper.log.Info("Enter in Correct Formate/Fill all Field");
                    ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
=======
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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
<<<<<<< HEAD
                Thread CommentPosterThread = new Thread(Obj_Scrapingg.StartLikePoster);
                CommentPosterThread.Start();
                GlobusLogHelper.log.Info("------ ScrapeUser Proccess Started ------");
=======



                Thread CommentPosterThread = new Thread(Obj_Scrapingg.StartLikePoster);
                CommentPosterThread.Start();
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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
<<<<<<< HEAD
                Thread objScrapeUser = new Thread(stopMultiThreadScrapeUser);
                objScrapeUser.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void stopMultiThreadScrapeUser()
        {
            try
            {
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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

<<<<<<< HEAD
        public void closeEvent()
        { }


=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        private void Select_To_ScrapeUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Select_To_Scrapeuser.SelectedIndex == 1)
                {
<<<<<<< HEAD
                    UserControlScrapeuserbyUsername obj_UserControlScrapeuserbyUsername = new UserControlScrapeuserbyUsername();
                    var window = new ModernDialog
                    {

                        Content = obj_UserControlScrapeuserbyUsername
                       
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                   // window.Topmost = true;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };
                    window.ShowDialog();
                    string s1 = string.Empty;
                    try
                    {
                        if (string.IsNullOrEmpty(obj_UserControlScrapeuserbyUsername.txt_ScrapeUserName_LoadUsersPath.Text))
                        {
                            ModernDialog.ShowMessage("Upload Username", "Upload Usename", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter Username");
                            return;
                        }
                        if (obj_UserControlScrapeuserbyUsername.rdoBtn_ScrapeUserName_SingleUser.IsChecked == true)
                        {
                            ScrapingManager.UserScape_Path = string.Empty;
                            ScrapingManager.UserScrape_single = obj_UserControlScrapeuserbyUsername.txt_ScrapeUserName_LoadUsersPath.Text;
                            ScrapingManager.No_UserCount = Convert.ToInt32(obj_UserControlScrapeuserbyUsername.txtMessage_UserName_NoOfUser.Text);
                        }
                        if (obj_UserControlScrapeuserbyUsername.rdoBtn_ScrapeUserName_MultipleUser.IsChecked == true)
                        {
                            ScrapingManager.UserScrape_single = string.Empty;
                            ScrapingManager.UserScape_Path = obj_UserControlScrapeuserbyUsername.txt_ScrapeUserName_LoadUsersPath.Text;
                            ScrapingManager.No_UserCount = Convert.ToInt32(obj_UserControlScrapeuserbyUsername.txtMessage_UserName_NoOfUser.Text);
                        }
                        ScrapingManager.UserScraper_Username = true;
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }




                }
                if (Select_To_Scrapeuser.SelectedIndex == 2)
                {
                    UserControlScarpeUser_Keyword obj_UserControlScarpeUser_Keyword = new UserControlScarpeUser_Keyword();
                    var window = new ModernDialog
                    {
                        Content = obj_UserControlScarpeUser_Keyword
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };
                  //  window.Topmost = true;
                    window.ShowDialog();
                    string s1 = string.Empty;
                    try
                    {
                        if ((obj_UserControlScarpeUser_Keyword.ScrapeUser_keyword_slect.SelectedItem).ToString()=="")
                        {
                            ModernDialog.ShowMessage("Upload keyword", "Upload keyword", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter keyword");
                            return;
                        }
                        else
                        {
                            string Selected_item = obj_UserControlScarpeUser_Keyword.ScrapeUser_keyword_slect.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                            ScrapingManager.User_key = Selected_item;
                            ScrapingManager.No_UserCount_keyword = Convert.ToInt32(obj_UserControlScarpeUser_Keyword.txtMessage_UserName_keyword_NoOfUser.Text);
                            ScrapingManager.UserScraper_UserByKeyword = true;

                        }
                        // PhotoManager.IsDownLoadImageUsingHashTag = true;
                        ScrapingManager.UserScraper_Username = false;
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }
                    
=======
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
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                }
            }
            catch (Exception ex)
            {
                // GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

<<<<<<< HEAD
        public void AccountReport_ScrapeUser()
        {
            
             try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Account_User");
                dt.Columns.Add("UserName");
                dt.Columns.Add("DateTime");
                dt.Columns.Add("UserLink");
                dt.Columns.Add("Status");

                int counter = 0;
                DataSet ds = null;
                try
                {
                    ds = Qm.SelectAccountreport("ScrapeUser");
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
                        string UserLink = ds_item[9].ToString();
                        string Status = ds_item[7].ToString();
                        dt.Rows.Add(Account_User, UserName,DateTime,UserLink,Status);


                    }
                    catch { };


                }
                DataView dv;
                this.Dispatcher.Invoke(new Action(delegate
                {
                    dtGrdScrapeUser_ScrapeUser_AccountsReport.ItemsSource = dt.DefaultView;

                }));
            }
            catch(Exception ex)
            {
                
            }
        }

        private void RefreshAccountreport_ScrapeUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountReport_ScrapeUser();
            }
            catch (Exception ex)
            {

            }
        }
        QueryManager Qm = new QueryManager();
        private void DeleteAccountModule_ScrapeUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = ModernDialog.ShowMessage("Are You Sure Delete Loaded Account ?? ", "Delete Account", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        QueryExecuter.deleteQueryforAccountReport("ScrapeUser");
                        IGGlobals.listAccounts.Clear();
                        AccountReport_ScrapeUser();
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ExportScrapeUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f


    }
}
