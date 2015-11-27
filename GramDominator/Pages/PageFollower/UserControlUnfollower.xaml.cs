using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Follower;
using Globussoft;
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

namespace GramDominator.Pages.PageFollower
{
    /// <summary>
    /// Interaction logic for UserControlUnfollower.xaml
    /// </summary>
    public partial class UserControlUnfollower : UserControl
    {
        public UserControlUnfollower()
        {
            InitializeComponent();
        }

        private void Unfollower_browser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Unfollower_Progess.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_UnfollowerUser.Text = dlg.FileName.ToString();
                    readUnFollowerFile(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
                Unfollower_Progess.IsIndeterminate = false;
                
            }

            catch { };
        }

        public void readUnFollowerFile(string commentidFilePath)
        {
            try
            {
                ClGlobul.lstUnfollowerList.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {

                    ClGlobul.lstUnfollowerList.Add(commentidlist_item);
                }
                ClGlobul.lstUnfollowerList = ClGlobul.lstUnfollowerList.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.lstUnfollowerList.Count + " UserName  Uploaded. ]");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
            }
        }


        Utils objUtils = new Utils();
        private void btnMessage_ScrapeImageUrl_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {

                        if (string.IsNullOrEmpty(txt_UnfollowerUser.Text))
                        {
                            GlobusLogHelper.log.Info("Please Upload  UserName");
                            ModernDialog.ShowMessage("Please Upload  UserName", "Upload Message", MessageBoxButton.OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                    obj_follower.isStopFollowerPoster = false;
                    obj_follower.lstThreadsFollowerPoster.Clear();
                    obj_follower.IsUnFollow = true;
                    Regex checkNo = new Regex("^[0-9]*$");
                    int processorCount = objUtils.GetProcessor();
                    int threads = 25;
                    int maxThread = 25 * processorCount;
                    try
                    {
                        try
                        {
                            FollowerFollowing.minDelayUnFollowerPoster = Convert.ToInt32(txtMessage_Unfollower_DelayMin.Text);
                            FollowerFollowing.maxDelayUnFollowerPoster = Convert.ToInt32(txtMessage_Unfollower_DelayMax.Text);
                            FollowerFollowing.No_UnFollow_User = Convert.ToInt32(txtMessage_no_of_Unfollower.Text);
                            FollowerFollowing.Nothread_Follower = Convert.ToInt32(txtMessage_Unfollower_NoOfThreads.Text);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Enter in Correct Format");                           
                        }


                        if (rdo_Unfollow_MultipleUser.IsChecked == true)
                        {

                            FollowerFollowing.UserName_path_Unfollow = txt_UnfollowerUser.Text;
                        }
                        if (rdo_Unfollow_SingleUser.IsChecked == true)
                        {

                            FollowerFollowing.txt_UserName_Unfollow = txt_UnfollowerUser.Text;
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                    if (!string.IsNullOrEmpty(txtMessage_Unfollower_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_Unfollower_NoOfThreads.Text))
                    {
                        threads = Convert.ToInt32(txtMessage_Unfollower_NoOfThreads.Text);
                    }
                    if (threads > maxThread)
                    {
                        threads = 25;
                    }
                    obj_follower.NoOfThreadsFollowerPoster = threads;
                    Thread CommentPosterThread = new Thread(obj_follower.StartFollowing);
                    CommentPosterThread.Start();
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

        FollowerFollowing obj_follower = new FollowerFollowing();
        
        

        private void btnMessage_ScrapeImageUrl_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                obj_follower.isStopFollowerPoster = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = obj_follower.lstThreadsFollowerPoster.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        obj_follower.lstThreadsFollowerPoster.Remove(item);
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

        private void rdo_SingleUser_Unfollower(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.lstUnfollowerList.Clear();
            }
            catch { }
            try
            {
                Unfollower_browser.Visibility = Visibility.Hidden;
            }
            catch { };
            try
            {
                txt_UnfollowerUser.IsReadOnly = false;
            }
            catch { };
        }

        private void rdo_MultipleUser_Unfollower(object sender, RoutedEventArgs e)
        {
            try
            {
                Unfollower_browser.Visibility = Visibility.Visible;
            }
            catch { };
            try
            {
                txt_UnfollowerUser.IsReadOnly = true;
            }
            catch { };
        }

        private void btnMessage_Clear_click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_UnfollowerUser.Text = string.Empty;
                txtMessage_Unfollower_NoOfThreads.Text = string.Empty;
                txtMessage_no_of_Unfollower.Text = string.Empty;
                txtMessage_Unfollower_DelayMin.Text = string.Empty;
                txtMessage_Unfollower_DelayMax.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        
        

    }
}
