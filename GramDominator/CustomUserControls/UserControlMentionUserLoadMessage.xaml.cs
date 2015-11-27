using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
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
    /// Interaction logic for UserControlMentionUserLoadMessage.xaml
    /// </summary>
    public partial class UserControlMentionUserLoadMessage : UserControl
    {
        public UserControlMentionUserLoadMessage()
        {
            InitializeComponent();
        }

        private void btn_MentionUsers_BrowseMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        txt_MentionUser_LoadMessage_MessageFilePath.Text = dlg.FileName;
                    }));

                    List<string> tmpList = Globussoft.GlobusFileHelper.ReadFiletoStringList(dlg.FileName);
                    GlobalDeclration.objMentionUser.listOfMessageToComment = tmpList.Distinct().ToList();

                    GlobusLogHelper.log.Info(tmpList.Count + " Urls Uploaded ");

                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
            }
        }

        private void rdoBtn_MentionUser_LoadMessage_SingleMessage_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_MentionUsers_BrowseMessage.Visibility = Visibility.Hidden;
                lblLoadMessage.Content = "Enter Message : ";
            }
            catch(Exception ex)
            {

            }
        }

        private void rdoBtn_MentionUser_LoadMessage_MultipleMessage_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_MentionUsers_BrowseMessage.Visibility = Visibility.Visible;
                lblLoadMessage.Content = "Load Message : ";
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_MentionUsers_SaveMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(txt_MentionUser_LoadMessage_MessageFilePath.Text))
                {
                    GlobusLogHelper.log.Info("Please Enter Or Load Message File");
                    ModernDialog.ShowMessage("Please Enter Or Load Message File", "Load Message", MessageBoxButton.OK);
                    txt_MentionUser_LoadMessage_MessageFilePath.Focus();
                    return;
                }
                else
                {
                    if(GlobalDeclration.objMentionUser.listOfMessageToComment.Count==0)
                    {
                        if(!string.IsNullOrEmpty(txt_MentionUser_LoadMessage_MessageFilePath.Text))
                        {
                            GlobalDeclration.objMentionUser.listOfMessageToComment.Add(txt_MentionUser_LoadMessage_MessageFilePath.Text);
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
