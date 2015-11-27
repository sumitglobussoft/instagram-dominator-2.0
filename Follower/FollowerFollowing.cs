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

namespace Follower
{
    public class FollowerFollowing
    {

        #region Global Variables For Direct Message Poster
        readonly object lockrThreadControlleFollowerPoster = new object();
        public bool isStopFollowerPoster = false;
        public bool IsFollow = false;
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
                string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
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
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please Enter Maximum No. of count of Follwers To Be Folllowed ]");
                            return;
                        }



                        if (No_Follow_User != 0)
                        {
                            maximumNoOfCount = No_Follow_User;
                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please Enter Maximum No. of count of Follwers To Be Folllowed ]");
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
                            #region already Following 
                            string FollowerName = followingList_item;
                            string HomeAcc_Url = "http://websta.me/n/" + obj_folow.username;
                            string res_data = obj.getHtmlfromUrl(new Uri(HomeAcc_Url), "");
                            string Data = Utils.getBetween(res_data, "ul class=\"list-inline user", "</ul>");
                            string Data1 = Utils.getBetween(Data, "<a href=\"/follows/", "\"><span class=");
                            string following_url = "http://websta.me/follows/" + Data1;
                            string res_data1 = obj.getHtmlfromUrl(new Uri(following_url), "");
                            string[] split_data = Regex.Split(res_data1, "div class=\"pull-left");
                            foreach (string item in split_data)
                            {
                                string user_following = Utils.getBetween(item, "a href=\"/n/", "\"");
                                AllreadyfollowingList.Add(user_following);
                            }
                            if (res_data1.Contains("Next Page"))
                            {
                                value = true;
                                while (value)
                                {
                                    if (res_data1.Contains("Next Page"))
                                    {
                                        string nextpage_Url = Utils.getBetween(res_data1, "<ul class=\"pager\">", "</ul>");
                                        string[] page_split = Regex.Split(nextpage_Url, "<a href=");
                                        string next = Utils.getBetween(page_split[2], "\"", "\"> Next Page");
                                        string finalNext_FollowingURL = "http://websta.me" + next;
                                        Page_Url.Add(finalNext_FollowingURL);
                                        res_data1 = obj.getHtmlfromUrl(new Uri(finalNext_FollowingURL), "");
                                        string[] split_data1 = Regex.Split(res_data1, "div class=\"pull-left");
                                        foreach (string item in split_data1)
                                        {
                                            string user_following = Utils.getBetween(item, "a href=\"/n/", "\"");
                                            AllreadyfollowingList.Add(user_following);
                                        }
                                    }
                                    else
                                    {
                                        value = false;
                                    }
                                }
                            }
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
            string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");

            if (!UserName.Contains("http://websta.me/n/"))
            {
                UserName = "http://websta.me/n/" + UserName + "/";
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
            try
            {
                //if (UserPageContent.Contains("This user is private."))
                //{
                //    return "private";
                //}
                string PK = string.Empty;
                if (UserPageContent.Contains(""))
                {
                    PK = Utils.getBetween(UserPageContent, "id=\"follow_btn_wrapper\"", ">").Replace("data-target=", "").Replace("\"", "").Trim();
                }

                if (string.IsNullOrEmpty(PK))
                {
                    PK = Utils.getBetween(UserPageContent, "id=\"message_user_id", ">").Replace(">", "").Replace("value=", string.Empty).Replace("\"", string.Empty).Trim();//.Replace("\"", "").Trim();
                }

                string PostData = "action=follow";//"&pk=" + PK + "&t=9208";
                string FollowedPageSource = string.Empty;

                if (!string.IsNullOrEmpty(PK))
                {
                    try
                    {
                        FollowedPageSource = accountManager.globusHttpHelper.postFormData(new Uri("http://websta.me/api/relationships/" + PK), PostData, UserName, "http://websta.me");
                    }
                    catch { }
                }
                if (string.IsNullOrEmpty(FollowedPageSource))
                {

                }
                //try
                //{
                //    nameval.Add("Origin", "http://web.stagram.com");
                //    nameval.Add("X-Requested-With", "XMLHttpRequest");
                //}
                //catch { };

                if (FollowedPageSource.Contains("OK"))
                {
                    //return "Followed";
                    string status = string.Empty;
                    try
                    {
                        status = QueryExecuter.getFollowStatus1(accountManager.username, UserName);
                    }
                    catch { }
                    if (string.IsNullOrEmpty(status))
                    {
                        if (FollowedPageSource.Contains("requested"))
                        {
                            status = "requested";
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
                string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
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


        public void unfollowAccount(ref InstagramUser accountManager, string account , int count)
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
            string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");

            string pageSource = string.Empty;
            string response = string.Empty;
            string profileId = string.Empty;
            const string websta = "http://websta.me/api/relationships/";
            const string accountUrl = "http://websta.me/n/";
            try
            {
                try
                {
                    if (account.Contains("http://websta.me/n"))
                    {
                        pageSource = accountManager.globusHttpHelper.getHtmlfromUrlProxy(new Uri(account), "", 80, "", "");

                    }
                    else
                    {
                        pageSource = accountManager.globusHttpHelper.getHtmlfromUrlProxy(new Uri(accountUrl + account), "", 80, "", "");
                    }
                }
                catch { };
                if (!string.IsNullOrEmpty(pageSource))
                {
                    if (pageSource.Contains("<ul class=\"list-inline user-"))
                    {
                        try
                        {
                            profileId = Utils.getBetween(pageSource, "<ul class=\"list-inline user-", "\">");
                        }
                        catch { }
                        if (!string.IsNullOrEmpty(profileId) && NumberHelper.ValidateNumber(profileId))
                        {
                            try
                            {
                                response = accountManager.globusHttpHelper.postFormData(new Uri(websta + profileId), "action=unfollow", accountUrl + account, "");
                            }
                            catch { }


                            if (!string.IsNullOrEmpty(response) && response.Contains("OK"))
                            {
                                if (_boolStopUnfollow) return;
                                string status = string.Empty;
                                try
                                {
                                    status = QueryExecuter.getFollowStatus(accountManager.username, account);



                                    switch (status)
                                    {
                                        //case "Followed": QueryExecuter.updateFollowStatus(accountManager.Username, account, "Unfollowed");
                                        case "Followed": QueryExecuter.updateFollowStatus(accountManager.username, account, "Unfollowed");
                                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Unfollowed " + account + " from " + accountManager.username + " Count" +count+"]");
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
                                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + accountManager.username + " Unfollowed " + account +" Count" +count+"]"); //account
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
                    }
                }
                else
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ The remote server returned an error: (404) Not Found. " + account + " from " + accountManager.username +  " Count" +count+"]");
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
            }
        }
    }

}
