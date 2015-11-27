using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
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
    /// Interaction logic for UserControlUnlikePhoto.xaml
    /// </summary>
    public partial class UserControlUnlikePhoto : UserControl
    {
        public UserControlUnlikePhoto()
        {
            InitializeComponent();
        }

        private void Select_To_UnlikePhoto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Select_To_UnlikePhoto.SelectedIndex == 1)
                {
                    var window = new ModernDialog
                    {
                        Content = new UserControlUnlikePhotoByPhotoId()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
                if (Select_To_UnlikePhoto.SelectedIndex == 2)
                {
                    var window = new ModernDialog
                    {
                        Content = new UserControlUnlikePhotoByUsername()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        Utils objUtils = new Utils();

        private void btnMessage_UnLike_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ObjPhotoManager_new.unlike = true;
                ObjPhotoManager_new.isStopLikePoster = false;
                ObjPhotoManager_new.lstThreadsLikePoster.Clear();

                Regex checkNo = new Regex("^[0-9]*$");

                int processorCount = objUtils.GetProcessor();

                int threads = 25;

                int maxThread = 25 * processorCount;
                try
                {
                    PhotoManager.minDelayUnLikePoster = Convert.ToInt32(txtMessage_UnLike_DelayMin.Text);
                    PhotoManager.maxDelayUnLikePoster = Convert.ToInt32(txtMessage_UnLike_DelayMax.Text);
                    PhotoManager.Nothread_UnLikePoster = Convert.ToInt32(txtMessage_UnLike_NoOfThreads.Text);


                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    return;
                }

                if (!string.IsNullOrEmpty(txtMessage_UnLike_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_UnLike_NoOfThreads.Text))
                {
                    threads = Convert.ToInt32(txtMessage_UnLike_NoOfThreads.Text);
                }

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

        private void btnMessage_UnLike_Stop_Click(object sender, RoutedEventArgs e)
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
    }
}
