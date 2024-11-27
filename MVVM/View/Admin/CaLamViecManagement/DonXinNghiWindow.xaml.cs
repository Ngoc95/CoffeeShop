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

namespace QuanLiCoffeeShop.MVVM.View.Admin.CaLamViecManagement
{
    /// <summary>
    /// Interaction logic for DonXinNghiWindow.xaml
    /// </summary>
    public partial class DonXinNghiWindow : Window
    {

        public DonXinNghiWindow(string hoTen)
        {
            InitializeComponent();

        }

        private void btnChapNhan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTuChoi_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

