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

namespace QuanLiCoffeeShop.MVVM.View.Admin.ErrorManagement
{
    /// <summary>
    /// Interaction logic for ErrorUserControl.xaml
    /// </summary>
    public partial class ErrorUserControl : UserControl
    {
        public ErrorUserControl()
        {
            InitializeComponent();
            cbFilter.SelectedIndex = 0;
        }

        private void PART_ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (cbStatus.Items.Count >= 2)
            {
                // Lấy index của item hiện tại
                int currentIndex = cbStatus.SelectedIndex;

                // Chuyển đổi index
                cbStatus.SelectedIndex = currentIndex == 0 ? 1 : 0;
            }
        }
    }
}
