using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using UsingUsercontrol;

namespace GramDominator.Pages.Using_Username
{
    /// <summary>
    /// Interaction logic for UsercontrolUsingUserName.xaml
    /// </summary>
    public partial class UsercontrolUsingUserName : UserControl
    {
        public UsercontrolUsingUserName()
        {
            InitializeComponent();
        }

        Utils objUtils = new Utils();
        private void btn_UsingUsername_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                objj_UsingUsernameManager.isStopUsingUsername = false;
                objj_UsingUsernameManager.lstThreadsUsingUsername.Clear();

                Regex checkNo = new Regex("^[0-9]*$");

                int processorCount = objUtils.GetProcessor();

                int threads = 25;

                int maxThread = 25 * processorCount;
                try
                {
                    UsingUsernameManager.minDelayUsingUsername = Convert.ToInt32(txt_UsingUsername_DelayMin.Text);
                    UsingUsernameManager.maxDelayUsingUsername = Convert.ToInt32(txt_UsingUsername_DelayMax.Text);
                    UsingUsernameManager.NothreadUsingUsername = Convert.ToInt32(txt_UsingUsername_NoOfThreads.Text);
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    return;
                }

                if (!string.IsNullOrEmpty(txt_UsingUsername_NoOfThreads.Text) && checkNo.IsMatch(txt_UsingUsername_NoOfThreads.Text))
                {
                    threads = Convert.ToInt32(txt_UsingUsername_NoOfThreads.Text);
                }

                if (threads > maxThread)
                {
                    threads = 25;
                }
                objj_UsingUsernameManager.NoOfThreadsUsingUsername = threads;



                Thread CommentPosterThread = new Thread(objj_UsingUsernameManager.StartUsingUsername);
                CommentPosterThread.Start();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        UsingUsernameManager objj_UsingUsernameManager = new UsingUsernameManager();

        private void btn_UsingUsername_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objj_UsingUsernameManager.isStopUsingUsername = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = objj_UsingUsernameManager.lstThreadsUsingUsername.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        objj_UsingUsernameManager.lstThreadsUsingUsername.Remove(item);
                    }
                    catch (Exception ex)
                    {
                        //Thread.ResetAbort();
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            GlobusLogHelper.log.Info("Process Stopped !");
            GlobusLogHelper.log.Debug("Process Stopped !");
        }

        private void Select_To_LikePhoto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Select_To_LikePhoto.SelectedIndex == 1)
                {
                    var window = new ModernDialog
                    {
                        Content = new UsercontrolUsingUsernamelike()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
                if (Select_To_LikePhoto.SelectedIndex == 2)
                {
                    var window = new ModernDialog
                    {
                        Content = new UsercontrolUsingUsernamecomment()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
                if (Select_To_LikePhoto.SelectedIndex == 3)
                {
                    var window = new ModernDialog
                    {
                        Content = new UserControlUsingUsernamelikeandcomment()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

       
    }
}
