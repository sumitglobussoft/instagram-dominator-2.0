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
    /// Interaction logic for UserControlUnlikePhotoByPhotoId.xaml
    /// </summary>
    public partial class UserControlUnlikePhotoByPhotoId : UserControl
    {
        public UserControlUnlikePhotoByPhotoId()
        {
            InitializeComponent();
        }

        private void rdoBtn_LikeBy_UnPhotoID_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.PhotoList.Clear();
            }
            catch { }
            try
            {
                btn_UnLikePhoto_Id_BrowseUsers.Visibility = Visibility.Hidden;

            }
            catch { };
            try
            {
                txt_UnLikePhoto_Id_LoadUsersPath.IsReadOnly = false;

            }
            catch { };
        }

        private void rdoBtn_UnLikeBy_PhotoId_MultipleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_UnLikePhoto_Id_BrowseUsers.Visibility = Visibility.Visible;

            }
            catch { };
            try
            {
                txt_UnLikePhoto_Id_LoadUsersPath.IsReadOnly = true;

            }
            catch { };
        }

        private void btn_UnLikePhoto_Id_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_UnLikePhoto_Id_LoadUsersPath.Text = dlg.FileName.ToString();
                    ReadLargePhotoFile(dlg.FileName);

                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
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
                
            }
        }

        

        PhotoManager ObjPhotoManager = new PhotoManager();

        private void btn_UnLikePhoto_Id_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_UnLikePhoto_Id_LoadUsersPath.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }
    }

}
