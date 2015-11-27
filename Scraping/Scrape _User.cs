using BaseLib;
using BaseLibID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scraping
{
    class Scrape__User
    {

        #region Global Variable

        int countThreadControllerScrape_User = 0;



        #endregion

        //public int NoOfThreadsScarpeUser
        //{
        //    get;
        //    set;
        //}



        //public void StartLikePoster()
        //{
        //    countThreadControllerScrape_User = 0;
        //    try
        //    {
        //        int numberOfAccountPatch = 25;

        //        if (NoOfThreadsScarpeUser > 0)
        //        {
        //            numberOfAccountPatch = NoOfThreadsScarpeUser;
        //        }

        //        List<List<string>> list_listAccounts = new List<List<string>>();
        //        if (IGGlobals.listAccounts.Count >= 1)
        //        {

        //            list_listAccounts = Utils.Split(IGGlobals.listAccounts, numberOfAccountPatch);

        //            foreach (List<string> listAccounts in list_listAccounts)
        //            {

        //                foreach (string account in listAccounts)
        //                {
        //                    try
        //                    {
        //                        lock (lockrThreadControlleScrapeUser)
        //                        {
        //                            try
        //                            {
        //                                if (countThreadControllerScrapeUser >= listAccounts.Count)
        //                                {
        //                                    Monitor.Wait(lockrThreadControlleScrapeUser);
        //                                }

        //                                string acc = account.Remove(account.IndexOf(':'));

        //                                //Run a separate thread for each account
        //                                InstagramUser item = null;
        //                                IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);


        //                                if (item != null)
        //                                {
        //                                    Thread profilerThread = new Thread(StartMultiThreadsScrapeModule);
        //                                    profilerThread.Name = "workerThread_Profiler_" + acc;
        //                                    profilerThread.IsBackground = true;

        //                                    profilerThread.Start(new object[] { item });

        //                                    countThreadControllerScrapeUser++;
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
        //    }
        //}




        public void Scrape_userpholiker()
        {
            try
            {

            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error:" + ex.StackTrace);
            }
        }

    }
}
