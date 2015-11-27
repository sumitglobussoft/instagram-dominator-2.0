using Accounts;
using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using GramDominator.CustomUserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GramDominator.Pages.PageAccount
{
    /// <summary>
    /// Interaction logic for ManageAccounts.xaml
    /// </summary>
    public partial class ManageAccounts : UserControl
    {


        public static ManageAccounts objManageAccounts;
        public ManageAccounts()
        {
            InitializeComponent();
            AccounLoad();

            try
            {
                objManageAccounts = this;
            }
            catch { };

        }
        QueryManager Qm = new QueryManager();
        private void btnAccounts_ManageAccounts_LoadAccounts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAccountProgressBar.IsIndeterminate = true;
                Thread uploadAccountThread = new Thread(LoadAccounts);
                uploadAccountThread.SetApartmentState(System.Threading.ApartmentState.STA);
                uploadAccountThread.IsBackground = true;

                uploadAccountThread.Start();
               // uploadAccountThread.Join();
               
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

        }

        private void btnAccounts_ManageAccounts_Delete_Click(object sender, RoutedEventArgs e)
        {
            var result = ModernDialog.ShowMessage("Are You Sure Delete Loaded Account ?? ", "Delete Account", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                QueryExecuter.deleteQuery();
                IGGlobals.listAccounts.Clear();
                AccounLoad();

               
            } 
        }

        private void btnAccounts_ManageAccounts_AddSingleAccounts_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                var window = new ModernDialog
                {
                    Title = " Add Single Account ",
                    Content = new UserControlMobilePhones()
                };
                window.ShowInTaskbar = true;

                window.ShowDialog();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : 55" + ex.Message);
            }
            LoadAccountsFromDataBase();
            
        }

        private void addSingleAccount_Click(object sender, RoutedEventArgs e)
        {

            var window = new ModernDialog
            {
                Content = new UserControlMobilePhones()
            };
            window.MinHeight = 250;
            window.MinWidth = 450;
            window.Title = "Add Single Account";
            window.ShowDialog();

            LoadAccountsFromDataBase();
        }

        public void LoadAccountsFromDataBase()
        {
            try
            {
                IGGlobals.loadedAccountsDictionary.Clear();
                IGGlobals.listAccounts.Clear();

                DataTable dt = new DataTable();

                dt.Columns.Add("UserName");
                dt.Columns.Add("Password");
                dt.Columns.Add("proxyAddress");
                dt.Columns.Add("proxyPort");
                dt.Columns.Add("proxyUsername");
                dt.Columns.Add("proxyPassword");
                dt.Columns.Add("Path");
                dt.Columns.Add("LogInStatus");
                dt.Columns.Add("Posts");
                dt.Columns.Add("Followers");
                dt.Columns.Add("Followings");
                dt.Columns.Add("Authorized");
                dt.Columns.Add("Status");


             


                int counter = 0;
                DataSet ds = null;
                try
                {
                    ds = Qm.SelectAccounts();                  
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }


                foreach (DataRow ds_item in ds.Tables[0].Rows)
                {
                    if (Globals.CheckLicenseManager == "fdfreetrial" && counter == 5)
                    {
                        break;
                    }
                    counter = counter + 1;
                    try
                    {
                        string item = ds_item[1].ToString() + ":" + ds_item[2].ToString() + ":" + ds_item[3].ToString() + ":" + ds_item[4].ToString() + ":" + ds_item[5].ToString() + ":" + ds_item[6].ToString() + ":" + ds_item[7].ToString() + ":" + ds_item[8].ToString() + ":" + ds_item[9].ToString() + ":" + ds_item[10].ToString() + ":" + ds_item[11].ToString() + ":" + ds_item[12].ToString() + ":" + ds_item[13].ToString();
                        string account = item;
                        string[] AccArr = account.Split(':');
                        if (AccArr.Count() > 1)
                        {
                            string accountUser = account.Split(':')[0];
                            string accountPass = account.Split(':')[1];

                            string proxyAddress = string.Empty;
                            string proxyPort = string.Empty;
                            string proxyUsername = string.Empty;
                            string proxyPassword = string.Empty;
                            string Path = string.Empty;
                            string LogInStatus = string.Empty;
                            string Posts = string.Empty;
                            string Followers = string.Empty;
                            string Followings = string.Empty;
                            string Authorized = string.Empty;
                            string Status = string.Empty;

                            DataGridColumn newcolumn = new DataGridHyperlinkColumn();


                            int DataCount = account.Split(':').Length;
                            if (DataCount == 2)
                            {
                                //Globals.accountMode = AccountMode.NoProxy;

                            }
                            else if (DataCount == 4)
                            {

                                proxyAddress = account.Split(':')[2];
                                proxyPort = account.Split(':')[3];
                            }
                            else if (DataCount > 5 && DataCount < 7)
                            {

                                proxyAddress = account.Split(':')[2];
                                proxyPort = account.Split(':')[3];
                                proxyUsername = account.Split(':')[4];
                                proxyPassword = account.Split(':')[5];

                            }
                            else if (DataCount >= 7)
                            {
                                proxyAddress = account.Split(':')[2];
                                proxyPort = account.Split(':')[3];
                                proxyUsername = account.Split(':')[4];
                                proxyPassword = account.Split(':')[5];
                                Path = account.Split(':')[6];
                                LogInStatus = account.Split(':')[7];
                                Posts = account.Split(':')[8];
                                Followers = account.Split(':')[9];
                                Followings = account.Split(':')[10];
                                Authorized = account.Split(':')[11];
                                Status = account.Split(':')[12];


                            }

                            dt.Rows.Add(accountUser, accountPass, proxyAddress, proxyPort, proxyUsername, proxyPassword, Path, LogInStatus, Posts, Followers, Followings, Authorized, Status);


                            try
                            {
                                InstagramUser obj_InstagramUser = new InstagramUser("","","","");
                                obj_InstagramUser.username = accountUser;
                                obj_InstagramUser.password = accountPass;
                                obj_InstagramUser.proxyip = proxyAddress;
                                obj_InstagramUser.proxyport = proxyPort;
                                obj_InstagramUser.proxyusername = proxyUsername;
                                obj_InstagramUser.proxypassword = proxyPassword;

                                IGGlobals.loadedAccountsDictionary.Add(obj_InstagramUser.username, obj_InstagramUser);

                                #region MyRegion
                                //try
                                //{
                                //    if (cmbGroups_GroupCampaignManager_Accounts.InvokeRequired)
                                //    {
                                //        cmbScraper__fanscraper_Accounts.Invoke(new MethodInvoker(delegate
                                //        {
                                //            cmbScraper__fanscraper_Accounts.Items.Add(accountUser);
                                //        }));
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                //}

                                //try
                                //{
                                //    if (cmbScraper__CustomAudiencesScraper_Accounts.InvokeRequired)
                                //    {
                                //        cmbScraper__CustomAudiencesScraper_Accounts.Invoke(new MethodInvoker(delegate
                                //        {
                                //            cmbScraper__CustomAudiencesScraper_Accounts.Items.Add(accountUser);
                                //        }));
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                //}
                                //try
                                //{
                                //    //cmbCommentsOnPostSelectAccount
                                //    if (cmbCommentsOnPostSelectAccount.InvokeRequired)
                                //    {
                                //        cmbCommentsOnPostSelectAccount.Invoke(new MethodInvoker(delegate
                                //        {
                                //            cmbCommentsOnPostSelectAccount.Items.Add(accountUser + ":" + accountPass);
                                //        }));
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    GlobusLogHelper.log.Error(ex.Message);
                                //}
                                //try
                                //{
                                //    if (cmbGroups_GroupCampaignManager_Accounts.InvokeRequired)
                                //    {
                                //        cmbGroups_GroupCampaignManager_Accounts.Invoke(new MethodInvoker(delegate
                                //        {
                                //            cmbGroups_GroupCampaignManager_Accounts.Items.Add(accountUser);
                                //        }));
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                //}

                                //try
                                //{
                                //    if (cmbScraper__GroupMemberScraper_Accounts.InvokeRequired)
                                //    {
                                //        cmbScraper__GroupMemberScraper_Accounts.Invoke(new MethodInvoker(delegate
                                //        {
                                //            cmbScraper__GroupMemberScraper_Accounts.Items.Add(accountUser);
                                //        }));
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                //} 
                                #endregion

                                IGGlobals.listAccounts.Add(obj_InstagramUser.username + ":" + obj_InstagramUser.password + ":" + obj_InstagramUser.proxyip + ":" + obj_InstagramUser.proxyport + ":" + obj_InstagramUser.proxyusername + ":" + obj_InstagramUser.proxypassword);
                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                            }

                            ///Set this to "0" if loading unprofiled accounts
                            ///
                            string profileStatus = "0";


                        }
                        else
                        {
                            GlobusLogHelper.log.Info("Account has some problem : " + item);
                            GlobusLogHelper.log.Debug("Account has some problem : " + item);
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                }
                DataView dv = ds.Tables[0].DefaultView;
                dv.AllowNew = false;

                this.Dispatcher.Invoke(new Action(delegate
                {
                    grvAccounts_AccountCreator_AccountDetails.ItemsSource = dv;

                }));

                GlobusLogHelper.log.Debug("Accounts Loaded : " + dt.Rows.Count);
                GlobusLogHelper.log.Info("Accounts Loaded : " + dt.Rows.Count);

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void DeleteSingleAccount_Click(object sender, RoutedEventArgs e)
        {
            DeleteSingleAccount();
        }

        private void DeleteSingleAccount()
        {
            QueryManager qm = new QueryManager();
            MessageBoxButton btn = MessageBoxButton.OK;
            MessageBoxButton btnC = MessageBoxButton.YesNoCancel;

            try
            {
                int i = grvAccounts_AccountCreator_AccountDetails.SelectedIndex;

                if (i < 0)
                {
                    GlobusLogHelper.log.Info("Please Select Account For Deletion !");

                    var ResultMessageBox = ModernDialog.ShowMessage("Please Select Account For Deletion !", " Delete Account ", btnC);

                    return;
                }


                var result = ModernDialog.ShowMessage("Are You Want To Delete This Accounts Permanently?", " Delete Account ", btnC);

                if (result == MessageBoxResult.Yes)
                {
                    foreach (var selection in grvAccounts_AccountCreator_AccountDetails.SelectedItems)
                    {
                        try
                        {
                            DataRowView row = (DataRowView)selection;

                            string Username = row["UserName"].ToString();
                            string Password = row["Password"].ToString();
                            qm.DeleteAccounts(Username);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : 55" + ex.Message);
                        }
                    }
                    LoadAccountsFromDataBase();
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : 55" + ex.Message);
            }
        }

        private void LoadAccounts()
        {

            try
            {
                DataSet ds;
                
                DataTable dt = new DataTable();

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                string Path = dlg.ToString().Replace("Microsoft.Win32.OpenFileDialog: Title: , FileName","");
                if (result == true)
                {
                    DateTime sTime = DateTime.Now;

                    dt.Columns.Add("UserName");
                    dt.Columns.Add("Password");
                    dt.Columns.Add("ProxyAddress");
                    dt.Columns.Add("ProxyPort");
                    dt.Columns.Add("ProxyUserName");
                    dt.Columns.Add("ProxyPassword");

                    ds = new DataSet();
                    ds.Tables.Add(dt);

                    List<string> templist = GlobusFileHelper.ReadFile(dlg.FileName);

                    if (templist.Count > 0)
                    {
                        IGGlobals.loadedAccountsDictionary.Clear();
                      //  IGGlobals.listAccounts.Clear();
                       
                    }
                    int counter = 0;
                    
                    foreach (string item in templist)
                    {
                        if (Globals.CheckLicenseManager == "fdfreetrial" && counter == 5)
                        {
                            break;
                        }
                        counter = counter + 1;
                        try
                        {
                            string account = item;
                            string[] AccArr = account.Split(':');
                            if (AccArr.Count() > 1)
                            {
                                string accountUser = account.Split(':')[0];
                                string accountPass = account.Split(':')[1];
                                string proxyAddress = string.Empty;
                                string proxyPort = string.Empty;
                                string proxyUserName = string.Empty;
                                string proxyPassword = string.Empty;
                                string status = string.Empty;

                                int DataCount = account.Split(':').Length;
                                if (DataCount == 2)
                                {
                                    //Globals.accountMode = AccountMode.NoProxy;

                                }
                                else if (DataCount == 4)
                                {

                                    proxyAddress = account.Split(':')[2];
                                    proxyPort = account.Split(':')[3];
                                }
                                else if (DataCount > 5 && DataCount < 7)
                                {

                                    proxyAddress = account.Split(':')[2];
                                    proxyPort = account.Split(':')[3];
                                    proxyUserName = account.Split(':')[4];
                                    proxyPassword = account.Split(':')[5];

                                }
                                else if (DataCount == 7)
                                {

                                    proxyAddress = account.Split(':')[2];
                                    proxyPort = account.Split(':')[3];
                                    proxyUserName = account.Split(':')[4];
                                    proxyPassword = account.Split(':')[5];

                                }

                                dt.Rows.Add(accountUser, accountPass, proxyAddress, proxyPort, proxyUserName, proxyPassword);
                                Qm.DeleteAccounts(accountUser);
                                Qm.AddAccountInDataBase(accountUser, accountPass, proxyAddress, proxyPort, proxyUserName, proxyPassword, Path);
                             

                                try
                                {
                                    InstagramUser objInstagramUser = new InstagramUser("","","","");
                                    objInstagramUser.username = accountUser;
                                    objInstagramUser.password = accountPass;
                                    objInstagramUser.proxyip = proxyAddress;
                                    objInstagramUser.proxyport = proxyPort;
                                    objInstagramUser.proxyusername = proxyUserName;
                                    objInstagramUser.proxypassword = proxyPassword;

                                   IGGlobals.loadedAccountsDictionary.Add(objInstagramUser.username, objInstagramUser);

                           

                                    IGGlobals.listAccounts.Add(objInstagramUser.username + ":" + objInstagramUser.password + ":" + objInstagramUser.proxyip + ":" + objInstagramUser.proxyport + ":" + objInstagramUser.proxyusername + ":" + objInstagramUser.proxypassword);
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                }

                                ///Set this to "0" if loading unprofiled accounts
                                ///
                                string profileStatus = "0";


                            }
                            else
                            {
                                GlobusLogHelper.log.Info("Account has some problem : " + item);
                                GlobusLogHelper.log.Debug("Account has some problem : " + item);
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }

                    }

                  //  DataView dv = dt.DefaultView;
                 //   dv.AllowNew = false;

                    AccounLoad();



                    
                    
                    try
                    {


                
                        DateTime eTime = DateTime.Now;

                        string timeSpan = (eTime - sTime).TotalSeconds.ToString();

                        Application.Current.Dispatcher.Invoke(new Action(() => { lblaccounts_ManageAccounts_LoadsAccountsCount.Content = dt.Rows.Count.ToString(); }));

                        GlobusLogHelper.log.Debug("Accounts Loaded : " + dt.Rows.Count.ToString() + " In " + timeSpan + " Seconds");

                        GlobusLogHelper.log.Info("Accounts Loaded : " + dt.Rows.Count.ToString() + " In " + timeSpan + " Seconds");
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
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                LoadAccountProgressBar.IsIndeterminate = false;
            }));

        }
        
        public void  AccounLoad()
        {
             string accountUser = string.Empty;
             string accountPass = string.Empty;
             string proxyAddress = string.Empty;
             string proxyPort = string.Empty;
             string proxyUserName = string.Empty;
             string proxyPassword = string.Empty;
             string status = string.Empty;
            QueryExecuter QME = new QueryExecuter();
            DataSet ds = QME.getAccount();
            if (ds.Tables[0].Rows.Count != 0)
            {
                //try
                //{
                //    path = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                //    if (!string.IsNullOrEmpty(path))
                //    {
                //        try
                //        {
                //            //txtAddAccounts1.Text = path;
                //        }
                //        catch { };
                //    }
                //}
                //catch { }
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
                        try
                        {
                            this.Dispatcher.Invoke(new Action(delegate
                            {
                                grvAccounts_AccountCreator_AccountDetails.ItemsSource = ds.Tables[0].DefaultView;

                            }));
                        }
                        catch { };
                    }
                    catch { };                  
                 
                }
                try
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        grvAccounts_AccountCreator_AccountDetails.ItemsSource = ds.Tables[0].DefaultView;

                    }));
                }
                catch { };



                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + IGGlobals.listAccounts.Count + " Accounts Loaded ]");
            }
            else
            {
                this.Dispatcher.Invoke(new Action(delegate
                {
                    grvAccounts_AccountCreator_AccountDetails.ItemsSource = ds.Tables[0].DefaultView;

                }));
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  No Accounts Loaded ]");
            }

          
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                 var window = new ModernDialog
                {
                    Title = " Divide Setting ",
                    Content = new UserControldividedata()
                };
                window.ShowInTaskbar = true;
               // window.Topmost = true;

                window.ShowDialog();
            }           
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }


        int acc_checker_counter1 = 0;
        int Maxthread = 1;
        private void btnAccounts_Checker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IGGlobals.listAccounts.Count() == 0)
                {
                    ModernDialog.ShowMessage("Please Upload Account", "Message", MessageBoxButton.OK);
                    GlobusLogHelper.log.Info("Please Uplaod Account");
                }
                else
                {
                    GlobusLogHelper.log.Info("Starting Process For Account Checker");
                     List<List<string>> list_listAccounts = new List<List<string>>();
                                    

                      foreach (string account in IGGlobals.listAccounts)
                      {
                          try
                          {
                              string acc = account.Remove(account.IndexOf(':'));
                              InstagramUser item = null;
                              IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);
                              if (item != null)
                              {
                                  Thread profilerThread = new Thread(Account_Check);
                                  profilerThread.Name = "workerThread_Profiler_" + acc;
                                  profilerThread.IsBackground = true;

                                  profilerThread.Start(new object[] { item });
                              }

                          }                          
                          catch (Exception ex)
                          {
                              GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                          }
                      }
                  
                    }
                }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }            
        }

        public void Account_Check(object parameters)
        {
            DataSet ds = null;
            
            try
            {
                Array paramsArray = new object[1];
                paramsArray = (Array)parameters;

                InstagramUser objFacebookUser = (InstagramUser)paramsArray.GetValue(0);

                //if (!objFacebookUser.isloggedin)
                //{
                    GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();

                    objFacebookUser.globusHttpHelper = objGlobusHttpHelper;

                    //Login Process

                    Accounts.AccountManager objAccountManager = new AccountManager();
                    objAccountManager.LoginUsingGlobusHttp(ref objFacebookUser);
               // }
                try
                {
                    ds = Qm.SelectAccounts();
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
                this.Dispatcher.Invoke(new Action(delegate
                {
                    grvAccounts_AccountCreator_AccountDetails.ItemsSource = ds.Tables[0].DefaultView;
                    
                }));
                GlobusLogHelper.log.Info("Successfully Account Checked===>" + objFacebookUser.username);
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
            finally
            {
               // GlobusLogHelper.log.Info("Successfully Account Cheaked Processes Completed");
            }
        }

        private void AddUserAgrnt_Account_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new ModernDialog
                {
                    Content = new UserControlAddUserAgent()
                };
                window.MinHeight = 250;
                window.MinWidth = 450;
                window.Title = "Add Single Account";
               // window.Topmost = true;
                window.ShowDialog();
            }
             catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        private void Grid_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            if (e.Command == DataGrid.DeleteCommand)
            {
                try
                {
                    int i = grvAccounts_AccountCreator_AccountDetails.SelectedIndex;

                    if (i < 0)
                    {
                        GlobusLogHelper.log.Info("Please select account for deletion");
                        return;
                    }
                    QueryManager qm = new QueryManager();
                    MessageBoxButton btn = MessageBoxButton.OK;
                    MessageBoxButton btnC = MessageBoxButton.YesNo;

                    var result = ModernDialog.ShowMessage("Are you want to delete this Accounts permanently?", " Delete Account ", btnC);

                    if (result == MessageBoxResult.Yes)
                    {
                        foreach (var selection in grvAccounts_AccountCreator_AccountDetails.SelectedItems)
                        {
                            try
                            {
                                DataRowView row = (DataRowView)selection;

                                string Username = row[1].ToString();
                                string Password = row[2].ToString();
                                qm.DeleteAccounts(Username);
                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Error("Error : 55" + ex.Message);
                            }
                        }
                        LoadAccountsFromDataBase();
                    }

                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : 55" + ex.Message);
                }
                e.Handled = true;
            }
        }

        private void SingleAccount_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    var window = new ModernDialog
                    {
                        Title = " Using Selected Account ",
                        Content = new usercontrolusesingleAccount()
                    };
                    window.ShowInTaskbar = true;
                    //window.Topmost = true;

                    window.ShowDialog();
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                }
            }
            catch(Exception ex)
            {

            }
        }

    }



}
