using Accounts;
using BaseLib;
using BaseLibID;
using Globussoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HashTagsManager
{
    public class hash_managerlibry
    {

        public static bool DivideEqual = false;
        public static int NumberAcoount = 0;
        public static List<string> DivideEqual_list = new List<string>();
        public static bool DivideByUser = false;
        public static int Divide_data_NoUser = 0;
        public static int DivideData_Thread = 0;
        public static bool chkNotSendRequest = false;


        # region Global Variable

        public static bool Hash_comment = false;
        public bool isStopHash_comment = false;
        public List<Thread> lstThreadsHash_comment = new List<Thread>();
        public static int minDelayHash_comment = 0;
        public static int maxDelayHash_comment = 0;
        public static int NothreadHash_comment = 0;
        int countThreadControllerHash_comment = 0;
        readonly object lockrThreadControllHash_comment = new object();
        public static string status = string.Empty;
        public static string Hash_comment_Usernamesingle = string.Empty;
        public static string Hash_comment_Usernamepath = string.Empty;
        public static string Hash_comment_Messagesingle = string.Empty;
        public static string Hash_comment_Messagepath = string.Empty;
        public static int Number_Hash_photocomment = 0;
        public int mindelay = 0;
        public int maxdelay = 0;
        public int i = 0;
        public int counterComment = 0;
        public static string Hash_Like_Unlike_path = string.Empty;
        public static string Hash_Like_Unlike_single = string.Empty;
        public static bool Hash_Like = false;
        public static int hashlike_no_photo = 0;
        public static string Hash_Follower_single = string.Empty;
        public static string Hash_Follower_path = string.Empty;
        public static int hashFollower_Number = 0;
        public static bool Hash_Follow = false;
        public int counterFollow = 0;

        #endregion


        public int NoOfThreadsHash_comment
        {
            get;
            set;
        }


        #region Common Login Method

        public void StartHash_comment()
        {
            countThreadControllerHash_comment = 0;
            try
            {
                int numberOfAccountPatch = 25;

                if (NoOfThreadsHash_comment > 0)
                {
                    numberOfAccountPatch = NoOfThreadsHash_comment;
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
                                lock (lockrThreadControllHash_comment)
                                {
                                    try
                                    {
                                        if (countThreadControllerHash_comment >= listAccounts.Count)
                                        {
                                            Monitor.Wait(lockrThreadControllHash_comment);
                                        }

                                        string acc = account.Remove(account.IndexOf(':'));

                                        //Run a separate thread for each account
                                        InstagramUser item = null;
                                        IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                                        if (item != null)
                                        {
                                            Thread profilerThread = new Thread(StartMultiThreadsHashModule);
                                            profilerThread.Name = "workerThread_Profiler_" + acc;
                                            profilerThread.IsBackground = true;

                                            profilerThread.Start(new object[] { item });

                                            countThreadControllerHash_comment++;
                                            
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

        public void StartMultiThreadsHashModule(object parameters)
        {
            try
            {
                if (!isStopHash_comment)
                {
                    try
                    {
                        lstThreadsHash_comment.Add(Thread.CurrentThread);
                        lstThreadsHash_comment.Distinct();
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
                            StartActionHashModule(ref objFacebookUser);
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
                        lock (lockrThreadControllHash_comment)
                        {
                            countThreadControllerHash_comment--;
                            Monitor.Pulse(lockrThreadControllHash_comment);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }

        private void StartActionHashModule(ref InstagramUser fbUser)
        {

            try
            {
                if (Hash_comment == true)
                {
                    startProcessUsingHashTag(ref fbUser);
                }
                if (Hash_Like == true)
                {
                    start_hashLike(ref fbUser);
                }
                if (Hash_Follow == true)
                {
                   
                    Start_hashFollow(ref fbUser);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        #endregion



        #region Hash Follower
        public static int Count_theard = 0;
        public static List<Thread> lstFollower_hashtag = new List<Thread>();
        public void Start_hashFollow(ref InstagramUser obj_Hashfollow)
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
                lstFollower_hashtag.Add(Thread.CurrentThread);
                if(lstFollower_hashtag.Count>1)
                {
                    Thread.Sleep(10 * 5000);

                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                string res_secondURL = obj_Hashfollow.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/"), "");
                if (!string.IsNullOrEmpty(Hash_Follower_single))
                {
                    ClGlobul.HashFollower.Clear();
                    string s = Hash_Follower_single;
                    if (s.Contains(','))
                    {
                        try
                        {
                            string[] Data = s.Split(',');

                            foreach (var item in Data)
                            {
                                if (!ClGlobul.HashFollower.Contains(item))
                                {
                                    ClGlobul.HashFollower.Add(item);
                                    ClGlobul.HashFollower = ClGlobul.HashFollower.Distinct().ToList();
                                }
                            }
                        }
                        catch { };
                    }
                    else
                    {
                        if (!ClGlobul.HashFollower.Contains(Hash_Follower_single))
                        {
                            ClGlobul.HashFollower.Add(Hash_Follower_single);
                            ClGlobul.HashFollower = ClGlobul.HashFollower.Distinct().ToList();
                        }
                    }
                }

                if (ClGlobul.HashFollower.Count != 0)
                {
                    if (hashFollower_Number != 0)
                    {
                        ClGlobul.NumberOfProfilesToFollow = hashFollower_Number;
                        ClGlobul.SnapVideosCounterfollow = hashFollower_Number * ClGlobul.HashFollower.Count*IGGlobals.listAccounts.Count;

                    }
                    else
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Please enter the numbers of profiles to follow and continue. ]");
                        return;
                    }
                }

                #region scrapHashTagFollowr

                
               // int Count_theard =0;
               
                int countForhashFollower = ClGlobul.HashFollower.Count;
                try
                {
                    string[] list_follower = ClGlobul.HashFollower.ToArray();

                    if (Count_theard < 1)
                    {
                        Count_theard++;
                        foreach (string hashKeyword in list_follower)
                        {
                            try
                            {

                                GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraping Users with HashTag " + hashKeyword + "]");
                                ClGlobul.lstHashTagUserIdTemp = GetUser(hashKeyword, ref obj_Hashfollow);
                                ClGlobul.lstHashTagUserId.AddRange(ClGlobul.lstHashTagUserIdTemp);

                            }
                            catch { };
                        }
                    }
                    else
                    {
                        Thread.Sleep(10 * 1000);
                    }
                }
                catch { };

                #endregion

                try
                {
                    if (countForhashFollower != 0)
                    {
                        try
                        {
                            lstThreadsHash_comment.Add(Thread.CurrentThread);
                            lstThreadsHash_comment.Distinct();
                            Thread.CurrentThread.IsBackground = true;
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }

                        HashTagFollow(ref obj_Hashfollow, ClGlobul.lstHashTagUserId);
                        
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


        public void HashTagFollow(ref InstagramUser accountManager, List<string> Usercount)
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string res_secondURL = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/"), "");
            try
            {
                //Globals.lstScrapThreaddatahashliker.Add(Thread.CurrentThread);
                //Thread.CurrentThread.IsBackground = true;
                //Globals.lstScrapThreaddatahashliker = Globals.lstScrapThreaddatahashliker.Distinct().ToList();
            }
            catch { };
            #region variables

            //List<string> Usercount = new List<string>();
            #endregion variables

            #region commented

            #endregion
            try
            {
                foreach (string urlToFollow in Usercount)
                {

                    FollowUrls(ref accountManager, urlToFollow);


                    if (minDelayHash_comment != 0)
                    {
                        mindelay = minDelayHash_comment;
                    }
                    if (maxDelayHash_comment != 0)
                    {
                        maxdelay = maxDelayHash_comment;
                    }

                    Random obj_rn = new Random();
                    int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                    delay = obj_rn.Next(mindelay, maxdelay);
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                    Thread.Sleep(delay * 1000);
                }
            }


            catch (Exception ex)
            {

            }
            finally
            {
                GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[Process completed for Follow Using HashTag. ]");
            }


        }

        public void FollowUrls(ref InstagramUser accountManager, string url)
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            string followStatus = string.Empty;
            try
            {
                string res_secondURL = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/"), "");
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
                    if (!(counterFollow == ClGlobul.SnapVideosCounterfollow))
                    {

                        string status = Follow(url, ref accountManager);
                        // Thread.Sleep(ClGlobul.hashTagDelay * 1000);
                        counterFollow++;
                        if (status == "Followed")
                        {
                            DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,FollowerName, Message,Photo_Id,HashOperation,Status) values('" + "HashComment" + "','" + accountManager.username + "','" + url + "','" + "-" + "','" + "-" + "','" + "#Follow" + "','" + "Success" + "')", "tbl_AccountReport");
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Profile followed with url : " + url + " with User = " + user + " , " + "Count" + counterFollow + "]");
                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Profile Unfollowed with url : " + url + " with User = " + user + " , " + "Count" + counterFollow + " ]");
                            //Log("[ " + DateTime.Now + "] " + " [ " + ClGlobul.NumberOfProfilesToFollow + " profiles Unfollowed ]");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        public string Follow(string UserName, ref InstagramUser accountManager)
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
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
               
                string PK = string.Empty;
                if (UserPageContent.Contains(""))
                {
                   
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
                        FollowedPageSource = accountManager.globusHttpHelper.postFormDatainta(new Uri(postData), "", "https://www.instagram.com/" + txt_name + "/", csrf_token);
                    }
                    catch { }
                }
                if (string.IsNullOrEmpty(FollowedPageSource))
                {

                }


                if (FollowedPageSource.Contains("followed_by_viewer\":true"))
                {

                    string status = string.Empty;
                    try
                    {
                        status = QueryExecuter.getFollowStatus1(accountManager.username, UserName);
                    }
                    catch { }
                    if (string.IsNullOrEmpty(status))
                    {
                        if (FollowedPageSource.Contains("has_requested_viewer\":true"))
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


        public List<string> GetUser(string hashTag , ref InstagramUser obj_user)
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            List<string> lstUser = new List<string>();
            int count_listdata = 0;
            string response = string.Empty;
            try
            {
                string Home_icon_Url = obj_user.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                string PPagesource = obj_user.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                string responce_icon = obj_user.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");
                if (!string.IsNullOrEmpty(responce_icon))
                {

                    string url = "http://iconosquare.com/viewer.php#/search/" + hashTag;


                    string referer = "http://iconosquare.com/viewer.php";
                    string viewer_responce = obj_user.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php "), "");
                    string crs_token = Utils.getBetween(viewer_responce, " <div id=\"accesstoken\" style=\"display:none;\">", "</div>");
                    response = obj_user.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");

                    string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + crs_token + "&q=" + hashTag;
                    string respon_scrapeuser = obj_user.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), referer);
                    string[] data_divided = Regex.Split(respon_scrapeuser, "username");
                    

                        count_listdata++;

                        foreach (string var in data_divided)
                        {
                            if (var.Contains("profile_picture"))
                            {
                                string User_list = Utils.getBetween(var, "\":\"", "\"");
                                if (lstUser.Count < ClGlobul.NumberOfProfilesToFollow)
                                {

                                    lstUser.Add(User_list);
                                    // lstUser = lstUser.Distinct().ToList();


                                }
                                else
                                {
                                    return lstUser;
                                }
                            }
                        }
                }
                    else
                    {
                        Thread.Sleep(1 * 1000);
                    }
                }
        

            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
            return lstUser;
        }


        #endregion


        #region HashComment

        public void startProcessUsingHashTag(ref InstagramUser obj_hashcomment)
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                string res_secondURL = obj_hashcomment.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                if (string.IsNullOrEmpty(Hash_comment_Usernamepath) && string.IsNullOrEmpty(Hash_comment_Messagepath))
                {
                    if (!string.IsNullOrEmpty(Hash_comment_Usernamesingle) && !string.IsNullOrEmpty(Hash_comment_Messagesingle))
                    {
                        string s = Hash_comment_Usernamesingle;
                        string k = Hash_comment_Messagesingle;
                        if (s.Contains(','))
                        {
                            try
                            {
                                string[] Data = s.Split(',');

                                foreach (var item in Data)
                                {
                                    if (!ClGlobul.HashComment.Contains(item))
                                    {
                                        ClGlobul.HashComment.Add(item);
                                    }
                                }
                            }
                            catch { };
                        }
                        else
                        {
                            if (!ClGlobul.HashComment.Contains(Hash_comment_Usernamesingle))
                            {
                                ClGlobul.HashComment.Add(Hash_comment_Usernamesingle);
                            }
                        }
                        if (k.Contains(","))
                        {
                            try
                            {
                                string[] Data = k.Split(',');

                                foreach (var item in Data)
                                {
                                    if (!ClGlobul.HashCommentMessage.Contains(item))
                                    {
                                        ClGlobul.HashCommentMessage.Add(item);
                                    }
                                }
                            }
                            catch { };
                        }
                        else
                        {
                            if (ClGlobul.HashCommentMessage.Contains(Hash_comment_Messagesingle))
                            {
                                ClGlobul.HashCommentMessage.Add(Hash_comment_Messagesingle);
                            }
                        }
                    }
                }
                if (ClGlobul.HashComment.Count != 0)
                {
                    if (Number_Hash_photocomment != 0)
                    {
                        ClGlobul.NumberofSnapsVideosToComment = Number_Hash_photocomment * IGGlobals.listAccounts.Count * ClGlobul.HashComment.Count;
                        ClGlobul.SnapVideosCounterComment = Number_Hash_photocomment * ClGlobul.HashComment.Count;
                    }
                    else
                    {

                        //MessageBox.Show("Please enter the numbers of snaps or videos to like and continue.");
                        //AddToHashLoggerDAta("[ " + DateTime.Now + "] " + "[ Please enter the numbers of snaps or videos to like and continue. ]");
                        return;
                    }
                }

                Start_HashComment(ref obj_hashcomment);

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
            }
        }
        List<string> lstHashTagUserIdCommentTemp = new List<string>();
        List<string> lstHashTagCommentUserId = new List<string>();
        public void Start_HashComment(ref InstagramUser obj_hashcomment)
        {

            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            GlobusHttpHelper obj = obj_hashcomment.globusHttpHelper;
            string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");

           
            lstHashTagCommentUserId.Clear();
            foreach (string item in ClGlobul.HashComment)
            {
                lstHashTagUserIdCommentTemp = GetPhotoId1(item);
                lstHashTagCommentUserId.AddRange(lstHashTagUserIdCommentTemp);

            }

            if (ClGlobul.HashComment.Count != 0)
            {
                ///foreach (string hashTagComment in ClGlobul.HashComment)
                {
                    HashTagComment(ref obj_hashcomment, lstHashTagCommentUserId);
                }
            }

        }

        public void HashTagComment(ref InstagramUser accountManager, List<string> hashTag)
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                string res_secondURL = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                List<string> hashcomment_list = new List<string>();
                foreach (string item in ClGlobul.HashComment)
                {
                    int NoOfCommentsDone = 0;
                    i = 0;
                    // hashcomment_list = hashTag.ToList().Distinct().ToList();

                     foreach (string urlToComment in hashTag.ToList())
                    {

                        NoOfCommentsDone++;
                        if (NoOfCommentsDone > ClGlobul.NumberofSnapsVideosToComment)
                        {
                            break;
                        }
                        CommentOnSnapsVideos(ref accountManager, urlToComment);
                       // Thread.Sleep(15 * 1000);
                        if (minDelayHash_comment != 0)
                        {
                            mindelay = minDelayHash_comment;
                        }
                        if (maxDelayHash_comment != 0)
                        {
                            maxdelay = maxDelayHash_comment;
                        }

                        Random obj_rand = new Random();
                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                        delay = obj_rand.Next(mindelay, maxdelay);
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                        Thread.Sleep(delay * 1000);
                         hashTag.Remove(urlToComment);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ClGlobul.countNoOFAccountHashComment--;
                if (hashTag.Count() == 0)
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[Process completed for Comment Using HashTag. ]");
                }
            }
        }


        public void CommentOnSnapsVideos(ref InstagramUser accountManager, string url)
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string message = string.Empty;
            ClGlobul.checkHashTagComment = true;
            string user = accountManager.username;
            string commentStatus = string.Empty;
            string likeStatus = string.Empty;
            try
            {
                string res_secondURL = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                //GlobusLogHelper.log.Info("Login Sucess" + accountManager.username);
                try
                {
                    if (ClGlobul.HashCommentMessage.Count >= ClGlobul.NumberofSnapsVideosToComment)
                    {
                        message = ClGlobul.HashCommentMessage[i];
                        i++;
                    }
                    else
                    {
                        message = ClGlobul.HashCommentMessage[RandomNumberGenerator.GenerateRandom(0, ClGlobul.HashCommentMessage.Count)];
                    }
                }
                catch (Exception ex)
                { }
                try
                {
                    try
                    {
                        DataSet commentDS = DataBaseHandler.SelectQuery("select comment_status from comment_hash_tag where account_holder ='" + user + "' and photo_id = '" + url.Replace("http://websta.me/p/", string.Empty) + "'", "comment_hash_tag");
                        if (commentDS.Tables[0].Rows.Count != 0)
                        {
                            commentStatus = commentDS.Tables[0].Rows[0].ItemArray[0].ToString();
                        }
                    }
                    catch (Exception ex)
                    { }
                    try
                    {
                        DataSet likeDS = DataBaseHandler.SelectQuery("select like_status from liker_hash_tag where account_holder ='" + user + "' and photo_id ='" + url.Replace("http://websta.me/p/", string.Empty) + "'", "liker_hash_tag");
                        if (likeDS.Tables[0].Rows.Count != 0)
                        {
                            likeStatus = likeDS.Tables[0].Rows[0].ItemArray[0].ToString();
                        }
                    }
                    catch (Exception ex)
                    { }

                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                }

                if (!(commentStatus == "Success"))
                {
                    //if (!(counterComment == ClGlobul.NumberofSnapsVideosToComment))
                    if (!(counterComment == ClGlobul.NumberofSnapsVideosToComment))
                    {
                        string status = Comment(url, message, ref accountManager);
                        Thread.Sleep(ClGlobul.hashTagDelay * 1000);
                        if (status == "Success")
                        {
                           // string Photo_Db_ID = url.Replace("http://websta.me/p/", string.Empty);
                            DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User, Message,Photo_Id,HashOperation,Status) values('" + "HashComment" + "','" + accountManager.username + "','" + message + "','" + url + "','" + "#Comment" + "','" + "Success" + "')", "tbl_AccountReport");
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Commented on snap/video with url : " + url + " , " + " Count" + counterComment + " with this message : " + message + " ]");
                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Could not comment on snap/video with url : " + url + " , " + " Count" + counterComment + "]");
                        }



                        if (ClGlobul.isCommentAndLikeChecked == true)
                        {
                            if (!(likeStatus == "LIKED"))
                            {
                                ClGlobul.checkHashTagLiker = true;
                                string photoStatus = photolike(url, ref accountManager);
                                Thread.Sleep(ClGlobul.hashTagDelay * 1000);
                                if (photoStatus == "LIKED")
                                {
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Snap/Video liked with url : " + url + " , " + " Count" + counterComment + "]");
                                }
                                else
                                {
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Snap/Video not liked with url : " + " , " + " Count" + counterComment + "]");
                                }
                            }
                            else
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Snap/video with url : " + url + " , " + " Count" + counterComment + " ]");
                            }
                        }

                        counterComment++;



                    }
                    else
                    {
                        if (counterComment == ClGlobul.NumberofSnapsVideosToComment)
                        // if (counterComment == ClGlobul.SnapVideosCounterfollow)
                        {
                            return;
                        }
                        ClGlobul.isCommentLimitReached = true;
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Commented on " + ClGlobul.NumberofSnapsVideosToLike + " snaps/videos. ");
                        return;
                    }
                }
                else if ((commentStatus == "Success"))
                {
                    if ((counterComment >= ClGlobul.NumberofSnapsVideosToComment))
                    {
                        string status = Comment(url, message, ref accountManager);
                        Thread.Sleep(ClGlobul.hashTagDelay * 1000);
                        if (status == "Success")
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Commented on snap/video with url : " + url + " , " + " Count" + counterComment + " with this message : " + message + " ]");
                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Could not comment on snap/video with url : " + url + " , " + " Count" + counterComment + "]");
                        }



                        if (ClGlobul.isCommentAndLikeChecked == true)
                        {
                            if (!(likeStatus == "LIKED"))
                            {
                                ClGlobul.checkHashTagLiker = true;
                                string photoStatus = photolike(url, ref accountManager);
                                Thread.Sleep(ClGlobul.hashTagDelay * 1000);
                                if (photoStatus == "LIKED")
                                {
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Snap/Video liked with url : " + url + " , " + " Count" + counterComment + "]");
                                }
                                else
                                {
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Snap/Video not liked with url : " + " , " + " Count" + counterComment + "]");
                                }
                            }
                            else
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Snap/video with url : " + url + " , " + " Count" + counterComment + " ]");
                            }
                        }

                        counterComment++;



                    }
                    else
                    {
                        // if (counterComment == ClGlobul.NumberofSnapsVideosToComment)
                        if (counterComment == ClGlobul.SnapVideosCounterfollow)
                        {
                            return;
                        }
                        ClGlobul.isCommentLimitReached = true;
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Commented on " + ClGlobul.NumberofSnapsVideosToLike + " snaps/videos. ");
                        return;
                    }
                }
            }

            catch (Exception ex)
            { }
        }

        public string Comment(string commentId, string CommentMsg, ref InstagramUser accountManager)
        {

            string FollowedPageSource = string.Empty;

            try
            {
                string res_secondURL = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
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
                string resp_photourl = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(CommentIdlink), "");
                string Cooment_ID = Utils.getBetween(resp_photourl, "content=\"instagram://media?id=", " />").Replace("\"", "");
                string postdata_url = "https://www.instagram.com/web/comments/" + Cooment_ID + "/add/";
                string poatdata = "comment_text=" + CommentMsg;
                string token = Utils.getBetween(resp_photourl, "csrf_token\":\"", "\"");

                try
                {
                    FollowedPageSource = accountManager.globusHttpHelper.postFormDatainta(new Uri(postdata_url), poatdata, "https://www.instagram.com/", token);
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
                            DataBaseHandler.InsertQuery("insert into comment_hash_tag (account_holder, photo_id, comment_date, comment_status) values ('" + accountManager.username + "','" + commentId + "','" + Convert.ToString(DateTime.Now) + "','" + FollowedPageSource + "')", "comment_hash_tag");
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

        public string photolike(string PhotoId, ref InstagramUser accountManager)
        {


            //string Photolink = string.Empty;
            //string FollowedPageSource = string.Empty;
            //string like = string.Empty;
            //string new_repce = string.Empty;
            //int mindelay = 0;
            //int maxdelay = 1;
            //if (!string.IsNullOrEmpty(obj.txtdelaymin.Text) && NumberHelper.ValidateNumber(obj.txtdelaymin.Text))
            //{
            //    mindelay = Convert.ToInt32(obj.txtdelaymin.Text);
            //}
            //if (!string.IsNullOrEmpty(obj.txtdelaymax.Text) && NumberHelper.ValidateNumber(obj.txtdelaymax.Text))
            //{
            //    maxdelay = Convert.ToInt32(obj.txtdelaymax.Text);
            //}


            //#region like by User NameBy  Anil


            //if (frm_stagram.var1 == true)
            //{
            //    try
            //    {
            //        string count = frm_stagram.Usercount_like;
            //        int temp = int.Parse(count);
            //        int flag = 0;
            //        string Url_user = "http://websta.me/n/" + PhotoId;
            //        string responce_user = accountManager.httpHelper.getHtmlfromUrl(new Uri(Url_user), "");
            //        string[] data = Regex.Split(responce_user, "class=\"mainimg_wrapper\"");
            //        foreach (string item in data)
            //        {
            //            if (flag < temp)
            //            {

            //                if (!item.Contains("!DOCTYPE html>"))
            //                {
            //                    PhotoId = Utils.getBetween(item, " href=\"/p/", "\"");
            //                    flag++;
            //                    try
            //                    {
            //                        if (PhotoId.Contains("http://websta.me/p/"))
            //                        {
            //                            PhotoId = PhotoId.Replace("http://websta.me/p/", string.Empty);
            //                        }
            //                        if (!PhotoId.Contains("http://web.stagram.com/"))
            //                        {
            //                            Photolink = "http://websta.me/api/like/" + PhotoId + "/".Replace("http://websta.me/p/", "");

            //                        }
            //                        else
            //                        {
            //                            Photolink = PhotoId;

            //                        }

            //                        try
            //                        {
            //                            string photoPage = accountManager.httpHelper.getHtmlfromUrl(new Uri("http://websta.me/p/" + PhotoId), "");

            //                            string isLikeCheck = Utils.getBetween(photoPage, "fa fa-heart\"></i> ", "</button>");
            //                            if (isLikeCheck.Contains("Liked"))
            //                            {
            //                                FollowedPageSource = "Already LIKED";
            //                                obj.AddTophotoLogger("[ " + DateTime.Now + " ] => [ " + accountManager.Username + " Already LIKED : " + "http://websta.me/p/" + PhotoId + " ]");
            //                                //return FollowedPageSource;
            //                            }

            //                        }
            //                        catch { };

            //                        if (!FollowedPageSource.Contains("Already LIKED"))
            //                        {
            //                            //string PageContent = accountManager.httpHelper.getHtmlfromUrl(new Uri(Photolink), "", "", accountManager.proxyAddress);
            //                            string PageContent = string.Empty;
            //                            if (string.IsNullOrEmpty(accountManager.proxyPort))
            //                            {
            //                                accountManager.proxyPort = "80";
            //                            }
            //                            try
            //                            {
            //                                if (ClGlobul.checkHashTagLiker == true)
            //                                {
            //                                    PageContent = accountManager.httpHelper.getHtmlfromUrl(new Uri(Photolink), "");
            //                                }
            //                                else
            //                                {
            //                                    PageContent = accountManager.httpHelper.getHtmlfromUrlProxy(new Uri(Photolink), accountManager.proxyAddress, Convert.ToInt32(accountManager.proxyPort), accountManager.proxyUsername, accountManager.proxyPassword);
            //                                }

            //                            }
            //                            catch { }
            //                            if (string.IsNullOrEmpty(PageContent))
            //                            {
            //                                if (ClGlobul.checkHashTagLiker == true)
            //                                {
            //                                    PageContent = accountManager.httpHelper.getHtmlfromUrl(new Uri(Photolink));
            //                                }
            //                                else
            //                                {
            //                                    PageContent = accountManager.httpHelper.getHtmlfromUrlProxy(new Uri(Photolink), "", 80, "", "");
            //                                }
            //                            }

            //                            if (PageContent.Contains("message\":\"LIKED\""))
            //                            {
            //                                #region commented code
            //                                //bool isContain = accountManager.httpHelper.CheckAttributeexsist(PageContent, "span", "dislike_button");

            //                                //if (PageContent.Contains("img/liked.png"))
            //                                //{
            //                                //    FollowedPageSource = "All ready LIKED";
            //                                //}

            //                                //if (lstLikes.Count > 0)
            //                                //{
            //                                //    try
            //                                //    {
            //                                //        like = lstLikes[0];
            //                                //    }
            //                                //    catch
            //                                //    {
            //                                //    }
            //                                //}
            //                                //if (like == "1")
            //                                //{
            //                                //    FollowedPageSource = "All ready LIKED";
            //                                //}
            //                                //if (isContain)
            //                                //{
            //                                //    FollowedPageSource = "All ready LIKED";
            //                                //}
            //                                //else
            //                                //{
            //                                //string PostData = "&pk=" + PhotoId + "&t=653"; 
            //                                #endregion

            //                                #region commented
            //                                //string PostData = "&pk=" + PhotoId + "&t=" + RandomNumber() + "";
            //                                //namevalue.Add("Accept-Language", "en-us,en;q=0.5");
            //                                //namevalue.Add("X-Requested-With", "XMLHttpRequest");
            //                                //namevalue.Add("Origin", "http://web.stagram.com");
            //                                //namevalue.Add("X-Requested-With", "XMLHttpRequest");
            //                                //FollowedPageSource = accountManager.httpHelper.postFormDataForFollowUser(new Uri("http://web.stagram.com/do_like/"), PostData.Trim(), Photolink, namevalue);
            //                                //if (FollowedPageSource.Contains("\"message\":null}"))
            //                                //{
            //                                //    FollowedPageSource = "All ready LIKED";
            //                                //} 
            //                                #endregion

            //                                FollowedPageSource = "LIKED";

            //                                try
            //                                {
            //                                    if (ClGlobul.checkHashTagLiker == true)
            //                                    {
            //                                        try
            //                                        {
            //                                            DataBaseHandler.InsertQuery("insert into liker_hash_tag (account_holder, photo_id, like_date, like_status) values ('" + accountManager.Username + "','" + PhotoId + "','" + Convert.ToString(DateTime.Now) + "','" + FollowedPageSource + "')", "liker_hash_tag");
            //                                        }
            //                                        catch
            //                                        { }
            //                                    }
            //                                }
            //                                catch (Exception ex)
            //                                { }

            //                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + accountManager.Username + "  LIKED : " + "http://websta.me/p/" + PhotoId + " ]");
            //                            }
            //                            //else if (string.IsNullOrEmpty(FollowedPageSource))
            //                            //{
            //                            //    FollowedPageSource = "Already LIKED";
            //                            //}
            //                            //lock (_lockObject)
            //                            //{

            //                            //}
            //                        }
            //                    }

            //                    catch { };


            //                }
            //            }
            //            int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
            //            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds For " + accountManager.Username + " ]");
            //            Thread.Sleep(delay * 1000);

            //            if (flag == temp)
            //            {
            //                break;
            //            }
            //        }

            //    }
            //    catch { };

            //#endregion

            //}
            //else
            //{
            //    try
            //    {
            //        if (PhotoId.Contains("http://websta.me/p/"))
            //        {
            //            PhotoId = PhotoId.Replace("http://websta.me/p/", string.Empty);
            //        }
            //        if (!PhotoId.Contains("http://web.stagram.com/"))
            //        {
            //            Photolink = "http://websta.me/api/like/" + PhotoId + "/".Replace("http://websta.me/p/", "");
            //        }
            //        else
            //        {
            //            Photolink = PhotoId;

            //        }

            //        try
            //        {
            //            string photoPage = accountManager.httpHelper.getHtmlfromUrl(new Uri("http://websta.me/p/" + PhotoId), "");

            //            string isLikeCheck = Utils.getBetween(photoPage, "fa fa-heart\"></i> ", "</button>");
            //            if (isLikeCheck.Contains("Liked"))
            //            {
            //                FollowedPageSource = "Already LIKED";
            //                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + accountManager.Username + " Already LIKED : " + "http://websta.me/p/" + PhotoId + " ]");
            //                //return FollowedPageSource;
            //            }

            //        }
            //        catch { };


            //        if (!FollowedPageSource.Contains("Already LIKED"))
            //        {
            //            //string PageContent = accountManager.httpHelper.getHtmlfromUrl(new Uri(Photolink), "", "", accountManager.proxyAddress);
            //            string PageContent = string.Empty;
            //            if (string.IsNullOrEmpty(accountManager.proxyPort))
            //            {
            //                accountManager.proxyPort = "80";
            //            }
            //            try
            //            {
            //                if (ClGlobul.checkHashTagLiker == true)
            //                {
            //                    PageContent = accountManager.httpHelper.getHtmlfromUrl(new Uri(Photolink), "");
            //                }
            //                else
            //                {
            //                    PageContent = accountManager.httpHelper.getHtmlfromUrlProxy(new Uri(Photolink), accountManager.proxyAddress, Convert.ToInt32(accountManager.proxyPort), accountManager.proxyUsername, accountManager.proxyPassword);
            //                }

            //            }
            //            catch { }
            //            if (string.IsNullOrEmpty(PageContent))
            //            {
            //                if (ClGlobul.checkHashTagLiker == true)
            //                {
            //                    PageContent = accountManager.httpHelper.getHtmlfromUrl(new Uri(Photolink));
            //                }
            //                else
            //                {
            //                    PageContent = accountManager.httpHelper.getHtmlfromUrlProxy(new Uri(Photolink), "", 80, "", "");
            //                }
            //            }

            //            if (PageContent.Contains("message\":\"LIKED\""))
            //            {                                                  
            //                FollowedPageSource = "LIKED";

            //                try
            //                {
            //                    if (ClGlobul.checkHashTagLiker == true)
            //                    {
            //                        try
            //                        {
            //                            DataBaseHandler.InsertQuery("insert into liker_hash_tag (account_holder, photo_id, like_date, like_status) values ('" + accountManager.Username + "','" + PhotoId + "','" + Convert.ToString(DateTime.Now) + "','" + FollowedPageSource + "')", "liker_hash_tag");
            //                        }
            //                        catch
            //                        { }
            //                    }
            //                }
            //                catch (Exception ex)
            //                { }


            //            }
            //            if (PageContent.Contains("\"message\":\"you cannot like this media\""))
            //            {
            //                FollowedPageSource = "Cannot Like this media";
            //            }
            //        }
            //    }
            //    catch { };

            //}
            return null;
        }

        #region ContainsUnicodeCharacter
        public bool ContainsUnicodeCharacter(string input)
        {
            const int MaxAnsiCode = 255;

            return input.Any(c => c > MaxAnsiCode);
        }
        #endregion


        public List<string> GetPhotoId1(string hashTag)
        {

            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch
            {
            }

            string url = string.Empty;
            bool value = true;
            
            try
            {
                url = "https://www.instagram.com/explore/tags/" + hashTag + "/";
                GlobusHttpHelper objInstagramUser = new GlobusHttpHelper();


                string pageSource = objInstagramUser.getHtmlfromUrl(new Uri(url), "", "");
                if (string.IsNullOrEmpty(pageSource))
                {
                    pageSource = objInstagramUser.getHtmlfromUrl(new Uri(url), "", "");
                }

                if (!string.IsNullOrEmpty(pageSource))
                {
                    if (pageSource.Contains("date"))
                    {
                        string token = Utils.getBetween(pageSource, "csrf_token\":\"", "\"}");
                        string[] arr = Regex.Split(pageSource, "code");
                        string imageId = string.Empty;
                        string imageSrc = string.Empty;
                        if (arr.Length > 1)
                        {
                            arr = arr.Skip(1).ToArray();
                            foreach (string itemarr in arr)
                            {
                                try
                                {
                                    // By Anil 
                                    if (itemarr.Contains("date"))
                                    {
                                        imageId = Utils.getBetween(itemarr, "\":\"", "\"");
                                        if (!string.IsNullOrEmpty(imageId))
                                        {
                                            //lstPhotoId.Add(imageId);
                                            //  lstPhotoId.Distinct();
                                            if (ClGlobul.lstPhotoId.Count < ClGlobul.SnapVideosCounterComment)
                                            {
                                                if (!ClGlobul.lstPhotoId.Contains(imageId))
                                                {
                                                    ClGlobul.lstPhotoId.Add(imageId);
                                                    ClGlobul.lstPhotoId.Distinct();
                                                    //  return lstPhotoId;
                                                }
                                            }
                                            else
                                            {
                                                return ClGlobul.lstPhotoId;
                                            }

                                            //imageId = "http://websta.me"+imageId;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                            #region Forpagination
                            if (pageSource.Contains("has_next_page\":true"))
                            {
                                while (value)
                                {
                                    if (pageSource.Contains("has_next_page\":true") && ClGlobul.lstPhotoId.Count < ClGlobul.NumberofSnapsVideosToComment)
                                    {
                                        string IDD = Utils.getBetween(pageSource, "\"id\":\"", "\"");
                                        string code_ID = Utils.getBetween(pageSource, "end_cursor\":\"", "\"");
                                        string postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                        pageSource = objInstagramUser.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, "https://www.instagram.com/" + hashTag, token);
                                        string[] data1 = Regex.Split(pageSource, "code");
                                        foreach (string val in data1)
                                        {
                                            if (val.Contains("date"))
                                            {
                                                if (ClGlobul.lstPhotoId.Count < ClGlobul.NumberofSnapsVideosToComment)
                                                {
                                                    string photo_codes = Utils.getBetween(val, "\":\"", "\"");
                                                    if (!ClGlobul.lstPhotoId.Contains(photo_codes))
                                                    {
                                                        ClGlobul.lstPhotoId.Add(photo_codes);
                                                    }
                                                }
                                                else
                                                {
                                                    return ClGlobul.lstPhotoId;
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
                            #endregion
                        }

                    }
                }

            }

            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }








            //string url = IGGlobals.Instance.IGWEPME + "tag/" + hashTag.Replace("%23", "").Replace("#", "");
            //GlobusHttpHelper objInstagramUser = new GlobusHttpHelper();
            //List<string> lstPhotoId = new List<string>();

            //string pageSource = objInstagramUser.getHtmlfromUrl(new Uri(url), "");
            //if (string.IsNullOrEmpty(pageSource))
            //{
            //    pageSource = objInstagramUser.getHtmlfromUrl(new Uri(url), "");
            //}
            //if (!string.IsNullOrEmpty(pageSource))
            //{
            //    if (pageSource.Contains("<div class=\"mainimg_wrapper\">"))
            //    {
            //        string[] arr = Regex.Split(pageSource, "<div class=\"mainimg_wrapper\">");
            //        if (arr.Length > 1)
            //        {
            //            arr = arr.Skip(1).ToArray();
            //            foreach (string itemarr in arr)
            //            {
            //                try
            //                {
            //                    string startString = "<a href=\"/p/";
            //                    string endString = "\" class=\"mainimg\"";
            //                    string imageId = string.Empty;
            //                    string imageSrc = string.Empty;
            //                    if (itemarr.Contains("<a href=\"/p/"))
            //                    {
            //                        int indexStart = itemarr.IndexOf("<a href=\"/p/");
            //                        string itemarrNow = itemarr.Substring(indexStart);
            //                        if (itemarrNow.Contains(startString) && itemarrNow.Contains(endString))
            //                        {
            //                            try
            //                            {
            //                                imageId = Utils.getBetween(itemarrNow, startString, endString).Replace("/", "");
            //                            }
            //                            catch { }
            //                            if (!string.IsNullOrEmpty(imageId))
            //                            {
            //                                lstPhotoId.Add(imageId);
            //                                lstPhotoId.Distinct();
            //                                if (lstPhotoId.Count >= ClGlobul.SnapVideosCounterComment)
            //                                {
            //                                    return lstPhotoId;
            //                                }

            //                                //imageId = "http://websta.me"+imageId;
            //                            }
            //                        }
            //                    }
            //                }
            //                catch (Exception ex)
            //                {

            //                }

            //            }

            //            #region pagination

            //            string pageLink = string.Empty;
            //            while (true)
            //            {

            //                try
            //                {
            //                    //Globals.HasTagListListThread.Add(Thread.CurrentThread);
            //                    //Globals.HasTagListListThread.Distinct();
            //                    //Thread.CurrentThread.IsBackground = true;
            //                }
            //                catch
            //                {
            //                }
            //                //if (stopScrapImageBool) return;
            //                string startString = "<a href=\"/p/";
            //                string endString = "\" class=\"mainimg\"";
            //                string imageId = string.Empty;
            //                string imageSrc = string.Empty;

            //                if (!string.IsNullOrEmpty(pageLink))
            //                {
            //                    pageSource = objInstagramUser.getHtmlfromUrl(new Uri(pageLink));
            //                }

            //                if (pageSource.Contains("<ul class=\"pager\">") && pageSource.Contains("rel=\"next\">"))
            //                {
            //                    try
            //                    {
            //                        pageLink = Utils.getBetween(pageSource, "<ul class=\"pager\">", "rel=\"next\">");
            //                    }
            //                    catch { }
            //                    if (!string.IsNullOrEmpty(pageLink))
            //                    {
            //                        try
            //                        {
            //                            int len = pageLink.IndexOf("<a href=\"");
            //                            len = len + ("<a href=\"").Length;
            //                            pageLink = pageLink.Substring(len);
            //                            pageLink = pageLink.Trim();
            //                            pageLink = pageLink.TrimEnd(new char[] { '"' });
            //                            pageLink = "http://websta.me/" + pageLink;
            //                        }
            //                        catch { }
            //                        if (!string.IsNullOrEmpty(pageLink))
            //                        {
            //                            string response = string.Empty;
            //                            try
            //                            {
            //                                response = objInstagramUser.getHtmlfromUrl(new Uri(pageLink));
            //                            }
            //                            catch { }
            //                            if (!string.IsNullOrEmpty(response))
            //                            {
            //                                if (response.Contains("<div class=\"mainimg_wrapper\">"))
            //                                {
            //                                    try
            //                                    {
            //                                        string[] arr1 = Regex.Split(response, "<div class=\"mainimg_wrapper\">");
            //                                        if (arr1.Length > 1)
            //                                        {
            //                                            arr1 = arr1.Skip(1).ToArray();
            //                                            foreach (string items in arr1)
            //                                            {
            //                                                try
            //                                                {
            //                                                    //if (stopScrapImageBool) return;
            //                                                    if (items.Contains("<a href=\"/p/"))
            //                                                    {
            //                                                        int indexStart = items.IndexOf("<a href=\"/p/");
            //                                                        string itemarrNow = items.Substring(indexStart);

            //                                                        try
            //                                                        {
            //                                                            imageId = Utils.getBetween(itemarrNow, startString, endString).Replace("/", "");
            //                                                        }
            //                                                        catch { }
            //                                                        if (!string.IsNullOrEmpty(imageId))
            //                                                        {
            //                                                            lstPhotoId.Add(imageId);
            //                                                            lstPhotoId.Distinct();
            //                                                            if (lstPhotoId.Count >= ClGlobul.NumberofSnapsVideosToComment)
            //                                                            {
            //                                                                return lstPhotoId;
            //                                                            }

            //                                                            //imageId = "http://websta.me"+imageId;
            //                                                        }


            //                                                        //counter++;

            //                                                        //Addtologger("Image DownLoaded with ImageName  "+imageId+"_"+counter);
            //                                                        if (lstPhotoId.Count >= ClGlobul.NumberofSnapsVideosToComment)
            //                                                        {
            //                                                            return lstPhotoId;
            //                                                        }
            //                                                    }
            //                                                }

            //                                                catch { }
            //                                            }
            //                                            if (lstPhotoId.Count >= ClGlobul.NumberofSnapsVideosToComment)
            //                                            {
            //                                                return lstPhotoId;
            //                                            }
            //                                        }
            //                                    }
            //                                    catch { }

            //                                }
            //                            }
            //                            else
            //                            {

            //                            }

            //                        }
            //                        else
            //                        {
            //                            break;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    break;
            //                }
            //            }
            //            #endregion
            //        }
            //    }
            //}

            return ClGlobul.lstPhotoId;
        }


        #endregion


        #region For Like And Unlike

        public void start_hashLike(ref InstagramUser obj_HashLike)
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            try
            {


                string res_secondURL = obj_HashLike.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/"), "");
                if (!string.IsNullOrEmpty(Hash_Like_Unlike_single))
                {
                    ClGlobul.HashLiker.Clear();
                    string s = Hash_Like_Unlike_single;
                    if (s.Contains(','))
                    {
                        try
                        {
                            string[] Data = s.Split(',');
                            foreach (var item in Data)
                            {
                                ClGlobul.HashLiker.Add(item);
                            }
                        }
                        catch { };

                    }
                    else
                    {
                        ClGlobul.HashLiker.Add(Hash_Like_Unlike_single);
                    }
                }
                if (ClGlobul.HashLiker.Count != 0)
                {
                    if (hashlike_no_photo != 0)  //txtNumberPicsVideosLike
                    {
                        ClGlobul.NumberofSnapsVideosToLike = hashlike_no_photo;
                        ClGlobul.SnapVideosCounter = hashlike_no_photo * ClGlobul.HashLiker.Count;
                    }
                    else
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Please enter the numbers of snaps or videos to like and continue. ]");
                        return;
                    }
                }

                #region ScraperhashLiker
                List<string> lstHashTagUserIdLikeTemp = new List<string>();
                List<string> lstHashLikeTagUserLikeId = new List<string>();
                foreach (string hashLikerKeyword in ClGlobul.HashLiker)
                {

                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraping PhotoId with HashTag " + hashLikerKeyword + "]");
                    lstHashTagUserIdLikeTemp = GetPhotoId(hashLikerKeyword);
                    lstHashLikeTagUserLikeId.AddRange(lstHashTagUserIdLikeTemp);
                    //GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraped Users with HashTag " + hashLikerKeyword + "]");
                }
                //  }
                #endregion





                if (ClGlobul.HashLiker.Count != 0)
                {
                    //foreach (string hashTagLike in ClGlobul.HashLiker)


                    HashTagLike(ref obj_HashLike, lstHashLikeTagUserLikeId);

                }





            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }


        public void HashTagLike(ref InstagramUser obj_hashlike, List<string> hashTag)
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                string res_secondURL = obj_hashlike.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                foreach (string urlToLike in hashTag)//hashTag //snapsVideoUrl
                {


                    string result = LikeSnapsVideos(ref obj_hashlike, urlToLike);

                    if (result.Contains("LIKED"))
                    {
                        DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User, Message,Photo_Id,HashOperation,Status) values('" + "HashComment" + "','" + obj_hashlike.username + "','" + "-" + "','" + urlToLike + "','" + "#Like" + "','" + "Success" + "')", "tbl_AccountReport");
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Liked " + urlToLike + " Success ]");
                    }
                    if (result.Contains("Already LIKED"))
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Allready Liked " + urlToLike + " ]");
                    }
                    if(string.IsNullOrEmpty(result))
                    {
                        GlobusLogHelper.log.Info("------ Page Not found ------");
                    }
                    if (minDelayHash_comment != 0)
                    {
                        mindelay = minDelayHash_comment;
                    }
                    if (maxDelayHash_comment != 0)
                    {
                        maxdelay = maxDelayHash_comment;
                    }
                    Random obj_ran = new Random();
                    int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                    delay = obj_ran.Next(mindelay, maxdelay);
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                    Thread.Sleep(delay * 1000);

                }
            }

            catch (Exception ex)
            {

            }

            finally
            {
                //process completed.

                if (ClGlobul.countNOOfFollowersandImageDownload == 0)
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[Process completed for Like Using HashTag. ]");
                }
            }
        }
        public string LikeSnapsVideos(ref InstagramUser Photo_likebyID, string PhotoId)
        {

            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string Photolink = string.Empty;
            string FollowedPageSource = string.Empty;
            string like = string.Empty;
            string new_repce = string.Empty;
            string resp_photodetail = Photo_likebyID.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/p/" + PhotoId+"/"), "");
            string phto_ID = Utils.getBetween(resp_photodetail, "content=\"instagram://media?id=", " />").Replace("\"", "");

            try
            {
                GlobusHttpHelper obj = Photo_likebyID.globusHttpHelper;
                string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                if (PhotoId.Contains("https://www.instagram.com/p/"))
                {
                    PhotoId = PhotoId.Replace("https://www.instagram.com/p/", string.Empty);
                }
                if (!PhotoId.Contains("https://www.instagram.com/p/"))
                {
                    Photolink = "https://www.instagram.com/web/likes/" + phto_ID + "/like/ ".Replace(IGGlobals.Instance.IGhomeurl, "");
                }
                else
                {
                    Photolink = PhotoId;

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
                            //DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User, Photo_Id,Status) values('" + "PhotoLikeModule" + "','" + Photo_likebyID.username + "','" + PhotoId + "','" + "LIKED" + "')", "tbl_AccountReport");

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
        




        public List<string> GetPhotoId(string hashTag)
        {

            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string url = string.Empty;
            bool value = true;
            List<string> lstPhotoId = new List<string>();
            try
            {
                url = "https://www.instagram.com/explore/tags/" + hashTag + "/";
                GlobusHttpHelper objInstagramUser = new GlobusHttpHelper();
                

                string pageSource = objInstagramUser.getHtmlfromUrl(new Uri(url), "", "");
                if (string.IsNullOrEmpty(pageSource))
                {
                    pageSource = objInstagramUser.getHtmlfromUrl(new Uri(url), "", "");
                }

                if (!string.IsNullOrEmpty(pageSource))
                {
                    if (pageSource.Contains("date"))
                    {
                        string token = Utils.getBetween(pageSource, "csrf_token\":\"", "\"}");
                        string[] arr = Regex.Split(pageSource, "code");
                        string imageId = string.Empty;
                        string imageSrc = string.Empty;
                        if (arr.Length > 1)
                        {
                            arr = arr.Skip(1).ToArray();
                            foreach (string itemarr in arr)
                            {
                                try
                                {
                                    // By Anil 
                                    if (itemarr.Contains("date"))
                                    {
                                        imageId = Utils.getBetween(itemarr, "\":\"", "\"");
                                        if (!string.IsNullOrEmpty(imageId))
                                        {
                                            //lstPhotoId.Add(imageId);
                                          //  lstPhotoId.Distinct();
                                            if (lstPhotoId.Count < ClGlobul.NumberofSnapsVideosToLike)
                                            {
                                                lstPhotoId.Add(imageId);
                                                lstPhotoId.Distinct();
                                              //  return lstPhotoId;
                                            }
                                            else
                                            {
                                                return lstPhotoId;
                                            }

                                            //imageId = "http://websta.me"+imageId;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                            #region Forpagination
                            if (pageSource.Contains("has_next_page\":true"))
                            {
                                while (value)
                                {
                                    if (pageSource.Contains("has_next_page\":true") && lstPhotoId.Count < ClGlobul.NumberofSnapsVideosToLike)
                                    {
                                        string IDD = Utils.getBetween(pageSource, "\"id\":\"", "\"");
                                        string code_ID = Utils.getBetween(pageSource, "end_cursor\":\"", "\"");
                                        string postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                        pageSource = objInstagramUser.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, "https://www.instagram.com/" + hashTag, token);
                                        string[] data1 = Regex.Split(pageSource, "code");
                                        foreach (string val in data1)
                                        {
                                            if (val.Contains("date"))
                                            {
                                                if (lstPhotoId.Count < ClGlobul.NumberofSnapsVideosToLike)
                                                {
                                                    string photo_codes = Utils.getBetween(val, "\":\"", "\"");
                                                    lstPhotoId.Add(photo_codes);
                                                }
                                                else
                                                {
                                                    return lstPhotoId;
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
                            #endregion
                        }

                    }
                }
               
            }
                
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
            return lstPhotoId;
        }
    

            
        


        #endregion


        #region StartDivide equally

        public void StartDivide()
        {
            try
            {
                if (Hash_Like == true)
                {
                    Divide_LikeHash();
                }
                if (Hash_comment == true)
                {
                    Divide_Comment();
                }
                if (Hash_Follow == true)
                {
                    Divide_Follow();
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }


        }


        #region HashLike By Divide Equal Rule

        public void Divide_LikeHash()
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            try
            {


                
                if (!string.IsNullOrEmpty(Hash_Like_Unlike_single))
                {
                    ClGlobul.HashLiker.Clear();
                    string s = Hash_Like_Unlike_single;
                    if (s.Contains(','))
                    {
                        try
                        {
                            string[] Data = s.Split(',');
                            foreach (var item in Data)
                            {
                                ClGlobul.HashLiker.Add(item);
                            }
                        }
                        catch { };

                    }
                    else
                    {
                        ClGlobul.HashLiker.Add(Hash_Like_Unlike_single);
                    }
                }
                if (ClGlobul.HashLiker.Count != 0)
                {
                    if (hashlike_no_photo != 0)  //txtNumberPicsVideosLike
                    {
                        ClGlobul.NumberofSnapsVideosToLike = hashlike_no_photo;
                        ClGlobul.SnapVideosCounter = hashlike_no_photo * ClGlobul.HashLiker.Count;
                    }
                    else
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Please enter the numbers of snaps or videos to like and continue. ]");
                        return;
                    }
                }


                List<string> lstHashTagUserIdLikeTemp = new List<string>();
                List<string> lstHashLikeTagUserLikeId = new List<string>();
                foreach (string hashLikerKeyword in ClGlobul.HashLiker)
                {

                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraping PhotoId with HashTag " + hashLikerKeyword + "]");
                    lstHashTagUserIdLikeTemp = GetPhotoId(hashLikerKeyword);
                    lstHashLikeTagUserLikeId.AddRange(lstHashTagUserIdLikeTemp);
                    //GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraped Users with HashTag " + hashLikerKeyword + "]");
                }
                NumberAcoount = IGGlobals.listAccounts.Count;

                Dictionary<string, string> dicAccHash = new Dictionary<string, string>();
                Queue<string> queAcc = new Queue<string>();
                foreach (string itemAcc in IGGlobals.listAccounts)
                {
                    queAcc.Enqueue(itemAcc);
                }

                int temp = hashlike_no_photo / NumberAcoount;
                int _checkTemp = temp;
                string _checkAcc = string.Empty;
                foreach (string itemHash in lstHashLikeTagUserLikeId)
                {
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

                foreach (var itemDic in dicAccHash)
                {

                    string[] arrHash = Regex.Split(itemDic.Value, ":");
                    Thread thrStart = new Thread(() => Operation(itemDic.Key, arrHash));
                    thrStart.Start();
                }



            }
            catch { }
        }
        public void Operation(string Acc, string[] Hash)
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
            string Result = string.Empty;
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
                                Result = LikeSnapsVideos(ref objInstagramUser, Photo_ID);
                            }
                            else
                            {
                                break;
                            }
                            if (Result.Contains("LIKED"))
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Liked " + Photo_ID + " Success ]");
                            }
                            if (Result.Contains("Already LIKED"))
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Allready Liked " + Photo_ID + " ]");
                            }
                            if (minDelayHash_comment != 0)
                            {
                                mindelay = minDelayHash_comment;
                            }
                            if (maxDelayHash_comment != 0)
                            {
                                maxdelay = maxDelayHash_comment;
                            }
                            Random obj_ran = new Random();
                            int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                            delay = obj_ran.Next(mindelay, maxdelay);
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);

                        }

                    }
                    GlobusLogHelper.log.Info("Done");
                }
            }
        }

        #endregion



        #region HashComment By Divide Equal Rule

        public void Divide_Comment()
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                if (string.IsNullOrEmpty(Hash_comment_Usernamepath) && string.IsNullOrEmpty(Hash_comment_Messagepath))
                {
                    if (!string.IsNullOrEmpty(Hash_comment_Usernamesingle) && !string.IsNullOrEmpty(Hash_comment_Messagesingle))
                    {
                        string s = Hash_comment_Usernamesingle;
                        string k = Hash_comment_Messagesingle;
                        if (s.Contains(','))
                        {
                            try
                            {
                                string[] Data = s.Split(',');

                                foreach (var item in Data)
                                {
                                    if (!ClGlobul.HashComment.Contains(item))
                                    {
                                        ClGlobul.HashComment.Add(item);
                                    }
                                }
                            }
                            catch { };
                        }
                        else
                        {
                            if (!ClGlobul.HashComment.Contains(Hash_comment_Usernamesingle))
                            {
                                ClGlobul.HashComment.Add(Hash_comment_Usernamesingle);
                            }
                        }
                        if (k.Contains(","))
                        {
                            try
                            {
                                string[] Data = k.Split(',');

                                foreach (var item in Data)
                                {
                                    if (!ClGlobul.HashCommentMessage.Contains(item))
                                    {
                                        ClGlobul.HashCommentMessage.Add(item);
                                    }
                                }
                            }
                            catch { };
                        }
                        else
                        {
                            if (!ClGlobul.HashCommentMessage.Contains(Hash_comment_Messagesingle))
                            {
                                ClGlobul.HashCommentMessage.Add(Hash_comment_Messagesingle);
                            }
                        }
                    }
                }
                if (ClGlobul.HashComment.Count != 0)
                {
                    if (Number_Hash_photocomment != 0)
                    {
                        ClGlobul.NumberofSnapsVideosToComment = Number_Hash_photocomment * IGGlobals.listAccounts.Count * ClGlobul.HashComment.Count;
                        ClGlobul.SnapVideosCounterComment = Number_Hash_photocomment; //* ClGlobul.HashComment.Count;
                    }
                    else
                    {

                        //MessageBox.Show("Please enter the numbers of snaps or videos to like and continue.");
                        //AddToHashLoggerDAta("[ " + DateTime.Now + "] " + "[ Please enter the numbers of snaps or videos to like and continue. ]");
                        return;
                    }
                }

                Start_divide_HashComment();

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
            }

        }

        public void Start_divide_HashComment()
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            lstHashTagCommentUserId.Clear();
            foreach (string item in ClGlobul.HashComment)
            {
                lstHashTagUserIdCommentTemp = GetPhotoId1(item);
                lstHashTagCommentUserId.AddRange(lstHashTagUserIdCommentTemp);

            }

            NumberAcoount = IGGlobals.listAccounts.Count;

            Dictionary<string, string> dicAccHash = new Dictionary<string, string>();
            Queue<string> queAcc = new Queue<string>();
            foreach (string itemAcc in IGGlobals.listAccounts)
            {
                queAcc.Enqueue(itemAcc);
            }

            int temp = Number_Hash_photocomment / NumberAcoount;
            int _checkTemp = temp;
            string _checkAcc = string.Empty;
            foreach (string itemHash in lstHashTagCommentUserId)
            {
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

            foreach (var itemDic in dicAccHash)
            {
                string[] arrHash = Regex.Split(itemDic.Value, ":");
                Thread thrStart = new Thread(() => CommentOperation(itemDic.Key, arrHash));
                thrStart.Start();
            }
        }


        public void CommentOperation(string Acc, string[] Hash)
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
                                CommentOnSnapsVideos(ref objInstagramUser, Photo_ID);
                            }
                            else
                            {
                                break;
                            }
                            if (minDelayHash_comment != 0)
                            {
                                mindelay = minDelayHash_comment;
                            }
                            if (maxDelayHash_comment != 0)
                            {
                                maxdelay = maxDelayHash_comment;
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


        #region HashFollow By Divide Equal Rule


        public void Divide_Follow()
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                if (!string.IsNullOrEmpty(Hash_Follower_single))
                {
                    ClGlobul.HashFollower.Clear();
                    string s = Hash_Follower_single;
                    if (s.Contains(','))
                    {
                        try
                        {
                            string[] Data = s.Split(',');

                            foreach (var item in Data)
                            {
                                ClGlobul.HashFollower.Add(item);
                            }
                        }
                        catch { };
                    }
                    else
                    {
                        ClGlobul.HashFollower.Add(Hash_Follower_single);
                    }
                }

                if (ClGlobul.HashFollower.Count != 0)
                {
                    if (hashFollower_Number != 0)
                    {
                        ClGlobul.NumberOfProfilesToFollow = hashFollower_Number;
                        ClGlobul.SnapVideosCounterfollow = hashFollower_Number * ClGlobul.HashFollower.Count;

                    }
                    else
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Please enter the numbers of profiles to follow and continue. ]");
                        return;
                    }

                }

                List<string> lstHashTagUserIdTemp = new List<string>();
                List<string> lstHashTagUserId = new List<string>();


                foreach (string hashKeyword in ClGlobul.HashFollower)
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraping Users with HashTag " + hashKeyword + "]");
                   // lstHashTagUserIdTemp = GetUser(hashKeyword);
                    lstHashTagUserId.AddRange(lstHashTagUserIdTemp);
                }

                NumberAcoount = IGGlobals.listAccounts.Count;

                Dictionary<string, string> dicAccHash = new Dictionary<string, string>();
                Queue<string> queAcc = new Queue<string>();
                foreach (string itemAcc in IGGlobals.listAccounts)
                {
                    queAcc.Enqueue(itemAcc);
                }

                int temp = hashFollower_Number / NumberAcoount;
                int _checkTemp = temp;
                string _checkAcc = string.Empty;
                foreach (string itemHash in lstHashTagUserId)
                {
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

                foreach (var itemDic in dicAccHash)
                {
                    string[] arrHash = Regex.Split(itemDic.Value, ":");
                    Thread thrStart = new Thread(() => FollowOperation(itemDic.Key, arrHash));
                    thrStart.Start();
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
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
                                FollowUrls(ref objInstagramUser, Photo_ID);
                            }
                            else
                            {
                                break;
                            }

                            if (minDelayHash_comment != 0)
                            {
                                mindelay = minDelayHash_comment;
                            }
                            if (maxDelayHash_comment != 0)
                            {
                                maxdelay = maxDelayHash_comment;
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


        #endregion


        #region StartDivide By User

        public void StartDivideUser()
        {
            try
            {
                try
                {
                    lstThreadsHash_comment.Add(Thread.CurrentThread);
                    lstThreadsHash_comment.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
                try
                {
                    if (Hash_comment == true)
                    {
                        StartDivide_Comment();
                    }
                    if (Hash_Like == true)
                    {
                        Start_LikeDividUser();
                    }
                    if (Hash_Follow == true)
                    {
                        Start_DivideByUser_Follow();
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }



            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }

        public void Start_LikeDividUser()
        {
            try
            {
                try
                {


                    
                    if (!string.IsNullOrEmpty(Hash_Like_Unlike_single))
                    {
                        ClGlobul.HashLiker.Clear();
                        string s = Hash_Like_Unlike_single;
                        if (s.Contains(','))
                        {
                            try
                            {
                                string[] Data = s.Split(',');
                                foreach (var item in Data)
                                {
                                    ClGlobul.HashLiker.Add(item);
                                }
                            }
                            catch { };

                        }
                        else
                        {
                            ClGlobul.HashLiker.Add(Hash_Like_Unlike_single);
                        }
                    }
                    if (ClGlobul.HashLiker.Count != 0)
                    {
                        if (hashlike_no_photo != 0)  //txtNumberPicsVideosLike
                        {
                            ClGlobul.NumberofSnapsVideosToLike = hashlike_no_photo;
                            ClGlobul.SnapVideosCounter = hashlike_no_photo * ClGlobul.HashLiker.Count;
                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Please enter the numbers of snaps or videos to like and continue. ]");
                            return;
                        }
                    }
                    List<string> lstHashTagUserIdLikeTemp = new List<string>();
                    List<string> lstHashLikeTagUserLikeId = new List<string>();
                    foreach (string hashLikerKeyword in ClGlobul.HashLiker)
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraping PhotoId with HashTag " + hashLikerKeyword + "]");
                        lstHashTagUserIdLikeTemp = GetPhotoId(hashLikerKeyword);
                        lstHashLikeTagUserLikeId.AddRange(lstHashTagUserIdLikeTemp);
                        //GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraped Users with HashTag " + hashLikerKeyword + "]");
                    }


                    NumberAcoount = IGGlobals.listAccounts.Count;
                    int temp = Number_Hash_photocomment / NumberAcoount;
                    int _checkTemp = temp;
                    string _checkAcc = string.Empty;
                    int count = 0;
                    Dictionary<string, string> dicAccHash = new Dictionary<string, string>();
                    Queue<string> queAcc = new Queue<string>();
                    Queue<string> queList = new Queue<string>();
                    foreach (string List_ID in lstHashLikeTagUserLikeId)
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
                        foreach (string item_Id in lstHashLikeTagUserLikeId)
                        {
                            //if (queAcc.Count != 0)
                            //{
                            string Item_listid = queList.Dequeue();
                            if (count < Divide_data_NoUser)
                            {
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
                            //   }
                            //else
                            //{
                            //    foreach (string itemAccc in IGGlobals.listAccounts)
                            //    {
                            //        queAcc.Enqueue(itemAccc);
                            //    }
                            //}
                        }
                    }

                    foreach (var itemDic in dicAccHash)
                    {
                        string[] arrHash = Regex.Split(itemDic.Value, ":");
                        Thread thrStart = new Thread(() => Operation(itemDic.Key, arrHash));
                        thrStart.Start();
                    }






                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }



        public void StartDivide_Comment()
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }

            try
            {
                if (string.IsNullOrEmpty(Hash_comment_Usernamepath) && string.IsNullOrEmpty(Hash_comment_Messagepath))
                {
                    if (!string.IsNullOrEmpty(Hash_comment_Usernamesingle) && !string.IsNullOrEmpty(Hash_comment_Messagesingle))
                    {
                        string s = Hash_comment_Usernamesingle;
                        string k = Hash_comment_Messagesingle;
                        if (s.Contains(','))
                        {
                            try
                            {
                                string[] Data = s.Split(',');

                                foreach (var item in Data)
                                {
                                    if (!ClGlobul.HashComment.Contains(item))
                                    {
                                        ClGlobul.HashComment.Add(item);
                                    }
                                }
                            }
                            catch { };
                        }
                        else
                        {
                            if (!ClGlobul.HashComment.Contains(Hash_comment_Usernamesingle))
                            {
                                ClGlobul.HashComment.Add(Hash_comment_Usernamesingle);
                            }
                        }
                        if (k.Contains(","))
                        {
                            try
                            {
                                string[] Data = k.Split(',');

                                foreach (var item in Data)
                                {
                                    if (!ClGlobul.HashCommentMessage.Contains(item))
                                    {
                                        ClGlobul.HashCommentMessage.Add(item);
                                    }
                                }
                            }
                            catch { };
                        }
                        else
                        {
                            if (!ClGlobul.HashCommentMessage.Contains(Hash_comment_Messagesingle))
                            {
                                ClGlobul.HashCommentMessage.Add(Hash_comment_Messagesingle);
                            }
                        }
                    }
                }
                if (ClGlobul.HashComment.Count != 0)
                {
                    if (Number_Hash_photocomment != 0)
                    {
                        ClGlobul.NumberofSnapsVideosToComment = Number_Hash_photocomment * IGGlobals.listAccounts.Count * ClGlobul.HashComment.Count;
                        ClGlobul.SnapVideosCounterComment = Number_Hash_photocomment; //* ClGlobul.HashComment.Count;
                    }
                    else
                    {

                        //MessageBox.Show("Please enter the numbers of snaps or videos to like and continue.");
                        //AddToHashLoggerDAta("[ " + DateTime.Now + "] " + "[ Please enter the numbers of snaps or videos to like and continue. ]");
                        return;
                    }
                }

                   Start_DivideByUser();

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }


        public void Start_DivideByUser()
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            lstHashTagCommentUserId.Clear();
            foreach (string item in ClGlobul.HashComment)
            {
                lstHashTagUserIdCommentTemp = GetPhotoId1(item);
                lstHashTagCommentUserId.AddRange(lstHashTagUserIdCommentTemp);

            }

            NumberAcoount = IGGlobals.listAccounts.Count;
            int temp = Number_Hash_photocomment / NumberAcoount;
            int _checkTemp = temp;
            string _checkAcc = string.Empty;
            int count = 0;
            Dictionary<string, string> dicAccHash = new Dictionary<string, string>();
            Queue<string> queAcc = new Queue<string>();
            Queue<string> queList = new Queue<string>();
            foreach (string List_ID in lstHashTagCommentUserId)
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
                foreach (string item_Id in lstHashTagCommentUserId)
                {
                    
                    string Item_listid = queList.Dequeue();
                    if (count < Divide_data_NoUser)
                    {
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
                Thread thrStart = new Thread(() => CommentOperation(itemDic.Key, arrHash));
                thrStart.Start();
            }
        }




        public void Start_DivideByUser_Follow()
        {
            try
            {
                lstThreadsHash_comment.Add(Thread.CurrentThread);
                lstThreadsHash_comment.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
            try
            {
                if (!string.IsNullOrEmpty(Hash_Follower_single))
                {
                    ClGlobul.HashFollower.Clear();
                    string s = Hash_Follower_single;
                    if (s.Contains(','))
                    {
                        try
                        {
                            string[] Data = s.Split(',');

                            foreach (var item in Data)
                            {
                                ClGlobul.HashFollower.Add(item);
                            }
                        }
                        catch { };
                    }
                    else
                    {
                        ClGlobul.HashFollower.Add(Hash_Follower_single);
                    }
                }

                if (ClGlobul.HashFollower.Count != 0)
                {
                    if (hashFollower_Number != 0)
                    {
                        ClGlobul.NumberOfProfilesToFollow = hashFollower_Number;
                        ClGlobul.SnapVideosCounterfollow = hashFollower_Number * ClGlobul.HashFollower.Count;

                    }
                    else
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Please enter the numbers of profiles to follow and continue. ]");
                        return;
                    }

                }

                List<string> lstHashTagUserIdTemp = new List<string>();
                List<string> lstHashTagUserId = new List<string>();


                foreach (string hashKeyword in ClGlobul.HashFollower)
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraping Users with HashTag " + hashKeyword + "]");
                   // lstHashTagUserIdTemp = GetUser(hashKeyword);
                    lstHashTagUserId.AddRange(lstHashTagUserIdTemp);
                }


                NumberAcoount = IGGlobals.listAccounts.Count;
                int temp = Number_Hash_photocomment / NumberAcoount;
                int _checkTemp = temp;
                string _checkAcc = string.Empty;
                int count = 0;
                Dictionary<string, string> dicAccHash = new Dictionary<string, string>();
                Queue<string> queAcc = new Queue<string>();
                Queue<string> queList = new Queue<string>();
                foreach (string List_ID in lstHashTagUserId)
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
                    foreach (string item_Id in lstHashTagUserId)
                    {
                       
                        string Item_listid = queList.Dequeue();
                        if (count < Divide_data_NoUser)
                        {
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

        #endregion


        


    }
}