using Accounts;
using BaseLib;
using BaseLibID;
using Comment;
using Globussoft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Scraping
{

    public delegate void addComboDelegate();

    public class ScrapingManager
    {

        public static addComboDelegate objaddComboDelegate;

        #region Global Data
        public static bool Userphotoliker_userscrape = false;

        public bool value = false;
        public static string UserScrape_single = string.Empty;
        public static string UserScape_Path = string.Empty;
        public static int No_UserCount = 0;
        public bool ScrapFollower = false;
        static readonly Object _lockObject = new Object();
        readonly object lockrThreadControlleScrapeUser = new object();
        readonly object lockrThreadControlleScrapeFollower = new object();
        public bool isStopScrapeUser = false;
        public bool isStopScrapeFollower = false;
        public bool isStop_ScrapeImage = false;
        public List<Thread> lstThreads_ScarpeImage = new List<Thread>();
        public bool ImageScarpe = false;
        public static int Number_ScrapeImage = 0;
        public static int minDelay_ScrapeImage = 0;
        public static int maxDelay_ScrapeImage = 0;
        public static string ScrapeImage_single = string.Empty;
        public static string ScrapeImage_Multiple = string.Empty;
        public static int Nothread_ScarpeImage = 0;
        public bool useOriginalScrapeUser = true;
        int countThreadControllerScrapeUser = 0;
        int countThreadControllerScrapeFollower = 0;
        public static int TotalNoOfScrapeUserCounter = 0;
        public static int messageCountScrapeUser = 0;
        int countScrapeUser = 1;
        public static Dictionary<string, string> Duplicatedatascrape = new Dictionary<string, string>();
        public static string User_key = string.Empty;

        public List<Thread> lstThreadsScrapeUser = new List<Thread>();
        public List<Thread> lstThreadsScrapeFollower = new List<Thread>();
        public static string Username_ScrapFollower = string.Empty;
        public List<string> lstCommentPostURLsScrapeUser = new List<string>();
        public List<string> lstScrapeUserURLsTitles = new List<string>();
        public List<string> lstCommentPostURLsScrapeFollower = new List<string>();
        //public List<string> lstScrapeUserURLsTitles = new List<string>();
        public static List<string> selected_Account = new List<string>();
        public static int minDelayScrapeUser = 10;
        public static int maxDelayScrapeUser = 20;
        public static string status = string.Empty;
        public GlobusHttpHelper httpHelper = new GlobusHttpHelper();
        public ChilkatHttpHelpr chilkathttpHelper = new ChilkatHttpHelpr();
        public static int mindelay = 0;
        public static int maxdelay = 0;
        public static bool stopScrapBool = false;
        public static bool UserScraper_UserByKeyword = false;
        public static int Nothread_ScrapeUser = 0;
        public static bool UserScraper_Username = false;
        public static int No_UserCount_keyword = 0;
        public static int Mindelay_ScarpeFollower = 0;
        public static int Maxdelay_ScarpeFollower = 0;
        public static int No_ThreadFollower = 0;
        public static int No_ScrapeFollowerUser = 0;
        private const string CSVHeader = "HashTag,UserName,UserLink";
        private string CSVPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Gram Dominator\\scrapedUserDetails.csv";
        public static List<string> User_Bykey = new List<string>();
        Dictionary<string, string> duplicatlink = new Dictionary<string, string>();
        public static string CSVPath_followerScrape = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator\\Scrape_Followers\\";
        private const string CSVHeaderr = "Username, Name, Follower count, Following count, Picture count, Day, Month, Year";
        Dictionary<string, string> postDataDictionary = new Dictionary<string, string>();
        private const string CSVHeader_Imageurl = "HashTag,Image Link,Profile Url,Full Name";
        private string CSVPath_Imageurl = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Gram Dominator\\ImageDetails.csv";
        int count_scrapeImage = 0;


        public static bool uploadpic = false;

        #endregion



        public int NoOfThreadsLikePosterScarpeUser
        {
            get;
            set;
        }
        public int NoOfThreadsScarpeFollower
        {
            get;
            set;
        }

        public void StartLikePoster()
        {
            countThreadControllerScrapeUser = 0;
            try
            {
                int numberOfAccountPatch = 25;

                if (NoOfThreadsLikePosterScarpeUser > 0)
                {
                    numberOfAccountPatch = NoOfThreadsLikePosterScarpeUser;
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
                                lock (lockrThreadControlleScrapeUser)
                                {
                                    try
                                    {
                                        if (countThreadControllerScrapeUser >= listAccounts.Count)
                                        {
                                            Monitor.Wait(lockrThreadControlleScrapeUser);
                                        }

                                        string acc = account.Remove(account.IndexOf(':'));

                                        //Run a separate thread for each account
                                        InstagramUser item = null;
                                        IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                                        if (item != null)
                                        {
                                            Thread profilerThread = new Thread(StartMultiThreadsScrapeModule);
                                            profilerThread.Name = "workerThread_Profiler_" + acc;
                                            profilerThread.IsBackground = true;

                                            profilerThread.Start(new object[] { item });

                                            countThreadControllerScrapeUser++;
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

        public void StartMultiThreadsScrapeModule(object parameters)
        {
            try
            {
                if (!isStopScrapeUser)
                {
                    try
                    {
                        lstThreadsScrapeUser.Add(Thread.CurrentThread);
                        lstThreadsScrapeUser.Distinct();
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

                            StartActionScrapeModule(ref objFacebookUser);

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
                        lock (lockrThreadControlleScrapeUser)
                        {
                            countThreadControllerScrapeUser--;
                            Monitor.Pulse(lockrThreadControlleScrapeUser);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }

        private void StartActionScrapeModule(ref InstagramUser fbUser)
        {

            try
            {
                lstThreadsScrapeUser.Add(Thread.CurrentThread);
                lstThreadsScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                if (Userphotoliker_userscrape == true)
                {
                   // Scrape_liker(ref fbUser);
                }


                if (UserScraper_Username == true)
                {
                    Start_scraping(ref fbUser);
                }
                if (UserScraper_UserByKeyword == true)
                {
                    scrapeUesr_Key(User_key, ref fbUser);
                    // tried(ref fbUser);
                }
                if (ScrapFollower == true)
                {
                    StartFollowerScrape(ref fbUser);
                }
                if (ImageScarpe == true)
                {
                    Start_ImageUrl(ref fbUser);
                }



            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }



        #region Scrape Image and Download

        public void Start_ImageUrl(ref InstagramUser obj_DwonloadImange)
        {
            try
            {
                lstThreadsScrapeUser.Add(Thread.CurrentThread);
                lstThreadsScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                object obj_Scrape = obj_DwonloadImange;

                string res_secondURL = obj_DwonloadImange.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                if (string.IsNullOrEmpty(ScrapeImage_Multiple))
                {
                    if (!string.IsNullOrEmpty(ScrapeImage_single))
                    {
                        ClGlobul.ImageTagForScrap.Clear();

                        string s = ScrapeImage_single;

                        if (s.Contains(','))
                        {
                            string[] Data = s.Split(',');

                            foreach (var item in Data)
                            {
                                ClGlobul.ImageTagForScrap.Add(item);
                            }
                        }
                        else
                        {
                            ClGlobul.ImageTagForScrap.Add(ScrapeImage_single);
                        }
                    }
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Scraping Started for Image Urls ]");
                try
                {

                    Thread profilerThread = new Thread(() => DownloadingImage(obj_Scrape));
                    profilerThread.Name = obj_DwonloadImange.username;
                    profilerThread.Start();

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
        public static List<Thread> lstDownloadingImageProcessRunning = new List<Thread>();

        public void DownloadingImage(object obj_Scrape)
        {
            try
            {
                lstThreadsScrapeUser.Add(Thread.CurrentThread);
                lstThreadsScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
                lstDownloadingImageProcessRunning.Add(Thread.CurrentThread);
                if (lstDownloadingImageProcessRunning.Count() > 1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            InstagramUser obj_Insta = (InstagramUser)obj_Scrape;
            try
            {
                try
                {

                    string res_secondURL = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                }
                catch { };


            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            try
            {
                int minDelayTime = minDelay_ScrapeImage;
                int maxDelayTime = maxDelay_ScrapeImage;
                int Delay = RandomNumberGenerator.GenerateRandom(minDelayTime, maxDelayTime);

                foreach (string itemImageTag in ClGlobul.ImageTagForScrap)
                {
                    startDownloadingImage(itemImageTag, Delay, ref obj_Insta);
                    Thread.Sleep(5000);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Process Completed ]");

            }
            catch { }
        }

        public void startDownloadingImage(string itemImageTag, int delay, ref InstagramUser obj_Insta)
        {
            try
            {
                lstThreadsScrapeUser.Add(Thread.CurrentThread);
                lstThreadsScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string pageSource = string.Empty;
            string input_itemImage = string.Empty;
            List<string> lstCountScrape = new List<string>();
            if (isStop_ScrapeImage) return;
            //try
            //{
            GlobusHttpHelper _GlobusHttpHelper = new GlobusHttpHelper();
            try
            {
                pageSource = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/"), "");
            }
            catch { }
            if (!string.IsNullOrEmpty(pageSource))
            {
                if (itemImageTag.Contains("#"))
                {
                    itemImageTag = itemImageTag.Replace("#", "");
                    input_itemImage = "#" + itemImageTag;
                }
                else
                {
                    input_itemImage = itemImageTag;
                }
                // string url = mainUrl + "tag/" + itemImageTag;
                string url = string.Empty;
                string response = string.Empty;
                // GlobusHttpHelper _GlobusHttpHelper = new GlobusHttpHelper();
                string Home_icon_Url = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                string PPagesource = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                string responce_icon = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");
                if (!string.IsNullOrEmpty(responce_icon))
                {

                    url = "http://iconosquare.com/viewer.php#/search/" + itemImageTag;

                }

                try
                {
                    pageSource = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");
                    string referer = "http://iconosquare.com/viewer.php";
                    string viewer_responce = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php "), "");
                    string crs_token = Utils.getBetween(viewer_responce, " <div id=\"accesstoken\" style=\"display:none;\">", "</div>");
                    response = _GlobusHttpHelper.getHtmlfromUrl(new Uri(url), "");

                    string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + crs_token + "&q=" + itemImageTag;
                    string respon_scrapeuser = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), referer);
                    string[] data_divided = Regex.Split(respon_scrapeuser, "username");
                    if (data_divided.Count() == 1)
                    {
                        return;
                    }
                    else
                    {
                        foreach (string itemmm in data_divided)
                        {
                            if (itemmm.Contains("profile_picture"))
                            {
                                string websiteLink = Utils.getBetween(itemmm, "\":\"", "\"");
                                websiteLink = "https://www.instagram.com/" + websiteLink + "/";
                                string imageLink = string.Empty;
                                try
                                {
                                    imageLink = Utils.getBetween(itemmm, "\"https", "\"");
                                    imageLink = "https" + imageLink.Replace("\\", "");
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Info("Image is Not There===>" + websiteLink);
                                }
                                string user_responce = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri(websiteLink), "");
                                string FullName = Utils.getBetween(user_responce, "<title>", "(").Replace("\n", "");
                                if (string.IsNullOrEmpty(FullName))
                                {
                                    FullName = "Not Persent";
                                }



                                try
                                {
                                    if (!string.IsNullOrEmpty(imageLink))
                                    {
                                        // duplicatlink.Add(imageLink);
                                        string CSVData = input_itemImage.Replace(",", string.Empty) + "," + imageLink.Replace(",", string.Empty) + "," + websiteLink.Replace(",", string.Empty) + "," + FullName.Replace(",", string.Empty);
                                        DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,UserName,Status) values('" + "ScrapeImageUrl" + "','" + obj_Insta.username + "','" + DateTime.Now + "','" + FullName + "','" + "Success" + "')", "tbl_AccountReport");
                                        //string CSVData = websiteLink.Replace(",", string.Empty) + "," + imageLink.Replace(",", string.Empty) + "," + imageid.Replace(",", string.Empty) + "," + FullName.Replace(",", string.Empty);
                                        GlobusFileHelper.ExportDataCSVFile(CSVHeader_Imageurl, CSVData, CSVPath_Imageurl);
                                        if (lstCountScrape.Count >= Number_ScrapeImage)
                                        {
                                            return;
                                        }
                                    }
                                }
                                catch { }










                                lstCountScrape.Add(imageLink);
                                lstCountScrape = lstCountScrape.Distinct().ToList();

                                if (isStop_ScrapeImage) return;

                                if (isStop_ScrapeImage) return;
                                try
                                {
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + websiteLink + " ]");

                                    if (minDelay_ScrapeImage != 0)
                                    {
                                        mindelay = minDelay_ScrapeImage;
                                    }
                                    if (maxDelay_ScrapeImage != 0)
                                    {
                                        maxdelay = maxDelay_ScrapeImage;
                                    }

                                    Random obj_rn = new Random();
                                    int delay2 = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                    delay2 = obj_rn.Next(mindelay, maxdelay);
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay2 + " Seconds ]");
                                    Thread.Sleep(delay2 * 1000);


                                    //objclasssforlogger.AddToImageTagLogger("[ " + DateTime.Now + " ] => [ Delay for " + delay + " seconds ]");
                                    //Thread.Sleep(delay * 1000);

                                    if (lstCountScrape.Count >= Number_ScrapeImage)
                                    {
                                        return;
                                    }
                                }
                                catch { };
                            }
                        }
                    }
                    if (lstCountScrape.Count >= Number_ScrapeImage)
                    {
                        return;
                    }

                }
                catch { };


            }
            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Process Completed ]");

        }







        #endregion

        #region Scrap Follower

        public void StartScrapFollower()
        {

            try
            {
                countThreadControllerScrapeFollower = 0;
                try
                {
                    int numberOfAccountPatch = 25;

                    if (NoOfThreadsScarpeFollower > 0)
                    {
                        numberOfAccountPatch = NoOfThreadsScarpeFollower;
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
                                    string[] data = Regex.Split(account, ":");
                                    string Accout = data[0];

                                    if (selected_Account.Contains(Accout))
                                    //if (Accout.Contains(selected_Account))
                                    {
                                        lock (lockrThreadControlleScrapeFollower)
                                        {
                                            try
                                            {
                                                if (countThreadControllerScrapeFollower >= listAccounts.Count)
                                                {
                                                    Monitor.Wait(lockrThreadControlleScrapeFollower);
                                                }

                                                string acc = account.Remove(account.IndexOf(':'));

                                                //Run a separate thread for each account
                                                InstagramUser item = null;

                                                IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                                                if (item != null)
                                                {
                                                    Thread profilerThread = new Thread(StartMultiThreadsScrapeFollower);
                                                    profilerThread.Name = "workerThread_Profiler_" + acc;
                                                    profilerThread.IsBackground = true;

                                                    profilerThread.Start(new object[] { item, });
                                                    //DS = item.ds;
                                                    countThreadControllerScrapeUser++;
                                                }
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
                            }
                        }
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

        public void StartMultiThreadsScrapeFollower(object parameters)
        {
            try
            {
                if (!isStopScrapeFollower)
                {
                    try
                    {
                        lstThreadsScrapeFollower.Add(Thread.CurrentThread);
                        lstThreadsScrapeFollower.Distinct();
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

                            StartActionScrapeModule(ref objFacebookUser);

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
                        lock (lockrThreadControlleScrapeUser)
                        {
                            countThreadControllerScrapeUser--;
                            Monitor.Pulse(lockrThreadControlleScrapeUser);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }
        List<string> follower_list = new List<string>();
        public void StartFollowerScrape(ref InstagramUser obj_folowerscrape)
        {
            try
            {
                lstThreadsScrapeFollower.Add(Thread.CurrentThread);
                lstThreadsScrapeFollower.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string rawUsernameID = string.Empty;
            string usernamePgSource = string.Empty;

            string res_secondURL = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
            try
            {
                foreach (string itemusername in ClGlobul.listOfScrapeFollowerUserame)
                {
                    Username_ScrapFollower = itemusername;

                    ClGlobul.switchAccount = false;
                    string Home_icon_Url = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                    string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                    string PPagesource = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                    string crstoken = Utils.getBetween(PPagesource, "<div id=\"accesstoken\"", "/div>");
                    string CRS_Token = Utils.getBetween(crstoken, "\">", "<");
                    string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + CRS_Token + "&q=" + Username_ScrapFollower;
                    string respo_profile = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), "http://iconosquare.com/viewer.php");
                    string profile_ID = Utils.getBetween(respo_profile, "\"id\":\"", "\"");
                    string post_Url = "http://iconosquare.com/rqig.php?e=/users/" + profile_ID + "/follows&a=ico2&t=" + CRS_Token + "&count=20";
                    string Profile_responce = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php#/user/" + profile_ID), "http://iconosquare.com/viewer.php");
                    string list_follower = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php#/followers/" + profile_ID), "");
                    postdata = "http://iconosquare.com/rqig.php?e=/users/" + profile_ID + "/followed-by&a=ico2&t=" + CRS_Token + "&count=20";
                    string follow_respo = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), "http://iconosquare.com/viewer.php");
                    string[] data = Regex.Split(follow_respo, "username");
                    foreach (string var in data)
                    {
                        if (var.Contains("profile_picture"))
                        {
                            if (No_ScrapeFollowerUser > follower_list.Count())
                            {
                                string user_name = Utils.getBetween(var, "\":\"", "\"");
                                follower_list.Add(user_name);
                                GlobusLogHelper.log.Info("Scraped===>" + user_name);
                                try
                                {
                                    DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + user_name + "')", "ScrapedUsername");
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error:" + ex.StackTrace);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (follow_respo.Contains("next_url"))
                    {
                        value = true;
                        while (value)
                        {
                            if (follow_respo.Contains("next_url") && No_ScrapeFollowerUser > follower_list.Count())
                            {
                                string next_pageurl_token = Utils.getBetween(follow_respo, "next_cursor\":\"", "\"},");
                                string page_Url = postdata + "&cursor=" + next_pageurl_token;
                                follow_respo = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(page_Url), "http://iconosquare.com/viewer.php");
                                string[] data1 = Regex.Split(follow_respo, "username");
                                foreach (string item in data1)
                                {
                                    if (No_ScrapeFollowerUser > follower_list.Count())
                                    {
                                        if (item.Contains("profile_picture"))
                                        {
                                            string username = Utils.getBetween(item, ":\"", "\"");
                                            follower_list.Add(username);
                                            GlobusLogHelper.log.Info("Scraped===>" + username);

                                            try
                                            {
                                                DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + username + "')", "ScrapedUsername");
                                            }
                                            catch (Exception ex)
                                            {

                                            }
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
            
                foreach (string followerName in follower_list)
                {
                    string followerUrl = "https://www.instagram.com/" + followerName + "/";
                    string usernameFollowerUrl = "https://www.instagram.com/" + Username_ScrapFollower + "/";
                    ScrapeFollowerDetails(ref obj_folowerscrape, followerUrl, usernameFollowerUrl);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }





        public void ScrapeFollowerDetails(ref InstagramUser accountManager, string followerUrl, string usernameFollowerUrl)
        {
            try
            {

                try
                {
                    lstThreadsScrapeFollower.Add(Thread.CurrentThread);
                    lstThreadsScrapeFollower.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }

                Thread.Sleep(1000);
                string followerPageSource = accountManager.globusHttpHelper.getHtmlfromUrl1(new Uri(followerUrl), usernameFollowerUrl);
                if (string.IsNullOrEmpty(followerPageSource))
                {
                    Thread.Sleep(10000);
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[ Pagesource is empty. Delaying for 10 seconds. ]");
                    followerPageSource = accountManager.globusHttpHelper.getHtmlfromUrl1(new Uri(followerUrl), usernameFollowerUrl);
                    if (string.IsNullOrEmpty(followerPageSource))
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[ Pagesource is still empty. Please restart the software. The process will resume. ]");
                    }
                }

                string followerUsername = Utils.getBetween(followerUrl, ".com/", "/");

                if (followerPageSource.Contains("This user is private."))
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[ Data can not be scraped of the  private user with user name : " + followerUsername + ". ]");
                    return;
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[ Scraping detail from profile with username-" + followerUsername + " ]");
                //  string rawFollowerInfo = Utils.getBetween(followerPageSource, "<div class=\"userinfo\">", "</div>");
                // string[] infoSplit = Regex.Split(rawFollowerInfo, "<li>");

                string subFollowingCount = Utils.getBetween(followerPageSource, "\"follows\":{\"count\":", "}");
                string subFollowerCount = Utils.getBetween(followerPageSource, "followed_by\":{\"count\":", "}");


                string countAndLatestPostUrl = GetPictureCountAndLatestSnapUrlfollower(ref accountManager, followerPageSource);
                if (countAndLatestPostUrl == "stop")
                {
                    return;
                }
                string[] count_SPlit_Url = Regex.Split(countAndLatestPostUrl, "splitHere");
                string latestPostUrl = count_SPlit_Url[0].Trim();
                string latestPostPageResponse = string.Empty;
                double uTimestamp = 0;
                string date = string.Empty;
                string day = string.Empty;
                string month = string.Empty;
                string year = string.Empty;
                string pictureCount = Utils.getBetween(followerPageSource, "media\":{\"count\":", ",").Replace("}", "");

                try
                {
                    latestPostPageResponse = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(latestPostUrl), followerUrl);
                }
                catch { };
                try
                {
                    uTimestamp = Convert.ToDouble(Utils.getBetween(latestPostPageResponse, "date\":", ","));
                }
                catch { };
                DateTime rawDate = UnixTimeStampToDateTime(uTimestamp);
                try
                {
                    date = rawDate.ToString("dd-MM-yyyy");
                }
                catch { };

                string[] date_split = Regex.Split(date, "-");
                try
                {
                    day = date_split[0].ToString();
                }
                catch { };
                try
                {
                    month = date_split[1].ToString();
                }
                catch { };
                try
                {


                    year = date_split[2].ToString();
                }
                catch { };



                DataBaseHandler.InsertQuery("insert into tb_scrape_follower(username, name, follower_count, following_count, picture_count, day, month, year) values('" + Username_ScrapFollower + "','" + followerUsername + "','" + subFollowerCount + "','" + subFollowingCount + "','" + pictureCount + "','" + day + "','" + month + "','" + year + "')", "tb_scrape_follower");
                DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,FollowerName, Status) values('" + "ScrapeFollower_Module" + "','" + accountManager.username + "','" + DateTime.Now + "','" + followerUsername + "','" + "Scraped" + "')", "tbl_AccountReport");
                GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[ Scraped detail saved of username-" + followerUsername + " ]");
                objaddComboDelegate();
                if (!string.IsNullOrEmpty(followerUsername))
                {
                    #region CSV Write
                    try
                    {
                        if (!string.IsNullOrEmpty(followerUsername))
                        {
                            postDataDictionary.Add(followerUsername, followerUsername);
                            string CSVData = Username_ScrapFollower.Replace(",", string.Empty) + "," + followerUsername.Replace(",", string.Empty) + "," + subFollowerCount.Replace(",", string.Empty) + "," + subFollowingCount.Replace(",", string.Empty) + "," + pictureCount.Replace(",", string.Empty) + "," + day.Replace(",", string.Empty) + "," + month.Replace(",", string.Empty) + "," + year.Replace(",", string.Empty);
                            GlobusFileHelper.ExportDataCSVFile(CSVHeaderr, CSVData, CSVPath_followerScrape + Username_ScrapFollower + ".csv");
                        }
                    }
                    catch { }
                    try
                    {

                        GlobusLogHelper.log.Info("[" + followerUsername + "," + "followerUsername:" + "," + subFollowerCount + "," + "rawFollowerInfo:" + "," + subFollowingCount + "," + "subFollowingCount:" + "," + pictureCount + "," + "pictureCount" + "," + day + "," + "day" + "," + month + "," + "month" + "," + year + "," + "year]");

                    }
                    catch { };

                    #endregion


                    if (Mindelay_ScarpeFollower != 0)
                    {
                        mindelay = Mindelay_ScarpeFollower;
                    }
                    if (Maxdelay_ScarpeFollower != 0)
                    {
                        maxdelay = Maxdelay_ScarpeFollower;
                    }

                    lock (_lockObject)
                    {
                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                        Thread.Sleep(delay * 1000);
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        public string GetPictureCountAndLatestSnapUrlfollower(ref InstagramUser accountManager, string followerPageSource)
        {
            try
            {
                lstThreadsScrapeFollower.Add(Thread.CurrentThread);
                lstThreadsScrapeFollower.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string retCountAndUrl = string.Empty;
            try
            {
                if (!ClGlobul.isStopScrapeFollowers)
                {
                    Thread.CurrentThread.IsBackground = true;
                    ClGlobul.lstThreadsScrapeFollowers.Add(Thread.CurrentThread);
                    ClGlobul.lstThreadsScrapeFollowers = ClGlobul.lstThreadsScrapeFollowers.Distinct().ToList();
                }
                else
                {
                    retCountAndUrl = "stop";
                    return retCountAndUrl;
                }
                bool enterOnce = false;
                string rawLatestPostUrl = string.Empty;
                string latestPostUrl = string.Empty;
                int pictureCount = 0;
                string[] Picture_Split = Regex.Split(followerPageSource, "code");
                foreach (string picture in Picture_Split)
                {
                    if (!picture.Contains("<!DOCTYPE html>"))
                    {
                        if (picture.Contains("date"))
                        {
                            if (!enterOnce)
                            {
                                rawLatestPostUrl = Utils.getBetween(picture, "\":\"", "\"");
                                latestPostUrl = "https://www.instagram.com/p/" + rawLatestPostUrl + "/";
                                enterOnce = true;
                            }
                            pictureCount++;
                        }
                    }
                }

                string snapCount = pictureCount.ToString();
                retCountAndUrl = latestPostUrl + "splitHere" + snapCount;
            }
            catch (Exception ex)
            {


            }
            return retCountAndUrl;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }



        #endregion

        #region Scrape by username
        public void Start_scraping(ref InstagramUser Obj_UserScrape)
        {
            try
            {
                lstThreadsScrapeUser.Add(Thread.CurrentThread);
                lstThreadsScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string test = Obj_UserScrape.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
            if (string.IsNullOrEmpty(UserScape_Path))
            {
                if (!string.IsNullOrEmpty(UserScrape_single)) ;
                {

                    string s = UserScrape_single;

                    if (s.Contains(','))
                    {
                        string[] Data = s.Split(',');

                        foreach (var item in Data)
                        {


                            ClGlobul.HashTagForScrap.Add(item);
                        }
                    }
                    else
                    {
                        ClGlobul.HashTagForScrap.Add(UserScrape_single);
                    }
                }
            }

            if (ClGlobul.HashTagForScrap.Count > 0)
            {
                if (minDelayScrapeUser != null)
                {

                    // AddToHashTagLogger("[ " + DateTime.Now + " ] => [ Scraping Started for User Ids ]");
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Scraping Started for User Ids ]");

                    try
                    {
                        //Thread _ThreadHashStart = new Thread(ScrapUserName);
                        //_ThreadHashStart.Start();
                        ScrapUserName(ref Obj_UserScrape);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please enter preferred delay time ]");
                }
            }
            else
            {
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ No Tags Uploaded ]");
            }
        }
        public void ScrapUserName(ref InstagramUser obj_newobject)
        {
            try
            {
                lstThreadsScrapeUser.Add(Thread.CurrentThread);
                lstThreadsScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            try
            {
                string test = obj_newobject.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                foreach (string itemHash in ClGlobul.HashTagForScrap)
                {
                    startUserScraper(itemHash, ref obj_newobject);

                    Thread.Sleep(5000);
                }

                //GlobusLogHelper.log.Info("[" + DateTime.Now + " ]=>[Process Completed]");
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Process Completed ]");
            }
            catch { }
        }
        public void startUserScraper(string itemHash, ref InstagramUser obj_scrapeuser)
        {

            try
            {
                lstThreadsScrapeUser.Add(Thread.CurrentThread);
                lstThreadsScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string pageSource = string.Empty;
            string response = string.Empty;
            string postData = string.Empty;
            List<string> lstCountScrapUser = new List<string>();
            string test = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
            if (stopScrapBool) return;
            try
            {
                GlobusHttpHelper _GlobusHttpHelper = new GlobusHttpHelper();
                string Home_icon_Url = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                string PPagesource = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                string responce_icon = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");
                if (!string.IsNullOrEmpty(responce_icon))
                {

                    string url = "http://iconosquare.com/viewer.php#/search/" + itemHash;
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
                    string viewer_responce = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php "), "");
                    string crs_token = Utils.getBetween(viewer_responce, " <div id=\"accesstoken\" style=\"display:none;\">", "</div>");
                    response = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");

                    string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + crs_token + "&q=" + itemHash;
                    string respon_scrapeuser = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), referer);
                    string[] data_divided = Regex.Split(respon_scrapeuser, "username");
                    foreach (string var in data_divided)
                    {
                        if (var.Contains("profile_picture"))
                        {
                            string User_list = Utils.getBetween(var, "\":\"", "\"");
                            if (lstCountScrapUser.Count < No_UserCount)
                            {
                                lstCountScrapUser.Add(User_list);
                                lstCountScrapUser = lstCountScrapUser.Distinct().ToList();
                            }
                        }
                    }
                    #region Export

                    foreach (string itre in lstCountScrapUser)
                    {
                        string Username = itre;
                        string Userlink = "https://www.instagram.com/" + itre + "/";
                        string itemhashdata = itemHash;
                        try
                        {
                            string CSVData = itemhashdata + "," + Username + "," + Userlink;
                            GlobusFileHelper.ExportDataCSVFile(CSVHeader, CSVData, CSVPath);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                        }
                        try
                        {
                            DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,UserName,UserLink, Status) values('" + "ScrapeUser" + "','" + obj_scrapeuser.username + "','" + DateTime.Now + "','" + Username + "','" + Userlink + "','" + "Scraped" + "')", "tbl_AccountReport");
                            GlobusLogHelper.log.Info("[" + Username + "," + "itemHash:" + "," + itemhashdata + "," + "userName:" + "," + Username + "," + "userLink:" + "," + Userlink + "]");
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                        }

                    }




                    #endregion
                    // code is in middle .......






                    if (!string.IsNullOrEmpty(response))
                    {
                        if (url.Contains("search"))
                        {
                            if (response.Contains("class=\"username\""))
                            {
                                try
                                {
                                    string[] arrOfUserName = Regex.Split(response, "class=\"username\"");

                                    if (arrOfUserName.Length > 0)
                                    {
                                        arrOfUserName = arrOfUserName.Skip(1).ToArray();
                                        foreach (string itemArray in arrOfUserName)
                                        {
                                            if (stopScrapBool) return;
                                            try
                                            {
                                                string startString = "href=\"/n/";
                                                string endString = "\">";
                                                if (itemArray.Contains(startString) && itemArray.Contains(endString))
                                                {
                                                    string userName = string.Empty;
                                                    try
                                                    {
                                                        userName = Utils.getBetween(itemArray, startString, endString).Replace("class=\"profimg", "").Replace("\"", "");
                                                        lstCountScrapUser.Add(userName);
                                                        lstCountScrapUser = lstCountScrapUser.Distinct().ToList();
                                                        string itemhashdata = itemHash;
                                                        string Username = userName.Replace("class=\"profimg", "").Replace("\"", "");
                                                        string UserLink = ("http://websta.me/n/" + userName).Replace(" class=\"profimg", "").Replace("\"", "");


                                                        if (!string.IsNullOrEmpty(userName))
                                                        {
                                                            #region CSV Write
                                                            try
                                                            {

                                                                Duplicatedatascrape.Add(userName, UserLink);

                                                                string CSVData = itemhashdata + "," + Username + "," + UserLink;
                                                                GlobusFileHelper.ExportDataCSVFile(CSVHeader, CSVData, CSVPath);
                                                            }

                                                            catch { }
                                                            try
                                                            {
                                                                DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,UserName,UserLink, Status) values('" + "ScrapeUser" + "','" + obj_scrapeuser.username + "','" + DateTime.Now + "','" + Username + "','" + UserLink + "','" + "Scraped" + "')", "tbl_AccountReport");
                                                                GlobusLogHelper.log.Info("[" + Username + "," + "itemHash:" + "," + itemhashdata + "," + "userName:" + "," + Username + "," + "userLink:" + "," + UserLink + "]");
                                                                //objclasssforlogger2.AddToLogger_Scrape_Bykey("[ " + Username + "," + "itemHash:" + "," + itemhashdata + "," + "userName:" + "," + Username + "," + "userLink:" + "," + UserLink + "] ");
                                                            }

                                                            catch { };
                                                            #endregion

                                                            try
                                                            {
                                                                if (stopScrapBool) return;

                                                                try
                                                                {

                                                                    GlobusLogHelper.log.Info("=> [  UserName" + Username + " ]");
                                                                    if (minDelayScrapeUser != 0)
                                                                    {
                                                                        mindelay = minDelayScrapeUser;
                                                                    }
                                                                    if (maxDelayScrapeUser != 0)
                                                                    {
                                                                        maxdelay = maxDelayScrapeUser;
                                                                    }

                                                                    Random obj_rn = new Random();
                                                                    int delay1 = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                                                    delay1 = obj_rn.Next(mindelay, maxdelay);
                                                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay1 + " Seconds ]");
                                                                    Thread.Sleep(delay1 * 1000);


                                                                }
                                                                catch { }

                                                                if (lstCountScrapUser.Count >= No_UserCount)
                                                                {
                                                                    return;
                                                                }
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                    catch { }
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            try
                            {
                                if (response.Contains("div class=\"userinfo clearfix"))
                                {
                                    string[] splites_User = Regex.Split(response, "div class=\"userinfo clearfix");
                                    foreach (string item in splites_User)
                                    {
                                        if (!item.Contains("<!DOCTYPE html>"))
                                        {
                                            try
                                            {
                                                string User_Url = Utils.getBetween(item, "<a href=\"", "\"");
                                                User_Url = (IGGlobals.Instance.IGWEPME + User_Url);
                                                string User_Name = Utils.getBetween(item, "class=\"username\">", "</a>");
                                                lstCountScrapUser.Add(User_Name);
                                                lstCountScrapUser = lstCountScrapUser.Distinct().ToList();
                                                if (!string.IsNullOrEmpty(User_Name))
                                                {
                                                    #region CSV Write
                                                    try
                                                    {

                                                        Duplicatedatascrape.Add(User_Name, User_Url);

                                                        string CSVData = itemHash + "," + User_Name + "," + User_Url;
                                                        GlobusFileHelper.ExportDataCSVFile(CSVHeader, CSVData, CSVPath);
                                                    }

                                                    catch { }
                                                    try
                                                    {

                                                        GlobusLogHelper.log.Info("[" + User_Name + "," + "itemHash:" + "," + itemHash + "," + "userName:" + "," + User_Name + "," + "userLink:" + "," + User_Url + "]");
                                                        //objclasssforlogger2.AddToLogger_Scrape_Bykey("[ " + Username + "," + "itemHash:" + "," + itemhashdata + "," + "userName:" + "," + Username + "," + "userLink:" + "," + UserLink + "] ");
                                                    }

                                                    catch { };
                                                    #endregion
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                                            }
                                        }

                                        if (lstCountScrapUser.Count >= No_UserCount)
                                        {
                                            return;
                                        }
                                        if (minDelayScrapeUser != 0)
                                        {
                                            mindelay = minDelayScrapeUser;
                                        }
                                        if (maxDelayScrapeUser != 0)
                                        {
                                            maxdelay = maxDelayScrapeUser;
                                        }

                                        Random obj_rn = new Random();
                                        int delay1 = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                        delay1 = obj_rn.Next(mindelay, maxdelay);
                                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay1 + " Seconds ]");
                                        Thread.Sleep(delay1 * 1000);

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                            }
                        }
                    }
                    else
                    {
                        //Do Nothing yet
                    }

                }
            }
            catch { }

        }
        #endregion

        #region Scrape BY KeyWord
        public void scrapeUesr_Key(string Key, ref InstagramUser obj_keyword)
        {

            GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();

            int total_page = 0;
            string Main_url = IGGlobals.Instance.IGwebstakeywordurl + Key;
            string PPagesource = objGlobusHttpHelper.getHtmlfromUrl(new Uri(Main_url), "");
            string findtotal_pge1 = Utils.getBetween(PPagesource, "<ul class=\"pagination\">", "</ul>");
            string[] slit_data = Regex.Split(findtotal_pge1, "&amp;");
            foreach (string item1 in slit_data)
            {
                try
                {
                    if (!(item1.Contains("<a href=")))
                    {
                        string totlno_pge = Utils.getBetween(item1, "page=", "\">");
                        total_page = int.Parse(totlno_pge);

                    }
                }
                catch { };

            }

            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ START SCRAPING ]");
            for (int i = 1; i <= total_page; i++)
            {
                try
                {
                    string url = IGGlobals.Instance.IGwebstakeywordurl + Key + "?&page=" + i;
                    string responces = objGlobusHttpHelper.getHtmlfromUrl(new Uri(url), "");

                    string getData = Utils.getBetween(responces, "<ul class=\"userlist\">", "<ul class=\"pagination\"");

                    string[] data = Regex.Split(getData, "<strong");
                    foreach (string item in data)
                    {
                        try
                        {
                            if (item.Contains("</strong>"))
                            {
                                string local_valu = Utils.getBetween(item, ">", "</strong>");
                                User_Bykey.Add(local_valu);
                            }
                        }
                        catch { };

                    }

                }
                catch { };

            }
            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  Total Number Of user  : " + User_Bykey.Count + " ]");

            #region CSV Write
            try
            {

                foreach (string itrm in User_Bykey)
                {
                    string Username = itrm;
                    string UserLink = "http://websta.me/n/" + itrm;
                    DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,UserName,UserLink, Status) values('" + "ScrapeUser" + "','" + obj_keyword.username + "','" + DateTime.Now + "','" + Username + "','" + UserLink + "','" + "Scraped" + "')", "tbl_AccountReport");
                    string CSVData = Username + "," + UserLink;
                    GlobusFileHelper.ExportDataCSVFile(CSVHeader, CSVData, CSVPath);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  CSV File Built Successfully ]");
            }


            catch { }
            #endregion


            // string total_pge = "";
            // int total_page = int.Parse("total_pge");
            //for (int i = 1; i < total_page; i++)
            //{
            //    string url = "http://websta.me/keyword/" + Key + "?&page=" + i;
            //}
        }
        #endregion


        #region Scarpe_usernew Requiedment

        //    public void Scrape_liker(ref InstagramUser obj_GDuser)
        //    {

        //        string username_giving = "aasuuna";
        //             string res_secondURL = obj_GDuser.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
        //        try
        //        {
        //            ClGlobul.switchAccount = false;
        //           string Url_user = "https://www.instagram.com/" + PhotoId;
        //                string responce_user = Photo_likebyID.globusHttpHelper.getHtmlfromUrl(new Uri(Url_user), "");
        //                string token = Utils.getBetween(responce_user, "csrf_token\":\"", "\"}");
        //                string[] photo_code = Regex.Split(responce_user, "code\":");
        //                foreach (string list in photo_code)
        //                {
        //                    if (list.Contains("date"))
        //                    {
        //                        string photo_codes = Utils.getBetween("@"+list, "@\"", "\"");
        //                        phto_list.Add(photo_codes);
        //                    }
        //                }
        //                if(responce_user.Contains("has_next_page\":true"))
        //                {
        //                    while(value)
        //                    {
        //                        if(responce_user.Contains("has_next_page\":true") && phto_list.Count<temp)
        //                        {
        //                            string IDD = Utils.getBetween(responce_user,"\"id\":\"","\"");
        //                            string code_ID = Utils.getBetween(responce_user,"end_cursor\":\"","\"");
        //                            string postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
        //                            responce_user = Photo_likebyID.globusHttpHelper.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, "https://www.instagram.com/" + PhotoId, token);
        //                            string[] data1 = Regex.Split(responce_user, "code");
        //                            foreach(string val in data1)
        //                            {
        //                                if (val.Contains("date"))
        //                                {
        //                                    if (phto_list.Count < temp)
        //                                    {
        //                                        string photo_codes = Utils.getBetween(val, "\":\"", "\"");
        //                                        phto_list.Add(photo_codes);
        //                                    }
        //                                    else
        //                                    {
        //                                        break;
        //                                    }
        //                                }
        //                            }



        //                        }
        //                        else
        //                        {
        //                            value = false;
        //                        }
        //        catch (Exception ex)
        //        {
        //            GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
        //        }
        //    }


          #endregion








    }

    public class ScrapeUser
    {
        #region Global Variable For Scrape User 

        public bool isScrapeFollower = false;
        public bool isScrapeFollowing = false;
        public bool isScrapeUserWhoCommentOnPhoto = false;
        public bool isScrapePhotoLikeOfUser = false;
        public bool isScrapeHashTag = false;
        public bool isScrapeUserWhoLikesUserPhoto = false;
        public bool isScrapePhotoURL = false;
        public bool isScrapeUserFromHashTag = false;
        public bool isScrapeImageFromHashTag = false;

        public bool isStopScrapeUser = false;

        public string usernmeToScrape = string.Empty;     
        public List<string> selectedAccountToScrape = new List<string>();

        public int noOfUserToScrape = 0;
        public int noOfPhotoToScrape = 0;
        public int minDelayScrapeUser = 10;
        public int maxDelayScrapeUser = 20;
        public int NoOfThreadsScarpeUser = 25;

        public List<string> listOfPhotoIdAnd = new List<string>();
        public List<string> listOfMessageToComment = new List<string>();

        public List<string> listOfHashTag = new List<string>();
        public List<Thread> lstofThreadScrapeUser = new List<Thread>();
        public List<string> listOfFollower = new List<string>();
        public List<string> listOfFollowing = new List<string>();
        public List<string> listOfUploadedUrls = new List<string>();

        public List<string> listOfUsernameForCommentuserScraper = new List<string>();
        public List<string> listOfUsernameForPhotouserScraper = new List<string>();
        public List<string> listOfPhotoUrl = new List<string>();

        static readonly Object _lockObject = new Object();
        readonly object lockrThreadControlleScrapeUser = new object();
        readonly object lockrThreadControlleScrapeFollower = new object();
        

        int countThreadControllerScrapeUser = 0;
        int countThreadControllerScrapeFollower = 0;

        private const string CSVHeader = "HashTag,UserName,UserLink";
        private string CSVPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Gram Dominator\\scrapedUserDetails.csv";

        private const string CSVHeader_following = "Username,ScrapeUser";
        private string CSVPath_following = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Gram Dominator\\ScrapedFollowingDetails.csv";

        private const string CVSHeader_PhotoUrl = "Username,PhotoUrl";
        private string CSVPath_PhotoUrl = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Gram Dominator\\ScrapedPhotoUrl.csv";

        private const string CVSHeader_PhotoCommentUser = "Username,PhotoUrl,CommentUser";
        private string CSVPath_PhotoCommentUrl = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Gram Dominator\\ScrapedPhotoCommentUser.csv";

        private const string CVSHeader_PhotoLikerUser = "Username,PhotoUrl,LikerUser";
        private string CSVPath_PhotoLikerLikerUser = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Gram Dominator\\ScrapedPhotoLikerUser.csv";



        public static Dictionary<string, string> Duplicatedatascrape = new Dictionary<string, string>();

       // ScrapingManager objScrapingManager = new ScrapingManager();

        #endregion

        public void StartScrapUser()
        {

            try
            {
                countThreadControllerScrapeFollower = 0;
                try
                {
                    int numberOfAccountPatch = 25;

                    if (NoOfThreadsScarpeUser > 0)
                    {
                        numberOfAccountPatch = NoOfThreadsScarpeUser;
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
                                    string[] data = Regex.Split(account, ":");
                                    string Accout = data[0];

                                    if (selectedAccountToScrape.Contains(Accout))
                                    //if (Accout.Contains(selectedAccountToScrape))
                                    {
                                        lock (lockrThreadControlleScrapeFollower)
                                        {
                                            try
                                            {
                                                if (countThreadControllerScrapeFollower >= listAccounts.Count)
                                                {
                                                    Monitor.Wait(lockrThreadControlleScrapeFollower);
                                                }

                                                string acc = account.Remove(account.IndexOf(':'));

                                                //Run a separate thread for each account
                                                InstagramUser item = null;

                                                IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                                                if (item != null)
                                                {
                                                    Thread profilerThread = new Thread(StartMultiThreadsScrapeFollower);
                                                    profilerThread.Name = "workerThread_Profiler_" + acc;
                                                    profilerThread.IsBackground = true;

                                                    profilerThread.Start(new object[] { item, });
                                                    //DS = item.ds;
                                                    countThreadControllerScrapeUser++;
                                                }
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
                            }
                        }
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

        public void StartMultiThreadsScrapeFollower(object parameters)
        {
            try
            {
                if (!isStopScrapeUser)
                {
                    try
                    {
                        lstofThreadScrapeUser.Add(Thread.CurrentThread);
                        lstofThreadScrapeUser.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                    try
                    {
                        string status = string.Empty;
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

                            StartActionScrapeUser(ref objFacebookUser);

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
                        lock (lockrThreadControlleScrapeUser)
                        {
                            countThreadControllerScrapeUser--;
                            Monitor.Pulse(lockrThreadControlleScrapeUser);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }

        public void StartActionScrapeUser(ref InstagramUser objInstagramUser)
        {
            if(isStopScrapeUser)
            {
                return;
            }
            try
            {
                lstofThreadScrapeUser.Add(Thread.CurrentThread);
                lstofThreadScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            try
            {
                if(isScrapeFollower)
                {
                    StartScrapeFollower(ref objInstagramUser);
                }
                else if(isScrapeFollowing)
                {
                    StartScrapeFollowing(ref objInstagramUser);
                }
                else if(isScrapeHashTag)
                {
                    if (isScrapeUserFromHashTag)
                    {
                        ScrapUserNameFromHashTag(ref objInstagramUser);
                    }
                    else if(isScrapeImageFromHashTag)
                    {
                        startScrapeImageFromHashtag(ref objInstagramUser);
                    }
                }
                else if(isScrapePhotoURL)
                {
                    ScrapePhotoURL(ref objInstagramUser);
                }
                else if(isScrapePhotoLikeOfUser)
                {
                    Start_Scrapephotoliker(ref objInstagramUser);
                }
                else if(isScrapeUserWhoCommentOnPhoto)
                {
                    start_scrapephotocomment_User(ref objInstagramUser);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            finally
            {
                GlobusLogHelper.log.Info("----- Process Completed --------");
            }
        }

        public void Start_Scrapephotoliker(ref InstagramUser obj_GDUSER)
        {
            if (isStopScrapeUser)
            {
                return;
            }
            try
            {
                lstofThreadScrapeUser.Add(Thread.CurrentThread);
                lstofThreadScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            //string username_liker = usernmeToScrape; //"i_am_komal_jha";
            isScrapePhotoLikeOfUser = false;
            int temp = noOfPhotoToScrape;
            string response = string.Empty;
            List<string> photoId_list = new List<string>();
            List<string> User_photolike = new List<string>();
            string respon_scrapeuser = string.Empty;
            bool value = true;

            foreach (string username_liker in listOfUsernameForPhotouserScraper)
            {
                try
                {
                    string Home_icon_Url = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                    string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                    string PPagesource = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                    string responce_icon = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");
                    if (!string.IsNullOrEmpty(responce_icon))
                    {

                        string url = "http://iconosquare.com/viewer.php#/search/" + username_liker;


                        string referer = "http://iconosquare.com/viewer.php";
                        string viewer_responce = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php "), "");
                        string crs_token = Utils.getBetween(viewer_responce, " <div id=\"accesstoken\" style=\"display:none;\">", "</div>");
                        response = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");

                        string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + crs_token + "&q=" + username_liker;
                        respon_scrapeuser = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), referer);
                        string ID = Utils.getBetween(respon_scrapeuser, "id\":\"", "\"");
                        string media_Url = "http://iconosquare.com/rqig.php?e=/users/" + ID + "/media/recent&a=ico2&t=" + crs_token + "&count=20";
                        respon_scrapeuser = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(media_Url), "");
                        string[] data_divided = Regex.Split(respon_scrapeuser, "user_has_liked");
                        foreach (string itemmm in data_divided)
                        {
                            try
                            {
                                if (!itemmm.Contains("pagination"))
                                {
                                    if (temp > photoId_list.Count())
                                    {
                                        string photo_ID = Utils.getBetween(itemmm, "id\":\"", ",\"").Replace("\"", "");
                                        string imageUrl = "https://www.instagram.com/p/" + photo_ID + "/";
                                        photoId_list.Add(photo_ID);
                                        photoId_list = photoId_list.Distinct().ToList();
                                        try
                                        {
                                            DataBaseHandler.InsertQuery("insert into ScrapedImage(Username,ImageURL,ImageID) values('" + username_liker + "','" + imageUrl + "','" + photo_ID + "')", "ScrapedImage");
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
                            }
                        }
                        if (respon_scrapeuser.Contains("next_url"))
                        {
                            while (value)
                            {
                                if (respon_scrapeuser.Contains("next_url") && temp > photoId_list.Count())
                                {
                                    string Items = Utils.getBetween(respon_scrapeuser, "next_max_id\":\"", "\"");
                                    string new_paginationUrl = media_Url + "&cursor=&max_id=" + Items;
                                    respon_scrapeuser = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(media_Url), "");
                                    string[] data_dividedd = Regex.Split(respon_scrapeuser, "user_has_liked");
                                    foreach (string itemmmm in data_dividedd)
                                    {
                                        try
                                        {
                                            if (!itemmmm.Contains("pagination"))
                                            {
                                                if (temp > photoId_list.Count())
                                                {
                                                    string photo_ID = Utils.getBetween(itemmmm, "id\":\"", ",\"").Replace("\"", "");
                                                    string imageUrl = "https://www.instagram.com/p/" + photo_ID + "/";
                                                    photoId_list.Add(photo_ID);
                                                    photoId_list = photoId_list.Distinct().ToList();
                                                    try
                                                    {
                                                        DataBaseHandler.InsertQuery("insert into ScrapedImage(Username,ImageURL,ImageID) values('" + username_liker + "','" + imageUrl + "','" + photo_ID + "')", "ScrapedImage");
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
                                        }
                                    }

                                }
                                else
                                {
                                    value = false;
                                }
                            }
                        }

                        foreach (string item_Photo in photoId_list)
                        {
                            try
                            {
                                string Photoliker_Url = "http://iconosquare.com/viewer.php#/detail/" + item_Photo;
                                string resp_Photo_liker = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(Photoliker_Url), "");
                                string List_likerurl = "http://iconosquare.com/rqig.php?e=/media/" + item_Photo + "/likes&a=ico2&t=" + crs_token + "&count=20&cursor=&max_id=&max_like_id=";
                                string List_Liker = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(List_likerurl), "");
                                string[] data_userlist = Regex.Split(List_Liker, "username");
                                foreach (string listofUser in data_userlist)
                                {
                                    try
                                    {
                                        if (listofUser.Contains("profile_picture"))
                                        {
                                            string Username_photolike = Utils.getBetween(listofUser, "\":\"", "\"");                                           
                                            User_photolike.Add(Username_photolike);
                                            User_photolike = User_photolike.Distinct().ToList();
                                            try
                                            {
                                                GlobusLogHelper.log.Info("Scraped===>" + Username_photolike);
                                                string CSVData = username_liker.Replace(",", string.Empty) + "," + item_Photo.Replace(",", string.Empty) + "," + Username_photolike.Replace(",", string.Empty);
                                                GlobusFileHelper.ExportDataCSVFile(CVSHeader_PhotoLikerUser, CSVData, CSVPath_PhotoLikerLikerUser);
                                            }
                                            catch(Exception ex)
                                            {

                                                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                                            }
                                            try
                                            {
                                                DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + Username_photolike + "')", "ScrapedUsername");
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
                }
                finally
                {
                    GlobusLogHelper.log.Info("----- Process Completed of Scrape user Form PhotoLiker ------");
                }
            }
        }

        public void start_scrapephotocomment_User(ref InstagramUser obj_GDUSER)
        {
            if (isStopScrapeUser)
            {
                return;
            }
            try
            {
                lstofThreadScrapeUser.Add(Thread.CurrentThread);
                lstofThreadScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            List<string> PhotoId_list = new List<string>();
            isScrapeUserWhoCommentOnPhoto = false;
            List<string> ListofUser_commentOnphoto = new List<string>();
            try
            {
                foreach (string item1 in listOfUsernameForCommentuserScraper)
                {
                    string username = item1;//"i_am_komal_jha";
                    int temp = noOfPhotoToScrape;
                    bool value = true;

                    string Url_user = "https://www.instagram.com/" + username;
                    string responce_user = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(Url_user), "");
                    string token = Utils.getBetween(responce_user, "csrf_token\":\"", "\"}");
                    string[] photo_code = Regex.Split(responce_user, "code\":");
                    foreach (string list in photo_code)
                    {
                        if (list.Contains("date"))
                        {
                            string photo_codes = Utils.getBetween("@" + list, "@\"", "\"");
                            PhotoId_list.Add(photo_codes);
                        }
                    }
                    if (responce_user.Contains("has_next_page\":true"))
                    {
                        while (value)
                        {
                            if (responce_user.Contains("has_next_page\":true") && PhotoId_list.Count < temp)
                            {
                                string IDD = Utils.getBetween(responce_user, "\"id\":\"", "\"");
                                string code_ID = Utils.getBetween(responce_user, "end_cursor\":\"", "\"");
                                string postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                responce_user = obj_GDUSER.globusHttpHelper.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, "https://www.instagram.com/" + username, token);
                                string[] data1 = Regex.Split(responce_user, "code");
                                foreach (string val in data1)
                                {
                                    if (val.Contains("date"))
                                    {
                                        if (PhotoId_list.Count < temp)
                                        {
                                            string photo_codes = Utils.getBetween(val, "\":\"", "\"");
                                            PhotoId_list.Add(photo_codes);
                                            string imageUrl = "https://www.instagram.com/p/" + photo_codes + "/";                                            
                                            try
                                            {
                                                DataBaseHandler.InsertQuery("insert into ScrapedImage(Username,ImageURL,ImageID) values('" + username + "','" + imageUrl + "','" + photo_codes + "')", "ScrapedImage");
                                            }
                                            catch (Exception ex)
                                            {

                                            }
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
                foreach (string item in PhotoId_list)
                {
                    try
                    {
                        string url = "https://www.instagram.com/p/" + item + "/";
                        string responce = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");
                        if (responce.Contains("comments\":{\"count"))
                        {
                            string[] data = Regex.Split(responce, "text");
                            foreach (string itemm in data)
                            {
                                if (itemm.Contains("username"))
                                {
                                    string Username_List = Utils.getBetween(itemm, "username\":\"", "\"");
                                    ListofUser_commentOnphoto.Add(Username_List);
                                    try
                                    {
                                        GlobusLogHelper.log.Info("Scraped===>" + Username_List);
                                        string CSVData = usernmeToScrape.Replace(",", string.Empty) + "," + url.Replace(",", string.Empty) + "," + Username_List.Replace(",", string.Empty);
                                        GlobusFileHelper.ExportDataCSVFile(CVSHeader_PhotoCommentUser, CSVData, CSVPath_PhotoCommentUrl);
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                                    } 

                                    try
                                    {
                                        DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + Username_List + "')", "ScrapedUsername");
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                                    }  
                                    //ListofUser_commentOnphoto = ListofUser_commentOnphoto.Distinct().ToList();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error:" + ex.StackTrace);
                    }
                }
            

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error:" + ex.StackTrace);
            }
            finally
            {
                GlobusLogHelper.log.Info("----- Process Completed Scrape USer Form Comment --------");
            }
        }

        public void ScrapUserNameFromHashTag(ref InstagramUser obj_newobject)
        {
            if (isStopScrapeUser)
            {
                return;
            }
           
            try
            {
                lstofThreadScrapeUser.Add(Thread.CurrentThread);
                lstofThreadScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            try
            {
                if (!string.IsNullOrEmpty(usernmeToScrape))
                {
                    listOfHashTag.Add(usernmeToScrape);
                    string test = obj_newobject.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");

                    foreach (string itemHash in listOfHashTag)
                    {
                        startUserScraper(itemHash, ref obj_newobject);

                        Thread.Sleep(5000);
                    }

                    //GlobusLogHelper.log.Info("[" + DateTime.Now + " ]=>[Process Completed]");
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Process Completed ]");
                }
            }
            catch { }
        }

        public void ScrapePhotoURL(ref InstagramUser objInstagramUser)
        {
            if (isStopScrapeUser)
            {
                return;
            }
            try
            {
                lstofThreadScrapeUser.Add(Thread.CurrentThread);
                lstofThreadScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            List<string> PhotoUrlList = new List<string>();
            List<string> ListofUser_commentOnphoto = new List<string>();
            isScrapePhotoURL = false;
            try
            {
                foreach (string item in listOfPhotoUrl)
                {
                    string username = item;//"i_am_komal_jha";
                    int temp = noOfPhotoToScrape;
                    bool value = true;

                    string Url_user = "https://www.instagram.com/" + username;
                    string responce_user = objInstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri(Url_user), "");
                    string token = Utils.getBetween(responce_user, "csrf_token\":\"", "\"}");
                    string[] photo_code = Regex.Split(responce_user, "code\":");
                    foreach (string list in photo_code)
                    {
                        if (list.Contains("date"))
                        {
                            string photo_codes = Utils.getBetween("@" + list, "@\"", "\"");
                            if (PhotoUrlList.Count < temp)
                            {
                                string Imageurl = "https://www.instagram.com/p/" + photo_codes + "/";
                                PhotoUrlList.Add(Imageurl);

                                try
                                {
                                    GlobusLogHelper.log.Info("Scraped===>" + Imageurl);
                                    string CSVData = username.Replace(",", string.Empty) + "," + Imageurl.Replace(",", string.Empty);
                                    GlobusFileHelper.ExportDataCSVFile(CVSHeader_PhotoUrl, CSVData, CSVPath_PhotoUrl + ".csv");
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
                                }

                                try
                                {
                                    DataBaseHandler.InsertQuery("insert into ScrapedImage(Username,ImageURL,ImageID) Values('" + username + "','" + Imageurl + "','" + photo_code + "')", "ScrapedImage");
                                }
                                catch(Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (responce_user.Contains("has_next_page\":true"))
                    {
                        while (value)
                        {
                            if (responce_user.Contains("has_next_page\":true") && PhotoUrlList.Count < temp)
                            {
                                string IDD = Utils.getBetween(responce_user, "\"id\":\"", "\"");
                                string code_ID = Utils.getBetween(responce_user, "end_cursor\":\"", "\"");
                                string postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                responce_user = objInstagramUser.globusHttpHelper.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, "https://www.instagram.com/" + username, token);
                                string[] data1 = Regex.Split(responce_user, "code");
                                foreach (string val in data1)
                                {
                                    if (val.Contains("date"))
                                    {
                                        if (PhotoUrlList.Count < temp)
                                        {
                                            string photo_codes = Utils.getBetween(val, "\":\"", "\"");
                                            string Imageurl = "https://www.instagram.com/p/" + photo_codes + "/";
                                            PhotoUrlList.Add(Imageurl);
                                            try
                                            {
                                                DataBaseHandler.InsertQuery("insert into ScrapedImage(Username,ImageURL,ImageID) Values('" + username + "','" + Imageurl + "','" + photo_code + "')", "ScrapedImage");
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
                                            }
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
                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
            }
            finally
            {
                GlobusLogHelper.log.Info("-----Process Completed to scrape PhotoUrl------");
            }
        }

        public void StartScrapeFollower(ref InstagramUser obj_folowerscrape)
        {
            isScrapeFollower = false;
            if (isStopScrapeUser)
            {
                return;
            }        

            try
            {
                lstofThreadScrapeUser.Add(Thread.CurrentThread);
                lstofThreadScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string rawUsernameID = string.Empty;
            string usernamePgSource = string.Empty;
            bool value = true;

            string res_secondURL = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
            try
            {
                ClGlobul.switchAccount = false;
                string Home_icon_Url = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                string PPagesource = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                string crstoken = Utils.getBetween(PPagesource, "<div id=\"accesstoken\"", "/div>");
                string CRS_Token = Utils.getBetween(crstoken, "\">", "<");
                string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + CRS_Token + "&q=" + usernmeToScrape;
                string respo_profile = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), "http://iconosquare.com/viewer.php");
                string profile_ID = Utils.getBetween(respo_profile, "\"id\":\"", "\"");
                string post_Url = "http://iconosquare.com/rqig.php?e=/users/" + profile_ID + "/follows&a=ico2&t=" + CRS_Token + "&count=20";
                string Profile_responce = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php#/user/" + profile_ID), "http://iconosquare.com/viewer.php");
                string list_follower = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php#/followers/" + profile_ID), "");
                postdata = "http://iconosquare.com/rqig.php?e=/users/" + profile_ID + "/followed-by&a=ico2&t=" + CRS_Token + "&count=20";
                string follow_respo = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), "http://iconosquare.com/viewer.php");
                string[] data = Regex.Split(follow_respo, "username");
                foreach (string var in data)
                {
                    if (var.Contains("profile_picture"))
                    {
                        string user_name = Utils.getBetween(var, "\":\"", "\"");
                        listOfFollower.Add(user_name);
                        try
                        {
                            DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + user_name + "')", "ScrapedUsername");
                        }
                        catch(Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                        }                       

                        GlobusLogHelper.log.Info("Scraped===>" + user_name);
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
                            follow_respo = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(page_Url), "http://iconosquare.com/viewer.php");
                            string[] data1 = Regex.Split(follow_respo, "username");
                            foreach (string item in data1)
                            {
                                if (item.Contains("profile_picture"))
                                {
                                    string username = Utils.getBetween(item, ":\"", "\"");
                                    listOfFollower.Add(username);
                                    try
                                    {
                                        DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + username + "')", "ScrapedUsername");
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                                    }
                                    GlobusLogHelper.log.Info("Scraped===>" + username);
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
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
            }
        }
               
        public void StartScrapeFollowing(ref InstagramUser obj_folowerscrape)
        {
            isScrapeFollowing = false;
            try
            {
                lstofThreadScrapeUser.Add(Thread.CurrentThread);
                lstofThreadScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string rawUsernameID = string.Empty;
            string usernamePgSource = string.Empty;
            List<string> ScrapeFollowingUser = new List<string>();
            bool value = true;
            foreach (string usernmeToScrape in listOfFollowing)
            {
                string res_secondURL = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                try
                {
                    ClGlobul.switchAccount = false;
                    string resp = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                    string Home_icon_Url = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                    string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                    string PPagesource = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                    string crstoken = Utils.getBetween(PPagesource, "<div id=\"accesstoken\"", "/div>");
                    string CRS_Token = Utils.getBetween(crstoken, "\">", "<");
                    string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + CRS_Token + "&q=" + usernmeToScrape;
                    string respo_profile = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), "http://iconosquare.com/viewer.php");
                    string profile_ID = Utils.getBetween(respo_profile, "\"id\":\"", "\"");
                    string post_Url = "http://iconosquare.com/rqig.php?e=/users/" + profile_ID + "/follows&a=ico2&t=" + CRS_Token + "&count=20";
                    string Profile_responce = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php#/user/" + profile_ID), "http://iconosquare.com/viewer.php");
                    string list_follower = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php#/followings/" + profile_ID), "");
                    postdata = "http://iconosquare.com/rqig.php?e=/users/" + profile_ID + "/follows&a=ico2&t=" + CRS_Token + "&count=20";
                    string follow_respo = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), "http://iconosquare.com/viewer.php");
                    string[] data = Regex.Split(follow_respo, "username");
                    foreach (string var in data)
                    {
                        if (var.Contains("profile_picture"))
                        {
                            string user_name = Utils.getBetween(var, "\":\"", "\"");
                            if (!ScrapeFollowingUser.Contains(user_name))
                            {
                                if (ScrapeFollowingUser.Count < noOfUserToScrape)
                                {
                                    ScrapeFollowingUser.Add(user_name);
                                    ScrapeFollowingUser = ScrapeFollowingUser.Distinct().ToList();

                                    try
                                    {
                                        GlobusLogHelper.log.Info("Scraped===>" + user_name);
                                        string CSVData = usernmeToScrape.Replace(",", string.Empty) + "," + user_name.Replace(",", string.Empty);
                                        GlobusFileHelper.ExportDataCSVFile(CSVHeader_following, CSVData, CSVPath_following);
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
                                    }

                                    try
                                    {
                                        DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + user_name + "')", "ScrapedUsername");
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
                                    }
                                    // GlobusLogHelper.log.Info("Scraped===>" + user_name);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (follow_respo.Contains("next_url"))
                    {
                        value = true;
                        while (value)
                        {
                            if (follow_respo.Contains("next_url") && noOfUserToScrape > ScrapeFollowingUser.Count())
                            {
                                string next_pageurl_token = Utils.getBetween(follow_respo, "next_cursor\":\"", "\"},");
                                string page_Url = postdata + "&cursor=" + next_pageurl_token;
                                follow_respo = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(page_Url), "http://iconosquare.com/viewer.php");
                                string[] data1 = Regex.Split(follow_respo, "username");
                                foreach (string item in data1)
                                {
                                    if (item.Contains("profile_picture"))
                                    {
                                        string follower = Utils.getBetween(item, ":\"", "\"");
                                        if (!ScrapeFollowingUser.Contains(follower))
                                        {
                                            if (ScrapeFollowingUser.Count < noOfUserToScrape)
                                            {
                                                ScrapeFollowingUser.Add(follower);
                                                ScrapeFollowingUser = ScrapeFollowingUser.Distinct().ToList();
                                                try
                                                {
                                                    GlobusLogHelper.log.Info("Scraped===>" + follower);
                                                    string CSVData = usernmeToScrape.Replace(",", string.Empty) + "," + follower.Replace(",", string.Empty);
                                                    GlobusFileHelper.ExportDataCSVFile(CSVHeader_following, CSVData, CSVPath_following);
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusLogHelper.log.Info("Error ==> " + ex.StackTrace);
                                                }

                                                try
                                                {
                                                    DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + follower + "')", "ScrapedUsername");
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
                                                }
                                                // GlobusLogHelper.log.Info("Scraped===>" + username);
                                            }
                                            else
                                            {
                                                break;
                                            }
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
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
                }
                finally
                {
                    GlobusLogHelper.log.Info("------Scraping Following Done------");
                }
            }
        }

        public void startUserScraper(string itemHash, ref InstagramUser obj_scrapeuser)
        {
            if (isStopScrapeUser)
            {
                return;
            }         

            try
            {
                lstofThreadScrapeUser.Add(Thread.CurrentThread);
                lstofThreadScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            string pageSource = string.Empty;
            string response = string.Empty;
            string postData = string.Empty;
            List<string> lstCountScrapUser = new List<string>();
            string test = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
            if (isStopScrapeUser) return;
            try
            {
                GlobusHttpHelper _GlobusHttpHelper = new GlobusHttpHelper();
                string Home_icon_Url = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                string PPagesource = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                string responce_icon = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");
                if (!string.IsNullOrEmpty(responce_icon))
                {

                    string url = "http://iconosquare.com/viewer.php#/search/" + itemHash;
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
                    string viewer_responce = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com/viewer.php "), "");
                    string crs_token = Utils.getBetween(viewer_responce, " <div id=\"accesstoken\" style=\"display:none;\">", "</div>");
                    response = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");

                    string postdata = "http://iconosquare.com/rqig.php?e=/users/search&a=ico2&t=" + crs_token + "&q=" + itemHash;
                    string respon_scrapeuser = obj_scrapeuser.globusHttpHelper.getHtmlfromUrl(new Uri(postdata), referer);
                    string[] data_divided = Regex.Split(respon_scrapeuser, "username");
                    foreach (string var in data_divided)
                    {
                        if (var.Contains("profile_picture"))
                        {
                            string User_list = Utils.getBetween(var, "\":\"", "\"");
                            if (lstCountScrapUser.Count < noOfUserToScrape)
                            {
                                lstCountScrapUser.Add(User_list);
                                lstCountScrapUser = lstCountScrapUser.Distinct().ToList();
                                try
                                {
                                    DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + User_list + "')", "ScrapedUsername");
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                                }                       

                            }
                        }
                    }
                    #region Export

                    foreach (string itre in lstCountScrapUser)
                    {
                        string Username = itre;
                        string Userlink = "https://www.instagram.com/" + itre + "/";
                        string itemhashdata = itemHash;
                        try
                        {
                            string CSVData = itemhashdata + "," + Username + "," + Userlink;
                            GlobusFileHelper.ExportDataCSVFile(CSVHeader, CSVData, CSVPath);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                        }
                        try
                        {
                            DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,UserName,UserLink, Status) values('" + "ScrapeUser" + "','" + obj_scrapeuser.username + "','" + DateTime.Now + "','" + Username + "','" + Userlink + "','" + "Scraped" + "')", "tbl_AccountReport");
                            GlobusLogHelper.log.Info("[" + Username + "," + "itemHash:" + "," + itemhashdata + "," + "userName:" + "," + Username + "," + "userLink:" + "," + Userlink + "]");
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                        }

                    }




                    #endregion
                    // code is in middle .......

                    if (!string.IsNullOrEmpty(response))
                    {
                        if (url.Contains("search"))
                        {
                            if (response.Contains("class=\"username\""))
                            {
                                try
                                {
                                    string[] arrOfUserName = Regex.Split(response, "class=\"username\"");

                                    if (arrOfUserName.Length > 0)
                                    {
                                        arrOfUserName = arrOfUserName.Skip(1).ToArray();
                                        foreach (string itemArray in arrOfUserName)
                                        {
                                            if (isStopScrapeUser) return;
                                            try
                                            {
                                                string startString = "href=\"/n/";
                                                string endString = "\">";
                                                if (itemArray.Contains(startString) && itemArray.Contains(endString))
                                                {
                                                    string userName = string.Empty;
                                                    try
                                                    {
                                                        userName = Utils.getBetween(itemArray, startString, endString).Replace("class=\"profimg", "").Replace("\"", "");
                                                        lstCountScrapUser.Add(userName);
                                                        lstCountScrapUser = lstCountScrapUser.Distinct().ToList();
                                                        string itemhashdata = itemHash;
                                                        string Username = userName.Replace("class=\"profimg", "").Replace("\"", "");
                                                        string UserLink = ("http://websta.me/n/" + userName).Replace(" class=\"profimg", "").Replace("\"", "");

                                                        try
                                                        {
                                                            DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + Username + "')", "ScrapedUsername");
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                                                        } 


                                                        if (!string.IsNullOrEmpty(userName))
                                                        {
                                                            #region CSV Write
                                                            try
                                                            {

                                                                Duplicatedatascrape.Add(userName, UserLink);

                                                                string CSVData = itemhashdata + "," + Username + "," + UserLink;
                                                                GlobusFileHelper.ExportDataCSVFile(CSVHeader, CSVData, CSVPath);
                                                            }

                                                            catch { }
                                                            try
                                                            {
                                                                DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,UserName,UserLink, Status) values('" + "ScrapeUser" + "','" + obj_scrapeuser.username + "','" + DateTime.Now + "','" + Username + "','" + UserLink + "','" + "Scraped" + "')", "tbl_AccountReport");
                                                                GlobusLogHelper.log.Info("[" + Username + "," + "itemHash:" + "," + itemhashdata + "," + "userName:" + "," + Username + "," + "userLink:" + "," + UserLink + "]");
                                                                //objclasssforlogger2.AddToLogger_Scrape_Bykey("[ " + Username + "," + "itemHash:" + "," + itemhashdata + "," + "userName:" + "," + Username + "," + "userLink:" + "," + UserLink + "] ");
                                                            }

                                                            catch { };
                                                            #endregion

                                                            try
                                                            {
                                                                if (isStopScrapeUser) return;

                                                                try
                                                                {

                                                                    GlobusLogHelper.log.Info("=> [  UserName" + Username + " ]");
                                                                    
                                                                    Random obj_rn = new Random();
                                                                    int delay1 = RandomNumberGenerator.GenerateRandom(minDelayScrapeUser, maxDelayScrapeUser);
                                                                    delay1 = obj_rn.Next(minDelayScrapeUser, maxDelayScrapeUser);
                                                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay1 + " Seconds ]");
                                                                    Thread.Sleep(delay1 * 1000);


                                                                }
                                                                catch { }

                                                                if (lstCountScrapUser.Count >= noOfUserToScrape)
                                                                {
                                                                    return;
                                                                }
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                    catch { }
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            try
                            {
                                if (response.Contains("div class=\"userinfo clearfix"))
                                {
                                    string[] splites_User = Regex.Split(response, "div class=\"userinfo clearfix");
                                    foreach (string item in splites_User)
                                    {
                                        if (!item.Contains("<!DOCTYPE html>"))
                                        {
                                            try
                                            {
                                                string User_Url = Utils.getBetween(item, "<a href=\"", "\"");
                                                User_Url = (IGGlobals.Instance.IGWEPME + User_Url);
                                                string User_Name = Utils.getBetween(item, "class=\"username\">", "</a>");
                                                lstCountScrapUser.Add(User_Name);
                                                lstCountScrapUser = lstCountScrapUser.Distinct().ToList();

                                                try
                                                {
                                                    DataBaseHandler.InsertQuery("insert into ScrapedUsername(Username) values('" + User_Name + "')", "ScrapedUsername");
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                                                } 

                                                if (!string.IsNullOrEmpty(User_Name))
                                                {
                                                    #region CSV Write
                                                    try
                                                    {

                                                        Duplicatedatascrape.Add(User_Name, User_Url);

                                                        string CSVData = itemHash + "," + User_Name + "," + User_Url;
                                                        GlobusFileHelper.ExportDataCSVFile(CSVHeader, CSVData, CSVPath);
                                                    }

                                                    catch { }
                                                    try
                                                    {

                                                        GlobusLogHelper.log.Info("[" + User_Name + "," + "itemHash:" + "," + itemHash + "," + "userName:" + "," + User_Name + "," + "userLink:" + "," + User_Url + "]");
                                                        //objclasssforlogger2.AddToLogger_Scrape_Bykey("[ " + Username + "," + "itemHash:" + "," + itemhashdata + "," + "userName:" + "," + Username + "," + "userLink:" + "," + UserLink + "] ");
                                                    }

                                                    catch { };
                                                    #endregion
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                                            }
                                        }

                                        if (lstCountScrapUser.Count >= noOfUserToScrape)
                                        {
                                            return;
                                        }
                                        //if (minDelayScrapeUser != 0)
                                        //{
                                        //    mindelay = minDelayScrapeUser;
                                        //}
                                        //if (maxDelayScrapeUser != 0)
                                        //{
                                        //    maxdelay = maxDelayScrapeUser;
                                        //}

                                        Random obj_rn = new Random();
                                        int delay1 = RandomNumberGenerator.GenerateRandom(minDelayScrapeUser, maxDelayScrapeUser);
                                        delay1 = obj_rn.Next(minDelayScrapeUser, maxDelayScrapeUser);
                                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay1 + " Seconds ]");
                                        Thread.Sleep(delay1 * 1000);

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                            }
                        }
                    }
                    else
                    {
                        //Do Nothing yet
                    }

                }
            }
            catch { }

        }

        public void startScrapeImageFromHashtag(ref InstagramUser obj_GDUSER)
        {
            if (isStopScrapeUser)
            {
                return;
            }
            try
            {
                lstofThreadScrapeUser.Add(Thread.CurrentThread);
                lstofThreadScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            string username = usernmeToScrape;
            int temp = noOfPhotoToScrape;
            bool value = true;
            List<string> List_PhotoUrl = new List<string>();
            try
            {
                string url = "https://www.instagram.com/explore/tags/" + username + "/";
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
                        string imageUrl = string.Empty;
                        string imageSrc = string.Empty;
                        if (arr.Length > 1)
                        {
                            arr = arr.Skip(1).ToArray();
                            foreach (string itemarr in arr)
                            {
                                try
                                {

                                    if (itemarr.Contains("date"))
                                    {
                                        imageId = Utils.getBetween(itemarr, "\":\"", "\"");
                                        imageUrl = "https://www.instagram.com/p/" + imageId + "/";
                                        if (!string.IsNullOrEmpty(imageId))
                                        {
                                            if (temp > List_PhotoUrl.Count())
                                            {

                                                List_PhotoUrl.Add(imageId);
                                                List_PhotoUrl = List_PhotoUrl.Distinct().ToList();

                                                try
                                                {
                                                    DataBaseHandler.InsertQuery("insert into ScrapedImage(Username,ImageURL,ImageID) values('" + username + "','" + imageUrl + "','" + imageId + "') ", "ScrapedImage");
                                                }
                                                catch(Exception ex)
                                                {

                                                }

                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }

                            if (pageSource.Contains("has_next_page\":true"))
                            {
                                while (value)
                                {
                                    if (pageSource.Contains("has_next_page\":true") && List_PhotoUrl.Count < temp)
                                    {
                                        string IDD = Utils.getBetween(pageSource, "\"id\":\"", "\"");
                                        string code_ID = Utils.getBetween(pageSource, "end_cursor\":\"", "\"");
                                        string postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                        pageSource = objInstagramUser.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, "https://www.instagram.com/" + username, token);
                                        string[] data1 = Regex.Split(pageSource, "code");
                                        foreach (string val in data1)
                                        {
                                            if (val.Contains("date"))
                                            {
                                                string photo_codes = Utils.getBetween(val, "\":\"", "\"");
                                                imageUrl = "https://www.instagram.com/p/" + photo_codes + "/";
                                                if (temp > List_PhotoUrl.Count())
                                                {
                                                    List_PhotoUrl.Add(photo_codes);
                                                    List_PhotoUrl = List_PhotoUrl.Distinct().ToList();
                                                    try
                                                    {
                                                        DataBaseHandler.InsertQuery("insert into ScrapedImage(Username,ImageURL,ImageID) values('" + username + "','" + imageUrl + "','" + photo_codes + "') ", "ScrapedImage");
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                                else
                                                {

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
                }
            }
            catch { }


        }

        public void startScrapePhotoUrlByHashtag(ref InstagramUser obj_GDUSER)
        {
            if (isStopScrapeUser)
            {
                return;
            }
            try
            {
                lstofThreadScrapeUser.Add(Thread.CurrentThread);
                lstofThreadScrapeUser.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            string username = "Tree";
            int temp = 50;
            bool value = true;
            List<string> List_PhotoUrl = new List<string>();
            try
            {
                string url = "https://www.instagram.com/explore/tags/" + username + "/";
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

                                    if (itemarr.Contains("date"))
                                    {
                                        imageId = Utils.getBetween(itemarr, "\":\"", "\"");
                                        imageId = "https://www.instagram.com/p/" + imageId + "/";
                                        if (!string.IsNullOrEmpty(imageId))
                                        {
                                            if (temp > List_PhotoUrl.Count())
                                            {

                                                List_PhotoUrl.Add(imageId);
                                                List_PhotoUrl = List_PhotoUrl.Distinct().ToList();

                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }

                            if (pageSource.Contains("has_next_page\":true"))
                            {
                                while (value)
                                {
                                    if (pageSource.Contains("has_next_page\":true") && List_PhotoUrl.Count < temp)
                                    {
                                        string IDD = Utils.getBetween(pageSource, "\"id\":\"", "\"");
                                        string code_ID = Utils.getBetween(pageSource, "end_cursor\":\"", "\"");
                                        string postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                        pageSource = objInstagramUser.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, "https://www.instagram.com/" + username, token);
                                        string[] data1 = Regex.Split(pageSource, "code");
                                        foreach (string val in data1)
                                        {
                                            if (val.Contains("date"))
                                            {
                                                string photo_codes = Utils.getBetween(val, "\":\"", "\"");
                                                photo_codes = "https://www.instagram.com/p/" + photo_codes + "/";
                                                if (temp > List_PhotoUrl.Count())
                                                {
                                                    List_PhotoUrl.Add(photo_codes);
                                                    List_PhotoUrl = List_PhotoUrl.Distinct().ToList();
                                                }
                                                else
                                                {

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
                }
            }
            catch { }


        }
    }

    public class MentionUser
    {
        #region Mention User Variable And Property

        public bool IsInsertUrlToMention = false;
        public bool IsUseScrapedurlToMention = false;
        public bool IsUploadUrlToMention = false;
        public bool IsNoOfUserToMention = false;
        public bool IsRandomNoUserMention = false;
        public bool IsStopMentionUser = false;
        public bool IsDailySchedule = false;
        
        public List<string> selectedAccountToScrape = new List<string>();

        public int noOfUserToMention = 0;
        public int noOfRandomNoUserToMention = 0;
        public int minDelayMentionUser = 10;
        public int maxDelayMentionUser = 20;
        public int NoOfThreadsMentionUser = 25;

        public string scheduleStartTime = string.Empty;
        public string scheduleEndTime = string.Empty;

        public List<Thread> listOfStopThreadMentionUser = new List<Thread>();
        public List<string> listOfUrlToMentionUser = new List<string>();
        public List<string> listOfMessageToComment = new List<string>();

        static readonly Object _lockObject = new Object();
        readonly object lockrThreadControlleScrapeUser = new object();
        readonly object lockrThreadControlleScrapeFollower = new object();


        int countThreadControllerScrapeUser = 0;
        int countThreadControllerScrapeFollower = 0;

        #endregion


        public void StartMentionUser()
        {

            try
            {
                countThreadControllerScrapeFollower = 0;
                try
                {
                    int numberOfAccountPatch = 25;

                    if (NoOfThreadsMentionUser > 0)
                    {
                        numberOfAccountPatch = NoOfThreadsMentionUser;
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
                                    string[] data = Regex.Split(account, ":");
                                    string Accout = data[0];


                                    if (selectedAccountToScrape.Contains(Accout))
                                   // if (Accout.Contains(selectedAccountToScrape))
                                    {
                                        lock (lockrThreadControlleScrapeFollower)
                                        {
                                            try
                                            {
                                                if (countThreadControllerScrapeFollower >= listAccounts.Count)
                                                {
                                                    Monitor.Wait(lockrThreadControlleScrapeFollower);
                                                }

                                                string acc = account.Remove(account.IndexOf(':'));

                                                //Run a separate thread for each account
                                                InstagramUser item = null;

                                                IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


                                                if (item != null)
                                                {
                                                    Thread profilerThread = new Thread(StartMultiThreadsMentionUser);
                                                    profilerThread.Name = "workerThread_Profiler_" + acc;
                                                    profilerThread.IsBackground = true;

                                                    profilerThread.Start(new object[] { item, });
                                                    //DS = item.ds;
                                                    countThreadControllerScrapeUser++;
                                                }
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
                            }
                        }
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
            finally
            {
                if (IsDailySchedule)
                {
                    try
                    {
                        DateTime d1 = DateTime.Parse(scheduleStartTime);
                        DateTime d2 = DateTime.Parse(scheduleEndTime);
                        TimeSpan t = d2 - DateTime.Now;

                        while (true)
                        {
                            if (d1.Hour == (DateTime.Now.Hour) && d1.Minute == (DateTime.Now.Minute))
                            {
                                GlobusLogHelper.log.Info("Scheduler Started With Time ==> " + d1.ToString());
                                StartMentionUser();
                                break;
                            }
                            //if (d2.Hour == (DateTime.Now.Hour) && d2.Minute == (DateTime.Now.Minute))
                            //{
                            //    GlobusLogHelper.log.Info("Scheduler Stopped With Time ==> " + d2.ToString());
                            //}
                            Thread.Sleep(15 * 1000);
                        }
                    }
                    catch (Exception ex)
                    {


                    }

                }
            }
        }

        public void StartMultiThreadsMentionUser(object parameters)
        {
            try
            {
                if (!IsStopMentionUser)
                {
                    try
                    {
                        listOfStopThreadMentionUser.Add(Thread.CurrentThread);
                        listOfStopThreadMentionUser.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                    try
                    {
                        string status = string.Empty;
                        Array paramsArray = new object[1];
                        paramsArray = (Array)parameters;

                        InstagramUser objInstagramUser = (InstagramUser)paramsArray.GetValue(0);

                        if (!objInstagramUser.isloggedin)
                        {
                            GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();

                            objInstagramUser.globusHttpHelper = objGlobusHttpHelper;

                            //Login Process

                            Accounts.AccountManager objAccountManager = new AccountManager();
                            status = objAccountManager.LoginUsingGlobusHttp(ref objInstagramUser);


                        }

                        if (objInstagramUser.isloggedin)
                        {

                            StartActionMentionUser(ref objInstagramUser);

                        }
                        else
                        {
                            GlobusLogHelper.log.Info("Couldn't Login With Username : " + objInstagramUser.username);
                            GlobusLogHelper.log.Debug("Couldn't Login With Username : " + objInstagramUser.username);
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
                        lock (lockrThreadControlleScrapeUser)
                        {
                            countThreadControllerScrapeUser--;
                            Monitor.Pulse(lockrThreadControlleScrapeUser);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }

        public void StartActionMentionUser(ref InstagramUser objInstagramUser)
        {
            try
            {
                try
                {
                    listOfStopThreadMentionUser.Add(Thread.CurrentThread);
                    listOfStopThreadMentionUser.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }

                if(IsInsertUrlToMention)
                {
                    if(listOfUrlToMentionUser.Count>0)
                    {
                        if (IsNoOfUserToMention)
                        {
                            if (noOfUserToMention > 0)
                            {
                                foreach (string url in listOfUrlToMentionUser)
                                {
                                    foreach (string msg in listOfMessageToComment)
                                    {
                                        try
                                        {
                                            string messageToComment = msg;
                                            DataSet Ds = DataBaseHandler.SelectQuery("select Username from ScrapedUsername  limit '" + noOfUserToMention + "' ", "ScrapedUsername");
                                            foreach (DataRow dr in Ds.Tables[0].Rows)
                                            {
                                                messageToComment = messageToComment + " @" + dr.ItemArray[0].ToString();

                                                DataBaseHandler.DeleteQuery("delete from ScrapedUsername where Username='" + dr.ItemArray[0].ToString() + "' ", "ScrapedUsername");
                                            }

                                            start_democomment(ref objInstagramUser, url, messageToComment);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error Random Select Username " + ex.Message);
                                        }
                                    }
                                }
                            }
                        }
                        else if (IsRandomNoUserMention)
                        {
                            if (noOfRandomNoUserToMention > 0)
                            {
                                foreach (string url in listOfUrlToMentionUser)
                                {
                                    foreach (string msg in listOfMessageToComment)
                                    {
                                        try
                                        {
                                            string messageToComment = msg;
                                            DataSet Ds = DataBaseHandler.SelectQuery("select Username from ScrapedUsername Order By random() limit '" + noOfRandomNoUserToMention + "' ", "ScrapedUsername");
                                            Random rnd = new Random();
                                            int randomNo = rnd.Next(1, noOfRandomNoUserToMention);
                                            DataSet dataSetRandom = DataBaseHandler.SelectQuery("select Username from '" + Ds.Tables[0] + "' Order By random() limit '" + randomNo + "' ", "ScrapedRandomUsername");
                                            foreach (DataRow dr in dataSetRandom.Tables[0].Rows)
                                            {
                                                messageToComment = messageToComment + " @" + dr.ItemArray[0].ToString();

                                                DataBaseHandler.DeleteQuery("delete from ScrapedUsername where Username='" + dr.ItemArray[0].ToString() + "' ", "ScrapedUsername");
                                            }

                                            start_democomment(ref objInstagramUser, url, messageToComment);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error Random Select Username " + ex.Message);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                else if(IsUseScrapedurlToMention)
                {
                    if (listOfUrlToMentionUser.Count > 0)
                    {
                        if (IsNoOfUserToMention)
                        {
                            if (noOfUserToMention > 0)
                            {
                                foreach (string url in listOfUrlToMentionUser)
                                {
                                    foreach (string msg in listOfMessageToComment)
                                    {
                                        try
                                        {
                                            string messageToComment = msg;
                                            DataSet Ds = DataBaseHandler.SelectQuery("select Username from ScrapedUsername  limit '" + noOfUserToMention + "' ", "ScrapedUsername");
                                            foreach (DataRow dr in Ds.Tables[0].Rows)
                                            {
                                                messageToComment = messageToComment + " @" + dr.ItemArray[0].ToString();

                                                DataBaseHandler.DeleteQuery("delete from ScrapedUsername where Username='" + dr.ItemArray[0].ToString() + "' ", "ScrapedUsername");
                                            }

                                            start_democomment(ref objInstagramUser, url, messageToComment);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error Random Select Username " + ex.Message);
                                        }
                                    }
                                }
                            }
                        }
                        else if (IsRandomNoUserMention)
                        {
                            if (noOfRandomNoUserToMention > 0)
                            {
                                foreach (string url in listOfUrlToMentionUser)
                                {
                                    foreach (string msg in listOfMessageToComment)
                                    {
                                        try
                                        {
                                            string messageToComment = msg;
                                            DataSet Ds = DataBaseHandler.SelectQuery("select Username from ScrapedUsername Order By random() limit '" + noOfRandomNoUserToMention + "' ", "ScrapedUsername");
                                            Random rnd = new Random();
                                            int randomNo = rnd.Next(1, noOfRandomNoUserToMention);
                                            DataSet dataSetRandom = DataBaseHandler.SelectQuery("select Username from '" + Ds.Tables[0] + "' Order By random() limit '" + randomNo + "' ", "ScrapedRandomUsername");
                                            foreach (DataRow dr in dataSetRandom.Tables[0].Rows)
                                            {
                                                messageToComment = messageToComment + " @" + dr.ItemArray[0].ToString();

                                                DataBaseHandler.DeleteQuery("delete from ScrapedUsername where Username='" + dr.ItemArray[0].ToString() + "' ", "ScrapedUsername");
                                            }

                                            start_democomment(ref objInstagramUser, url, messageToComment);
                                        }
                                        catch(Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error Random Select Username " + ex.Message);
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                else if(IsUploadUrlToMention)
                {
                    if (listOfUrlToMentionUser.Count > 0)
                    {
                        if (IsNoOfUserToMention)
                        {
                            if (noOfUserToMention > 0)
                            {
                                foreach (string url in listOfUrlToMentionUser)
                                {
                                    foreach (string msg in listOfMessageToComment)
                                    {
                                        try
                                        {
                                            string messageToComment = msg;
                                            DataSet Ds = DataBaseHandler.SelectQuery("select Username from ScrapedUsername  limit '" + noOfUserToMention + "' ", "ScrapedUsername");
                                            foreach (DataRow dr in Ds.Tables[0].Rows)
                                            {
                                                messageToComment = messageToComment + " @" + dr.ItemArray[0].ToString();

                                                DataBaseHandler.DeleteQuery("delete from ScrapedUsername where Username='" + dr.ItemArray[0].ToString() + "' ", "ScrapedUsername");
                                            }

                                            start_democomment(ref objInstagramUser, url, messageToComment);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error Random Select Username " + ex.Message);
                                        }
                                    }
                                }
                            }
                        }
                        else if (IsRandomNoUserMention)
                        {
                            if (noOfRandomNoUserToMention > 0)
                            {
                                foreach (string url in listOfUrlToMentionUser)
                                {
                                    foreach (string msg in listOfMessageToComment)
                                    {
                                        try
                                        {
                                            string messageToComment = msg;
                                            DataSet Ds = DataBaseHandler.SelectQuery("select Username from ScrapedUsername Order By random() limit '" + noOfRandomNoUserToMention + "' ", "ScrapedUsername");
                                            Random rnd = new Random();
                                            int randomNo = rnd.Next(1, noOfRandomNoUserToMention);
                                            DataSet dataSetRandom = DataBaseHandler.SelectQuery("select Username from '" + Ds.Tables[0] + "' Order By random() limit '" + randomNo + "' ", "ScrapedRandomUsername");
                                            foreach (DataRow dr in dataSetRandom.Tables[0].Rows)
                                            {
                                                messageToComment = messageToComment + " @" + dr.ItemArray[0].ToString();

                                                DataBaseHandler.DeleteQuery("delete from ScrapedUsername where Username='" + dr.ItemArray[0].ToString() + "' ", "ScrapedUsername");
                                            }

                                            start_democomment(ref objInstagramUser, url, messageToComment);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error Random Select Username " + ex.Message);
                                        }
                                    }

                                }
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

        public void start_democomment(ref InstagramUser obj_GDUSER,string commentUrl,string Message)
        {
            try
            {
                string commenturl = commentUrl;// "https://www.instagram.com/p/BAIlg1_kPRi/";
                string messsage = Message; // "Hello";
                string FollowedPageSource = string.Empty;

                try
                {
                    string res_secondURL = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                    string CommentIdlink = string.Empty;
                    string commentIdLoggedInLink = string.Empty;
                    if (commenturl.Contains("https://www.instagram.com/p/"))
                    {
                        commenturl = commenturl.Replace(IGGlobals.Instance.IGhomeurl, string.Empty);
                    }

                    if (!commenturl.Contains("https://www.instagram.com/p/"))
                    {
                        try
                        {

                            CommentIdlink = "https://www.instagram.com/p/" + commenturl + "/";

                            commentIdLoggedInLink = "https://www.instagram.com/p/" + commenturl;
                        }
                        catch (Exception ex)
                        {
                            // ex.Message;
                        }
                    }

                    #region Change

                    string url = commenturl;
                    string resp_photourl = obj_GDUSER.globusHttpHelper.getHtmlfromUrl(new Uri(commenturl), "");
                    string Cooment_ID = Utils.getBetween(resp_photourl, "content=\"instagram://media?id=", " />").Replace("\"", "");
                    string postdata_url = "https://www.instagram.com/web/comments/" + Cooment_ID + "/add/";
                    string poatdata = "comment_text=" + messsage;
                    string token = Utils.getBetween(resp_photourl, "csrf_token\":\"", "\"");

                    try
                    {
                        FollowedPageSource = obj_GDUSER.globusHttpHelper.postFormDatainta(new Uri(postdata_url), poatdata, "https://www.instagram.com/", token);
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
                            //   ex.Message;
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
                                //  ex.Message;
                            };
                        }
                    }
                    #endregion
                    if(FollowedPageSource.Contains("Success"))
                    {
                        GlobusLogHelper.log.Info("Comment Has Been Succefully Done In Url => " + url + "And Message Is " + messsage);
                    }

                }
                catch { }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
            }
        }

    }
}
