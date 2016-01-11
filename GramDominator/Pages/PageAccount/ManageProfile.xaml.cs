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
using Accounts;
using BaseLibID;
using Globussoft;
using System.Threading;
using System.Data;


namespace GramDominator.Pages.PageAccount
{
    /// <summary>
    /// Interaction logic for ManageProfile.xaml
    /// </summary>
    public partial class ManageProfile : UserControl
    {
        public ManageProfile()
        {
            InitializeComponent();
           // LoadManageprofile_Accountreport();
        }

        ManageProfiledata obj_ManageProfiledata = new ManageProfiledata();
        QueryManager Qm = new QueryManager();


        private void rdo_changeprofile_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                changeProfile.IsEnabled = true;
                changepassword.IsEnabled = false;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void rdo_changepassword_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                changepassword.IsEnabled = true;
                changeProfile.IsEnabled = false ;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void Btn_LoadUsername_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    Txt_Loadnewusername.Text = dlg.FileName.ToString();
                    readLoadUserFile(dlg.FileName);
                    Lbl_AccountProcess_ManageAccount.Content = "UserName Uploaded"; 

                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        private void Btn_LoadPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    Txt_LoadPassword.Text = dlg.FileName.ToString();
                    readLoadPasswordFile(dlg.FileName);
                    Lbl_AccountProcess_ManageAccount.Content = "Password Uploaded"; 
                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        public void readLoadUserFile(string commentidFilePath)
        {
            try
            {
                ClGlobul.ListUsername_Manageprofile.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {
                    //add Comment Id's In Globol Comment Id List ...
                    ClGlobul.ListUsername_Manageprofile.Add(commentidlist_item);
                }
                ClGlobul.ListUsername_Manageprofile = ClGlobul.ListUsername_Manageprofile.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.ListUsername_Manageprofile.Count + " Username update. ]");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        public void readLoadPasswordFile(string commentidFilePath)
        {
            try
            {
                ClGlobul.ListPassword.Clear();
                //Read Data From Selected File ....
                List<string> commentidlist = GlobusFileHelper.ReadFile((string)commentidFilePath);
                foreach (string commentidlist_item in commentidlist)
                {
                    //add Comment Id's In Globol Comment Id List ...
                    ClGlobul.ListPassword.Add(commentidlist_item);
                }
                ClGlobul.ListPassword = ClGlobul.ListPassword.Distinct().ToList();

                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + ClGlobul.ListPassword.Count + " Password update. ]");
            }
            catch (Exception ex)
            {

            }
        }

        private void Profile_manage_start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(rdo_changepassword.IsChecked == true)
                {
                    obj_ManageProfiledata.edit_password = true;
                }else if(rdo_changeprofile.IsChecked == true)
                {
                    obj_ManageProfiledata.edit_profile = true;
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Checked One For Operation Start");
                    return;
                }
                Thread ForDivideUser = new Thread(obj_ManageProfiledata.startChangingPassword);
                ForDivideUser.Start();
                GlobusLogHelper.log.Info("------ Change Profile Proccess Started ------");

            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        private void RefreshAccRepotManageProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //LoadManageprofile_Accountreport();
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        private void DeleteAccRepotManageProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        private void ExportAccReptManageProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        //public void LoadManageprofile_Accountreport()
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();

        //        dt.Columns.Add("Account_User");                
        //        dt.Columns.Add("Status");

        //        int counter = 0;
        //        DataSet ds = null;
        //        try
        //        {
        //            ds = Qm.SelectAccountreport("Manage Account");
        //        }
        //        catch (Exception ex)
        //        {
        //            GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
        //        }
        //        foreach (DataRow ds_item in ds.Tables[0].Rows)
        //        {
        //            try
        //            {

        //                string Account_User = ds_item.ItemArray[2].ToString();                        
        //                string Status = ds_item[7].ToString();
        //                dt.Rows.Add(Account_User, Status);


        //            }
        //            catch { };


        //        }

               
        //        DataView dv;
                

        //        this.Dispatcher.Invoke(new Action(delegate
        //        {
        //            dtGrdManageProfile_AccountsReport.ItemsSource = dt.DefaultView;

        //        }));
                
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
        //    }
        //}
    }
}
