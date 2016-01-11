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
using BaseLibID;
using HashTagsManager;
using FirstFloor.ModernUI.Windows.Controls;
<<<<<<< HEAD
using Follower;
using Comment;
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f

namespace GramDominator.CustomUserControls
{
    /// <summary>
    /// Interaction logic for UserControldividedata.xaml
    /// </summary>
    public partial class UserControldividedata : UserControl
    {
        public UserControldividedata()
        {
            InitializeComponent();
        }
        hash_managerlibry obj_hash_managerlibry = new hash_managerlibry();
        private void btnUsercontrolFollowSaveLoadedData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(rdo_DivideData_Equally.IsChecked==true)
                {
                    hash_managerlibry.DivideEqual = true;
<<<<<<< HEAD
                   
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
                if(rdo_DivideData_Mannully.IsChecked == true)
                {
                    hash_managerlibry.DivideByUser = true;
                    hash_managerlibry.Divide_data_NoUser = Convert.ToInt32(txt_DivideNoperUser.Text);
                    hash_managerlibry.DivideData_Thread = Convert.ToInt32(txt_Divide_noThread.Text);
<<<<<<< HEAD
                    FollowerFollowing.Divide_data_NoUser = Convert.ToInt32(txt_DivideNoperUser.Text);
                    FollowerFollowing.DivideData_Thread = Convert.ToInt32(txt_Divide_noThread.Text);
                    CommentManager.Divide_data_NoUser = Convert.ToInt32(txt_DivideNoperUser.Text);
                    CommentManager.DivideData_Thread = Convert.ToInt32(txt_Divide_noThread.Text);
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
                    ModernDialog.ShowMessage("Your Data Has Been Saved Successfully!!", "Success Message", MessageBoxButton.OK);
                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error :" + ex.StackTrace);
            }
        }

        private void rdo_DivideData_Equally_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                hash_managerlibry.DivideEqual = true;
                txt_DivideNoperUser.IsEnabled = false;
                txt_Divide_noThread.IsEnabled = false;
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }



        hash_managerlibry obj_obj_UserControldividedata = new hash_managerlibry();

        private void rdo_DivideData_Mannully_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_DivideNoperUser.IsEnabled =true;
                txt_Divide_noThread.IsEnabled = true;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Info("Error : " + ex.StackTrace);
            }
        }
    }
}
