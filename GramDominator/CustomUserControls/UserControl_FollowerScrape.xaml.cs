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
using System.Data;
using FirstFloor.ModernUI.Windows.Controls;

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControl_FollowerScrape.xaml
    /// </summary>
    public partial class UserControl_FollowerScrape : UserControl
    {
        public static UserControl_FollowerScrape objUserControl_FollowerScrape;

        public UserControl_FollowerScrape()
        {
            InitializeComponent();
            showDetails();
            //objUserControl_FollowerScrape = this;
        }

        private void dgv_FollowerDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        public void showDetails()
        {
            try 
            {
                string query = "select *from tb_scrape_follower where username='"+Globals.selectedUsername+"' ";
                DataSet ds = DataBaseHandler.SelectQuery(query, "tb_scrape_follower");
                DataView dv = ds.Tables[0].DefaultView;
                dgv_FollowerDetails.ItemsSource = dv;
            }
            catch { };
        }
    }
}
