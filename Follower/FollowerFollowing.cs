using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLib;
using BaseLibID;
using Accounts;
using System.Threading;
using Globussoft;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;

namespace Follower
{
    public class FollowerFollowing
    {

        #region Global Variables For Direct Message Poster
        readonly object lockrThreadControlleFollowerPoster = new object();
        public bool isStopFollowerPoster = false;
        public bool IsFollow = false;
        //public static bool DivideByUser = false;
        //public static bool DivideEqual = false;
        public static int Divide_data_NoUser = 0;
        public static int DivideData_Thread = 0;
        public bool useOriginalMessage = true;
        public bool value = false;
        static readonly Object _lockObject = new Object();
        int countThreadControllerFollowerPoster = 0;
        public static int TotalNoOfFollowerPosterCounter = 0;
        public static int messageCountFollowerPoster = 0;
        int countFollowerPoster = 1;
        //  public static string DirectmessagePhoto_ID = string.Empty;
        //public static string message_Directmessage = string.Empty;
        // public static string DirectmessagePhoto_ID_path = string.Empty;
        // public static string message_Directmessage_path = string.Empty;
        public static int Nothread_Follower = 0;
        public List<Thread> lstThreadsFollowerPoster = new List<Thread>();
        public bool chkNotSendRequest = false;

        public List<string> lstFollowerPostURLsCommentPoster = new List<string>();
        public List<string> lstFollowerPostURLsTitles = new List<string>();
        public static int minDelayFollowerPoster = 0;
        public static int maxDelayFollowerPoster = 0;
        // public static int minDelayFollowerPoster = 10;
        // public static int maxDelayFollowerPoster = 20;
        public static string status = string.Empty;
        public GlobusHttpHelper httpHelper = new GlobusHttpHelper();
        public ChilkatHttpHelpr chilkathttpHelper = new ChilkatHttpHelpr();
        public static int mindelay = 0;
        public static int maxdelay = 0;
        public static int No_Follow_User = 0;
        public static string UserName_path = string.Empty;
        // public static string txt_DM_Message = string.Empty;
        public static string txt_UserName = string.Empty;
        // public static int Nothread_Follower = 0;
        List<string> AlreadyFollowedlist = new List<string>();
        List<string> NotFollowedlist = new List<string>();
        List<string> followingList = new List<string>();
        List<string> AllreadyfollowingList = new List<string>();
        List<string> Page_Url = new List<string>();
        int counter_follow = 0;

        #region Global Unfolloer
        public static int minDelayUnFollowerPoster = 0;
        public static int maxDelayUnFollowerPoster = 0;
        public static int No_UnFollow_User = 0;
        public static string UserName_path_Unfollow = string.Empty;
        public static string txt_UserName_Unfollow = string.Empty;
        public bool IsUnFollow = false;
        public int NoOfMaximumNoOfUnfollow = 0;
        public int NoOfUnFollowCompleted = 0;
        private static bool _boolStopUnfollow = false;
     
        #endregion

        
        #endregion


        public int NoOfThreadsFollowerPoster
        {
            get;
            set;
        }

