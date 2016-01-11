using BaseLib;
<<<<<<< HEAD
using BaseLibID;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
using Photo;
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

namespace GramDominator.Pages.PagePhoto
{
    /// <summary>
    /// Interaction logic for UserControlLikePhoto.xaml
    /// </summary>
    public partial class UserControlLikePhoto : UserControl
    {
        public UserControlLikePhoto()
        {
            InitializeComponent();
<<<<<<< HEAD
            AccountReport_PhotoLike();
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        }


        Utils objUtils = new Utils();
<<<<<<< HEAD
        QueryManager Qm = new QueryManager();
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        private void btnMessage_Like_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ObjPhotoManager_new.Like = true;
                    ObjPhotoManager_new.isStopLikePoster = false;
                    ObjPhotoManager_new.lstThreadsLikePoster.Clear();

                    Regex checkNo = new Regex("^[0-9]*$");

                    int processorCount = objUtils.GetProcessor();

                    int threads = 25;

                    int maxThread = 25 * processorCount;
                    try
                    {
                        PhotoManager.minDelayLikePoster = Convert.ToInt32(txtMessage_Like_DelayMin.Text);
                        PhotoManager.maxDelayLikePoster = Convert.ToInt32(txtMessage_Like_DelayMax.Text);                        
                        PhotoManager.Nothread_LikePoster = Convert.ToInt32(txtMessage_Like_NoOfThreads.Text);
                    }
                    catch (Exception ex)
                    {
<<<<<<< HEAD
                        GlobusLogHelper.log.Error("Enter in Correct Formate/Fill all Field");
                        ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
=======
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtMessage_Like_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_Like_NoOfThreads.Text))
                    {
                        threads = Convert.ToInt32(txtMessage_Like_NoOfThreads.Text);
                    }

                    if (threads > maxThread)
                    {
                        threads = 25;
                    }
                    ObjPhotoManager_new.NoOfThreadsLikePoster = threads;
<<<<<<< HEAD
                    Thread CommentPosterThread = new Thread(ObjPhotoManager_new.StartLikePoster);
                    CommentPosterThread.Start();
                    GlobusLogHelper.log.Info("------ LikePhoto Proccess Started ------");
=======



                    Thread CommentPosterThread = new Thread(ObjPhotoManager_new.StartLikePoster);
                    CommentPosterThread.Start();
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
                
            }
        PhotoManager ObjPhotoManager_new = new PhotoManager();
        

        private void btnMessage_Like_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
<<<<<<< HEAD
                Thread objStopLikePhoto = new Thread(stopMultiThreadLikePhoto);
                objStopLikePhoto.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void stopMultiThreadLikePhoto()
        {
            try
            {
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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

<<<<<<< HEAD

        public void closeEvent()
        {


        }

=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        private void Select_To_LikePhoto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Select_To_LikePhoto.SelectedIndex == 1)
                {
<<<<<<< HEAD
                    UserControlLikePhotoByID obj_UserControlLikePhotoByID = new UserControlLikePhotoByID();
                    var window = new ModernDialog
                    {
                        Content = obj_UserControlLikePhotoByID
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
                        if (string.IsNullOrEmpty(obj_UserControlLikePhotoByID.txt_LikePhoto_Id_LoadUsersPath.Text))
                        {
                            ModernDialog.ShowMessage("Upload PhotoId", "Upload PhotoId", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter PhotoId");
                            return;
                        }
                        if (obj_UserControlLikePhotoByID.rdoBtn_LikeBy_PhotoId_SingleUser.IsChecked == true)
                        {
                            PhotoManager.LikePhoto_ID_path = string.Empty;
                            PhotoManager.LikePhoto_ID = obj_UserControlLikePhotoByID.txt_LikePhoto_Id_LoadUsersPath.Text;
                        }
                        if (obj_UserControlLikePhotoByID.rdoBtn_LikeBy_PhotoId_MultipleUser.IsChecked == true)
                        {
                            PhotoManager.LikePhoto_ID = string.Empty;
                            PhotoManager.LikePhoto_ID_path = obj_UserControlLikePhotoByID.txt_LikePhoto_Id_LoadUsersPath.Text;
                        }
                       // PhotoManager.IsDownLoadImageUsingHashTag = true;
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                     catch(Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }
                }
                if (Select_To_LikePhoto.SelectedIndex == 2)
                {
                    UserControlLikePhotoByUserName obj_UserControlLikePhotoByUserName = new UserControlLikePhotoByUserName();
                    var window = new ModernDialog
                    {
                        Content = obj_UserControlLikePhotoByUserName
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
                        if (string.IsNullOrEmpty(obj_UserControlLikePhotoByUserName.txt_LikePhoto_Username_LoadUsersPath.Text) && string.IsNullOrEmpty(obj_UserControlLikePhotoByUserName.txtMessage_Like_NoOfFollowers.Text))
                        {
                            ModernDialog.ShowMessage("Upload Username And No.Photo", "Upload Username/Count", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter Username");
                            return;
                        }
                        if (obj_UserControlLikePhotoByUserName.rdoBtn_LikeBy_PhotoUser_SingleUser.IsChecked == true)
                        {
                            PhotoManager.LikePhoto_username_path = string.Empty;
                            PhotoManager.LikePhoto_Username = obj_UserControlLikePhotoByUserName.txt_LikePhoto_Username_LoadUsersPath.Text;
                        }
                        if (obj_UserControlLikePhotoByUserName.rdoBtn_LikeBy_PhotoUser_MultipleUser.IsChecked == true)
                        {
                            PhotoManager.LikePhoto_Username = string.Empty;
                            PhotoManager.LikePhoto_ID_path = obj_UserControlLikePhotoByUserName.txt_LikePhoto_Username_LoadUsersPath.Text;
                        }
                        // PhotoManager.IsDownLoadImageUsingHashTag = true;
                        PhotoManager.noPhotoLike_username = Convert.ToInt32(obj_UserControlLikePhotoByUserName.txtMessage_Like_NoOfFollowers.Text);
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
               GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void AccountReport_PhotoLike()
        {
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Account_User");
                dt.Columns.Add("Photo_Id");
                dt.Columns.Add("DateTime");
                dt.Columns.Add("Status");

                int counter = 0;
                DataSet ds = null;
                try
                {
                    ds = Qm.SelectAccountreport("PhotoLikeModule");
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
                        string Photo_Id = ds_item[3].ToString();
                        string DateTime = ds_item[12].ToString();
                        string Status = ds_item[7].ToString();
                        dt.Rows.Add(Account_User, Photo_Id,DateTime,Status);
                    }
                    catch { };


                }
                DataView dv;
                this.Dispatcher.Invoke(new Action(delegate
                {
                    dtGrdLikePhoto_AccountsReport.ItemsSource = dt.DefaultView;

                }));

            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        private void RefreshAccountreport_PhotoLike_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountReport_PhotoLike();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void DeleteAccountModule_PhotoLike_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 var result = ModernDialog.ShowMessage("Are You Sure Delete Data ?? ", "Delete Detail", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        QueryExecuter.deleteQueryforAccountReport("PhotoLikeModule");
                        IGGlobals.listAccounts.Clear();
                        AccountReport_PhotoLike();
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

        private void ExportPhotoLike_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
=======
                    var window = new ModernDialog
                    {
                        Content = new UserControlLikePhotoByID()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
                if (Select_To_LikePhoto.SelectedIndex == 2)
                {
                    var window = new ModernDialog
                    {
                        Content = new UserControlLikePhotoByUserName()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
            }
            catch (Exception ex)
            {
               // GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
            }
        }

        
    }
}
