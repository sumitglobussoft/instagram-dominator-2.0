using BaseLib;
<<<<<<< HEAD
using GramDominator;
using FaceDominator3._0;
using FirstFloor.ModernUI.Windows.Controls;
using LicensingManager;
using System;
using System.Collections.Generic;
=======
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
using System.Diagnostics;
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

<<<<<<< HEAD
namespace facedominator
=======
namespace GramDominator
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
{
    /// <summary>
    /// Interaction logic for LicensingWindow.xaml
    /// </summary>
    public partial class LicensingWindow : ModernWindow
    {
        public LicensingWindow()
        {
            InitializeComponent();
            Thread objnew = new Thread(StartFormLoadMethod);
<<<<<<< HEAD
            objnew.Start();
           // StartFormLoadMethod();
        }

=======
           objnew.Start();
        }


>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        private void StartFormLoadMethod()
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                licenseCheckProgressbar.IsActive = true;
<<<<<<< HEAD
                 btnActivate.Visibility = Visibility.Hidden;
                 btnValidate.Visibility = Visibility.Hidden;
                
            })); 
            
            LoadFrmMethod();
            StartLicenseValidation();
         
=======
                btnActivate.Visibility = Visibility.Hidden;
                btnValidate.Visibility = Visibility.Hidden;

            }));

            LoadFrmMethod();
            StartLicenseValidation();
            Thread.Sleep(1000);
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

            if (status == "active")
            {
                this.Dispatcher.Invoke(new Action(delegate
                {
                    licenseCheckProgressbar.IsActive = false;
                    btnValidate.Visibility = Visibility.Visible;
<<<<<<< HEAD

                    licenseCheckProgressbar.IsActive = true;
                    btnValidate.Visibility = Visibility.Hidden;
                    Thread threadCheckLicense = new Thread(checkLicense);
                    threadCheckLicense.SetApartmentState(ApartmentState.STA);
                    threadCheckLicense.Start();
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                }));
            }
            else
            {
<<<<<<< HEAD
                MessageBox.Show("Please Contact skype support - Facedominatorsupport");
            }
          
           
          
        }
     
=======
              //  MessageBox.Show("Please Contact skype support - Facedominatorsupport");
                ModernDialog.ShowMessage("Please Contact skype support - Facedominatorsupport", "Message", MessageBoxButton.OK);
            }



        }

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        private void LoadFrmMethod()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
<<<<<<< HEAD
                    {
                        cpuID = licensemanager.FetchMacId();
                        lblCPUId.Content += "=" + cpuID;
                        DisableControls();
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            btnActivate.Visibility = Visibility.Hidden;
                        }));
                    }));

              
          
                  
                //.Text = "Validate Your License";
               /// AddToLogs("Please Click on Validate Your License Button And Follow The Instruction !");
=======
                {
                    cpuID = licensemanager.FetchMacId();
                    lblCPUId.Content += "=" + cpuID;
                    DisableControls();
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        btnActivate.Visibility = Visibility.Hidden;
                    }));
                }));




                //.Text = "Validate Your License";
                /// AddToLogs("Please Click on Validate Your License Button And Follow The Instruction !");
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            licenseCheckProgressbar.IsActive = true;
            btnValidate.Visibility = Visibility.Hidden;
            Thread threadCheckLicense = new Thread(checkLicense);
            threadCheckLicense.SetApartmentState(ApartmentState.STA);
            threadCheckLicense.Start();
        }
<<<<<<< HEAD
            
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        private void checkLicense()
        {
            try
            {

<<<<<<< HEAD
                if (activate=="Activate")
=======
                if (activate == "Activate")
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        new MainWindow().Show();
                        this.Hide();
<<<<<<< HEAD
                    })); 