        public void StartFollowing()
        {
            countThreadControllerFollowerPoster = 0;
            try
            {
                int numberOfAccountPatch = 25;

                if (NoOfThreadsFollowerPoster > 0)
                {
                    numberOfAccountPatch = NoOfThreadsFollowerPoster;
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
                                lock (lockrThreadControlleFollowerPoster)
                                {
                                    try
                                    {
                                        if (countThreadControllerFollowerPoster >= listAccounts.Count)
                                        {
                                            Monitor.Wait(lockrThreadControlleFollowerPoster);
                                        }

                                        string acc = account.Remove(account.IndexOf(':'));

                                        //Run a separate thread for each account
                                        InstagramUser item = null;
                                        IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                                        if (item != null)
                                        {
                                            Thread profilerThread = new Thread(StartMultiThreadsFollowerModule);
                                            profilerThread.Name = "workerThread_Profiler_" + acc;
                                            profilerThread.IsBackground = true;

                                            profilerThread.Start(new object[] { item });

                                            countThreadControllerFollowerPoster++;
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

        public void StartMultiThreadsFollowerModule(object parameters)
        {
            try
            {
                if (!isStopFollowerPoster)
                {
                    try
                    {
                        lstThreadsFollowerPoster.Add(Thread.CurrentThread);
                        lstThreadsFollowerPoster.Distinct();
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
                            StartActionFollowerModule(ref objFacebookUser);
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
                        lock (lockrThreadControlleFollowerPoster)
                        {
                            countThreadControllerFollowerPoster--;
                            Monitor.Pulse(lockrThreadControlleFollowerPoster);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }

        private void StartActionFollowerModule(ref InstagramUser fbUser)
        {

            try
            {
                if (IsFollow == true)
                {
                    Start_Follow(ref fbUser);
                }
                if(IsUnFollow == true)
                {
                    Start_Unfollow(ref fbUser);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void Start_Follow(ref InstagramUser obj_follow)
        {
            try
            {
                lstThreadsFollowerPoster.Add(Thread.CurrentThread);
                lstThreadsFollowerPoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                if (string.IsNullOrEmpty(UserName_path))
                {
                    if (!string.IsNullOrEmpty(txt_UserName))
                    {
                        ClGlobul.followingList.Clear();

                        string s = txt_UserName;

                        if (s.Contains(','))
                        {
                            string[] Data = s.Split(',');

                            foreach (var item in Data)
                            {
                                ClGlobul.followingList.Add(item);
                            }
                        }
                        else
                        {
                            ClGlobul.followingList.Add(txt_UserName);
                        }
                    }
                }
                if (status == "Success")
                {
                    getFollow(ref obj_follow);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void getFollow(ref InstagramUser obj_folow)
        {
            try
            {
                lstThreadsFollowerPoster.Add(Thread.CurrentThread);
                lstThreadsFollowerPoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                GlobusHttpHelper obj = obj_folow.globusHttpHelper;
               // string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                string res_resonce = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                if (ClGlobul.followingList.Count != 0)
                {
                    //foreach (string followingList_item in ClGlobul.followingList) //commented when divide data implemented.
                    int maximumNoOfCount = 0;
                    try
                    {
                        if (No_Follow_User != 0)
                        {
                            maximumNoOfCount = No_Follow_User;

                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please Enter Maximum No. of count of Follwers To Be Followed ]");
                            return;
                        }



                        if (No_Follow_User != 0)
                        {
                            maximumNoOfCount = No_Follow_User;
                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please Enter Maximum No. of count of Follwers To Be Followed ]");
                            return;

                        }
                    }
                    catch { };
                    int CountOfFollowersInForeach = 0;
                    int  count = 0; 
                    counter_follow = IGGlobals.listAccounts.Count();

                    foreach (string followingList_item in ClGlobul.followingList)
                    {
                        try
                        {
                            //------------------------------------------------------------------------------------------//

                            // for finding list of following form Iconsqure.
                            string FollowerName = followingList_item;

                            string Home_icon_Url = obj.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                            string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                            string PPagesource = obj.getHtmlfromUrl(new Uri(Icon_url), "");
                            string responce_icon = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");
                           
                            string view_UrlFollowing = obj.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php#/myFollowings/"), "");
                            string ID = Utils.getBetween(responce_icon, "<div id=\"accesstoken\" style=\"display:none;\">", "</div>");
                            string ID_Nxt = Utils.getBetween(ID, "", ".");
                            string refer = " http://iconosquare.com/viewer.php";
                            string post_Url = "http://iconosquare.com/rqig.php?e=/users/"+ID_Nxt+"/follows&a=ico2&t="+ID+"&count=20";
                            string responce_list = obj.getHtmlfromUrl(new Uri(post_Url), refer);
                            string[] scrape_USername = Regex.Split(responce_list, "username");
                            foreach(string item in scrape_USername)
                            {
                                if(item.Contains("profile_picture"))
                                {
                                    string username = Utils.getBetween(item, ":\"", "\"");
                                    AllreadyfollowingList.Add(username);
                                }
                            }

                            if (responce_list.Contains("next_url"))
                            {
                                value = true;
                                while (value)
                                {
                                    if (responce_list.Contains("next_url"))
                                    {
                                        string next_pageurl_token = Utils.getBetween(responce_list, "next_cursor\":\"", "\"},");
                                        string page_Url = post_Url + "&cursor=" + next_pageurl_token;
                                        responce_list = obj.getHtmlfromUrl(new Uri(page_Url), refer);
                                        string[] data = Regex.Split(responce_list, "username");
                                        foreach (string item in scrape_USername)
                                        {
                                            if (item.Contains("profile_picture"))
                                            {
                                                string username = Utils.getBetween(item, ":\"", "\"");
                                                AllreadyfollowingList.Add(username);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        value = false;
                                    }
                                }
                            }


                       







                           // ------------------------------------------------------------------------------------------//
                            #region already Following
                            //string FollowerName = followingList_item;
                            //string HomeAcc_Url = "http://websta.me/n/" + obj_folow.username;
                            //string res_data = obj.getHtmlfromUrl(new Uri(HomeAcc_Url), "");
                            //string Data = Utils.getBetween(res_data, "ul class=\"list-inline user", "</ul>");
                            //string Data1 = Utils.getBetween(Data, "<a href=\"/follows/", "\"><span class=");
                            //string following_url = "http://websta.me/follows/" + Data1;
                            //string res_data1 = obj.getHtmlfromUrl(new Uri(following_url), "");
                            //string[] split_data = Regex.Split(res_data1, "div class=\"pull-left");
                            //foreach (string item in split_data)
                            //{
                            //    string user_following = Utils.getBetween(item, "a href=\"/n/", "\"");
                            //    AllreadyfollowingList.Add(user_following);
                            //}
                            //if (res_data1.Contains("Next Page"))
                            //{
                            //    value = true;
                            //    while (value)
                            //    {
                            //        if (res_data1.Contains("Next Page"))
                            //        {
                            //            string nextpage_Url = Utils.getBetween(res_data1, "<ul class=\"pager\">", "</ul>");
                            //            string[] page_split = Regex.Split(nextpage_Url, "<a href=");
                            //            string next = Utils.getBetween(page_split[2], "\"", "\"> Next Page");
                            //            string finalNext_FollowingURL = "http://websta.me" + next;
                            //            Page_Url.Add(finalNext_FollowingURL);
                            //            res_data1 = obj.getHtmlfromUrl(new Uri(finalNext_FollowingURL), "");
                            //            string[] split_data1 = Regex.Split(res_data1, "div class=\"pull-left");
                            //            foreach (string item in split_data1)
                            //            {
                            //                string user_following = Utils.getBetween(item, "a href=\"/n/", "\"");
                            //                AllreadyfollowingList.Add(user_following);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            value = false;
                            //        }
                            //    }
                            //}
                            #endregion



                            string Result = string.Empty;
                            if (!AllreadyfollowingList.Contains(FollowerName))
                            {
                                if (count < maximumNoOfCount)
                                {
                                    Result = Follow(FollowerName, ref obj_folow);
                                    count++;
                                }
                            }
                            else
                            {
                                //GlobusLogHelper.log.Info(" Already User Following");

                            }
                            if (CountOfFollowersInForeach >= maximumNoOfCount)
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Maximum No of Followers are Followed . Its completed with username - " + obj_folow.username + " ]");
                                return;
                            }

                            if (Result == "Followed")
                            {
                                CountOfFollowersInForeach++;
                                ClGlobul.TotalNoOfFollow++;
                                DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime ,FollowerName,Status) values('" + "FollowModule" + "','" + obj_folow.username + "','" + DateTime.Now + "','" + FollowerName + "','" + Result + "')", "tbl_AccountReport");
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [" + obj_folow.username + " Followed " + FollowerName + " , " + "Count" + CountOfFollowersInForeach + " ]");
                                try
                                {
                                    string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator";
                                    path_AppDataFolder = path_AppDataFolder + "\\FollwedList";
                                    if (!File.Exists(path_AppDataFolder))
                                    {
                                        Directory.CreateDirectory(path_AppDataFolder);
                                    }
                                    string FollowIDFilePath = path_AppDataFolder + "\\" + obj_folow.username + ".csv";
                                    string CSV_Header = "Username,FollowerName,Followed/Requested";
                                    string CSV_Content = obj_folow.username.Replace(",", "") + "," + FollowerName.Replace(",", "") + "," + Result;
                                    GlobusFileHelper.ExportDataCSVFile(CSV_Header, CSV_Content, FollowIDFilePath);
                                }
                                catch { };
                                if (minDelayFollowerPoster != 0)
                                {
                                    mindelay = minDelayFollowerPoster;
                                }
                                if (maxDelayFollowerPoster != 0)
                                {
                                    maxdelay = maxDelayFollowerPoster;
                                }
                                
                                    Random FolloweRandom = new Random();
                                    int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                   // delay = FolloweRandom.Next(mindelay, maxdelay);
                                    
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                                    Thread.Sleep(delay * 1000);
                               

                                if (!followingList.Contains(obj_folow.username))
                                {
                                    followingList.Add(obj_folow.username);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("Followed: " + FollowerName + " By: " + obj_folow.username + ":" + obj_folow.password, GlobusFileHelper.FollowIDFilePath);
                                }
                                List<string> StrListReadData = new List<string>();
                                try
                                {
                                    try
                                    {
                                        StreamReader strReader = new StreamReader(ClGlobul.FollowerListUploadedPath);
                                        string text = "";
                                        while ((text = strReader.ReadLine()) != null)
                                        {                                  //string strUserListFromFilePath = strReader.ReadToEnd();
                                            StrListReadData.Add(text);
                                        }
                                        strReader.Close();
                                        if (StrListReadData.Contains(FollowerName))
                                        {
                                            try
                                            {
                                                StrListReadData.Remove(FollowerName);
                                            }
                                            catch { };
                                        }
                                    }
                                    catch { };
                                    try
                                    {
                                        StreamWriter strWriter = new StreamWriter(ClGlobul.FollowerListUploadedPath);
                                        strWriter.Write("");
                                        if (StrListReadData.Count() > 0)
                                        {
                                            foreach (string itemStr in StrListReadData)
                                            {
                                                try
                                                {
                                                    strWriter.WriteLine(itemStr);
                                                }
                                                catch { };
                                            }
                                        }
                                        strWriter.Close();
                                    }
                                    catch { };
                                }
                                catch { };

                                // AddToUnfollowLogger("[ " + DateTime.Now + " ] => [ " + FollowerName + " Removed From TextFile -  " + ClGlobul.FollowerListUploadedPath + " ]");

                            }



                            else if (Result == "private")
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Followed: " + FollowerName + " is a private user and can not be followed. ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(obj_folow.username + ":" + obj_folow.password, GlobusFileHelper.FollowedOptionNotAvailableFilePath);
                                 if (minDelayFollowerPoster != 0)
                                {
                                    mindelay = minDelayFollowerPoster;
                                }
                                if (maxDelayFollowerPoster != 0)
                                {
                                    maxdelay = maxDelayFollowerPoster;
                                }
                                
                                    Random FolloweRandom = new Random();
                                    int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                    // delay = FolloweRandom.Next(mindelay, maxdelay);
                                    Thread.Sleep(delay * 1000);
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                                
                            }
                            else if (Result == "requested")
                            {
                                CountOfFollowersInForeach++;
                                DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime, FollowerName,Status) values('" + "FollowModule" + "','" + obj_folow.username + "','" + DateTime.Now + "','" + FollowerName + "','" + Result + "')", "tbl_AccountReport");
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ By " + obj_folow.username + " Request has been sent to . " + FollowerName + "Count" + CountOfFollowersInForeach + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(obj_folow.username + ":" + obj_folow.password + ":" + " Requested", GlobusFileHelper.hasBeenRequestedFilePath);
                                try
                                {
                                    string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator";

                                    path_AppDataFolder = path_AppDataFolder + "\\FollwedList";
                                    if (!File.Exists(path_AppDataFolder))
                                    {
                                        Directory.CreateDirectory(path_AppDataFolder);
                                    }
                                    string FollowIDFilePath = path_AppDataFolder + "\\" + obj_folow.username + ".csv";
                                    string CSV_Header = "Username,FollowerName,Followed/Requested";
                                    string CSV_Content = obj_folow.username.Replace(",", "") + "," + FollowerName.Replace(",", "") + "," + Result;
                                    GlobusFileHelper.ExportDataCSVFile(CSV_Header, CSV_Content, FollowIDFilePath);
                                    if (minDelayFollowerPoster != 0)
                                    {
                                        mindelay = minDelayFollowerPoster;
                                    }
                                    if (maxDelayFollowerPoster != 0)
                                    {
                                        maxdelay = maxDelayFollowerPoster;
                                    }
                                    
                                        Random FolloweRandom = new Random();
                                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                        delay = FolloweRandom.Next(mindelay, maxdelay);
                                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                                        Thread.Sleep(delay * 1000);
                                                                    }
                                catch { };

                                List<string> StrListReadData = new List<string>();
                                try
                                {
                                    try
                                    {
                                        StreamReader strReader = new StreamReader(ClGlobul.FollowerListUploadedPath);
                                        string text = "";
                                        while ((text = strReader.ReadLine()) != null)
                                        {                                  //string strUserListFromFilePath = strReader.ReadToEnd();
                                            StrListReadData.Add(text);
                                        }
                                        strReader.Close();
                                        if (StrListReadData.Contains(FollowerName))
                                        {
                                            try
                                            {
                                                StrListReadData.Remove(FollowerName);
                                            }
                                            catch { };
                                        }
                                    }
                                    catch { };
                                    try
                                    {
                                        StreamWriter strWriter = new StreamWriter(ClGlobul.FollowerListUploadedPath);
                                        strWriter.Write("");
                                        if (StrListReadData.Count() > 0)
                                        {
                                            foreach (string itemStr in StrListReadData)
                                            {
                                                try
                                                {
                                                    strWriter.WriteLine(itemStr);
                                                }
                                                catch { };
                                            }
                                        }
                                        strWriter.Close();
                                    }
                                    catch { };
                                }
                                catch { };
                                //AddToUnfollowLogger("[ " + DateTime.Now + " ] => [ " + FollowerName + " Removed From TextFile -  " + ClGlobul.FollowerListUploadedPath + " ]");

                            }
                            else if (Result == "Already Followed")
                            {
                                ClGlobul.TotalNoOfFollow++;
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Account:" + obj_folow.username + ClGlobul.TotalNoOfFollow + " Already Followed " + FollowerName + " ]");

                                if (!AlreadyFollowedlist.Contains(obj_folow.username))
                                {
                                    AlreadyFollowedlist.Add(obj_folow.username);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("Already Followed: " + FollowerName + " By: " + obj_folow.username + ":" + obj_folow.password, GlobusFileHelper.AllReadyFollowedIdFilePath);
                                }
                                if (minDelayFollowerPoster != 0)
                                {
                                    mindelay = minDelayFollowerPoster;
                                }
                                if (maxDelayFollowerPoster != 0)
                                {
                                    maxdelay = maxDelayFollowerPoster;
                                }
                                
                                    Random FolloweRandom = new Random();
                                    int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                    delay = FolloweRandom.Next(mindelay, maxdelay);
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                                    Thread.Sleep(delay * 1000);
                                

                                List<string> StrListReadData = new List<string>();
                                try
                                {
                                    try
                                    {
                                        StreamReader strReader = new StreamReader(ClGlobul.FollowerListUploadedPath);
                                        string text = "";
                                        while ((text = strReader.ReadLine()) != null)
                                        {                                  //string strUserListFromFilePath = strReader.ReadToEnd();
                                            StrListReadData.Add(text);
                                        }
                                        strReader.Close();
                                        if (StrListReadData.Contains(FollowerName))
                                        {
                                            try
                                            {
                                                StrListReadData.Remove(FollowerName);
                                            }
                                            catch { };
                                        }
                                    }
                                    catch { };
                                    try
                                    {
                                        StreamWriter strWriter = new StreamWriter(ClGlobul.FollowerListUploadedPath);
                                        strWriter.Write("");
                                        if (StrListReadData.Count() > 0)
                                        {
                                            foreach (string itemStr in StrListReadData)
                                            {
                                                try
                                                {
                                                    strWriter.WriteLine(itemStr);
                                                }
                                                catch { };
                                            }
                                        }
                                        strWriter.Close();
                                    }
                                    catch { };
                                }
                                catch { };

                                // AddToUnfollowLogger("[ " + DateTime.Now + " ] => [ " + FollowerName + " Removed From TextFile -  " + ClGlobul.FollowerListUploadedPath + " ]");



                            }
                            else if (Result == "Follow option is not available In page...!!")
                            {

                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Follow option is not available In page...!!" + obj_folow.username + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(obj_folow.username + ":" + obj_folow.password, GlobusFileHelper.FollowedOptionNotAvailableFilePath);
                            }
                            else if(Result == "Instagram API does not respond")
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Instagram API does not respond to follow...!!" + FollowerName + " ]");
                            }
                            else if(Result == "User is Private")
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ User is Private " + FollowerName + " ]");
                            }
                            else
                            {

                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + obj_folow.username + " Already Followed " + FollowerName + " ]");


                                if (!NotFollowedlist.Contains(obj_folow.username))
                                {
                                    NotFollowedlist.Add(obj_folow.username);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(obj_folow.username + ":" + obj_folow.password, GlobusFileHelper.UnFollowIdFilePath);
                                }
                            }

                           
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                else
                {

                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please upload Following ID's ]");
                }
            }

            catch (Exception ex)
            {

            }


            finally
            {
                ClGlobul.FolloConpletedList.Add(obj_folow.username + ":" + obj_folow.password);
                ClGlobul.TotalNoOfIdsForFollow--;

                No_Follow_User--;
                if (No_Follow_User == 0)
               {
                    GlobusLogHelper.log.Info("-----------------------------------------------------------------------------------------------");
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    GlobusLogHelper.log.Info("-----------------------------------------------------------------------------------------------");
              }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                try
                {
                    string UserName = obj_folow.username.ToString();
                    var DicValue = ClGlobul.ThreadList.Single(s => s.Key.Contains(UserName));
                    Thread value = DicValue.Value;
                    if (value.IsAlive || value.IsBackground)
                    {
                        value = null;
                    }
                }
                catch (Exception ex)
                {


                }
            }

        }

        public string Follow(string UserName, ref InstagramUser accountManager)
        {
            try
            {
                lstThreadsFollowerPoster.Add(Thread.CurrentThread);
                lstThreadsFollowerPoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            GlobusHttpHelper obj = accountManager.globusHttpHelper;
           // string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
            string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");

            if (!UserName.Contains(IGGlobals.Instance.IGWEP_HomePage))
            {
                //UserName = IGGlobals.Instance.IGWEP_HomePage + UserName + "/";
                UserName = "https://www.instagram.com/" + UserName + "/";
            }
            string UserPageContent = string.Empty;

            if (!string.IsNullOrEmpty(accountManager.proxyip) && !string.IsNullOrEmpty(accountManager.proxyport))
            {
                try
                {
                    UserPageContent = accountManager.globusHttpHelper.getHtmlfromUrlProxy(new Uri(UserName), accountManager.proxyip, Convert.ToInt32(accountManager.proxyport), accountManager.proxyusername, accountManager.proxypassword);
                }
                catch (Exception ex)
                {

                }
                if (string.IsNullOrEmpty(UserPageContent))
                {
                    Thread.Sleep(1000);
                    try
                    {
                        UserPageContent = accountManager.globusHttpHelper.getHtmlfromUrlProxy(new Uri(UserName), accountManager.proxyip, Convert.ToInt32(accountManager.proxyport), accountManager.proxyusername, accountManager.proxypassword);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                try
                {
                    UserPageContent = accountManager.globusHttpHelper.getHtmlfromUrlProxy(new Uri(UserName), "", 80, "", "");
                }
                catch { };

                if (string.IsNullOrEmpty(UserPageContent))
                {
                    Thread.Sleep(1000);
                    try
                    {
                        UserPageContent = accountManager.globusHttpHelper.getHtmlfromUrlProxy(new Uri(UserName), "", 80, "", "");
                    }
                    catch { };
                }
            }

            if (chkNotSendRequest == true)
            {
                if (UserPageContent.Contains("\"is_private\":true"))
                {               
                    return "User is Private";
                }
            }


            try
            {
                //if (UserPageContent.Contains("This user is private."))
                //{
                //    return "private";
                //}
                string PK = string.Empty;
                if (UserPageContent.Contains(""))
                {
                   // PK = Utils.getBetween(UserPageContent, "id=\"follow_btn_wrapper\"", ">").Replace("data-target=", "").Replace("\"", "").Trim();
                    PK = Utils.getBetween(UserPageContent, "\"id\":", "\",").Replace("\"","");
                }

                if (string.IsNullOrEmpty(PK))
                {
                    PK = Utils.getBetween(UserPageContent, "id=\"message_user_id", ">").Replace(">", "").Replace("value=", string.Empty).Replace("\"", string.Empty).Trim();//.Replace("\"", "").Trim();
                }

                string PostData = "action=follow";//"&pk=" + PK + "&t=9208";
                string postData = "https://www.instagram.com/web/friendships/" + PK + "/follow/";
                string FollowedPageSource = string.Empty;

                if (!string.IsNullOrEmpty(PK))
                {
                    try
                    {
                        string test = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(UserName));
                        string txt_name = Utils.getBetween(UserName,"www.instagram.com/","/");
                        string csrf_token = Utils.getBetween(UserPageContent, "csrf_token\":\"", "\"}");

                        #region Commented
                        // FollowedPageSource = accountManager.globusHttpHelper.postFormDataFollower(new Uri(IGGlobals.Instance.IGFollowapiUrl + PK), PostData, UserName,"");
                        //string cookies = "mid=VlgP7AAEAAGek4AP2tkKKkB1nVop; sessionid=IGSC23aaf0fd3e37cbe487ed42ff34c9b6fd2aafa7dffcc24d2174e46d716f61e7f3%3AViHt3HzffpuWQuVIIK3juvKlSeULsC7H%3A%7B%22_token_ver%22%3A2%2C%22_auth_user_id%22%3A2212052873%2C%22_token%22%3A%222212052873%3A00vc2gt5Bu7HtXh3McR53VUbU3xoUd4J%3Af29a940088bddd8e5c3d7411bcb32a7bac726f5ab580e8b5cdb58528a9b5f5df%22%2C%22_auth_user_backend%22%3A%22accounts.backends.CaseInsensitiveModelBackend%22%2C%22last_refreshed%22%3A1450426011.547766%2C%22_platform%22%3A4%7D; s_network=; ig_pr=1; ig_vw=294; csrftoken=da65eda4fbe59f98d4f7d4d9664af428; ds_user_id=2212052873";
                        //string[] dslfjsdlk = Regex.Split(cookies, ";");
                        //accountManager.globusHttpHelper.gCookies = new System.Net.CookieCollection();
                        //foreach (string item in dslfjsdlk)
                        //{
                        //    try
                        //    {

                        //        System.Net.Cookie cookies12 = new System.Net.Cookie();

                        //        cookies12.Name = Regex.Split(item, "=")[0].Replace(" ", "");
                        //        cookies12.Value = Regex.Split(item, "=")[1].Replace(" ", "");
                        //        cookies12.Domain = "instagram.com";

                        //        accountManager.globusHttpHelper.gCookies.Add(cookies12);



                        //    }
                        //    catch { };


                        //}

                        #endregion

                        FollowedPageSource = accountManager.globusHttpHelper.postFormDatainta(new Uri(postData), "", "https://www.instagram.com/" + txt_name + "/", csrf_token);
                    }
                    catch { }
                }
                if (string.IsNullOrEmpty(FollowedPageSource))
                {

                }
                

                if (FollowedPageSource.Contains("followed_by_viewer\":true") || FollowedPageSource.Contains("requested_by_viewer\":true"))
                {
                    
                    string status = string.Empty;
                    try
                    {
                        status = QueryExecuter.getFollowStatus1(accountManager.username, UserName);
                    }
                    catch { }
                    if (string.IsNullOrEmpty(status))
                    {
                        if (FollowedPageSource.Contains("has_requested_viewer\":true") || FollowedPageSource.Contains("requested_by_viewer\":true"))
                        {
                            status = "requested";
                        }
                        if(FollowedPageSource.Contains("followed_by_viewer\":true"))
                        {
                            status = "Followed";
                        }
                       
                    }
                    switch (status)
                    {
                        case "Followed":  //status = "Followed";
                            QueryExecuter.updateFollowStatus(accountManager.username, UserName, "Unfollowed");
                            break;

                        case "Unfollowed": status = "Unfollowed";
                            QueryExecuter.updateFollowStatus(accountManager.username, UserName, "Unfollowed");
                            break;

                        case "requested": status = "requested";
                            QueryExecuter.updateFollowStatus(accountManager.username, UserName, "requested");
                            break;
                        default: status = "Followed";
                            try
                            {
                                QueryExecuter.insertFollowInfo(accountManager.username, UserName, "Followed");
                            }
                            catch { }
                            break;
                    }
                    return status;
                }
                if (FollowedPageSource.Contains("Instagram API does not respond"))
                    {
                        return "Instagram API does not respond";
                    }
                else
                {
                    return "UnFollowed";
                }
            }
            catch (Exception)
            {
                return "Follow option is not available In page...!!";
            }


        }


        public void Start_Unfollow(ref InstagramUser Obj_Unfollow)
        {
            try
            {
                lstThreadsFollowerPoster.Add(Thread.CurrentThread);
                lstThreadsFollowerPoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                GlobusHttpHelper obj = Obj_Unfollow.globusHttpHelper;
               // string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                if (string.IsNullOrEmpty(UserName_path_Unfollow))
                {
                    if (!string.IsNullOrEmpty(txt_UserName_Unfollow))
                    {
                        ClGlobul.UnfollowingList.Clear();

                        string s = txt_UserName_Unfollow;

                        if (s.Contains(','))
                        {
                            string[] Data = s.Split(',');

                            foreach (var item in Data)
                            {
                                ClGlobul.lstUnfollowerList.Add(item);
                            }
                        }
                        else
                        {
                            ClGlobul.lstUnfollowerList.Add(txt_UserName_Unfollow);
                        }
                    }
                }
                if (Obj_Unfollow.isloggedin == true)
                {
                    try
                    {
                        if (No_UnFollow_User != 0)
                        {
                            NoOfMaximumNoOfUnfollow = No_UnFollow_User;  //

                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please Enter Maximum No. of count of Follwers To Be Folllowed ]");
                            return;

                        }
                    }
                    catch { };
                    int Data_count = 0;
                    int sum = NoOfMaximumNoOfUnfollow ;
                    foreach (string item in ClGlobul.lstUnfollowerList)
                    {
                        try
                        {


                            Data_count = Data_count + 1;
                            if (Data_count > sum)
                            {
                                //AddToLogger("[ " + DateTime.Now + " ] => [ Maximum No of UnFollowers are UnFollowed . Its completed with username - " + AccountName + " ]");
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Maximum No of UnFollowers are All Ready UnFollowed . Its completed with username - " + Obj_Unfollow.username + " ]");
                                break;

                            }

                            try
                            {
                                unfollowAccount(ref Obj_Unfollow, item, Data_count);
                               // count++;
                            }
                            catch { };

                            if (minDelayUnFollowerPoster != 0)
                            {
                                mindelay = minDelayUnFollowerPoster;
                            }
                            if (maxDelayUnFollowerPoster != 0)
                            {
                                maxdelay = maxDelayUnFollowerPoster;
                            }

                           
                                Random rn = new Random();

                                int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                delay = rn.Next(mindelay, maxdelay);
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                                Thread.Sleep(delay * 1000);                           
                         }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }

                    }
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [Process Completed from " + Obj_Unfollow.username + " ]");

                }
            }

            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }


        public void unfollowAccount(ref InstagramUser accountManager, string account, int count)
        {
            try
            {
                lstThreadsFollowerPoster.Add(Thread.CurrentThread);
                lstThreadsFollowerPoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            GlobusHttpHelper obj = accountManager.globusHttpHelper;
            //   string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
            string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
            string pageSource = string.Empty;
            string response = string.Empty;
            string profileId = string.Empty;
            const string websta = "http://websta.me/api/relationships/";
            // const string accountUrl = "http://websta.me/n/";
            const string accountUrl = "https://www.instagram.com/";
            try
            {
                try
                {
                    if (account.Contains(IGGlobals.Instance.IGWEP_HomePage))
                    {
                        pageSource = accountManager.globusHttpHelper.getHtmlfromUrlProxy(new Uri(account), "", 80, "", "");

                    }
                    else
                    {
                        pageSource = accountManager.globusHttpHelper.getHtmlfromUrlProxy(new Uri(accountUrl + account), "", 80, "", "");
                    }
                }
                catch { };
                if(pageSource.Contains("followed_by_viewer\":false") && pageSource.Contains("requested_by_viewer\":false"))
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [Allready Unfollowed " + account + " from " + accountManager.username + " Count" + count + "]");
                    return;
                }
                else if (!string.IsNullOrEmpty(pageSource))
                {
                    try
                    {
                        // string test = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(UserName));
                        // string txt_name = Utils.getBetween(UserName, "www.instagram.com/", "/");
                        string csrf_token = Utils.getBetween(pageSource, "csrf_token\":\"", "\"");
                        string PK = Utils.getBetween(pageSource, "\"id\":", "\",").Replace("\"", "");
                        string postdata = "https://www.instagram.com/web/friendships/" + PK + "/unfollow/";

                        response = accountManager.globusHttpHelper.postFormDatainta(new Uri(postdata), "", "https://www.instagram.com/" + account + "/", csrf_token);
                    }
                    catch (Exception ex)
                    {

                    }


                    if (!string.IsNullOrEmpty(response) && response.Contains("ok"))
                    {
                        if (_boolStopUnfollow) return;
                        DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime ,UnfollowUser,Status) values('" + "UnfollowModule" + "','" + accountManager.username + "','" + DateTime.Now + "','" + account + "','" + "Success" + "')", "tbl_AccountReport");
                        string status = string.Empty;
                        try
                        {
                            status = QueryExecuter.getFollowStatus(accountManager.username, account);



                            switch (status)
                            {
                                //case "Followed": QueryExecuter.updateFollowStatus(accountManager.Username, account, "Unfollowed");
                                case "Followed": QueryExecuter.updateFollowStatus(accountManager.username, account, "Unfollowed");
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Unfollowed " + account + " from " + accountManager.username + " Count" + count + "]");
                                    try
                                    {
                                        //  NoOfUnFollowCompleted++;
                                        string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator";
                                        path_AppDataFolder = path_AppDataFolder + "\\UnFollwedList";
                                        if (!File.Exists(path_AppDataFolder))
                                        {
                                            Directory.CreateDirectory(path_AppDataFolder);
                                        }

                                        string FollowIDFilePath = path_AppDataFolder + "\\" + accountManager.username + ".csv";
                                        string CSV_Header = "Username,UnFollowerName,Unfollowed";
                                        string CSV_Content = accountManager.username.Replace(",", "") + "," + account.Replace(",", "") + "," + status;
                                        GlobusFileHelper.ExportDataCSVFile(CSV_Header, CSV_Content, FollowIDFilePath);
                                        List<string> StrListReadData = new List<string>();
                                        try
                                        {
                                            try
                                            {
                                                StreamReader strReader = new StreamReader(ClGlobul.UnFollowerListUploadedPath);
                                                string text = "";
                                                while ((text = strReader.ReadLine()) != null)
                                                {                                  //string strUserListFromFilePath = strReader.ReadToEnd();
                                                    StrListReadData.Add(text);
                                                }
                                                strReader.Close();
                                                if (StrListReadData.Contains(account))
                                                {
                                                    try
                                                    {
                                                        StrListReadData.Remove(account);
                                                    }
                                                    catch { };
                                                }
                                            }
                                            catch { };
                                            try
                                            {
                                                StreamWriter strWriter = new StreamWriter(ClGlobul.UnFollowerListUploadedPath);
                                                strWriter.Write("");
                                                if (StrListReadData.Count() > 0)
                                                {
                                                    foreach (string itemStr in StrListReadData)
                                                    {
                                                        try
                                                        {
                                                            strWriter.WriteLine(itemStr);
                                                        }
                                                        catch { };
                                                    }
                                                }
                                                strWriter.Close();
                                            }
                                            catch { };
                                        }
                                        catch { };

                                        //AddToUnfollowLogger("[ " + DateTime.Now + " ] => [ " + account + " Removed From TextFile -  " + ClGlobul.UnFollowerListUploadedPath + " ]");
                                    }
                                    catch { };
                                    break;
                                case "Unfollowed":
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ All Ready UnFollowed " + account + " from " + accountManager.username + "]");
                                    break;
                                    try
                                    {
                                        //  NoOfUnFollowCompleted++;
                                        string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator";
                                        path_AppDataFolder = path_AppDataFolder + "\\UnFollwedList";
                                        if (!File.Exists(path_AppDataFolder))
                                        {
                                            Directory.CreateDirectory(path_AppDataFolder);
                                        }

                                        string FollowIDFilePath = path_AppDataFolder + "\\" + accountManager.username + ".csv";
                                        string CSV_Header = "Username,UnFollowerName,Unfollowed";
                                        string CSV_Content = accountManager.username.Replace(",", "") + "," + account.Replace(",", "") + "," + status;
                                        GlobusFileHelper.ExportDataCSVFile(CSV_Header, CSV_Content, FollowIDFilePath);
                                        List<string> StrListReadData = new List<string>();
                                        try
                                        {
                                            try
                                            {
                                                StreamReader strReader = new StreamReader(ClGlobul.UnFollowerListUploadedPath);
                                                string text = "";
                                                while ((text = strReader.ReadLine()) != null)
                                                {                                  //string strUserListFromFilePath = strReader.ReadToEnd();
                                                    StrListReadData.Add(text);
                                                }
                                                strReader.Close();
                                                if (StrListReadData.Contains(account))
                                                {
                                                    try
                                                    {
                                                        StrListReadData.Remove(account);
                                                    }
                                                    catch { };
                                                }
                                            }
                                            catch { };
                                            try
                                            {
                                                StreamWriter strWriter = new StreamWriter(ClGlobul.UnFollowerListUploadedPath);
                                                strWriter.Write("");
                                                if (StrListReadData.Count() > 0)
                                                {
                                                    foreach (string itemStr in StrListReadData)
                                                    {
                                                        try
                                                        {
                                                            strWriter.WriteLine(itemStr);
                                                        }
                                                        catch { };
                                                    }
                                                }
                                                strWriter.Close();
                                            }
                                            catch { };
                                        }
                                        catch { };

                                        //AddToUnfollowLogger("[ " + DateTime.Now + " ] => [ " + account + " Removed From TextFile -  " + ClGlobul.UnFollowerListUploadedPath + " ]");
                                    }
                                    catch { };
                                    break;
                                default:
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + accountManager.username + " Unfollowed " + account + " Count" + count + "]"); //account
                                    QueryExecuter.updateFollowStatus(accountManager.username, account, "Unfollowed");

                                    //  NoOfUnFollowCompleted++;
                                    try
                                    {
                                        string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator";
                                        path_AppDataFolder = path_AppDataFolder + "\\UnFollwedList";
                                        if (!File.Exists(path_AppDataFolder))
                                        {
                                            Directory.CreateDirectory(path_AppDataFolder);
                                        }

                                        string FollowIDFilePath = path_AppDataFolder + "\\" + accountManager.username + ".csv";
                                        string CSV_Header = "Username,UnFollowerName";
                                        string CSV_Content = accountManager.username.Replace(",", "") + "," + account.Replace(",", "");
                                        GlobusFileHelper.ExportDataCSVFile(CSV_Header, CSV_Content, FollowIDFilePath);
                                        List<string> StrListReadData = new List<string>();
                                        try
                                        {
                                            try
                                            {
                                                StreamReader strReader = new StreamReader(ClGlobul.UnFollowerListUploadedPath);
                                                string text = "";
                                                while ((text = strReader.ReadLine()) != null)
                                                {                                  //string strUserListFromFilePath = strReader.ReadToEnd();
                                                    StrListReadData.Add(text);
                                                }
                                                strReader.Close();
                                                if (StrListReadData.Contains(account))
                                                {
                                                    try
                                                    {
                                                        StrListReadData.Remove(account);
                                                    }
                                                    catch { };
                                                }
                                            }
                                            catch { };
                                            try
                                            {
                                                StreamWriter strWriter = new StreamWriter(ClGlobul.UnFollowerListUploadedPath);
                                                strWriter.Write("");
                                                if (StrListReadData.Count() > 0)
                                                {
                                                    foreach (string itemStr in StrListReadData)
                                                    {
                                                        try
                                                        {
                                                            strWriter.WriteLine(itemStr);
                                                        }
                                                        catch { };
                                                    }
                                                }
                                                strWriter.Close();
                                            }
                                            catch { };
                                        }
                                        catch { };

                                        // AddToUnfollowLogger("[ " + DateTime.Now + " ] => [ " + account + " Removed From TextFile -  " + ClGlobul.UnFollowerListUploadedPath + " ]");


                                    }
                                    catch { };

                                    break;
                            }

                        }
                        catch { }

                        //AddToUnfollowLogger("[ " + DateTime.Now + " ] => [ Unfollowed " + account + " from " + accountManager.Username + " ]");
                    }
                    else
                    {
                        if (_boolStopUnfollow) return;
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Could not Unfollow " + account + " from " + accountManager.username + " Count" + count + "]");
                    }
                }
                else
                {
                    GlobusLogHelper.log.Info("Page Not Found");
                }
            }
            catch (Exception ex)
            {

            }       

        }
                    
             


        #region Divide data by User

        public void startFollowerdividedataUser()
        {
            try
            {
                if (!isStopFollowerPoster)
                {
                    try
                    {
                        lstThreadsFollowerPoster.Add(Thread.CurrentThread);
                        lstThreadsFollowerPoster.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }
                try
                {
                    if (IsFollow == true)
                    {
                        Start_DivideByUser_Follow();
                    }
                    if (IsUnFollow == true)
                    {
                       // Start_Unfollow(ref fbUser);
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }

            }
           catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
            
        }


        public void Start_DivideByUser_Follow()
        {
            try
            {
                lstThreadsFollowerPoster.Add(Thread.CurrentThread);
                lstThreadsFollowerPoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
            try
            {
                if (string.IsNullOrEmpty(UserName_path))
                {
                    if (!string.IsNullOrEmpty(txt_UserName))
                    {
                        ClGlobul.followingList.Clear();

                        string s = txt_UserName;

                        if (s.Contains(','))
                        {
                            string[] Data = s.Split(',');

                            foreach (var item in Data)
                            {
                                ClGlobul.followingList.Add(item);
                            }
                        }
                        else
                        {
                            ClGlobul.followingList.Add(txt_UserName);
                        }
                    }
                }


                /////////////////////////////////////////////////////////////////////


                int NumberAcoount = IGGlobals.listAccounts.Count;
               // int temp = NotFollowedlist / NumberAcoount;
               // int _checkTemp = temp;
                string _checkAcc = string.Empty;
                int count = 0;
                Dictionary<string, string> dicAccHash = new Dictionary<string, string>();
                Queue<string> queAcc = new Queue<string>();
                Queue<string> queList = new Queue<string>();
                foreach (string List_ID in ClGlobul.followingList)
                {
                    queList.Enqueue(List_ID);
                }
                foreach (string itemAcc in IGGlobals.listAccounts)
                {
                    queAcc.Enqueue(itemAcc);
                }
                foreach (string itemAcc in IGGlobals.listAccounts)
                {
                    count = 0;
                    queAcc.Dequeue();
                    foreach (string item_Id in ClGlobul.followingList)
                    {
                       if(count < Divide_data_NoUser)
                       { 
                        string Item_listid = queList.Dequeue();
                        //if (count < Divide_data_NoUser)
                        //{
                            if (!dicAccHash.ContainsKey(itemAcc))
                            {
                                dicAccHash.Add(itemAcc, Item_listid + ",");

                            }
                            else
                            {
                                dicAccHash[itemAcc] += Item_listid + ",";

                            }
                            count++;
                        }
                        else
                        {
                            break;
                        }
                        
                    }
                }


                foreach (var itemDic in dicAccHash)
                {
                    string[] arrHash = Regex.Split(itemDic.Value, ":");
                    Thread thrStart = new Thread(() => FollowOperation(itemDic.Key, arrHash));
                    thrStart.Start();
                }



            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        public void FollowOperation(string Acc, string[] Hash)
        {
            string status = string.Empty;
            string[] arrAcc = Regex.Split(Acc, ":");
            InstagramUser objInstagramUser = new InstagramUser(arrAcc[0], arrAcc[1], arrAcc[2], arrAcc[3]);
            GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();
            objInstagramUser.globusHttpHelper = objGlobusHttpHelper;
            AccountManager objAccountManager = new AccountManager();
            if (!objInstagramUser.isloggedin)
            {
                status = objAccountManager.LoginUsingGlobusHttp(ref objInstagramUser);
            }
            if (status == "Success" || (objInstagramUser.isloggedin))
            {

                foreach (string itemHash in Hash)
                {
                    if (!string.IsNullOrEmpty(itemHash))
                    {
                        //Operation
                        string[] Data_ID = Regex.Split(itemHash, ",");
                        string daaa = objInstagramUser.username;
                        foreach (string Photo_ID in Data_ID)
                        {
                            if (!string.IsNullOrEmpty(Photo_ID))
                            {
                                FollowUrls(ref objInstagramUser,Photo_ID);
                            }
                            else
                            {
                                break;
                            }

                            if (minDelayFollowerPoster != 0)
                            {
                                mindelay = minDelayFollowerPoster;
                            }
                            if (maxDelayFollowerPoster != 0)
                            {
                                maxdelay = maxDelayFollowerPoster;
                            }

                            Random obj_rn = new Random();
                            int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                            delay = obj_rn.Next(mindelay, maxdelay);
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);
                        }

                    }
                    GlobusLogHelper.log.Info("=========================");
                    GlobusLogHelper.log.Info("Process Completed !!!");
                    GlobusLogHelper.log.Info("=========================");
                }


            }
        }

             public void FollowUrls(ref InstagramUser accountManager, string url)
        {
            try
            {
                //lstThreadsHash_comment.Add(Thread.CurrentThread);
                //lstThreadsHash_comment.Distinct();
                //Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            string followStatus = string.Empty;
            try
            {
                string res_secondURL = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                string user = accountManager.username;
                try
                {
                    DataSet DS = DataBaseHandler.SelectQuery("Select FollowStatus from FollowInfo where AccountHolder='" + user + "' and FollowingUser='" + url + "'", "FollowInfo");
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        followStatus = DS.Tables[0].Rows[0].ItemArray[0].ToString();
                    }
                }
                catch (Exception ex)
                { }
                if (!(followStatus == "Followed"))
                {
                    if (!(No_Follow_User == ClGlobul.SnapVideosCounterfollow))
                    {

                        string status = Follow(url, ref accountManager);
                        // Thread.Sleep(ClGlobul.hashTagDelay * 1000);
                        No_Follow_User++;
                        if (status == "Followed")
                        {
                            DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime ,FollowerName,Status) values('" + "FollowModule" + "','" + accountManager.username + "','" + DateTime.Now + "','" + url + "','" + status + "')", "tbl_AccountReport");
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Profile followed with url : " + url + " with User = " + user + " , " + "Count" + No_Follow_User + "]");
                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Profile followed with url : " + url + " with User = " + user + " , " + "Count" + No_Follow_User + " ]");
                            //Log("[ " + DateTime.Now + "] " + " [ " + ClGlobul.NumberOfProfilesToFollow + " profiles Unfollowed ]");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        
        #endregion


        #region Dividedata Eqully


       public void StartDividedatabyequally()
       {
           try
           {
               lstThreadsFollowerPoster.Add(Thread.CurrentThread);
               lstThreadsFollowerPoster.Distinct();
               Thread.CurrentThread.IsBackground = true;
           }
           catch (Exception ex)
           {
               GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
           }
           try
           {              
                if (string.IsNullOrEmpty(UserName_path))
                {
                    if (!string.IsNullOrEmpty(txt_UserName))
                    {
                        ClGlobul.followingList.Clear();

                        string s = txt_UserName;

                        if (s.Contains(','))
                        {
                            string[] Data = s.Split(',');

                            foreach (var item in Data)
                            {
                                ClGlobul.followingList.Add(item);
                            }
                        }
                        else
                        {
                            ClGlobul.followingList.Add(txt_UserName);
                        }
                    }
                }
           }
           catch(Exception ex)
           {
               GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
           }

           StartFollowEquallydivide();
       }


        public void StartFollowEquallydivide()
       {
           try
           {
               lstThreadsFollowerPoster.Add(Thread.CurrentThread);
               lstThreadsFollowerPoster.Distinct();
               Thread.CurrentThread.IsBackground = true;
           }
           catch (Exception ex)
           {
               GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
           }
            try
            {
                if (ClGlobul.followingList.Count != 0)
                {
                    if (No_Follow_User != 0)
                    {
                        ClGlobul.NumberOfProfilesToFollow = No_Follow_User;
                        ClGlobul.SnapVideosCounterfollow = No_Follow_User * ClGlobul.followingList.Count;

                    }
                    else
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Please enter the numbers of profiles to follow and continue. ]");
                        return;
                    }

                }

                //List<string> lstHashTagUserIdTemp = new List<string>();
                //List<string> lstHashTagUserId = new List<string>();


                //foreach (string hashKeyword in ClGlobul.HashFollower)
                //{
                //    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraping Users with HashTag " + hashKeyword + "]");
                //    lstHashTagUserIdTemp = GetUser(hashKeyword);
                //    lstHashTagUserId.AddRange(lstHashTagUserIdTemp);
                //}

                int NumberAcoount = IGGlobals.listAccounts.Count;

                Dictionary<string, string> dicAccHash = new Dictionary<string, string>();
                Queue<string> queAcc = new Queue<string>();
                foreach (string itemAcc in IGGlobals.listAccounts)
                {
                    queAcc.Enqueue(itemAcc);
                }

                int temp = No_Follow_User / NumberAcoount;
                int _checkTemp = temp;
                int count = 0;
                string _checkAcc = string.Empty;
                foreach (string itemHash in ClGlobul.followingList)
                {
                    if (No_Follow_User > count)
                    {
                        count++;
                        if (temp == _checkTemp)
                        {
                            if (queAcc.Count > 0)
                            {
                                _checkAcc = queAcc.Dequeue();
                            }
                            else
                            {
                                foreach (string itemAcc in IGGlobals.listAccounts)
                                {
                                    queAcc.Enqueue(itemAcc);
                                }
                                _checkAcc = queAcc.Dequeue();
                            }
                            _checkTemp = 0;
                        }
                        if (!dicAccHash.ContainsKey(_checkAcc))
                        {
                            dicAccHash.Add(_checkAcc, itemHash + ",");
                        }
                        else
                        {
                            dicAccHash[_checkAcc] += itemHash + ",";
                        }
                        _checkTemp++;
                    }
                }
                foreach (var itemDic in dicAccHash)
                {
                    string[] arrHash = Regex.Split(itemDic.Value, ":");
                    Thread thrStart = new Thread(() => FollowOperationequal(itemDic.Key, arrHash));
                    thrStart.Start();
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }

       }

        public void FollowOperationequal(string Acc, string[] Hash)
        {
            string status = string.Empty;
            string[] arrAcc = Regex.Split(Acc, ":");
            InstagramUser objInstagramUser = new InstagramUser(arrAcc[0], arrAcc[1], arrAcc[2], arrAcc[3]);
            GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();
            objInstagramUser.globusHttpHelper = objGlobusHttpHelper;
            AccountManager objAccountManager = new AccountManager();
            if (!objInstagramUser.isloggedin)
            {
                status = objAccountManager.LoginUsingGlobusHttp(ref objInstagramUser);
            }
            if (status == "Success" || (objInstagramUser.isloggedin))
            {

                foreach (string itemHash in Hash)
                {
                    if (!string.IsNullOrEmpty(itemHash))
                    {
                        //Operation
                        string[] Data_ID = Regex.Split(itemHash, ",");
                        string daaa = objInstagramUser.username;
                        foreach (string Photo_ID in Data_ID)
                        {
                            if (!string.IsNullOrEmpty(Photo_ID))
                            {
                                FollowUrls(ref objInstagramUser, Photo_ID);
                            }
                            else
                            {
                                break;
                            }

                            if (minDelayFollowerPoster != 0)
                            {
                                mindelay = minDelayFollowerPoster;
                            }
                            if (maxDelayFollowerPoster != 0)
                            {
                                maxdelay = maxDelayFollowerPoster;
                            }

                            Random obj_rn = new Random();
                            int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                            delay = obj_rn.Next(mindelay, maxdelay);
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);
                        }

                    }
                    GlobusLogHelper.log.Info("=========================");
                    GlobusLogHelper.log.Info("Process Completed !!!");
                    GlobusLogHelper.log.Info("=========================");
                }
            }
        }





        #endregion





    }

}
