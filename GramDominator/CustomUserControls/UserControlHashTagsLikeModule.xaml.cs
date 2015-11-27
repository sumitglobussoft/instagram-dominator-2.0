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
    /// Interaction logic for UserControlHashTagsLikeModule.xaml
    /// </summary>
    public partial class UserControlHashTagsLikeModule : UserControl
    {
        public UserControlHashTagsLikeModule()
        {
            InitializeComponent();
        }

        

        private void rdoBtn_LikeBy_SingleUser_HashsTags_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.HashLiker.Clear();
            }
            catch { }
            try
            {
                btn_HashTags_like_Username_BrowseUsers.Visibility = Visibility.Hidden;
            }
            catch { };
            try
            {
                txt_HashTags_like_Username_LoadUsersPath.IsReadOnly = false;
            }
            catch { };
        }

        private void rdoBtn_HashTags_MultipleUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_HashTags_like_Username_BrowseUsers.Visibility = Visibility.Visible;
            }
            catch { };
            try
            {
                txt_HashTags_like_Username_LoadUsersPath.IsReadOnly = true;
            }
            catch { };
        }

        private void btn_HashTags_like_unlike_User_BrowseUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    txt_HashTags_like_Username_LoadUsersPath.Text = dlg.FileName.ToString();
                    ReadLarge_hashlike(dlg.FileName);
                    //  GlobusLogHelper.log.Info(" [ " + objFollower.lstOfUserIDToFollow.Count + "] UserId Uploaded");
                }
            }
            catch { };
        }
        public void ReadLarge_hashlike(string photoFilename)
        {
            ClGlobul.HashLiker.Clear();
            try
            {
                List<string> photolist = GlobusFileHelper.ReadFile((string)photoFilename);
                foreach (string phoyoList_item in photolist)
                {
                    ClGlobul.HashLiker.Add(phoyoList_item);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.HashLiker.Count + " UserName Uploaded. ]");
            }
            catch (Exception ex)
            {
                
            }
        }

        private void Hashtags_save_Click(object sender, RoutedEventArgs e)
       {
            try
            {
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
                        hash_managerlibry.Hash_Like = true;
                        if (string.IsNullOrEmpty(txt_HashTags_like_Username_LoadUsersPath.Text))
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


                    if (rdoBtn_HashTags_SingleUser.IsChecked == true)
                    {
                        hash_managerlibry.Hash_Like_Unlike_single = txt_HashTags_like_Username_LoadUsersPath.Text;                       
                    }
                    if (rdoBtn_LikeBy_PhotoUser_MultipleUser.IsChecked == true)
                    {
                        hash_managerlibry.Hash_Like_Unlike_path = txt_HashTags_like_Username_LoadUsersPath.Text;                      
                    }
                    hash_managerlibry.hashlike_no_photo = Convert.ToInt32(Txt_Hashlike_count.Text);
                    

                }

                else
                {
                    GlobusLogHelper.log.Info("Please Load Accounts !");
                    GlobusLogHelper.log.Debug("Please Load Accounts !");

                }
                if ((!string.IsNullOrEmpty(txt_HashTags_like_Username_LoadUsersPath.Text)))
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

        private void Hashtags_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_HashTags_like_Username_LoadUsersPath.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

       
    }
}
