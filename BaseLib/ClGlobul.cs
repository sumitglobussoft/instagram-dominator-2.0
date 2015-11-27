using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaseLibID
{
   public class ClGlobul
    {
        public static int accountIndexForLoopingBack = 0;
        public static bool switchAccount = false;
        public static List<string> lstUnfollowerList = new List<string>();
        public static List<string> DM_Messagelist = new List<string>();
        public static List<string> campaignfollowingList = new List<string>();
        public static List<string> campaignPhotoIDList = new List<string>();
        public static List<string> UsingUsername_onlycommentusernameList = new List<string>();
        public static List<string> Campiagn_CommentList = new List<string>();
        public static List<string> Campiagn_Comment_PhotoIDList = new List<string>();
        public static List<string> Workingproxylist = new List<string>();
        public static List<string> UsingUsername_onlycommentmessageList = new List<string>();
        public static List<string> UsingUsername_likecommentUserList = new List<string>();
        public static List<string> UsingUsername_likecommentMessageList = new List<string>();
        public static List<string> UsingUsername_likecommentFollowerpicture = new List<string>();
        public static List<string> ListPassword = new List<string>();
        public static List<string> UsingUername_likecommentPhotoIDList = new List<string>();
        public static List<string> UsingUsername_commentFollowerpicture = new List<string>();
        public static List<string> UsingUername_commentPhotoIDList = new List<string>();
        public static List<string> DM_UserList = new List<string>();
        public static List<string> ListUsername_Manageprofile = new List<string>();
        public static List<string> lstStoreDownloadImageKeyword = new List<string>();
        public static List<string> UsingUsername_Usernmaelist = new List<string>();
        public static List<string> UsingUsername_likeFollowerpicture = new List<string>();
        public static List<string> UsingUername_PhotoIDList = new List<string>();

        public static List<string> listOfScrapeFollowerUserame = new List<string>();
        public static int ProxyCheckNoOfThread = 0;
        public static int NoOfPhotoLikeThread = 0;
        public static int NoOfcommentThread = 0;
        public static int NumberOfProfilesToFollow = 0;
        public static int NumberofSnapsVideosToLike = 0;
        public static int SnapVideosCounter = 0;
        public static int SnapVideosCounterComment = 0;
        public static int SnapVideosCounterfollow = 0;
        public static int NumberofSnapsVideosToComment = 0;
        public static int proxyIndex = 0;
       public static List<string> list_Account = new List<string>();
        public static bool checkHashTagLiker = false;
        public static bool checkHashTagComment = false;
        public static bool isCommentAndLikeChecked = false;
        public static bool isFollowLimitReached = false;
        public static bool isLikeLimitReached = false;
        public static bool isCommentLimitReached = false;
        public static bool isHashTagFollowComplete = false;
        public static bool isHashTagLikeComplete = false;
        public static bool isHashTagCommentComplete = false;
        public static int hashTagDelay = 10;
        public static int hashTagFolloweDelay = 10;
        public static bool isUnfollowSingle = false;
        public static bool isProxyCheckComplete = false;
        public static bool isStopScrapeFollowers = false;
        public static int countNoOFAccountHashFollower = 0;
        public static int countNoOFAccountHashComment = 0;
        public static int countNoOFAccountHashLike = 0;
        public static int countNOOfFollowersandImageDownload = 0;
        public static string FollowerListUploadedPath = "";
       
        public static string UnFollowerListUploadedPath = "";
        public static List<string> lstUrls = new List<string>();

        public static List<string> listOfAccountToAddInGroup = new List<string>();
        public static string scrapeFollowerAndFollowingUsername = string.Empty;

       
        public static List<string> ProxyList = new List<string>();
        public static List<string> accountList = new List<string>();
        public static List<string> followingList = new List<string>();
        public static List<string> UnfollowingList = new List<string>();
        public static List<string> finalProxyList = new List<string>();
        public static List<string> PhotoList = new List<string>();
        public static List<string> Userlist = new List<string>();
        public static List<string> userlist_liker = new List<string>();
        public static List<string> user_listlike = new List<string>();
        public static List<string> commentMsgList = new List<string>();
        public static List<string> CommentIdsForMSG = new List<string>();
        public static List<string> HashTagForScrap = new List<string>();
        public static List<string> ImageTagForScrap = new List<string>();
        public static Queue<string[]> ProxyQueue = new Queue<string[]>();
        public static List<string> FolloConpletedList = new List<string>();
        public static List<string> photoLikesCompletedList = new List<string>();
        public static List<string> HashFollower = new List<string>();
        public static List<string> HashLiker = new List<string>();
        public static List<string> HashComment = new List<string>();
        public static List<string> HashCommentMessage = new List<string>();
        public static List<string> listUsernameScrapeFollowers = new List<string>();
        public static List<Thread> lstThreadsScrapeFollowers = new List<Thread>();
        public static List<string> SingalPhotoUser = new List<string>();
        public static string proxAdd = "60.169.78.218";
        public static string proxyPort = "808";
        public static string proxyUser = "";
        public static string proxyPass = "";

        public static DateTime yourDate = new DateTime();
        public static bool insertedTime = false;
        public static bool oneHourProcessCompleted = false;
        public static bool userOverFollowing = false;
        public static bool userOverFollower = false;

        public static Dictionary<string, Thread> ThreadList = new Dictionary<string, Thread>();
        public static readonly object locker_QueueProxyList = new object();
        public static int TotalNoOfFollow = 0;
        public static int TotalNoOfIdsForFollow = 0;
        //Comment globul file ...
        public static List<string> CommentCompletedList = new List<string>();
        public static List<string> NotCommentList = new List<string>();

        public static List<string> lstHashTagUserIdTemp = new List<string>();
       public static List<string> lstHashTagUserId = new List<string>();
       public static List<string> lstPhotoId = new List<string>();
        #region New Like Module

        public static int PhotosCount = 0;
        public static int UsersCount = 0;
        public static List<string> lstUsername = new List<string>();
        public static bool StartLikerLike = false;

        #endregion

        public static void AddProxyInQueur()
        {
            if (finalProxyList.Count != 0)
            {
                foreach (var item in finalProxyList)
                {
                    ProxyQueue.Enqueue(new string[] { item });
                }
            }
        }


    }
}
