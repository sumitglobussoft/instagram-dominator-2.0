using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using Follower;
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
using System.Threading;
using System.Text.RegularExpressions;
using Globussoft;

namespace GramDominator.Pages.PageFollower
{
    /// <summary>
    /// Interaction logic for UserControlFollower.xaml
    /// </summary>
    public partial class UserControlFollower : UserControl
    {
        public UserControlFollower()
        {
            InitializeComponent();
        }

        private void follower_browser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                followprogress.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_followerUser.Text = dlg.FileName.ToString();
                    readFollowerFile(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
                followprogress.IsIndeterminate = false;
            }
            catch { };
        }

        public void readFollowerFile(string commentidFilePath)
        {
            try
            {
                ClGlobul.followingList.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {

                    ClGlobul.followingList.Add(commentidlist_item);
                }
                ClGlobul.followingList = ClGlobul.followingList.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.followingList.Count + " UserName  Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        private void rdo_FollowInput_SingleFollow_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.followingList.Clear();
            }
            catch { }
            try
            {
                follower_browser.Visibility = Visibility.Hidden;
                
            }
            catch { };
            try
            {
                txt_followerUser.IsReadOnly = false;
               
            }
            catch { };
        }

        private void rdo_FollowInput_MultipleFollow_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                follower_browser.Visibility = Visibility.Visible;

            }
            catch { };
            try
            {
                txt_followerUser.IsReadOnly = true;

            }
            catch { };
        }

        Utils objUtils = new Utils();
        private void btnMessage_follower_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {

                        if (string.IsNullOrEmpty(txt_followerUser.Text))
                        {
                            GlobusLogHelper.log.Info("Please Upload  Message");
                            ModernDialog.ShowMessage("Please Upload  Message", "Upload Message", MessageBoxButton.OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                    obj_follower.isStopFollowerPoster = false;
                    obj_follower.lstThreadsFollowerPoster.Clear();
                    obj_follower.IsFollow = true;
                    Regex checkNo = new Regex("^[0-9]*$");
                    int processorCount = objUtils.GetProcessor();
                    int threads = 25;
                    int maxThread = 25 * processorCount;
                    try
                    {
                        try
                        {
                            FollowerFollowing.minDelayFollowerPoster = Convert.ToInt32(txtMessage_follower_DelayMin.Text);
                            FollowerFollowing.maxDelayFollowerPoster = Convert.ToInt32(txtMessage_follower_DelayMax.Text);
                            FollowerFollowing.Nothread_Follower = Convert.ToInt32(txtMessage_follower_NoOfThreads.Text);
                            FollowerFollowing.No_Follow_User = Convert.ToInt32(txtNo_Follower.Text);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Enter in Correct Format");
                            return;
                        }

                        if (rdo_MultipleUser_follower.IsChecked == true)
                        {

                            FollowerFollowing.UserName_path = txt_followerUser.Text;
                        }
                        if (rdo_SingleUser_follower.IsChecked == true)
                        {

                            FollowerFollowing.txt_UserName = txt_followerUser.Text;
                        }


                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                    if (!string.IsNullOrEmpty(txtMessage_follower_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_follower_NoOfThreads.Text))
                    {
                        threads = Convert.ToInt32(txtMessage_follower_NoOfThreads.Text);
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


        private void btnMessage_follower_Stop_Click(object sender, RoutedEventArgs e)
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

        private void btnMessage_Clear_click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_followerUser.Text = string.Empty;
                txtMessage_follower_NoOfThreads.Text = string.Empty;
                txtNo_Follower.Text = string.Empty;
                txtMessage_follower_DelayMin.Text = string.Empty;
                txtMessage_follower_DelayMax.Text = string.Empty;

            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

       

       
    }
}
