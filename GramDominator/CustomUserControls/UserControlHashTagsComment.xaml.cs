using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using HashTagsManager;
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

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControlHashTagsComment.xaml
    /// </summary>
    public partial class UserControlHashTagsComment : UserControl
    {
        public UserControlHashTagsComment()
        {
            InitializeComponent();
        }

        private void rdoBtn_HashTagsComment_SingleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.HashComment.Clear();
                ClGlobul.HashCommentMessage.Clear();
            }
            catch { }
            try
            {
                btn_HashTags_Comment_BrowseUsers.Visibility = Visibility.Hidden;
                btn_HashTags_Comment_messagebrwer.Visibility = Visibility.Hidden;
               
            }
            catch { };
            try
            {
                txt_HashTags_Comment_Message.IsReadOnly = false;
                txt_HashTags_Comment_UserName.IsReadOnly = false;
               
            }
            catch { };
        }

        private void rdoBtn_HashTagsComment_MultipleUser(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_HashTags_Comment_BrowseUsers.Visibility = Visibility.Visible;
                btn_HashTags_Comment_messagebrwer.Visibility = Visibility.Visible;

            }
            catch { };
            try
            {
                txt_HashTags_Comment_UserName.IsReadOnly = true ;
                txt_HashTags_Comment_Message.IsReadOnly = true;

            }
            catch { };
        }

        private void btn_HashTagsComment_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_HashTags_Comment_UserName.Text = dlg.FileName.ToString();
                    readHash_comment(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
            }
            catch { };
        }

        public void readHash_comment(string commentidFilePath)
        {
            try
            {
                ClGlobul.HashComment.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {

                    ClGlobul.HashComment.Add(commentidlist_item);
                }
                ClGlobul.HashComment = ClGlobul.HashComment.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.HashComment.Count + " Message  Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

                
        private void btn_HashTagsComment_SaveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
                        hash_managerlibry.Hash_comment = true;
                        if (string.IsNullOrEmpty(txt_HashTags_Comment_UserName.Text) && string.IsNullOrEmpty(txt_HashTags_Comment_Message.Text))
                        {
                            GlobusLogHelper.log.Info("Please Upload Photo ID");
                            ModernDialog.ShowMessage("Please Upload Photo Id", "Upload Message", MessageBoxButton.OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }


                    if (rdoBtn_HashTags_Comment_SingleUser.IsChecked == true)
                    {
                        hash_managerlibry.Hash_comment_Usernamesingle = txt_HashTags_Comment_UserName.Text;
                        hash_managerlibry.Hash_comment_Messagesingle = txt_HashTags_Comment_Message.Text;

                    }
                    if (rdoBtn_HashTags_Comment_MultipleUser.IsChecked == true)
                    {
                        hash_managerlibry.Hash_comment_Usernamepath = txt_HashTags_Comment_UserName.Text;
                        hash_managerlibry.Hash_comment_Messagepath = txt_HashTags_Comment_Message.Text;
                        
                    }
                    hash_managerlibry.Number_Hash_photocomment = Convert.ToInt32(txtMessage_commenthashtag_NoOfphoto.Text);

                }

                else
                {
                    GlobusLogHelper.log.Info("Please Load Accounts !");
                    GlobusLogHelper.log.Debug("Please Load Accounts !");

                }
                if ((!string.IsNullOrEmpty(txt_HashTags_Comment_UserName.Text)) && (!string.IsNullOrEmpty(txt_HashTags_Comment_Message.Text)) && (!string.IsNullOrEmpty(txtMessage_commenthashtag_NoOfphoto.Text)))
                {
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        hash_managerlibry objj_hash_managerlibry = new hash_managerlibry();

        private void btn_HashTagsComment_messagebrwer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_HashTags_Comment_Message.Text = dlg.FileName.ToString();
                    readMessage_commenthash(dlg.FileName);
                }
                //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
            }
            catch { };
        }

        public void readMessage_commenthash(string commentidFilePath)
        {
            try
            {
                ClGlobul.HashCommentMessage.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {

                    ClGlobul.HashCommentMessage.Add(commentidlist_item);
                }
                ClGlobul.HashCommentMessage = ClGlobul.HashCommentMessage.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.HashCommentMessage.Count + " Message  Uploaded. ]");
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_HashTagsComment_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_HashTags_Comment_UserName.Text = string.Empty;
                txt_HashTags_Comment_Message.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
    }
}
