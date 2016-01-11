using BaseLib;
<<<<<<< HEAD
using BaseLibID;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
using HashTagsManager;
using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Data;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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


namespace GramDominator.Pages.PageHashTags
{
    /// <summary>
    /// Interaction logic for UserControlHashTage.xaml
    /// </summary>
    public partial class UserControlHashTage : UserControl
    {
        public UserControlHashTage()
        {
            InitializeComponent();
<<<<<<< HEAD
            AccountReport_HashTageModule();
        }

        public void closeEvent()
        {

=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        }

        private void Select_To_HashTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Select_To_TagsOption.SelectedIndex == 1)
                {
<<<<<<< HEAD
                    UserControlHashTagsLikeModule obj_UserControlHashTagsLikeModule = new UserControlHashTagsLikeModule();
                    var window = new ModernDialog
                    {
                        Content = obj_UserControlHashTagsLikeModule
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };
                  //  window.Topmost = true;
                    window.ShowDialog();
                    string s1 = string.Empty;
                    try
                    {
                        if (string.IsNullOrEmpty(obj_UserControlHashTagsLikeModule.txt_HashTags_like_Username_LoadUsersPath.Text))
                        {
                            ModernDialog.ShowMessage("Upload Username", "Upload Username", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter Username");
                            return;
                        }
                        if (obj_UserControlHashTagsLikeModule.rdoBtn_HashTags_SingleUser.IsChecked == true)
                        {
                            hash_managerlibry.Hash_Like = true;
                            hash_managerlibry.Hash_Like_Unlike_path = string.Empty;
                            hash_managerlibry.Hash_Like_Unlike_single = obj_UserControlHashTagsLikeModule.txt_HashTags_like_Username_LoadUsersPath.Text;
                            hash_managerlibry.hashlike_no_photo = Convert.ToInt32(obj_UserControlHashTagsLikeModule.Txt_Hashlike_count.Text);

                        }
                        if (obj_UserControlHashTagsLikeModule.rdoBtn_LikeBy_PhotoUser_MultipleUser.IsChecked == true)
                        {
                            hash_managerlibry.Hash_Like = true;
                            hash_managerlibry.Hash_Like_Unlike_single = string.Empty;
                            hash_managerlibry.Hash_Like_Unlike_path = obj_UserControlHashTagsLikeModule.txt_HashTags_like_Username_LoadUsersPath.Text;
                            hash_managerlibry.hashlike_no_photo = Convert.ToInt32(obj_UserControlHashTagsLikeModule.Txt_Hashlike_count.Text);
                        }                       
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                    catch(Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
                    }



                }
                if (Select_To_TagsOption.SelectedIndex == 2)
                {
                    UserControlHashTagsComment obj_UserControlHashTagsComment = new UserControlHashTagsComment();
                    var window = new ModernDialog
                    {
                        Content = obj_UserControlHashTagsComment
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };
                    //window.Topmost = true;
                    window.ShowDialog();
                     string s1 = string.Empty;
                    try
                    {
                        if (string.IsNullOrEmpty(obj_UserControlHashTagsComment.txt_HashTags_Comment_UserName.Text) && string.IsNullOrEmpty(obj_UserControlHashTagsComment.txt_HashTags_Comment_Message.Text))
                        {
                            ModernDialog.ShowMessage("Upload Username", "Upload Username", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter Username");
                            return;
                        }
                        if (obj_UserControlHashTagsComment.rdoBtn_HashTags_Comment_SingleUser.IsChecked == true)
                        {
                            hash_managerlibry.Hash_comment = true;                         
                            hash_managerlibry.Hash_comment_Usernamesingle = obj_UserControlHashTagsComment.txt_HashTags_Comment_UserName.Text;
                            hash_managerlibry.Hash_comment_Messagesingle = obj_UserControlHashTagsComment.txt_HashTags_Comment_Message.Text;
                            hash_managerlibry.Number_Hash_photocomment = Convert.ToInt32(obj_UserControlHashTagsComment.txtMessage_commenthashtag_NoOfphoto.Text);
                            try
                            {
                                hash_managerlibry.Hash_comment_Usernamepath = string.Empty;
                                hash_managerlibry.Hash_comment_Messagepath = string.Empty;
                            }
                            catch { }

                        }
                        if (obj_UserControlHashTagsComment.rdoBtn_HashTags_Comment_MultipleUser.IsChecked == true)
                        {
                            hash_managerlibry.Hash_comment = true;                            
                            hash_managerlibry.Hash_comment_Usernamepath = obj_UserControlHashTagsComment.txt_HashTags_Comment_UserName.Text;
                            hash_managerlibry.Hash_comment_Messagepath = obj_UserControlHashTagsComment.txt_HashTags_Comment_UserName.Text;
                            hash_managerlibry.Number_Hash_photocomment = Convert.ToInt32(obj_UserControlHashTagsComment.txtMessage_commenthashtag_NoOfphoto.Text);
                            try
                            {
                                hash_managerlibry.Hash_comment_Usernamesingle = string.Empty;
                                hash_managerlibry.Hash_comment_Messagesingle = string.Empty;
                            }
                            catch { }
                        }
                        // PhotoManager.IsDownLoadImageUsingHashTag = true;
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                    catch(Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
                    }

                }
                if (Select_To_TagsOption.SelectedIndex == 3)
                {
                    UserControlHashtagsfollower obj_UserControlHashtagsfollower = new UserControlHashtagsfollower();
                    var window = new ModernDialog
                    {
                        Content = obj_UserControlHashtagsfollower
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                   // window.Topmost = true;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };
                    window.ShowDialog();
                    string s1 = string.Empty;
                    try
                    {
                        if (string.IsNullOrEmpty(obj_UserControlHashtagsfollower.txt_HashTags_follower_LoadUsersPath.Text))
                        {
                            ModernDialog.ShowMessage("Upload Username", "Upload Username", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter Username");
                            return;
                        }
                        if (obj_UserControlHashtagsfollower.rdoBtn_HashTags_follower_SingleUser.IsChecked == true)
                        {
                            hash_managerlibry.Hash_Follow = true;
                            hash_managerlibry.Hash_Follower_single = obj_UserControlHashtagsfollower.txt_HashTags_follower_LoadUsersPath.Text;
                            hash_managerlibry.hashFollower_Number = Convert.ToInt32(obj_UserControlHashtagsfollower.txtMessage_hashtagfollower_NoOfuser.Text);
                            try
                            {
                                hash_managerlibry.Hash_Follower_path = string.Empty;
                                
                            }
                            catch { }

                        }
                        if (obj_UserControlHashtagsfollower.rdoBtn_HashTags_follower_MultipleUser.IsChecked == true)
                        {
                            hash_managerlibry.Hash_Follow = true;
                            hash_managerlibry.Hash_Follower_path = obj_UserControlHashtagsfollower.txt_HashTags_follower_LoadUsersPath.Text;
                            hash_managerlibry.hashFollower_Number = Convert.ToInt32(obj_UserControlHashtagsfollower.txtMessage_hashtagfollower_NoOfuser.Text);
                            try
                            {
                                hash_managerlibry.Hash_Follower_single = string.Empty;
                                
                            }
                            catch { }
                        }
                        // PhotoManager.IsDownLoadImageUsingHashTag = true;
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                    catch(Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }


=======
                    var window = new ModernDialog
                    {
                        Content = new UserControlHashTagsLikeModule()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
                if (Select_To_TagsOption.SelectedIndex == 2)
                {
                    var window = new ModernDialog
                    {
                         Content = new UserControlHashTagsComment()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
                if (Select_To_TagsOption.SelectedIndex == 3)
                {
                    var window = new ModernDialog
                    {
                        Content = new UserControlHashtagsfollower()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                }
            }
            catch (Exception ex)
            {
                // GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        Utils objUtils = new Utils();
<<<<<<< HEAD
        QueryManager Qm = new QueryManager();
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        private void btnMessage_HashTags_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                obj_hash_managerlibry.isStopHash_comment = false;
                obj_hash_managerlibry.lstThreadsHash_comment.Clear();

                Regex checkNo = new Regex("^[0-9]*$");

                int processorCount = objUtils.GetProcessor();

                int threads = 25;

                int maxThread = 25 * processorCount;
                try
                {
                    hash_managerlibry.minDelayHash_comment = Convert.ToInt32(txtMessage_HashTags_DelayMin.Text);
                    hash_managerlibry.maxDelayHash_comment = Convert.ToInt32(txtMessage_HashTags_DelayMax.Text);
                    hash_managerlibry.NothreadHash_comment = Convert.ToInt32(txtMessage_HashTags_NoOfThreads.Text);
                }
                catch (Exception ex)
                {
<<<<<<< HEAD
                    GlobusLogHelper.log.Error("Enter in Correct Format/Fill all Field");
                    ModernDialog.ShowMessage("Enter in Correct Format/Fill all Field", "Error", MessageBoxButton.OK);
=======
                    GlobusLogHelper.log.Error("Enter in Correct Format");
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                    return;
                }

                if (!string.IsNullOrEmpty(txtMessage_HashTags_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_HashTags_NoOfThreads.Text))
                {
                    threads = Convert.ToInt32(txtMessage_HashTags_NoOfThreads.Text);
                }

                if (threads > maxThread)
                {
                    threads = 25;
                }
                obj_hash_managerlibry.NoOfThreadsHash_comment = threads;


                if (hash_managerlibry.DivideByUser == true)
                {
                    Thread ForDivideUser = new Thread(obj_hash_managerlibry.StartDivideUser);
                    ForDivideUser.Start();
<<<<<<< HEAD
                    GlobusLogHelper.log.Info("------ HashTag Proccess Started ------");
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                }
                else
                {
                    if (hash_managerlibry.DivideEqual == true)
                    {
                        Thread ForDivideEqual = new Thread(obj_hash_managerlibry.StartDivide);
                        ForDivideEqual.Start();
<<<<<<< HEAD
                        GlobusLogHelper.log.Info("------ HashTag Proccess Started ------");
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                    }
                    else
                    {
                        Thread CommentPosterThread = new Thread(obj_hash_managerlibry.StartHash_comment);
                        CommentPosterThread.Start();
<<<<<<< HEAD
                        GlobusLogHelper.log.Info("------ HashTag Proccess Started ------");
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        hash_managerlibry obj_hash_managerlibry = new hash_managerlibry();
       
        private void btnMessage_HashTags_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
<<<<<<< HEAD
                Thread objStopHashTag = new Thread(stopMultithreadHashTag);
                objStopHashTag.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void stopMultithreadHashTag()
        {
            try
            {
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                obj_hash_managerlibry.isStopHash_comment = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = obj_hash_managerlibry.lstThreadsHash_comment.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        obj_hash_managerlibry.lstThreadsHash_comment.Remove(item);
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
<<<<<<< HEAD

        public void AccountReport_HashTageModule()
        {
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Account_User");
                dt.Columns.Add("Message");
                dt.Columns.Add("Photo_Id");              
                dt.Columns.Add("FollowerName");
                dt.Columns.Add("Status");
                dt.Columns.Add("HashOperation");

                int counter = 0;
                DataSet ds = null;
                try
                {
                    ds = Qm.SelectAccountreport("HashComment");
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
                        string Message = ds_item[4].ToString();
                        string Photo_Id = ds_item[3].ToString();                     
                        string Status = ds_item[7].ToString();
                        string FollowerName = ds_item[5].ToString();
                        string HashOperation = ds_item[10].ToString();
                        dt.Rows.Add(Account_User, Message, Photo_Id,FollowerName, Status, HashOperation);


                    }
                    catch { };


                }
                DataView dv;
                this.Dispatcher.Invoke(new Action(delegate
                {
                    dtGrdHashtag_HashTag_AccountsReport.ItemsSource = dt.DefaultView;

                }));
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void RefreshAccountreport_HashTage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountReport_HashTageModule();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void DeleteAccountModule_HashTag_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = ModernDialog.ShowMessage("Are You Sure Delete Loaded Account ?? ", "Delete Account", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        QueryExecuter.deleteQueryforAccountReport("HashComment");
                        IGGlobals.listAccounts.Clear();
                        AccountReport_HashTageModule();
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void ExportHashTag_Click(object sender, RoutedEventArgs e)
        {
            try
            {

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
