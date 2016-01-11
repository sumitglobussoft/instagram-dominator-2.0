using BaseLib;
using BaseLibID;
<<<<<<< HEAD
using FirstFloor.ModernUI.Windows.Controls;
=======
<<<<<<< HEAD
using FirstFloor.ModernUI.Windows.Controls;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
using Globussoft;
using Photo;
using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Data;
=======
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
    /// Interaction logic for UserControlPhotoUserliker.xaml
    /// </summary>
    public partial class UserControlPhotoUserliker : UserControl
    {
        public UserControlPhotoUserliker()
        {
            InitializeComponent();
<<<<<<< HEAD
            AccountReport_likeLiker();
=======
<<<<<<< HEAD
            AccountReport_likeLiker();
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
        }

        private void btu_UserPhoto_liker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserPhoto_liker_progress.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_UserPhoto_liker.Text = dlg.FileName.ToString();
                    ReadLargePhotoFile(dlg.FileName);
                }                
                UserPhoto_liker_progress.IsIndeterminate = false;
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
                    ClGlobul.lstUsername.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.lstUsername.Count + " UserName Uploaded. ]");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void rdo_Userphoto_liker_single_check(object sender, RoutedEventArgs e)
        {
            try
            {
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
                txt_UserPhoto_liker.Clear();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
            }
            try
            {
<<<<<<< HEAD
=======
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
                btu_UserPhoto_liker.Visibility = Visibility.Hidden;
            }
            catch { };
            try
            {
                txt_UserPhoto_liker.IsReadOnly = false;
            }
            catch { }
        }

        private void rdo_Userphoto_liker_multiple_check(object sender, RoutedEventArgs e)
        {
            try
            {
                btu_UserPhoto_liker.Visibility = Visibility.Visible;
            }
            catch { };
            try
            {
                txt_UserPhoto_liker.IsReadOnly = true;
            }
            catch { }
        }


        Utils objUtils = new Utils();
        private void btnMessage_Userphoto_liker_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ObjPhotoManager_new.liker_photo = true;
                ObjPhotoManager_new.isStopLikerPoster = false;
                ObjPhotoManager_new.lstThreadsLikerPoster.Clear();

<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
                if(string.IsNullOrEmpty(txt_UserPhoto_liker.Text))
                {
                    GlobusLogHelper.log.Info("Uplaod Enter Username");
                    ModernDialog.ShowMessage("Upload Enter Username", "Upload Message", MessageBoxButton.OK);
                    return;
                }

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
                    PhotoManager.minDelayLikerPoster = Convert.ToInt32(txt_Userphoto_liker_DelayMin.Text);
                    PhotoManager.mixDelayLikerPoster = Convert.ToInt32(txt_Userphoto_liker_DelayMax.Text);
                    PhotoManager.NoUser_LikerPoster = Convert.ToInt32(txt_Userphoto_liker_NoOfUser.Text);
                    PhotoManager.no_Photo_liker = Convert.ToInt32(txt_Userphoto_liker_nophoto.Text);

                }
                catch (Exception ex)
                {
<<<<<<< HEAD
                    GlobusLogHelper.log.Info("Enter in Correct Formate/Fill all Field");
                    ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
=======
<<<<<<< HEAD
                    GlobusLogHelper.log.Info("Enter in Correct Formate/Fill all Field");
                    ModernDialog.ShowMessage("Enter in Correct Formate/Fill all Field", "Error", MessageBoxButton.OK);
=======
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
                    return;
                }
                if (rdo_Userphoto_liker.IsChecked == true)
                {
<<<<<<< HEAD
                    PhotoManager.txt_username_Liker_Multiple = string.Empty;
=======
<<<<<<< HEAD
                    PhotoManager.txt_username_Liker_Multiple = string.Empty;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
                    PhotoManager.txt_username_Liker_single = txt_UserPhoto_liker.Text;
                }
                if (rdo_Userphoto_liker_multipleUser.IsChecked ==true)
                {
<<<<<<< HEAD
                    PhotoManager.txt_username_Liker_single = string.Empty;
=======
<<<<<<< HEAD
                    PhotoManager.txt_username_Liker_single = string.Empty;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
                    PhotoManager.txt_username_Liker_Multiple = txt_UserPhoto_liker.Text;
                }

                //if (!string.IsNullOrEmpty(txt_Userphoto_liker_NoOfThreads.Text) && checkNo.IsMatch(txt_Userphoto_liker_NoOfThreads.Text))
                //{
                //    threads = Convert.ToInt32(txt_Userphoto_liker_NoOfThreads.Text);
                //}

                if (threads > maxThread)
                {
                    threads = 25;
                }
                ObjPhotoManager_new.NoOfThreadsLikePoster = threads;
<<<<<<< HEAD
                Thread CommentPosterThread = new Thread(ObjPhotoManager_new.StartLikePoster);
                CommentPosterThread.Start();
                GlobusLogHelper.log.Info("------ PhotoUserLiker Proccess Started ------");
=======
<<<<<<< HEAD
                Thread CommentPosterThread = new Thread(ObjPhotoManager_new.StartLikePoster);
                CommentPosterThread.Start();
                GlobusLogHelper.log.Info("------ PhotoUserLiker Proccess Started ------");
=======



                Thread CommentPosterThread = new Thread(ObjPhotoManager_new.StartLikePoster);
                CommentPosterThread.Start();
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

        }
        PhotoManager ObjPhotoManager_new = new PhotoManager();
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        QueryManager Qm = new QueryManager();

        private void btnMessage_Userphoto_liker_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread objStopUserPhoto = new Thread(stopMultiThreadPhotoUserLiker);
                objStopUserPhoto.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void stopMultiThreadPhotoUserLiker()
=======


        private void btnMessage_Userphoto_liker_Stop_Click(object sender, RoutedEventArgs e)
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
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

        private void btnMessage_Userphoto_liker_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_UserPhoto_liker.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :  " + ex.StackTrace);
            }
        }

<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        public void AccountReport_likeLiker()
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
                    ds = Qm.SelectAccountreport("Like_LikerPhoto");
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
                    dtGrdLike_LikerPhoto_AccountsReport.ItemsSource = dt.DefaultView;

                }));

            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void RefreshAccountreport_LikeLiker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccountReport_likeLiker();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void DeleteAccountModule_Likeliker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = ModernDialog.ShowMessage("Are You Sure Delete Data ?? ", "Delete Detail", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        QueryExecuter.deleteQueryforAccountReport("Like_LikerPhoto");
                        IGGlobals.listAccounts.Clear();
                        AccountReport_likeLiker();
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

        private void ExportLikeLiker_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
<<<<<<< HEAD
=======
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master


    }
}
