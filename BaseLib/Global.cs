using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace BaseLib
{
   public class Global
    {
        static string EmailsFilePath = string.Empty;

        public static List<Thread> HasTagListListThread = new List<Thread>();
        public static List<Thread> lstThread = new List<Thread>();
        
        public static List<Thread> lstThreads = new List<Thread>();
        public static List<Thread> lstScrapThreaddata = new List<Thread>();
        public static List<Thread> lstScrapThreaddataunfollower = new List<Thread>();
        public static List<Thread> lstScrapThreaddatahashliker = new List<Thread>();
        public static bool chkDivideDataFollow1 = false;
        public static bool rdbDivideEqually1 = false;
        public static bool rdbDivideGivenByUser1 = false;
        public static string txtDiveideByUser1 = "";
        public static string textBox4 = "";
        
        public static string FbAccountDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\FaceDominatorFbAccount";
        public static string imageDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\HashtagImageSave";


        public static List<string> lstDesktopFilePaths = new List<string>() { Path.Combine(FbAccountDesktopPath, "DisableFbAccount.txt"), Path.Combine(FbAccountDesktopPath, "IncorrectFbAccount.txt"), Path.Combine(FbAccountDesktopPath, "PhoneVerifyFbAccount.txt"), Path.Combine(FbAccountDesktopPath, "CorrectFbAccount.txt"), Path.Combine(FbAccountDesktopPath, "TemporarilyFbAccount.txt"), Path.Combine(FbAccountDesktopPath, "AccountNotConfirmed.txt"), Path.Combine(FbAccountDesktopPath, "CorrectFbAccount.txt"), Path.Combine(FbAccountDesktopPath, "Uploadimages.txt") };

        Regex IdCheck = new Regex("^[0-9]*$");
    }

   
    }

