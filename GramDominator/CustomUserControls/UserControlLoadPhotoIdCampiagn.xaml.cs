using BaseLib;
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
    /// Interaction logic for UserControlLoadPhotoIdCampiagn.xaml
    /// </summary>
    public partial class UserControlLoadPhotoIdCampiagn : UserControl
    {
        public UserControlLoadPhotoIdCampiagn()
        {
            InitializeComponent();
        }

        private void btnPhotolikeLoadUserName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                        CampaignDetails.CampaignPhotoLike.txt_PhotoIdCampaign = dlg.FileName;
                    }
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        txtPhotolikeUsernameLocation.Text = dlg.FileName;
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

        private void btnUsercontrolPhotolikeSaveLoadedData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtPhotolikeUsernameLocation.Text) && !string.IsNullOrEmpty(txtPhotolikeCampaignNoOfUserTobelike.Text))
                {
                    CampaignDetails.PhotoLikeCampaignPhotoLikeUserPath = txtPhotolikeUsernameLocation.Text;
                    int temp = int.Parse(txtPhotolikeCampaignNoOfUserTobelike.Text);
                    CampaignDetails.PhotoLikeCampaignNoOfPhotLikePerAccount = temp;
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Fill All Detail Properly");
                    ModernDialog.ShowMessage("Please Fill All Required Field", "Load User To Photo like", MessageBoxButton.OK);
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void btnUsercontrolPhotolikeClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtPhotolikeUsernameLocation.Text = string.Empty;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
    }
}
