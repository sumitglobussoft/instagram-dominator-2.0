using BaseLibID;
using Globussoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLib;
using System.Text.RegularExpressions;

namespace Accounts
{
    public class AccountManager
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string proxyAddress { get; set; }
        public string proxyPort { get; set; }
        public string proxyUsername { get; set; }
        public string proxyPassword { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public bool LoggedIn { get; set; }
        public static string Authorization = string.Empty;
        public static string acc_status = string.Empty;
        public GlobusHttpHelper httpHelper = new GlobusHttpHelper();
        public ChilkatHttpHelpr chilkathttpHelper = new ChilkatHttpHelpr();
        bool value = true;
                
        public string LoginUsingGlobusHttp(ref InstagramUser InstagramUser)
        {
            ///Sign In
            #region comment

            //GlobusHttpHelper httpHelper = InstagramUser.globusHttpHelper;


            //GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logging in with Account : " + InstagramUser.username + " ]");
            //string Status = "Failed";
            //try
            //{
            //    string firstUrl = "https://api.instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
            //    #region for Chk Authorization By Anil
            //    //  string Authorization_respo = httpHelper.getHtmlfromUrl(new Uri(firstUrl));

            //    #endregion
            //    //https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes

            //    string secondURL = "https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
            //    // string Authorization_respo1 = httpHelper.getHtmlfromUrl(new Uri(secondURL));
            //    ChilkatHttpHelpr objchilkat = new ChilkatHttpHelpr();
            //    string res_secondURL = string.Empty;
            //    if (!string.IsNullOrEmpty(proxyAddress) && !string.IsNullOrEmpty(proxyPort))
            //    {
            //        try
            //        {
            //            // res_secondURL = objchilkat.GetHtmlProxy(secondURL, proxyAddress, proxyPort, proxyUsername, proxyPassword);
            //            res_secondURL = httpHelper.getHtmlfromUrlProxy(new Uri(secondURL), "", proxyAddress, proxyPort, proxyUsername, proxyPassword);
            //        }
            //        catch { };
            //    }
            //    else
            //    {
            //        res_secondURL = httpHelper.getHtmlfromUrl(new Uri(secondURL), "");
            //        //res_secondURL = HttpHelper.getHtmlfromUrlProxy(new Uri(secondURL), "", proxyAddress, proxyPort, proxyUsername, proxyPassword);
            //    }
            //   string nextUrl = string.Empty;
            //    string res_nextUrl = string.Empty;

            //    if (!string.IsNullOrEmpty(res_secondURL))
            //    {
            //        nextUrl = "https://instagram.com/accounts/login/?force_classic_login=&next=/oauth/authorize/%3Fclient_id%3D9d836570317f4c18bca0db6d2ac38e29%26redirect_uri%3Dhttp%3A//websta.me/%26response_type%3Dcode%26scope%3Dcomments%2Brelationships%2Blikes";

            //        res_nextUrl = httpHelper.getHtmlfromUrl(new Uri(nextUrl), "");//postFormDataProxy
            //    }
            //    else
            //    {
            //        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logged in Failed with Account :" + InstagramUser.username + " ]");
            //        Status = "Failed";
            //        this.LoggedIn = false;
            //    }



            //    try
            //    {
            //        int FirstPointToken_nextUrl = res_nextUrl.IndexOf("csrfmiddlewaretoken");//csrfmiddlewaretoken
            //        string FirstTokenSubString_nextUrl = res_nextUrl.Substring(FirstPointToken_nextUrl);
            //        int SecondPointToken_nextUrl = FirstTokenSubString_nextUrl.IndexOf("/>");
            //        this.Token = FirstTokenSubString_nextUrl.Substring(0, SecondPointToken_nextUrl).Replace("csrfmiddlewaretoken", string.Empty).Replace("value=", string.Empty).Replace("\"", string.Empty).Replace("'", string.Empty).Trim();
            //    }
            //    catch { };


            //    string login = "https://instagram.com/accounts/login/?force_classic_login=&next=/oauth/authorize/%3Fclient_id%3D9d836570317f4c18bca0db6d2ac38e29%26redirect_uri%3Dhttp%3A//websta.me/%26response_type%3Dcode%26scope%3Dcomments%2Brelationships%2Blikes";


            //    string postdata_Login = string.Empty;
            //    string res_postdata_Login = string.Empty;
            //    try
            //    {
            //        postdata_Login = "csrfmiddlewaretoken=" + this.Token + "&username=" + InstagramUser.username + "&password=" + InstagramUser.password + "";
            //    }
            //    catch { };
            //    try
            //    {

            //        res_postdata_Login = httpHelper.postFormData(new Uri(login), postdata_Login, login, "");
            //        if(res_postdata_Login.Contains("value=\"Authorize\""))
            //    {
            //        string res_token= string.Empty;
            //        //string csrftoken = "https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
            //        try
            //        {
            //             res_token = httpHelper.getHtmlfromUrl(new Uri("https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes"), "https://instagram.com/accounts/login/?force_classic_login=&next=/oauth/authorize/%3Fclient_id%3D9d836570317f4c18bca0db6d2ac38e29%26redirect_uri%3Dhttp%3A//websta.me/%26response_type%3Dcode%26scope%3Dcomments%2Brelationships%2Blikes");
            //        }
            //        catch { };
            //        string csrftoken = Utils.getBetween(res_token, "\"csrfmiddlewaretoken\" value=\"", "\"/>");
            //        string login_Authorise="https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
            //        string postAuthorise = "csrfmiddlewaretoken=" + csrftoken + "&allow=Authorize";
            //        try
            //        {
            //            string res_postAuthorise = httpHelper.postFormData(new Uri(login_Authorise), postAuthorise, login_Authorise, "");
            //        }
            //        catch { };
            //    }

            //        if (res_postdata_Login.Contains("Authorization Request &mdash; Instagram"))
            //        {
            //            Authorization = "No";
            //        }
            //        else
            //        {
            //            Authorization = "Yes";
            //        }
            //        if (res_postdata_Login.Contains("Please register your email address from") || res_postdata_Login.Contains("Please register your Phone Number from"))
            //        {
            //            if (res_postdata_Login.Contains("Please register your Phone Number from"))
            //            {
            //                acc_status = "Phone";
            //            }
            //            else
            //            {
            //                acc_status = "Email";
            //            }

            //        }
            //        else
            //        {
            //            acc_status = "Ok";
            //        }
            //    }
            //    catch { };

            //    string autho = "https://instagram.com/oauth/authorize/?scope=comments+likes+relationships&redirect_uri=http%3A%2F%2Fwww.gramfeed.com%2Foauth%2Fcallback%3Fpage%3D&response_type=code&client_id=b59fbe4563944b6c88cced13495c0f49";

            //    if (res_postdata_Login.Contains("Please enter a correct username and password"))
            //    {
            //        Status = "Failed";
            //        this.LoggedIn = false;
            //    }
            //    else if (res_postdata_Login.Contains("requesting access to your Instagram account") || postdata_Login.Contains("is requesting to do the following"))
            //    {
            //        Status = "AccessIssue";
            //    }
            //    else if (res_postdata_Login.Contains("logout") || postdata_Login.Contains("LOG OUT"))
            //{
            //        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logged in with Account Success :" + InstagramUser.username + " ]");
            //        Status = "Success";
            //        this.LoggedIn = true;
            //        string str = httpHelper.getHtmlfromUrl(new Uri("http://websta.me/n/" + InstagramUser.username));
            //        UpdateCampaign(InstagramUser.username, str, "Success");
            //        InstagramUser.isloggedin = true;
            //    }
            //    else if (string.IsNullOrEmpty(res_secondURL))
            //    {

            //        Status = "Failed";
            //        this.LoggedIn = false;
            //        InstagramUser.isloggedin = false;
            //    }

            //    //nameval.Clear();
            //    return Status;
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;

            //}

            #endregion
            
            try
            {
                string Status = "Failed";
                string url = IGGlobals.Instance.IGInstagramurl;
                string firstResoponse = InstagramUser.globusHttpHelper.GetData_LoginThroughInstagram(new Uri(url), InstagramUser.proxyip, InstagramUser.proxyport, InstagramUser.proxyusername, InstagramUser.proxyport);
                string poatData = "username=" + InstagramUser.username + "&password=" + InstagramUser.password;  //csrftoken
                url = IGGlobals.Instance.IGInstagramurlsecond;
                string token = "";
                string response = InstagramUser.globusHttpHelper.PostData_LoginThroughInstagram(new Uri(url), poatData, "", token);
                string dataBeforelogin = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGInstagramAuthorizeurl), IGGlobals.Instance.IGWEPME, "");

                



                if (dataBeforelogin.Contains("Was This You?"))
             {
                 string crs_token  = Utils.getBetween(response,"=\"csrfmiddlewaretoken\" value=\"","\"/>");
                 string post_data = "csrfmiddlewaretoken=" + crs_token + "&approve=It+Was+Me";
                 string next_hit = InstagramUser.globusHttpHelper.PostData_LoginThroughInstagram(new Uri("https://www.instagram.com/integrity/checkpoint/?next=%2F"), post_data,"",crs_token);
                 string post_data2 = "csrfmiddlewaretoken="+crs_token+"&OK=OK";
                 string finalhit = InstagramUser.globusHttpHelper.PostData_LoginThroughInstagram(new Uri("https://www.instagram.com/integrity/checkpoint/?next=%2F"), post_data2, "", crs_token);
                 dataBeforelogin = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGInstagramAuthorizeurl), IGGlobals.Instance.IGWEPME, "");
             }

                //#region For icono

                //string Home_icon_Url = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri("http://iconosquare.com"), "");
                //string Icon_url = IGGlobals.Instance.IGiconosquareAuthorizeurl;
                //string PPagesource = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri(Icon_url), "");
                //string responce_icon = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGiconosquareviewUrl), "");

                //#endregion


                // dataBeforelogin = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGInstagramAuthorizeurl), IGGlobals.Instance.IGWEPME, "");
             //   dataBeforelogin = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGWebstaFeedUrl), IGGlobals.Instance.IGWEPME, "");
               // dataBeforelogin = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGInstagramAuthorizeurl), IGGlobals.Instance.IGWEPME, "");
             //   dataBeforelogin = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri(IGGlobals.Instance.IGWebstaFeedUrl), IGGlobals.Instance.IGWEPME, "");

                try
                {
                    if (dataBeforelogin.Contains("Authorization Request &mdash; Instagram"))
                    {
                        Authorization = "No";
                    }
                    else
                    {
                        Authorization = "Yes";
                    }
                    if (dataBeforelogin.Contains("Please register your email address from") || dataBeforelogin.Contains("Please register your Phone Number from"))
                    {
                        if (dataBeforelogin.Contains("Please register your Phone Number from"))
                        {
                            acc_status = "Phone";
                        }
                        else
                        {
                            acc_status = "Email";
                        }

                    }
                    else
                    {
                        acc_status = "Ok";
                    }
                }
                catch(Exception ex)
                {
                    GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
                }


                if (dataBeforelogin.Contains(InstagramUser.username.ToLower()))//marieturnipseed55614
                {
                    if (value)
                    {
                        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logged in with Account Success :" + InstagramUser.username + " ]");
                    }
                    InstagramUser.isloggedin = true;
                    Status = "Success";
                    this.LoggedIn = true;
                    string str = httpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/" + InstagramUser.username+"/"));
                    UpdateCampaign(InstagramUser.username, str, "Success");
                    value = false;
                    
                }
                else
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logged in with Account Fail :" + InstagramUser.username + " ]");
                }
                return Status;
            }
               
            catch
            {
                return null;
            };
            

        }

        public void UpdateCampaign(string account, string page, string status)
        {
            string post = "";
            string Follower = "";
            string Following = "";
            try
            {
                //string[] postsList = Regex.Split(page, "counts_media");
                post = Utils.getBetween(page, "media\":{\"count\":", ",\"");
            }
            catch { };

            try
            {
                //string[] FollowerList = Regex.Split(page, "counts_followed_by");
                Following = Utils.getBetween(page, "follows\":{\"count\":", "},");
            }
            catch { };

            try
            {
                //string[] FollowingList = Regex.Split(page, "following");
                Follower = Utils.getBetween(page, "followed_by\":{\"count\":", "},");
            }
            catch { };

            string Authorizationn = Authorization;
            string Status = acc_status;


            try
            {
                string query = "Update AccountInfo  set LogInStatus = '" + status + "',Posts = '" + post + "',Followers = '" + Follower + "',Followings ='" + Following + "',Authorized ='" + Authorization + "',Status ='" + Status + "' where Username= '" + account + "'";
                DataBaseHandler.InsertQuery(query, "AccountInfo");
                //GlobusLogHelper.log.Info(account + "   Checked");
            }
            catch { };

            
        }






    }
}
