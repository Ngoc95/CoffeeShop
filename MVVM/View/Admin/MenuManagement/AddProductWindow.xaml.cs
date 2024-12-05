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

namespace QuanLiCoffeeShop.MVVM.View.Admin.MenuManagement
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        List<string> _GenreProductLists = new List<string>();
        public AddProductWindow()
        {
            InitializeComponent();
        }

        public AddProductWindow(List<string> genre_Prd, string Genre)
        {
            InitializeComponent();
            if (_GenreProductLists.Count > 0) return;
            _GenreProductLists = genre_Prd;
            Genre_Product_cbb.ItemsSource = _GenreProductLists;
        }

        private void moveAddCusWin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

    }
}
