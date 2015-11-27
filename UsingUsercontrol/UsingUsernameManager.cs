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
        public string follow_user = string.Empty;
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
            string response =string.Empty;
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
                            ClGlobul.UsingUsername_Usernmaelist.Distinct();
                        }
                    }
                }

                foreach (string user in ClGlobul.UsingUsername_Usernmaelist)
                {
                    int count = 0;
                    
                    ClGlobul.UsingUsername_likeFollowerpicture.Clear();
                    string res_secondURL = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/"), "");
                    string home_respone = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/" + user+"/"));

                    try
                    {
                        string Home_icon_Url = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                        string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                        string PPagesource = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                        string responce_icon = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");
                        if (!string.IsNullOrEmpty(responce_icon))
                        {

                            string url = "http://iconosquare.com/viewer.php#/search/" + user;
                            //postData = "q=" + Uri.EscapeDataString(itemHash);
                            //if (!itemHash.Contains("#"))
                            //{
                            //    url = IGGlobals.Instance.IGwebstaSearchUrl + postData.Substring(postData.IndexOf("=") + 1);

                            //}
                            //else
                            //{
                            //    url = IGGlobals.Instance.IGwebstaSearchUrl + postData.Substring(postData.IndexOf("=") + 1).Replace("%23", "");
                            //}

                            string referer = "http://iconosquare.com/viewer.php";
                            string viewer_responce = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php "), "");
                            string crs_token = Utils.getBetween(viewer_responce, " <div id=\"accesstoken\" style=\"display:none;\">", "</div>");
                            response = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");

                            string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + crs_token + "&q=" + user;
                            string respon_scrapeuser = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), referer);
                            string ID = Utils.getBetween(respon_scrapeuser, "id\":\"", "\"");
                            //string[] data_divided = Regex.Split(respon_scrapeuser, "username");
                            string Profile_user = "http://iconosquare.com/viewer.php#/user/" + ID + "/";
                            string post_data = "http://iconosquare.com/rqig.php?e=/users/" + ID + "/follows&a=ico2&t=" + crs_token + "&count=20";
                            string list_follower = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php#/followers/" + ID), "");
                            string follow_respo = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri(post_data), "http://iconosquare.com/viewer.php");
                            string[] data = Regex.Split(follow_respo, "username");
                            foreach (string item in data)
                            {
                                try
                                {
                                    if (item.Contains("profile_picture"))
                                    {
                                        string follower_name = Utils.getBetween(item, "\":\"", "\"");
                                        if (UsingUsername_like_Nouser > count)
                                        {
                                            ClGlobul.UsingUsername_likeFollowerpicture.Add(follower_name);
                                            GlobusLogHelper.log.Info(follower_name);
                                            count++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                                }
                            }



                            if (follow_respo.Contains("next_url"))
                            {
                                value = true;
                                while (value)
                                {
                                    if (follow_respo.Contains("next_url"))
                                    {
                                        string next_pageurl_token = Utils.getBetween(follow_respo, "next_cursor\":\"", "\"},");
                                        string page_Url = postdata + "&cursor=" + next_pageurl_token;
                                        follow_respo = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri(page_Url), "http://iconosquare.com/viewer.php");
                                        string[] data1 = Regex.Split(follow_respo, "username");
                                        foreach (string item in data1)
                                        {
                                            if (item.Contains("profile_picture"))
                                            {
                                                string follower_user = Utils.getBetween(item, "\":\"", "\"");
                                                 if(UsingUsername_like_Nouser > count)
                                                 {
                                                ClGlobul.UsingUsername_likeFollowerpicture.Add(follower_user);
                                                GlobusLogHelper.log.Info(follower_user);
                                                count++;
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
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                    }                   
                    try
                    {
                        foreach (string URL_follower in ClGlobul.UsingUsername_likeFollowerpicture)
                        {
                            int num = 0;
                            follow_user = URL_follower;
                            string follower_home = Obj_Likefollowerpic.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/" + URL_follower+"/"));
                            if(follower_home.Contains("is_private\":true"))
                            {
                                GlobusLogHelper.log.Info("This user is private" + URL_follower);
                               // break;
                            }
                            string[] data_resp = Regex.Split(follower_home, "code");
                            if(data_resp.Count()==1)
                            {
                                GlobusLogHelper.log.Info("NO Post is Posted" + URL_follower);
                              //  break;
                            }
                            
                            foreach(string value in data_resp)
                            {
                               
                                if(value.Contains("date"))
                                {
                                    if (num < 1)
                                    {
                                        string update_postId = Utils.getBetween(value, "\":\"", "\"");
                                        ClGlobul.UsingUername_PhotoIDList.Add(update_postId);
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
                                           // DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User, Photo_Id,Status) values('" + "UsingUser" + "','" + Obj_Likefollowerpic.username + "','" + PhotoList_item + "','" + Result + "')", "tbl_AccountReport");
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
                                            DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,UserName,Message, Photo_Id,Status,Operation) values('" + "UsingUser" + "','" + Obj_Likefollowerpic.username + "','" + follow_user.Replace("/n/",string.Empty) + "','" + " - " + "','" + photoID + "','"+"Success"+"','"+"Like only"+"')", "tbl_AccountReport");
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
            GlobusHttpHelper obj = obj_liking.globusHttpHelper;
            string Photolink = string.Empty;
            string FollowedPageSource = string.Empty;
            string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
            string get_photoId = obj.getHtmlfromUrl(new Uri("https://www.instagram.com/p/" + PhotoId),"");
           string photo_ID = Utils.getBetween(get_photoId, "content=\"instagram://media?id=", " />").Replace("\"","");


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

                                PageContent = obj_liking.globusHttpHelper.PostData_LoginThroughInstagram(new Uri(Photolink), "", "https://www.instagram.com/", token);
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

                        if (PageContent.Contains("Instagram API does not respond"))
                        {
                            FollowedPageSource = "Instagram API does not respond";
                        }

                        if (PageContent.Contains("{\"status\":\"ok\"}"))
                        {


                            FollowedPageSource = "LIKED";
                         //   DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User, Photo_Id,Status) values('" + "UsingUser" + "','" + obj_liking.username + "','" + PhotoId + "','" + "LIKED" + "')", "tbl_AccountReport");

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
                    }
                    else if (Check_like.Contains("viewer_has_liked\":true"))
                    {
                        FollowedPageSource = "Already LIKED";
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
                        string resp = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");

                        int count = 0;
                        ClGlobul.UsingUsername_commentFollowerpicture.Clear();
                        string response = string.Empty;

                        try
                        {
                            string Home_icon_Url = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                            string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                            string PPagesource = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                            string responce_icon = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");
                            if (!string.IsNullOrEmpty(responce_icon))
                            {

                                string url = "http://iconosquare.com/viewer.php#/search/" + user;
                                //postData = "q=" + Uri.EscapeDataString(itemHash);
                                //if (!itemHash.Contains("#"))
                                //{
                                //    url = IGGlobals.Instance.IGwebstaSearchUrl + postData.Substring(postData.IndexOf("=") + 1);

                                //}
                                //else
                                //{
                                //    url = IGGlobals.Instance.IGwebstaSearchUrl + postData.Substring(postData.IndexOf("=") + 1).Replace("%23", "");
                                //}

                                string referer = "http://iconosquare.com/viewer.php";
                                string viewer_responce = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php "), "");
                                string crs_token = Utils.getBetween(viewer_responce, " <div id=\"accesstoken\" style=\"display:none;\">", "</div>");
                                response = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");

                                string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + crs_token + "&q=" + user;
                                string respon_scrapeuser = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), referer);
                                string ID = Utils.getBetween(respon_scrapeuser, "id\":\"", "\"");
                                //string[] data_divided = Regex.Split(respon_scrapeuser, "username");
                                string Profile_user = "http://iconosquare.com/viewer.php#/user/" + ID + "/";
                                string post_data = "http://iconosquare.com/rqig.php?e=/users/" + ID + "/follows&a=ico2&t=" + crs_token + "&count=20";
                                string list_follower = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php#/followers/" + ID), "");
                                string follow_respo = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri(post_data), "http://iconosquare.com/viewer.php");
                                string[] data = Regex.Split(follow_respo, "username");
                                foreach (string item in data)
                                {
                                    try
                                    {
                                        if (item.Contains("profile_picture"))
                                        {
                                            string follower_name = Utils.getBetween(item, "\":\"", "\"");
                                            if (UsingUsername_onlycomment_Nouser > count)
                                            {
                                                ClGlobul.UsingUsername_commentFollowerpicture.Add(follower_name);
                                                GlobusLogHelper.log.Info(follower_name);
                                                count++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                                    }
                                }



                                if (follow_respo.Contains("next_url") && UsingUsername_onlycomment_Nouser > count)
                               {
                                    value = true;
                                    while (value)
                                    {
                                        if (follow_respo.Contains("next_url") || UsingUsername_onlycomment_Nouser > count)
                                        {
                                            string next_pageurl_token = Utils.getBetween(follow_respo, "next_cursor\":\"", "\"},");
                                            string page_Url = postdata + "&cursor=" + next_pageurl_token;
                                            follow_respo = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri(page_Url), "http://iconosquare.com/viewer.php");
                                            string[] data1 = Regex.Split(follow_respo, "username");
                                            foreach (string item in data1)
                                            {
                                                if (item.Contains("profile_picture"))
                                                {
                                                    string follower_user = Utils.getBetween(item, "\":\"", "\"");
                                                    if (UsingUsername_onlycomment_Nouser > count)
                                                    {
                                                        ClGlobul.UsingUsername_commentFollowerpicture.Add(follower_user);
                                                        GlobusLogHelper.log.Info(follower_user);
                                                        count++;
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
                            }
                        
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                        }
                        
                        try
                        {
                            foreach (string URL_follower in ClGlobul.UsingUsername_commentFollowerpicture)
                            {
                                int num = 0;
                                string follower_home = obj_onlycomment.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/" + URL_follower+"/"));
                                string[] Data = Regex.Split(follower_home, "code");
                                foreach (string item in Data)
                                {
                                    if (follower_home.Contains("is_private\":true"))
                                    {
                                        GlobusLogHelper.log.Info("This user is private --->" + URL_follower);
                                        break;
                                    }
                                    if (Data.Count() == 1)
                                    {
                                        GlobusLogHelper.log.Info("NO Post is Posted --->" + URL_follower);
                                        break;
                                    }



                                    if (item.Contains("date"))
                                        {
                                            if (num < 1)
                                            {
                                                string update_postId = Utils.getBetween(item, "\":\"", "\"");
                                                ClGlobul.UsingUername_commentPhotoIDList.Add(update_postId);
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
                                    DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,UserName,Message, Photo_Id,Status,Operation) values('" + "UsingUser" + "','" + obj_onlycomment.username + "','" + follow_user.Replace("/n/", string.Empty) + "','" +  message  + "','" + PhotoId + "','" + "Success" + "','" + "Comment Only" + "')", "tbl_AccountReport");
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  comment is successfully posted from " + obj_onlycomment .username+"      To===>  " +PhotoId + "]");
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
                string test_resp = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
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
                        string resp = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");

                        int count = 0;
                        ClGlobul.UsingUsername_likecommentFollowerpicture.Clear();
                        string response = string.Empty;
                        try
                        {
                            string Home_icon_Url = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                            string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                            string PPagesource = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                            string responce_icon = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");
                            if (!string.IsNullOrEmpty(responce_icon))
                            {

                                string url = "http://iconosquare.com/viewer.php#/search/" + user;
                                

                                string referer = "http://iconosquare.com/viewer.php";
                                string viewer_responce = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php "), "");
                                string crs_token = Utils.getBetween(viewer_responce, " <div id=\"accesstoken\" style=\"display:none;\">", "</div>");
                                response = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");

                                string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + crs_token + "&q=" + user;
                                string respon_scrapeuser = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), referer);
                                string ID = Utils.getBetween(respon_scrapeuser, "id\":\"", "\"");
                                //string[] data_divided = Regex.Split(respon_scrapeuser, "username");
                                string Profile_user = "http://iconosquare.com/viewer.php#/user/" + ID + "/";
                                string post_data = "http://iconosquare.com/rqig.php?e=/users/" + ID + "/follows&a=ico2&t=" + crs_token + "&count=20";
                                string list_follower = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php#/followers/" + ID), "");
                                string follow_respo = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri(post_data), "http://iconosquare.com/viewer.php");
                                string[] data = Regex.Split(follow_respo, "username");
                                foreach (string item in data)
                                {
                                    try
                                    {
                                        if (item.Contains("profile_picture"))
                                        {
                                            string follower_name = Utils.getBetween(item, "\":\"", "\"");
                                            if (UsingUsername_likecomment_Nouser > count)
                                            {
                                                ClGlobul.UsingUername_likecommentPhotoIDList.Add(follower_name);
                                                GlobusLogHelper.log.Info(follower_name);
                                                count++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                                    }
                                }



                                if (follow_respo.Contains("next_url") && UsingUsername_likecomment_Nouser > count)
                                {
                                    value = true;
                                    while (value)
                                    {
                                        if (follow_respo.Contains("next_url") || UsingUsername_likecomment_Nouser > count)
                                        {
                                            string next_pageurl_token = Utils.getBetween(follow_respo, "next_cursor\":\"", "\"},");
                                            string page_Url = postdata + "&cursor=" + next_pageurl_token;
                                            follow_respo = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri(page_Url), "http://iconosquare.com/viewer.php");
                                            string[] data1 = Regex.Split(follow_respo, "username");
                                            foreach (string item in data1)
                                            {
                                                if (item.Contains("profile_picture"))
                                                {
                                                    string follower_user = Utils.getBetween(item, "\":\"", "\"");
                                                    if (UsingUsername_likecomment_Nouser > count)
                                                    {
                                                        ClGlobul.UsingUername_likecommentPhotoIDList.Add(follower_user);
                                                        GlobusLogHelper.log.Info(follower_user);
                                                        count++;
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
                            }
                        
                        }
                        catch(Exception ex)
                        {
                            GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
                        }                        
                        try
                        {
                            foreach (string URL_follower in ClGlobul.UsingUername_likecommentPhotoIDList)
                            {
                                int num = 0;
                                string follower_home = obj_likecommnet.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/" + URL_follower));
                                string[] Data = Regex.Split(follower_home, "code");
                                foreach (string item in Data)
                                {
                                    if (follower_home.Contains("is_private\":true"))
                                    {
                                        GlobusLogHelper.log.Info("This user is private --->" + URL_follower);
                                        break;
                                    }
                                    if (Data.Count() == 1)
                                    {
                                        GlobusLogHelper.log.Info("NO Post is Posted --->" + URL_follower);
                                        break;
                                    }
                                    if (item.Contains("date"))
                                    {
                                        if (num < 1)
                                        {
                                            string photoid = Utils.getBetween(item, "\":\"", "\"");
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
                            foreach (string photoIdd in ClGlobul.UsingUername_PhotoIDList)
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

