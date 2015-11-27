using System;
using System.Collections.Generic;
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
using BaseLib;

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControlScrapeUserWhereToMention.xaml
    /// </summary>
    public partial class UserControlScrapeUserWhereToMention : UserControl
    {
        public UserControlScrapeUserWhereToMention()
        {
            InitializeComponent();
        }

        private void btn_ScrapeUser_MentionUser_BrowsePhotoIdorURL_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                string Path = dlg.ToString().Replace("Microsoft.Win32.OpenFileDialog: Title: , FileName","");
                if (result == true)
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        txt_ScrapeUsers_MentionUser_LoadPhotoIDOrUrlsPath.Text = dlg.FileName;
                    }));

                    List<string> lstTemp=Globussoft.GlobusFileHelper.ReadFiletoStringList(dlg.FileName);
                    GlobalDeclration.objScrapeUser.listOfPhotoIdAnd = lstTemp.Distinct().ToList();

                    GlobusLogHelper.log.Info(lstTemp.Count + " PhotoId Loaded ");
                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);  
            }
        }

        private void btn_ScrapeUser_MentionUser_BrowseMessageorURL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                string Path = dlg.ToString().Replace("Microsoft.Win32.OpenFileDialog: Title: , FileName", "");
                if (result == true)
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        txt_ScrapeUsers_MentionUser_LoadPhotoIDOrUrlsPath.Text = dlg.FileName;
                    }));

                    List<string> lstTemp = Globussoft.GlobusFileHelper.ReadFiletoStringList(dlg.FileName);
                    GlobalDeclration.objScrapeUser.listOfMessageToComment = lstTemp.Distinct().ToList();

                    GlobusLogHelper.log.Info(lstTemp.Count + " Message Loaded ");
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
            }
        }
    }
}
