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

namespace QuanLiCoffeeShop.MVVM.View.Admin.CaLamViecManagement
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
            KhoiTaoDanhSachNhanVien();
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

        private void KhoiTaoDanhSachNhanVien()
        {
            var danhSachNhanVien = new List<NhanVien>
            {
                new NhanVien { ID = "001", HoTen = "Nguyễn Văn e" },
                new NhanVien { ID = "002", HoTen = "Trần Thị B" },
                new NhanVien { ID = "003", HoTen = "Phạm Văn C" },
                new NhanVien { ID = "004", HoTen = "Phạm Văn D" },
                new NhanVien { ID = "005", HoTen = "Phạm Văn E" },
                new NhanVien { ID = "006", HoTen = "Phạm Văn F" },
            };
            dataGridNhanVien.ItemsSource = danhSachNhanVien;
        }

        private void DataGridNhanVien_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataGridNhanVien.SelectedItem is NhanVien nhanVien)
            {
                string hoTen = nhanVien.HoTen;

                // Truyền họ tên vào constructor của DonXinNghiWindow
                var donXinNghiWindow = new DonXinNghiWindow(hoTen);
                donXinNghiWindow.ShowDialog();
            }
        }

        public class NhanVien
        {
            public string ID { get; set; }
            public string HoTen { get; set; }
        }
    }
}

