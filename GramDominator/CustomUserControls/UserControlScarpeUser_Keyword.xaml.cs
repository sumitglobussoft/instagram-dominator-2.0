using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Scraping;
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
    /// Interaction logic for UserControlScarpeUser_Keyword.xaml
    /// </summary>
    public partial class UserControlScarpeUser_Keyword : UserControl
    {
        public UserControlScarpeUser_Keyword()
        {
            InitializeComponent();
        }

        private void btn_ScrapeUsername_keyword_SaveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Selected_item = string.Empty;
                if (IGGlobals.listAccounts.Count > 0)
                {
                    try
                    {
                        Selected_item = ScrapeUser_keyword_slect.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ","");
                        
                        if (string.IsNullOrEmpty(Selected_item) && string.IsNullOrEmpty(txtMessage_UserName_keyword_NoOfUser.Text))
                        {
                            GlobusLogHelper.log.Info("Please Fill All Detail");
                            ModernDialog.ShowMessage("Please Fill All Detail", "Upload Message", MessageBoxButton.OK);
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }



                    ScrapingManager.User_key = Selected_item;
                    ScrapingManager.No_UserCount_keyword = Convert.ToInt32(txtMessage_UserName_keyword_NoOfUser.Text);
                    ScrapingManager.UserScraper_UserByKeyword = true;
                }


                else
                {
                    GlobusLogHelper.log.Info("Please Load Accounts !");
                    GlobusLogHelper.log.Debug("Please Load Accounts !");

                }
                if ((!string.IsNullOrEmpty(Selected_item)) && (!string.IsNullOrEmpty(txtMessage_UserName_keyword_NoOfUser.Text)))
                {
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        ScrapingManager Obj_Scraping_key = new ScrapingManager();

        private void btn_ScrapeUserName_keyword_Submit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
