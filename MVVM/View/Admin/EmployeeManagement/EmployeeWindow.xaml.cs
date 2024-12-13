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

namespace QuanLiCoffeeShop.MVVM.View.Admin.EmployeeManagement
{
    public partial class EmployeeWindow : UserControl
    {
        public EmployeeWindow()
        {
            InitializeComponent();
            cbFilter.SelectedIndex = 0;
        }

        private void addEmp_Button_Click(object sender, RoutedEventArgs e)
        {
            var addEmpView = new AddEmployeeWindow();
            addEmpView.ShowDialog();
        }

        private void btnShowDetail_Click(object sender, RoutedEventArgs e)
        {
            var showDetailEmpView = new EditEmployeeWindow();
            showDetailEmpView.ShowDialog();
        }


    }
}
