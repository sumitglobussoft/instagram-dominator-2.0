using Accounts;
using BaseLib;
using BaseLibID;
using Globussoft;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Photo
{
    public class PhotoManager
    {

        #region Global Variables For PhotoLike by ID

        static readonly Object _lockObject = new Object();
        readonly object lockrThreadControlleLikePoster = new object();
        public bool isStopLikePoster = false;
        public bool isStopLikerPoster = false;
        public bool unlike = false;
        public bool Like = false;
        public bool liker_photo = false;
        public bool DownloadPhoto = false;
        public bool useOriginalMessage = true;
        int countThreadControllerLikePoster = 0;
        public static int TotalNoOfLikePosterCounter = 0;
        public static int messageCountLikePoster = 0;
        int countLikePoster = 1;
        public static string LikePhoto_ID = string.Empty;
        //public static string message_like = string.Empty;
        public static string LikePhoto_ID_path = string.Empty;
        // public static string message_Like_path = string.Empty;
        public static string UnLikePhoto_ID = string.Empty;
        public static string UnLikePhoto_ID_Path = string.Empty;
        public static int Nothread_LikePoster = 0;
        public static string LikePhoto_Username = string.Empty;
        public static string LikePhoto_username_path = string.Empty;
        public static int noPhotoLike_username = 0;

        public List<Thread> lstThreadsLikePoster = new List<Thread>();
        public List<Thread> lstThreadsDwonloadPoster = new List<Thread>();
        public List<string> lstCommentPostURLsLikePoster = new List<string>();
        public List<string> lstLikePostURLsTitles = new List<string>();
        public List<Thread> lstThreadsLikerPoster = new List<Thread>();
        public static int no_photounlike_Username = 0;
        public static string UnLikePhoto_UserName = string.Empty;
        public static string UnLikePhoto_Username_Path = string.Empty;

        public static int minDelayLikePoster = 10;
        public static int maxDelayLikePoster = 20;
        public static string status = string.Empty;
        public GlobusHttpHelper httpHelper = new GlobusHttpHelper();
        public ChilkatHttpHelpr chilkathttpHelper = new ChilkatHttpHelpr();
        public static int mindelay = 0;
        public static int maxdelay = 0;
        public static int minDelayUnLikePoster = 0;
        public static int maxDelayUnLikePoster = 0;
        public static int Nothread_UnLikePoster = 0;
        public bool isStopDwonloadPoster = true;
        public static int no_photo_Download = 0;
        public static string UserphotoDownload_Single = string.Empty;
        public static string UserphotoDownload_Multiple = string.Empty;
        public static bool IsDownLoadImageUsingHashTag = false;
        public static bool IsDownLoadImageUsingUserName = false;
        public static int minDelayDownloadPoster = 0;
        public static int maxDelayDownloadPoster = 0;
        public static int Nothread_DownloadPoster = 0;
        private static int unlikeCompletionCount = 0;
        private static bool _boolUnlike = false;
        public static int no_Photo_liker = 0;
        public static int NoUser_LikerPoster = 0;
        public static int minDelayLikerPoster = 0;
        public static int mixDelayLikerPoster = 0;
        public static string txt_username_Liker_single = string.Empty;
        public static string txt_username_Liker_Multiple = string.Empty;
        List<Thread> Lst_photoLikethread = new List<Thread>();
        public static string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator\\Downloaded_Image";
        public static string FileData = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\imageuploade\\LoggerError.txt";

        #endregion



        public int NoOfThreadsLikePoster
        {
            get;
            set;
        }





        public void StartLikePoster()
        {
            countThreadControllerLikePoster = 0;
            try
            {
                int numberOfAccountPatch = 25;
                if (NoOfThreadsLikePoster > 0)
                {
                    numberOfAccountPatch = NoOfThreadsLikePoster;
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
                                lock (lockrThreadControlleLikePoster)
                                {
                                    try
                                    {
                                        if (countThreadControllerLikePoster >= listAccounts.Count)
                                        {
                                            Monitor.Wait(lockrThreadControlleLikePoster);
                                        }

                                        string acc = account.Remove(account.IndexOf(':'));

                                        //Run a separate thread for each account
                                        InstagramUser item = null;
                                        IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);
                                        if (item != null)
                                        {
                                            Thread profilerThread = new Thread(StartMultiThreadsPhotoModule);
                                            profilerThread.Name = "workerThread_Profiler_" + acc;
                                            profilerThread.IsBackground = true;
                                            profilerThread.Start(new object[] { item });
                                            countThreadControllerLikePoster++;
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

        public void StartMultiThreadsPhotoModule(object parameters)
        {
            try
            {
                if (!isStopLikePoster)
                {
                    try
                    {
                        lstThreadsLikePoster.Add(Thread.CurrentThread);
                        lstThreadsLikePoster.Distinct();
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

                            StartActionLikePoster(ref objFacebookUser);
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
                        lock (lockrThreadControlleLikePoster)
                        {
                            countThreadControllerLikePoster--;
                            Monitor.Pulse(lockrThreadControlleLikePoster);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }

        private void StartActionLikePoster(ref InstagramUser fbUser)
        {
            try
            {
                if (Like == true)
                {
                    Start_LikePhoto_ID(ref fbUser);
                }
                if (unlike == true)
                {
                    getPhotoUnlike(ref fbUser);
                }
                if (DownloadPhoto == true)
                {
                    StartDownload_Photo(ref fbUser);
                }
                if (liker_photo == true)
                {
                    StartLiker(ref fbUser);
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }


        public void getPhotoUnlike(ref InstagramUser Unlike_ID)
        {
            try
            {
                lstThreadsLikePoster.Add(Thread.CurrentThread);
                lstThreadsLikePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
              //  ClGlobul.PhotoList.Clear();
                GlobusHttpHelper obj = Unlike_ID.globusHttpHelper;
           //     string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");

                if (string.IsNullOrEmpty(UnLikePhoto_ID_Path))
                {
                    if (!string.IsNullOrEmpty(UnLikePhoto_ID))
                    {
                        ClGlobul.PhotoList.Clear();

                        string s = UnLikePhoto_ID;

                        if (s.Contains(','))
                        {
                            string[] Data = s.Split(',');

                            foreach (var item in Data)
                            {


                                ClGlobul.PhotoList.Add(item);
                            }
                        }
                        else
                        {
                            ClGlobul.PhotoList.Add(UnLikePhoto_ID);
                        }
                    }

                    if (!string.IsNullOrEmpty(UnLikePhoto_UserName) || !string.IsNullOrEmpty(UnLikePhoto_Username_Path))
                    {

                        ClGlobul.PhotoList.Clear();

                        string s = UnLikePhoto_UserName;
                        if (!string.IsNullOrEmpty(UnLikePhoto_UserName))
                        {
                            ClGlobul.Userlist.Clear();
                            if (s.Contains(','))
                            {
                                string[] Data = Regex.Split(s,",");

                                foreach (var item in Data)
                                {
                                    ClGlobul.Userlist.Add(item);
                                }
                            }
                            else
                            {
                                ClGlobul.Userlist.Add(UnLikePhoto_UserName);
                            }
                        }
                        int count_user = 0;
                        int Usercount = 1;
                        int temp = no_photounlike_Username ;
                        bool value = true;
                        List<string> lstUserlist = ClGlobul.Userlist;

                        foreach (string item in lstUserlist)
                        {
                            count_user = 0;
                            string photoID_username = string.Empty;
                            string Url_foruser = "https://www.instagram.com/" + item+"/";
                            string resp_foruser = Unlike_ID.globusHttpHelper.getHtmlfromUrl(new Uri(Url_foruser), "");
                            string token = Utils.getBetween(resp_foruser, "csrf_token\":\"", "\"}");
                            string[] data = Regex.Split(resp_foruser, "code");
                            foreach (string item1 in data)
                            {
                                if (item1.Contains("date"))
                                {
                                    if (temp > count_user)
                                    {
                                         photoID_username = Utils.getBetween(item1, "\":\"", "\"");
                                      //  string photo_respo = Unlike_ID.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/p/" + photoID_username), "");
                                       // string photo_ID = Utils.getBetween(photo_respo, "content=\"instagram://media?id=", "\" />");
                                        ClGlobul.PhotoList.Add(photoID_username);
                                        ClGlobul.PhotoList= ClGlobul.PhotoList.Distinct().ToList();
                                        count_user++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            if (resp_foruser.Contains("has_next_page\":true"))
                            {
                                while (value)
                                {
                                    if (resp_foruser.Contains("has_next_page\":true") && ClGlobul.PhotoList.Count < temp)
                                    {
                                        string IDD = Utils.getBetween(resp_foruser, "\"id\":\"", "\"");
                                        string code_ID = Utils.getBetween(resp_foruser, "end_cursor\":\"", "\"");
                                        string postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                        resp_foruser = Unlike_ID.globusHttpHelper.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, "https://www.instagram.com/" + photoID_username, token);
                                        string[] data1 = Regex.Split(resp_foruser, "code");
                                        foreach (string val in data1)
                                        {
                                            if (val.Contains("date"))
                                            {
                                                if (count_user < temp)
                                                {
                                                    string photo_codes = Utils.getBetween(val, "\":\"", "\"");
                                                    ClGlobul.PhotoList.Add(photo_codes);
                                                    ClGlobul.PhotoList = ClGlobul.PhotoList.Distinct().ToList();
                                                    count_user++;
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

                }
            }

            catch { };

            bool ProcessStartORnot = false;

            if (IGGlobals.listAccounts.Count > 0)
            {
                if (ClGlobul.PhotoList.Count > 0)
                {
                    unlikeCompletionCount = 0;
                    ClGlobul.PhotoList.Distinct();
                    Lst_photoLikethread.Clear();
                    _boolUnlike = false;
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Starting Photo Unlike Process ]");
                    ThreadPool.SetMaxThreads(5, 5);
                    PhotoUnlike(ref Unlike_ID);
                }
            }

        }


        public void PhotoUnlike(ref InstagramUser unlikephoto_ID)
        {
            try
            {
                lstThreadsLikePoster.Add(Thread.CurrentThread);
                lstThreadsLikePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                GlobusHttpHelper obj = unlikephoto_ID.globusHttpHelper;
                string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                int count = 0;

                foreach (string itemPhotos in ClGlobul.PhotoList)
                {
                    if (_boolUnlike) return;

                    try
                    {
                        Globals.lstThread.Add(Thread.CurrentThread);
                        Thread.CurrentThread.IsBackground = true;
                        Globals.lstThread = Globals.lstThread.Distinct().ToList();
                    }
                    catch { };
                    try
                    {
                        string pageSource = string.Empty;
                        pageSource = unlikephoto_ID.globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://www.instagram.com/p/" + itemPhotos), "", 80, "", "");
                        string Photo_ID_unlike = Utils.getBetween(pageSource,"content=\"instagram://media?id=","\" />");
                        string token = Utils.getBetween(pageSource, "csrf_token\":\"", "\"}");
                        if (!string.IsNullOrEmpty(pageSource))
                        {
                            string like = string.Empty;
                            if (pageSource.Contains("viewer_has_liked\":true"))
                            {
                                try
                                {
                                    string unlike_postdata = "https://www.instagram.com/web/likes/" + Photo_ID_unlike + "/unlike/";
                                    string refer = "https://www.instagram.com/p/" + itemPhotos + "/";
                                    string response = unlikephoto_ID.globusHttpHelper.PostData_LoginThroughInstagram(new Uri(unlike_postdata), "", refer, token);
                                    if (!string.IsNullOrEmpty(response))
                                    {
                                        if (response.Contains("{\"status\":\"ok\"}"))
                                        {
                                            if (_boolUnlike) return;
                                            DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,Photo_Id, Status) values('" + "PhotoUnlikeModule" + "','" + unlikephoto_ID.username + "','" + DateTime.Now + "','" + itemPhotos + "','" + "Unliked" + "')", "tbl_AccountReport");
                                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Unliked " + itemPhotos + " From : " + unlikephoto_ID.username + " ]");
                                        }
                                        else
                                        {
                                            if (_boolUnlike) return;
                                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Failed To Unlike From : " + unlikephoto_ID.username + " ]");
                                        }
                                    }
                                    else
                                    {
                                        if (_boolUnlike) return;
                                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Failed To Unlike From : " + unlikephoto_ID.username + " ]");
                                    }


                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            else
                            {
                                if (pageSource.Contains("viewer_has_liked\":false"))
                                {
                                   // if (_boolUnlike) return;
                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + itemPhotos + " is not liked previously From : " + unlikephoto_ID.username + " ]");

                                    // GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Already Unliked " + itemPhotos + " From : " + unlikephoto_ID.username + " ]");
                                }
                            }
                        }
                        ClGlobul.PhotoList.Remove(itemPhotos);
                            if (minDelayUnLikePoster != 0)
                            {
                                mindelay = minDelayUnLikePoster;
                            }
                            if (maxDelayUnLikePoster != 0)
                            {
                                maxdelay = maxDelayUnLikePoster;
                            }

                            lock (_lockObject)
                            {
                                int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds For " + unlikephoto_ID.username + " ]");
                                Thread.Sleep(delay * 1000);
                                count++;
                            }
                            //if (ClGlobul.PhotoList.Count == count)
                            //{
                            //    break;
                            //}

                        
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Photo_ID  " + itemPhotos + " Incorrect ]");
                    }
                }
                GlobusLogHelper.log.Info("----------------------------------");
                  GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ UnLike Process Completed For " + unlikephoto_ID.username + " ]");
               // GlobusLogHelper.log.Info("----- UnLike Process Completed for " +  );
                GlobusLogHelper.log.Info("------------------------------------");
            }
            catch { };
        }


        public void Start_LikePhoto_ID(ref InstagramUser Like_photo)
        {
            try
            {
                lstThreadsLikePoster.Add(Thread.CurrentThread);
                lstThreadsLikePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            try
            {
                
                GlobusHttpHelper obj = Like_photo.globusHttpHelper;
               // string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                if (!string.IsNullOrEmpty(LikePhoto_Username))
                {
                    ClGlobul.PhotoList.Clear();
                    string s = LikePhoto_Username;
                    IGGlobals.Check_likephoto_Byusername = true;
                    if (s.Contains(','))
                    {
                        string[] Data = s.Split(',');

                        foreach (var item in Data)
                        {
                            ClGlobul.PhotoList.Add(item);
                        }
                    }
                    else
                    {
                        ClGlobul.PhotoList.Add(LikePhoto_Username);
                    }
                }

                if (!string.IsNullOrEmpty(LikePhoto_ID))
                {

                    ClGlobul.PhotoList.Clear();
                    string s = LikePhoto_ID;

                    if (s.Contains(','))
                    {
                        string[] Data = s.Split(',');

                        foreach (var item in Data)
                        {
                            ClGlobul.PhotoList.Add(item);
                        }
                    }
                    else
                    {
                        ClGlobul.PhotoList.Add(LikePhoto_ID);
                    }

                }



                if (string.IsNullOrEmpty(LikePhoto_ID_path))
                {

                    if (string.IsNullOrEmpty(LikePhoto_username_path))
                    {
                        bool ProcessStartORnot = false;
                        if (IGGlobals.listAccounts.Count != 0)
                        {
                            if (Nothread_LikePoster == 0)
                            {
                                //if (MessageBox.Show("Do you really want to Start Without Thread", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                //{
                                //    ProcessStartORnot = true;
                                //    ClGlobul.NoOfPhotoLikeThread = 1;
                                //}
                                //else
                                //{
                                //}
                            }
                            else
                            {
                                try
                                {
                                    ClGlobul.NoOfPhotoLikeThread = Nothread_LikePoster;
                                    ProcessStartORnot = true;
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                }
                            }
                        }
                        else
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please Upload Accounts. ]");

                        }
                    }
                    else
                    {
                        IGGlobals.Check_likephoto_Byusername = true;
                    }
                }

                if (ClGlobul.ProxyList.Count > 0)
                {
                    //Frm_proxy frmProxy = new Frm_proxy();

                    //AddTophotoLogger("[ " + DateTime.Now + " ] => [ Please Upload Proxies. ]");
                    //MessageBox.Show("Please Upload Proxies.", "Please Upload Proxies.");
                    //frmProxy.Show();
                }

                if (status == "Success")
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logged In From : " + Like_photo.username + " ]");
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Starting Photo Like From : " + Like_photo.username + " ]");
                    getPhotoLike(ref Like_photo);
                }
                else
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logged In Fail : " + Like_photo.username + " ]");
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void getPhotoLike(ref InstagramUser Photo_likee)
        {
            try
            {
                lstThreadsLikePoster.Add(Thread.CurrentThread);
                lstThreadsLikePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                GlobusHttpHelper obj = Photo_likee.globusHttpHelper;
                string res_secondURL = obj.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                foreach (string PhotoList_item in ClGlobul.PhotoList)
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
                                if (IGGlobals.Check_likephoto_Byusername == true)
                                {
                                    photolike(photoId, ref Photo_likee);
                                }
                                else
                                {
                                    Result = photolike(photoId, ref Photo_likee);
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
                                    if (Result.Contains("Instagram API does not respond"))
                                    {
                                        GlobusLogHelper.log.Info("Instagram API does not respond" +    PhotoList_item);
                                    }
                                    else
                                    {
                                        QueryExecuter.insertLikeStatus(photoId, Photo_likee.username, 1);
                                        //GlobusLogHelper.log.Info("photoID in Incorrect");
                                    }

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

                            if (minDelayLikePoster != 0)
                            {
                                mindelay = minDelayLikePoster;
                            }
                            if (maxDelayLikePoster != 0)
                            {
                                maxdelay = maxDelayLikePoster;
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
                lstThreadsLikePoster.Add(Thread.CurrentThread);
                lstThreadsLikePoster.Distinct();
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
                            string photo_codes = Utils.getBetween("@"+list, "@\"", "\"");
                            phto_list.Add(photo_codes);
                        }
                    }
                    if(responce_user.Contains("has_next_page\":true"))
                    {
                        while(value)
                        {
                            if(responce_user.Contains("has_next_page\":true") && phto_list.Count<temp)
                            {
                                string IDD = Utils.getBetween(responce_user,"\"id\":\"","\"");
                                string code_ID = Utils.getBetween(responce_user,"end_cursor\":\"","\"");
                                string postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                responce_user = Photo_likebyID.globusHttpHelper.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, "https://www.instagram.com/" + PhotoId, token);
                                string[] data1 = Regex.Split(responce_user, "code");
                                foreach(string val in data1)
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

                                                DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,Photo_Id,Status) values('" + "PhotoLikeModule" + "','" + Photo_likebyID.username + "','" + DateTime.Now + "','" + item + "','" + "LIKED" + "')", "tbl_AccountReport");
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
                                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ " + Photo_likebyID.username + "  Already LIKED : " + PhotoId + "==>PhotoID===>" + item + "]");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                                    }

                                }
                            }
                            if (flag == temp)
                            {
                                break;
                            }
                        

                        if (minDelayLikePoster != 0)
                        {
                            mindelay = minDelayLikePoster;
                        }
                        if (maxDelayLikePoster != 0)
                        {
                            maxdelay = maxDelayLikePoster;
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

        public static List<Thread> lstDownloadingPhoto = new List<Thread>();
        public void StartDownload_Photo(ref InstagramUser Download_pic)
        {
            try
            {
                lstThreadsLikePoster.Add(Thread.CurrentThread);
                lstThreadsLikePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
                lstDownloadingPhoto.Add(Thread.CurrentThread);
                if(lstDownloadingPhoto.Count()>1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                GlobusHttpHelper objGlobusHttpHelper = Download_pic.globusHttpHelper;
                string resp = objGlobusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                if (string.IsNullOrEmpty(UserphotoDownload_Multiple))
                {
                    if (!string.IsNullOrEmpty(UserphotoDownload_Single))
                    {
                        ClGlobul.lstStoreDownloadImageKeyword.Clear();

                        string s = UserphotoDownload_Single;

                        if (s.Contains(','))
                        {
                            string[] Data = s.Split(',');

                            foreach (var item in Data)
                            {
                                ClGlobul.lstStoreDownloadImageKeyword.Add(item);
                            }
                        }
                        else
                        {
                            ClGlobul.lstStoreDownloadImageKeyword.Add(UserphotoDownload_Single);
                        }
                    }
                }
                DownloadingImage(ref Download_pic);
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }


        public void DownloadingImage(ref InstagramUser Download_Photo)
        {
            try
            {
                lstThreadsLikePoster.Add(Thread.CurrentThread);
                lstThreadsLikePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                GlobusHttpHelper objGlobusHttpHelper = Download_Photo.globusHttpHelper;
                string resp = objGlobusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com"), "");
                Globals.lstThread.Add(Thread.CurrentThread);
                Thread.CurrentThread.IsBackground = true;
                Globals.lstThread = Globals.lstThread.Distinct().ToList();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {

                foreach (string item_image in ClGlobul.lstStoreDownloadImageKeyword)
                {
                    startDownloadingImage(item_image, ref Download_Photo);

                    Thread.Sleep(5000);
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] =>[Process completed.");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }


        public void startDownloadingImage(string itemImageTag, ref InstagramUser Download)
        {
            try
            {
                lstThreadsLikePoster.Add(Thread.CurrentThread);
                lstThreadsLikePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                Thread.CurrentThread.IsBackground = true;
                Lst_photoLikethread.Add(Thread.CurrentThread);
                Lst_photoLikethread = Lst_photoLikethread.Distinct().ToList();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            string pageSource = string.Empty;
            int counter = 0;
            GlobusHttpHelper objGlobusHttpHelper = Download.globusHttpHelper;
            List<string> lstCountImage = new List<string>();
            string url = string.Empty;
            string mainUrl = "https://www.instagram.com";
            List<string> list_pics = new List<string>();
            bool value = true;
            string postdata = string.Empty;
            if (IsDownLoadImageUsingHashTag)
            {

               // url = mainUrl + "tag/" + itemImageTag.Replace("#", "");
                url = "https://www.instagram.com/explore/tags/" + itemImageTag.Replace("#", "");
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] =>[Process Using HashTag =" + itemImageTag);
            }

            else if (IsDownLoadImageUsingUserName)
            {
                url = mainUrl + "/" + itemImageTag+"/";
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] =>[Process Selected Using UserName  =" + itemImageTag);
            }



            else
            {
                url = mainUrl + "tag/" + itemImageTag.Replace("#", "");
                
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] =>[Process Using HashTag" + itemImageTag);
            }
            try
            {
                pageSource = objGlobusHttpHelper.getHtmlfromUrl(new Uri(url), "");
            }
            catch { }

            if (string.IsNullOrEmpty(pageSource))
            {
                pageSource = objGlobusHttpHelper.getHtmlfromUrl(new Uri(url), "");
            }
            if (!string.IsNullOrEmpty(pageSource))
            {
                string token = Utils.getBetween(pageSource, "csrf_token\":\"", "\"}");
                if (pageSource.Contains("code"))
                {
                    string[] arr = Regex.Split(pageSource, "code");
                    foreach (string var in arr)
                    {
                        if (var.Contains("date"))
                        {
                            list_pics.Add(var);
                        }
                    }
                    if(pageSource.Contains("has_next_page\":true"))
                    {
                        while (value)
                        {
                            if (pageSource.Contains("has_next_page\":true") && list_pics.Count < no_photo_Download)
                            {
                                string IDD = Utils.getBetween(pageSource, "\"id\":\"", "\"");
                                string code_ID = Utils.getBetween(pageSource, "end_cursor\":\"", "\"");
                                if (IsDownLoadImageUsingUserName)
                                {
                                     postdata = "q=ig_user(" + IDD + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                }
                                else
                                {
                                    postdata = "q=ig_hashtag(" + itemImageTag.Replace("#","") + ")+%7B+media.after(" + code_ID + "%2C+12)+%7B%0A++count%2C%0A++nodes+%7B%0A++++caption%2C%0A++++code%2C%0A++++comments+%7B%0A++++++count%0A++++%7D%2C%0A++++date%2C%0A++++dimensions+%7B%0A++++++height%2C%0A++++++width%0A++++%7D%2C%0A++++display_src%2C%0A++++id%2C%0A++++is_video%2C%0A++++likes+%7B%0A++++++count%0A++++%7D%2C%0A++++owner+%7B%0A++++++id%0A++++%7D%2C%0A++++thumbnail_src%0A++%7D%2C%0A++page_info%0A%7D%0A+%7D&ref=users%3A%3Ashow";
                                }
                                pageSource = Download.globusHttpHelper.postFormDatainta(new Uri("https://www.instagram.com/query/"), postdata, url, token);
                                string[] data1 = Regex.Split(pageSource, "code");
                                foreach (string val in data1)
                                {
                                    if (val.Contains("date"))
                                    {
                                        if (list_pics.Count < no_photo_Download)
                                        {

                                            list_pics.Add(val);
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









                    if (list_pics.Count > 1)
                    {
                        arr = list_pics.Skip(1).ToArray();

                        foreach (string itemarr in list_pics)
                        {

                            try
                            {
                                if (itemarr.Contains("date"))
                                {
                                    //string startString = "<a href=\"";
                                    //string endString = "\" class=\"mainimg\"";
                                    string imageId = string.Empty;
                                    string imageSrc = string.Empty;
                                   
                                        
                                            try
                                            {

                                                imageId = Utils.getBetween(itemarr, "\":\"", "\"");
                                            }
                                            catch { }
                                            if (!string.IsNullOrEmpty(imageId))
                                            {

                                            }


                                            if (itemarr.Contains("display_src"))
                                        {
                                            try
                                            {
                                                imageSrc = Utils.getBetween(itemarr, "display_src\":\"", "\"}").Replace("\\","");

                                            }
                                            catch { }
                                            counter++;


                                            SaveImageWithUrl(imageSrc, FileData, imageId + "_" + counter);


                                            lstCountImage.Add(imageSrc);
                                            lstCountImage = lstCountImage.Distinct().ToList();
                                            DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,UserName,Photo_Id, Status) values('" + "DownloadImage" + "','" + Download.username + "','" + DateTime.Now + "','" + itemImageTag + "','" + imageId + "','" + "Downloaded" + "')", "tbl_AccountReport");
                                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] =>[Image DownLoaded with user=" + itemImageTag + "_" + imageId + "_" + counter);
                                            if (minDelayDownloadPoster != 0)
                                            {
                                                mindelay = minDelayDownloadPoster;
                                            }
                                            if (maxDelayDownloadPoster != 0)
                                            {
                                                maxdelay = maxDelayDownloadPoster;
                                            }
                                            lock (_lockObject)
                                            {
                                                int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                                                Thread.Sleep(delay * 1000);
                                            }

                                            if (lstCountImage.Count >= no_photo_Download)
                                            {
                                                return;
                                            }


                                            try
                                            {

                                            }
                                            catch { }
                                        }
                                    
                                }
                            }
                            catch { }
                        }
                        #region pagination
                        string pageLink = string.Empty;
                        while (true)
                        {
                            //if (stopScrapImageBool) return;
                            string startString = "<a href=\"";
                            string endString = "\" class=\"mainimg\"";
                            string imageId = string.Empty;
                            string imageSrc = string.Empty;

                            if (!string.IsNullOrEmpty(pageLink))
                            {
                                pageSource = objGlobusHttpHelper.getHtmlfromUrl(new Uri(pageLink));
                                if (string.IsNullOrEmpty(pageSource))
                                {
                                    pageSource = objGlobusHttpHelper.getHtmlfromUrl(new Uri(pageLink));
                                }
                            }

                            if (pageSource.Contains("<ul class=\"pager\">") && pageSource.Contains("rel=\"next\">"))
                            {
                                try
                                {
                                    pageLink = Utils.getBetween(pageSource, "<ul class=\"pager\">", "rel=\"next\">");
                                }
                                catch { }
                                if (!string.IsNullOrEmpty(pageLink))
                                {
                                    try
                                    {
                                        int len = pageLink.IndexOf("<a href=\"");
                                        len = len + ("<a href=\"").Length;
                                        pageLink = pageLink.Substring(len);
                                        pageLink = pageLink.Trim();
                                        pageLink = pageLink.TrimEnd(new char[] { '"' });
                                        pageLink = IGGlobals.Instance.IGWEPME + pageLink;
                                    }
                                    catch { }
                                    if (!string.IsNullOrEmpty(pageLink))
                                    {
                                        string response = string.Empty;
                                        try
                                        {
                                            response = objGlobusHttpHelper.getHtmlfromUrl(new Uri(pageLink));
                                        }
                                        catch { }
                                        if (!string.IsNullOrEmpty(response))
                                        {
                                            if (response.Contains("<div class=\"mainimg_wrapper\">"))
                                            {
                                                try
                                                {
                                                    string[] arr1 = Regex.Split(response, "<div class=\"mainimg_wrapper\">");
                                                    if (arr1.Length > 1)
                                                    {
                                                        arr1 = arr1.Skip(1).ToArray();
                                                        foreach (string items in arr1)
                                                        {
                                                            try
                                                            {
                                                                //if (stopScrapImageBool) return;
                                                                if (items.Contains("<a href=\"/p/"))
                                                                {
                                                                    int indexStart = items.IndexOf("<a href=\"/p/");
                                                                    string itemarrNow = items.Substring(indexStart);

                                                                    try
                                                                    {
                                                                        imageId = Utils.getBetween(itemarrNow, startString, endString).Replace("/", "");
                                                                    }
                                                                    catch { }
                                                                    if (!string.IsNullOrEmpty(imageId))
                                                                    {
                                                                        
                                                                    }

                                                                    if (itemarrNow.Contains("<img src=\""))
                                                                    {
                                                                        try
                                                                        {
                                                                            imageSrc = Utils.getBetween(itemarrNow, "<img src=\"", "\"");
                                                                        }
                                                                        catch { }
                                                                    }

                                                                    counter++;

                                                                    {
                                                                        SaveImageWithUrl(imageSrc, FileData, imageId + "_" + counter);
                                                                    }
                                                                    lstCountImage.Add(imageSrc);
                                                                    lstCountImage = lstCountImage.Distinct().ToList();
                                                                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] =>[Image DownLoaded with ImageName  " + imageId + "_" + counter);
                                                                    if (minDelayDownloadPoster != 0)
                                                                    {
                                                                        mindelay = minDelayDownloadPoster;
                                                                    }
                                                                    if (maxDelayDownloadPoster != 0)
                                                                    {
                                                                        maxdelay = maxDelayDownloadPoster;
                                                                    }
                                                                    lock (_lockObject)
                                                                    {
                                                                        int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                                                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds ]");
                                                                        Thread.Sleep(delay * 1000);
                                                                    }
                                                                    if (lstCountImage.Count >= ClGlobul.countNOOfFollowersandImageDownload)
                                                                    {
                                                                        return;
                                                                    }

                                                                }

                                                            }
                                                            catch { }
                                                        }
                                                        if (lstCountImage.Count >= ClGlobul.countNOOfFollowersandImageDownload)
                                                        {
                                                            return;
                                                        }
                                                    }
                                                }
                                                catch { }

                                            }
                                        }
                                        else
                                        {

                                        }

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        #endregion

                    }
                }
            }

            if (!string.IsNullOrEmpty(pageSource))
            {
                url = mainUrl + "n/" + itemImageTag;
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] =>[" + itemImageTag + "  There is no more images. ");

            }
            else
            {
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] =>[" + itemImageTag + " = This user does not exist.");
            }
        }

        private void SaveImageWithUrl(string imgeUri, string saveto, string imageName)
        {

            try
            {
                Globals.lstThread.Add(Thread.CurrentThread);
                Thread.CurrentThread.IsBackground = true;
                Globals.lstThread = Globals.lstThread.Distinct().ToList();
            }
            catch { };
            try
            {
                // GlobusFileHelper.AppendStringToTextfileNewLine(imgeUri, ImageUrlData);
                using (WebClient webClient = new WebClient())
                {
                    using (Stream stream = webClient.OpenRead(imgeUri))
                    {
                        byte[] oImageBytes = webClient.DownloadData(imgeUri);
                        {


                            File.WriteAllBytes(filepath + "\\" + imageName + ".jpg", oImageBytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }


        public void StartLiker(ref InstagramUser Liker)
        {
            try
            {
                lstThreadsLikePoster.Add(Thread.CurrentThread);
                lstThreadsLikePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            ClGlobul.StartLikerLike = true;
            GlobusHttpHelper obj = Liker.globusHttpHelper;
            string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
            try
            {
                if (no_Photo_liker != 0)
                {
                    ClGlobul.PhotosCount = no_Photo_liker;
                }
                else
                {
                    GlobusLogHelper.log.Info("Photos Count can't remain empty.");
                    return;
                }

                if (NoUser_LikerPoster != 0)
                {
                    ClGlobul.UsersCount = NoUser_LikerPoster;
                }
                else
                {
                    GlobusLogHelper.log.Info("Photos Count can't remain empty.");
                    return;
                }

                if (string.IsNullOrEmpty(txt_username_Liker_Multiple))
                {
                    if (!string.IsNullOrEmpty(txt_username_Liker_single))
                    {
                        try
                        {

                           
                            ClGlobul.lstUsername.Clear();

                            string s = txt_username_Liker_single;

                            if (s.Contains(','))
                            {
                                try
                                {
                                    string[] Data = s.Split(',');

                                    foreach (var item in Data)
                                    {


                                        ClGlobul.lstUsername.Add(item);
                                    }
                                }
                                catch { };
                            }
                            else
                            {
                                ClGlobul.lstUsername.Add(txt_username_Liker_single);
                            }
                        }
                        catch { };
                    }
                }
                if (status == "Success")
                {
                    if (ClGlobul.StartLikerLike)
                    {
                        StartLike_liker(ref Liker);
                    }
                    else
                    {
                        GlobusLogHelper.log.Info("----------");
                    }

                }
                else
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logged In Fail : " + Liker.username + " ]");
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Exception : " + ex.Message);
            }
        }

        
        public void StartLike_liker(ref InstagramUser likerr_obj)
        {
            try
            {
                lstThreadsLikePoster.Add(Thread.CurrentThread);
                lstThreadsLikePoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            try
            {
                GlobusHttpHelper obj = likerr_obj.globusHttpHelper;
                string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                foreach (var itemUsername in ClGlobul.lstUsername)
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Scraping the photo Likers of user : " + itemUsername + " ]");
                    List<string> lstLikersName = new List<string>();
                    List<string> lstToken = new List<string>();

                    string PPagesource = string.Empty;
                    try
                    {
                        PPagesource = likerr_obj.globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://www.instagram.com/" + itemUsername + ""), "", 444, "", "");
                        string[] spite = Regex.Split(PPagesource, "code");
                            foreach(string var in spite)
                            {
                                if(var.Contains("date"))
                                {
                                    string phto_Id = Utils.getBetween(var, "\":\"", "\"");
                                    string URL = "https://www.instagram.com/p/" + phto_Id;
                                    
                                    //string respo = likerr_obj
                                }
                            }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Username Pagesource Error : " + ex.Message + "]");
                    }

                    string accessToken = string.Empty;
                    int arrTokenCount = 0;

                    while (lstLikersName.Count < ClGlobul.UsersCount)
                    {

                        if (PPagesource.Contains("accessToken"))
                        {
                            accessToken = Utils.getBetween(PPagesource, "accessToken", ")").Replace("'", "").Replace(",", "").Trim();
                        }
                        if (PPagesource.Contains("data-target"))
                        {
                            string[] arrToken = Regex.Split(PPagesource, "data-target");
                            int i = 0;
                            foreach (var itemToken in arrToken)
                            {
                                if (!itemToken.Contains("<!DOCTYPE html>"))
                                {
                                    string token = Utils.getBetween(itemToken, "=", ">").Replace("\"", "").Trim();
                                    if (token.Contains("data-user"))
                                    {
                                        token = Utils.getBetween(token, "", "data-user").Trim();
                                    }
                                    if (token.Contains("_") && !token.Contains("#"))
                                    {
                                        if (!lstToken.Contains(token))
                                        {
                                            lstToken.Add(token);
                                        }
                                    }
                                }
                            }
                        }


                        foreach (string item in lstToken)
                        {
                            string Id = item;
                            string New_Url = IGGlobals.Instance.IGApi_media + Id + "/likes?access_token=" + accessToken;
                            string newurl = IGGlobals.Instance.IGphotolikeurl + Id;
                            string new_urlresp = likerr_obj.globusHttpHelper.getHtmlfromUrlProxy(new Uri(newurl), "", 444, "", "");
                            string split = "";
                            string[] data = Regex.Split(new_urlresp, "\"username\":");
                            foreach (string item1 in data)
                            {
                                try
                                {

                                    if (item1.Contains("profile_picture"))
                                    {
                                        string user = Utils.getBetween(item1, "\"", "\"");
                                        string user_link = "/n/" + user;
                                        if (lstLikersName.Count < ClGlobul.UsersCount)
                                        {
                                            lstLikersName.Add(user_link);
                                        }
                                    }
                                }
                                catch { };


                            }
                        }



                        break;
                    }
                    int LikersCount = ClGlobul.UsersCount;                   
                    int mindelay = 0;
                    int maxdelay = 1;
                    foreach (var itemLikers in lstLikersName)
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Liking the photo of user : " + itemLikers + "]");

                        if (minDelayLikerPoster != 0)
                        {
                            mindelay = minDelayLikerPoster;
                        }
                        if (mixDelayLikerPoster != 0)
                        {
                            maxdelay = mixDelayLikerPoster;
                        }

                        
                        //int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                        //GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds For " + likerr_obj.username + " ]");
                        //Thread.Sleep(delay * 1000);
                       
                        int PhotosCount = ClGlobul.PhotosCount;

                        string LikersPagesource = string.Empty;
                        try
                        {
                            LikersPagesource = likerr_obj.globusHttpHelper.getHtmlfromUrlProxy(new Uri(IGGlobals.Instance.IGWEPME + itemLikers + ""), "", 444, "", "");
                        }
                        catch (Exception ex)
                        {
                        }

                        if (!string.IsNullOrEmpty(LikersPagesource))
                        {
                            if (!LikersPagesource.Contains("This user is private."))
                            {
                                if (LikersPagesource.Contains("data-target"))
                                {
                                    List<string> lstPhotosToken = new List<string>();
                                    string[] arrPhotosToLike = Regex.Split(LikersPagesource, "data-target");
                                    foreach (var itemPhotosToLike in arrPhotosToLike)
                                    {
                                        if (!itemPhotosToLike.Contains("<!DOCTYPE html>"))
                                        {
                                            string tokenPhotos = Utils.getBetween(itemPhotosToLike, "=", ">").Replace("\"", "").Trim();
                                            if (!tokenPhotos.Contains("#") && tokenPhotos.Contains("_") && !lstPhotosToken.Contains(tokenPhotos) && !tokenPhotos.Contains("data-user"))
                                            {
                                                lstPhotosToken.Add(tokenPhotos);
                                            }
                                        }
                                    }
                                    string[] arrToken = lstToken.ToArray();
                                    int i = 0;
                                    foreach (var itemToken in lstPhotosToken)
                                    {
                                        if (PhotosCount != 0)
                                        {
                                            string finalPagesource = string.Empty;
                                            try
                                            {
                                                finalPagesource = likerr_obj.globusHttpHelper.getHtmlfromUrlProxy(new Uri(IGGlobals.Instance.IGLikewebsta_api + itemToken + ""), "", 444, "", "");
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Like Pagesource Error : " + ex.Message + "]");
                                            }

                                            if (finalPagesource.Contains("OK") && finalPagesource.Contains("LIKED"))
                                            {
                                                DataBaseHandler.InsertQuery("insert into tbl_AccountReport(ModuleName,Account_User,DateTime,Photo_Id, Status) values('" + "Like_LikerPhoto" + "','" + likerr_obj.username + "','" + DateTime.Now + "','" + itemToken + "','" + "LIKED" + "')", "tbl_AccountReport");
                                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [" + ++i + " Liked" + "Photo_ID" + itemToken + "]");
                                                int delay = RandomNumberGenerator.GenerateRandom(mindelay, maxdelay);
                                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Delay For " + delay + " Seconds For " + likerr_obj.username + " ]");
                                                Thread.Sleep(delay * 1000);

                                                PhotosCount--;
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
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ This user is private.]");                             
                            }
                        }
                        if (PhotosCount == ClGlobul.PhotosCount)
                        {
                            continue;
                        }
                        else
                        {
                            LikersCount--;
                        }
                        if (LikersCount == 0)
                        {
                            break;
                        }
                    }
                }
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Process Completed ]");
            }
            catch (Exception ex)
            {
            }

        }

    }
}

