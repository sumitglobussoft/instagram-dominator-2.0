using BaseLib;
using CampaignDetailsManager;
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
    /// Interaction logic for UserControlSchedul_Comment.xaml
    /// </summary>
    public partial class UserControlSchedul_Comment : UserControl
    {
        public UserControlSchedul_Comment()
        {
            InitializeComponent();
        }

        private void btnCampaignSaveSchedularTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime date = DateTime.Now;
                string startTimeHrs = ComboCampaignSchedule_StartHrs.Text;
                string startTimeMin = ComboCampaignSchedule_StartMins.Text;
                string startingDate = dtPickerCampaign_SchedulerStartDate.Text;
                if (string.IsNullOrEmpty(startingDate))
                {
                    GlobusLogHelper.log.Info("Please Select Starting Date");
                    ModernDialog.ShowMessage("Please Select Starting Date", "Schedular Input", MessageBoxButton.OK);
                    dtPickerCampaign_SchedulerStartDate.Focus();
                    return;
                }
                CampaignDetails.PhotoCommentCampaignStartTime = startingDate + " " + startTimeHrs + ":" + startTimeMin + ":" + "00";

                string stopTimeHrs = ComboCampaignSchedule_StopHrs.Text;
                string stopTimeMin = ComboCampaignSchedule_StopMins.Text;
                string stopDate = dtPickerCampaignSchedule_SchedulerStopDate.Text;

                if (string.IsNullOrEmpty(stopDate))
                {
                    GlobusLogHelper.log.Info("Please Select End Date");
                    ModernDialog.ShowMessage("Please Select End Date", "Schedular Input", MessageBoxButton.OK);
                    dtPickerCampaignSchedule_SchedulerStopDate.Focus();
                    return;
                }

                CampaignDetails.PhotoCommentCampaignEndTime = stopDate + " " + stopTimeHrs + ":" + stopTimeMin + ":" + "00";

                CampaignDetails.PhotoCommentCampaignDelayMin = int.Parse(txtCampaignSchedule_DelayStartFrom.Text);
                CampaignDetails.PhotoCommentCampaignDelayMax = int.Parse(txtCampaignSchedule_DelayStopAt.Text);
                if ((!string.IsNullOrEmpty(startingDate)) && (!string.IsNullOrEmpty(stopDate)))
                {
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void chkCampaignSchedule_ScheduleDaily_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
