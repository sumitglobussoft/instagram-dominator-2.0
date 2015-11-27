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
using FirstFloor.ModernUI.Windows.Controls;
using BaseLibID;

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControlScrapeUsersScrapeFollower.xaml
    /// </summary>
    public partial class UserControlScrapeUsersScrapeFollower : UserControl
    {
        public UserControlScrapeUsersScrapeFollower()
        {
            InitializeComponent();
            BindAccount();
        }

        private void btnSave_ScrapeUsers_ScrapeFollower_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(Txt_ScrapeFolower.Text))
                {
                    GlobalDeclration.objScrapeUser.usernmeToScrape = Txt_ScrapeFolower.Text;
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Enter Username of User To Scrape Follower");
                    ModernDialog.ShowMessage("Please Enter Username of User To Scrape Follower", "Scrape Follower", MessageBoxButton.OK);
                    Txt_ScrapeFolower.Focus();
                    return;
                }
                if(!string.IsNullOrEmpty(cmb_Select_To_Account.Text))
                {
                    GlobalDeclration.objScrapeUser.selectedAccountToScrape = cmb_Select_To_Account.Text;
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Select Account To Scrape Follower");
                    ModernDialog.ShowMessage("Please Select Account To Scrape Follower", "Scrape Follower", MessageBoxButton.OK);
                    cmb_Select_To_Account.Focus();
                    return;
                }
                if (!string.IsNullOrEmpty(Txt_ScrapeUser_ScrapeFollower_NoOfUserToScrape.Text))
                {
                    GlobalDeclration.objScrapeUser.noOfUserToScrape = int.Parse(Txt_ScrapeUser_ScrapeFollower_NoOfUserToScrape.Text);
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Enter No Of User To Scrape Follower");
                    ModernDialog.ShowMessage("Please Enter No Of User To Scrape Follower", "Scrape Follower", MessageBoxButton.OK);
                    Txt_ScrapeUser_ScrapeFollower_NoOfUserToScrape.Focus();
                    return;
                }
                ModernDialog.ShowMessage("Your Data Has Been Saved Successfully", "Success Message", MessageBoxButton.OK);
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);              
            }
        }

        private void BindAccount()
        {
            try
            {
                cmb_Select_To_Account.Items.Clear();
                if (IGGlobals.listAccounts.Count > 0)
                {
                    foreach (var item in IGGlobals.listAccounts)
                    {
                        cmb_Select_To_Account.Items.Add(item.Split(':')[0]);
                    }

                }
                else
                {
                    //
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

        }
    }
}
