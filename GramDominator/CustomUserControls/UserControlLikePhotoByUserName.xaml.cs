using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using Photo;
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
    /// Interaction logic for UserControlLikePhotoByUserName.xaml
    /// </summary>
    public partial class UserControlLikePhotoByUserName : UserControl
    {
        public UserControlLikePhotoByUserName()
        {
            InitializeComponent();
        }

        private void rdoBtn_LikeBy_Username_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.PhotoList.Clear();
            }
            catch { }
            try
            {
                btn_LikePhoto_Username_BrowseUsers.Visibility = Visibility.Hidden;

            }
            catch { };
            try
            {
                txt_LikePhoto_Username_LoadUsersPath.IsReadOnly = false;

            }
            catch { };
        }

        private void rdoBtn_LikeBy_Username_MultipleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_LikePhoto_Username_BrowseUsers.Visibility = Visibility.Visible;

            }
            catch { };
            try
            {
                txt_LikePhoto_Username_LoadUsersPath.IsReadOnly = true;

            }
            catch { };
        }

        private void btn_LikePhoto_Username_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_LikePhoto_Username_LoadUsersPath.Text = dlg.FileName.ToString();
                    ReadLargePhotoFile(dlg.FileName);
                }

            }
            catch { };
        }

        public void ReadLargePhotoFile(string photoFilename)
        {
            ClGlobul.PhotoList.Clear();
            try
            {
                List<string> photolist = GlobusFileHelper.ReadFile((string)photoFilename);
                foreach (string phoyoList_item in photolist)
                {
                    ClGlobul.PhotoList.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.PhotoList.Count + " Image IDs Uploaded. ]");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void btn_LikePhoto_Username_SaveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
                        PhotoManager.noPhotoLike_username = Convert.ToInt32(txtMessage_Like_NoOfFollowers.Text);
                        if (string.IsNullOrEmpty(txt_LikePhoto_Username_LoadUsersPath.Text))
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


                    if (rdoBtn_LikeBy_PhotoUser_SingleUser.IsChecked == true)
                    {
                        PhotoManager.LikePhoto_username_path = string.Empty;
                        PhotoManager.LikePhoto_Username = txt_LikePhoto_Username_LoadUsersPath.Text;

                    }
                    if (rdoBtn_LikeBy_PhotoUser_MultipleUser.IsChecked == true)
                    {
                        PhotoManager.LikePhoto_Username = string.Empty;
                        PhotoManager.LikePhoto_username_path = txt_LikePhoto_Username_LoadUsersPath.Text;
                        //ObjPhotoManager.message_comment = txtMessage_Comment_LoadMessages.Text;
                    }


                }


                else
                {
                    GlobusLogHelper.log.Info("Please Load Accounts !");
                    GlobusLogHelper.log.Debug("Please Load Accounts !");

                }
                if ((!string.IsNullOrEmpty(txt_LikePhoto_Username_LoadUsersPath.Text)))
                {
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        PhotoManager ObjPhotoManager = new PhotoManager();

        private void btn_LikePhoto_Username_Clear_click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_LikePhoto_Username_LoadUsersPath.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

       
    }
}
