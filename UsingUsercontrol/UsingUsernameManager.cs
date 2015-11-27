using Accounts;
using BaseLib;
using BaseLibID;
using Globussoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
namespace UsingUsercontrol
{
    public class UsingUsernameManager
    {

        #region Global variable

        #region global for onlylike
        public static int UsingUsername_like_Nouser = 0;
        public static string UsingUsercontrol_Like_single = string.Empty;
        public static string UsingUsercontrol_Like_path = string.Empty;
        public static int UsingUsername_likecomment_Nouser = 0;
        public static string UsingUsercontrol_Likecomment_single = string.Empty;
        public static string UsingUsercontrol_Likecomment_message_single = string.Empty;
        public static string UsingUsercontrol_Likecomment_User_path = string.Empty;
        public static string UsingUsercontrol_Likecomment_message_single_path = string.Empty;
        public bool isStopUsingUsername = false;
        public List<Thread> lstThreadsUsingUsername = new List<Thread>();
        public static bool onlyLike = false;
        public static string status = string.Empty;
        public static int minDelayUsingUsername = 0;
        public static int maxDelayUsingUsername = 0;
        public static int NothreadUsingUsername = 0;
        int countThreadControllerUsingUsername = 0;
        readonly object lockrThreadControlleUsingUsername = new object();
        public bool value = false;
        public static int mindelay = 0;
        public static int maxdelay = 0;
        public static bool likeandcomment = false;
        #endregion

        #region global for only comment

        public static bool onlyComment = false;
        public static int UsingUsername_onlycomment_Nouser = 0;
        public static string UsingUsercontrol_onlycomment_single = string.Empty;
        public static string UsingUsercontrol_onlycommentmessage_single = string.Empty;
        public static string UsingUsercontrol_onlycomment_path = string.Empty;
        public static string UsingUsercontrol_onlycommentmessgae = string.Empty;

        #endregion

        #region Both Like And Comment



        #endregion





        #endregion


        public int NoOfThreadsUsingUsername
        {
            get;
            set;
        }

