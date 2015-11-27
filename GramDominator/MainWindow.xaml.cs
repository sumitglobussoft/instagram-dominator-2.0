
using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GramDominator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public static MainWindow mainFormReference = null;
        public MainWindow()
        {
            XmlConfigurator.Configure();
            mainFormReference = this;
            InitializeComponent();
            GlobusLogHelper.log.Info("Test");
            CopyDatabase();
            makeFileScraper();
            
            
        }

        public static string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator\\Downloaded_Image";
        public static string filepathScraper = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator\\Scrape_Followers";
        public static string CSVPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Gram Dominator\\Scrape_Followers\\";

        private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxButton btnUsed = MessageBoxButton.YesNo;
            var objDialogresult = ModernDialog.ShowMessage("Do you Really want to exit?", "GramDominator 2.0", btnUsed);
            if (objDialogresult.ToString().Equals("Yes"))
            {
                var prc = System.Diagnostics.Process.GetProcesses();
                foreach (var item in prc)
                {
                    try
                    {
                        if (item.ProcessName.Contains("GramDominator"))
                        {
                            item.Kill();
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
                    }                   
                }
                this.Close();
            }
            else
            {
                e.Cancel = true;
                this.Activate();
            }
        }

        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public void CopyDatabase()                                                 //*--------Copy DataBase method--------*//
        {
            
            string startupDb = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\GramDominator.db";
            string localAppDbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\gramdominator_db\\GramDominator.db";
            string startAppDbPath86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + "\\GramDominator.db";

            if (!File.Exists(localAppDbPath))
            {
                if (File.Exists(startupDb))
                {
                    try
                    {
                        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\gramdominator_db");
                        File.Copy(startupDb, localAppDbPath);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Could not find a part of the path"))
                        {
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\gramdominator_db");
                            File.Copy(startupDb, localAppDbPath);
                        }
                    }
                }
                else if (File.Exists(startAppDbPath86))   //for 64 Bit
                {
                    try
                    {
                        File.Copy(startAppDbPath86, localAppDbPath);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Could not find a part of the path"))
                        {
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\GramDominator.db");
                            File.Copy(startAppDbPath86, localAppDbPath);
                        }
                    }
                }
            }
        }


        public void makeFileScraper()
        {
            //Make Folder 
            try
            {
                if (!Directory.Exists(GlobusFileHelper.path_AppDataFolder))
                {
                    Directory.CreateDirectory(GlobusFileHelper.path_AppDataFolder);
                }
                if (!Directory.Exists(filepathScraper))
                {
                    Directory.CreateDirectory(filepathScraper);
                }
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
            }
            catch (Exception)
            {
            }
        }

       
       




       
    }
    #region LogFornetclass
    public class GlobusLogAppender : log4net.Appender.AppenderSkeleton
    {

        private static readonly object lockerLog4Append = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggingEvent"></param>
        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            try
            {
                string loggerName = loggingEvent.Level.Name;

                MainWindow frmGramDominator = MainWindow.mainFormReference ;
              

                lock (lockerLog4Append)
                {
                    switch (loggingEvent.Level.Name)
                    {
                        case "DEBUG":
                            try
                            {

                                {
                                    if (!frmGramDominator.lstLogger.Dispatcher.CheckAccess())
                                    {
                                        frmGramDominator.lstLogger.Dispatcher.Invoke(new Action(delegate
                                        {
                                            try
                                            {
                                                if (frmGramDominator.lstLogger.Items.Count > 1000)
                                                {
                                                    frmGramDominator.lstLogger.Items.RemoveAt(frmGramDominator.lstLogger.Items.Count - 1);//.Add(frmDominator.listBoxLogs.Items.Add(loggingEvent.TimeStamp + "\t" + loggingEvent.LoggerName + "\r\t\t" + loggingEvent.RenderedMessage);
                                                }

                                                frmGramDominator.lstLogger.Items.Insert(0, loggingEvent.TimeStamp + "\t" + "GramDominator 2.0" + "\r\t" + loggingEvent.RenderedMessage);
                                            }
                                            catch (Exception ex)
                                            {
                                               GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);
                                            }

                                        }));

                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (frmGramDominator.lstLogger.Items.Count > 1000)
                                            {
                                                frmGramDominator.lstLogger.Items.RemoveAt(frmGramDominator.lstLogger.Items.Count - 1);
                                            }

                                            frmGramDominator.lstLogger.Items.Insert(0, loggingEvent.TimeStamp + "\t" + "GramDominator 2.0 " + "\r\t" + loggingEvent.RenderedMessage);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error : 74" + ex.Message);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error Case Debug : " + ex.StackTrace);
                                Console.WriteLine("Error Case Debug : " + ex.Message);
                              // GlobusLogHelper.log.Error(" Error : " + ex.Message);
                            }
                            break;
                        case "INFO":
                            try
                            {


                                if (!frmGramDominator.lstLogger.Dispatcher.CheckAccess())
                                {
                                    frmGramDominator.lstLogger.Dispatcher.Invoke(new Action(delegate
                                    {
                                        try
                                        {
                                            if (frmGramDominator.lstLogger.Items.Count > 1000)
                                            {
                                                frmGramDominator.lstLogger.Items.RemoveAt(frmGramDominator.lstLogger.Items.Count - 1);
                                            }

                                            frmGramDominator.lstLogger.Items.Insert(0, loggingEvent.TimeStamp + "\t" + " GramDominator 2.0 " + "\t\t" + loggingEvent.RenderedMessage);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);
                                        }

                                    }));

                                }
                                else
                                {
                                    try
                                    {
                                        if (frmGramDominator.lstLogger.Items.Count > 1000)
                                        {
                                            frmGramDominator.lstLogger.Items.RemoveAt(frmGramDominator.lstLogger.Items.Count - 1);
                                        }

                                        frmGramDominator.lstLogger.Items.Insert(0, loggingEvent.TimeStamp + "\t" + "GramDominator 2.0 " + "\t\t" + loggingEvent.RenderedMessage);
                                    }
                                    catch (Exception ex)
                                    {
                                      // GlobusLogHelper.log.Error("Error : 75" + ex.Message);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error Case INFO : " + ex.StackTrace);
                                Console.WriteLine("Error Case INFO : " + ex.Message);
                               // GlobusLogHelper.log.Error(" Error : " + ex.Message);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
               // GlobusLogHelper.log.Error("Error : 76" + ex.Message);
            }

        }


    }
    #endregion
}
