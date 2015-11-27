using BaseLib;
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

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControlMentionUsersUploadUrls.xaml
    /// </summary>
    public partial class UserControlMentionUsersUploadUrls : UserControl
    {
        public UserControlMentionUsersUploadUrls()
        {
            InitializeComponent();
        }

        private void btn_MentionUser_UploadUrls_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        txt_MentionUser_LoadUrls_UploadUrlsFilePath.Text = dlg.FileName;
                    }));

                    List<string> tmpList = Globussoft.GlobusFileHelper.ReadFiletoStringList(dlg.FileName);
                    GlobalDeclration.objMentionUser.listOfUrlToMentionUser = tmpList.Distinct().ToList();

                    GlobusLogHelper.log.Info(tmpList.Count + " Urls Uploaded ");

                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
            }
        }
    }
}
