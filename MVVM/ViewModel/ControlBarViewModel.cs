using QuanLiCoffeeShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
   
    internal class ControlBarViewModel : ObservableObject
    {
        #region commands
        public ICommand CloseWindowCommand { get; set; }
        #endregion
         
        public ControlBarViewModel()
        {
            CloseWindowCommand = new RelayCommand(parameter =>
            {
                if (parameter is Window window)
                {
                    window.Close();
                }
            });
        }

    }
}
