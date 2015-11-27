using BaseLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GramDominator
{
    class CheckListBoxViewModel
    {

        private readonly ObservableCollection<AccountNotifyPropertyChanged> _userAccountName = new ObservableCollection<AccountNotifyPropertyChanged>();
        //AccountNotifyPropertyChanged objAccountNotifyPropertyChanged = new AccountNotifyPropertyChanged();
        public ObservableCollection<AccountNotifyPropertyChanged> UserAccountName
        {
            get
            {
                return _userAccountName;
            }
            set { }
        }


        public CheckListBoxViewModel()
        {
            DataSet ds = new DataSet();
            DataTable datatable = new DataTable();

            ds = DataBaseHandler.SelectQuery("select * from AccountInfo", "AccountInfo");
            datatable = ds.Tables[0];



            foreach (DataRow item in datatable.Rows)
            {
                AccountNotifyPropertyChanged objAccountNotifyPropertyChanged = new AccountNotifyPropertyChanged();
                objAccountNotifyPropertyChanged.AccountName = item[1].ToString();

                _userAccountName.Add(objAccountNotifyPropertyChanged);
            }


        }


    }
}
