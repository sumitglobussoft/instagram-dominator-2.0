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
    /// Interaction logic for UsercontrolUsingUsernamelike.xaml
    /// </summary>
    public partial class UsercontrolUsingUsernamelike : UserControl
    {
        public UsercontrolUsingUsernamelike()
        {
            InitializeComponent();
        }

        private void btn_UsingUserName_likeBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_UsingUsername_like.Text = dlg.FileName.ToString();
                    ReadLargeOnlyLike(dlg.FileName);
                }

            }
            catch { };
        }
        public void ReadLargeOnlyLike(string photoFilename)
        {
            ClGlobul.UsingUsername_Usernmaelist.Clear();
           
            try
            {
                List<string> photolist = GlobusFileHelper.ReadFile((string)photoFilename);
                foreach (string phoyoList_item in photolist)
                {
                    ClGlobul.UsingUsername_Usernmaelist.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.UsingUsername_Usernmaelist.Count + " Image IDs Uploaded. ]");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        //private void btn_HashTagsfollower_SaveUser_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (IGGlobals.listAccounts.Count > 0)
        //        {
        //            try
        //            {
        //                UsingUsernameManager.onlyLike = true;
        //                UsingUsernameManager.UsingUsername_like_Nouser = Convert.ToInt32(txtMessage_UsingUsername_NoOfuser.Text);
        //                if (string.IsNullOrEmpty(txt_UsingUsername_like.Text))
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


        //            if (rdoBtn_UsingUsername_likeSingle.IsChecked == true)
        //            {
        //                UsingUsernameManager.UsingUsercontrol_Like_single = txt_UsingUsername_like.Text;

        //            }
        //            if (rdoBtn_UsingUsername_likeMultiple.IsChecked == true)
        //            {
        //                UsingUsernameManager.UsingUsercontrol_Like_path = txt_UsingUsername_like.Text;
        //                //ObjPhotoManager.message_comment = txtMessage_Comment_LoadMessages.Text;
        //            }


        //        }


        //        else
        //        {
        //            GlobusLogHelper.log.Info("Please Load Accounts !");
        //            GlobusLogHelper.log.Debug("Please Load Accounts !");

        //        }
        //        if ((!string.IsNullOrEmpty(txt_UsingUsername_like.Text)) && (!string.IsNullOrEmpty(txtMessage_UsingUsername_NoOfuser.Text)))
        //        {
        //            ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
        //    }
        //}

        UsingUsernameManager obj_UsingUsernameManager = new UsingUsernameManager();

        private void rdoBtn_UsingUsername_likeSingle_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.UsingUsername_Usernmaelist.Clear();
            }
            catch { }
            try
            {
                btn_UsingUserName_likeBrowse.Visibility = Visibility.Hidden;
               

            }
            catch { };
            try
            {
                txt_UsingUsername_like.IsReadOnly = false;
                

            }
            catch { };
        }

        private void rdoBtn_HashTagsfollower_MultipleUser(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_UsingUserName_likeBrowse.Visibility = Visibility.Visible;
               

            }
            catch { };
            try
            {
                txt_UsingUsername_like.IsReadOnly = true;
               

            }
            catch { };
        }

        private void btn_HashTagsfollower_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_UsingUsername_like.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
    }
}
