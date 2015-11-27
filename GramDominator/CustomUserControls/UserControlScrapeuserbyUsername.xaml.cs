using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using Scraping;
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

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControlScrapeuserbyUsername.xaml
    /// </summary>
    public partial class UserControlScrapeuserbyUsername : UserControl
    {
        public UserControlScrapeuserbyUsername()
        {
            InitializeComponent();
        }

        private void rdoBtn_LikeBy_ScrapeUserName_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.HashTagForScrap.Clear();
            }
            catch { }
            try
            {
                btn_ScrapeUserName_BrowseUsers.Visibility = Visibility.Hidden;

            }
            catch { };
            try
            {
                txt_ScrapeUserName_LoadUsersPath.IsReadOnly = false;

            }
            catch { };
        }

        private void rdoBtn_ScarpeUserName_MultipleUser(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_ScrapeUserName_BrowseUsers.Visibility = Visibility.Visible;

            }
            catch { };
            try
            {
                txt_ScrapeUserName_LoadUsersPath.IsReadOnly = true;

            }
            catch { };
        }

        private void btn_ScrapeUserName_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_ScrapeUserName_LoadUsersPath.Text = dlg.FileName.ToString();
                    readScrap_userFile(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
            }
            catch { };
        }
        public void readScrap_userFile(string commentidFilePath)
        {
            try
            {
                ClGlobul.HashTagForScrap.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {

                    ClGlobul.HashTagForScrap.Add(commentidlist_item);
                }
                ClGlobul.HashTagForScrap = ClGlobul.HashTagForScrap.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.HashTagForScrap.Count + " UserName  Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_ScrapeUsername_SaveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {

                        if (string.IsNullOrEmpty(txt_ScrapeUserName_LoadUsersPath.Text) && string.IsNullOrEmpty(txtMessage_UserName_NoOfUser.Text))
                        {
                            GlobusLogHelper.log.Info("Please Fill All Detail");
                            ModernDialog.ShowMessage("Please Fill All Detail", "Upload Message", MessageBoxButton.OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }


                    if (rdoBtn_ScrapeUserName_SingleUser.IsChecked == true)
                    {
                        ScrapingManager.UserScrape_single = txt_ScrapeUserName_LoadUsersPath.Text;

                    }
                    if (rdoBtn_ScrapeUserName_MultipleUser.IsChecked == true)
                    {
                        ScrapingManager.UserScape_Path = txt_ScrapeUserName_LoadUsersPath.Text;
                        //ObjPhotoManager.message_comment = txtMessage_Comment_LoadMessages.Text;
                    }

                    ScrapingManager.No_UserCount = Convert.ToInt32(txtMessage_UserName_NoOfUser.Text);
                    ScrapingManager.UserScraper_Username = true;
                }


                else
                {
                    GlobusLogHelper.log.Info("Please Load Accounts !");
                    GlobusLogHelper.log.Debug("Please Load Accounts !");

                }
                if ((!string.IsNullOrEmpty(txtMessage_UserName_NoOfUser.Text)))
                {
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        ScrapingManager Obj_Scraping = new ScrapingManager();

        private void btn_ScrapeUsername_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_ScrapeUserName_LoadUsersPath.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
    }
}
