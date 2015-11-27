using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
using GramDominator.CustomUserControls;
using HashTagsManager;
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


namespace GramDominator.Pages.PageHashTags
{
    /// <summary>
    /// Interaction logic for UserControlHashTage.xaml
    /// </summary>
    public partial class UserControlHashTage : UserControl
    {
        public UserControlHashTage()
        {
            InitializeComponent();
        }

        private void Select_To_HashTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Select_To_TagsOption.SelectedIndex == 1)
                {
                    var window = new ModernDialog
                    {
                        Content = new UserControlHashTagsLikeModule()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
                if (Select_To_TagsOption.SelectedIndex == 2)
                {
                    var window = new ModernDialog
                    {
                         Content = new UserControlHashTagsComment()
                    };
                    window.MinHeight = 300;
                    window.MinWidth = 700;
                    window.ShowDialog();
                }
                if (Select_To_TagsOption.SelectedIndex == 3)
                {
                    var window = new ModernDialog
                    {
                        Content = new UserControlHashtagsfollower()
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

        Utils objUtils = new Utils();
        private void btnMessage_HashTags_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                obj_hash_managerlibry.isStopHash_comment = false;
                obj_hash_managerlibry.lstThreadsHash_comment.Clear();

                Regex checkNo = new Regex("^[0-9]*$");

                int processorCount = objUtils.GetProcessor();

                int threads = 25;

                int maxThread = 25 * processorCount;
                try
                {
                    hash_managerlibry.minDelayHash_comment = Convert.ToInt32(txtMessage_HashTags_DelayMin.Text);
                    hash_managerlibry.maxDelayHash_comment = Convert.ToInt32(txtMessage_HashTags_DelayMax.Text);
                    hash_managerlibry.NothreadHash_comment = Convert.ToInt32(txtMessage_HashTags_NoOfThreads.Text);
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Enter in Correct Format");
                    return;
                }

                if (!string.IsNullOrEmpty(txtMessage_HashTags_NoOfThreads.Text) && checkNo.IsMatch(txtMessage_HashTags_NoOfThreads.Text))
                {
                    threads = Convert.ToInt32(txtMessage_HashTags_NoOfThreads.Text);
                }

                if (threads > maxThread)
                {
                    threads = 25;
                }
                obj_hash_managerlibry.NoOfThreadsHash_comment = threads;


                if (hash_managerlibry.DivideByUser == true)
                {
                    Thread ForDivideUser = new Thread(obj_hash_managerlibry.StartDivideUser);
                    ForDivideUser.Start();
                }
                else
                {
                    if (hash_managerlibry.DivideEqual == true)
                    {
                        Thread ForDivideEqual = new Thread(obj_hash_managerlibry.StartDivide);
                        ForDivideEqual.Start();
                    }
                    else
                    {
                        Thread CommentPosterThread = new Thread(obj_hash_managerlibry.StartHash_comment);
                        CommentPosterThread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        hash_managerlibry obj_hash_managerlibry = new hash_managerlibry();
       
        private void btnMessage_HashTags_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                obj_hash_managerlibry.isStopHash_comment = true;

                List<Thread> lstTemp = new List<Thread>();
                lstTemp = obj_hash_managerlibry.lstThreadsHash_comment.Distinct().ToList();

                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                        obj_hash_managerlibry.lstThreadsHash_comment.Remove(item);
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
    }
}
