using BaseLib;
using BaseLibID;
<<<<<<< HEAD
using FirstFloor.ModernUI.Windows.Controls;
=======
<<<<<<< HEAD
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using GramDominator.CustomUserControls;
using Photo;
using System;
using System.Collections.Generic;
using System.Data;
=======
>>>>>>> origin/master
using Globussoft;
using Photo;
using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Data;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
            AccountReport_Downloadimage();
        }

      

     

     
        Utils objUtils = new Utils();
        QueryManager Qm = new QueryManager();
<<<<<<< HEAD
=======
=======
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
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
        private void btnMessage_downloadphoto_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    ObjPhotoManager_new.DownloadPhoto = true;
                    ObjPhotoManager_new.isStopDwonloadPoster = false;
                    ObjPhotoManager_new.lstThreadsDwonloadPoster.Clear();

<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master

                    //if(string.IsNullOrEmpty(txt_downloadphoto_Username.Text))
                    //{
                    //    ModernDialog.ShowMessage("Upload Username", "Upload Message", MessageBoxButton.OK);
                    //    return;
                    //}

<<<<<<< HEAD
=======
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
                        GlobusLogHelper.log.Info("Enter in Correct Formate/Fill all Field");
                        ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
                        return;
                    }
                    
<<<<<<< HEAD
=======
=======
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
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
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
<<<<<<< HEAD
                    GlobusLogHelper.log.Info("------ DownloadPhoto Proccess Started ------");
=======
<<<<<<< HEAD
                    GlobusLogHelper.log.Info("------ DownloadPhoto Proccess Started ------");
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
                Thread objStopDownloadPhoto = new Thread(stopMultiThreadDownloadPhoto);
                objStopDownloadPhoto.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void stopMultiThreadDownloadPhoto()
        {
            try
            {
<<<<<<< HEAD
=======
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
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
<<<<<<< HEAD
              //  txt_downloadphoto_Username.Text = string.Empty;
=======
<<<<<<< HEAD
              //  txt_downloadphoto_Username.Text = string.Empty;
=======
                txt_downloadphoto_Username.Text = string.Empty;
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        public void AccountReport_Downloadimage()
        {
            try
            {
                 
                DataTable dt = new DataTable();

                dt.Columns.Add("Account_User");
                dt.Columns.Add("UserName");
                dt.Columns.Add("Photo_Id");
                dt.Columns.Add("DateTime");
                dt.Columns.Add("Status");

                int counter = 0;
                DataSet ds = null;
                try
                {
                    ds = Qm.SelectAccountreport("DownloadImage");
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
                        string UserName = ds_item[6].ToString();
                        string Photo_Id = ds_item[3].ToString();
                        string DateTime = ds_item[12].ToString();
                        string Status = ds_item[7].ToString();
                        dt.Rows.Add(Account_User, UserName,Photo_Id,DateTime,Status);


                    }
                    catch { };


                }
                DataView dv;
                this.Dispatcher.Invoke(new Action(delegate
                {
                    dtGrdDownloadImage_AccountsReport.ItemsSource = dt.DefaultView;

                }));             
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void RefreshAccountreport_DownloadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountReport_Downloadimage();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void DeleteAccountModule_DownloadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = ModernDialog.ShowMessage("Are You Sure Delete Data ?? ", "Delete Detail", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        QueryExecuter.deleteQueryforAccountReport("DownloadImage");
                        IGGlobals.listAccounts.Clear();
                        AccountReport_Downloadimage();
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                    }
                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        private void ExportDownloadImage_Click(object sender, RoutedEventArgs e)
        {

        }

        public void closeEvent()
        {


        }

        private void combo_selectoption_Downloadphoto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                if (combo_selectoption_Downloadphoto.SelectedIndex == 1)
                {
                   
                    PhotoDownload_username obj = new PhotoDownload_username();
                    var window = new ModernDialog
                    {

                        Content = obj
                    };
                    window.ShowInTaskbar = true;
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };

                    window.ShowDialog();

                    string s1 = string.Empty;

                    try
                    {
                        if (string.IsNullOrEmpty(obj.txt_downloadphoto_Username.Text))
                        {
                            ModernDialog.ShowMessage("Upload Username", "Upload Message", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter Username");
                            return;
                        }
                        if (obj.rdo_DownloadSingle_Byusername.IsChecked == true)
                        {
                            PhotoManager.UserphotoDownload_Single = obj.txt_downloadphoto_Username.Text;
                        }
                        if (obj.rdo_DownloadMultiple_Byusername.IsChecked == true)
                        {
                            PhotoManager.UserphotoDownload_Multiple = obj.txt_downloadphoto_Username.Text;
                        }
                        PhotoManager.IsDownLoadImageUsingUserName = true;
                        ModernDialog.ShowMessage("Notice", "Data Successfully Save", MessageBoxButton.OK);
                        GlobusLogHelper.log.Info("Your Data Successfully Save");

                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }


                }
                if (combo_selectoption_Downloadphoto.SelectedIndex == 2)
                {
                    //var window = new ModernDialog
                    //{
                    //     Content = new UserControlPhotoDownload_Hashtag()
                    //};
                    //window.MinHeight = 300;
                    //window.MinWidth = 700;
                    //window.Topmost = true;
                    //window.ShowDialog();

                    UserControlPhotoDownload_Hashtag obj_UserControlPhotoDownload_Hashtag = new UserControlPhotoDownload_Hashtag();
                    var window = new ModernDialog
                    {

                        Content = obj_UserControlPhotoDownload_Hashtag
                    };
                    window.ShowInTaskbar = true;
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    Button customButton = new Button() { Content = "Save" };
                    customButton.Click += (ss, ee) => { closeEvent(); window.Close(); };
                    window.Buttons = new Button[] { customButton };

                    window.ShowDialog();

                    string s1 = string.Empty;
                    try
                    {
                        if (string.IsNullOrEmpty(obj_UserControlPhotoDownload_Hashtag.txt_downloadphoto_Hashtag.Text))
                        {
                              ModernDialog.ShowMessage("Upload Username", "Upload Message", MessageBoxButton.OK);
                            GlobusLogHelper.log.Info("Please Enter Username");
                            return;
                        }

                        if (obj_UserControlPhotoDownload_Hashtag.rdo_DownloadSingle_ByHashtag.IsChecked == true)
                        {
                            PhotoManager.UserphotoDownload_Single = obj_UserControlPhotoDownload_Hashtag.txt_downloadphoto_Hashtag.Text;
                        }
                        if (obj_UserControlPhotoDownload_Hashtag.rdo_DownloadMultiple_ByHashtag.IsChecked == true)
                        {
                            PhotoManager.UserphotoDownload_Multiple = obj_UserControlPhotoDownload_Hashtag.txt_downloadphoto_Hashtag.Text;
                        }
                        PhotoManager.IsDownLoadImageUsingHashTag = true;
                        GlobusLogHelper.log.Info("Your Data Successfully Save");
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }



                }
            }
             catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
            }
        }
<<<<<<< HEAD
=======
=======




>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master




    }
}
