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
using System.Windows.Shapes;

namespace QuanLiCoffeeShop.MVVM.View.Admin.TableManagement
{
    /// <summary>
    /// Interaction logic for ResInforAdminWindow.xaml
    /// </summary>
    public partial class ResInforAdminWindow : Window
    {
        public ResInforAdminWindow()
        {
            InitializeComponent();
            Loaded += ResInforAdminWindow_Loaded;
        }



        private void ResInforAdminWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (status.Text == "Khách đã nhận bàn")
            {
                CheckInBtn.Content = "Hủy trạng thái đã xác nhận";
            }
            else
            {
                CheckInBtn.Content = "Check In";
            }
           
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }

        }
    }
}
