using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.View.Admin.CaLamViecManagement
{
    public partial class DieuChinhNgayLamViecWindow : Window
    {
        public ObservableCollection<NgayLamViec> DanhSachNgayLamViec { get; set; }

        public DieuChinhNgayLamViecWindow()
        {
            InitializeComponent();
            DanhSachNgayLamViec = new ObservableCollection<NgayLamViec>();
            KhoiTaoNgayLamViec();
            this.DataContext = this;
        }

        private void moveChangeNgayWin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        // Phương thức khởi tạo dữ liệu cho DataGrid
        private void KhoiTaoNgayLamViec()
        {
            // Lấy danh sách nhân viên (có thể là từ cơ sở dữ liệu)
            var danhSachNhanVien = new List<(string ID, string HoTen)>
            {
                ("001", "Nguyễn Văn A"),
                ("002", "Trần Thị B"),
                ("003", "Phạm Văn C"),
            };

            // Thêm thông tin nhân viên vào DanhSachNgayLamViec
            foreach (var nhanVien in danhSachNhanVien)
            {
                DanhSachNgayLamViec.Add(new NgayLamViec
                {
                    ID = nhanVien.ID,
                    Name = nhanVien.HoTen,
                });
            }
        }

        public class NgayLamViec
        {
            public string ID { get; set; }
            public string Name { get; set; }

        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            int maxId = 0;
            foreach (var item in DanhSachNgayLamViec)
            {
                if (int.TryParse(item.ID.Substring(2), out int idSo))
                {
                    if (idSo > maxId)
                        maxId = idSo;
                }
            }

            var idMoi = $"ID{maxId + 1:D3}";
            var tenMoi = $"Nhân viên mới {DanhSachNgayLamViec.Count + 1}";

            DanhSachNgayLamViec.Add(new NgayLamViec
            {
                ID = idMoi,
                Name = tenMoi,
            });

        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridNgayLamViec.SelectedItem is NgayLamViec selectedItem)
            {
                DanhSachNgayLamViec.Remove(selectedItem);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa.");
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (DanhSachNgayLamViec != null)
            {
                foreach (var ngayLamViec in DanhSachNgayLamViec)
                {
                    Console.WriteLine($"ID: {ngayLamViec.ID}, Họ Tên: {ngayLamViec.Name}");
                }
                MessageBox.Show("Dữ liệu đã được lưu!");
            }
        }

        private void btnBoQua_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}