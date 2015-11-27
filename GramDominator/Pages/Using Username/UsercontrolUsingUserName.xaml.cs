using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
using System;
using System.Collections.Generic;
using System.Data;
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
using UsingUsercontrol;

namespace GramDominator.Pages.Using_Username
{
    /// <summary>
    /// Interaction logic for UsercontrolUsingUserName.xaml
    /// </summary>
    public partial class UsercontrolUsingUserName : UserControl
    {
        public UsercontrolUsingUserName()
        {
            InitializeComponent();
            AccountReport_UsingUserName();
        }

        Utils objUtils = new Utils();
        private void btn_UsingUsername_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                objj_UsingUsernameManager.isStopUsingUsername = false;
                objj_UsingUsernameManager.lstThreadsUsingUsername.Clear();

                Regex checkNo = new Regex("^[0-9]*$");

                int processorCount = objUtils.GetProcessor();

                int threads = 25;

                int maxThread = 25 * processorCount;
                try
                {
                    UsingUsernameManager.minDelayUsingUsername = Convert.ToInt32(txt_UsingUsername_DelayMin.Text);
                    UsingUsernameManager.maxDelayUsingUsername = Convert.ToInt32(txt_UsingUsername_DelayMax.Text);
                    UsingUsernameManager.NothreadUsingUsername = Convert.ToInt32(txt_UsingUsername_NoOfThreads.Text);
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Enter in Correct Formate/Fill all Field");
                    ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
                    return;
                }

                if (!string.IsNullOrEmpty(txt_UsingUsername_NoOfThreads.Text) && checkNo.IsMatch(txt_UsingUsername_NoOfThreads.Text))
                {
                    threads = Convert.ToInt32(txt_UsingUsername_NoOfThreads.Text);
                }

                if (threads > maxThread)
                {
                    threads = 25;
                }
                objj_UsingUsernameManager.NoOfThreadsUsingUsername = threads;
                Thread CommentPosterThread = new Thread(objj_UsingUsernameManager.StartUsingUsername);
                CommentPosterThread.Start();
                GlobusLogHelper.log.Info("------ UsingUsrname Proccess Started ------");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        UsingUsernameManager objj_UsingUsernameManager = new UsingUsernameManager();

