using QuanLiCoffeeShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class HomeViewModel : ObservableObject
    {
        public List<Tuple<string, string>> NoticeList = new List<Tuple<string, string>>() { new Tuple<string, string>("1", "2") };


        #region Command

        #endregion
        HomeViewModel()
        {
            
        }

    }
}
