using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
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
using UsingUsercontrol;

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControlUsingUsernamelikeandcomment.xaml
    /// </summary>
    public partial class UserControlUsingUsernamelikeandcomment : UserControl
    {
        public UserControlUsingUsernamelikeandcomment()
        {
            InitializeComponent();
        }

        private void btn_UsingUsername_likecomment_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_UsingUserName_User.Text = dlg.FileName.ToString();
                    ReadLargeFile_usingUser_likecommentuser(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");

            }
            catch { };
        }

        public void ReadLargeFile_usingUser_likecommentuser(string photoFilename)
        {
            ClGlobul.UsingUsername_likecommentUserList.Clear();
            
            try
            {
                List<string> photolist = GlobusFileHelper.ReadFile((string)photoFilename);
                foreach (string phoyoList_item in photolist)
                {
                    ClGlobul.UsingUsername_likecommentUserList.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.UsingUsername_likecommentUserList.Count + " UserName Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_UsingUserName_likecomment_messagebrwer_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_UsingUsername_commentmessage.Text = dlg.FileName.ToString();
                    ReadLargeFile_usingUser_onlycommentmessage(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");

            }
            catch { };
        }

        public void ReadLargeFile_usingUser_onlycommentmessage(string photoFilename)
        {
           
            ClGlobul.UsingUsername_likecommentMessageList.Clear();
            try
            {
                List<string> photolist = GlobusFileHelper.ReadFile((string)photoFilename);
                foreach (string phoyoList_item in photolist)
                {
                    ClGlobul.UsingUsername_likecommentMessageList.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.UsingUsername_likecommentMessageList.Count + " Message Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_UsingUsername_likecomment_SaveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
<<<<<<< HEAD

=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                        UsingUsernameManager.likeandcomment = true;
                        UsingUsernameManager.UsingUsername_likecomment_Nouser = Convert.ToInt32(txt_UsingUsername_likecomment_nouser.Text);
                        if (string.IsNullOrEmpty(txt_UsingUserName_User.Text) && string.IsNullOrEmpty(txt_UsingUsername_commentmessage.Text))
                        {
                            GlobusLogHelper.log.Info("Please Upload UserName/Comment Message");
                            ModernDialog.ShowMessage("Please Upload UserName/Comment Message", "Upload Message", MessageBoxButton.OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }


                    if (rdoBtn_UsingUsername_like_comment_SingleUser.IsChecked == true)
                    {
                        UsingUsernameManager.UsingUsercontrol_Likecomment_single = txt_UsingUserName_User.Text;
                        UsingUsernameManager.UsingUsercontrol_Likecomment_message_single = txt_UsingUsername_commentmessage.Text;

                    }
                    if (rdoBtn_UsingUsername_like_comment_MultipleUser.IsChecked == true)
                    {
                        UsingUsernameManager.UsingUsercontrol_Likecomment_User_path = txt_UsingUserName_User.Text;
                        UsingUsernameManager.UsingUsercontrol_Likecomment_message_single_path = txt_UsingUsername_commentmessage.Text;
                    }
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Load Accounts !");
                    GlobusLogHelper.log.Debug("Please Load Accounts !");

                }
                if ((!string.IsNullOrEmpty(txt_UsingUserName_User.Text)) && (!string.IsNullOrEmpty(txt_UsingUsername_commentmessage.Text)) && (!string.IsNullOrEmpty(txt_UsingUsername_likecomment_nouser.Text)))
                {
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Upload UserName/Comment Message/No.User");
                    ModernDialog.ShowMessage("Please Upload UserName/Comment Message/No.User", "Upload Message", MessageBoxButton.OK);
                    return;
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        UsingUsernameManager obj_UsingUsernameManager = new UsingUsernameManager();
        

        private void rdoBtn_UsingUsername_like_comment_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.UsingUsername_likecommentMessageList.Clear();
                ClGlobul.UsingUsername_likecommentUserList.Clear();

            }
            catch { }
            try
            {
                btn_UsingUsername_likecomment_BrowseUsers.Visibility = Visibility.Hidden;
                btn_UsingUserName_likecomment_messagebrwer.Visibility = Visibility.Hidden;

            }
            catch { };
            try
            {
                txt_UsingUserName_User.IsReadOnly = false;
                txt_UsingUsername_commentmessage.IsReadOnly = false;

            }
            catch { };
        }

        private void rdoBtn_UsingUsername_like_comment_MultipleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_UsingUsername_likecomment_BrowseUsers.Visibility = Visibility.Visible;
                btn_UsingUserName_likecomment_messagebrwer.Visibility = Visibility.Visible;

            }
            catch { };
            try
            {
                txt_UsingUserName_User.IsReadOnly = true;
                txt_UsingUsername_commentmessage.IsReadOnly = true;

            }
            catch { };
        }

        private void btn_UsingUsername_likecomment_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_UsingUserName_User.Text = string.Empty;
                txt_UsingUsername_commentmessage.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
    }
}
