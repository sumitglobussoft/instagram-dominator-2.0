using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
using BaseLibID;
using Scraping;
using System.Text.RegularExpressions;
using System.Threading;

namespace GramDominator.Pages.ScrapeUsers
{
    /// <summary>
    /// Interaction logic for UserControlScrapeUsers.xaml
    /// </summary>
    public partial class UserControlScrapeUsers : UserControl
    {
        public UserControlScrapeUsers()
        {
            InitializeComponent();
            BindAccount();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        //private void cmb_SelectOptionToScrapeUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if(cmb_SelectOptionToScrapeUser.SelectedIndex==1)
        //        {
        //            try
        //            {
        //                GlobalDeclration.objScrapeUser.isScrapeFollower = true;
        //                var dialog = new ModernDialog
        //                {
        //                    Content = new UserControlScrapeUsersScrapeFollower()
        //                };
        //                dialog.MinWidth = 700;
        //                dialog.MinHeight = 300;

        //                var customButton = new Button() { Content = "Cancel" };
        //                customButton.Click += (ee, vv) => { dialog.Close(); };
        //                dialog.Buttons = new Button[] { customButton };
        //                dialog.ShowDialog();
        //            }
        //            catch(Exception ex)
        //            {
        //                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace); 
        //            }
        //        }
        //        else if (cmb_SelectOptionToScrapeUser.SelectedIndex == 2)
        //        {
        //            try
        //            {
        //                GlobalDeclration.objScrapeUser.isScrapeFollowing = true;
        //                var dialog = new ModernDialog
        //                {
        //                    Content = new UserControlScrapeUserScrapeFollowing()
        //                };
        //                dialog.MinWidth = 700;
        //                dialog.MinHeight = 300;

        //                var customButton = new Button() { Content = "Cancel" };
        //                customButton.Click += (ee, vv) => { dialog.Close(); };
        //                dialog.Buttons = new Button[] { customButton };
        //                dialog.ShowDialog();
        //            }
        //            catch (Exception ex)
        //            {
        //                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
        //            }
        //        }
        //        else if (cmb_SelectOptionToScrapeUser.SelectedIndex == 3)
        //        {
        //            try
        //            {
        //                GlobalDeclration.objScrapeUser.isScrapeUserWhoCommentOnPhoto = true;
        //                var dialog = new ModernDialog
        //                {
        //                    Content = new UserControlScrapeUsersWhoCommentOnPhoto()
        //                };
        //                dialog.MinWidth = 700;
        //                dialog.MinHeight = 300;

        //                var customButton = new Button() { Content = "Cancel" };
        //                customButton.Click += (ee, vv) => { dialog.Close(); };
        //                dialog.Buttons = new Button[] { customButton };
        //                dialog.ShowDialog();
        //            }
        //            catch (Exception ex)
        //            {
        //                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
        //            }
        //        }
        //        else if (cmb_SelectOptionToScrapeUser.SelectedIndex == 4)
        //        {
        //            try
        //            {
        //                GlobalDeclration.objScrapeUser.isScrapePhotoURL = true;
        //                var dialog = new ModernDialog
        //                {
        //                    Content = new UserControlScrapeUserScrapePhotoURL()
        //                };
        //                dialog.MinWidth = 700;
        //                dialog.MinHeight = 300;

        //                var customButton = new Button() { Content = "Cancel" };
        //                customButton.Click += (ee, vv) => { dialog.Close(); };
        //                dialog.Buttons = new Button[] { customButton };
        //                dialog.ShowDialog();
        //            }
        //            catch (Exception ex)
        //            {
        //                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
        //            }
        //        }
        //        else if (cmb_SelectOptionToScrapeUser.SelectedIndex == 5)
        //        {
        //            try
        //            {
        //                GlobalDeclration.objScrapeUser.isScrapeHashTag = true;
        //                var dialog = new ModernDialog
        //                {
        //                    Content = new UserControlScrapeUserScrapeHashTag()
        //                };
        //                dialog.MinWidth = 700;
        //                dialog.MinHeight = 300;

        //                var customButton = new Button() { Content = "Cancel" };
        //                customButton.Click += (ee, vv) => { dialog.Close(); };
        //                dialog.Buttons = new Button[] { customButton };
        //                dialog.ShowDialog();
        //            }
        //            catch (Exception ex)
        //            {
        //                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
        //            }
        //        }
        //        else if (cmb_SelectOptionToScrapeUser.SelectedIndex == 6)
        //        {
        //            try
        //            {
        //                GlobalDeclration.objScrapeUser.isScrapeUserWhoLikesUserPhoto = true;
        //                var dialog = new ModernDialog
        //                {
        //                    Content = new UserControlScrapeUserWhoLikeUserPhoto()
        //                };
        //                dialog.MinWidth = 700;
        //                dialog.MinHeight = 300;

        //                var customButton = new Button() { Content = "Cancel" };
        //                customButton.Click += (ee, vv) => { dialog.Close(); };
        //                dialog.Buttons = new Button[] { customButton };
        //                dialog.ShowDialog();
        //            }
        //            catch (Exception ex)
        //            {
        //                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
        //            }
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace); 
        //    }
        //}

