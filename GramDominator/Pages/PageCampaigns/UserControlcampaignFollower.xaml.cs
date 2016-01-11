using BaseLib;
using BaseLibID;
using CampaignDetailsManager;
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GramDominator.Pages.PageCampaigns
{
    /// <summary>
    /// Interaction logic for UserControlcampaignFollower.xaml
    /// </summary>
    public partial class UserControlcampaignFollower : UserControl
    {
        public UserControlcampaignFollower()
        {
            InitializeComponent();
            showToGridViewCampaign();
            BindAccount();
            
        }
        QueryManager Qm = new QueryManager();




        private void BindAccount()
        {
            try
            {
                cmb_Follow_Account.Items.Clear();
                if (IGGlobals.listAccounts.Count > 0)
                {
                    foreach (var item in IGGlobals.listAccounts)
                    {
                        cmb_Follow_Account.Items.Add(item.Split(':')[0]);
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

        private void btnFollowCampaignSave_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (string.IsNullOrEmpty(txt_followCampaignName.Text))
                {
                    GlobusLogHelper.log.Info("Campaign Name Should not be Empty, Please Enter Campaign Name !");
                    ModernDialog.ShowMessage("Campaign Name Should not be Empty, Please Enter Campaign Name !", "Campaign Send Message", MessageBoxButton.OK);
                    txt_followCampaignName.Focus();
                    return;
                }
                else
                {
                    CampaignDetails.followCampaignName = txt_followCampaignName.Text;
                }
                if (string.IsNullOrEmpty(cmb_Follow_Account.Text))
                {
                    GlobusLogHelper.log.Info("Please Select Account !");
                    ModernDialog.ShowMessage("Please Select Account !", "Campaign Send Message", MessageBoxButton.OK);
                    cmb_Follow_Account.Focus();
                    return;
                }
                else
                {
                    CampaignDetails.followCampaignAccount = cmb_Follow_Account.Text;
                }
                DataSet ds = Qm.SelectCampaignName("tbl_Campaign_Follow", CampaignDetails.followCampaignName);
                //DataSet ds = DataBaseHandler.SelectQuery("select * from tbl_GroupPostCampaign where CampaignName='"+CampaignDetails.campaignGroupPost_CampaignName+"' ", "tbl_GroupPostCampaign");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    if ((!string.IsNullOrEmpty(txt_followCampaignName.Text)) && !(string.IsNullOrEmpty(cmb_Follow_Account.Text)))
                    {
                        try
                        {
                            string InsertQueryGroupRequestCampaign = "insert into tbl_Campaign_Follow(CampaignName,AccountName,FollowingFilePath,NoUserToFollow,ScheduledDaily,StartTime,EndTime,DelayFrom,DelayTo,Threads,Module) values('" + CampaignDetails.followCampaignName + "','" + CampaignDetails.followCampaignAccount + "','" + CampaignDetails.followCampaignFollowUserPath+ "','"
                                                                + CampaignDetails.followCampaignNoOfFollowPerAccount + "','" + CampaignDetails.followCampaignScheduledDaily + "','" + CampaignDetails.followCampaignStartTime + "','" + CampaignDetails.followCampaignEndTime + "','"
                                                                + CampaignDetails.followCampaignDelayMax + "','" + CampaignDetails.followCampaignDelayMin + "','" + CampaignDetails.followCampaignNoOfThread + "','"
                                                                + CampaignDetails.campaignFollower_Module + "' )";

                            DataBaseHandler.InsertQuery(InsertQueryGroupRequestCampaign, "tbl_Campaign_Follow");
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }
                        showToGridViewCampaign();
                    }
                }
                else
                {
                    ModernDialog.ShowMessage("Campaign Name Already Exits Please Try With Other Campaign Name !", "Save Campain ", MessageBoxButton.OK);
                    GlobusLogHelper.log.Info("Campaign Name Already Exits Please Try With Other Campaign Name !");
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        


        public void showToGridViewCampaign()
        {
            try
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Campaign Name");
                dt.Columns.Add("Account");
                dt.Columns.Add("Start Time");
                dt.Columns.Add("End Time");
                dt.Columns.Add("Scheduled Daily");

                DataSet ds = null;
                try
                {
                    try
                    {
                        string selectQuery = "select CampaignName,AccountName,StartTime,EndTime,ScheduledDaily from tbl_Campaign_Follow";
                        ds = DataBaseHandler.SelectQuery(selectQuery, "tbl_Campaign_Follow");

                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }


                foreach (DataRow ds_item in ds.Tables[0].Rows)
                {


                    try
                    {
                        //string item = ds_item[0].ToString() + ":" + ds_item[1].ToString() + ":" + ds_item[2].ToString() + ":" + ds_item[3].ToString() + ":" + ds_item[4].ToString();
                        //string Data = item;

                        string accountUser = ds_item[0].ToString();

                        string campaignName = string.Empty;
                        string campaignStartTime = string.Empty;
                        string campaignStopTime = string.Empty;
                        string ScheduledDaily = string.Empty;
                        string status = string.Empty;

                        campaignName = ds_item[1].ToString();
                        campaignStartTime = ds_item[2].ToString();
                        campaignStopTime = ds_item[3].ToString();
                        ScheduledDaily = ds_item[4].ToString();

                        dt.Rows.Add(accountUser, campaignName, campaignStartTime, campaignStopTime, ScheduledDaily);
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                }
                DataView dv = dt.DefaultView;
                dv.AllowNew = false;

                dgv_CampaignFollow.ItemsSource = dv;


            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }



        private void chkfollowcampaignLoaduserfile_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new ModernDialog
                {
                    Content = new UserControlLoadfollowCampaign()
                };
                window.MinWidth = 550;
                window.MinHeight = 350;
                // window.Title = "Upload Follow Details";               
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            } 
        }

        private void chkFollowScrapeUseToFollow_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void chkfollowcampaignScheduleTimesettings_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Globals.checkcheduleSetting = "Follow";
                var window = new ModernDialog
                {
                    Content = new UserControlcampaignfollowersScheduler()
                };
                window.MinWidth = 850;
                window.MinHeight = 400;
                window.Title = "Scheduler Settings";

                window.ShowDialog();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            } 
        }

        private void DeleteCampaign_Click(object sender, RoutedEventArgs e)
        {
            DataSet DS = new DataSet();
            DataTable DT = new DataTable();
            string campaignName = string.Empty;
            DataRowView drv = (DataRowView)dgv_CampaignFollow.SelectedItem;
            try
            {
                campaignName = drv.Row.ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            if (!string.IsNullOrEmpty(campaignName))
            {

                var result = ModernDialog.ShowMessage("Are You Sure Delete This Campaign ?? ", "Delete Campaign", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    string DeleteQuery = "delete from tbl_Campaign_Follow where CampaignName='" + campaignName + "'";
                    DataBaseHandler.DeleteQuery(DeleteQuery, "tbl_Campaign_Follow");

                    showToGridViewCampaign();
                }
            }
            else
            {
                ModernDialog.ShowMessage("Please Select Campaign From Datagrid !", "Delete Campaign ", MessageBoxButton.OK);
                GlobusLogHelper.log.Info("Please Select Campaign From Datagrid !");
            }
        }

        
        private void StopProcess(object sender, RoutedEventArgs e)
        {

            try
            {
                DataRowView dv = (DataRowView)dgv_CampaignFollow.SelectedItem;
                string campaignName = dv.Row.ItemArray[0].ToString();

                Thread threadToStopCampaign = new Thread(() => StopCurrentCampaign(campaignName));

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }


           

        }
        public void StopCurrentCampaign(string CampaignName)
        {
            try
            {
                obj_CampaignFollow.isStopFollower = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = obj_CampaignFollow.lstThreadsFollower.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        obj_CampaignFollow.lstThreadsFollower.Remove(item);
                    }
                    catch (Exception ex)
                    {
                        //Thread.ResetAbort();
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            GlobusLogHelper.log.Info("Process Stopped !");
            GlobusLogHelper.log.Debug("Process Stopped !");

        }

        private void StartProcess(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dv = (DataRowView)dgv_CampaignFollow.SelectedItem;
                string campaignName = dv.Row.ItemArray[0].ToString();
                Thread CommentPosterThread = new Thread(() => obj_CampaignFollow.startCampaignFollow(campaignName));
                CommentPosterThread.Start();
                
            }
            catch (Exception ex)
            { 

            }
        }
        CampaignDetailsManager.CampaignDetails.CampaignFollow obj_CampaignFollow = new CampaignDetailsManager.CampaignDetails.CampaignFollow();

        private void EditCampaign_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshCampaign_Click(object sender, RoutedEventArgs e)
        {


        }
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master

        private void chkSkip_private_user_checked(object sender, RoutedEventArgs e)
        {
            try
            {
                CampaignDetails.Skipprivate = true;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
            }
        }
<<<<<<< HEAD
=======
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
    }
}
