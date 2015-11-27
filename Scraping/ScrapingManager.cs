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
        public static string selected_Account = string.Empty;
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
                if(UserScraper_Username == true)
                {
                    Start_scraping(ref fbUser);
                }
                if(UserScraper_UserByKeyword==true)
                {
                  scrapeUesr_Key(User_key);
                   // tried(ref fbUser);
                }
                if(ScrapFollower == true)
                {
                    StartFollowerScrape(ref fbUser);
                }
                if(ImageScarpe == true)
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

                string res_secondURL = obj_DwonloadImange.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/login"), "");
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

        public void DownloadingImage(object obj_Scrape)
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
            InstagramUser obj_Insta = (InstagramUser)obj_Scrape;
            try
            {              
                try
                {
                    
                    string res_secondURL = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/login"), "");
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
                pageSource = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/"), "");

            }
            catch { }
            if (!string.IsNullOrEmpty(pageSource))
            {
                if (itemImageTag.Contains("#"))
                {
                    itemImageTag = itemImageTag.Replace("#", "");
                    input_itemImage = "#"+itemImageTag;
                }
                else
                {
                    input_itemImage =  itemImageTag;
                }
                // string url = mainUrl + "tag/" + itemImageTag;
                string url = "http://websta.me/" + "search/" + itemImageTag;
                try
                {
                    pageSource = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri(url), "");

                }
                catch { }
                if (!string.IsNullOrEmpty(pageSource))
                {
           
                    if (pageSource.Contains("class=\"username\""))
                    {
                        try
                        {                            
                            string[] arr = Regex.Split(pageSource, "class=\"username\"");
                            if (arr.Length > 1)
                            {
                                arr = arr.Skip(1).ToArray();
                                foreach (var item in arr)
                                {
                                    string imageid = string.Empty;
                                    string FullName = string.Empty;
                                    string websiteLink = Utils.getBetween(item, " href=\"/", "\"");
                                    websiteLink = "http://websta.me/" + websiteLink;
                                    string resp_Profile = obj_Insta.globusHttpHelper.getHtmlfromUrl(new Uri(websiteLink), "</div>");
                                    string profile = Utils.getBetween(resp_Profile, "<div class=\"profbox\">", "</div>");
                                    string imageLink = Utils.getBetween(profile, "<img src=\"", "\"");
                                     FullName = Utils.getBetween(profile, "<h2 class=\"fullname-headline\">", "</h2>");
                                    if (FullName.Contains("<span class"))
                                    {
                                        FullName = Utils.getBetween(profile, "<h2 class=\"fullname-headline\">", "<span class");
                                    }
                                    try
                                    {
                                        string[] spilt_imagelink = Regex.Split(imageLink, "/");
                                         imageid = spilt_imagelink[5].Replace("_a.jpg", "");
                                    }
                                    catch (Exception)
                                    {
                                        GlobusLogHelper.log.Info("Image is Not There");
                                         imageid = "Null";
                                    }                                    
                                    lstCountScrape.Add(imageLink);
                                    lstCountScrape = lstCountScrape.Distinct().ToList();

                                    if (isStop_ScrapeImage) return;
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(imageLink))
                                        {
                                            duplicatlink.Add(imageLink, imageid);
                                            string CSVData = input_itemImage.Replace(",", string.Empty) + "," + imageLink.Replace(",", string.Empty) + "," + websiteLink.Replace(",", string.Empty) + "," + FullName.Replace(",", string.Empty);
                                            //string CSVData = websiteLink.Replace(",", string.Empty) + "," + imageLink.Replace(",", string.Empty) + "," + imageid.Replace(",", string.Empty) + "," + FullName.Replace(",", string.Empty);
                                            GlobusFileHelper.ExportDataCSVFile(CSVHeader_Imageurl, CSVData, CSVPath_Imageurl);
                                            if (lstCountScrape.Count >= Number_ScrapeImage)
                                            {
                                                return;
                                            }
                                        }
                                    }
                                    catch { }
                                    if (isStop_ScrapeImage) return;
                                    try
                                    {
                                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + websiteLink + " ]");

                                        if (minDelay_ScrapeImage != 0 )
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

                            if (lstCountScrape.Count >= Number_ScrapeImage)
                            {
                                return;
                            }

                        }
                        catch { };
                    }
                }
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
                                    if (Accout.Contains(selected_Account))
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

                                                    profilerThread.Start(new object[] { item,});
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
            catch(Exception ex)
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
            string res_secondURL = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/login"), "");
            try
            {
                ClGlobul.switchAccount = false;

               
                string usernameUrl = "http://websta.me/n/" + Username_ScrapFollower;
                try
                {
                    usernamePgSource = obj_folowerscrape.globusHttpHelper.getHtmlfromUrl(new Uri(usernameUrl), "");
                }
                catch { };
               

                rawUsernameID = Utils.getBetween(usernamePgSource, "<div class=\"userinfo\">", "</div>");
                string[] userInfoSplit = Regex.Split(rawUsernameID, "<li>");

                string rawFollowerUrl = Utils.getBetween(userInfoSplit[2], "<a href=\"", "\">");
                string rawFollowingUrl = Utils.getBetween(userInfoSplit[3], "<a href=\"", "\">");


                string usernameFollowerUrl = "http://websta.me" + rawFollowerUrl;
                string usernameFollowingUrl = "http://websta.me" + rawFollowingUrl;

                string usernameFollowerCount = Utils.getBetween(userInfoSplit[2], "class=\"counts_followed_by\">", "</span>");
                string usernameFollowingCount = Utils.getBetween(userInfoSplit[3], "class=\"following\">", "</span>");


                GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Scraping Followers ]");
                ScrapeFollowerUrl(ref obj_folowerscrape, usernameFollowerUrl, Username_ScrapFollower);

                #region LoadCombobox
                
                DataSet DS = DataBaseHandler.SelectQuery("select distinct username from tb_scrape_follower", "tb_scrape_follower");
                obj_folowerscrape.ds=DS;                
                #endregion

            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (!ClGlobul.isStopScrapeFollowers)
                {
                    DataBaseHandler.UpdateQuery("update manage_time set process_status='yes' where process_status='no'", "manage_time");
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] " + "[ Switching onto next account. ]");
                    ClGlobul.switchAccount = true;

                }
                if (ClGlobul.userOverFollower)
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[ Scraped all users who follows " + Username_ScrapFollower + " ]");
                }
                if (ClGlobul.userOverFollowing)
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[ Scraped all users who is followed by " + Username_ScrapFollower + " ]");
                }
                ClGlobul.oneHourProcessCompleted = true;
            }
        }

        public void ScrapeFollowerUrl(ref InstagramUser accountManager, string usernameFollowerUrl, string username)
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

            List<string> listFollowers = new List<string>();
            try
            {
                string followerListPgSource = string.Empty;
                DataSet DS = DataBaseHandler.SelectQuery("select url from tb_follower_url where username ='" + username + "' and used='no'", "tb_follower_url");
                if (DS.Tables[0].Rows.Count == 0)
                {
                    followerListPgSource = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(usernameFollowerUrl), "");
                }
                else
                {
                    usernameFollowerUrl = DS.Tables[0].Rows[0]["url"].ToString();
                    followerListPgSource = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(usernameFollowerUrl));
                    DataBaseHandler.UpdateQuery("update tb_follower_url set used='yes' where used='no'", "tb_follower_url");
                }

                

               

                #region already Following
                //string FollowerName = followingList_item;
                string HomeAcc_Url = usernameFollowerUrl;
                string res_data = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(HomeAcc_Url), "");
                string Data = Utils.getBetween(res_data, "ul class=\"list-inline user", "</ul>");
                string Data1 = Utils.getBetween(Data, "<a href=\"/follows/", "\"><span class=");
                string following_url = "http://websta.me/followed-by/" + Data1;
                string res_data1 = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(following_url), "");
                DataBaseHandler.InsertQuery("insert into tb_follower_url (url, username, used) values ('" + following_url + "','" + username + "','" + "no" + "')", "tb_follower_url");
                string res_data2 = Utils.getBetween(res_data1, "<ul class=\"userlist\">", "</ul>");
                string[] split_data = Regex.Split(res_data2, "<li>");
                foreach (string item in split_data)
                {
                    if (item.Contains(" href"))
                    {
                        string user_following = Utils.getBetween(item, "a href=\"/n/", "\"");
                        listFollowers.Add(user_following);
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[Follower UserName : " + user_following + ". ]");
                    }
                }
                if (res_data1.Contains("Next Page"))
                {
                    value = true;
                    while (value)
                    {
                        if (res_data1.Contains("Next Page"))
                        {
                            string nextpage_Url = Utils.getBetween(res_data1, "<ul class=\"pager nm\"", "</ul>");
                            string[] page_split = Regex.Split(nextpage_Url, "<a href=");
                            string next = Utils.getBetween(page_split[2], "\"", "\"> Next Page");
                            string finalNext_FollowingURL = "http://websta.me" + next;
                            //Page_Url.Add(finalNext_FollowingURL);
                            res_data1 = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(finalNext_FollowingURL), "");
                            string res_data3 = Utils.getBetween(res_data1, "<ul class=\"userlist\">", "</ul>");
                            string[] split_data4 = Regex.Split(res_data3, "<li>");
                            foreach (string item in split_data4)
                            {
                                if (item.Contains(" href"))
                                {
                                    string user_following = Utils.getBetween(item, "a href=\"/n/", "\"");
                                    listFollowers.Add(user_following);
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[Follower UserName : " + user_following + ". ]");
                                }
                            }
                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[Total No Of Follower  : " + listFollowers.Count + ". ]");
                            value = false;
                        }
                    }
                }
                #endregion

                #region Commented-Pagination

                #endregion

                listFollowers = listFollowers.Distinct().ToList();


                foreach (string followerName in listFollowers)
                {
                    string followerUrl = "http://websta.me/n/" + followerName;
                    ScrapeFollowerDetails(ref accountManager, followerUrl, usernameFollowerUrl);

                }


            }
            catch (Exception ex)
            {
                
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

                string followerUsername = Utils.getBetween(followerUrl + "@", ".me/n/", "@");

                if (followerPageSource.Contains("This user is private."))
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[ Data can not be scraped of the  private user with user name : " + followerUsername + ". ]");
                    return;
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + "] => " + "[ Scraping detail from profile with username-" + followerUsername + " ]");
                string rawFollowerInfo = Utils.getBetween(followerPageSource, "<div class=\"userinfo\">", "</div>");
                string[] infoSplit = Regex.Split(rawFollowerInfo, "<li>");

                string subFollowerCount = Utils.getBetween(infoSplit[2], "<span class=\"counts_followed_by\">", "</span>");
                string subFollowingCount = Utils.getBetween(infoSplit[3], "<span class=\"following\">", "</span>");


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
                string pictureCount = Utils.getBetween(followerPageSource, "\"counts_media\">", "</span>");

                try
                {
                    latestPostPageResponse = accountManager.globusHttpHelper.getHtmlfromUrl(new Uri(latestPostUrl), followerUrl);
                }
                catch { };
                try
                {
                    uTimestamp = Convert.ToDouble(Utils.getBetween(latestPostPageResponse, "data-utime=\"", "\">"));
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
                string[] Picture_Split = Regex.Split(followerPageSource, "<div class=\"mainimg_wrapper\">");
                foreach (string picture in Picture_Split)
                {
                    if (!picture.Contains("<!DOCTYPE html>"))
                    {
                        if (!picture.Contains("fancy-video"))
                        {
                            if (!enterOnce)
                            {
                                rawLatestPostUrl = Utils.getBetween(picture, "<a href=\"", "\"");
                                latestPostUrl = "http://websta.me" + rawLatestPostUrl;
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
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Scraping Started for User Ids ]");
                        try
                        {
                            Thread _ThreadHashStart = new Thread(ScrapUserName);
                            _ThreadHashStart.Start();
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
        public void ScrapUserName()
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

                foreach (string itemHash in ClGlobul.HashTagForScrap)
                {
                    startUserScraper(itemHash);

                    Thread.Sleep(5000);
                }

                //GlobusLogHelper.log.Info("[" + DateTime.Now + " ]=>[Process Completed]");
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Process Completed ]");
            }
            catch { }
        }
        public void startUserScraper(string itemHash)
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

            if (stopScrapBool) return;
            try
            {
                GlobusHttpHelper _GlobusHttpHelper = new GlobusHttpHelper();
                pageSource = _GlobusHttpHelper.getHtmlfromUrl(new Uri("http://websta.me/"), "");
                
                if (!string.IsNullOrEmpty(pageSource))
                {
                   
                    string url = string.Empty;
                    postData = "q=" + Uri.EscapeDataString(itemHash);
                    if (!itemHash.Contains("#"))
                    {
                         url = "http://websta.me/search/" + postData.Substring(postData.IndexOf("=") + 1);
                        
                    }
                    else
                    {
                        url = "http://websta.me/tag/" + postData.Substring(postData.IndexOf("=") + 1).Replace("%23","");
                    }
                   
                    string referer = "http://websta.me/";
                    response = _GlobusHttpHelper.postFormData(new Uri(url), postData, referer, "");

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
                                                User_Url = ("http://websta.me/" + User_Url);
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
        public void scrapeUesr_Key(string Key)
        {

            GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();

            int total_page = 0;
            string Main_url = "http://websta.me/keyword/" + Key;
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
                    string url = "http://websta.me/keyword/" + Key + "?&page=" + i;
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

       

    }

    }

