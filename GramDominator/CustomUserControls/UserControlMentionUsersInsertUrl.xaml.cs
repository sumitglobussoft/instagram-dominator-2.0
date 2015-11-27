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
    /// Interaction logic for UserControlMentionUsersInsertUrl.xaml
    /// </summary>
    public partial class UserControlMentionUsersInsertUrl : UserControl
    {
        public UserControlMentionUsersInsertUrl()
        {
            InitializeComponent();
        }

        private void btn_MentionUser_InsertUrl_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txtBlock_MentionUsers_InsertUrl.Text))
                {
                    string url = txtBlock_MentionUsers_InsertUrl.Text;
                    GlobalDeclration.objMentionUser.listOfUrlToMentionUser.Add(url);
                    GlobusLogHelper.log.Info("Message Loaded Successfully");
                    ModernDialog.ShowMessage("Message Loaded Successfully", "Success Message", MessageBoxButton.OK);
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Insert Url");
                    ModernDialog.ShowMessage("Please Insert Url", "Insert Url", MessageBoxButton.OK);
                    txtBlock_MentionUsers_InsertUrl.Focus();
                    return;
                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error ==> " + ex.Message);
            }
        }
    }
}
