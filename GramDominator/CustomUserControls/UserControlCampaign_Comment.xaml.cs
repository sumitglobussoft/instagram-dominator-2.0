using System;
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
using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using CampaignDetailsManager;
using System.Text.RegularExpressions;
using Globussoft;



namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControlCampaign_Comment.xaml
    /// </summary>
    public partial class UserControlCampaign_Comment : UserControl
    {
        public UserControlCampaign_Comment()
        {
            InitializeComponent();
        }

        private void btn_Campaign_Photoid_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_Campaign_Comment_UserName.Text = dlg.FileName.ToString();
                    ReadFile_campaign_comment(dlg.FileName);
                }

            }
            catch { };
        }

        public void ReadFile_campaign_comment(string commentidFilePath)
        {
            try
            {
                ClGlobul.Campiagn_CommentList.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {

                    ClGlobul.Campiagn_CommentList.Add(commentidlist_item);
                }
                ClGlobul.Campiagn_CommentList = ClGlobul.Campiagn_CommentList.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.Campiagn_CommentList.Count + " Comment  Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        public void ReadFile_campaign_PhotoId(string commentidFilePath)
        {
            try
            {
                ClGlobul.Campiagn_Comment_PhotoIDList.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {

                    ClGlobul.Campiagn_Comment_PhotoIDList.Add(commentidlist_item);
                }
                ClGlobul.Campiagn_Comment_PhotoIDList = ClGlobul.Campiagn_Comment_PhotoIDList.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.Campiagn_Comment_PhotoIDList.Count + " PhotoID  Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_Campaign_Comment_messagebrwer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_Campaign_Comment_Message.Text = dlg.FileName.ToString();
                    ReadFile_campaign_PhotoId(dlg.FileName);
                }

            }
            catch { };
        }


        Utils objUtils = new Utils();
        private void btn_Campaign_Comment_SaveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    if ((string.IsNullOrEmpty(txt_Campaign_Comment_UserName.Text)) || (string.IsNullOrEmpty(txt_Campaign_Comment_Message.Text)) || (string.IsNullOrEmpty(txtMessage_Campaign_comment_NoOfphoto.Text)))
                    {
                        GlobusLogHelper.log.Info("Please Upload Comment Message/No.of photo");
                        ModernDialog.ShowMessage("Please Upload Comment Message/No.Photo", "Upload Message", MessageBoxButton.OK);
                        return;
                    }
                    obj_CampaignDetails.isStopCommentPoster = false;
                    obj_CampaignDetails.lstThreadsCampaingCommentPoster.Clear();

                    Regex checkNo = new Regex("^[0-9]*$");

                    int processorCount = objUtils.GetProcessor();

                    int threads = 25;

                    int maxThread = 25 * processorCount;
                    try
                    {

                        CampaignDetails.No_Photo_Commented = Convert.ToInt32(txtMessage_Campaign_comment_NoOfphoto.Text);
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }



                    CampaignDetails.PhotoCommentCampaignUserPath = txt_Campaign_Comment_UserName.Text;
                    CampaignDetails.PhotoCommentCampaignMessagePath = txt_Campaign_Comment_Message.Text;
                   
                    if ((!string.IsNullOrEmpty(txt_Campaign_Comment_UserName.Text) && (!string.IsNullOrEmpty(txt_Campaign_Comment_Message.Text)) && (!string.IsNullOrEmpty(txtMessage_Campaign_comment_NoOfphoto.Text))))
                    {
                        ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                    }
                }
                else
                {
                    GlobusLogHelper.log.Info("=========================");
                    GlobusLogHelper.log.Info("Please Uplaod Account First");
                    GlobusLogHelper.log.Info("=========================");
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        CampaignDetails obj_CampaignDetails = new CampaignDetails();

        private void btn_Campaign_Comment_Clear_click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_Campaign_Comment_UserName.Text = string.Empty;
                txt_Campaign_Comment_Message.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

    }
    }

