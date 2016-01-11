using BaseLib;
using BaseLibID;
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
    /// Interaction logic for UserControlScrapeUserScrapeFollowing.xaml
    /// </summary>
    public partial class UserControlScrapeUserScrapeFollowing : UserControl
    {
        public UserControlScrapeUserScrapeFollowing()
        {
            InitializeComponent();
            BindAccount();
        }

        private void btnSave_ScrapeUsers_ScrapeFollowing_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_ScrapeFolowing.Text))
                {
                    GlobalDeclration.objScrapeUser.usernmeToScrape = Txt_ScrapeFolowing.Text;
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Enter Username of User To Scrape Following");
                    ModernDialog.ShowMessage("Please Enter Username of User To Scrape Following", "Scrape Following", MessageBoxButton.OK);
                    Txt_ScrapeFolowing.Focus();
                    return;
                }
                //if (!string.IsNullOrEmpty(cmb_Select_To_Account.Text))
                //{
                //    GlobalDeclration.objScrapeUser.selectedAccountToScrape = cmb_Select_To_Account.Text;
                //}
                //else
                //{
                //    GlobusLogHelper.log.Info("Please Select Account To Scrape Following");
                //    ModernDialog.ShowMessage("Please Select Account To Scrape Following", "Scrape Following", MessageBoxButton.OK);
                //    cmb_Select_To_Account.Focus();
                //    return;
                //}
                try
                {
                    List<CheckBox> tempListOfAccount = new List<CheckBox>();
                    foreach (CheckBox item in cmb_Select_To_Account.Items)
                    {
                        tempListOfAccount.Add(item);
                    }
                    if (tempListOfAccount.Count > 0)
                    {
                        tempListOfAccount = tempListOfAccount.Where(x => x.IsChecked == true).ToList();
                        if (tempListOfAccount.Count == 0)
                        {
                            GlobusLogHelper.log.Info("Please Select Account From List");
                            ModernDialog.ShowMessage("Please Select Account From List", "Select Account", MessageBoxButton.OK);
                            cmb_Select_To_Account.Focus();
                            return;
                        }
                        else
                        {
                            foreach (CheckBox checkedItem in tempListOfAccount)
                            {
                                if (checkedItem.IsChecked == true)
                                {
                                    GlobalDeclration.objScrapeUser.selectedAccountToScrape.Add(checkedItem.Content.ToString());
                                }
                            }
                            GlobusLogHelper.log.Info(GlobalDeclration.objScrapeUser.selectedAccountToScrape.Count + " Account Selected");
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                }
                if (!string.IsNullOrEmpty(Txt_ScrapeUser_ScrapeFollowing_NoOfUserToScrape.Text))
                {
                    GlobalDeclration.objScrapeUser.noOfUserToScrape = int.Parse(Txt_ScrapeUser_ScrapeFollowing_NoOfUserToScrape.Text);
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Enter No Of User To Scrape Following");
                    ModernDialog.ShowMessage("Please Enter No Of User To Scrape Following", "Scrape Following", MessageBoxButton.OK);
                    Txt_ScrapeUser_ScrapeFollowing_NoOfUserToScrape.Focus();
                    return;
                }
                ModernDialog.ShowMessage("Your Data Has Been Saved Successfully", "Success Message", MessageBoxButton.OK);
            }
            catch (Exception ex)
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