        public void SaveScrapeFollowerData()
        {
            try
            {
                
            }
            catch(Exception ex)
            {

            }
        }

        private void chkBox_ScrapeUser_WhereToMentionUsers_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog=new ModernDialog
                {
                   Content =new UserControlScrapeUserWhereToMention()
                };
                dialog.MinWidth = 700;
                dialog.MinHeight = 300;
                var customButton = new Button() { Content = "Save" };
                customButton.Click += (ee, vv) => { dialog.Close(); };
                dialog.Buttons = new Button[] { customButton };
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }
        //ScrapingManager Obj_Scrapingg = new ScrapingManager();
        Utils objUtils = new Utils();
        private void btnMessage_ScrapeUser_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
                        //if (string.IsNullOrEmpty(Txt_ScrapeFolower.Text))
                        //{
                        //    GlobusLogHelper.log.Info("Please Upload Username ");
                        //    ModernDialog.ShowMessage("Please Upload Username ", "Upload Message", MessageBoxButton.OK);
                        //    return;
                        //}
                        GlobalDeclration.objScrapeUser.isStopScrapeUser = false;
                        GlobalDeclration.objScrapeUser.lstofThreadScrapeUser.Clear();
                        
                        Regex checkNo = new Regex("^[0-9]*$");

                        int processorCount = objUtils.GetProcessor();

                        int threads = 25;

                        int maxThread = 25 * processorCount;
                        try
                        {
                            GlobalDeclration.objScrapeUser.minDelayScrapeUser = Convert.ToInt32(txt_ScrapeUsers_DelayMin.Text);
                            GlobalDeclration.objScrapeUser.maxDelayScrapeUser = Convert.ToInt32(txt_ScrapeUsers_DelayMax.Text);
                            GlobalDeclration.objScrapeUser.NoOfThreadsScarpeUser = Convert.ToInt32(txt_Tweet_ScrapeUsers_NoOfThreads.Text);
                           // ScrapingManager.Username_ScrapFollower = Txt_ScrapeFolower.Text;
                          //  ScrapingManager.selected_Account = Select_To_Account.Text.ToString();
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Enter in Correct Formate/Fill all Field");
                            ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
                            return;
                        }

                        //if (!string.IsNullOrEmpty(txtMessage_Scrapefollower_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_Scrapefollower_NoOfThreads.Text))
                        //{
                        //    threads = Convert.ToInt32(txtMessage_Scrapefollower_NoOfThreads.Text);
                        //}

                        if (threads > maxThread)
                        {
                            threads = 25;
                        }

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

