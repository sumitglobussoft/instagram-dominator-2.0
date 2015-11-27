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
using BaseLibID;
using Photo;
using Globussoft;

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControlPhotoDownload_Hashtag.xaml
    /// </summary>
    public partial class UserControlPhotoDownload_Hashtag : UserControl
    {
        public UserControlPhotoDownload_Hashtag()
        {
            InitializeComponent();
        }

        private void btu_Downloadphoto_loadByHashtag_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_downloadphoto_Hashtag.Text = dlg.FileName.ToString();
                    ReadLargePhotoFile(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
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
                    ClGlobul.lstStoreDownloadImageKeyword.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.lstStoreDownloadImageKeyword.Count + " UserName Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        private void rdo_DownloadSingle_ByHashtag_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btu_Downloadphoto_loadByuserhash.Visibility = Visibility.Hidden;
            }
            catch { };
            try
            {
                txt_downloadphoto_Hashtag.IsReadOnly = false;
            }
            catch { };
        }

        private void rdo_DownloadMultiple_ByHashtag_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btu_Downloadphoto_loadByuserhash.Visibility = Visibility.Visible;
            }
            catch { };
            try
            {
                txt_downloadphoto_Hashtag.IsReadOnly = true;
            }
            catch { };
        }

        private void btu_DownloadPhoto_Clear_hash_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_downloadphoto_Hashtag.Text = string.Empty;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

       
    }
}
