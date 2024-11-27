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

namespace QuanLiCoffeeShop.MVVM.View.Staff.StaffCaLamViec
{
    /// <summary>
    /// Interaction logic for XinNghiPhepOrDoiCaWindow.xaml
    /// </summary>
    public partial class XinNghiPhepOrDoiCaWindow : Window
    {
        public XinNghiPhepOrDoiCaWindow()
        {
            InitializeComponent();
        }

        private void btnBoQua_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
