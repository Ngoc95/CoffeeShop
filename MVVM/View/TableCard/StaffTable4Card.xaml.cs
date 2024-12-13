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

namespace QuanLiCoffeeShop.MVVM.View.TableCard
{
    /// <summary>
    /// Interaction logic for StaffTable4Card.xaml
    /// </summary>
    public partial class StaffTable4Card : UserControl
    {
        public StaffTable4Card()
        {
            InitializeComponent();
            Loaded += StaffTable4Card_Loaded;
        }

        private void StaffTable4Card_Loaded(object sender, RoutedEventArgs e)
        {
            if (Status.Text == "Còn trống")
                ToggleButton.IsChecked = false;
            else if (Status.Text == "Đang bận")
                ToggleButton.IsChecked = true;
            else
            {
                ToggleButton.Opacity = 0.5;
                ToggleButton.IsChecked = false;
                ToggleButton.IsEnabled = false;
            }
        }
    }
}
