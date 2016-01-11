<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Net.NetworkInformation;
using BaseLib;
using GramDominator;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using BaseLibID;

namespace LicensingManager
{
    public class LicenseManager        
    {
        //public string checkForLicense()
        //{         
        //}

        public void CreateLicense()
        {
            
        }


        public string FetchMacId()            
        {

=======
﻿using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Management;
using BaseLibID;

namespace GramDominator
{
    class LicenseManager
    {

        public void CreateLicense()
        {

        }


        public string FetchMacId()
        {
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
            string macAddresses = "";
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        if (!string.IsNullOrEmpty(macAddresses))
                        {
                            break;
                        }
                        macAddresses += nic.GetPhysicalAddress().ToString();
                        //break;
                    }
                }
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
            
=======
                // MessageBox.Show(ex.Message);
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
            }
            return macAddresses;
        }


        public string getCPUID()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;

                }
            }
            return cpuInfo;
<<<<<<< HEAD
            
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        }


        ChilkatHttpHelpr HttpHelpr = new ChilkatHttpHelpr();

        /// <summary>
        /// Checks the status of the CPUID from Database
        /// If status is Active, MainFrm starts
        /// </summary>
        public bool ValidateCPUID(ref string statusMessage, string cpuID)
<<<<<<< HEAD
         {
            //string cpuID = getCPUID();
            try
            {
                #region Drct
=======
        {
            //string cpuID = getCPUID();
            try
            {
                //#region Drct
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                //string cpuID = FetchMacId();
                //string SelectQuery = "Select * from users where cpuid='" + cpuID + "'";
                //DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "users");
                //if (ds.Tables[0].Rows.Count == 1)
                //{
                //    string status = ds.Tables[0].Rows[0]["status"].ToString();
                //    if (status.ToLower() == "active")
                //    {
                //        statusMessage = "active";
                //        return true;
                //    }
                //    else if (status.ToLower() == "nonactive")
                //    {
                //        statusMessage = "nonactive";
                //        return false;
                //    }
                //    else if (status.ToLower() == "suspended")
                //    {
                //        statusMessage = "suspended";
                //        return false;
                //    }
                //}
<<<<<<< HEAD
                
                #endregion
=======

              //  #endregion
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

                #region Through php

                //string cpuID = FetchMacId();
                //ChilkatHttpHelpr HttpHelpr = new ChilkatHttpHelpr();
                string res = string.Empty;
                res = HttpHelpr.GetHtml("http://faced.extrem-hosting.net/GetUserData.php?cpid=" + cpuID + "");//HttpHelpr.GetHtml("http://faced.extrem-hosting.net/checkLicence.php?cpid=" + cpuID + "");

                if (string.IsNullOrEmpty(res))
                {
                    System.Threading.Thread.Sleep(1000);
                    res = HttpHelpr.GetHtml("http://faced.extrem-hosting.net/releases/GetUserData.php?cpid=" + cpuID + "");
                }

                if (!string.IsNullOrEmpty(res))
                {
                    string status = string.Empty;
                    string dateTime = string.Empty;
                    string username = string.Empty;
                    string txnID = string.Empty;

                    string trimmed_response = res.Replace("<pre>", "").Replace("</pre>", "").Trim().ToLower();

                    string[] array_status = System.Text.RegularExpressions.Regex.Split(trimmed_response, "<:>");
                    try
                    {
                        status = array_status[0].ToLower();
                    }
                    catch { }
                    try
                    {
                        dateTime = array_status[1].ToLower();
                    }
                    catch { }
                    try
                    {
                        username = array_status[2].ToLower();
                    }
                    catch { }
                    try
                    {
                        txnID = array_status[3].ToLower();
                    }
                    catch { }

                    if (trimmed_response.ToLower().Contains("fdfreetrial") && ((status.ToLower() == "active") || (status.ToLower() == "nonactive")))
                    {
                        if (CheckActivationUpdateStatus(cpuID, dateTime, status, ""))
                        {
                            statusMessage = "Active";
                            return true;
                        }
                        else
                        {
                            statusMessage = "trialexpired";
                            return false;
                        }
                    }
                    else if (status.ToLower() == "active")
                    {
                        statusMessage = "active";
                        return true;
                        // DisableControls();
                    }
                    else if (status.ToLower() == "nonactive")
                    {
                        statusMessage = "nonactive";
                        MessageBoxButton btnC = MessageBoxButton.OK;

                        var result = ModernDialog.ShowMessage("Verification of your txn is under process.\n Please wait for your Transaction to be verified", " Message Box ", btnC);

                        if (result == MessageBoxResult.Yes)
                        {
                        }
<<<<<<< HEAD
                      //  Modu.Show("Verification of your txn is under process.\n Please wait for your Transaction to be verified");
=======
                        //  Modu.Show("Verification of your txn is under process.\n Please wait for your Transaction to be verified");
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                        return false;
                        // DisableControls();
                    }
                    else if (trimmed_response.Contains("trialexpired"))
                    {
                        statusMessage = "trialexpired";
                        MessageBox.Show("Your 3 Days Trial Version has Expired. Please visit our site: facedominator.com to purchase your License");
                        return false;
                    }
                    else if (trimmed_response.ToLower() == "suspended")
                    {
                        statusMessage = "suspended";
                        return false;
                    }
                    else if (trimmed_response.Contains("no record found"))
                    {
                        statusMessage = "norecordfound";
                        return false;
                    }
                    else
                    {
                        statusMessage = "Some Error in Status Field";
                        return false;
<<<<<<< HEAD
                    }                    
=======
                    }
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                }
                else
                {
                    statusMessage = "ServerDown";
                    return false;
                }

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                statusMessage = "Error in License Validation";
                MessageBox.Show(ex.StackTrace);
            }
            return false;
        }

<<<<<<< HEAD
       
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

        public string RegisterUser(string Username, string Password, string cpuID, string TransactionID, string Email, string servr)
        {
            ChilkatHttpHelpr HttpHelpr = new ChilkatHttpHelpr();
            string res = string.Empty;
            try
            {
                string regUrl = "http://" + servr + "/register.php?user=" + Username + "&pass=" + Password + "&cpid=" + cpuID + "&transid=" + TransactionID + "&email=" + Email + "";
                res = HttpHelpr.GetHtml("http://" + servr + "/register.php?user=" + Username + "&pass=" + Password + "&cpid=" + cpuID + "&transid=" + TransactionID + "&email=" + Email + "");

                if (string.IsNullOrEmpty(res))
                {
                    System.Threading.Thread.Sleep(1000);
                    res = HttpHelpr.GetHtml("http://" + servr + "/register.php?user=" + Username + "&pass=" + Password + "&cpid=" + cpuID + "&transid=" + TransactionID + "&email=" + Email + "");
                }

                if (string.IsNullOrEmpty(res))
                {
                    MessageBox.Show("Error Connecting to Facedominator Server,Please check if www.facedominator.com is opening for you.");
<<<<<<< HEAD
                   // Application.Exit();
=======
                    // Application.Exit();
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// Checks the status of the CPUID from Database
        /// If status is Active, MainFrm starts
        /// </summary>
<<<<<<< HEAD
        public bool ValidateCPUID(ref string statusMessage, string servr, ref string username,ref string Password , ref string txnID, string freeTrialKey, string cpuID,ref string Email)
=======
        public bool ValidateCPUID(ref string statusMessage, string servr, ref string username, ref string Password, ref string txnID, string freeTrialKey, string cpuID, ref string Email)
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
        {
            //string cpuID = getCPUID();
            try
            {
<<<<<<< HEAD
              
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                #region Drct
                //string cpuID = FetchMacId();
                //string SelectQuery = "Select * from users where cpuid='" + cpuID + "'";
                //DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "users");
                //if (ds.Tables[0].Rows.Count == 1)
                //{
                //    string status = ds.Tables[0].Rows[0]["status"].ToString();
                //    if (status.ToLower() == "active")
                //    {
                //        statusMessage = "active";
                //        return true;
                //    }
                //    else if (status.ToLower() == "nonactive")
                //    {
                //        statusMessage = "nonactive";
                //        return false;
                //    }
                //    else if (status.ToLower() == "suspended")
                //    {
                //        statusMessage = "suspended";
                //        return false;
                //    }
                //}

                #endregion

                #region Through php

<<<<<<< HEAD
               
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                //string cpuID = FetchMacId();
                //ChilkatHttpHelpr HttpHelpr = new ChilkatHttpHelpr();
                HttpHelpr = new ChilkatHttpHelpr();

                #region Servr 1
                {
                    string res = string.Empty;
                    res = HttpHelpr.GetHtml("http://" + servr + "/GetUserData.php?cpid=" + cpuID + "");
<<<<<<< HEAD
                   
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                    if (string.IsNullOrEmpty(res))
                    {
                        System.Threading.Thread.Sleep(1000);
                        res = HttpHelpr.GetHtml("http://" + servr + "/releases/GetUserData.php?cpid=" + cpuID + "");
                    }



                    if (!string.IsNullOrEmpty(res))
                    {
                        string activationstatus = string.Empty;
                        string dateTime = string.Empty;
<<<<<<< HEAD
                  
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                        string trimmed_response = res.Replace("<pre>", "").Replace("</pre>", "").Trim().ToLower();

                        string[] array_status = System.Text.RegularExpressions.Regex.Split(trimmed_response, "<:>");
                        try
                        {
                            activationstatus = array_status[0].ToLower();
                        }
                        catch { }
                        try
                        {
                            dateTime = array_status[1].ToLower();
                        }
                        catch { }
                        try
                        {
                            username = array_status[2].ToLower();
                        }
                        catch { }
                        try
                        {
                            Password = array_status[3].ToLower();
                        }
                        catch { };

                        try
                        {
                            txnID = array_status[4].ToLower();
                        }
                        catch { }
                        try
                        {
                            Email = array_status[5].ToLower();
                        }
                        catch { };

                        try
                        {
                            if (txnID.Contains("freetrial"))
                            {
                                Globals.Licence_Details = username + ":" + dateTime + "&" + "FreeTrial" + "";
                            }
                            else
                            {
                                Globals.Licence_Details = username + ":" + dateTime + "&" + "Full Version" + "";
                            }
                        }
                        catch { }

                        if (trimmed_response.ToLower().Contains(freeTrialKey) && ((activationstatus.ToLower() == "active") || (activationstatus.ToLower() == "nonactive")))
                        {

                            IGGlobals.Instance.isfreeversion = true;

                            if (CheckActivationUpdateStatus(cpuID, dateTime, activationstatus, servr))
                            {
                                statusMessage = "active";
                                return true;
                            }
                            else
                            {
                                statusMessage = "trialexpired";
                                return false;
                            }

                            if (activationstatus.ToLower() == "active")
                            {
                                statusMessage = "active";
                                return true;
                            }
                            else//else if (activationstatus.ToLower() == "nonactive")
                            {
                                //Update status as Active
                                string updateRes = HttpHelpr.GetHtml("http://" + servr + "/UpdateStatus.php?cpid=" + cpuID + "&status=" + "Active");
                                if (string.IsNullOrEmpty(updateRes))
                                {
                                    System.Threading.Thread.Sleep(1000);
                                    updateRes = HttpHelpr.GetHtml("http://" + servr + "/UpdateStatus.php?cpid=" + cpuID + "&status=" + "Active");
                                }
                                MessageBox.Show("Your Free Version is Activated");
                                return true;
                            }

<<<<<<< HEAD
                     
                            

                           
=======




>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                        }
                        else if (activationstatus.ToLower() == "active")
                        {
                            statusMessage = "active";
<<<<<<< HEAD
                          
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                            return true;
                            // DisableControls();
                        }
                        else if (activationstatus.ToLower() == "nonactive")
                        {
                            statusMessage = "nonactive";
<<<<<<< HEAD
                          
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                            MessageBox.Show("Verification of your txn is under process.\n Please wait for your Transaction to be verified.\n Please Contact To Support Team to activate your license,   Skype Id Is :- Facedominatorsupport");
                            return false;
                            //DisableControls();
                        }
                        else if (trimmed_response.Contains("trialexpired"))
                        {
                            statusMessage = "trialexpired";
                            MessageBox.Show("Your 3 Days Trial Version has Expired. Please visit our site: facedominator.com to purchase your License");
                            return false;
                        }
                        else if (trimmed_response.ToLower() == "suspended")
                        {
                            statusMessage = "suspended";
                            return false;
                        }
                        else if (trimmed_response.Contains("no record found"))
                        {
                            statusMessage = "norecordfound";
                            return false;
                        }
                        else
                        {
                            statusMessage = "Some Error in Licensing Server";
                            return false;
                        }

                    }
                    else
                    {
                        statusMessage = "ServerDown";
                        return false;
                    }
                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                statusMessage = "Error in License Validation";
                MessageBox.Show(ex.StackTrace);
            }
            return false;
        }

        private bool CheckActivationUpdateStatus(string cpuID, string dateTime, string status, string servr)
        {
            try
            {
                string strdateTime_DataBase = dateTime;

                DateTime dt = DateTime.Parse(strdateTime_DataBase);

                strdateTime_DataBase = dt.ToString("yyyy-MM-dd hh:mm:ss");

                string res_ServerDateTime = HttpHelpr.GetHtml("http://" + servr + "/datetime.php");
                if (string.IsNullOrEmpty(res_ServerDateTime))
                {
                    System.Threading.Thread.Sleep(1000);
                    res_ServerDateTime = HttpHelpr.GetHtml("http://" + servr + "/datetime.php");
                }

                DateTime dt_now = DateTime.Parse(res_ServerDateTime);

                TimeSpan dt_Difference = dt_now.Subtract(dt);

                if (dt_Difference.Days > 3)
                {
                    //string updateQuery = "Update users Set status='" + "TrialExpired" + "' where cpuID='" + cpuID + "'";
                    //DataBaseHandler.UpdateQuery(updateQuery, "users");
                    //return false;

                    string updateRes = HttpHelpr.GetHtml("http://" + servr + "/UpdateStatus.php?cpid=" + cpuID + "&status=" + "TrialExpired");
                    if (string.IsNullOrEmpty(updateRes))
                    {
                        System.Threading.Thread.Sleep(1000);
                        updateRes = HttpHelpr.GetHtml("http://" + servr + "/UpdateStatus.php?cpid=" + cpuID + "&status=" + "TrialExpired");
                    }
                    MessageBox.Show("Your 3 Days Trial Version has Expired. Please visit our site: facedominator.com to purchase your License");
                    return false;
                }
                else if (status == "nonactive")
                {
                    //Update status as Active
                    string updateRes = HttpHelpr.GetHtml("http://" + servr + "/UpdateStatus.php?cpid=" + cpuID + "&status=" + "Active");
                    if (string.IsNullOrEmpty(updateRes))
                    {
                        System.Threading.Thread.Sleep(1000);
                        updateRes = HttpHelpr.GetHtml("http://" + servr + "/UpdateStatus.php?cpid=" + cpuID + "&status=" + "Active");
                    }
                    MessageBox.Show("Your 3 Days Trial Version is Activated");
                    return true;
                }

                return true;
            }
            catch { return false; }
        }
<<<<<<< HEAD
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
    }
}
