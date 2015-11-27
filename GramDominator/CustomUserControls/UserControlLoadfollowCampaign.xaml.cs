using BaseLib;
using BaseLibID;
using CampaignDetailsManager;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
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
    /// Interaction logic for UserControlLoadfollowCampaign.xaml
    /// </summary>
    public partial class UserControlLoadfollowCampaign : UserControl
    {
        public UserControlLoadfollowCampaign()
        {
            InitializeComponent();
        }

        private void btnFollowLoadUserName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClGlobul.campaignfollowingList.Clear();
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    DateTime sTime = DateTime.Now;
                   List<string> templist = GlobusFileHelper.ReadFile(dlg.FileName);

                    foreach (string item in templist)
                    {
                        //ClGlobul.campaignfollowingList.Add(item);
                        CampaignDetails.Text_CampaignFollowPath = dlg.FileName;
                    }
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        txtFollowUsernameLocation.Text = dlg.FileName;
                    }));
                    try
                    {
                        DateTime eTime = DateTime.Now;
                        string timeSpan = (eTime - sTime).TotalSeconds.ToString();
                        GlobusLogHelper.log.Info("Username To Follow Loaded : " + templist.Count() + " In " + timeSpan + " Seconds");
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
               GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }        
        }

        private void btnUsercontrolFollowSaveLoadedData_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (!string.IsNullOrEmpty(txtFollowUsernameLocation.Text) && !string.IsNullOrEmpty(txtFollowCampaignNoOfUserTobeFollow.Text))
                {
                    CampaignDetails.followCampaignFollowUserPath = txtFollowUsernameLocation.Text;
                    int temp = int.Parse(txtFollowCampaignNoOfUserTobeFollow.Text);
                    CampaignDetails.followCampaignNoOfFollowPerAccount = temp;
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Fill All Detail Properly");
                    ModernDialog.ShowMessage("Please Fill All Required Field", "Load User To Follow", MessageBoxButton.OK);
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : "+ex.StackTrace);
            }
        }

        private void rdoFollowInputCampaignDivideEqually_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rdoFollowInputCampaignDivideGivenByUser_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnUsercontrolFollowCleardata_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtFollowUsernameLocation.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
    }
}
