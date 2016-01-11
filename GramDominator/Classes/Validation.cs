using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;
using Comment;

namespace GramDominator.Classes
{
<<<<<<< HEAD
    public class Validation : NotifyPropertyChanged, IDataErrorInfo
=======
<<<<<<< HEAD
    public class Validation : NotifyPropertyChanged, IDataErrorInfo
=======
    class Validation : NotifyPropertyChanged, IDataErrorInfo
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
    {
        private string _delaymaximum = string.Empty;
        private string _delayminimum = string.Empty;
        private string _NoThread = string.Empty;
        private string _UserTxtField = string.Empty;
        private string _UserTxtField_two = string.Empty;
        private string _UserTxtField_three = string.Empty;
        private string _password = string.Empty;
        private string _NoFollower = string.Empty;
        private string _nounfollow = string.Empty;
        private string _NoPhoto = string.Empty;
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
        private string _Usernmame = string.Empty;
        private bool _Ischecked;

       // public string Usernmame { get; set; }

        //public bool Ischecked { get; set; }


        public string Usernmame
        {
            get { return this._Usernmame; }
            set
            {
                if (this._Usernmame != value)
                {
                    this._Usernmame = value;
                   // Check_Maxi_delay(_delaymaximum);
                    OnPropertyChanged("Usernmame");
                }
            }
        }

        public bool Ischecked
        {
            get { return this._Ischecked; }
            set
            {
                if (this._Ischecked != value)
                {
                    this._Ischecked = true;
                    //Check_Maxi_delay(_delaymaximum);
                    OnPropertyChanged("Ischecked");
                }
            }
        }
<<<<<<< HEAD
=======
=======

>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master

        public string maxi
        {
            get { return this._delaymaximum; }
            set
            {
                if (this._delaymaximum != value)
                {
                    this._delaymaximum = value;
                  Check_Maxi_delay(_delaymaximum);
                    OnPropertyChanged("maxi");
                }
            }
        }


        public void Check_Maxi_delay(string Maximumdelay)
        {
            try
            {
                int maxi_value = Convert.ToInt32(Maximumdelay);
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Enter in Int Formate");
                ModernDialog.ShowMessage("Please enter in Correct Formate", "Message", MessageBoxButton.OK);
            }
        }



        public string Nofollower
        {
            get { return this._NoFollower; }
        set
            {
            if(this._NoFollower != value)
            {
                this._NoFollower = value;
                check_nofollow(_NoFollower);
                OnPropertyChanged("Nofollower");
            }
            }
        }

        public void check_nofollow(string value)
        {
            try
            {
                int no_follow = Convert.ToInt32(value);
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Enter in Int Formate");
                ModernDialog.ShowMessage("Please enter in Correct Formate", "Message", MessageBoxButton.OK);
            }
        }

        public string mini
        {
            get { return this._delayminimum; }
            set
            {
                if (this._delayminimum != value)
                {
                    this._delayminimum = value;
                    Check_Mini_delay(_delayminimum);
                    OnPropertyChanged("mini");

                }
            }
        }

        public void Check_Mini_delay(string mini)
        {
            try
            {
                int Mini_value = Convert.ToInt32(mini);
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Enter in Int Formate");
                ModernDialog.ShowMessage("Please enter in Correct Formate", "Message", MessageBoxButton.OK);
            }
        }

        


        public string Thread
        {
            get { return this._NoThread; }
            set
            {
                if (this._NoThread != value)
                {
                    this._NoThread = value;
                   checkThread(_NoThread);
                    OnPropertyChanged("Thread");
                }
            }
        }


        public void checkThread(string Thread)
        {
            try
            {
                int thread = Convert.ToInt32(Thread);
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Enter in Int Formate");
                ModernDialog.ShowMessage("Please enter in Correct Formate", "Message", MessageBoxButton.OK);
            }
        }


        public string TxtUserField
        {
            get { return this._UserTxtField; }
            set
            {
                if (this._UserTxtField != value)
                {
                    this._UserTxtField = value;
                   // validation_Follow(_UserTxtField);
                    OnPropertyChanged("TxtUserField");
                }
            }
        }

        //public void validation_Follow(string Name)
        //{

        //    try
        //    {
        //        if (IGGlobals.listAccounts.Count > 0)
        //        {
        //            try
        //            {

        //                if (string.IsNullOrEmpty(TxtUserField))
        //                {
        //                    GlobusLogHelper.log.Info("Please Upload  Message");
        //                    ModernDialog.ShowMessage("Please Upload  Message", "Upload Message", MessageBoxButton.OK);
        //                    return;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
        //            }
        //        }
        //        else
        //        {
        //            GlobusLogHelper.log.Info("Please Upload Account");
        //            return;
        //        }
        //    }
        //    catch { }
        //}


        public string TxtField_two
        {
            get { return this._UserTxtField_two; }
            set
            {
                if (this._UserTxtField_two != value)
                {
                    this._UserTxtField_two = value;
                    
                    OnPropertyChanged("TxtField_two");
                }
            }
        }
        public string nounfollow
        {
            get { return this._nounfollow; }
            set
            {
                if (this._nounfollow != value)
                {
                    this._nounfollow = value;
                    check_nounfollow(_nounfollow);
                    OnPropertyChanged("nounfollow");
                }
            }
        }


        public void check_nounfollow(string value)
        {
            try
            {
                int no_follower = Convert.ToInt32(value);
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Enter in Correct formate");
                ModernDialog.ShowMessage("Please enter in Correct Formate", "Message", MessageBoxButton.OK);
            }
        }




        public string NoPhoto
        {
            get { return this._NoPhoto; }
            set
            {
                if (this._NoPhoto != value)
                {
                    this._NoPhoto = value;
                    check_noPhoto(_NoPhoto);
                    OnPropertyChanged("NoPhoto");
                }
            }
        }


        public void check_noPhoto(string value)
        {
            try
            {
                int nophoto = Convert.ToInt32(value);
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Info("Enter in Correct formate");
                ModernDialog.ShowMessage("Please enter in Correct Formate", "Message", MessageBoxButton.OK);
            }
        }


        public string TxtField_three
        {
            get { return this._UserTxtField_three; }
            set
            {
                if (this._UserTxtField_three != value)
                {
                    this._UserTxtField_three = value;
                    OnPropertyChanged("TxtField_three");
                }
            }
        }

        public string password
        {
            get { return this._password; }
            set
            {
                if (this._password != value)
                {
                    this._password = value;
                    OnPropertyChanged("password");

                }
            }
        }


       

        public string Error
        {
            get { return null; }
        }


        public string this[string columnName]
        {
            get
            {
                if (columnName == "maxi")
                {
                    //return string.IsNullOrEmpty(this._delaymaximum) ? "Maximum Delay " : null;

                    try
                    {

                        int i = int.Parse(this._delaymaximum);
                        return null;

                    }
                    catch
                    {
                        return "Maximum Delay";

                    }


                   
                }
                if (columnName == "Nofollower")
                {
                   // return string.IsNullOrEmpty(this._NoFollower) ? "Input No.Follower" : null;

                    try
                    {

                        int i = int.Parse(this._NoFollower);
                        return null;

                    }
                    catch
                    {
                        return "Input No.Follower";

                    }

                }
                if (columnName == "nounfollow")
                {
                  //  return string.IsNullOrEmpty(this._nounfollow) ? "Input No.Unfollow" : null;


                    try
                    {

                        int i = int.Parse(this._nounfollow);
                        return null;

                    }
                    catch
                    {
                        return "Input No.Unfollower";

                    }




                }
                if (columnName == "mini")
                {
                    //return string.IsNullOrEmpty(this._delayminimum) ? " Minimum Delay" : null;


                    try
                    {
                        int i = int.Parse(this._delayminimum);
                        return null;
                    }
                    catch
                    {
                        return "Minimum Delay";

                    }
                }
                if (columnName == "NoPhoto")
                {
                    return string.IsNullOrEmpty(this._NoPhoto) ? " Input No.Photo" : null;
                }
                if (columnName == "Thread")
                {
                  //  return string.IsNullOrEmpty(this._NoThread) ? " Input No.Thread" : null;

                    try
                    {

                        int i = int.Parse(this._NoThread);
                        return null;
 
                    }
                    catch
                    {
                       return "Input No.Thread";

                    }                                       
                }
                if (columnName == "TxtUserField")
                {
                    return string.IsNullOrEmpty(this._UserTxtField) ? "Required" : null;
                   
                }
                if (columnName == "TxtField_two")
                {
                    return string.IsNullOrEmpty(this._UserTxtField_two) ? "Required" : null;
                }
                if (columnName == "TxtField_three")
                {
                    return string.IsNullOrEmpty(this._UserTxtField_three) ? "Required" : null;
                    
                }
                if (columnName == "password")
                {
                    return string.IsNullOrEmpty(this._password) ? "Password Required" : null;
                }
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
                if (columnName == "Username")
                {
                    return string.IsNullOrEmpty(this._password) ? "Password Required" : null;
                }
                if (columnName == "Ischecked")
                {
                    //return bool.IsNullOrEmpty(this._password) ? "Password Required" : null;
                }
<<<<<<< HEAD
=======
=======
>>>>>>> 040a8d35fce59f25e2f75d75646c50226d83374f
>>>>>>> origin/master
                return null;
            }
        }



      

    }
}
