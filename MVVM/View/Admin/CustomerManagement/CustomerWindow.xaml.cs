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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiCoffeeShop.MVVM.View.Admin.CustomerManagement
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : UserControl
    {
        public CustomerWindow()
        {
            InitializeComponent();
        }
        private void addCus_Button_Click(object sender, RoutedEventArgs e)
        {
            var addCusWin = new AddCustomerWindow();
            addCusWin.ShowDialog();
        }

        private void deleteCus_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
