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
        public ICommand MinimizeWindowCommand { get; set; }
        #endregion

        public ControlBarViewModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                FrameworkElement parent = GetParent(p);
                Window w = parent as Window;
                if (w != null)
                {
                    w.Owner?.Close(); //đóng wd owner (loginwd)
                    w.Close();
                }
            });
            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                FrameworkElement parent = GetParent(p);
                Window w = parent as Window;
                if (w != null)
                    w.WindowState = WindowState.Minimized;
            });
        }

        FrameworkElement GetParent(UserControl p)
        {
            FrameworkElement parent = p;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
