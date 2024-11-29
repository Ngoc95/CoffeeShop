using QuanLiCoffeeShop.MVVM.View.Admin.CaLamViecManagement;
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

namespace QuanLiCoffeeShop.MVVM.View.Staff.StaffCaLamViec
{
    /// <summary>
    /// Interaction logic for CaLamViecTable.xaml
    /// </summary>
    public partial class CaLamViecTable : UserControl
    {
        public CaLamViecTable()
        {
            InitializeComponent();
            dataGridCaLamViec.LoadingRow += DataGridCa_LoadingRow;
            KhoiTaoCaLamViec();
        }
        private void DataGridCa_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var danhSachCa = new List<string> { "Ca Sáng", "Ca Chiều", "Ca Tối" };
            int index = e.Row.GetIndex();
            if (index >= 0 && index < danhSachCa.Count)
            {

                e.Row.Header = danhSachCa[index];
            }
        }
        private void btnDieuChinhCaLamViec_Click(object sender, RoutedEventArgs e)
        {
            var DieuChinhCa = new DieuChinhCaLamViecWindow();
            DieuChinhCa.ShowDialog();
        }

        private void btnXuatExcelOverlay_Click(object sender, RoutedEventArgs e)
        {

        }

        public class CaLamViec
        {
            public string ThuCa { get; set; }
            public string Thu2 { get; set; }
            public string Thu3 { get; set; }
            public string Thu4 { get; set; }
            public string Thu5 { get; set; }
            public string Thu6 { get; set; }
            public string Thu7 { get; set; }
            public string ChuNhat { get; set; }
        }

        private void KhoiTaoCaLamViec()
        {
            var danhSachCa = new List<CaLamViec>
             {
                 new CaLamViec { ThuCa = "Ca Sáng" },
                 new CaLamViec { ThuCa = "Ca Chiều" },
                 new CaLamViec { ThuCa = "Ca Tối" }
             };

            dataGridCaLamViec.ItemsSource = danhSachCa;
        }

        private void btnDieuChinhNgayLamViec_Click(object sender, RoutedEventArgs e)
        {
            var DieuChinhNgay = new DieuChinhNgayLamViecWindow();
            DieuChinhNgay.ShowDialog();
        }

        private void dataGridCaLamViec_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dataGridCaLamViec.SelectedCells.Count > 0)
            {
                var cellInfo = dataGridCaLamViec.SelectedCells[0];
                var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock;

                if (cellContent != null)
                {
                    string cellValue = cellContent.Text;
                    MessageBox.Show($"Giá trị ô được chọn: {cellValue}");
                }
            }
        }

        private void btnXinNghiPhepDoiCa_Click(object sender, RoutedEventArgs e)
        {
            var XinNghiOrDoiCa = new XinNghiPhepOrDoiCaWindow();
            XinNghiOrDoiCa.ShowDialog();
        }
    }
}

