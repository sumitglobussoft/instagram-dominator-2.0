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
<<<<<<< HEAD
using HashTagsManager;
using System.Data;
using GramDominator.CustomUserControls;
using FaceDominator3._0.PageWall;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

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
<<<<<<< HEAD
            AccuntReport_Follow();
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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

<<<<<<< HEAD
      
        private void rdo_FollowInput_SingleFollow_Checked(object sender, RoutedEventArgs e)
        {
            #region commentedcode
            //try
            //{
            //    UserControlLoadTextMessage obj = new UserControlLoadTextMessage();
            //    obj.UserControlHeader.Text = "Enter Targeted Username Here ";
            //    obj.txtEnterSingleMessages.ToolTip = "Username Format : AmitabhBachchan";
            //    var window = new ModernDialog
            //    {

            //        Content = obj
            //    };
            //    window.ShowInTaskbar = true;
            //    Button customButton = new Button() { Content = "Save" };
            //    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
            //    window.Buttons = new Button[] { customButton };

            //    window.ShowDialog();

            //    if (rdo_SingleUser_follower.IsChecked == true)
            //    {
            //        TextRange textRange = new TextRange(obj.txtEnterSingleMessages.Document.ContentStart, obj.txtEnterSingleMessages.Document.ContentEnd);
            //        if (!string.IsNullOrEmpty(textRange.Text))
            //        {
            //            ClGlobul.followingList.Add(textRange.Text);                       
            //        }                   
            //        GlobusLogHelper.log.Debug("Text Messages Loaded : " + ClGlobul.followingList.Count);

            //    }           
            //}
            //catch (Exception ex)
            //{
            //    GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
            //}
            #endregion



            txt_followerUser.Clear();
=======
        private void rdo_FollowInput_SingleFollow_Checked(object sender, RoutedEventArgs e)
        {
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
            try
            {
                ClGlobul.followingList.Clear();
            }
            catch { }
            try
            {
                follower_browser.Visibility = Visibility.Hidden;
<<<<<<< HEAD

=======
                
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
            }
            catch { };
            try
            {
                txt_followerUser.IsReadOnly = false;
<<<<<<< HEAD

=======
               
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
            }
            catch { };
        }

<<<<<<< HEAD
        public void closeEvent()
        {

        }

        private void rdo_FollowInput_MultipleFollow_Checked(object sender, RoutedEventArgs e)
        {
            txt_followerUser.Clear();
=======
        private void rdo_FollowInput_MultipleFollow_Checked(object sender, RoutedEventArgs e)
        {
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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
<<<<<<< HEAD
                
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {

<<<<<<< HEAD
                        //if (string.IsNullOrEmpty(txt_followerUser.Text))
                        //{
                        //    GlobusLogHelper.log.Info("Please Upload  Username");
                        //    ModernDialog.ShowMessage("Please Upload  Username", "Upload Username", MessageBoxButton.OK);
                        //    return;
                        //}
=======
                        if (string.IsNullOrEmpty(txt_followerUser.Text))
                        {
                            GlobusLogHelper.log.Info("Please Upload  Message");
                            ModernDialog.ShowMessage("Please Upload  Message", "Upload Message", MessageBoxButton.OK);
                            return;
                        }
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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
<<<<<<< HEAD
                            GlobusLogHelper.log.Info("Enter in Correct Format/Fill all field");
                            ModernDialog.ShowMessage("Enter in Correct Format/Fill all field", "Error", MessageBoxButton.OK);
=======
                            GlobusLogHelper.log.Info("Enter in Correct Format");
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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
<<<<<<< HEAD

                    if (hash_managerlibry.DivideByUser == true)
                    {
                        Thread ForDivideUser = new Thread(obj_follower.startFollowerdividedataUser);
                        ForDivideUser.Start();
                        GlobusLogHelper.log.Info("------ Follow Proccess Started ------");
                    }
                    else
                    {
                        if (hash_managerlibry.DivideEqual == true)
                        {
                            Thread ForDivideEqual = new Thread(obj_follower.StartDividedatabyequally);
                           ForDivideEqual.Start();
                           GlobusLogHelper.log.Info("------ Follow Proccess Started ------");
                        }
                        else
                        {
                           // btnMessage_follower_Start.IsEnabled = false;
                            Thread CommentPosterThread = new Thread(obj_follower.StartFollowing);
                            CommentPosterThread.Start();
                            GlobusLogHelper.log.Info("------ Follow Proccess Started ------");
                        }
                    }
=======
                    Thread CommentPosterThread = new Thread(obj_follower.StartFollowing);
                    CommentPosterThread.Start();
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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
<<<<<<< HEAD
                Thread stopFollower = new Thread(stopMultiThreadFollower);
                stopFollower.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void stopMultiThreadFollower()
        {
            try
            {
                obj_follower.isStopFollowerPoster = true;
                // btnMessage_follower_Start.IsEnabled = true;
=======
                obj_follower.isStopFollowerPoster = true;

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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
<<<<<<< HEAD
                FollowerFollowing.txt_UserName= string.Empty;
                FollowerFollowing.UserName_path = string.Empty;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

<<<<<<< HEAD

        QueryManager Qm = new QueryManager();
       public void AccuntReport_Follow()
        {
           try
           {
               try
               {
                   DataTable dt = new DataTable();

                   dt.Columns.Add("Account_User");
                   dt.Columns.Add("FollowerName");
                   dt.Columns.Add("DateTime");
                   dt.Columns.Add("Status");

                   int counter = 0;
                   DataSet ds = null;
                   try
                   {
                       ds = Qm.SelectAccountreport("FollowModule");
                   }
                   catch (Exception ex)
                   {
                       GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                   }
                   foreach (DataRow ds_item in ds.Tables[0].Rows)
                   {
                       try
                       {

                           string Account_User = ds_item.ItemArray[2].ToString();
                           string FollowerName = ds_item[5].ToString();
                           string DateTime = ds_item[12].ToString();
                           string Status = ds_item[7].ToString();
                           
                           dt.Rows.Add(Account_User, FollowerName,DateTime ,Status);


                       }
                       catch { };


                   }                  
                   DataView dv;
                   this.Dispatcher.Invoke(new Action(delegate
                   {
                       dtGrdMessage_follower_AccountsReport.ItemsSource = dt.DefaultView;

                   }));

               }
               catch (Exception ex)
               {
                   GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
               }
           }
           catch(Exception ex)
           {
               GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
           }
        }

       private void refreshAccountrepot_follow_Click(object sender, RoutedEventArgs e)
       {
           try
           {
               AccuntReport_Follow();
           }
           catch(Exception ex)
           {
               GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
           }
       }

       private void DeleteAccountrepot_follow_Click(object sender, RoutedEventArgs e)
       {
           var result = ModernDialog.ShowMessage("Are You Sure Delete Loaded Account ?? ", "Delete Account", MessageBoxButton.YesNoCancel);
           if (result == MessageBoxResult.Yes)
           {
               try
               {
                   QueryExecuter.deleteQueryforAccountReport("FollowModule");
                   IGGlobals.listAccounts.Clear();
                   AccuntReport_Follow();
               }
               catch (Exception ex)
               {
                   GlobusLogHelper.log.Info("Error : " + ex.StackTrace);                  
               }
           }
       }

       private void ExportAccountreport_follow_Click(object sender, RoutedEventArgs e)
       {

       }

       private void chk_DontSendRequest_Follower_Checked(object sender, RoutedEventArgs e)
       {
           try
           {
               obj_follower.chkNotSendRequest = true;
           }
           catch (Exception ex)
           {
               GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
           }
       }
=======
       
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

       
    }
}
