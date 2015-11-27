using BaseLib;
using BaseLibID;
using Comment;
using FirstFloor.ModernUI.Windows.Controls;
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

namespace GramDominator.Pages.Pagecomment
{
    /// <summary>
    /// Interaction logic for UserControlCommentPhoto.xaml
    /// </summary>
    public partial class UserControlCommentPhoto : UserControl
    {
        public UserControlCommentPhoto()
        {
            InitializeComponent();
        }

        private void rdo_CommentInput_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.commentMsgList.Clear();
                ClGlobul.CommentIdsForMSG.Clear();
            }
            catch { }
            try
            {
                btnMessage_Comment_LoadMessages.Visibility = Visibility.Hidden;
                btnMessage_Comment_LoadPhotoID.Visibility = Visibility.Hidden;
               
            }
            catch { };
            try
            {
                txtMessage_Comment_LoadMessages.IsReadOnly = false;
                txtMessage_Comment_PhotoID.IsReadOnly = false;
            }
            catch { };
        }

        private void rdo_CommentInput_MultipleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btnMessage_Comment_LoadMessages.Visibility = Visibility.Visible;
                btnMessage_Comment_LoadPhotoID.Visibility = Visibility.Visible;
               
            }
            catch { };
            try
            {
                txtMessage_Comment_LoadMessages.IsReadOnly = true;
                txtMessage_Comment_PhotoID.IsReadOnly = true;
            }
            catch { };
        }

        private void btnMessage_Comment_LoadMessages_click(object sender, RoutedEventArgs e)
        {
            try
            {
                Comment_idprogress.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txtMessage_Comment_LoadMessages.Text = dlg.FileName.ToString();
                    readcommentFile(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
                 Comment_idprogress.IsIndeterminate = false;
            }
            catch { };
        }


        public void readcommentFile(string commentFilePath)
        {
            try
            {
                ClGlobul.commentMsgList.Clear();
                List<string> MSGlist = GlobusFileHelper.ReadFile((string)commentFilePath);
                foreach (string MSGlist_item in MSGlist)
                {
                    //add Photo Id's In maine photo list...
                    ClGlobul.commentMsgList.Add(MSGlist_item);
                }
                ClGlobul.commentMsgList = ClGlobul.commentMsgList.Distinct().ToList();
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.commentMsgList.Count + " Messages Uploaded. ]");
            }
            catch (Exception ex)
            {
                
            }
        }


        private void btnMessage_Comment_LoadPhotoID_click(object sender, RoutedEventArgs e)
        {
            try
            {
                Comment_idprogress.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txtMessage_Comment_PhotoID.Text = dlg.FileName.ToString();
                    readcommentidFile(dlg.FileName);
                    
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
                Comment_idprogress.IsIndeterminate = false;
            }
            catch { };
        }

        public void readcommentidFile(string commentidFilePath)
        {
            try
            {
                ClGlobul.CommentIdsForMSG.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {
                    //add Comment Id's In Globol Comment Id List ...
                    ClGlobul.CommentIdsForMSG.Add(commentidlist_item);
                }
                ClGlobul.CommentIdsForMSG = ClGlobul.CommentIdsForMSG.Distinct().ToList();
               
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.CommentIdsForMSG.Count + " Image IDs Uploaded. ]");
            }
            catch (Exception ex)
            {
                
            }
        }








        Utils objUtils = new Utils();
        private void btnMessage_Comment_Start_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {

                        if (string.IsNullOrEmpty(txtMessage_Comment_LoadMessages.Text) && string.IsNullOrEmpty(txtMessage_Comment_PhotoID.Text))
                        {
                            GlobusLogHelper.log.Info("Please Upload Comment Message");
                            ModernDialog.ShowMessage("Please Upload Comment Message", "Upload Message", MessageBoxButton.OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                    ObjCommentManager.isStopCommentPoster = false;
                    ObjCommentManager.lstThreadsCommentPoster.Clear();

                    Regex checkNo = new Regex("^[0-9]*$");

                    int processorCount = objUtils.GetProcessor();

                    int threads = 25;

                    int maxThread = 25 * processorCount;
                    try
                    {
                        try
                        {
                            CommentManager.minDelayCommentPoster = Convert.ToInt32(txt_Comment_delaystart.Text);
                            CommentManager.maxDelayCommentPoster = Convert.ToInt32(txt_Comment_delaystop.Text);
                            CommentManager.Nothread_comment = Convert.ToInt32(txt_Comment_thread.Text);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Enter in Correct Format");
                            return;
                        }

                     if(rdo_CommentInput_MultipleUser.IsChecked == true)
                     {
                         CommentManager.CommentPhoto_ID_path = txtMessage_Comment_PhotoID.Text;
                         CommentManager.message_comment_path = txtMessage_Comment_LoadMessages.Text;
                     }
                        if(rdo_CommentInput_SingleUser.IsChecked==true)
                        {
                            CommentManager.CommentPhoto_ID = txtMessage_Comment_PhotoID.Text;
                            CommentManager.message_comment = txtMessage_Comment_LoadMessages.Text;
                        }                                                                 
                       
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                    if (!string.IsNullOrEmpty(txt_Comment_thread.Text) && checkNo.IsMatch(txt_Comment_thread.Text))
                    {
                        threads = Convert.ToInt32(txt_Comment_thread.Text);
                    }

                    if (threads > maxThread)
                    {
                        threads = 25;
                    }
                    ObjCommentManager.NoOfThreadsCommentPoster = threads;


                   
                    Thread CommentPosterThread = new Thread(ObjCommentManager.StartCommentPoster);
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

        CommentManager ObjCommentManager = new CommentManager();

        private void btnMessage_Comment_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ObjCommentManager.isStopCommentPoster = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = ObjCommentManager.lstThreadsCommentPoster.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        ObjCommentManager.lstThreadsCommentPoster.Remove(item);
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

        private void btnMessage_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMessage_Comment_PhotoID.Text = string.Empty;
                txtMessage_Comment_LoadMessages.Text = string.Empty;
                txt_Comment_thread.Text = string.Empty;
                txt_Comment_delaystart.Text = string.Empty;
                txt_Comment_delaystop.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

       
    }
}
