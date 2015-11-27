using Accounts;
using BaseLib;
using BaseLibID;
using Globussoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;

namespace DirectMessage
{
    public class DirectMessageManager
    {

        #region Global Variables For Direct Message Poster
        readonly object lockrThreadControlleDirectmessagePoster = new object();
        public bool isStopDirectmessagePoster = false;
        public bool useOriginalMessage = true;
        public bool isStopDMPoster = false;
        int countThreadControllerDirectmessagePoster = 0;
        public static int TotalNoOfDirectmessagePosterCounter = 0;
        public static int messageCountDirectmessagePoster = 0;
        int countDirectmessagePoster = 1;
        public static string DirectmessagePhoto_ID = string.Empty;
        public static string message_Directmessage = string.Empty;
        public static string DirectmessagePhoto_ID_path = string.Empty;
        public static string message_Directmessage_path = string.Empty;
        public static int Nothread_Directmessage = 0;
        public List<Thread> lstThreadsDirectmessagePoster = new List<Thread>();
        public List<Thread> lstThreadsDMPoster = new List<Thread>();
        public List<string> lstDirectmessagePostURLsCommentPoster = new List<string>();
        public List<string> lstDirectmessagePostURLsTitles = new List<string>();
        public static int minDelayDMoster = 0;
        public static int maxDelayDMPoster = 0;
        public static int minDelayDirectmessagePoster = 10;
        public static int maxDelayDirectmessagePoster = 20;
        public static string status = string.Empty;
        public GlobusHttpHelper httpHelper = new GlobusHttpHelper();
        public ChilkatHttpHelpr chilkathttpHelper = new ChilkatHttpHelpr();
        public static int mindelay = 0;
        public static int maxdelay = 0;
        public static string DM_Message_path = string.Empty;
        public static string UserName_path = string.Empty;
        public static string txt_DM_Message = string.Empty;
        public static string txt_UserName = string.Empty;
        public static int Nothread_DM = 0;

        #endregion

        public int NoOfThreadsDirectmessagePoster
        {
            get;
            set;
        }

