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
    /// Interaction logic for StaffTable2Card.xaml
    /// </summary>
    public partial class StaffTable2Card : UserControl
    {
        public StaffTable2Card()
        {
            InitializeComponent();
            Loaded += StaffTable2Card_Loaded;
        }

        private void StaffTable2Card_Loaded(object sender, RoutedEventArgs e)
        {
            if (Status.Text == "Còn trống")
                ToggleButton.IsChecked = false;
            else if(Status.Text == "Đang bận")
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
