using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLib;
using System.Threading;
using Accounts;
using Globussoft;
using BaseLibID;
using System.IO;
using System.Text.RegularExpressions;


namespace CampaignDetailsManager
{
    public class CampaignDetails
    {

        #region Global Value

        #region Globals variable for Follow campaign module

      public static  int counter_follow = 0;
      public static bool Skipprivate = false;
        public static string followCampaignName = string.Empty;
        public static string followCampaignAccount = string.Empty;
        public static string followCampaignFollowUserPath = string.Empty;
        public static int followCampaignNoOfFollowPerAccount = 0;
        public static int followCampaignScheduledDaily = 0;
        public static string followCampaignStartTime = string.Empty;
        public static string followCampaignEndTime = string.Empty;
        public static int followCampaignDelayMax = 25;
        public static int followCampaignDelayMin = 10;
        public static int followCampaignNoOfThread = 25;
        public static string campaignFollower_Module = string.Empty;
        public static bool value = false;
        public static int mindelay = 0;
        public static int maxdelay = 0;
        static readonly Object _lockObject = new Object();
       public static List<string> followingList = new List<string>();
       public static List<string> AllreadyfollowingList = new List<string>();
        public static List<string> AlreadyFollowedlist = new List<string>();
         public static List<string> NotFollowedlist = new List<string>();
         public static string Text_CampaignFollowPath = string.Empty;
       

        #endregion

        #region Globals variable for Photo Like campaign module

        public static string PhotoLikeCampaignName = string.Empty;
        public static string PhotoLikeCampaignAccount = string.Empty;
        public static string PhotoLikeCampaignPhotoLikeUserPath = string.Empty;
        public static int PhotoLikeCampaignNoOfPhotLikePerAccount = 10;
        public static int PhotoLikeCampaignScheduledDaily = 0;
        public static string PhotoLikeCampaignStartTime = string.Empty;
        public static string PhotoLikeCampaignEndTime = string.Empty;
        public static int PhotoLikeCampaignDelayMax = 25;
        public static int PhotoLikeCampaignDelayMin = 10;
        public static int PhotoLikeCampaignNoOfThread = 25;

        #endregion


        #region Globals Variable for photo comment Module

        public static string PhotoCommentCampaignName = string.Empty;
        public static string PhotoCommentCampaignAccount = string.Empty;
        public static string PhotoCommentCampaignUserPath = string.Empty;
        public static string PhotoCommentCampaignMessagePath = string.Empty;
        public static int PhotoCommentNoOfPerAccount = 10;
        public static int PhotoCommentScheduledDaily = 0;
        public static string PhotoCommentCampaignStartTime = string.Empty;
        public static string PhotoCommentCampaignEndTime = string.Empty;
        public static int PhotoCommentCampaignDelayMax = 25;
        public static int PhotoCommentCampaignDelayMin = 10;
        public static int PhotoCommentCampaignNoOfThread = 25;
        public List<Thread> lstThreadsCampaingCommentPoster = new List<Thread>();
        public bool isStopCommentPoster = false;
        public static string Comment_message = string.Empty;
        public static string Comment_PhotoId = string.Empty;
        public static int No_Photo_Commented = 0;
        public static string Comment_PhotoId_path = string.Empty;
        public static string Comment_message_path = string.Empty;




        #endregion



        #endregion




        public class CampaignFollow
        {

            #region Global variable

            int countThreadControllerFollower = 0;
            readonly object lockrThreadControlleFollower = new object();
            public bool isStopFollower = false;
            public List<Thread> lstThreadsFollower = new List<Thread>();
            public static string status = string.Empty;
            public static string campaignFollower_Module = string.Empty;
            public static string followPath = string.Empty;

            #endregion


            public int NoOfThreadsFollower
            {
                get;
                set;
            }


