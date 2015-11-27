using BaseLib;
using BaseLibID;
using Globussoft;
using Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GramDominator.Pages.PagePhoto
{
    /// <summary>
    /// Interaction logic for UserControlPhotoUserliker.xaml
    /// </summary>
    public partial class UserControlPhotoUserliker : UserControl
    {
        public UserControlPhotoUserliker()
        {
            InitializeComponent();
        }

        private void btu_UserPhoto_liker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserPhoto_liker_progress.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_UserPhoto_liker.Text = dlg.FileName.ToString();
                    ReadLargePhotoFile(dlg.FileName);
                }                
                UserPhoto_liker_progress.IsIndeterminate = false;
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
                    ClGlobul.lstUsername.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.lstUsername.Count + " UserName Uploaded. ]");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void rdo_Userphoto_liker_single_check(object sender, RoutedEventArgs e)
        {
            try
            {
                btu_UserPhoto_liker.Visibility = Visibility.Hidden;
            }
            catch { };
            try
            {
                txt_UserPhoto_liker.IsReadOnly = false;
            }
            catch { }
        }

        private void rdo_Userphoto_liker_multiple_check(object sender, RoutedEventArgs e)
        {
            try
            {
                btu_UserPhoto_liker.Visibility = Visibility.Visible;
            }
            catch { };
            try
            {
                txt_UserPhoto_liker.IsReadOnly = true;
            }
            catch { }
        }


        Utils objUtils = new Utils();
        private void btnMessage_Userphoto_liker_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ObjPhotoManager_new.liker_photo = true;
                ObjPhotoManager_new.isStopLikerPoster = false;
                ObjPhotoManager_new.lstThreadsLikerPoster.Clear();

                Regex checkNo = new Regex("^[0-9]*$");

                int processorCount = objUtils.GetProcessor();

                int threads = 25;

                int maxThread = 25 * processorCount;
                try
                {
                    PhotoManager.minDelayLikerPoster = Convert.ToInt32(txt_Userphoto_liker_DelayMin.Text);
                    PhotoManager.mixDelayLikerPoster = Convert.ToInt32(txt_Userphoto_liker_DelayMax.Text);
                    PhotoManager.NoUser_LikerPoster = Convert.ToInt32(txt_Userphoto_liker_NoOfUser.Text);
                    PhotoManager.no_Photo_liker = Convert.ToInt32(txt_Userphoto_liker_nophoto.Text);

                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    return;
                }
                if (rdo_Userphoto_liker.IsChecked == true)
                {
                    PhotoManager.txt_username_Liker_single = txt_UserPhoto_liker.Text;
                }
                if (rdo_Userphoto_liker_multipleUser.IsChecked ==true)
                {
                    PhotoManager.txt_username_Liker_Multiple = txt_UserPhoto_liker.Text;
                }

                //if (!string.IsNullOrEmpty(txt_Userphoto_liker_NoOfThreads.Text) && checkNo.IsMatch(txt_Userphoto_liker_NoOfThreads.Text))
                //{
                //    threads = Convert.ToInt32(txt_Userphoto_liker_NoOfThreads.Text);
                //}

                if (threads > maxThread)
                {
                    threads = 25;
                }
                ObjPhotoManager_new.NoOfThreadsLikePoster = threads;



                Thread CommentPosterThread = new Thread(ObjPhotoManager_new.StartLikePoster);
                CommentPosterThread.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

        }
        PhotoManager ObjPhotoManager_new = new PhotoManager();


        private void btnMessage_Userphoto_liker_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ObjPhotoManager_new.isStopLikePoster = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = ObjPhotoManager_new.lstThreadsLikePoster.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        ObjPhotoManager_new.lstThreadsLikePoster.Remove(item);
                    }
                    catch (Exception ex)
                    {
                        //Thread.ResetAbort();
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            GlobusLogHelper.log.Info("Process Stopped !");
            GlobusLogHelper.log.Debug("Process Stopped !");
        }

        private void btnMessage_Userphoto_liker_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_UserPhoto_liker.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :  " + ex.StackTrace);
            }
        }




    }
}
