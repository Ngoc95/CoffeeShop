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

namespace QuanLiCoffeeShop.MVVM.View.Admin.ThongKeManagement
{
    /// <summary>
    /// Interaction logic for ThongKeTable.xaml
    /// </summary>
    public partial class ThongKeTable : UserControl
    {
        public ThongKeTable()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string borderName = button.Tag.ToString();
                ResetBorders();
                HighlightBorder(borderName);
            }
        }
        private void ResetBorders()
        {
            HistoryBd.Background = new SolidColorBrush(Colors.White);
            RevenueBd.Background = new SolidColorBrush(Colors.White);
            FavorBd.Background = new SolidColorBrush(Colors.White);
        }

        private void HighlightBorder(string borderName)
        {
            switch (borderName)
            {
                case "HistoryBd":
                    HistoryBd.Background = new SolidColorBrush(Color.FromArgb(255, 255, 195, 161));
                    break;
                case "RevenueBd":
                    RevenueBd.Background = new SolidColorBrush(Color.FromArgb(255, 240, 195, 161));
                    break;
                case "FavorBd":
                    FavorBd.Background = new SolidColorBrush(Color.FromArgb(255, 240, 195, 161));
                    break;
            }
        }
    }
}

