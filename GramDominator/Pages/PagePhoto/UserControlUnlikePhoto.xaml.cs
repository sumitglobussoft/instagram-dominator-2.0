using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
using Photo;
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
            AccountReport_UnlikePhoto();
        }

        public void closeEvent()
        {

        }

        private void Select_To_UnlikePhoto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Select_To_UnlikePhoto.SelectedIndex == 1)
                {
                    UserControlUnlikePhotoByPhotoId obj_UserControlUnlikePhotoByPhotoId =new UserControlUnlikePhotoByPhotoId();
                    var window = new ModernDialog
                    {
                        Content = obj_UserControlUnlikePhotoByPhotoId
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                  //  window.Topmost = true;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };
                    window.ShowDialog();
                     string s1 = string.Empty;
                     try
                     {
                         if (string.IsNullOrEmpty(obj_UserControlUnlikePhotoByPhotoId.txt_UnLikePhoto_Id_LoadUsersPath.Text))
                         {
                             ModernDialog.ShowMessage("Upload PhotoId", "Upload PhotoId", MessageBoxButton.OK);
                             GlobusLogHelper.log.Info("Please Enter PhotoId");
                             return;
                         }
                         if (obj_UserControlUnlikePhotoByPhotoId.rdoBtn_UnLikeBy_PhotoId_SingleUser.IsChecked == true)
                         {
                             PhotoManager.UnLikePhoto_ID_Path = string.Empty;
                             PhotoManager.UnLikePhoto_ID = obj_UserControlUnlikePhotoByPhotoId.txt_UnLikePhoto_Id_LoadUsersPath.Text;
                             
                         }
                         if (obj_UserControlUnlikePhotoByPhotoId.rdoBtn_UnLikeBy_PhotoId_MultipleUser.IsChecked == true)
                         {
                             PhotoManager.UnLikePhoto_ID = string.Empty;
                             PhotoManager.UnLikePhoto_ID_Path = obj_UserControlUnlikePhotoByPhotoId.txt_UnLikePhoto_Id_LoadUsersPath.Text;
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
                if (Select_To_UnlikePhoto.SelectedIndex == 2)
                {
                    UserControlUnlikePhotoByUsername obj_UserControlUnlikePhotoByUsername = new UserControlUnlikePhotoByUsername();
                    var window = new ModernDialog
                    {
                        Content =  obj_UserControlUnlikePhotoByUsername
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                  //  window.Topmost = true;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };
                    window.ShowDialog();
                    string s1 = string.Empty;
                    try
                    {
                        if (string.IsNullOrEmpty(obj_UserControlUnlikePhotoByUsername.txt_UnLikePhoto_Username_LoadUsersPath.Text) && string.IsNullOrEmpty(obj_UserControlUnlikePhotoByUsername.txtMessage_UnLike_NoOfphotousername.Text))
                        {
                            ModernDialog.ShowMessage("Upload UserName", "Upload UserName", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter USerName");
                            return;
                        }
                        if (obj_UserControlUnlikePhotoByUsername.rdoBtn_UnLikeBy_PhotoUser_SingleUser.IsChecked == true)
                        {
                            PhotoManager.UnLikePhoto_Username_Path = string.Empty;
                            PhotoManager.UnLikePhoto_UserName = obj_UserControlUnlikePhotoByUsername.txt_UnLikePhoto_Username_LoadUsersPath.Text;
                        }
                        if (obj_UserControlUnlikePhotoByUsername.rdoBtn_UnLikeBy_PhotoUser_MultipleUser.IsChecked == true)
                        {
                            PhotoManager.UnLikePhoto_UserName = string.Empty;
                            PhotoManager.UnLikePhoto_Username_Path = obj_UserControlUnlikePhotoByUsername.txt_UnLikePhoto_Username_LoadUsersPath.Text;
                        }
                        // PhotoManager.IsDownLoadImageUsingHashTag = true;
                        PhotoManager.no_photounlike_Username = Convert.ToInt32(obj_UserControlUnlikePhotoByUsername.txtMessage_UnLike_NoOfphotousername.Text);
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                    catch(Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
                    }






                }
            }
            catch (Exception ex)
            {
                // GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        Utils objUtils = new Utils();
        QueryManager Qm = new QueryManager();

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
                    GlobusLogHelper.log.Info("Enter in Correct Formate/Fill all Field");
                    ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
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
                GlobusLogHelper.log.Info("------ UnlikePhoto Proccess Started ------");
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
                Thread objStopUnlLike= new Thread(stopMultiThreadUnLike);
                objStopUnlLike.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void stopMultiThreadUnLike()
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

        public void AccountReport_UnlikePhoto()
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
                    ds = Qm.SelectAccountreport("PhotoUnlikeModule");
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
                    dtGrdPhotoUnlike_AccountsReport.ItemsSource = dt.DefaultView;

                }));
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        private void RefreshAccountreport_PhotoUnLike_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountReport_UnlikePhoto();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        private void DeleteAccountModule_PhotoUnLike_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = ModernDialog.ShowMessage("Are You Sure Delete Data ?? ", "Delete Detail", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        QueryExecuter.deleteQueryforAccountReport("PhotoUnlikeModule");
                        IGGlobals.listAccounts.Clear();
                        AccountReport_UnlikePhoto();
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        private void ExportPhotoUnLike_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }


    }
}
