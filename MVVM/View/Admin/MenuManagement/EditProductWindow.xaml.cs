using QuanLiCoffeeShop.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        List<string> _GenreProductLists = new List<string>();   
        public EditProductWindow()
        {
            InitializeComponent();
        }

        public EditProductWindow(List<string> genre_Prd, string Genre)
        {
            InitializeComponent();
            if (_GenreProductLists.Count > 0) return;
            _GenreProductLists = genre_Prd;
            Genre_Product_cbb.ItemsSource = _GenreProductLists;
            for (int i = 0; i < genre_Prd.Count; i++)
            {
                if (Genre == genre_Prd[i])
                {
                    Genre_Product_cbb.SelectedIndex = i;
                    break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
