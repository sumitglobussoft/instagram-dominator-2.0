using BaseLib;
using BaseLibID;
using DirectMessage;
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

namespace GramDominator.Pages.PageMessage
{
    /// <summary>
    /// Interaction logic for UserControlDirectMessage.xaml
    /// </summary>
    public partial class UserControlDirectMessage : UserControl
    {
        public UserControlDirectMessage()
        {
            InitializeComponent();
        }

        private void rdo_DMInput_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.DM_Messagelist.Clear();
                 ClGlobul.DM_UserList.Clear();
            }
            catch { };
            try
            {
                btnMessage_DirectMessage_LoadMessages.Visibility = Visibility.Hidden;
                btnMessage_DirectMessage_LoadUser.Visibility = Visibility.Hidden;
            }
            catch { };
            try
            {
                txtMessage_DirectMessage_LoadMessages.IsReadOnly = false;
                txtMessage_DirectMessage_LoadUser.IsReadOnly = false;
            }
            catch { };
        }

        private void rdo_DMInput_MultipleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.DM_Messagelist.Clear();
                ClGlobul.DM_UserList.Clear();
            }
            catch { }
            try
            {
                btnMessage_DirectMessage_LoadMessages.Visibility = Visibility.Visible;
                btnMessage_DirectMessage_LoadUser.Visibility = Visibility.Visible;
            }
            catch { };
            try
            {
                txtMessage_DirectMessage_LoadMessages.IsReadOnly = true;
                txtMessage_DirectMessage_LoadUser.IsReadOnly = true;
            }
            catch { };
        }

        private void btnMessage_DirectMessage_LoadMessages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DM_progess.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txtMessage_DirectMessage_LoadMessages.Text = dlg.FileName.ToString();
                    readMessageFile(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
                DM_progess.IsIndeterminate = false;
            }
            catch { };
        }

        public void readMessageFile(string commentidFilePath)
        {
            try
            {
                ClGlobul.DM_Messagelist.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {
                   
                    ClGlobul.DM_Messagelist.Add(commentidlist_item);
                }
                ClGlobul.DM_Messagelist = ClGlobul.DM_Messagelist.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.DM_Messagelist.Count + " Message  Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        Utils objUtils = new Utils();
        private void btnMessage_DirectMessage_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {

                        if (string.IsNullOrEmpty(txtMessage_DirectMessage_LoadMessages.Text) && string.IsNullOrEmpty(txtMessage_DirectMessage_LoadUser.Text))
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
                    objDirectMessage.isStopDMPoster = false;
                    objDirectMessage.lstThreadsDMPoster.Clear();
                    Regex checkNo = new Regex("^[0-9]*$");
                    int processorCount = objUtils.GetProcessor();
                    int threads = 25;
                    int maxThread = 25 * processorCount;
                    try
                    {
                        DirectMessageManager.minDelayDMoster = Convert.ToInt32(txt_Delay_DM_Min.Text);
                        DirectMessageManager.maxDelayDMPoster = Convert.ToInt32(txt_Delay_DM_Max.Text);
                        DirectMessageManager.Nothread_DM = Convert.ToInt32(txt_no_Thread_DM.Text);

                        if (rdo_DMInput_MultipleUser.IsChecked == true)
                        {
                            DirectMessageManager.DM_Message_path = txtMessage_DirectMessage_LoadMessages.Text;
                            DirectMessageManager.UserName_path = txtMessage_DirectMessage_LoadUser.Text;
                        }
                        if (rdo_DMInput_SingleUser.IsChecked == true)
                        {
                            DirectMessageManager.txt_DM_Message = txtMessage_DirectMessage_LoadMessages.Text;
                            DirectMessageManager.txt_UserName = txtMessage_DirectMessage_LoadUser.Text;
                        }

                        DirectMessageManager.Nothread_DM = Convert.ToInt32(txt_no_Thread_DM.Text);
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Enter in Correct Formate");
                            return;
                    }

                    if (!string.IsNullOrEmpty(txt_no_Thread_DM.Text) && checkNo.IsMatch(txt_no_Thread_DM.Text))
                    {
                        threads = Convert.ToInt32(txt_no_Thread_DM.Text);
                    }
                    if (threads > maxThread)
                    {
                        threads = 25;
                    }
                    objDirectMessage.NoOfThreadsDirectmessagePoster = threads;
                    Thread CommentPosterThread = new Thread(objDirectMessage.StartCommentPoster);
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

        DirectMessageManager objDirectMessage = new DirectMessageManager();





        private void btnMessage_DirectMessage_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objDirectMessage.isStopDirectmessagePoster = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = objDirectMessage.lstThreadsDirectmessagePoster.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        objDirectMessage.lstThreadsDirectmessagePoster.Remove(item);
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

        private void btnMessage_DirectMessage_Loaduser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DM_progessuser.IsIndeterminate = true;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txtMessage_DirectMessage_LoadUser.Text = dlg.FileName.ToString();
                    readusernameFile(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
                DM_progessuser.IsIndeterminate = false;
            }
            catch { };
        }

        public void readusernameFile(string commentidFilePath)
        {
            try
            {
                ClGlobul.DM_UserList.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {

                    ClGlobul.DM_UserList.Add(commentidlist_item);
                }
                ClGlobul.DM_UserList = ClGlobul.DM_UserList.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.DM_UserList.Count + " UserName  Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        

        private void btnDM_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMessage_DirectMessage_LoadMessages.Text = string.Empty;
                txtMessage_DirectMessage_LoadUser.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }
    }
}
