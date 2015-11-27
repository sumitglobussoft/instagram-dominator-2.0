using Accounts;
using BaseLib;
using BaseLibID;
using Globussoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Comment
{
    public class CommentManager
    {
        #region Global Variables For Comment Poster


        readonly object lockrThreadControlleCommentPoster = new object();
        public bool isStopCommentPoster = false;
        public bool useOriginalMessage = true;
        int countThreadControllerCommentPoster = 0;
        public static int TotalNoOfCommentPosterCounter = 0;
        public static int messageCountCommentPoster = 0;
        int countCommentPoster = 1;
        public static string CommentPhoto_ID = string.Empty;
        public static string message_comment = string.Empty;
        public static string CommentPhoto_ID_path = string.Empty;
        public static string message_comment_path = string.Empty;
        public static int Nothread_comment = 0;
    
        public List<Thread> lstThreadsCommentPoster = new List<Thread>();
        public List<string> lstCommentPostURLsCommentPoster = new List<string>();
        public List<string> lstCommentPostURLsTitles = new List<string>();


        public static int minDelayCommentPoster = 10;
        public static int maxDelayCommentPoster = 20;
        public static string status = string.Empty;
        public GlobusHttpHelper httpHelper = new GlobusHttpHelper();
        public ChilkatHttpHelpr chilkathttpHelper = new ChilkatHttpHelpr();
        public static int mindelay = 0;
        public static int maxdelay = 0;


        #endregion

        public int NoOfThreadsCommentPoster
        {
            get;
            set;
        }

        public void StartCommentPoster()
        {
            countThreadControllerCommentPoster = 0;
            try
            {
                int numberOfAccountPatch = 25;

                if (NoOfThreadsCommentPoster > 0)
                {
                    numberOfAccountPatch = NoOfThreadsCommentPoster;
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
                                lock (lockrThreadControlleCommentPoster)
                                {
                                    try
                                    {
                                        if (countThreadControllerCommentPoster >= listAccounts.Count)
                                        {
                                            Monitor.Wait(lockrThreadControlleCommentPoster);
                                        }

                                        string acc = account.Remove(account.IndexOf(':'));

                                        //Run a separate thread for each account
                                        InstagramUser item = null;
                                        IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);
                                        

                                        if (item != null)
                                        {
                                            Thread profilerThread = new Thread(StartMultiThreadsCommentPoster);
                                            profilerThread.Name = "workerThread_Profiler_" + acc;
                                            profilerThread.IsBackground = true;

                                            profilerThread.Start(new object[] { item });

                                            countThreadControllerCommentPoster++;
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

        public void StartMultiThreadsCommentPoster(object parameters)
        {
            try
            {
                if (!isStopCommentPoster)
                {
                    try
                    {
                        lstThreadsCommentPoster.Add(Thread.CurrentThread);
                        lstThreadsCommentPoster.Distinct();
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
                            status= objAccountManager.LoginUsingGlobusHttp(ref objFacebookUser);
                           
                           
                        }

                        if (objFacebookUser.isloggedin)
                        {
                            status = "Success";           
                           StartActionCommentPoster(ref objFacebookUser);
                           

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
                        lock (lockrThreadControlleCommentPoster)
                        {
                            countThreadControllerCommentPoster--;
                            Monitor.Pulse(lockrThreadControlleCommentPoster);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }

        private void StartActionCommentPoster(ref InstagramUser fbUser)
        {

            try
            {
                Start_Comment(ref fbUser);
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        public void Start_Comment(ref InstagramUser IGcomment)
        {
            try
            {
                lstThreadsCommentPoster.Add(Thread.CurrentThread);
                lstThreadsCommentPoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            try
            {
                bool ProcessStartORnot = false;
                
                GlobusHttpHelper obj = IGcomment.globusHttpHelper;
                string res_secondURL = obj.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
                int parsedValue;

                if (!string.IsNullOrEmpty(CommentPhoto_ID) && !string.IsNullOrEmpty(message_comment))
                {
                    string s = CommentPhoto_ID;
                    string k = message_comment;
                    ClGlobul.CommentIdsForMSG.Clear();
                    ClGlobul.commentMsgList.Clear();
                    if (s.Contains(','))
                    {
                        string[] Data = s.Split(',');

                        foreach (var item in Data)
                        {
                            ClGlobul.CommentIdsForMSG.Add(item);
                        }
                    }
                    else
                    {
                        ClGlobul.CommentIdsForMSG.Add(CommentPhoto_ID);
                    }
                    if(k.Contains(","))
                    {
                        string[] data1 = Regex.Split(k,",");
                        foreach(string item in data1)
                        {
                            ClGlobul.commentMsgList.Add(item);
                        }
                    }
                    else
                    {
                        ClGlobul.commentMsgList.Add(message_comment);
                    }
                }
            }

            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            if (string.IsNullOrEmpty(message_comment_path))
            {
                if (!string.IsNullOrEmpty(message_comment))//txtsingalmsg
                {
                   // ClGlobul.commentMsgList.Clear();
                    if (Nothread_comment != 0)
                    {
                        //if (MessageBox.Show("Do you really want to Start Without Thread", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)  //txtsingalmsg
                        //{

                        //    ProcessStartORnot = true;
                        //    ClGlobul.NoOfcommentThread = 1;
                        //    try
                        //    {
                        //        string AllMessege = txtsingalmsg.Text.Trim();
                        //        string[] ListMessages = Regex.Split(AllMessege, ",");
                        //        foreach (string str in ListMessages)
                        //        {
                        //            ClGlobul.commentMsgList.Add(str);
                        //        }
                        //    }
                        //    catch { };
                        //}
                        //else
                        //{
                        //}
                    }
                    else
                    {
                        try
                        {
                            ClGlobul.NoOfcommentThread = Nothread_comment;
                            
                            try
                            {
                                 string AllMessege = message_comment;                              
                                string[] ListMessages = Regex.Split(AllMessege, ",");
                                foreach (string str in ListMessages)
                                {
                                    ClGlobul.commentMsgList.Add(str);
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
                else
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Please upload Comments. ]");
                   
                }
            }

                    if (status == "Success")
                        {
                         foreach (var CommentIdsForMSG_item in ClGlobul.CommentIdsForMSG)
                            {
                                getComment(CommentIdsForMSG_item, ref IGcomment);
                            }
                        }

                    else
                    {
                        if(status == "Failed")
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Login Fail. ]" + IGcomment.username);
                        }
                    }
                  
        }

        public void getComment(string CommentIdsForMSG_item , ref InstagramUser usercomment)
        {

            try
            {
                lstThreadsCommentPoster.Add(Thread.CurrentThread);
                lstThreadsCommentPoster.Distinct();
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

                string message = ClGlobul.commentMsgList[RandomNumberGenerator.GenerateRandom(0, ClGlobul.commentMsgList.Count)];
                try
                {
                    string status = Comment(CommentIdsForMSG_item, message,ref usercomment );
                    if (status == "Success")
                    {
                       
                         GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [  comment is successfully posted from " + CommentIdsForMSG_item +"]");
                    }
                    else
                    {
                        if( status == "Instagram API does not respond")
                        {
                            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Instagram API does not respond on  " + CommentIdsForMSG_item + "]");
                        }
                        else
                        {
                            if(status == "Fail")
                            {
                                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Fail to comment  " + CommentIdsForMSG_item + "]");
                            }
                        }
                    }

                    if (minDelayCommentPoster != 0)
                    {
                        mindelay = minDelayCommentPoster;
                    }
                    if (maxDelayCommentPoster != 0)
                    {
                        maxdelay = maxDelayCommentPoster;
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
                lstThreadsCommentPoster.Add(Thread.CurrentThread);
                lstThreadsCommentPoster.Distinct();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            string FollowedPageSource = string.Empty;

            try
            {
                string res_secondURL = User_comment.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGTestURL), "");
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

                        CommentIdlink = IGGlobals.Instance.IGstagramurl + commentId + "/";

                        commentIdLoggedInLink = IGGlobals.Instance.IGhomeurl + commentId;
                    }
                    catch(Exception ex)
                    {
                        return ex.Message;
                    }
                }

                #region Change
                //GlobusHttpHelper _GlobusHttpHelper = new GlobusHttpHelper();

                //ChilkatHttpHelpr _ChilkatHttpHelpr = new ChilkatHttpHelpr();

                //InstagramAccountManager _InstagramAccountManager = new InstagramAccountManager(accountManager.Username, accountManager.Password, accountManager.proxyAddress, accountManager.proxyPassword, accountManager.proxyUsername, accountManager.proxyPassword);

                string url = IGGlobals.Instance.IGStagram_api + commentId;

                bool checkunicode = ContainsUnicodeCharacter(CommentMsg);

                string CmntMSG = string.Empty;
               

                if (checkunicode == false)
                {
                    try
                    {
                        CmntMSG = CommentMsg.Replace(" ", "+");
                       
                    }
                    catch(Exception ex)
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
                    catch(Exception ex) 
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
                catch(Exception ex)
                {
                    return ex.Message;
                }

                if (FollowedPageSource.Contains("status\":\"OK\"") || FollowedPageSource.Contains("created_time"))
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
                    if(ClGlobul.checkHashTagComment == true)
                    {
                        try
                        {
                            DataBaseHandler.InsertQuery("insert into comment_hash_tag (account_holder, photo_id, comment_date, comment_status) values ('" + User_comment.username + "','" + commentId + "','" + Convert.ToString(DateTime.Now) + "','" + FollowedPageSource + "')", "comment_hash_tag");
                        }
                        catch(Exception ex)
                        {}
                    }
                }
                catch
                {}
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