            public void startCampaignFollow(string campaignName)
            {
                try
                {
                    lstThreadsFollower.Add(Thread.CurrentThread);
                    lstThreadsFollower.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch(Exception ex)
                {
                    GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                }
                try
                {
                    string query = "select * from tbl_Campaign_Follow where CampaignName='" + campaignName + "' ";
                    DataSet ds = DataBaseHandler.SelectQuery(query, "tbl_Campaign_Follow");
                    string followCampaignName = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    string followCampaignAccount = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    string followCampaignFollowUserPath = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                    string followCampaignNoOfFollowPerAccount = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                    string followCampaignScheduledDaily = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                    string followCampaignStartTime = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                    string followCampaignEndTime = ds.Tables[0].Rows[0].ItemArray[7].ToString();
                    string followCampaignDelayMax = ds.Tables[0].Rows[0].ItemArray[8].ToString();
                    string followCampaignDelayMin = ds.Tables[0].Rows[0].ItemArray[9].ToString();
                    string followCampaignNoOfThread = ds.Tables[0].Rows[0].ItemArray[10].ToString();
                     campaignFollower_Module = ds.Tables[0].Rows[0].ItemArray[11].ToString();
                     followPath = followCampaignFollowUserPath;
                    

                    DateTime StartTime = DateTime.Parse(followCampaignStartTime);
                    DateTime EndTime = DateTime.Parse(followCampaignEndTime);
                    if (string.IsNullOrEmpty(followCampaignStartTime) && string.IsNullOrEmpty(followCampaignEndTime))
                    {
                        Thread CommentPosterThread = new Thread(() => StartFollower(followCampaignName));
                        CommentPosterThread.Start();
                    }
                    else
                    {
                        while (true)
                        {
                            DateTime Currenttime = DateTime.Now;

                            if ((StartTime.TimeOfDay <= Currenttime.TimeOfDay) && (Currenttime.TimeOfDay <= EndTime.TimeOfDay))
                            {
                                GlobusLogHelper.log.Info("!!! Follow Process Start !!!!");
                                Thread CommentPosterThread = new Thread(() => StartFollower(followCampaignName));
                                CommentPosterThread.Name = followCampaignName + DateTime.Now;
                                CommentPosterThread.Start();
                               // break;
                                while (true)
                                {
                                    if (Currenttime.TimeOfDay >= EndTime.TimeOfDay)
                                    {
                                        break;

                                    }
                                    Thread.Sleep(10 * 1000);
                                }

                            }
                            

                                if ((Currenttime.TimeOfDay >= EndTime.TimeOfDay))
                                {
                                    if (followCampaignScheduledDaily == "1")
                                    {
                                        Thread.Sleep(10 * 1000);
                                        continue;

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else if ((StartTime.TimeOfDay >= Currenttime.TimeOfDay))
                                {
                                    Thread.Sleep(10 * 1000);
                                    continue;
                                }




                            }
                        }
                    }


               
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error ==>> " + ex.StackTrace);
                }

            }

            public void StartFollower(string CampaignName)
            {


                try
                {
                    lstThreadsFollower.Add(Thread.CurrentThread);
                    lstThreadsFollower.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }

                countThreadControllerFollower = 0;
                try
                {
                    int numberOfAccountPatch = 25;

                    if (NoOfThreadsFollower > 0)
                    {
                        numberOfAccountPatch = NoOfThreadsFollower;
                    }

                    List<List<string>> list_listAccounts = new List<List<string>>();
                    if (IGGlobals.listAccounts.Count >= 1)
                    {

                        list_listAccounts = Utils.Split(IGGlobals.listAccounts, numberOfAccountPatch);

                        string query = "select * from tbl_Campaign_Follow where CampaignName='" + CampaignName + "' ";
                        DataSet ds = DataBaseHandler.SelectQuery(query, "tbl_Campaign_Follow");
                        string followCampaignAccount = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                        string qury = "select * from AccountInfo where Username='" + followCampaignAccount + "'";
                        DataSet dt = DataBaseHandler.SelectQuery(qury, "AccountInfo");
                        string password = dt.Tables[0].Rows[0].ItemArray[2].ToString();
                        string account = followCampaignAccount + ":" + password;

                        string acc = account.Remove(account.IndexOf(':'));

                        //Run a separate thread for each account
                        InstagramUser item = null;
                        IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                        if (item != null)
                        {
                            Thread profilerThread = new Thread(StartMultiThreadsFollow);
                            // profilerThread.Name = "workerThread_Profiler_" + acc;
                            profilerThread.Name = CampaignName + DateTime.Now.ToString();
                            profilerThread.IsBackground = true;

                            profilerThread.Start(new object[] { item });

                            countThreadControllerFollower++;
                        }

                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }

            public void StartMultiThreadsFollow(object parameters)
            {
                try
                {
                    if (!isStopFollower)
                    {
                        try
                        {
                            lstThreadsFollower.Add(Thread.CurrentThread);
                            lstThreadsFollower.Distinct();
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
                                StartActionFollow(ref objFacebookUser);
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
                            lock (lockrThreadControlleFollower)
                            {
                                countThreadControllerFollower--;
                                Monitor.Pulse(lockrThreadControlleFollower);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }
            }

            private void StartActionFollow(ref InstagramUser fbUser)
            {

                try
                {
                    start_Follow(ref fbUser);
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }

            public void start_Follow(ref InstagramUser obj_campfollow)
            {
                try
                {
                    try
                    {
                        lstThreadsFollower.Add(Thread.CurrentThread);
                        lstThreadsFollower.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                    getFollow(ref obj_campfollow);



                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                }
            }

            public void getFollow(ref InstagramUser obj_folow)
            {
              
                try
                {
                    lstThreadsFollower.Add(Thread.CurrentThread);
                    lstThreadsFollower.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
                try
                {
                    ClGlobul.campaignfollowingList.Clear();
                    List<string> templist = GlobusFileHelper.ReadFile(followPath);

                    foreach (string item in templist)
                    {
                        ClGlobul.campaignfollowingList.Add(item);
                       
                    }
                }
                catch { };



                try
                {
                    GlobusHttpHelper obj = obj_folow.globusHttpHelper;
                    string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                    if (ClGlobul.campaignfollowingList.Count != 0)
                    {
                        //foreach (string followingList_item in ClGlobul.followingList) //commented when divide data implemented.
                        int maximumNoOfCount = 0;
                        try
                        {
                            if (followCampaignNoOfFollowPerAccount != 0)
                            {
                                maximumNoOfCount = followCampaignNoOfFollowPerAccount;

                            }
                            else
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please Enter Maximum No. of count of Follwers To Be Folllowed ]");
                                return;
                            }



                            if (followCampaignNoOfFollowPerAccount != 0)
                            {
                                maximumNoOfCount = followCampaignNoOfFollowPerAccount;
                            }
                            else
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please Enter Maximum No. of count of Follwers To Be Folllowed ]");
                                return;

                            }
                        }
                        catch { };
                        int CountOfFollowersInForeach = 0;
                        counter_follow = IGGlobals.listAccounts.Count();

                        foreach (string followingList_item in ClGlobul.campaignfollowingList)
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
                                          //  Page_Url.Add(finalNext_FollowingURL);
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
                                    Result = Follow(FollowerName, ref obj_folow);
                                }
                                else
                                {
                                    GlobusLogHelper.log.Info("Allready User Following");

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
                                    if (followCampaignDelayMin != 0)
                                    {
                                        mindelay = followCampaignDelayMin;
                                    }
                                    if (followCampaignDelayMax != 0)
                                    {
                                        maxdelay = followCampaignDelayMax;
                                    }
                                    lock (_lockObject)
                                    {
                                        Random FolloweRandom = new Random();
                                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                        delay = FolloweRandom.Next(mindelay, maxdelay);
                                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");

                                    }

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
                                }
                                else if (Result == "requested")
                                {
                                    CountOfFollowersInForeach++;
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ BY " + obj_folow.username + " Request has been sent to . " + FollowerName + " ]");
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
                                        if (followCampaignDelayMin != 0)
                                        {
                                            mindelay = followCampaignDelayMin;
                                        }
                                        if (followCampaignDelayMax != 0)
                                        {
                                            maxdelay = followCampaignDelayMax;
                                        }
                                        lock (_lockObject)
                                        {
                                            Random FolloweRandom = new Random();
                                            int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                            delay = FolloweRandom.Next(mindelay, maxdelay);
                                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                                            Thread.Sleep(delay * 1000);
                                        }
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
                                    if (followCampaignDelayMin != 0)
                                    {
                                        mindelay = followCampaignDelayMin;
                                    }
                                    if (followCampaignDelayMax != 0)
                                    {
                                        maxdelay = followCampaignDelayMax;
                                    }
                                    lock (_lockObject)
                                    {
                                        Random FolloweRandom = new Random();
                                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                        delay = FolloweRandom.Next(mindelay, maxdelay);
                                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                                        Thread.Sleep(delay * 1000);
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
                                else if (Result == "Follow option is not available In page...!!")
                                {

                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Follow option is not available In page...!!" + obj_folow.username + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(obj_folow.username + ":" + obj_folow.password, GlobusFileHelper.FollowedOptionNotAvailableFilePath);
                                }
                                else
                                {

                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + obj_folow.username + " All Ready Followed " + FollowerName + " ]");


                                    if (!NotFollowedlist.Contains(obj_folow.username))
                                    {
                                        NotFollowedlist.Add(obj_folow.username);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(obj_folow.username + ":" + obj_folow.password, GlobusFileHelper.UnFollowIdFilePath);
                                    }
                                }

                                lock (_lockObject)
                                {
                                    //int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);

                                    //AddToLogger("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");

                                    //_boolAddToLogger = true;
                                    //Thread.Sleep(delay * 1000);
                                    //_boolAddToLogger = false;
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

                    followCampaignNoOfFollowPerAccount--;
                    if (followCampaignNoOfFollowPerAccount == 0)
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
                    lstThreadsFollower.Add(Thread.CurrentThread);
                    lstThreadsFollower.Distinct();
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
                try
                {
                    if (Skipprivate == true)
                    {
                        if (UserPageContent.Contains("\"is_private\":true"))
                        {
                            return "User is Private";
                        }
                    }
                    //if (UserPageContent.Contains("This user is private."))
                    //{
                    //    return "private";
                    //}
                    string PK = string.Empty;
                    if (UserPageContent.Contains(""))
                    {
                        // PK = Utils.getBetween(UserPageContent, "id=\"follow_btn_wrapper\"", ">").Replace("data-target=", "").Replace("\"", "").Trim();
                        PK = Utils.getBetween(UserPageContent, "\"id\":", "\",").Replace("\"", "");
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
                            string txt_name = Utils.getBetween(UserName, "www.instagram.com/", "/");
                            string csrf_token = Utils.getBetween(UserPageContent, "csrf_token\":\"", "\"}");
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
                            if (FollowedPageSource.Contains("followed_by_viewer\":true"))
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
        }

        public class CampaignPhotoLike
            {

                #region Global variable

                int countThreadControllerPhotoLike = 0;
                readonly object lockrThreadControllePhotoLike = new object();
                public bool isStopPhotoLike = false;
                public List<Thread> lstThreadsPhotoLike = new List<Thread>();
                public static string status = string.Empty;
                public static string txt_PhotoIdCampaign = string.Empty;
                public static int noPhotoLike_username = 0;
                public static string PhotoIdPath = string.Empty;

                #endregion

                public int NoOfThreadsPhotoLike
                {
                    get;
                    set;
                }

                public void startCampaignPhotoLike(string campaignName)
                {
                    try
                    {
                        lstThreadsPhotoLike.Add(Thread.CurrentThread);
                        lstThreadsPhotoLike.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch(Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }


                    try
                    {
                        string query = "select * from tbl_Campaign_Photoliker where CampaignName='" + campaignName + "' ";
                        DataSet ds = DataBaseHandler.SelectQuery(query, "tbl_Campaign_Photoliker");
                        string PhotoLikeCampaignName = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        string PhotoLikeCampaignAccount = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                        string PhotoLikeCampaignPhotoLikeUserPath = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                        string PhotoLikeCampaignNoOfPhotLikePerAccount = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                        string PhotoLikeCampaignScheduledDaily = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                        string PhotoLikeCampaignStartTime = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                        string PhotoLikeCampaignEndTime = ds.Tables[0].Rows[0].ItemArray[7].ToString();
                        string PhotoLikeCampaignDelayMax = ds.Tables[0].Rows[0].ItemArray[8].ToString();
                        string followCampaignDelayMin = ds.Tables[0].Rows[0].ItemArray[9].ToString();
                        string PhotoLikeCampaignNoOfThread = ds.Tables[0].Rows[0].ItemArray[10].ToString();
                       // txt_PhotoIdCampaign = ds.Tables[0].Rows[0].ItemArray[11].ToString();
                        PhotoIdPath = PhotoLikeCampaignPhotoLikeUserPath;

                        DateTime StartTimePhoto = DateTime.Parse(PhotoLikeCampaignStartTime);
                        DateTime EndTimePhoto = DateTime.Parse(PhotoLikeCampaignEndTime);
                    if (string.IsNullOrEmpty(PhotoLikeCampaignStartTime) && string.IsNullOrEmpty(PhotoLikeCampaignEndTime))
                    {
                        Thread CommentPosterThread = new Thread(() => StartPhotoLike(followCampaignName));
                        CommentPosterThread.Start();
                    }
                    else
                    {
                        while (true)
                        {
                            DateTime Currenttime = DateTime.Now;

                            if ((StartTimePhoto.TimeOfDay <= Currenttime.TimeOfDay) && (Currenttime.TimeOfDay <= EndTimePhoto.TimeOfDay))
                            {
                                GlobusLogHelper.log.Info("!!! Follow Process Start !!!!");
                                Thread CommentPosterThread = new Thread(() => StartPhotoLike(PhotoLikeCampaignName));
                                CommentPosterThread.Name = PhotoLikeCampaignName + DateTime.Now;
                                CommentPosterThread.Start();
                                break;
                                while (true)
                                {
                                    if (Currenttime.TimeOfDay >= EndTimePhoto.TimeOfDay)
                                    {
                                        break;

                                    }
                                    Thread.Sleep(10 * 1000);
                                }

                            }


                            if ((Currenttime.TimeOfDay >= EndTimePhoto.TimeOfDay))
                                {
                                    if (PhotoLikeCampaignScheduledDaily == "1")
                                    {
                                        Thread.Sleep(10 * 1000);
                                        continue;

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else if ((StartTimePhoto.TimeOfDay >= Currenttime.TimeOfDay))
                                {
                                    Thread.Sleep(10 * 1000);
                                    continue;
                                }




                            }
                        }
                    }
               
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error ==>> " + ex.StackTrace);
                }

                }

                public void StartPhotoLike(string campaign)
                {

                    try
                    {
                        lstThreadsPhotoLike.Add(Thread.CurrentThread);
                        lstThreadsPhotoLike.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }

                    countThreadControllerPhotoLike = 0;
                    try
                    {
                        int numberOfAccountPatch = 25;

                        if (NoOfThreadsPhotoLike > 0)
                        {
                            numberOfAccountPatch = NoOfThreadsPhotoLike;
                        }

                        List<List<string>> list_listAccounts = new List<List<string>>();
                        string query = "select * from tbl_Campaign_Photoliker where CampaignName='" + campaign + "' ";
                        DataSet ds = DataBaseHandler.SelectQuery(query, "tbl_Campaign_Photoliker");
                        string PhotoLikeCampaignAccount = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                        string qury = "select * from AccountInfo where Username='" + PhotoLikeCampaignAccount + "'";
                        DataSet dt = DataBaseHandler.SelectQuery(qury, "AccountInfo");
                        string password = dt.Tables[0].Rows[0].ItemArray[2].ToString();
                        string account = PhotoLikeCampaignAccount + ":" + password;


                                               string acc = account.Remove(account.IndexOf(':'));

                                                //Run a separate thread for each account
                                                InstagramUser item = null;
                                                IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                                                if (item != null)
                                                {
                                                    Thread profilerThread = new Thread(StartMultiThreadsPhotoLike);
                                                  //  profilerThread.Name = "workerThread_Profiler_" + acc;
                                                    profilerThread.Name = campaign + DateTime.Now;
                                                    profilerThread.IsBackground = true;

                                                    profilerThread.Start(new object[] { item });

                                                    countThreadControllerPhotoLike++;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                            }
                                        }
                                    
                                    
                

                public void StartMultiThreadsPhotoLike(object parameters)
                {
                    try
                    {
                        lstThreadsPhotoLike.Add(Thread.CurrentThread);
                        lstThreadsPhotoLike.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }




                    try
                    {
                        if (!isStopPhotoLike)
                        {
                            try
                            {
                                lstThreadsPhotoLike.Add(Thread.CurrentThread);
                                lstThreadsPhotoLike.Distinct();
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
                                    StartActionPhotoLike(ref objFacebookUser);
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
                                lock (lockrThreadControllePhotoLike)
                                {
                                    countThreadControllerPhotoLike--;
                                    Monitor.Pulse(lockrThreadControllePhotoLike);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }
                    }
                }

                private void StartActionPhotoLike(ref InstagramUser fbUser)
                {

                    try
                    {
                        Start_LikePhoto(ref fbUser);
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }

                public void Start_LikePhoto(ref InstagramUser obj_Campphoto)
                {
                    try
                    {
                        lstThreadsPhotoLike.Add(Thread.CurrentThread);
                        lstThreadsPhotoLike.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                    try
                    {
                        ClGlobul.campaignPhotoIDList.Clear();

                        List<string> templist1 = GlobusFileHelper.ReadFile(PhotoIdPath);

                        foreach (string item in templist1)
                        {
                            ClGlobul.campaignPhotoIDList.Add(item);

                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                    }
                    getPhotoLike(ref obj_Campphoto);   

                }

                public void getPhotoLike(ref InstagramUser Photo_likee)
                {
                    try
                    {
                        lstThreadsPhotoLike.Add(Thread.CurrentThread);
                        lstThreadsPhotoLike.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                    try
                    {
                        GlobusHttpHelper obj = Photo_likee.globusHttpHelper;
                        int noPhotoLike = 0;
                        string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                        foreach (string PhotoList_item in ClGlobul.campaignPhotoIDList)
                        {

                            string query = "select * from LikeInfo where UseName='" + Photo_likee.username + "' and LikePhotoId='" + PhotoList_item + "'";
                            DataSet ds = DataBaseHandler.SelectQuery(query, "LikeInfo");
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                try
                                {
                                    string LikeName = PhotoList_item;
                                    string photoId = string.Empty;

                                    if (PhotoList_item.Contains("\0"))
                                    {
                                        photoId = PhotoList_item.Replace("\0", string.Empty).Trim();
                                    }
                                    else
                                    {
                                        photoId = PhotoList_item;
                                    }
                                    string Result = string.Empty;
                                    try
                                    {
                                        if (PhotoLikeCampaignNoOfPhotLikePerAccount > noPhotoLike)
                                        {
                                            Result = photolike(photoId, ref Photo_likee);
                                            noPhotoLike++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                    }


                                    if (!Result.Contains("LIKED") && !Result.Contains("All ready LIKED"))
                                    {
                                        #region commment
                                        try
                                        {
                                            QueryExecuter.insertLikeStatus(photoId, Photo_likee.username, 1);
                                            GlobusLogHelper.log.Info("photoID in Incorrect");


                                            try
                                            {
                                                string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator";
                                                path_AppDataFolder = path_AppDataFolder + "\\LikeList";
                                                if (!File.Exists(path_AppDataFolder))
                                                {
                                                    Directory.CreateDirectory(path_AppDataFolder);
                                                }
                                                string FollowIDFilePath = path_AppDataFolder + "\\" + Photo_likee.username + ".csv";
                                                string CSV_Header = "Username,PhotoID,Liked";
                                                string CSV_Content = Photo_likee.username.Replace(",", "") + "," + photoId.Replace(",", "") + "," + Result;
                                                GlobusFileHelper.ExportDataCSVFile(CSV_Header, CSV_Content, FollowIDFilePath);
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



                                        try
                                        {
                                            if (!ClGlobul.photoLikesCompletedList.Contains(Photo_likee.username))// + ":" + accountManager.Password + ":" + accountManager.proxyAddress + ":" + accountManager.proxyPort + ":" + accountManager.proxyUsername + ":" + accountManager.proxyPassword))
                                            {
                                                ClGlobul.photoLikesCompletedList.Add(Photo_likee.username);// + ":" + accountManager.Password + ":" + accountManager.proxyAddress + ":" + accountManager.proxyPort + ":" + accountManager.proxyUsername + ":" + accountManager.proxyPassword);
                                            }
                                            GlobusFileHelper.AppendStringToTextfileNewLine(Photo_likee.username + ":" + photoId, GlobusFileHelper.LikePhotoAccountIdFilePath);


                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                        }
                                        #endregion
                                        try
                                        {
                                            if (Result.Contains("LIKED"))
                                            {
                                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + Photo_likee.username + "   LIKED : " + PhotoList_item + " ]");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                        }
                                    }
                                    else if (Result.Contains("Already LIKED"))
                                    {
                                        #region if photoallready like
                                        try
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(Photo_likee.username + ":" + photoId, GlobusFileHelper.AllReadylikePhotoAccountIdFilePath);
                                            try
                                            {
                                                string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator";
                                                path_AppDataFolder = path_AppDataFolder + "\\LikeList";
                                                if (!File.Exists(path_AppDataFolder))
                                                {
                                                    Directory.CreateDirectory(path_AppDataFolder);
                                                }
                                                string FollowIDFilePath = path_AppDataFolder + "\\" + Photo_likee.username + ".csv";
                                                string CSV_Header = "Username,PhotoID,Already LIKED";
                                                string CSV_Content = Photo_likee.username.Replace(",", "") + "," + photoId.Replace(",", "") + "," + Result;
                                                GlobusFileHelper.ExportDataCSVFile(CSV_Header, CSV_Content, FollowIDFilePath);
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                            }

                                            //if (minDelayLikePoster != 0)
                                            //{
                                            //    mindelay = minDelayLikePoster;
                                            //}
                                            //if (maxDelayLikePoster != 0)
                                            //{
                                            //    maxdelay = maxDelayLikePoster;
                                            //}


                                            //int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                            //GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds For " + Photo_likee.username + " ]");
                                            //Thread.Sleep(delay * 1000);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                        }
                                        #endregion



                                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + Photo_likee.username + " Already LIKED :  " + PhotoList_item + " ]");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (Result.Contains("LIKED"))
                                            {
                                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + Photo_likee.username + "  LIKED : " + PhotoList_item + " ]");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                        }


                                    }

                                    if (PhotoLikeCampaignDelayMin != 0)
                                    {
                                        mindelay = PhotoLikeCampaignDelayMin;
                                    }
                                    if (PhotoLikeCampaignDelayMax != 0)
                                    {
                                        maxdelay = PhotoLikeCampaignDelayMax;
                                    }


                                    int delayy = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delayy + " Seconds For " + Photo_likee.username + " ]");
                                    Thread.Sleep(delayy * 1000);

                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                }


                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                    finally
                    {
                        GlobusLogHelper.log.Info("--------------------------------------------------------------------------------------------");
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Photo Like is Finished From =>" + Photo_likee.username + " ]");
                        GlobusLogHelper.log.Info("--------------------------------------------------------------------------------------------");
                    }
                }
                public string photolike(string PhotoId, ref InstagramUser Photo_likebyID)
                {
                    try
                    {
                        lstThreadsPhotoLike.Add(Thread.CurrentThread);
                        lstThreadsPhotoLike.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                    List<string> phto_list = new List<string>();
                    string Photolink = string.Empty;
                    string FollowedPageSource = string.Empty;
                    string like = string.Empty;
                    string new_repce = string.Empty;
                    bool value = true;
                    if (IGGlobals.Check_likephoto_Byusername == true)
                    {
                        try
                        {

                            int temp = noPhotoLike_username;
                            int flag = 0;
                            string Url_user = "https://www.instagram.com/" + PhotoId;
                            string responce_user = Photo_likebyID.globusHttpHelper.getHtmlfromUrl(new Uri(Url_user), "");
                            string token = Utils.getBetween(responce_user, "csrf_token\":\"", "\"}");
                            string[] photo_code = Regex.Split(responce_user, "code\":");
                            foreach (string list in photo_code)
                            {
                                if (list.Contains("date"))
                                {
                                    string photo_codes = Utils.getBetween("@" + list, "@\"", "\"");
                                    phto_list.Add(photo_codes);
                                }
                            }
                            if (responce_user.Contains("has_next_page\":true"))
                            {
                                while (value)
                                {
                                    if (responce_user.Contains("has_next_page\":true") && phto_list.Count < temp)
                                    {
                                        string IDD = Utils.getBetween(responce_user, "\"id\":\"", "\"");
                                        string code_ID = Utils.getBetween(responce_user, "end_cursor\":\"", "\"");
                                        string postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                        responce_user = Photo_likebyID.globusHttpHelper.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, "https://www.instagram.com/" + PhotoId, token);
                                        string[] data1 = Regex.Split(responce_user, "code");
                                        foreach (string val in data1)
                                        {
                                            if (val.Contains("date"))
                                            {
                                                if (phto_list.Count < temp)
                                                {
                                                    string photo_codes = Utils.getBetween(val, "\":\"", "\"");
                                                    phto_list.Add(photo_codes);
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }



                                    }
                                    else
                                    {
                                        value = false;
                                    }

                                }
                            }




                            foreach (string item in phto_list)
                            {


                                string resp_photodetail = Photo_likebyID.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/p/" + item), "");
                                string phto_ID = Utils.getBetween(resp_photodetail, "content=\"instagram://media?id=", " />").Replace("\"", "");
                                //string[] data = Regex.Split(responce_user, "class=\"mainimg_wrapper\"");
                                //GlobusHttpHelper obj = Photo_likebyID.globusHttpHelper;
                                //foreach (string item in data)
                                //{
                                if (flag < temp)
                                {

                                    if (!item.Contains("!DOCTYPE html>"))
                                    {
                                        //PhotoId = Utils.getBetween(item, " href=\"/p/", "\"");
                                        flag++;
                                        try
                                        {
                                            if (phto_ID.Contains("https://www.instagram.com/p/"))
                                            {
                                                phto_ID = PhotoId.Replace(IGGlobals.Instance.IGhomeurl, string.Empty);
                                            }
                                            if (!phto_ID.Contains("https://www.instagram.com/p/"))
                                            {
                                                Photolink = "https://www.instagram.com/web/likes/" + phto_ID + "/like/ ".Replace(IGGlobals.Instance.IGhomeurl, "");
                                            }
                                            else
                                            {
                                                Photolink = item;

                                            }
                                            string url = "https://www.instagram.com/p/" + item;
                                            string Check_like = Photo_likebyID.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");
                                            string token_paginaton = Utils.getBetween(Check_like, "csrf_token\":\"", "\"}");
                                            //    string Data = Utils.getBetween(Check_like, "class=\"list-inline pull-left\">", "</ul>");
                                            if (Check_like.Contains("\"viewer_has_liked\":false"))
                                            {
                                                //string PageContent = accountManager.httpHelper.getHtmlfromUrl(new Uri(Photolink), "", "", accountManager.proxyAddress);
                                                string PageContent = string.Empty;
                                                if (string.IsNullOrEmpty(Photo_likebyID.proxyport))
                                                {
                                                    Photo_likebyID.proxyport = "80";
                                                }
                                                try
                                                {
                                                    if (ClGlobul.checkHashTagLiker == true)
                                                    {
                                                        PageContent = Photo_likebyID.globusHttpHelper.getHtmlfromUrl(new Uri(Photolink), "");
                                                    }
                                                    else
                                                    {
                                                        PageContent = Photo_likebyID.globusHttpHelper.PostData_LoginThroughInstagram(new Uri(Photolink), "", "https://www.instagram.com/", token_paginaton);
                                                        //PageContent = Photo_likebyID.globusHttpHelper.getHtmlfromUrlProxy(new Uri(Photolink), Photo_likebyID.proxyip, Convert.ToInt32(Photo_likebyID.proxyport), Photo_likebyID.proxyusername, Photo_likebyID.proxypassword);
                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                                }
                                                if (string.IsNullOrEmpty(PageContent))
                                                {
                                                    if (ClGlobul.checkHashTagLiker == true)
                                                    {
                                                        PageContent = Photo_likebyID.globusHttpHelper.getHtmlfromUrl(new Uri(Photolink));
                                                    }
                                                    else
                                                    {
                                                        PageContent = Photo_likebyID.globusHttpHelper.getHtmlfromUrlProxy(new Uri(Photolink), "", 80, "", "");
                                                    }
                                                }

                                                if (PageContent.Contains("{\"status\":\"ok\"}"))
                                                {

                                                    DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User, Photo_Id,Status) values('" + "PhotoLikeModule" + "','" + Photo_likebyID.username + "','" + item + "','" + "LIKED" + "')", "tbl_AccountReport");
                                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + Photo_likebyID.username + "  LIKED : " + PhotoId + "==>PhotoID===>" + item + "]");
                                                    // FollowedPageSource = "LIKED";

                                                    try
                                                    {
                                                        if (ClGlobul.checkHashTagLiker == true)
                                                        {
                                                            try
                                                            {
                                                                DataBaseHandler.InsertQuery("insert into liker_hash_tag (account_holder, photo_id, like_date, like_status) values ('" + Photo_likebyID.username + "','" + PhotoId + "','" + Convert.ToString(DateTime.Now) + "','" + FollowedPageSource + "')", "liker_hash_tag");
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
                                            else if (Check_like.Contains("viewer_has_liked\":true"))
                                            {
                                                FollowedPageSource = "Already LIKED";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                        }

                                    }
                                }


                                if (PhotoLikeCampaignDelayMin != 0)
                                {
                                    mindelay = PhotoLikeCampaignDelayMin;
                                }
                                if (PhotoLikeCampaignDelayMax != 0)
                                {
                                    maxdelay = PhotoLikeCampaignDelayMax;
                                }


                                int delayy = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delayy + " Seconds For " + Photo_likebyID.username + " ]");
                                Thread.Sleep(delayy * 1000);
                            }

                        }

                     //   }

                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }
                        return FollowedPageSource;
                    }
                    else
                    {
                        try
                        {
                            GlobusHttpHelper obj = Photo_likebyID.globusHttpHelper;
                            string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                            string get_photoId = obj.getHtmlfromUrl(new Uri("https://www.instagram.com/p/" + PhotoId), "");
                            string photo_ID = Utils.getBetween(get_photoId, "content=\"instagram://media?id=", " />").Replace("\"", "");


                            if (photo_ID.Contains("https://www.instagram.com/p/"))
                            {
                                photo_ID = PhotoId.Replace(IGGlobals.Instance.IGhomeurl, string.Empty);
                            }
                            if (!photo_ID.Contains("https://www.instagram.com/p/"))
                            {
                                Photolink = "https://www.instagram.com/web/likes/" + photo_ID + "/like/ ".Replace(IGGlobals.Instance.IGhomeurl, "");
                            }
                            else
                            {
                                Photolink = photo_ID;

                            }
                            string url = "https://www.instagram.com/p/" + PhotoId;
                            string Check_like = obj.getHtmlfromUrl(new Uri(url), "");
                            string token = Utils.getBetween(Check_like, "csrf_token\":\"", "\"}");
                            //   string Data = Utils.getBetween(Check_like, "class=\"list-inline pull-left\">", "</ul>");
                            if (Check_like.Contains("viewer_has_liked\":false"))
                            {

                                string PageContent = string.Empty;
                                if (string.IsNullOrEmpty(Photo_likebyID.proxyport))
                                {
                                    Photo_likebyID.proxyport = "80";
                                }
                                try
                                {
                                    if (ClGlobul.checkHashTagLiker == true)
                                    {
                                        PageContent = Photo_likebyID.globusHttpHelper.getHtmlfromUrl(new Uri(Photolink), "");
                                    }
                                    else
                                    {

                                        PageContent = Photo_likebyID.globusHttpHelper.PostData_LoginThroughInstagram(new Uri(Photolink), "", "https://www.instagram.com/", token);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                }
                                if (string.IsNullOrEmpty(PageContent))
                                {
                                    if (ClGlobul.checkHashTagLiker == true)
                                    {
                                        PageContent = Photo_likebyID.globusHttpHelper.getHtmlfromUrl(new Uri(Photolink));
                                    }
                                    else
                                    {
                                        PageContent = Photo_likebyID.globusHttpHelper.getHtmlfromUrlProxy(new Uri(Photolink), "", 80, "", "");
                                    }
                                }

                                if (PageContent.Contains("Instagram API does not respond"))
                                {
                                    FollowedPageSource = "Instagram API does not respond";
                                }

                                if (PageContent.Contains("{\"status\":\"ok\"}"))
                                {


                                    FollowedPageSource = "LIKED";
                                    DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User, Photo_Id,Status) values('" + "PhotoLikeModule" + "','" + Photo_likebyID.username + "','" + PhotoId + "','" + "LIKED" + "')", "tbl_AccountReport");

                                    try
                                    {
                                        if (ClGlobul.checkHashTagLiker == true)
                                        {
                                            try
                                            {
                                                DataBaseHandler.InsertQuery("insert into liker_hash_tag (account_holder, photo_id, like_date, like_status) values ('" + Photo_likebyID.username + "','" + PhotoId + "','" + Convert.ToString(DateTime.Now) + "','" + FollowedPageSource + "')", "liker_hash_tag");
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
                            else if (Check_like.Contains("viewer_has_liked\":true"))
                            {
                                FollowedPageSource = "Already LIKED";
                            }


                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }


                        return FollowedPageSource;
                    }
                }


            }

        public class CampaignComment
        {

            #region Global Variable

            int countThreadControllerPhotoComment = 0;
            readonly object lockrThreadControllePhotoComment = new object();
            public bool isStopPhotoComment = false;
            public List<Thread> lstThreadsPhotoComment = new List<Thread>();
            public static string status = string.Empty;
            public static string commentpath = string.Empty;
            public static string commentPhoto_Id = string.Empty;
            string Account_Loaded = string.Empty;



            #endregion

            public int NoOfThreadsPhotoComment
            {
                get;
                set;
            }

            public void startCampaignComment(string campaign)
            {
                try
                {
                    lstThreadsPhotoComment.Add(Thread.CurrentThread);
                    lstThreadsPhotoComment.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch(Exception ex)
                {
                    GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                }


                try
                {
                    string query = "select * from tbl_Campaign_Comment where CampaignName='" + campaign + "' ";
                    DataSet ds = DataBaseHandler.SelectQuery(query, "tbl_Campaign_Comment");
                    string PhotoCommentCampaignName = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    string PhotoCommentCampaignAccount = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    string PhotoCommentCampaignPhotoCommentUserPath = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                    string PhotoCommentCampaignNoOfPhotLikePerAccount = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                    string PhotoCommentCampaignScheduledDaily = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                    string PhotoCommentCampaignStartTime = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                    string PhotoCommentCampaignEndTime = ds.Tables[0].Rows[0].ItemArray[7].ToString();
                    string PhotoCommentCampaignDelayMax = ds.Tables[0].Rows[0].ItemArray[8].ToString();
                    string PhotoCommentCampaignDelayMin = ds.Tables[0].Rows[0].ItemArray[9].ToString();
                    string PhotoCommentCampaignNoOfThread = ds.Tables[0].Rows[0].ItemArray[10].ToString();
                    // txt_PhotoIdCampaign = ds.Tables[0].Rows[0].ItemArray[11].ToString();
                    string PhotoCommentCampaignMessagePath = ds.Tables[0].Rows[0].ItemArray[11].ToString();
                    Account_Loaded = PhotoCommentCampaignAccount;
                    commentPhoto_Id = PhotoCommentCampaignPhotoCommentUserPath;
                    commentpath = PhotoCommentCampaignMessagePath;

                    DateTime StartTimePhoto = DateTime.Parse(PhotoCommentCampaignStartTime);
                    DateTime EndTimePhoto = DateTime.Parse(PhotoCommentCampaignEndTime);
                    if (string.IsNullOrEmpty(PhotoCommentCampaignStartTime) && string.IsNullOrEmpty(PhotoCommentCampaignEndTime))
                    {
                        Thread CommentPosterThread = new Thread(() => StartCommentPhoto(PhotoCommentCampaignName));
                        CommentPosterThread.Start();
                    }
                    else
                    {
                        while (true)
                        {
                            DateTime Currenttime = DateTime.Now;

                            if ((StartTimePhoto.TimeOfDay <= Currenttime.TimeOfDay) && (Currenttime.TimeOfDay <= EndTimePhoto.TimeOfDay))
                            {
                                GlobusLogHelper.log.Info("!!! Follow Process Start !!!!");
                                Thread CommentPosterThread = new Thread(() => StartCommentPhoto(PhotoCommentCampaignName));
                                CommentPosterThread.Name = PhotoCommentCampaignName + DateTime.Now;
                                CommentPosterThread.Start();
                                break;
                                while (true)
                                {
                                    if (Currenttime.TimeOfDay >= EndTimePhoto.TimeOfDay)
                                    {
                                        break;

                                    }
                                    Thread.Sleep(10 * 1000);
                                }

                            }


                            if ((Currenttime.TimeOfDay >= EndTimePhoto.TimeOfDay))
                            {
                                if (PhotoCommentCampaignScheduledDaily == "1")
                                {
                                    Thread.Sleep(10 * 1000);
                                    continue;

                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if ((StartTimePhoto.TimeOfDay >= Currenttime.TimeOfDay))
                            {
                                Thread.Sleep(10 * 1000);
                                continue;
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error ==>> " + ex.StackTrace);
                }




            }

            public void StartCommentPhoto(string Campaign)
            {

                try
                {
                    lstThreadsPhotoComment.Add(Thread.CurrentThread);
                    lstThreadsPhotoComment.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                }
                countThreadControllerPhotoComment = 0;
                try
                {
                    int numberOfAccountPatch = 25;

                    if (NoOfThreadsPhotoComment > 0)
                    {
                        numberOfAccountPatch = NoOfThreadsPhotoComment;
                    }

                    List<List<string>> list_listAccounts = new List<List<string>>();
                    string query = "select * from tbl_Campaign_Comment where CampaignName='" + Campaign + "' ";
                    DataSet ds = DataBaseHandler.SelectQuery(query, "tbl_Campaign_Comment");
                    string PhotoCommentCampaignAccount = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    string qury = "select * from AccountInfo where Username='" + PhotoCommentCampaignAccount + "'";
                    DataSet dt = DataBaseHandler.SelectQuery(qury, "AccountInfo");
                    string password = dt.Tables[0].Rows[0].ItemArray[2].ToString();
                    string account = PhotoCommentCampaignAccount + ":" + password;


                    string acc = account.Remove(account.IndexOf(':'));

                    //Run a separate thread for each account
                    InstagramUser item = null;
                    IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                    if (item != null)
                    {
                        Thread profilerThread = new Thread(StartMultiThreadsPhotoComment);
                        //  profilerThread.Name = "workerThread_Profiler_" + acc;
                        profilerThread.Name = Campaign + DateTime.Now;
                        profilerThread.IsBackground = true;

                        profilerThread.Start(new object[] { item });

                        countThreadControllerPhotoComment++;
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }

            public void StartMultiThreadsPhotoComment(object parameters)
            {
                try
                {
                    if (!isStopPhotoComment)
                    {
                        try
                        {
                            lstThreadsPhotoComment.Add(Thread.CurrentThread);
                            lstThreadsPhotoComment.Distinct();
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
                                StartActionPhotoComment(ref objFacebookUser);
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
                            lock (lockrThreadControllePhotoComment)
                            {
                                countThreadControllerPhotoComment--;
                                Monitor.Pulse(lockrThreadControllePhotoComment);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }
            }

            public void StartActionPhotoComment(ref InstagramUser Obj_Commnet)
            {
                try
                {
                    lstThreadsPhotoComment.Add(Thread.CurrentThread);
                    lstThreadsPhotoComment.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                }
                try
                {
                    string resp_Data = Obj_Commnet.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                    ClGlobul.Campiagn_Comment_PhotoIDList.Count();
                    ClGlobul.Campiagn_CommentList.Count();
                }
                catch(Exception ex)
                {
                    GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                }
                try
                {
                    ClGlobul.Campiagn_CommentList.Clear();

                    List<string> templist1 = GlobusFileHelper.ReadFile(commentPhoto_Id);

                    foreach (string item in templist1)
                    {
                        ClGlobul.Campiagn_CommentList.Add(item);

                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                }
                try
                {
                    ClGlobul.Campiagn_Comment_PhotoIDList.Clear();

                    List<string> templist1 = GlobusFileHelper.ReadFile(commentpath);

                    foreach (string item in templist1)
                    {
                        ClGlobul.Campiagn_Comment_PhotoIDList.Add(item);

                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                }
                foreach (var CommentIdsForMSG_item in ClGlobul.Campiagn_CommentList)
                {
                    StartCampaignComment(CommentIdsForMSG_item, ref Obj_Commnet);
                }

            }

            public void StartCampaignComment(string CommentIdsForMSG_item , ref InstagramUser usercomment)
            {
                try
                {
                    lstThreadsPhotoComment.Add(Thread.CurrentThread);
                    lstThreadsPhotoComment.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }

                Queue<string> CommentIdQueue = new Queue<string>();
                Queue<string> MsgQueue = new Queue<string>();

                try
                {

                    string photoLikeresult = string.Empty;


                    photoLikeresult = string.Empty;

                    string message = ClGlobul.Campiagn_Comment_PhotoIDList[RandomNumberGenerator.GenerateRandom(0, ClGlobul.Campiagn_Comment_PhotoIDList.Count)];
                    try
                    {
                        string status = Comment(CommentIdsForMSG_item, message, ref usercomment);
                        if (status == "Success")
                        {

                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  comment is successfully posted from " + CommentIdsForMSG_item + "]");
                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  comment is Fail posted from " + CommentIdsForMSG_item + "]");
                        }

                        if (PhotoCommentCampaignDelayMin != 0)
                        {
                            mindelay = PhotoCommentCampaignDelayMin;
                        }
                        if (PhotoCommentCampaignDelayMax != 0)
                        {
                            maxdelay = PhotoCommentCampaignDelayMax;
                        }

                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds For " + usercomment.username + " ]");
                        Thread.Sleep(delay * 1000);

                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                    finally
                    {
                        // GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Comment is Finished From Account : " + usercomment.username + " ]");
                    }

                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }

            public string Comment(string commentId, string CommentMsg, ref InstagramUser User_comment)
            {
                try
                {
                    lstThreadsPhotoComment.Add(Thread.CurrentThread);
                    lstThreadsPhotoComment.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }

                string FollowedPageSource = string.Empty;

                try
                {
                    string res_secondURL = User_comment.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                    string CommentIdlink = string.Empty;
                    string commentIdLoggedInLink = string.Empty;
                    if (commentId.Contains(IGGlobals.Instance.IGhomeurl))
                    {
                        commentId = commentId.Replace(IGGlobals.Instance.IGhomeurl, string.Empty);
                    }

                    if (!commentId.Contains("https://www.instagram.com/p/"))
                    {
                        try
                        {

                            CommentIdlink = "https://www.instagram.com/p/" + commentId + "/";

                            commentIdLoggedInLink = "https://www.instagram.com/p/" + commentId;
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }

                    #region Change
                    //GlobusHttpHelper _GlobusHttpHelper = new GlobusHttpHelper();

                    //ChilkatHttpHelpr _ChilkatHttpHelpr = new ChilkatHttpHelpr();

                    //InstagramAccountManager _InstagramAccountManager = new InstagramAccountManager(accountManager.Username, accountManager.Password, accountManager.proxyAddress, accountManager.proxyPassword, accountManager.proxyUsername, accountManager.proxyPassword);

                    string url = IGGlobals.Instance.IGStagram_api + commentId;
                    string resp_photourl = User_comment.globusHttpHelper.getHtmlfromUrl(new Uri(CommentIdlink), "");
                    string Cooment_ID = Utils.getBetween(resp_photourl, "content=\"instagram://media?id=", " />").Replace("\"", "");
                    string postdata_url = "https://www.instagram.com/web/comments/" + Cooment_ID + "/add/";
                    string poatdata = "comment_text=" + CommentMsg;
                    string token = Utils.getBetween(resp_photourl, "csrf_token\":\"", "\"");

                    try
                    {
                        FollowedPageSource = User_comment.globusHttpHelper.postFormDatainta(new Uri(postdata_url), poatdata, "https://www.instagram.com/", token);
                    }
                    catch (Exception ex)
                    {

                    }







                    if (FollowedPageSource.Contains("status\":\"ok\"") || FollowedPageSource.Contains("created_time"))
                    {
                        try
                        {
                            FollowedPageSource = "Success";
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        };
                    }
                    else
                    {
                        if (FollowedPageSource.Contains("Instagram API does not respond"))
                        {
                            FollowedPageSource = "Instagram API does not respond";
                        }
                        else
                        {
                            try
                            {
                                FollowedPageSource = "Fail";
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            };
                        }
                    }
                    #endregion


                    #region commented
                    //string firstUrl = "https://api.instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
                    //string secondURL = "https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
                    //string res_secondURL =_InstagramAccountManager.httpHelper.getHtmlfromUrlProxy(new Uri(secondURL), proxyAddress, 80, proxyUsername, proxyPassword);

                    //string nextUrl = "https://instagram.com/accounts/login/?force_classic_login=&next=/oauth/authorize/%3Fclient_id%3D9d836570317f4c18bca0db6d2ac38e29%26redirect_uri%3Dhttp%3A//websta.me/%26response_type%3Dcode%26scope%3Dcomments%2Brelationships%2Blikes";
                    //string res_nextUrl = _InstagramAccountManager.httpHelper.getHtmlfromUrlProxy(new Uri(nextUrl), proxyAddress, 80, proxyUsername, proxyPassword);

                    //int FirstPointToken_nextUrl = res_nextUrl.IndexOf("csrfmiddlewaretoken");
                    //string FirstTokenSubString_nextUrl = res_nextUrl.Substring(FirstPointToken_nextUrl);
                    //int SecondPointToken_nextUrl = FirstTokenSubString_nextUrl.IndexOf("/>");
                    //string Token = FirstTokenSubString_nextUrl.Substring(0, SecondPointToken_nextUrl).Replace("csrfmiddlewaretoken", string.Empty).Replace("value=", string.Empty).Replace("\"", string.Empty).Replace("'", string.Empty).Trim();
                    //string Token = string.Empty;
                    //try
                    //{
                    //    Token = getBetween(res_nextUrl, "accessToken', '", "')");
                    //}
                    //catch { }

                    //string login = "https://instagram.com/accounts/login/?force_classic_login=&next=/oauth/authorize/%3Fclient_id%3D9d836570317f4c18bca0db6d2ac38e29%26redirect_uri%3Dhttp%3A//websta.me/%26response_type%3Dcode%26scope%3Dcomments%2Brelationships%2Blikes";
                    //string postdata_Login = "csrfmiddlewaretoken=" + Token + "&username=" + Username + "&password=" + Password + "";

                    // string res_postdata_Login = _InstagramAccountManager.httpHelper.postFormData(new Uri(login), postdata_Login, login, "");

                    //string PageContent = string.Empty;
                    //PageContent = _GlobusHttpHelper.getHtmlfromUrl(new Uri(commentIdLoggedInLink), "", "", _InstagramAccountManager.proxyPassword);
                    ////if (res_postdata_Login.Contains("logout") || postdata_Login.Contains("LOG OUT"))
                    ////{
                    ////    PageContent = _InstagramAccountManager.httpHelper.getHtmlfromUrl(new Uri(CommentIdlink), "", "", accountManager.proxyPassword);
                    ////PageContent = _InstagramAccountManager.httpHelper.getHtmlfromUrl(new Uri(commentIdLoggedInLink));

                    //PageContent = _GlobusHttpHelper.getHtmlfromUrl(new Uri(CommentIdlink), "", "", accountManager.proxyPassword);
                    //PageContent = _GlobusHttpHelper.getHtmlfromUrl(new Uri(commentIdLoggedInLink));
                    ////}



                    //string PageContent = accountManager.httpHelper.getHtmlfromUrl(new Uri(CommentIdlink), "", "", accountManager.proxyAddress);
                    //string PageContent = accountManager.httpHelper.getHtmlfromUrl(new Uri(CommentIdlink), "", "", accountManager.proxyPassword);

                    //  if (PageContent.Contains("id=\"textarea"))
                    // if (PageContent.Contains("<div class=\"comments"))
                    //{
                    //check unicode character
                    //if (success.Equals("Success"))
                    //{
                    //    bool checkunicode = ContainsUnicodeCharacter(CommentMsg);

                    //    string CmntMSG = string.Empty;

                    //    if (checkunicode == false)
                    //    {
                    //        CmntMSG = CommentMsg.Replace(" ", "+");
                    //    }
                    //    else
                    //    {
                    //        CmntMSG = Uri.EscapeDataString(CommentMsg);
                    //    }

                    //    string commentPostData = "comment=+" + CmntMSG + "&media_id=" + commentId;

                    //    FollowedPageSource=_GlobusHttpHelper.postFormData(new Uri("http://websta.me/api/comments/" + commentId),commentPostData,commentIdLoggedInLink,"");

                    //    //string commentPostData = ("message=" + CmntMSG + "&messageid=" + commentId + "&t=" + RandomNumber() + "").Trim();
                    //    //string commentPostData = "comment=+" + CmntMSG + "&media_id="+commentId;
                    //   // string commentPostData = ("comment=+" + CmntMSG + "&media_id=" + commentId + "".Trim());


                    //   // // comment=+heloo&media_id=815573304185069562_3373974
                    //   // //comment=+hi&media_id=815582504685487428_17999944
                    //   // //namevalue.Add("Accept-Language", "en-us,en;q=0.5");
                    //   // namevalue.Add("Accept-Language", "en-US,en;q=0.8");
                    //   // namevalue.Add("Accept-Encoding", "gzip,deflate");
                    //   // namevalue.Add("X-Requested-With", "XMLHttpRequest");
                    //   // //namevalue.Add("Origin", "http://web.stagram.com");
                    //   // namevalue.Add("Origin", "http://websta.me");
                    //   // namevalue.Add("X-Requested-With", "XMLHttpRequest");

                    //   //// FollowedPageSource = accountManager.httpHelper.postFormDataForFollowUser(new Uri("http://web.stagram.com/post_comment/"), commentPostData, CommentIdlink, namevalue);
                    //   // //FollowedPageSource = accountManager.httpHelper.postFormDataForFollowUser(new Uri("http://websta.me/api/comments/"), commentPostData, CommentIdlink, namevalue);
                    //   // //FollowedPageSource = _GlobusHttpHelper.postFormDataForFollowUser(new Uri("http://websta.me/api/comments/"), commentPostData, CommentIdlink, namevalue);
                    //   // //FollowedPageSource = _InstagramAccountManager.httpHelper.postFormDataForFollowUser(new Uri("http://websta.me/api/comments/" + commentId), commentPostData, CommentIdlink, namevalue);
                    //   // //FollowedPageSource = _GlobusHttpHelper.postFormDataForFollowUserNew(new Uri("http://websta.me/api/comments/" + commentId), commentPostData, commentIdLoggedInLink, namevalue);

                    //} 
                    #endregion



                    try
                    {
                        if (ClGlobul.checkHashTagComment == true)
                        {
                            try
                            {
                                DataBaseHandler.InsertQuery("insert into comment_hash_tag (account_holder, photo_id, comment_date, comment_status) values ('" + User_comment.username + "','" + commentId + "','" + Convert.ToString(DateTime.Now) + "','" + FollowedPageSource + "')", "comment_hash_tag");
                            }
                            catch (Exception ex)
                            { }
                        }
                    }
                    catch
                    { }
                }
                catch
                {
                    FollowedPageSource = string.Empty;
                }
                return FollowedPageSource;

            }


            public bool ContainsUnicodeCharacter(string input)
            {
                const int MaxAnsiCode = 255;

                return input.Any(c => c > MaxAnsiCode);
            } 
        }



        }
    }
