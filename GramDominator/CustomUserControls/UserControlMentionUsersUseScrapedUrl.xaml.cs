using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for UserControlMentionUsersUseScrapedUrl.xaml
    /// </summary>
    public partial class UserControlMentionUsersUseScrapedUrl : UserControl
    {
        public UserControlMentionUsersUseScrapedUrl()
        {
            InitializeComponent();
            bindScrapedUrl();
        }

        public void bindScrapedUrl()
        {
            try
            {
                DataSet DS = DataBaseHandler.SelectQuery("select ImageURL from ScrapedImage", "ScrapedImage");

                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        cmbBox_MentionUser_UseScrapedUrl_LstOfUrls.Items.Add(new CheckBox() { Content = dr.ItemArray[0].ToString() });
                    }));
                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
            }
        }

        private void btn_MentionUsers_UseScrapedData_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(cmbBox_MentionUser_UseScrapedUrl_LstOfUrls.Text))
                {
                    List<CheckBox> temp = new List<CheckBox>();
                    foreach (CheckBox item in cmbBox_MentionUser_UseScrapedUrl_LstOfUrls.Items)
                    {
                        temp.Add(item);
                    }

                    if (temp.Where(x => x.IsChecked == true).ToList().Count == 0)
                    {
                        GlobusLogHelper.log.Info("Please Select Atleast One Account ");
                        return;
                    }
                    if (temp.Count > 0)
                    {
                        foreach (CheckBox item in temp)
                        {
                            if (item.IsChecked == true)
                            {
                                GlobalDeclration.objMentionUser.listOfUrlToMentionUser.Add(item.Content.ToString());
                            }
                        }

                    }      
                    //GlobalDeclration.objMentionUser.listOfUrlToMentionUser.Add(cmbBox_MentionUser_UseScrapedUrl_LstOfUrls.Text);
                    ModernDialog.ShowMessage("Your Data Has Been Saved Succefully", "Select Url", MessageBoxButton.OK);
                }
                else
                {
                    GlobusLogHelper.log.Info("Please Select Url From Dropdown List");
                    ModernDialog.ShowMessage("Please Select Url From Dropdown List", "Select Url", MessageBoxButton.OK);
                    cmbBox_MentionUser_UseScrapedUrl_LstOfUrls.Focus();
                    return;
                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
            }
        }
    }
}