        private void rdoBtn_MentionUsers_InsertUrlToMention_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rdoBtn_MentionUsers_InsertUrlToMention.IsChecked = true;
                rdoBtn_MentionUsers_UseScrapedToMention.IsChecked = false;
                rdoBtn_MentionUsers_UploadUrlsToMention.IsChecked = false;
                GlobalDeclration.objMentionUser.IsInsertUrlToMention = true;
                GlobalDeclration.objMentionUser.IsUseScrapedurlToMention = false;
                GlobalDeclration.objMentionUser.IsUploadUrlToMention = false;
                var dialog = new ModernDialog
                {
                    Content = new UserControlMentionUsersInsertUrl()
                };
                dialog.MinHeight = 200;
                dialog.MinWidth = 300;

                var customButton = new Button() { Content = "Cancel" };
                customButton.Click += (ee, vv) => { dialog.Close(); };
                dialog.Buttons = new Button[] { customButton };
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
            }
        }

        private void rdoBtn_MentionUsers_UseScrapedToMention_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rdoBtn_MentionUsers_InsertUrlToMention.IsChecked = false;
                rdoBtn_MentionUsers_UseScrapedToMention.IsChecked = true;
                rdoBtn_MentionUsers_UploadUrlsToMention.IsChecked = false;
                GlobalDeclration.objMentionUser.IsInsertUrlToMention = false;
                GlobalDeclration.objMentionUser.IsUseScrapedurlToMention = true;
                GlobalDeclration.objMentionUser.IsUploadUrlToMention = false;

                var dialog = new ModernDialog
                {
                    Content = new UserControlMentionUsersUseScrapedUrl()
                };
                dialog.MinHeight = 200;
                dialog.MinWidth = 650;

                var customButton = new Button() { Content = "Close" };
                customButton.Click += (ee, vv) => { dialog.Close(); };
                dialog.Buttons = new Button[] { customButton };
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
            }
        }

        private void rdoBtn_MentionUsers_UploadUrlsToMention_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rdoBtn_MentionUsers_InsertUrlToMention.IsChecked = false;
                rdoBtn_MentionUsers_UseScrapedToMention.IsChecked = false;
                rdoBtn_MentionUsers_UploadUrlsToMention.IsChecked = true;

                GlobalDeclration.objMentionUser.IsInsertUrlToMention = false;
                GlobalDeclration.objMentionUser.IsUseScrapedurlToMention = false;
                GlobalDeclration.objMentionUser.IsUploadUrlToMention = true;

                ShowDilogUploadUrls();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
            }
        }

        public void ShowDilogUploadUrls()
        {
            var dialog = new ModernDialog
            {
                Content = new UserControlMentionUsersUploadUrls()
            };
            dialog.MinHeight = 200;
            dialog.MinWidth = 650;

            var customButton = new Button() { Content = "Close" };
            customButton.Click += (ee, vv) => { dialog.Close(); };
            dialog.Buttons = new Button[] { customButton };
            dialog.ShowDialog();
        }

        private void chkBox_ScrapeUser_NoOfUsersMention_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalDeclration.objMentionUser.IsRandomNoUserMention = false;
                GlobalDeclration.objMentionUser.IsNoOfUserToMention = true;
                lblNoUserToBeMention.IsEnabled = true;
                lblRangeToRandomMention.IsEnabled = false;
                txt_ScrapeUser_NoOfUserToBeMention.IsEnabled = true;
                txt_ScrapeUser_RangeToRandomMention.IsEnabled = false;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
            }
        }

        private void chkBox_ScrapeUser_RandomUsersMention_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalDeclration.objMentionUser.IsRandomNoUserMention = true;
                GlobalDeclration.objMentionUser.IsNoOfUserToMention = false;
                lblNoUserToBeMention.IsEnabled = false;
                lblRangeToRandomMention.IsEnabled = true;
                txt_ScrapeUser_NoOfUserToBeMention.IsEnabled = false;
                txt_ScrapeUser_RangeToRandomMention.IsEnabled = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
            }
        }

        private void btn_MentionUsers_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
                        //if (string.IsNullOrEmpty(Txt_ScrapeFolower.Text))
                        //{
                        //    GlobusLogHelper.log.Info("Please Upload Username ");
                        //    ModernDialog.ShowMessage("Please Upload Username ", "Upload Message", MessageBoxButton.OK);
                        //    return;
                        //}
                        GlobalDeclration.objScrapeUser.isStopScrapeUser = false;
                        GlobalDeclration.objScrapeUser.lstofThreadScrapeUser.Clear();

                        Regex checkNo = new Regex("^[0-9]*$");

                        int processorCount = objUtils.GetProcessor();

                        int threads = 25;

                        int maxThread = 25 * processorCount;
                        try
                        {
                            GlobalDeclration.objScrapeUser.minDelayScrapeUser = Convert.ToInt32(txt_ScrapeUsers_DelayMin.Text);
                            GlobalDeclration.objScrapeUser.maxDelayScrapeUser = Convert.ToInt32(txt_ScrapeUsers_DelayMax.Text);
                            GlobalDeclration.objScrapeUser.NoOfThreadsScarpeUser = Convert.ToInt32(txt_Tweet_ScrapeUsers_NoOfThreads.Text);
                            // ScrapingManager.Username_ScrapFollower = Txt_ScrapeFolower.Text;
                            //  ScrapingManager.selected_Account = Select_To_Account.Text.ToString();
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Enter in Correct Formate/Fill all Field");
                            ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
                            return;
                        }

                        if (rdoBtn_MentionUsers_InsertUrlToMention.IsChecked==false && rdoBtn_MentionUsers_UploadUrlsToMention.IsChecked==false && rdoBtn_MentionUsers_UseScrapedToMention.IsChecked==false)
                        {
                            GlobusLogHelper.log.Info("Please Select Atlest 1 Option For Mention Users");
                            ModernDialog.ShowMessage("Please Select Atlest 1 Option For Mention Users", "Mention User", MessageBoxButton.OK);
                            rdoBtn_MentionUsers_InsertUrlToMention.Focus();
                            return;
                        }  
                        else
                        {
                            if(rdoBtn_MentionUsers_UploadUrlsToMention.IsChecked==true)
                            {
                                if(GlobalDeclration.objMentionUser.listOfUrlToMentionUser.Count==0)
                                {
                                    GlobusLogHelper.log.Info("You Have Not Uploaded Url, Please Upload Url");
                                    ModernDialog.ShowMessage("You Have Not Uploaded Url, Please Upload Url", "Mention User", MessageBoxButton.OK);
                                    ShowDilogUploadUrls();
                                    return;
                                }
                            }                            
                        }
                        if (chkBox_ScrapeUser_NoOfUsersMention.IsChecked==false && chkBox_ScrapeUser_RandomUsersMention.IsChecked==false)
                        {
                            GlobusLogHelper.log.Info("Please Enter No Of User To Mention");
                            ModernDialog.ShowMessage("Please Enter No Of User To Mention", "Mention User Input", MessageBoxButton.OK);
                            chkBox_ScrapeUser_NoOfUsersMention.Focus();
                            return;
                        }
                        else
                        {
                            if (chkBox_ScrapeUser_NoOfUsersMention.IsChecked == true)
                            {
                                if (!string.IsNullOrEmpty(txt_ScrapeUser_NoOfUserToBeMention.Text))
                                {
                                    GlobalDeclration.objMentionUser.noOfUserToMention = int.Parse(txt_ScrapeUser_NoOfUserToBeMention.Text);
                                }
                                else
                                {
                                    GlobusLogHelper.log.Info("Please Enter No Of User To Mention");
                                    ModernDialog.ShowMessage("Please Enter No Of User To Mention", "Mention User Input", MessageBoxButton.OK);
                                    txt_ScrapeUser_NoOfUserToBeMention.Focus();
                                    return;
                                }
                            }
                            else if (chkBox_ScrapeUser_RandomUsersMention.IsChecked == true)
                            {
                                if (!string.IsNullOrEmpty(txt_ScrapeUser_RangeToRandomMention.Text))
                                {
                                    string noOfUser = txt_ScrapeUser_RangeToRandomMention.Text.Split('-')[1];
                                    GlobalDeclration.objMentionUser.noOfRandomNoUserToMention = int.Parse(noOfUser);
                                }
                                else
                                {
                                    GlobusLogHelper.log.Info("Please Enter Range Of User To Mention");
                                    ModernDialog.ShowMessage("Please Enter Range Of User To Mention", "Mention User Input", MessageBoxButton.OK);
                                    txt_ScrapeUser_RangeToRandomMention.Focus();
                                    return;
                                }
                            }
                        }
                        ////if(string.IsNullOrEmpty(cmbBox_MentionUser_SelectAccount.Text))
                        //{
                        //    GlobusLogHelper.log.Info("Please Select An Account");
                        //    ModernDialog.ShowMessage("Please Select An Account", "Mention User", MessageBoxButton.OK);
                        //    cmbBox_MentionUser_SelectAccount.Focus();
                        //    return;
                        //}
                        //else
                        {
                            List<CheckBox> temp = new List<CheckBox>();
                            foreach (CheckBox item in cmbBox_MentionUser_SelectAccount.Items)
                            {
                                temp.Add(item);
                            }
                            if (temp.Where(x => x.IsChecked == true).ToList().Count == 0)
                            {
                                GlobusLogHelper.log.Info("Please Select Atleast One Account ");
                                return;
                            }
                            if (temp.Count > 0)
                            {
                                foreach (CheckBox item in temp)
                                {
                                    if (item.IsChecked == true)
                                    {
                                        GlobalDeclration.objMentionUser.selectedAccountToScrape.Add(item.Content.ToString());
                                    }
                                }

                            }               
		
                            
                        }
                        if (threads > maxThread)
                        {
                            threads = 25;
                        }
                        if(chkBox_ScrapeUser_LoadMessage.IsChecked==false)
                        {
                            GlobusLogHelper.log.Info("Please Load Or Enter Message Comment");
                            ModernDialog.ShowMessage("Please Load Or Enter Message To Comment", "Mention User", MessageBoxButton.OK);
                            chkBox_ScrapeUser_LoadMessage.Focus();
                            return;
                        }
                        GlobusLogHelper.log.Info("------ Scrape Mention User Proccess Started ------");
                        Thread CommentPosterThread = new Thread(GlobalDeclration.objMentionUser.StartMentionUser);
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
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
            }
        }
        public void BindAccount()
        {
            cmbBox_MentionUser_SelectAccount.Items.Clear();
            if(IGGlobals.listAccounts.Count>0)
            {
                foreach(var item in IGGlobals.listAccounts)
                {
                    cmbBox_MentionUser_SelectAccount.Items.Add(new CheckBox() { Content = item.Split(':')[0] });
                }
            }
        }

        private void chkBox_ScrapeUser_LoadMessage_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new ModernDialog
                {
                    Content = new UserControlMentionUserLoadMessage()
                };
                dialog.MinHeight = 200;
                dialog.MinWidth = 750;

                var customButton = new Button() { Content = "Close" };
                customButton.Click += (ee, vv) => { dialog.Close(); };
                dialog.Buttons = new Button[] { customButton };
                dialog.ShowDialog();

            }
            catch(Exception ex)
            {

            }
        }

        private void chkBox_ScrapeUsers_DailyScheduleTask_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new ModernDialog
                {
                    Content = new UserControlScheduleMentionUsers()
                };
                dialog.MinHeight = 300;
                dialog.MinWidth = 750;

                var customButton = new Button() { Content = "Close" };
                customButton.Click += (ee, vv) => { dialog.Close(); };
                dialog.Buttons = new Button[] { customButton };
                dialog.ShowDialog();

            }
            catch (Exception ex)
            {

            }
        }
    }    
}
