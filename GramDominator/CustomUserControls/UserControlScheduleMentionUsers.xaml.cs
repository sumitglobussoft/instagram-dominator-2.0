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
    /// Interaction logic for UserControlScheduleMentionUsers.xaml
    /// </summary>
    public partial class UserControlScheduleMentionUsers : UserControl
    {
        public UserControlScheduleMentionUsers()
        {
            InitializeComponent();
        }

        private void btn_MentionUsers_Scheduler_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime date = DateTime.Now;
                string startTimeHrs = ComboCampaignSchedule_StartHrs.Text;
                string startTimeMin = ComboCampaignSchedule_StartMins.Text;
                
                string StartTime =  startTimeHrs + ":" + startTimeMin + ":" + "00";
                GlobalDeclration.objMentionUser.scheduleStartTime = StartTime;

                string stopTimeHrs = ComboCampaignSchedule_StopHrs.Text;
                string stopTimeMin = ComboCampaignSchedule_StopMins.Text;


                string EndTime = stopTimeHrs + ":" + stopTimeMin + ":" + "00";
                GlobalDeclration.objMentionUser.scheduleEndTime = EndTime;

                if ((!string.IsNullOrEmpty(StartTime)) && (!string.IsNullOrEmpty(EndTime)))
                {
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
    }
}
