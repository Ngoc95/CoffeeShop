using System;
using QuanLiCoffeeShop.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLiCoffeeShop.MVVM.View.Admin.MenuManagement;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.ProductCardVM
{
    internal class ProCardViewModel:ObservableObject
    {
        public ICommand EditProductCommand { get; set; }
        ProCardViewModel()
        {

            //EditProductCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            //    {
            //        EditProductWindow wd = new EditProductWindow();
            //wd.ShowDialog();
            //    });
        }


    }
}
