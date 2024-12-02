using QuanLiCoffeeShop.MVVM.Model;
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

namespace QuanLiCoffeeShop.MVVM.View.ProductCard
{
    /// <summary>
    /// Interaction logic for ProductCard.xaml
    /// </summary>
    public partial class ProductCard : UserControl
    {
        //public static readonly DependencyProperty ProductProperty = DependencyProperty.Register("Product", typeof(Product), typeof(ProductCard), new PropertyMetadata(null));

        //public Product Product
        //{
        //    get { return (Product)GetValue(ProductProperty); }
        //    set { SetValue(ProductProperty, value); }
        //}
        public ProductCard()
        {
            InitializeComponent();
        }
    }
}
