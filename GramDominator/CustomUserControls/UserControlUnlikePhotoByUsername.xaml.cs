﻿using BaseLib;
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
    /// Interaction logic for UserControlUnlikePhotoByUsername.xaml
    /// </summary>
    public partial class UserControlUnlikePhotoByUsername : UserControl
    {
        public UserControlUnlikePhotoByUsername()
        {
            InitializeComponent();
        }

        private void rdoBtn_UnLikeBy_Username_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.PhotoList.Clear();
                ClGlobul.Userlist.Clear();
            }
            catch { }
            try
            {
                btn_UnLikePhoto_Username_BrowseUsers.Visibility = Visibility.Hidden;

            }
            catch { };
            try
            {
                txt_UnLikePhoto_Username_LoadUsersPath.IsReadOnly = false;

            }
            catch { };
        }

        private void rdoBtn_UnLikeBy_Username_MultipleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_UnLikePhoto_Username_BrowseUsers.Visibility = Visibility.Visible;

            }
            catch { };
            try
            {
                txt_UnLikePhoto_Username_LoadUsersPath.IsReadOnly = true;

            }
            catch { };
        }

        private void btn_UnLikePhoto_Username_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_UnLikePhoto_Username_LoadUsersPath.Text = dlg.FileName.ToString();
                    ReadLargePhotoFile(dlg.FileName);

                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
            }
            catch { };
        }

        public void ReadLargePhotoFile(string photoFilename)
        {
            ClGlobul.PhotoList.Clear();
            ClGlobul.Userlist.Clear();
            try
            {
                List<string> photolist = GlobusFileHelper.ReadFile((string)photoFilename);
                foreach (string phoyoList_item in photolist)
                {
                    ClGlobul.Userlist.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.PhotoList.Count + " Image IDs Uploaded. ]");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

<<<<<<< HEAD
        
=======
<<<<<<< HEAD
        
=======
        private void btn_UnLikePhoto_Username_SaveUser_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
                        PhotoManager.no_photounlike_Username = Convert.ToInt32(txtMessage_UnLike_NoOfphotousername.Text);
                        if (string.IsNullOrEmpty(txt_UnLikePhoto_Username_LoadUsersPath.Text))
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


                    if (rdoBtn_UnLikeBy_PhotoUser_SingleUser.IsChecked == true)
                    {
                        PhotoManager.UnLikePhoto_UserName = txt_UnLikePhoto_Username_LoadUsersPath.Text;

                    }
                    if (rdoBtn_UnLikeBy_PhotoUser_MultipleUser.IsChecked == true)
                    {
                        PhotoManager.UnLikePhoto_Username_Path = txt_UnLikePhoto_Username_LoadUsersPath.Text;
                        //ObjPhotoManager.message_comment = txtMessage_Comment_LoadMessages.Text;
                    }

                    if ((!string.IsNullOrEmpty(txt_UnLikePhoto_Username_LoadUsersPath.Text)))
                    {
                        ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
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
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master

        PhotoManager ObjPhotoManager = new PhotoManager();

        private void btn_UnLikePhoto_Username_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_UnLikePhoto_Username_LoadUsersPath.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
    }
}

