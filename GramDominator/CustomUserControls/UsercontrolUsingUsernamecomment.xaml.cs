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
    /// Interaction logic for UsercontrolUsingUsernamecomment.xaml
    /// </summary>
    public partial class UsercontrolUsingUsernamecomment : UserControl
    {
        public UsercontrolUsingUsernamecomment()
        {
            InitializeComponent();
        }

        private void btn_UsingUsername_Onlycomment_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_UsingUserName_onlycomment_User.Text = dlg.FileName.ToString();
                    ReadLargeFile_usingUser_onlycommentuser(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
                
            }
            catch { };
        }

        public void ReadLargeFile_usingUser_onlycommentuser(string photoFilename)
        {
            ClGlobul.UsingUsername_onlycommentusernameList.Clear();
            try
            {
                List<string> photolist = GlobusFileHelper.ReadFile((string)photoFilename);
                foreach (string phoyoList_item in photolist)
                {
                    ClGlobul.UsingUsername_onlycommentusernameList.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.UsingUsername_onlycommentusernameList.Count + " UserName Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_UsingUserName_onlycomment_messagebrwer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_UsingUsername_onlycommentmessage.Text = dlg.FileName.ToString();
                    ReadLargeFile_usingUser_onlycommentmessage(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
                
            }
            catch { };
        }

        public void ReadLargeFile_usingUser_onlycommentmessage(string photoFilename)
        {
            ClGlobul.UsingUsername_onlycommentmessageList.Clear();
            try
            {
                List<string> photolist = GlobusFileHelper.ReadFile((string)photoFilename);
                foreach (string phoyoList_item in photolist)
                {
                    ClGlobul.UsingUsername_onlycommentmessageList.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.UsingUsername_onlycommentmessageList.Count + " Message Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        //private void btn_UsingUsername_onlycomment_SaveUser_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (IGGlobals.listAccounts.Count > 0)
        //        {
        //            try
        //            {
        //                UsingUsernameManager.onlyComment = true;
        //                UsingUsernameManager.UsingUsername_onlycomment_Nouser = Convert.ToInt32(txt_UsingUsername_onlycomment_nouser.Text);
        //                if (string.IsNullOrEmpty(txt_UsingUserName_onlycomment_User.Text) && string.IsNullOrEmpty(txt_UsingUsername_onlycommentmessage.Text))
        //                {
        //                    GlobusLogHelper.log.Info("Please Upload UserName");
        //                    ModernDialog.ShowMessage("Please Upload UserName", "Upload Message", MessageBoxButton.OK);
        //                    return;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
        //            }


        //            if (rdoBtn_UsingUsername_onlycomment_SingleUser.IsChecked == true)
        //            {
        //                UsingUsernameManager.UsingUsercontrol_onlycomment_single = txt_UsingUserName_onlycomment_User.Text;
        //                UsingUsernameManager.UsingUsercontrol_onlycommentmessage_single = txt_UsingUsername_onlycommentmessage.Text;

        //            }
        //            if (rdoBtn_UsingUsername_onlycomment_MultipleUser.IsChecked == true)
        //            {
        //                UsingUsernameManager.UsingUsercontrol_onlycomment_path = txt_UsingUserName_onlycomment_User.Text;
        //                UsingUsernameManager.UsingUsercontrol_onlycommentmessgae = txt_UsingUsername_onlycommentmessage.Text;
        //            }
        //        }
        //        else
        //        {
        //            GlobusLogHelper.log.Info("Please Load Accounts !");
        //            GlobusLogHelper.log.Debug("Please Load Accounts !");
        //        }
        //        if ((!string.IsNullOrEmpty(txt_UsingUserName_onlycomment_User.Text)) && (!string.IsNullOrEmpty(txt_UsingUsername_onlycommentmessage.Text)) && (!string.IsNullOrEmpty(txt_UsingUsername_onlycomment_nouser.Text)))
        //        {
        //            ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
        //    }
        //}
<<<<<<< HEAD
=======
=======
        private void btn_UsingUsername_onlycomment_SaveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
                        UsingUsernameManager.onlyComment = true;
                        UsingUsernameManager.UsingUsername_onlycomment_Nouser = Convert.ToInt32(txt_UsingUsername_onlycomment_nouser.Text);
                        if (string.IsNullOrEmpty(txt_UsingUserName_onlycomment_User.Text) && string.IsNullOrEmpty(txt_UsingUsername_onlycommentmessage.Text))
                        {
                            GlobusLogHelper.log.Info("Please Upload UserName");
                            ModernDialog.ShowMessage("Please Upload UserName", "Upload Message", MessageBoxButton.OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }


                    if (rdoBtn_UsingUsername_onlycomment_SingleUser.IsChecked == true)
                    {
                        UsingUsernameManager.UsingUsercontrol_onlycomment_single = txt_UsingUserName_onlycomment_User.Text;
                        UsingUsernameManager.UsingUsercontrol_onlycommentmessage_single = txt_UsingUsername_onlycommentmessage.Text;

                    }
                    if (rdoBtn_UsingUsername_onlycomment_MultipleUser.IsChecked == true)
                    {
                        UsingUsernameManager.UsingUsercontrol_onlycomment_path = txt_UsingUserName_onlycomment_User.Text;
                        UsingUsernameManager.UsingUsercontrol_onlycommentmessgae = txt_UsingUsername_onlycommentmessage.Text;
                    }
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Load Accounts !");
                    GlobusLogHelper.log.Debug("Please Load Accounts !");
                }
                if ((!string.IsNullOrEmpty(txt_UsingUserName_onlycomment_User.Text)) && (!string.IsNullOrEmpty(txt_UsingUsername_onlycommentmessage.Text)) && (!string.IsNullOrEmpty(txt_UsingUsername_onlycomment_nouser.Text)))
                {
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master

        UsingUsernameManager obj_UsingUsernameManager = new UsingUsernameManager();

        private void rdoBtn_UsingUsername_onlycomment_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.UsingUsername_onlycommentusernameList.Clear();
                ClGlobul.UsingUsername_onlycommentmessageList.Clear();
            }
            catch { }

            try
            {
                btn_UsingUsername_Onlycomment_BrowseUsers.Visibility = Visibility.Hidden;
                btn_UsingUserName_onlycomment_messagebrwer.Visibility = Visibility.Hidden;


            }
            catch { };
            try
            {
                txt_UsingUserName_onlycomment_User.IsReadOnly = false;
                txt_UsingUsername_onlycommentmessage.IsReadOnly = false;


            }
            catch { };
        }

        private void rdoBtn_UsingUsername_onlycomment_MultipleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_UsingUsername_Onlycomment_BrowseUsers.Visibility = Visibility.Visible;
                btn_UsingUserName_onlycomment_messagebrwer.Visibility = Visibility.Visible;


            }
            catch { };
            try
            {
                txt_UsingUserName_onlycomment_User.IsReadOnly = true;
                txt_UsingUsername_onlycommentmessage.IsReadOnly = true;


            }
            catch { };
        }

        private void btn_UsingUsername_onlycomment_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_UsingUserName_onlycomment_User.Text = string.Empty;
                txt_UsingUsername_onlycommentmessage.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }
    }
}
