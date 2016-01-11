using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using HashTagsManager;
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
    /// Interaction logic for UserControlHashtagsfollower.xaml
    /// </summary>
    public partial class UserControlHashtagsfollower : UserControl
    {
        public UserControlHashtagsfollower()
        {
            InitializeComponent();
        }

        private void rdoBtn_HashTagsfollower_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.HashFollower.Clear();
            }
            catch { }
            try
            {
                btn_HashTags_follower_BrowseUsers.Visibility = Visibility.Hidden;

            }
            catch { };
            try
            {
                txt_HashTags_follower_LoadUsersPath.IsReadOnly = false;

            }
            catch { };
        }

        private void rdoBtn_HashTagsfollower_MultipleUser(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_HashTags_follower_BrowseUsers.Visibility = Visibility.Visible;

            }
            catch { };
            try
            {
                txt_HashTags_follower_LoadUsersPath.IsReadOnly = true;

            }
            catch { };
        }

        private void btn_HashTagsfollower_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_HashTags_follower_LoadUsersPath.Text = dlg.FileName.ToString();
                    readHash_Follower(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
            }
            catch { };
        }
        public void readHash_Follower(string commentidFilePath)
        {
            try
            {
                ClGlobul.HashFollower.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {

                    ClGlobul.HashFollower.Add(commentidlist_item);
                }
                ClGlobul.HashFollower = ClGlobul.HashFollower.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.HashFollower.Count + " Message  Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_HashTagsfollower_SaveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
                        hash_managerlibry.Hash_Follow = true;
                        if (string.IsNullOrEmpty(txt_HashTags_follower_LoadUsersPath.Text))
                        {
                            GlobusLogHelper.log.Info("Please Upload Photo ID");
                            ModernDialog.ShowMessage("Please Upload Photo Id", "Upload Message", MessageBoxButton.OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }


                    if (rdoBtn_HashTags_follower_SingleUser.IsChecked == true)
                    {
                        hash_managerlibry.Hash_Follower_single = txt_HashTags_follower_LoadUsersPath.Text;
                    }
                    if (rdoBtn_HashTags_follower_MultipleUser.IsChecked == true)
                    {
                        hash_managerlibry.Hash_Follower_path = txt_HashTags_follower_LoadUsersPath.Text;
                    }
                    hash_managerlibry.hashFollower_Number = Convert.ToInt32(txtMessage_hashtagfollower_NoOfuser.Text);


                }

                else
                {
                    GlobusLogHelper.log.Info("Please Load Accounts !");
                    GlobusLogHelper.log.Debug("Please Load Accounts !");

                }
                if ((!string.IsNullOrEmpty(txt_HashTags_follower_LoadUsersPath.Text)) && (!string.IsNullOrEmpty(txtMessage_hashtagfollower_NoOfuser.Text)))
                {
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        hash_managerlibry objj_hash_managerlibry = new hash_managerlibry();

        private void btn_HashTagsfollower_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_HashTags_follower_LoadUsersPath.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master

        private void chkNotSendRequest_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                hash_managerlibry.chkNotSendRequest = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        
<<<<<<< HEAD
=======
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
    }
}
