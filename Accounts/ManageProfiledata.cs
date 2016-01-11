using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLib;
using BaseLibID;
using System.Threading;
using Globussoft;
using System.Text.RegularExpressions;
using System.Web;


namespace Accounts
{
   public class ManageProfiledata
    {

        #region Global Variable

        public bool edit_profile = false;
        public bool edit_password = false;
        int countThreadControllerChangeprofile = 0;
        readonly object lockrThreadControlleChangeprofile = new object();
        public bool isStopChangeprofile = false;
        public List<Thread> lstThreadsChangeprofile = new List<Thread>();
        public static string status = string.Empty;
        


        #endregion

        public int NoOfThreadsChangeProfile
        {
            get;
            set;
        }

       public void startChangingPassword()
        {
            try
            {
                countThreadControllerChangeprofile = 0;
                try
                {
                    int numberOfAccountPatch = 25;

                    if (NoOfThreadsChangeProfile > 0)
                    {
                        numberOfAccountPatch = NoOfThreadsChangeProfile;
                    }

                    List<List<string>> list_listAccounts = new List<List<string>>();
                    if (IGGlobals.listAccounts.Count >= 1)
                    {

                        list_listAccounts = Utils.Split(IGGlobals.listAccounts, numberOfAccountPatch);

                        foreach (List<string> listAccounts in list_listAccounts)
                        {

                            foreach (string account in listAccounts)
                            {
                                try
                                {
                                    lock (lockrThreadControlleChangeprofile)
                                    {
                                        try
                                        {
                                            if (countThreadControllerChangeprofile >= listAccounts.Count)
                                            {
                                                Monitor.Wait(lockrThreadControlleChangeprofile);
                                            }

                                            string acc = account.Remove(account.IndexOf(':'));

                                            //Run a separate thread for each account
                                            InstagramUser item = null;
                                            IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                                            if (item != null)
                                            {
                                                Thread profilerThread = new Thread(StartMultiThreadsChangeprofile);
                                                profilerThread.Name = "workerThread_Profiler_" + acc;
                                                profilerThread.IsBackground = true;

                                                profilerThread.Start(new object[] { item });
                                               // profilerThread.Join();
                                                countThreadControllerChangeprofile++;
                                            }
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
                        }
                    }
                }
                catch(Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
            }
        }

       public void StartMultiThreadsChangeprofile(object parameters)
       {
           try
           {
               if (!isStopChangeprofile)
               {
                   try
                   {
                       lstThreadsChangeprofile.Add(Thread.CurrentThread);
                       lstThreadsChangeprofile.Distinct();
                       Thread.CurrentThread.IsBackground = true;
                   }
                   catch (Exception ex)
                   {
                       GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                   }
                   try
                   {
                       Array paramsArray = new object[1];
                       paramsArray = (Array)parameters;

                       InstagramUser objFacebookUser = (InstagramUser)paramsArray.GetValue(0);

                       if (!objFacebookUser.isloggedin)
                       {
                           GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();

                           objFacebookUser.globusHttpHelper = objGlobusHttpHelper;

                           //Login Process

                           Accounts.AccountManager objAccountManager = new AccountManager();
                           status = objAccountManager.LoginUsingGlobusHttp(ref objFacebookUser);
                           


                       }

                       if (objFacebookUser.isloggedin)
                       {
                           status = "Success";
                           StartActionChangeprofile(ref objFacebookUser);


                       }
                       else
                       {
                           GlobusLogHelper.log.Info("Couldn't Login With Username : " + objFacebookUser.username);
                           GlobusLogHelper.log.Debug("Couldn't Login With Username : " + objFacebookUser.username);
                       }
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

           finally
           {
               try
               {

                   {
                       lock (lockrThreadControlleChangeprofile)
                       {
                           countThreadControllerChangeprofile--;
                           Monitor.Pulse(lockrThreadControlleChangeprofile);
                       }
                   }
               }
               catch (Exception ex)
               {
                   GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
               }
           }
       }



       public void StartActionChangeprofile(ref InstagramUser obj_Gramuser)
       {
           try
           {
               if(edit_profile)
               {
                   int Username = RandomNumberGenerator.GenerateRandom(0, ClGlobul.ListUsername_Manageprofile.Count);
                   string email = ClGlobul.ListUsername_Manageprofile[Username];
                   //ChangeEmail(email, ref objPinChange);
                   Start_Change_Profile(email,ref obj_Gramuser);
               }
               if(edit_password)
               {
                   int Username = RandomNumberGenerator.GenerateRandom(0, ClGlobul.ListPassword.Count);
                   string Password = ClGlobul.ListPassword[Username];
                   Start_Change_password(Password, ref obj_Gramuser);
               }
           }
           catch(Exception ex)
           {
               GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
           }
       }

       public void Start_Change_password(String NewPassword,ref InstagramUser obj_GDuser)
       {
           try
           {
               
               string url ="https://www.instagram.com/accounts/password/change/";
               string edit_password = obj_GDuser.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "https://www.instagram.com", "");
               string edit_password2 = obj_GDuser.globusHttpHelper.getHtmlfromUrl(new Uri(url), "https://www.instagram.com/accounts/edit/", "");
               string crstoken = Utils.getBetween(edit_password2, "csrf_token", "\"}").Replace("\":\"",string.Empty);
               string postdata = "old_password=" + obj_GDuser.password + "&new_password1=" + NewPassword + "&new_password2=" + NewPassword + "&csrfmiddlewaretoken=" + crstoken;
               string resp = obj_GDuser.globusHttpHelper.PostDataWithInstagram(new Uri("https://www.instagram.com/accounts/password/change/"), postdata, "https://www.instagram.com/accounts/edit/");
               string responce_result = obj_GDuser.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/accounts/password/change/done/"), "", "https://www.instagram.com/accounts/password/change/");
               if (responce_result.Contains("Thanks! You have successfully changed your password."))
               {
                   GlobusLogHelper.log.Info("Password Successfully change of Account ===> " + obj_GDuser.username);
                   DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,Status) values('" + "Manage Account" + "','" + obj_GDuser.username + "','" + "Password Changed" + "')", "tbl_AccountReport");
                   GlobusLogHelper.log.Info("Password Successfully change of Account ===> " + obj_GDuser.username+"    newpassword=====>"+NewPassword);
                   ClGlobul.ListPassword.Remove(NewPassword);
                  // break;                 
               }
               else
               {
                   GlobusLogHelper.log.Info("Password Not Change Of Account ====>" + obj_GDuser.username);
                   ClGlobul.ListPassword.Remove(NewPassword);
                   //break;
               }
                       
               }
         
           catch(Exception ex)
           {
               GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
           }
       }



       public void Start_Change_Profile(string NewUsername, ref InstagramUser Obj_gramUser)
       {
           try
           {
               string crstoken = string.Empty;
               string Name = string.Empty;
               string Email_ID = string.Empty;
               string Phone_Number = string.Empty;
               string Gender = string.Empty;
               string Bio = string.Empty;
               string url = string.Empty;
               int count = 0;


              
                   
                       try
                       {
                       https://www.instagram.com/accounts/edit/
                           string HomePage_responce = Obj_gramUser.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "https://www.instagram.com", "");
                           string EditProfile_responce = Obj_gramUser.globusHttpHelper.getHtmlfromUrl(new Uri(" https://www.instagram.com/accounts/edit/"), "https://www.instagram.com/accounts/edit/", "");
                           string scrape_unchngedata = Utils.getBetween(EditProfile_responce, "<input type=\"hidden\"", "</form>");
                           string[] splited_data = Regex.Split(scrape_unchngedata, "<p name=");
                           foreach (string value in splited_data)
                           {
                               try
                               {
                                   if (value.Contains("csrfmiddlewaretoken"))
                                   {
                                       crstoken = Utils.getBetween(value, "value=\"", "\"");
                                   }
                                   if (value.Contains("first_name_section"))
                                   {
                                       Name = Utils.getBetween(value, "value=\"", "\"");
                                   }
                                   if (value.Contains("email_section"))
                                   {
                                       Email_ID = Utils.getBetween(value, "value=\"", "\"");
                                       Email_ID = HttpUtility.UrlEncode(Email_ID);
                                   }
                                   if (value.Contains("phone_number_section"))
                                   {
                                       Phone_Number = Utils.getBetween(value, "value=\"", "\"");
                                       Phone_Number = HttpUtility.UrlEncode(Phone_Number);
                                   }
                                   if (value.Contains("gender_section"))
                                   {

                                       Gender = Utils.getBetween(value, "selected\">", "</option>");
                                       if (Gender == "Female")
                                       {
                                           count = 2;
                                       }
                                       if (Gender == "Male")
                                       {
                                           count = 1;
                                       }
                                       if(Gender == "--------")
                                       {
                                           count = 1;
                                       }
                                   }
                                   if (value.Contains("biography_section"))
                                   {
                                       Bio = Utils.getBetween(value, "biography\">", "</option>");
                                   }
                                   if (value.Contains("external_url_section"))
                                   {
                                       url = Utils.getBetween(value, "external_url\" />", "</span>");
                                   }

                               }
                               catch (Exception ex)
                               {
                                   GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
                               }


                           }

                           string PastData_chnageUsername = "csrfmiddlewaretoken=" + crstoken + "&first_name=" + Name + "&email=" + Email_ID + "&username=" + NewUsername + "&phone_number=" + Phone_Number + "&gender=" + count + "&biography=" + Bio + "&external_url=" + url + "&chaining_enabled=on";
                           string resp_changeusername = Obj_gramUser.globusHttpHelper.PostDataWithInstagram(new Uri("https://www.instagram.com/accounts/edit/"), PastData_chnageUsername, "https://www.instagram.com/accounts/edit/");
                           if (resp_changeusername.Contains("Sorry, that username is taken"))
                           {
                               GlobusLogHelper.log.Info("Sorry, that username is taken" + "====>" + Obj_gramUser.username);
                           }
                           else if (resp_changeusername.Contains("Successfully updated your profile"))
                           {
                               DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,Status) values('" + "Manage Account" + "','" + Obj_gramUser.username + "','" + "Username Changed" + "')", "tbl_AccountReport");
                               GlobusLogHelper.log.Info("Successfully updated your profile Username ====>" + Obj_gramUser.username + "To==>" + NewUsername);
                               ClGlobul.ListUsername_Manageprofile.Remove(NewUsername);
                               //break;
                           }
                           else if(resp_changeusername.Contains("Usernames can only use letters, numbers, underscores and periods."))
                               {
                                   GlobusLogHelper.log.Info("Sorry Change Fail ,Usernames can only use letters, numbers, underscores and periods" + "====>" + Obj_gramUser.username);
                               }
                           else
                           {
                               GlobusLogHelper.log.Info("Fail To update your profile Username ===>" + Obj_gramUser.username);
                           }



                           
                       }

                       catch (Exception ex)
                       {
                           GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
                       }


                   } 
               
           
           catch(Exception ex)
           {
               GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
           }
       }

    }
}
