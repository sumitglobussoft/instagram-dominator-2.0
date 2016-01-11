using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControlLikePhotoByID.xaml
    /// </summary>
    public partial class UserControlLikePhotoByID : UserControl
    {
        public UserControlLikePhotoByID()
        {
            InitializeComponent();
        }

        

        private void btn_LikePhoto_Id_SaveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {

                        if (string.IsNullOrEmpty(txt_LikePhoto_Id_LoadUsersPath.Text))
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
                    
                    
                        if (rdoBtn_LikeBy_PhotoId_SingleUser.IsChecked == true)
                        {
<<<<<<< HEAD
                            PhotoManager.LikePhoto_ID_path = string.Empty;
=======
<<<<<<< HEAD
                            PhotoManager.LikePhoto_ID_path = string.Empty;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
                            PhotoManager.LikePhoto_ID = txt_LikePhoto_Id_LoadUsersPath.Text;
                            
                        }
                        if (rdoBtn_LikeBy_PhotoId_MultipleUser.IsChecked == true)
                        {
<<<<<<< HEAD
                            PhotoManager.LikePhoto_ID = string.Empty;
=======
<<<<<<< HEAD
                            PhotoManager.LikePhoto_ID = string.Empty;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
                            PhotoManager.LikePhoto_ID_path = txt_LikePhoto_Id_LoadUsersPath.Text;
                            //ObjPhotoManager.message_comment = txtMessage_Comment_LoadMessages.Text;
                        }

                    }
                               
           
                else
                {
                    GlobusLogHelper.log.Info("Please Load Accounts !");
                    GlobusLogHelper.log.Debug("Please Load Accounts !");

                }
                if ((!string.IsNullOrEmpty(txt_LikePhoto_Id_LoadUsersPath.Text)))
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

        private void rdoBtn_LikeBy_PhotoID_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.PhotoList.Clear();
            }
            catch { }
            try
            {
                btn_LikePhoto_Id_BrowseUsers.Visibility = Visibility.Hidden;
<<<<<<< HEAD

=======
<<<<<<< HEAD

=======
                
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                txt_LikePhoto_Id_LoadUsersPath.IsReadOnly = false;
<<<<<<< HEAD

=======
<<<<<<< HEAD

=======
                
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        public void closeEvent()
        {
        }


<<<<<<< HEAD
=======
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
        private void rdoBtn_LikeBy_PhotoId_MultipleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_LikePhoto_Id_BrowseUsers.Visibility = Visibility.Visible;
<<<<<<< HEAD
                txt_LikePhoto_Id_LoadUsersPath.Visibility = Visibility.Visible;
                
=======
<<<<<<< HEAD
                txt_LikePhoto_Id_LoadUsersPath.Visibility = Visibility.Visible;
                
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
            }
            catch { };
            try
            {
                txt_LikePhoto_Id_LoadUsersPath.IsReadOnly = true;

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void btn_LikePhoto_Id_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_LikePhoto_Id_LoadUsersPath.Text = dlg.FileName.ToString();
                    ReadLargePhotoFile(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
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

<<<<<<< HEAD
        

        private void Clear_photolike_ByID_Click(object sender, RoutedEventArgs e)
=======
<<<<<<< HEAD
        

        private void Clear_photolike_ByID_Click(object sender, RoutedEventArgs e)
=======
        private void btn_LikePhoto_Id_Clear_Click(object sender, RoutedEventArgs e)
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
        {
            try
            {
                txt_LikePhoto_Id_LoadUsersPath.Text = string.Empty;
<<<<<<< HEAD

            }
            catch (Exception ex)
=======
<<<<<<< HEAD

            }
            catch (Exception ex)
=======
            }
            catch(Exception ex)
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
    }
}
