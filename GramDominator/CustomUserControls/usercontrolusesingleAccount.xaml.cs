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
using BaseLibID;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using GramDominator.Classes;

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for usercontrolusesingleAccount.xaml
    /// </summary>
    public partial class usercontrolusesingleAccount : UserControl
    {
        public usercontrolusesingleAccount()
        {
            InitializeComponent();            
        }

        QueryManager Qm = new QueryManager();
        private void CheckedAccountFromDataGrid_Click(object sender, RoutedEventArgs e)
        {
            string accountUser = string.Empty;
             string accountPass = string.Empty;
             string proxyAddress = string.Empty;
             string proxyPort = string.Empty;
             string proxyUserName = string.Empty;
             string proxyPassword = string.Empty;
             string status = string.Empty;



            try
            {
                foreach (GramDominator.Classes.Validation objValidation in dgv_List_of_Account.SelectedItems)
                {

                    
                }              





                DataSet ds = null;
                   try
                   {
                       ds = Qm.SelectAccountreport("");
                   }
                catch(Exception ex)
                   {
                       GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
                   }
                   IGGlobals.listAccounts.Clear();
                   for (int noRow = 0; noRow < ds.Tables[0].Rows.Count; noRow++)
                   {
                    string account = ds.Tables[0].Rows[noRow].ItemArray[0].ToString() + ":" + ds.Tables[0].Rows[noRow].ItemArray[1].ToString() + ":" + ds.Tables[0].Rows[noRow].ItemArray[2].ToString() + ":" + ds.Tables[0].Rows[noRow].ItemArray[3].ToString() + ":" + ds.Tables[0].Rows[noRow].ItemArray[4].ToString() + ":" + ds.Tables[0].Rows[noRow].ItemArray[5].ToString();
                    IGGlobals.listAccounts.Add(account);
                  //  dv.AllowNew = false;
                    accountUser =ds.Tables[0].Rows[noRow].ItemArray[0].ToString();
                    accountPass =ds.Tables[0].Rows[noRow].ItemArray[1].ToString();
                    proxyAddress =ds.Tables[0].Rows[noRow].ItemArray[2].ToString();
                    proxyPort = ds.Tables[0].Rows[noRow].ItemArray[3].ToString();
                    proxyUserName=ds.Tables[0].Rows[noRow].ItemArray[4].ToString();
                    proxyPassword=ds.Tables[0].Rows[noRow].ItemArray[5].ToString();

                    InstagramUser objInstagramUser = new InstagramUser("","","","");
                    objInstagramUser.username = accountUser;
                    objInstagramUser.password = accountPass;
                    objInstagramUser.proxyip = proxyAddress;
                    objInstagramUser.proxyport = proxyPort;
                    objInstagramUser.proxyusername = proxyUserName;
                    objInstagramUser.proxypassword = proxyPassword;
                    try
                    {
                        IGGlobals.loadedAccountsDictionary.Add(objInstagramUser.username, objInstagramUser);

                    }
                    catch { }
                   }                  

            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }

        }

        private void btu_selected_Account_click(object sender, RoutedEventArgs e)
        {
            try
            {

                foreach (GramDominator.Classes.Validation objValidation in dgv_List_of_Account.SelectedItems)
                    {
                        if(objValidation.Ischecked==true)
                        {
                            //string data = objValidation.Usernmame;
                        }
                    }
               
                
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
            }
        }

        



    }

    public class UsernameDetails
    {
        public string Username { get; set; }
        public bool Ischecked { get; set; }
    }
}
