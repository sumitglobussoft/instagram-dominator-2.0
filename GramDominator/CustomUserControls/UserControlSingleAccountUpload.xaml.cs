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
using GramDominator.Pages.PageAccount;

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControlMobilePhones.xaml
    /// </summary>
    public partial class UserControlMobilePhones : UserControl
    {
        public UserControlMobilePhones()
        {
            InitializeComponent();
        }

        QueryManager Qm = new QueryManager();

        public static BaseLib.Events LoadAccounts = new BaseLib.Events();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxButton btnC = MessageBoxButton.OK;

                ManageAccounts obj_Manage_Accounts = new ManageAccounts();
                string singleUsername = string.Empty;
                string siglePassword = string.Empty;
                string singleproxy = string.Empty;
                string path = string.Empty;

                string proxyAddress = string.Empty;
                string proxyPort = string.Empty;
                string proxyUserName = string.Empty;
                string proxyPassword = string.Empty;

                singleUsername = txt_AddSingleAccount_Account.Text;
                siglePassword = txt_AddSingleAccount_Password.Password;

                if (string.IsNullOrEmpty(singleUsername))
                {
                    ModernDialog.ShowMessage("Please Enter Account !", "Message Box ", btnC);

                    txt_AddSingleAccount_Account.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(siglePassword))
                {
                    ModernDialog.ShowMessage("Please Enter Password !", "Message Box ", btnC);

                    txt_AddSingleAccount_Password.Focus();
                    return;

                }

                try
                {

                    proxyAddress = txt_AddSingleAccount_ProxyAddress.Text;

                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
                try
                {

                    proxyPort = txt_AddSingleAccount_ProxyPort.Text;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }

                try
                {

                    proxyUserName = txt_AddSingleAccount_ProxyUsername.Text;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
                try
                {

                    proxyPassword = txt_AddSingleAccount_ProxyPassword.Password;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }

                Qm.AddAccountInDataBase(singleUsername, siglePassword, proxyAddress, proxyPort, proxyUserName, proxyPassword, path);

                obj_Manage_Accounts.LoadAccountsFromDataBase();

                Window parentWindow = (Window)this.Parent;
                parentWindow.Close();

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

        }

        private void btnUserControlClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxButton btnC = MessageBoxButton.YesNoCancel;
                var result = ModernDialog.ShowMessage("Are you want to Clear all text box ?", " Message Box ", btnC);

                if (result == MessageBoxResult.Yes)
                {

                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        txt_AddSingleAccount_Account.Clear();
                        txt_AddSingleAccount_Password.Clear();
                        txt_AddSingleAccount_ProxyAddress.Clear();
                        txt_AddSingleAccount_ProxyPort.Clear();
                        txt_AddSingleAccount_ProxyUsername.Clear();
                        txt_AddSingleAccount_ProxyPassword.Clear();

                    }));

                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
    }
}
