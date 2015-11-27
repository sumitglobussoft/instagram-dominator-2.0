using BaseLib;
using BaseLibID;
using Globussoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyChecker
{
    public class ProxyManager
    {

        #region globul variable

        public static List<Thread> lstProxyThread = new List<Thread>();
        public static bool proxyStop = false;
        public static readonly object lockerforProxies = new object();
        public static readonly object lockerforNonWorkingProxies = new object();
        int countParseProxiesThreads = 0;
        int count_ThreadController = 0;
        static int Proxystatus = 0;
        public List<Thread> lstThreadsProxy = new List<Thread>();
        public static int Nothread_Proxy = 0;
        public static readonly object locker_finalProxyList = new object();
      



        #endregion
        public int NoOfThreadsProxy
        {
            get;
            set;
        }



        public void StartProxyChecker()
        {
            Thread.CurrentThread.IsBackground = true;
            lstProxyThread.Add(Thread.CurrentThread);
            lstProxyThread = lstProxyThread.Distinct().ToList();
            Proxystatus = ClGlobul.ProxyList.Count;

            GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Process For Proxy Checking ]");

            int numberOfThreads = ClGlobul.ProxyCheckNoOfThread;

            List<List<string>> list_Proxy = new List<List<string>>();

            list_Proxy = ListUtilities.Split(ClGlobul.ProxyList, numberOfThreads);


            #region Modified Proxy Check
            ThreadPool.SetMaxThreads(50, 50);
            int counter = 0;
            try
            {
                foreach (string itemProxy in ClGlobul.ProxyList)
                {
                    try
                    {
                        if (proxyStop)
                            return;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(getpageSourceFromProxy), new object[] { itemProxy });
                    }
                    catch { }
                }

            }
            catch (Exception ex)
            { }
            #endregion

            #region Previous Proxy Check
            //foreach (List<string> list_Proxy_ in list_Proxy)
            //{
            //    foreach (string list_Proxy_Item in list_Proxy_)
            //    {
            //        lock (lockr_ThreadController)
            //        {
            //            try
            //            {
            //                if (count_ThreadController >= list_Proxy_.Count)
            //                {
            //                    Monitor.Wait(lockr_ThreadController);
            //                }

            //                string Proxy_Item = list_Proxy_Item.Remove(list_Proxy_Item.IndexOf(':'));

            //                Thread likerThread = new Thread(getpageSourceFromProxy);
            //                likerThread.Name = "workerThread_Liker_" + Proxy_Item;
            //                likerThread.IsBackground = true;

            //                likerThread.Start(new object[] { list_Proxy_Item });

            //                count_ThreadController++;
            //            }
            //            catch (Exception ex)
            //            {

            //            }
            //        }
            //    }
            //}
            #endregion
        }

        public void getpageSourceFromProxy(object item)
        {
            if (proxyStop)
                return;
            try
            {
                Thread.CurrentThread.IsBackground = true;
                lstProxyThread.Add(Thread.CurrentThread);
                lstProxyThread = lstProxyThread.Distinct().ToList();
            }
            catch { }

            countParseProxiesThreads++;

            Array Item_value = (Array)item;
            string ClGlobul_ProxyList_item = (string)Item_value.GetValue(0);          
            Globussoft.GlobusHttpHelper GlobusHttpHelper = new Globussoft.GlobusHttpHelper();
            ChilkatHttpHelpr objchilkat = new ChilkatHttpHelpr();
            string proxyad = string.Empty;
            string proxyport = string.Empty;
            string proxyusername = string.Empty;
            string proxyPassword = string.Empty;
            string pagesource1 = string.Empty;
            string pagesource = string.Empty;


            try
            {
                string[] proxyLst = ClGlobul_ProxyList_item.Split(':');
                if (proxyLst.Count() > 3)
                {
                    proxyad = proxyLst[0];
                    proxyport = proxyLst[1];
                    proxyusername = proxyLst[2];
                    proxyPassword = proxyLst[3];
                }
                else if (proxyLst.Count() > 0 && proxyLst.Count() < 3)
                {
                    proxyad = proxyLst[0];
                    proxyport = proxyLst[1];
                }
                else
                {
                    return;
                }

                try
                {
                    if (proxyStop)
                        return;
                    //pagesource1 = GlobusHttpHelper.getHtmlfromUrlProxy(new Uri("http://websta.me/login"),proxyad, Convert.ToInt16(proxyport), proxyusername, proxyPassword);
                    pagesource1 = GlobusHttpHelper.getHtmlfromUrlProxy(new Uri("http://websta.me/"), proxyad, Convert.ToInt16(proxyport), proxyusername, proxyPassword);
                }
                catch { };

                if (string.IsNullOrEmpty(pagesource1))
                {
                    pagesource1 = string.Empty;
                    // pagesource1 = GlobusHttpHelper.getHtmlfromUrlProxy(new Uri("http://web.stagram.com/"), proxyad, Convert.ToInt32(proxyport), proxyusername, proxyPassword);
                    pagesource1 = objchilkat.GetHtmlProxy("http://websta.me/", proxyad, (proxyport), proxyusername, proxyPassword);
                }
                if (pagesource1.Contains("Access Denied"))
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Proxy Is not Working : " + proxyad + ":" + proxyport + ":" + proxyusername + ":" + proxyPassword + " ]");
                }


                //int FirstPointClientId = pagesource1.IndexOf("client_id=");
                //string FirstClientIdSubString = pagesource1.Substring(FirstPointClientId);
                //int SecondPointClientId = FirstClientIdSubString.IndexOf("&redirect_uri=");
                //string ClientId = FirstClientIdSubString.Substring(0, SecondPointClientId).Replace("'", string.Empty).Replace("client_id=", string.Empty).Trim();

                //string LoginUrl = "https://instagram.com/accounts/login/?next=/oauth/authorize/%3Fclient_id%3D" + ClientId + "%26redirect_uri%3Dhttp%253A%252F%252Fweb.stagram.com%252F%26response_type%3Dcode%26scope%3Dlikes%2Bcomments%2Brelationships";

                //pagesource = GlobusHttpHelper.getHtmlfromUrlProxy(new Uri(LoginUrl), proxyad, Convert.ToInt16(proxyport), proxyusername, proxyPassword);

                //if (string.IsNullOrEmpty(pagesource))
                //{
                //    pagesource = string.Empty;
                //    pagesource = GlobusHttpHelper.getHtmlfromUrlProxy(new Uri(LoginUrl), proxyad, Convert.ToInt16(proxyport), proxyusername, proxyPassword);
                //}

                //ADD in List list of Finally chacked.....
                if (!string.IsNullOrEmpty(pagesource1))
                {
                    if (proxyStop)
                        return;
                    addInFinalCheckedProxyist(proxyad, proxyport, proxyusername, proxyPassword, pagesource1);
                }
                else
                {
                    if (proxyStop)
                        return;
                    ClGlobul.isProxyCheckComplete = true;
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Proxy Is not Working : " + proxyad + ":" + proxyport + ":" + proxyusername + ":" + proxyPassword + " ]");
                    lock (lockerforNonWorkingProxies)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(proxyad + ":" + proxyport + ":" + proxyusername + ":" + proxyPassword, GlobusFileHelper.NonWorkingProxiesList);
                    }
                }

            }
            catch (Exception)
            {
                if (proxyStop)
                    return;
                GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Proxy Is not Working : " + proxyad + ":" + proxyport + ":" + proxyusername + ":" + proxyPassword + " ]");

                lock (lockerforNonWorkingProxies)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(proxyad + ":" + proxyport + ":" + proxyusername + ":" + proxyPassword, GlobusFileHelper.NonWorkingProxiesList);
                }
            }
            finally
            {

                lock (lockerforProxies)
                {
                    countParseProxiesThreads--;
                    Monitor.Pulse(lockerforProxies);
                }
                Proxystatus--;
                if (Proxystatus == 0)
                {
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    GlobusLogHelper.log.Info("-----------------------------------------------------------------------------------------------------------------------------------");
                }
            }

        }


        public void addInFinalCheckedProxyist(string proxyad, string proxyport, string proxyusername, string proxyPassword, string pagesource)
        {
            if (checkStatuse(pagesource))
            {
                if (proxyStop)
                    return;
                try
                {
                    if (proxyStop)
                        return;
                    GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Working Proxy : " + proxyad + ":" + proxyport + " ]");
                   ClGlobul.isProxyCheckComplete = true;
                }
                catch { };


                if (!string.IsNullOrEmpty(proxyusername) && !string.IsNullOrEmpty(proxyPassword) && !string.IsNullOrEmpty(proxyad) && !string.IsNullOrEmpty(proxyport))
                {
                    if (proxyStop)
                        return;
                    try
                    {
                        string add = proxyad + ":" + proxyport + ":" + proxyusername + ":" + proxyPassword;
                        lock (lockerforProxies)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(add, GlobusFileHelper.WorkingProxiesList);
                        }
                        lock (locker_finalProxyList)
                        {
                            ClGlobul.finalProxyList.Add(add);
                            Monitor.Pulse(locker_finalProxyList);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }


                else if (string.IsNullOrEmpty(proxyusername) && string.IsNullOrEmpty(proxyPassword) && !string.IsNullOrEmpty(proxyad) && !string.IsNullOrEmpty(proxyport))
                {
                    try
                    {
                        if (proxyStop)
                            return;
                        string add = proxyad + ":" + proxyport;
                        //lock (lockerforNonWorkingProxies)
                        //{
                        if (proxyStop)
                            return;
                        GlobusFileHelper.AppendStringToTextfileNewLine(add, GlobusFileHelper.WorkingProxiesList);

                        //}
                        //lock (locker_finalProxyList)
                        //{
                        ClGlobul.finalProxyList.Add(add);
                        Monitor.Pulse(locker_finalProxyList);
                        //}
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {

                }
            }
        }

        public bool checkStatuse(string page)
        {
            //if (page.Contains("<input type=\"submit\" class=\"button-green\" value=\"Log in\" />"))
            if (page.Contains("<a href=\"/login\">LOG IN</a></li>"))
            {
                return true;
            }

            return false;
        } 

    }
}