=======
                    }));
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
<<<<<<< HEAD
           
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        }


        string cpuID = string.Empty;

        LicenseManager licensemanager = new LicenseManager();

        string status = string.Empty;
        string validateLicense = "Validate Your License";
        string start = "Start";
        string activate = "Activate";

        string freeTrialKey = "fdfreetrial";

        //string server1 = "faced.extrem-hosting.net/FD2.0";
        //string server2 = "faced.extrem-hosting.net/FD2.0";
        //string server3 = "faced.extrem-hosting.net/FD2.0";

        string server1 = "licensing.facedominator.com/licensing/ID";
        string server2 = "licensing.facedominator.com/licensing/ID";
        string server3 = "licensing.facedominator.com/licensing/ID";

        public void startLicence()
        {
            new Thread(() =>
            {
                this.Dispatcher.Invoke(new Action(delegate
                {

                    if (btnActivate.Content.ToString() == activate)
                    {
<<<<<<< HEAD
                        Activate();
                    }
                   else if (btnActivate.Content.ToString() == start)
=======
                       Activate();
                    }
                    else if (btnActivate.Content.ToString() == start)
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                    {
                        StartLicenseValidation();
                    }
                    else if (btnActivate.Content.ToString() == validateLicense)
                    {
<<<<<<< HEAD
                       StartLicenseValidation();
=======
                        StartLicenseValidation();
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                    }

                    if (status == "active")
                    {
<<<<<<< HEAD
                        MessageBox.Show("License Validated, Please click on Validate Button");
=======
                      //  MessageBox.Show("License Validated, Please click on Validate Button");
                        ModernDialog.ShowMessage("License Validated, Please click on Validate Button", "Message", MessageBoxButton.OK);
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            btnActivate.Visibility = Visibility.Hidden;
                            btnValidate.Visibility = Visibility.Visible;
                            btnActivate.Content = "Active";
                            btnValidate.Background = Brushes.Green;
                        }));
                    }
                }));

            }).Start();

        }
        MessageBoxButton btnC = MessageBoxButton.OK;
        private new void Activate()
        {
            string Username = (txtUserName.Text).Trim();
            string Password = (txtPassword.Password);
            string TransactionID = (txtTransactionID).Text.Trim();
            string Email = (txtEmail.Text).Trim();
            //string Email = 

<<<<<<< HEAD
           // AddToLogs("Sending Details for Registration");
=======
            // AddToLogs("Sending Details for Registration");
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(TransactionID) && !string.IsNullOrEmpty(Email))
            {
                string response_Registration = licensemanager.RegisterUser(Username, Password, cpuID, TransactionID, Email, server2);

                if (string.IsNullOrEmpty(response_Registration))
                {
<<<<<<< HEAD
                      var result = ModernDialog.ShowMessage("Unable to Register your License", " Message Box ", btnC);

                      if (result == MessageBoxResult.Yes)
                      {

                         // MessageBox.Show("Unable to Register your License");
                          this.Close();
                      }
=======
                    var result = ModernDialog.ShowMessage("Unable to Register your License", " Message Box ", btnC);

                    if (result == MessageBoxResult.Yes)
                    {

                        // MessageBox.Show("Unable to Register your License");
                        this.Close();
                    }
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                }
                else
                {
                    response_Registration = response_Registration.Trim();
                    if (response_Registration == "Sucessfully Inserted")
                    {
                        StartLicenseValidation();
                    }
                    else
                    {
                        var result = ModernDialog.ShowMessage("Unable to Register your License", " Message Box ", btnC);

<<<<<<< HEAD
                             if (result == MessageBoxResult.Yes)
                             {
                                // MessageBox.Show("Unable to Register your License");
                             }
=======
                        if (result == MessageBoxResult.Yes)
                        {
                            // MessageBox.Show("Unable to Register your License");
                        }
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                    }

                }
            }
            else
            {
                var result = ModernDialog.ShowMessage("No Fields can be blank", " Message Box ", btnC);
<<<<<<< HEAD
              //  MessageBox.Show("No Fields can be blank");
=======
                //  MessageBox.Show("No Fields can be blank");
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
            }
        }

        /// <summary>
        /// Starts License Validation
        /// Is the 1st method to run when Form Loads
        /// </summary>

        private void StartLicenseValidation()
        {
<<<<<<< HEAD
           
              LicenseValidation();            
          
=======

            LicenseValidation();

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        }
        private void DisableControls()
        {
            txtEmail.IsEnabled = false;
            txtPassword.IsEnabled = false;
            txtUserName.IsEnabled = false;
            txtTransactionID.IsEnabled = false;
<<<<<<< HEAD
           
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        }

        private void EnableControls()
        {
            txtEmail.IsEnabled = true;
            txtPassword.IsEnabled = true;
            txtUserName.IsEnabled = true;
            txtTransactionID.IsEnabled = true;
<<<<<<< HEAD
          
          
=======


>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        }
        /// <summary>
        /// Validates License on Multiples Servers
        /// </summary>
        private void LicenseValidation()
        {
            try
            {
                string username = string.Empty;
                string txnID = string.Empty;
                string Password = string.Empty;
                string Email = string.Empty;
                this.Dispatcher.Invoke(new Action(delegate
                {
                    DisableControls();
                }));

                //Code to check if active
                //licensemanager.ValidateCPUID(ref status);

                //if (!(lblServr1Status.Text == "Activated" || lblServr2Status.Text == "Activated" ||lblServr3Status.Text == "Activated"))
                {
<<<<<<< HEAD
                   // AddToLogs("Validating on Server 1");
=======
                    // AddToLogs("Validating on Server 1");
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

                    if (licensemanager.ValidateCPUID(ref status, server1, ref username, ref Password, ref txnID, freeTrialKey, cpuID, ref Email))
                    {
                        #region Server 1
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            //lblServr1Status.Text = "Activated";
                            //lblServr1Status.BackColor = Color.Green;

                            //btnActivate.Text = "Start";

                            //lblstatus.Text = "Activated";
                            //lblstatus.BackColor = Color.Green;

                            txtUserName.Text = username;
                            txtTransactionID.Text = txnID;
                            txtPassword.Password = Password;
                            txtEmail.Text = Email;
                            txtTransactionID.Text = txnID;
<<<<<<< HEAD
                            LblActivetedStatus.Visibility = Visibility.Visible;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

                            try
                            {

                                if (username.Contains("fdfreetrial"))
                                {
                                    Globals.CheckLicenseManager = username;
                                }
                                else if (txnID.Contains("fdfreetrial"))
                                {
                                    Globals.CheckLicenseManager = txnID;
                                }
                                else if (Email.Contains("fdfreetrial"))
                                {
                                    Globals.CheckLicenseManager = Email;
                                }

                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                            }

                            Globals.LicenseCheckUserName = username;


                        }));

                        //FaceDominator.frmMain mainFrm = new FaceDominator.frmMain();
                        //mainFrm.Show();

                        //this.Close(); 
                        #endregion

                        return;
                    }
                    else if (status == "norecordfound")
                    {
                        NoRecordFoundMethod();
                        return;
                    }
                    else
                    {
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            LblActiveStatus.Content = status;
                        }));
                        //AddToLogs("Failed on Server 1, Status : " + status + " \nValidating on Server 2");
                    }
                    if (licensemanager.ValidateCPUID(ref status, server2, ref username, ref Password, ref txnID, freeTrialKey, cpuID, ref Email))
                    {
                        #region Server 2
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            LblActiveStatus.Content = "Activated";
<<<<<<< HEAD
                           
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                        }));
                        //FaceDominator.frmMain mainFrm = new FaceDominator.frmMain();
                        //mainFrm.Show();

                        //this.Close(); 
                        #endregion

                        return;
                    }
                    else if (status == "norecordfound")
                    {
                        NoRecordFoundMethod();
                        return;
                    }
                    else
                    {
                        this.Dispatcher.Invoke(new Action(delegate
                        {
<<<<<<< HEAD
                               LblActiveStatus.Content = status;
                        }));
                       // AddToLogs("Failed on Server 2, Status : " + status + " \nValidating on Server 3");
=======
                            LblActiveStatus.Content = status;
                        }));
                        // AddToLogs("Failed on Server 2, Status : " + status + " \nValidating on Server 3");
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                    }
                    if (licensemanager.ValidateCPUID(ref status, server3, ref username, ref Password, ref txnID, freeTrialKey, cpuID, ref Email))
                    {
                        #region Server 3
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            LblActiveStatus.Content = "Activated";
<<<<<<< HEAD
                                               
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                        }));

                        //FaceDominator.frmMain mainFrm = new FaceDominator.frmMain();
                        //mainFrm.Show();
                        //this.Close(); 
                        #endregion

                        return;
                    }
                    else
                    {
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            LblActiveStatus.Content = status;
                        }));
                    }
                    if (status == "nonactive")
                    {
<<<<<<< HEAD
                       // AddToLogs("Status: " + status + "");
=======
                        // AddToLogs("Status: " + status + "");
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

                        DisableControls();
                    }
                    else if (status == "norecordfound")
                    {
                        NoRecordFoundMethod();
                        return;
                    }

                    this.Dispatcher.Invoke(new Action(delegate
                    {

                        txtUserName.Text = username;
                        txtTransactionID.Text = txnID;
                        txtPassword.Password = Password;
                        txtEmail.Text = Email;
                        txtTransactionID.Text = txnID;
                        btnActivate.Visibility = Visibility.Hidden;
                        btnValidate.Visibility = Visibility.Hidden;
                        LblMessage.Text = "Verification of your txn is under process.\n Please wait for your Transaction to be verified.\n Please Contact To Support Team to activate your license, \n  Skype Id Is :- Facedominatorsupport";
                    }));
                }
                //else
                //{
                //    //OpenFrmMain();
                //}
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void NoRecordFoundMethod()
        {
            try
            {
                MessageBox.Show("Please activate your license by submitting your Details");
                this.Dispatcher.Invoke(new Action(delegate
                {
<<<<<<< HEAD
                   
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

                    LblActiveStatus.Content = "Activate";
                    EnableControls();
                    btnActivate.IsEnabled = true;
                    btnActivate.Visibility = Visibility.Visible;
                    btnValidate.Visibility = Visibility.Hidden;
                    LblMessage.Visibility = Visibility.Hidden;
                    //btnStartFD.BackColor = Color.Green;
                }));
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

        }

        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {
            Activate();
<<<<<<< HEAD
           
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        }

        private void LicenseCheckWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //logic of close program
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.Kill();
        }
<<<<<<< HEAD
=======


>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
    }
}