        #region login
        public void StartUsingUsername()
        {
            countThreadControllerUsingUsername = 0;
            try
            {
                int numberOfAccountPatch = 25;

                if (NoOfThreadsUsingUsername > 0)
                {
                    numberOfAccountPatch = NoOfThreadsUsingUsername;
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
                                lock (lockrThreadControlleUsingUsername)
                                {
                                    try
                                    {
                                        if (countThreadControllerUsingUsername >= listAccounts.Count)
                                        {
                                            Monitor.Wait(lockrThreadControlleUsingUsername);
                                        }

                                        string acc = account.Remove(account.IndexOf(':'));

                                        //Run a separate thread for each account
                                        InstagramUser item = null;
                                        IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                                        if (item != null)
                                        {
                                            Thread profilerThread = new Thread(StartMultiThreadsUsingUsername);
                                            profilerThread.Name = "workerThread_Profiler_" + acc;
                                            profilerThread.IsBackground = true;

                                            profilerThread.Start(new object[] { item });

                                            countThreadControllerUsingUsername++;
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

        public void StartMultiThreadsUsingUsername(object parameters)
        {
            try
            {
                if (!isStopUsingUsername)
                {
                    try
                    {
                        lstThreadsUsingUsername.Add(Thread.CurrentThread);
                        lstThreadsUsingUsername.Distinct();
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
                        lock (lockrThreadControlleUsingUsername)
                        {
                            countThreadControllerUsingUsername--;
                            Monitor.Pulse(lockrThreadControlleUsingUsername);
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
                if (onlyLike == true)
                {
                    Start_LikeFollowerpicture(ref fbUser);
                }
                if (onlyComment == true)
                {
                    Start_onlycommentonfollowerphoto(ref fbUser);
                }
                if (likeandcomment == true)
                {
                    Start_LikeAndComment(ref fbUser);
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        #endregion

        #region only Like !

        public void Start_LikeFollowerpicture(ref InstagramUser Obj_Likefollowerpic)
        {
            try
            {
                lstThreadsUsingUsername.Add(Thread.CurrentThread);
                lstThreadsUsingUsername.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            try
            {
                if (string.IsNullOrEmpty(UsingUsercontrol_Like_path))
                {
                    if (!string.IsNullOrEmpty(UsingUsercontrol_Like_single))
                    {
                        string s = UsingUsercontrol_Like_single;
                        if (s.Contains(","))
                        {
                            string[] data = Regex.Split(s, ",");
                            foreach (string item in data)
                            {
                                ClGlobul.UsingUsername_Usernmaelist.Add(item);
                            }
                        }
                        else
                        {
                            ClGlobul.UsingUsername_Usernmaelist.Add(UsingUsercontrol_Like_single);
                        }
                    }
                }

                foreach (string user in ClGlobul.UsingUsername_Usernmaelist)
                {
                    int count = 0;
                    ClGlobul.UsingUsername_likeFollowerpicture.Clear();
                    string res_secondURL = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                    string home_respone = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/n/" + user));
                    string follow_ID = Utils.getBetween(home_respone, "<ul class=\"list-inline user-", "\">");
                    string follow_responce = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/followed-by/" + follow_ID));
                    string FollowerList = Utils.getBetween(follow_responce, "<h1> Followers List</h1>", "</ul>");
                    string[] follower = Regex.Split(FollowerList, "<li>");
                    foreach (string item in follower)
                    {
                        if (item.Contains("<a href="))
                        {
                            if (UsingUsername_like_Nouser > count)
                            {
                                string FollowerUrl = Utils.getBetween(item, "<a href=\"", "\"");
                                ClGlobul.UsingUsername_likeFollowerpicture.Add(FollowerUrl);
                                GlobusLogHelper.log.Info(FollowerUrl);
                                count++;
                            }
                        }
                    }
                    if (UsingUsername_like_Nouser > count)
                    {
                        if (follow_responce.Contains("Next Page"))
                        {
                            value = true;
                            while (value)
                            {
                                if (follow_responce.Contains("Next Page"))
                                {
                                    string nextpage_Url = Utils.getBetween(follow_responce, "<ul class=\"pager nm\">", "</ul>");
                                    string[] page_split = Regex.Split(nextpage_Url, "<a href=");
                                    string next = Utils.getBetween(page_split[2], "\"", "\"> Next Page");
                                    string finalNext_FollowingURL = "http://websta.me" + next;
                                    //  Page_Url.Add(finalNext_FollowingURL);
                                    follow_responce = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri(finalNext_FollowingURL));
                                    string secound_rep = Utils.getBetween(follow_responce, "<h1> Followers List</h1>", "</ul>");
                                    string[] split_data1 = Regex.Split(secound_rep, "<li>");
                                    foreach (string item in split_data1)
                                    {
                                        if (UsingUsername_like_Nouser > count)
                                        {
                                            string user_following = Utils.getBetween(item, "a href=\"/n/", "\"");
                                            ClGlobul.UsingUsername_likeFollowerpicture.Add(user_following);
                                            GlobusLogHelper.log.Info(user_following);
                                            count++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    value = false;
                                }
                            }
                        }
                    }
                    try
                    {
                        foreach (string URL_follower in ClGlobul.UsingUsername_likeFollowerpicture)
                        {
                            int num = 0;
                            string follower_home = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me" + URL_follower));
                            string[] Data = Regex.Split(follower_home, "<div class=\"mainimg_wrapper\">");
                            foreach (string item in Data)
                            {
                                if (item.Contains("This user is private"))
                                {
                                    GlobusLogHelper.log.Info("This user is private" + URL_follower);
                                    break;
                                }
                                if (Data.Count() == 1)
                                {
                                    GlobusLogHelper.log.Info("NO Post is Posted" + URL_follower);
                                    break;
                                }

                                if (!item.Contains("<!DOCTYPE html>"))
                                {
                                    if (num < 1)
                                    {
                                        string photoid = Utils.getBetween(item, "href=\"/p/", "\"");
                                        ClGlobul.UsingUername_PhotoIDList.Add(photoid);
                                        num++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                        }
                        string Result = string.Empty;
                        try
                        {
                            foreach (string photoID in ClGlobul.UsingUername_PhotoIDList)
                            {
                                Result = UsingUserName_liking(ref Obj_Likefollowerpic, photoID);

                                if (!Result.Contains("LIKED") && !Result.Contains("All ready LIKED"))
                                {
                                    #region commment
                                    try
                                    {
                                        QueryExecuter.insertLikeStatus(photoID, Obj_Likefollowerpic.username, 1);
                                        GlobusLogHelper.log.Info("photoID in Incorrect");
                                        try
                                        {
                                            string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator";
                                            path_AppDataFolder = path_AppDataFolder + "\\LikeList";
                                            if (!File.Exists(path_AppDataFolder))
                                            {
                                                Directory.CreateDirectory(path_AppDataFolder);
                                            }
                                            string FollowIDFilePath = path_AppDataFolder + "\\" + Obj_Likefollowerpic.username + ".csv";
                                            string CSV_Header = "Username,PhotoID,Liked";
                                            string CSV_Content = Obj_Likefollowerpic.username.Replace(",", "") + "," + photoID.Replace(",", "") + "," + Result;
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
                                        if (!ClGlobul.photoLikesCompletedList.Contains(Obj_Likefollowerpic.username))// + ":" + accountManager.Password + ":" + accountManager.proxyAddress + ":" + accountManager.proxyPort + ":" + accountManager.proxyUsername + ":" + accountManager.proxyPassword))
                                        {
                                            ClGlobul.photoLikesCompletedList.Add(Obj_Likefollowerpic.username);// + ":" + accountManager.Password + ":" + accountManager.proxyAddress + ":" + accountManager.proxyPort + ":" + accountManager.proxyUsername + ":" + accountManager.proxyPassword);
                                        }
                                        GlobusFileHelper.AppendStringToTextfileNewLine(Obj_Likefollowerpic.username + ":" + photoID, GlobusFileHelper.LikePhotoAccountIdFilePath);


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
                                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + Obj_Likefollowerpic.username + "   LIKED : " + photoID + " ]");
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
                                        GlobusFileHelper.AppendStringToTextfileNewLine(Obj_Likefollowerpic.username + ":" + photoID, GlobusFileHelper.AllReadylikePhotoAccountIdFilePath);
                                        try
                                        {
                                            string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator";
                                            path_AppDataFolder = path_AppDataFolder + "\\LikeList";
                                            if (!File.Exists(path_AppDataFolder))
                                            {
                                                Directory.CreateDirectory(path_AppDataFolder);
                                            }
                                            string FollowIDFilePath = path_AppDataFolder + "\\" + Obj_Likefollowerpic.username + ".csv";
                                            string CSV_Header = "Username,PhotoID,Already LIKED";
                                            string CSV_Content = Obj_Likefollowerpic.username.Replace(",", "") + "," + photoID.Replace(",", "") + "," + Result;
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
                                    #endregion



                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + Obj_Likefollowerpic.username + " Already LIKED :  " + photoID + " ]");
                                }
                                else
                                {
                                    try
                                    {
                                        if (Result.Contains("LIKED"))
                                        {
                                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + Obj_Likefollowerpic.username + "  LIKED : " + photoID + "]");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                    }
                                }

                                if (minDelayUsingUsername != 0)
                                {
                                    mindelay = minDelayUsingUsername;
                                }
                                if (maxDelayUsingUsername != 0)
                                {
                                    maxdelay = maxDelayUsingUsername;
                                }
                                int delayy = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delayy + " Seconds For " + Obj_Likefollowerpic.username + " ]");
                                Thread.Sleep(delayy * 1000);
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                        }
                        finally
                        {

                            GlobusLogHelper.log.Info("========================");
                            GlobusLogHelper.log.Info("Process Completed !!");
                            GlobusLogHelper.log.Info("========================");
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
        public string UsingUserName_liking(ref InstagramUser obj_liking, string PhotoId)
        {
            try
            {
                lstThreadsUsingUsername.Add(Thread.CurrentThread);
                lstThreadsUsingUsername.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string res_secondURL = obj_liking.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
            string Photolink = string.Empty;
            string FollowedPageSource = string.Empty;
            string like = string.Empty;
            string new_repce = string.Empty;

            try
            {
                GlobusHttpHelper obj = obj_liking.globusHttpHelper;
                string res = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                if (PhotoId.Contains(IGGlobals.Instance.IGhomeurl))
                {
                    PhotoId = PhotoId.Replace(IGGlobals.Instance.IGhomeurl, string.Empty);
                }
                if (!PhotoId.Contains(IGGlobals.Instance.IGstagramurl))
                {
                    Photolink = IGGlobals.Instance.IGLikewebsta_api + PhotoId + "/".Replace(IGGlobals.Instance.IGhomeurl, "");
                }
                else
                {
                    Photolink = PhotoId;

                }
                string url = IGGlobals.Instance.IGhomeurl + PhotoId;
                string Check_like = obj_liking.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");
                string Data = Utils.getBetween(Check_like, "class=\"list-inline pull-left\">", "</ul>");
                if (Data.Contains("</i> Like</button>"))
                {
                    // string PageContent = Photo_likebyID.globusHttpHelper.getHtmlfromUrl(new Uri(Photolink), "", "", Photo_likebyID.proxyip);
                    string PageContent = string.Empty;
                    if (string.IsNullOrEmpty(obj_liking.proxyport))
                    {
                        obj_liking.proxyport = "80";
                    }
                    try
                    {
                        if (ClGlobul.checkHashTagLiker == true)
                        {
                            PageContent = obj_liking.globusHttpHelper.getHtmlfromUrl(new Uri(Photolink), "");
                        }
                        else
                        {
                            PageContent = obj_liking.globusHttpHelper.getHtmlfromUrlProxy(new Uri(Photolink), obj_liking.proxyip, Convert.ToInt32(obj_liking.proxyport), obj_liking.proxyusername, obj_liking.proxypassword);
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
                            PageContent = obj_liking.globusHttpHelper.getHtmlfromUrl(new Uri(Photolink));
                        }
                        else
                        {
                            PageContent = obj_liking.globusHttpHelper.getHtmlfromUrlProxy(new Uri(Photolink), "", 80, "", "");
                        }
                    }

                    if (PageContent.Contains("message\":\"LIKED\""))
                    {


                        FollowedPageSource = "LIKED";

                        try
                        {
                            if (ClGlobul.checkHashTagLiker == true)
                            {
                                try
                                {
                                    DataBaseHandler.InsertQuery("insert into liker_hash_tag (account_holder, photo_id, like_date, like_status) values ('" + obj_liking.username + "','" + PhotoId + "','" + Convert.ToString(DateTime.Now) + "','" + FollowedPageSource + "')", "liker_hash_tag");
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
                    if(PageContent.Contains("Instagram API does not respond"))
                    {
                        FollowedPageSource = "Instagram API does not respond";
                    }
                }
                else if (Data.Contains("</i> Liked</button>"))
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

        #endregion

        #region Only Comment

        public void Start_onlycommentonfollowerphoto(ref InstagramUser obj_onlycomment)
        {
            try
            {
                lstThreadsUsingUsername.Add(Thread.CurrentThread);
                lstThreadsUsingUsername.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            try
            {
                if ((!string.IsNullOrEmpty(UsingUsercontrol_onlycomment_single)) && (!string.IsNullOrEmpty(UsingUsercontrol_onlycommentmessage_single)))
                {
                    string s = UsingUsercontrol_onlycomment_single;
                    if (s.Contains(","))
                    {
                        string[] data = Regex.Split(s, ",");
                        foreach (string item in data)
                        {
                            ClGlobul.UsingUsername_onlycommentusernameList.Add(item);
                        }
                    }
                    else
                    {
                        ClGlobul.UsingUsername_onlycommentusernameList.Add(UsingUsercontrol_onlycomment_single);
                    }
                    string k = UsingUsercontrol_onlycommentmessage_single;
                    if (k.Contains(","))
                    {
                        string[] data1 = Regex.Split(k, ",");
                        foreach (string item1 in data1)
                        {
                            ClGlobul.UsingUsername_onlycommentmessageList.Add(item1);
                        }
                    }
                    else
                    {
                        ClGlobul.UsingUsername_onlycommentmessageList.Add(UsingUsercontrol_onlycommentmessage_single);
                    }
                }


                foreach (string user in ClGlobul.UsingUsername_onlycommentusernameList)
                {
                    try
                    {
                        string resp = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");

                        int count = 0;
                        ClGlobul.UsingUsername_commentFollowerpicture.Clear();
                        string res_secondURL = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                        string home_respone = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/n/" + user));
                        string follow_ID = Utils.getBetween(home_respone, "<ul class=\"list-inline user-", "\">");
                        string follow_responce = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/followed-by/" + follow_ID));
                        string FollowerList = Utils.getBetween(follow_responce, "<h1> Followers List</h1>", "</ul>");
                        string[] follower = Regex.Split(FollowerList, "<li>");
                        foreach (string item in follower)
                        {
                            if (item.Contains("<a href="))
                            {
                                if (UsingUsername_onlycomment_Nouser > count)
                                {
                                    string FollowerUrl = Utils.getBetween(item, "<a href=\"", "\"");
                                    ClGlobul.UsingUsername_commentFollowerpicture.Add(FollowerUrl);
                                    GlobusLogHelper.log.Info(FollowerUrl);
                                    count++;
                                }
                            }
                        }
                        if (UsingUsername_onlycomment_Nouser > count)
                        {
                            if (follow_responce.Contains("Next Page"))
                            {
                                value = true;
                                while (value)
                                {
                                    if (follow_responce.Contains("Next Page") && UsingUsername_onlycomment_Nouser > count)
                                    {
                                        string nextpage_Url = Utils.getBetween(follow_responce, "<ul class=\"pager nm\">", "</ul>");
                                        string[] page_split = Regex.Split(nextpage_Url, "<a href=");
                                        string next = Utils.getBetween(page_split[2], "\"", "\"> Next Page");
                                        string finalNext_FollowingURL = "http://websta.me" + next;
                                        //  Page_Url.Add(finalNext_FollowingURL);
                                        follow_responce = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri(finalNext_FollowingURL));
                                        string secound_rep = Utils.getBetween(follow_responce, "<h1> Followers List</h1>", "</ul>");
                                        string[] split_data1 = Regex.Split(secound_rep, "<li>");
                                        foreach (string item in split_data1)
                                        {
                                            if (UsingUsername_onlycomment_Nouser > count)
                                            {
                                                string user_following = Utils.getBetween(item, "a href=\"/n/", "\"");
                                                ClGlobul.UsingUsername_commentFollowerpicture.Add(user_following);
                                                GlobusLogHelper.log.Info(user_following);
                                                count++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        value = false;
                                    }
                                }
                            }
                        }
                        try
                        {
                            foreach (string URL_follower in ClGlobul.UsingUsername_commentFollowerpicture)
                            {
                                int num = 0;
                                string follower_home = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me" + URL_follower));
                                string[] Data = Regex.Split(follower_home, "<div class=\"mainimg_wrapper\">");
                                foreach (string item in Data)
                                {
                                    if (item.Contains("This user is private"))
                                    {
                                        GlobusLogHelper.log.Info("This user is private --->" + URL_follower);
                                        break;
                                    }
                                    if (Data.Count() == 1)
                                    {
                                        GlobusLogHelper.log.Info("NO Post is Posted --->" + URL_follower);
                                        break;
                                    }

                                    if (!item.Contains("<!DOCTYPE html>"))
                                    {
                                        if (num < 1)
                                        {
                                            string photoid = Utils.getBetween(item, "href=\"/p/", "\"");
                                            ClGlobul.UsingUername_commentPhotoIDList.Add(photoid);
                                            num++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }

                            }


                            foreach (string PhotoId in ClGlobul.UsingUername_commentPhotoIDList)
                            {
                                string message = ClGlobul.UsingUsername_onlycommentmessageList[RandomNumberGenerator.GenerateRandom(0, ClGlobul.UsingUsername_onlycommentmessageList.Count)];
                                string status = Comment(PhotoId, message, ref obj_onlycomment);
                                if (status == "Success")
                                {
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  comment is successfully posted from " + PhotoId + "]");
                                }
                                else
                                {

                                }

                                if (minDelayUsingUsername != 0)
                                {
                                    mindelay = minDelayUsingUsername;
                                }
                                if (maxDelayUsingUsername != 0)
                                {
                                    maxdelay = maxDelayUsingUsername;
                                }

                                int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds For " + obj_onlycomment.username + " ]");
                                Thread.Sleep(delay * 1000);

                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }
                        finally
                        {
                            GlobusLogHelper.log.Info("========================");
                            GlobusLogHelper.log.Info("Process Completed !!");
                            GlobusLogHelper.log.Info("========================");
                        }

                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }

            }
            catch { }
        }

        public string Comment(string commentId, string CommentMsg, ref InstagramUser User_comment)
        {
            //abc:
            try
            {
                lstThreadsUsingUsername.Add(Thread.CurrentThread);
                lstThreadsUsingUsername.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            string FollowedPageSource = string.Empty;
            try
            {
                string CommentIdlink = string.Empty;
                string commentIdLoggedInLink = string.Empty;
                if (commentId.Contains(IGGlobals.Instance.IGhomeurl))
                {
                    commentId = commentId.Replace(IGGlobals.Instance.IGhomeurl, string.Empty);
                }

                if (!commentId.Contains(IGGlobals.Instance.IGstagramurl))
                {
                    try
                    {

                        CommentIdlink = IGGlobals.Instance.IGstagramurl_2 + commentId + "/";

                        commentIdLoggedInLink = IGGlobals.Instance.IGhomeurl + commentId;
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }

                string url = IGGlobals.Instance.IGStagram_api + commentId;

                bool checkunicode = ContainsUnicodeCharacter(CommentMsg);

                string CmntMSG = string.Empty;


                if (checkunicode == false)
                {
                    try
                    {
                        CmntMSG = CommentMsg.Replace(" ", "+");

                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    };
                }
                else
                {
                    try
                    {
                        CmntMSG = Uri.EscapeDataString(CommentMsg);
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    };
                }

                //  string commentPostData = "comment=+" + CmntMSG + "&media_id=" + commentId;
                try
                {
                    string commentPostData = "comment=+++" + CmntMSG + "&media_id=" + commentId;

                    FollowedPageSource = User_comment.globusHttpHelper.postFormData(new Uri(url), commentPostData, CommentIdlink, "");
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

                if (FollowedPageSource.Contains("status\":\"OK\"") || FollowedPageSource.Contains("created_time"))
                {
                    try
                    {
                        FollowedPageSource = "Success";
                        // GlobusLogHelper.log.Info("");
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
                        //CommentMsg = ClGlobul.UsingUsername_onlycommentmessageList[RandomNumberGenerator.GenerateRandom(0, ClGlobul.UsingUsername_onlycommentmessageList.Count)];
                      //  goto abc;
                        FollowedPageSource = "Instagram API does not respond";
                        #region insta login
                        //try
                        //{
                        //    User_comment.globusHttpHelper = new GlobusHttpHelper();
                        //    string resp_home = User_comment.globusHttpHelper.getHtmlfromUrl(new Uri("https://instagram.com/"), "");
                        //    string token = Utils.getBetween(resp_home, "csrf_token\":\"", "\"");
                        //    string postdata = "username=" + User_comment.username + "&password=" + User_comment.password;
                        //    string login_Instagram = User_comment.globusHttpHelper.postFormDatainta(new Uri("https://instagram.com/accounts/login/ajax/"), postdata, "https://instagram.com/", token);
                        //    FollowedPageSource = "Instagram API does not respond";
                        //    string setting = User_comment.globusHttpHelper.getHtmlfromUrlinta(new Uri("https://instagram.com/accounts/manage_access/"), "", token);
                        //    string setting_token = Utils.getBetween(setting, "value=\"", "\"");
                        //    string postrevoke = "token=" + setting_token;
                        //    string rvoke = User_comment.globusHttpHelper.postFormDatainta(new Uri("https://instagram.com/publicapi/oauth/revoke_access"), postrevoke, "https://instagram.com/accounts/manage_access/", "");

                        //    AccountManager obj_AccountManager = new AccountManager();

                        //    status = obj_AccountManager.LoginUsingGlobusHttp(ref User_comment);
                        //    if (status == "Success")
                        //    {
                        //        goto abc;
                        //    }
                        //    else
                        //    {
                        //        GlobusLogHelper.log.Info("Instagram API does not respond");
                        //    }


                        //}
                        //catch (Exception ex)
                        //{
                        //    return ex.Message;
                        //};
                        #endregion
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
                try
                {
                    if (ClGlobul.checkHashTagComment == true)
                    {
                        try
                        {
                          // DataBaseHandler.InsertQuery("insert into comment_hash_tag (account_holder, photo_id, comment_date, comment_status) values ('" + accountManager.Username + "','" + commentId + "','" + Convert.ToString(DateTime.Now) + "','" + FollowedPageSource + "')", "comment_hash_tag");
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }
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


        #endregion

        #region Both Like And Comment

        public void Start_LikeAndComment(ref InstagramUser obj_likecommnet)
        {
            try
            {
                lstThreadsUsingUsername.Add(Thread.CurrentThread);
                lstThreadsUsingUsername.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                string test_resp = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                if (string.IsNullOrEmpty(UsingUsercontrol_Likecomment_User_path) && string.IsNullOrEmpty(UsingUsercontrol_Likecomment_message_single_path))
                { 
                if (!string.IsNullOrEmpty(UsingUsercontrol_Likecomment_single) && (!string.IsNullOrEmpty(UsingUsercontrol_Likecomment_message_single)))
                {
                    string s = UsingUsercontrol_Likecomment_single;
                    if (s.Contains(","))
                    {
                        string[] data = Regex.Split(s, ",");
                        foreach (string item in data)
                        {
                            ClGlobul.UsingUsername_likecommentUserList.Add(item);
                        }
                    }
                    else
                    {
                        ClGlobul.UsingUsername_likecommentUserList.Add(UsingUsercontrol_Likecomment_single);
                    }
                    string k = UsingUsercontrol_Likecomment_message_single;
                    if (k.Contains(","))
                    {
                        string[] data1 = Regex.Split(k, ",");
                        foreach (string item1 in data1)
                        {
                            ClGlobul.UsingUsername_likecommentMessageList.Add(item1);
                        }
                    }
                    else
                    {
                        ClGlobul.UsingUsername_likecommentMessageList.Add(UsingUsercontrol_Likecomment_message_single);
                    }
                }
                    }
                foreach (string user in ClGlobul.UsingUsername_likecommentUserList)
                {
                    try
                    {
                        string resp = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");

                        int count = 0;
                        ClGlobul.UsingUsername_likecommentFollowerpicture.Clear();
                        string res_secondURL = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                        string home_respone = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/n/" + user));
                        string follow_ID = Utils.getBetween(home_respone, "<ul class=\"list-inline user-", "\">");
                        string follow_responce = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/followed-by/" + follow_ID));
                        string FollowerList = Utils.getBetween(follow_responce, "<h1> Followers List</h1>", "</ul>");
                        string[] follower = Regex.Split(FollowerList, "<li>");
                        foreach (string item in follower)
                        {
                            if (item.Contains("<a href="))
                            {
                                if (UsingUsername_likecomment_Nouser > count)
                                {
                                    string FollowerUrl = Utils.getBetween(item, "<a href=\"", "\"");
                                    ClGlobul.UsingUsername_likecommentFollowerpicture.Add(FollowerUrl);
                                    GlobusLogHelper.log.Info(FollowerUrl);
                                    count++;
                                }
                            }
                        }
                        if (UsingUsername_likecomment_Nouser > count)
                        {
                            if (follow_responce.Contains("Next Page"))
                            {
                                value = true;
                                while (value)
                                {
                                    if (follow_responce.Contains("Next Page") && UsingUsername_likecomment_Nouser > count)
                                    {
                                        string nextpage_Url = Utils.getBetween(follow_responce, "<ul class=\"pager nm\">", "</ul>");
                                        string[] page_split = Regex.Split(nextpage_Url, "<a href=");
                                        string next = Utils.getBetween(page_split[2], "\"", "\"> Next Page");
                                        string finalNext_FollowingURL = "http://websta.me" + next;
                                        //  Page_Url.Add(finalNext_FollowingURL);
                                        follow_responce = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri(finalNext_FollowingURL));
                                        string secound_rep = Utils.getBetween(follow_responce, "<h1> Followers List</h1>", "</ul>");
                                        string[] split_data1 = Regex.Split(secound_rep, "<li>");
                                        foreach (string item in split_data1)
                                        {
                                            if (UsingUsername_likecomment_Nouser > count)
                                            {
                                                string user_following = Utils.getBetween(item, "a href=\"/n/", "\"");
                                                ClGlobul.UsingUsername_likecommentFollowerpicture.Add(user_following);
                                                GlobusLogHelper.log.Info(user_following);
                                                count++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        value = false;
                                    }
                                }
                            }
                        }
                        try
                        {
                            foreach (string URL_follower in ClGlobul.UsingUsername_likecommentFollowerpicture)
                            {
                                int num = 0;
                                string follower_home = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me" + URL_follower));
                                string[] Data = Regex.Split(follower_home, "<div class=\"mainimg_wrapper\">");
                                foreach (string item in Data)
                                {
                                    if (item.Contains("This user is private"))
                                    {
                                        GlobusLogHelper.log.Info("This user is private --->" + URL_follower);
                                        break;
                                    }
                                    if (Data.Count() == 1)
                                    {
                                        GlobusLogHelper.log.Info("NO Post is Posted --->" + URL_follower);
                                        break;
                                    }
                                    if (!item.Contains("<!DOCTYPE html>"))
                                    {
                                        if (num < 1)
                                        {
                                            string photoid = Utils.getBetween(item, "href=\"/p/", "\"");
                                            ClGlobul.UsingUername_likecommentPhotoIDList.Add(photoid);
                                            num++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            foreach (string photoIdd in ClGlobul.UsingUername_likecommentPhotoIDList)
                            {

                                string message = ClGlobul.UsingUsername_likecommentMessageList[RandomNumberGenerator.GenerateRandom(0, ClGlobul.UsingUsername_likecommentMessageList.Count)];
                                 bothlikecomment(photoIdd, message, ref obj_likecommnet);
                            }
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
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void bothlikecomment(string photoId, string photoMsg, ref InstagramUser User_comment)
        {
            try
            {
                lstThreadsUsingUsername.Add(Thread.CurrentThread);
                lstThreadsUsingUsername.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                string status_Like = UsingUserName_liking(ref User_comment, photoId);
                string status_comment = Comment(photoId, photoMsg, ref User_comment);
                if (status_Like == "LIKED" && status_comment == "Success")
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  Comment and Like is successfully posted from " + photoId + "]");
                }
                if (status_Like == "LIKED" && status_comment == "Fail")
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  Comment Not Done But Like is successfully posted from " + photoId + "]");
                    }
                if (status_comment == "Success" && status_Like == "Already LIKED")
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  Comment  Done But Like is Allready Done on " + photoId + "]");
                    }
                if (status_Like == "Instagram API does not respond" && status_comment == "Instagram API does not respond")
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  Fail to comment and Like on Because Instagram API does not respond " + photoId + "]");
                    }
                        if(status_Like == "" && status_comment == "Success")
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  only comment done on " + photoId + "]");
                        }
                        if (status_Like == "" && status_comment == "Fail")
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  Fail to comment and Like on  " + photoId + "]");
                        }       
                else
                {

                }

                if (minDelayUsingUsername != 0)
                {
                    mindelay = minDelayUsingUsername;
                }
                if (maxDelayUsingUsername != 0)
                {
                    maxdelay = maxDelayUsingUsername;
                }

                int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds For " + User_comment.username + " ]");
                Thread.Sleep(delay * 1000);
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
           
        }

        #endregion



    }
}

