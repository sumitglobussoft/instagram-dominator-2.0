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
    /// Interaction logic for UserControlDownloadPhoto.xaml
    /// </summary>
    public partial class UserControlDownloadPhoto : UserControl
    {
        public UserControlDownloadPhoto()
        {
            InitializeComponent();
        }

        private void btu_Downloadphoto_Loadfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PhotoDwonload_progess.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_downloadphoto_Username.Text = dlg.FileName.ToString();
                    ReadLargePhotoFile(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
                PhotoDwonload_progess.IsIndeterminate = false;
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
                    ClGlobul.lstStoreDownloadImageKeyword.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.lstStoreDownloadImageKeyword.Count + " UserName Uploaded. ]");
            }
            catch (Exception ex)
            {
                
            }
        }

        private void rdo_downloadphoto_single_check(object sender, RoutedEventArgs e)
        {
            try
            {
                btu_Downloadphoto_Loadfile.Visibility = Visibility.Hidden;
            }
            catch { };
            try
            {
                txt_downloadphoto_Username.IsReadOnly = false;
            }
            catch { };
        }

        private void rdo_downloadphoto_multiple_check(object sender, RoutedEventArgs e)
        {
            try
            {
                btu_Downloadphoto_Loadfile.Visibility = Visibility.Visible;
            }
            catch { };
            try
            {
                txt_downloadphoto_Username.IsReadOnly = true;
            }
            catch { };
        }

        Utils objUtils = new Utils();
        private void btnMessage_downloadphoto_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    ObjPhotoManager_new.DownloadPhoto = true;
                    ObjPhotoManager_new.isStopDwonloadPoster = false;
                    ObjPhotoManager_new.lstThreadsDwonloadPoster.Clear();

                    Regex checkNo = new Regex("^[0-9]*$");

                    int processorCount = objUtils.GetProcessor();

                    int threads = 25;

                    int maxThread = 25 * processorCount;
                    try
                    {
                        PhotoManager.minDelayDownloadPoster = Convert.ToInt32(txtMessage_Downloadphoto_DelayMin.Text);
                        PhotoManager.maxDelayDownloadPoster = Convert.ToInt32(txtMessage_Download_DelayMax.Text);
                        PhotoManager.Nothread_DownloadPoster = Convert.ToInt32(txtMessage_Downloadphoto_NoOfThreads.Text);
                        PhotoManager.no_photo_Download = Convert.ToInt32(txtDownload_nophoto.Text);                        
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        return;
                    }
                    if (rdo_downloadPhoto_SingleUser.IsChecked == true)
                    {
                        PhotoManager.UserphotoDownload_Single = txt_downloadphoto_Username.Text;
                    }
                    if (rdo_downloadPhoto_multipleUser.IsChecked == true)
                    {
                        PhotoManager.UserphotoDownload_Multiple = txt_downloadphoto_Username.Text;
                    }
                    if(rdo_HashTag.IsChecked==true)
                    {
                        PhotoManager.IsDownLoadImageUsingHashTag = true;
                    }
                    if(rdo_UserName.IsChecked==true)
                    {
                        PhotoManager.IsDownLoadImageUsingUserName = true;
                    }
                    if (!string.IsNullOrEmpty(txtMessage_Downloadphoto_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_Downloadphoto_NoOfThreads.Text))
                    {
                        threads = Convert.ToInt32(txtMessage_Downloadphoto_NoOfThreads.Text);
                    }

                    if (threads > maxThread)
                    {
                        threads = 25;
                    }
                    ObjPhotoManager_new.NoOfThreadsLikePoster = threads;



                    Thread CommentPosterThread = new Thread(ObjPhotoManager_new.StartLikePoster);
                    CommentPosterThread.Start();
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Upload Account");
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

        }
        PhotoManager ObjPhotoManager_new = new PhotoManager();

        private void btnMessage_downloadphoto_Stop_Click(object sender, RoutedEventArgs e)
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

        private void btnMessage_downloadphoto_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_downloadphoto_Username.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }









    }
}
