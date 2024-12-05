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
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace QuanLiCoffeeShop.MVVM.View.Admin.TableManagament
{
    /// <summary>
    /// Interaction logic for TableUserControl.xaml
    /// </summary>
    public partial class TableUserControl : UserControl
    {
        public MyTemplateSelector x {  get; set; }
        public TableUserControl()
        {
            InitializeComponent();
        }


    }
    public class MyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Table2Template { get; set; }
        public DataTemplate Table3Template { get; set; }
        public DataTemplate Table4Template { get; set; }
        public DataTemplate Table6Template { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            TableDTO temp = item as TableDTO;
            if (temp == null) return base.SelectTemplate(item, container);
            if (temp.GT_ID is int)
            {
                switch (temp.GT_ID)
                {
                    case 1:
                        return Table2Template;
                    case 2:
                        return Table3Template;
                    case 3:
                        return Table4Template;
                    case 4:
                        return Table6Template;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