        public void StartCommentPoster()
        {
            countThreadControllerDirectmessagePoster = 0;
            try
            {
                int numberOfAccountPatch = 25;

                if (NoOfThreadsDirectmessagePoster > 0)
                {
                    numberOfAccountPatch = NoOfThreadsDirectmessagePoster;
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
                                lock (lockrThreadControlleDirectmessagePoster)
                                {
                                    try
                                    {
                                        if (countThreadControllerDirectmessagePoster >= listAccounts.Count)
                                        {
                                            Monitor.Wait(lockrThreadControlleDirectmessagePoster);
                                        }

                                        string acc = account.Remove(account.IndexOf(':'));

                                        //Run a separate thread for each account
                                        InstagramUser item = null;
                                        IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                                        if (item != null)
                                        {
                                            Thread profilerThread = new Thread(StartMultiThreadsDirectMessage);
                                            profilerThread.Name = "workerThread_Profiler_" + acc;
                                            profilerThread.IsBackground = true;

                                            profilerThread.Start(new object[] { item });

                                            countThreadControllerDirectmessagePoster++;
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
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void StartMultiThreadsDirectMessage(object parameters)
        {
            try
            {
                if (!isStopDirectmessagePoster)
                {
                    try
                    {
                        lstThreadsDirectmessagePoster.Add(Thread.CurrentThread);
                        lstThreadsDirectmessagePoster.Distinct();
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
                            StartActionDirectMessage(ref objFacebookUser);
                            status = "Success";
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
                        lock (lockrThreadControlleDirectmessagePoster)
                        {
                            countThreadControllerDirectmessagePoster--;
                            Monitor.Pulse(lockrThreadControlleDirectmessagePoster);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }

        private void StartActionDirectMessage(ref InstagramUser fbUser)
        {

            try
            {
                Start_Send(ref fbUser);
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void Start_Send(ref InstagramUser send_obj)
        {
            try
            {
                lstThreadsDirectmessagePoster.Add(Thread.CurrentThread);
                lstThreadsDirectmessagePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
               // ClGlobul.DM_Messagelist.Clear();
              //  ClGlobul.DM_UserList.Clear();
                GlobusHttpHelper obj = send_obj.globusHttpHelper;
                string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                if (string.IsNullOrEmpty(DM_Message_path) && string.IsNullOrEmpty(UserName_path))
                {
                    if (!string.IsNullOrEmpty(txt_DM_Message) && !string.IsNullOrEmpty(txt_UserName))
                    {
                        try
                        {

                            //add following in followingList...
                            ClGlobul.DM_Messagelist.Clear();
                            ClGlobul.DM_UserList.Clear();
                            string s = txt_DM_Message;
                            string k = txt_UserName;
                            if (s.Contains(','))
                            {
                                try
                                {
                                    string[] Data = s.Split(',');

                                    foreach (var item in Data)
                                    {
                                        if (!ClGlobul.DM_Messagelist.Contains(item))
                                        {
                                        ClGlobul.DM_Messagelist.Add(item);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                }
                            }
                            else
                            {
                                if(!ClGlobul.DM_Messagelist.Contains(txt_DM_Message))
                                {
                                ClGlobul.DM_Messagelist.Add(txt_DM_Message);
                                }
                            }
                            if (k.Contains(','))
                            {
                                try
                                {
                                    string[] Data = k.Split(',');

                                    foreach (var item in Data)
                                    {
                                        if(!ClGlobul.DM_UserList.Contains(item))
                                        {
                                        ClGlobul.DM_UserList.Add(item);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                }
                            }
                            else
                            {
                                if(!ClGlobul.DM_UserList.Contains(txt_UserName))
                                {
                                ClGlobul.DM_UserList.Add(txt_UserName);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }
                    }

                    else
                    {
                        GlobusLogHelper.log.Info("----Fill Field Properly----");
                    }
                }
                if (status == "Success")
                {
                    DM_SendMessage(ref send_obj);
                }
                else
                {
                    GlobusLogHelper.log.Info("Login Fail");
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void DM_SendMessage(ref InstagramUser Obj_DMessage)
        {
            try
            {
                lstThreadsDirectmessagePoster.Add(Thread.CurrentThread);
                lstThreadsDirectmessagePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string PPagesource = string.Empty;
            string res_postdata = string.Empty;

            GlobusHttpHelper obj = Obj_DMessage.globusHttpHelper;
            string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");

            try
            {
                foreach (string username in ClGlobul.DM_UserList)
                {
                    //foreach (string message in ClGlobul.DM_Messagelist)
                    //{
                        string message = ClGlobul.DM_Messagelist[RandomNumberGenerator.GenerateRandom(0, ClGlobul.DM_Messagelist.Count)];
                        string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                        PPagesource = obj.getHtmlfromUrl(new Uri(Icon_url),"");
                        string responce_icon = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");
                        string responce1_icon = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconsquarecommentUrl), "");
                        if (!(PPagesource == ""))
                        {
                            string mesaagePageUrl = IGGlobals.Instance.IGiconsquaremessageUrl;
                            string referer = IGGlobals.Instance.IGiconosquareviewUrl;
                            string firstPagesource = obj.getHtmlfromUrl(new Uri(mesaagePageUrl), referer);

                            string messagePopupUrl = IGGlobals.Instance.IGiconsquaremessagepostUrl;
                            string refererForPopup = IGGlobals.Instance.IGiconsquaremessageUrl;
                            string secondPagesource = obj.getHtmlfromUrl(new Uri(messagePopupUrl), refererForPopup);

                            string finalPostData = "action=save-dm&username=" + username + "&message=" + message;
                            string finalUrl = IGGlobals.Instance.IGiconsquarecontrollerUrl;




                            string finalpagesource = obj.postFormData(new Uri(finalUrl), finalPostData, refererForPopup, "");

                            string checkmsg = IGGlobals.Instance.IGiconsquaremesagecontactUrl;
                            string referercheckmsg = IGGlobals.Instance.IGiconsquaremessageUrl;

                            string finalcheckmsg = obj.getHtmlfromUrl(new Uri(checkmsg), referercheckmsg);

                            if (string.IsNullOrEmpty(finalpagesource))
                            {
                                DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime UserName, Message,Status) values('" + "DirectMessage" + "','" + Obj_DMessage.username + "','" + DateTime.Now + "','" + username + "','" + message + "','" + status + "')", "tbl_AccountReport");
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Message send To : " + username + " ]");                                                                                          
                            }
                            else
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ We Can't Send Message To This User]");
                            }                           
                        }
                        if (minDelayDirectmessagePoster != 0)
                        {
                            mindelay = minDelayDirectmessagePoster;
                        }
                        if (maxDelayDirectmessagePoster != 0)
                        {
                            maxdelay = maxDelayDirectmessagePoster;
                        }

                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds For " + Obj_DMessage.username + " ]");
                        Thread.Sleep(delay * 1000);
                  
                    //}
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Process Completed ]");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }


    }
}