        private void btn_UsingUsername_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread objStopUsingUsername = new Thread(stopMultiThreadUsingUsername);
                objStopUsingUsername.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void stopMultiThreadUsingUsername()
        {
            try
            {
                objj_UsingUsernameManager.isStopUsingUsername = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = objj_UsingUsernameManager.lstThreadsUsingUsername.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        objj_UsingUsernameManager.lstThreadsUsingUsername.Remove(item);
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

        public void closeEvent()
        { }

        private void Select_To_LikePhoto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Select_To_LikePhoto.SelectedIndex == 1)
                {
                    UsercontrolUsingUsernamelike obj_UserControlLikePhotoByUsername = new UsercontrolUsingUsernamelike();
                    var window = new ModernDialog
                    {
                        Content = obj_UserControlLikePhotoByUsername
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
                        if (string.IsNullOrEmpty(obj_UserControlLikePhotoByUsername.txt_UsingUsername_like.Text))
                        {
                            ModernDialog.ShowMessage("Upload Username", "Upload Username", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter Username");
                            return;
                        }
                        if (obj_UserControlLikePhotoByUsername.rdoBtn_UsingUsername_likeSingle.IsChecked == true)
                        {
                            UsingUsernameManager.UsingUsercontrol_Like_path = string.Empty;
                            UsingUsernameManager.UsingUsercontrol_Like_single = obj_UserControlLikePhotoByUsername.txt_UsingUsername_like.Text;
                        }
                        if (obj_UserControlLikePhotoByUsername.rdoBtn_UsingUsername_likeMultiple.IsChecked == true)
                        {
                            UsingUsernameManager.UsingUsercontrol_Like_single = string.Empty;
                            UsingUsernameManager.UsingUsercontrol_Like_path = obj_UserControlLikePhotoByUsername.txt_UsingUsername_like.Text;
                        }
                        
                        UsingUsernameManager.onlyLike = true;
                        UsingUsernameManager.onlyComment = false;
                        UsingUsernameManager.likeandcomment = false;
                        UsingUsernameManager.UsingUsername_like_Nouser = Convert.ToInt32(obj_UserControlLikePhotoByUsername.txtMessage_UsingUsername_NoOfuser.Text);
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }


                }
                if (Select_To_LikePhoto.SelectedIndex == 2)
                {
                    UsercontrolUsingUsernamecomment obj_UserControlcommentByUsername = new UsercontrolUsingUsernamecomment();
                    var window = new ModernDialog
                    {
                        Content = obj_UserControlcommentByUsername
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                //    window.Topmost = true;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };
                    window.ShowDialog();
                    string s1 = string.Empty;
                    try
                    {
                        if (string.IsNullOrEmpty(obj_UserControlcommentByUsername.txt_UsingUserName_onlycomment_User.Text) && string.IsNullOrEmpty(obj_UserControlcommentByUsername.txt_UsingUsername_onlycommentmessage.Text))
                        {
                            ModernDialog.ShowMessage("Upload Username/Message", "Upload Username/Message", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter Username/Message");
                            return;
                        }
                        if (obj_UserControlcommentByUsername.rdoBtn_UsingUsername_onlycomment_SingleUser.IsChecked == true)
                        {
                            UsingUsernameManager.UsingUsercontrol_onlycomment_path = string.Empty;
                            UsingUsernameManager.UsingUsercontrol_onlycommentmessgae = string.Empty;
                            UsingUsernameManager.UsingUsercontrol_onlycomment_single = obj_UserControlcommentByUsername.txt_UsingUserName_onlycomment_User.Text;
                            UsingUsernameManager.UsingUsercontrol_onlycommentmessage_single = obj_UserControlcommentByUsername.txt_UsingUsername_onlycommentmessage.Text;
                        }
                        if (obj_UserControlcommentByUsername.rdoBtn_UsingUsername_onlycomment_MultipleUser.IsChecked == true)
                        {
                            UsingUsernameManager.UsingUsercontrol_onlycomment_single = string.Empty;
                            UsingUsernameManager.UsingUsercontrol_onlycommentmessage_single = string.Empty;
                            UsingUsernameManager.UsingUsercontrol_onlycomment_path = obj_UserControlcommentByUsername.txt_UsingUserName_onlycomment_User.Text;
                            UsingUsernameManager.UsingUsercontrol_onlycommentmessgae = obj_UserControlcommentByUsername.txt_UsingUsername_onlycommentmessage.Text;
                        }

                        UsingUsernameManager.onlyComment = true;
                        UsingUsernameManager.onlyLike = false;
                            UsingUsernameManager.likeandcomment = false;
                        UsingUsernameManager.UsingUsername_onlycomment_Nouser = Convert.ToInt32(obj_UserControlcommentByUsername.txt_UsingUsername_onlycomment_nouser.Text);
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }


                }
                if (Select_To_LikePhoto.SelectedIndex == 3)
                {
                    UserControlUsingUsernamelikeandcomment obj_UserControllikeandcommentByUsername = new UserControlUsingUsernamelikeandcomment();
                    var window = new ModernDialog
                    {
                        Content = obj_UserControllikeandcommentByUsername
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    //window.Topmost = true;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };
                    window.ShowDialog();
                    string s1 = string.Empty;
                    try
                    {
                        if (string.IsNullOrEmpty(obj_UserControllikeandcommentByUsername.txt_UsingUserName_User.Text) && string.IsNullOrEmpty(obj_UserControllikeandcommentByUsername.txt_UsingUsername_commentmessage.Text))
                        {
                            ModernDialog.ShowMessage("Upload Username/Message", "Upload Username/Message", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter Username/Message");
                            return;
                        }
                        if (obj_UserControllikeandcommentByUsername.rdoBtn_UsingUsername_like_comment_SingleUser.IsChecked == true)
                        {
                            UsingUsernameManager.UsingUsercontrol_Likecomment_User_path = string.Empty;
                            UsingUsernameManager.UsingUsercontrol_Likecomment_message_single_path = string.Empty;
                            UsingUsernameManager.UsingUsercontrol_Likecomment_single = obj_UserControllikeandcommentByUsername.txt_UsingUserName_User.Text;
                            UsingUsernameManager.UsingUsercontrol_Likecomment_message_single = obj_UserControllikeandcommentByUsername.txt_UsingUsername_commentmessage.Text;
                        }
                        if (obj_UserControllikeandcommentByUsername.rdoBtn_UsingUsername_like_comment_MultipleUser.IsChecked == true)
                        {
                            UsingUsernameManager.UsingUsercontrol_Likecomment_single = string.Empty;
                            UsingUsernameManager.UsingUsercontrol_onlycommentmessage_single = string.Empty;
                            UsingUsernameManager.UsingUsercontrol_Likecomment_User_path = obj_UserControllikeandcommentByUsername.txt_UsingUserName_User.Text;
                            UsingUsernameManager.UsingUsercontrol_Likecomment_message_single_path = obj_UserControllikeandcommentByUsername.txt_UsingUsername_commentmessage.Text;
                        }

                        UsingUsernameManager.likeandcomment = true;
                        UsingUsernameManager.onlyLike = false;
                        UsingUsernameManager.onlyComment = false;
                        UsingUsernameManager.UsingUsername_onlycomment_Nouser = Convert.ToInt32(obj_UserControllikeandcommentByUsername.txt_UsingUsername_likecomment_nouser.Text);
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                // GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        QueryManager Qm = new QueryManager();
        public void AccountReport_UsingUserName()
        {
           try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Account_User");
                dt.Columns.Add("UserName");
                dt.Columns.Add("Message");
                dt.Columns.Add("Photo_Id");
                dt.Columns.Add("Status");
                dt.Columns.Add("Operation");
              
                int counter = 0;
                DataSet ds = null;
                try
                {
                    ds = Qm.SelectAccountreport("UsingUser");
                }
                catch(Exception ex)
                {
                    GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                }
                foreach(DataRow ds_item in ds.Tables[0].Rows)
                {
                    try
                    {

                        string Account_User = ds_item.ItemArray[2].ToString();
                        string UserName = ds_item[6].ToString();
                        string Message = ds_item[4].ToString();
                        string Photo_Id = ds_item[3].ToString();
                        string Status = ds_item[7].ToString();
                        string Operation = ds_item[11].ToString();
                        dt.Rows.Add(Account_User,UserName,Message, Photo_Id, Status, Operation);


                    }
                    catch { };


                }

           
                DataView dv;
               
               
                this.Dispatcher.Invoke(new Action(delegate
                {
                    dtGrdUsingUsername_AccountsReport.ItemsSource = dt.DefaultView;

                }));
                
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }


        private void RefreshUsingUsername_UsingUsername_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountReport_UsingUserName();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void DeleteUsingUsername_UsingUsername_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportUsingUsername_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
