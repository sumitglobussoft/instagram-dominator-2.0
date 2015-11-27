using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BaseLib;
using System.Data;

namespace GramDominator.Classes
{
    public class SelectAccountViewModel : Validation
    {
        public static ObservableCollection<Validation> _listOfAccount = new ObservableCollection<Validation>();

        public ObservableCollection<Validation> ListOfAccount
        {
            get
            {
                return _listOfAccount;
            }
        }

        public SelectAccountViewModel()
        {
            try
            {
                DataSet DS = new DataSet();
                DS = DataBaseHandler.SelectQuery("select Username from AccountInfo", "AccountInfo");
                if(DS.Tables[0].Rows.Count>0)
                {
                    foreach(DataRow dr in DS.Tables[0].Rows)
                    {
                        Validation objSelect_Account = new Validation(); 
                        string username = dr.ItemArray[0].ToString();
                        bool ischecked = false;
                        objSelect_Account.Usernmame = username;
                        objSelect_Account.Ischecked = ischecked;
                        _listOfAccount.Add(objSelect_Account);
                    }
                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.Message);  
            }
        }
    }
}
