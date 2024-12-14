using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.ViewModel.Admin;
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

namespace QuanLiCoffeeShop.MVVM.View.Admin.CaLamViecManagement
{
    /// <summary>
    /// Interaction logic for CaLamViecTable.xaml
    /// </summary>
    public partial class CaLamViecTable : UserControl
    {
        public CaLamViecTable()
        {
            InitializeComponent();
            cbFilter.SelectedIndex = 1;
        }
    }
}

