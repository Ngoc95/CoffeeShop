using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using QuanLiCoffeeShop.MVVM.View.Admin.EmployeeManagement;
using Microsoft.Win32;
using OfficeOpenXml;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using static QuanLiCoffeeShop.MVVM.ViewModel.Admin.WorkshiftViewModel;
using GalaSoft.MvvmLight.Helpers;
using QuanLiCoffeeShop.Helpers;
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    public class EmployeeViewModel : ObservableObject
    {
        #region Employee Information

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        private string cccd;
        public string Cccd
        {
            get { return cccd; }
            set { cccd = value; }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        private DateTime bday;
        public DateTime Bday
        {
            get { return bday; }
            set { bday = value; }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private bool isMale;
        public bool IsMale
        {
            get => isMale;
            set
            {
                if (isMale != value)
                {
                    isMale = value;
                    OnPropertyChanged(nameof(IsMale));
                    UpdateGender();
                }
            }
        }
        private void UpdateGender()
        {
            Gender = IsMale ? "Nam" : "Nữ";
        }
        private string gender;
        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
        private decimal salary;
        public decimal Salary
        {
            get { return salary; }
            set
            {
                salary = value;
                OnPropertyChanged();
            }
        }
        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string role;
        public string Role
        {
            get { return role; }
            set { role = value; }
        }
        #endregion

        public int Count => EmpList?.Count ?? 0;
        private ObservableCollection<EmployeeDTO> _empList;
        public ObservableCollection<EmployeeDTO> EmpList
        {
            get { return _empList; }
            set
            {
                _empList = value;
                OnPropertyChanged(nameof(EmpList));
                OnPropertyChanged(nameof(Count));
            }
        }
        private EmployeeDTO _selectedItem;
        public EmployeeDTO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    EditEmp = new EmployeeDTO
                    {
                        EMP_ID = _selectedItem.EMP_ID,
                        EMP_NAME = _selectedItem.EMP_NAME,
                        EMP_CCCD = _selectedItem.EMP_CCCD,
                        EMP_BIRTHDAY = _selectedItem.EMP_BIRTHDAY,
                        EMP_EMAIL = _selectedItem.EMP_EMAIL,
                        EMP_GENDER = _selectedItem.EMP_GENDER,
                        EMP_PHONE = _selectedItem.EMP_PHONE,
                        EMP_ROLE = _selectedItem.EMP_ROLE,
                        EMP_SALARY = _selectedItem.EMP_SALARY,
                        EMP_STARTDATE = _selectedItem.EMP_STARTDATE,
                        EMP_STATUS = _selectedItem.EMP_STATUS,
                        EMP_USERNAME = _selectedItem.EMP_USERNAME,
                        EMP_TotalShifts = _selectedItem.EMP_TotalShifts
                    };
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        public ObservableCollection<string> RoleList { get; set; } = new ObservableCollection<string>
        {
            "Tất cả",
            "Quản lý",
            "Phục vụ",
            "Pha chế",
            "Thu ngân",
        };

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        private string _selectedRole;
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();

                CommandManager.InvalidateRequerySuggested();
                if (FilterRoleCM.CanExecute(null))
                    FilterRoleCM.Execute(null);

            }
        }

        private EmployeeDTO _editEmp;
        public EmployeeDTO EditEmp
        {
            get => _editEmp;
            set
            {
                _editEmp = value;
                OnPropertyChanged(nameof(EditEmp));
            }
        }
        public TextBox SearchEmp { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand CancelCM { get; set; }
        public ICommand SearchEmpCM { get; set; }
        public ICommand FilterRoleCM { get; set; }
        public ICommand EditEmpWdCM { get; set; }
        public ICommand EditEmpCM { get; set; }
        public ICommand AddEmpWdCM { get; set; }
        public ICommand AddEmpListCM { get; set; }
        public ICommand DeleteEmpListCM { get; set; }
        public ICommand ExportExcelCM { get; set; }
        public EmployeeViewModel()
        {
            StartDate = DateTime.Now;
            Bday = DateTime.Now;
            FirstLoadCM = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                await LoadEmpListAsync();
            });
            CancelCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                resetData();
                p.Close();
            });

            SearchEmpCM = new RelayCommand<TextBox>((p) => { return true; }, async (p) =>
            {
                if (p == null) return;

                string searchText = p.Text;
                await ApplyFilterAndSearch(searchText, SelectedRole);
            });

            FilterRoleCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                await ApplyFilterAndSearch(SearchEmp?.Text, SelectedRole);
            });

            EditEmpWdCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (SelectedItem == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa chọn nhân viên để xem");
                    return;
                }
                EditEmployeeWindow wd = new EditEmployeeWindow();
                wd.ShowDialog();
            });

            EditEmpCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(EditEmp.EMP_CCCD) || string.IsNullOrEmpty(EditEmp.EMP_EMAIL) || string.IsNullOrEmpty(EditEmp.EMP_NAME) ||
                    string.IsNullOrEmpty(EditEmp.EMP_PHONE) || string.IsNullOrEmpty(EditEmp.EMP_ROLE) ||
                    string.IsNullOrEmpty(EditEmp.EMP_USERNAME) || string.IsNullOrEmpty(EditEmp.EMP_GENDER))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn nhập thiếu thông tin");
                    return;
                }
                if (DateTime.Compare((DateTime)EditEmp.EMP_BIRTHDAY, new DateTime(1900, 1, 1)) < 0 || DateTime.Compare((DateTime)EditEmp.EMP_BIRTHDAY, DateTime.Now) > 0)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày sinh không hợp lệ");
                    return;
                }

                if (DateTime.Compare((DateTime)EditEmp.EMP_STARTDATE, new DateTime(1900, 1, 1)) < 0 || DateTime.Compare((DateTime)EditEmp.EMP_STARTDATE, DateTime.Now) > 0)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày bắt đầu không hợp lệ");
                    return;
                }

                if (((DateTime)EditEmp.EMP_STARTDATE).Year - ((DateTime)EditEmp.EMP_BIRTHDAY).Year < 16)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Nhân viên phải trên 16 tuổi");
                    return;
                }

                var employees = await EmployeeService.Ins.GetAllEmp();
                // Kiểm tra nếu tên tài khoản đã tồn tại
                bool IsExistUsername = employees
                    .Where(emp => emp.EMP_ID != EditEmp.EMP_ID && emp.EMP_USERNAME == EditEmp.EMP_USERNAME && emp.IS_DELETED == false)
                    .Any();
                if (IsExistUsername)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Tài khoản đã tồn tại");
                    return;
                }

                int empID = SelectedItem.EMP_ID;
                string pass;
                if (string.IsNullOrEmpty(EditEmp.EMP_PASSWORD))
                {
                    var employee = await EmployeeService.Ins.GetEmployeeById(empID);
                    EditEmp.EMP_PASSWORD = employee.EMP_PASSWORD;
                    pass = EditEmp.EMP_PASSWORD;
                }
                else
                    pass = PasswordHelper.MD5Hash(EditEmp.EMP_PASSWORD);
                EMPLOYEE newEmp = new EMPLOYEE
                {
                    EMP_ID = SelectedItem.EMP_ID,
                    EMP_NAME = EditEmp.EMP_NAME,
                    EMP_PHONE = EditEmp.EMP_PHONE,
                    EMP_EMAIL = EditEmp.EMP_EMAIL,
                    EMP_CCCD = EditEmp.EMP_CCCD,
                    EMP_BIRTHDAY = EditEmp.EMP_BIRTHDAY,
                    EMP_GENDER = EditEmp.EMP_GENDER,
                    EMP_PASSWORD = pass,
                    EMP_ROLE = EditEmp.EMP_ROLE,
                    EMP_SALARY = EditEmp.EMP_SALARY,
                    EMP_STARTDATE = EditEmp.EMP_STARTDATE,
                    EMP_STATUS = EditEmp.EMP_STATUS,
                    EMP_USERNAME = EditEmp.EMP_USERNAME,
                    IS_DELETED = false,
                };
                p.Close();
                (bool success, string messageEdit) = await EmployeeService.Ins.EditEmpList(newEmp, SelectedItem.EMP_ID);
                if (success)
                {
                    await LoadEmpListAsync();
                    Messenger.Default.Send(new EmployeeUpdatedMessage(empID, newEmp.EMP_NAME)); //thông báo để update bên view ca làm việc
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageEdit);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageEdit);
                }
            });

            AddEmpWdCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddEmployeeWindow wd = new AddEmployeeWindow();
                wd.ShowDialog();
            });

            AddEmpListCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(Password))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn nhập thiếu thông tin");
                    return;
                }
                string pass = PasswordHelper.MD5Hash(this.Password);

                EMPLOYEE newEmp = new EMPLOYEE
                {
                    EMP_NAME = this.Name,
                    EMP_GENDER = this.Gender,
                    EMP_EMAIL = this.Email,
                    EMP_CCCD = this.Cccd,
                    EMP_BIRTHDAY = this.Bday,
                    EMP_PASSWORD = pass,
                    EMP_PHONE = this.Phone,
                    EMP_ROLE = this.Role,
                    EMP_SALARY = this.Salary,
                    EMP_STARTDATE = this.StartDate,
                    EMP_STATUS = this.Status,
                    EMP_USERNAME = this.Username,
                    IS_DELETED = false,
                };
                (bool IsAdded, string messageAdd) = await EmployeeService.Ins.AddNewEmp(newEmp);
                if (IsAdded)
                {
                    Messenger.Default.Send(new EmployeeUpdatedMessage(newEmp.EMP_ID, newEmp.EMP_NAME));
                    p.Close();
                    await LoadEmpListAsync();
                    resetData();
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }

            });
            DeleteEmpListCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (SelectedItem == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa chọn nhân viên để xóa");
                    return;
                }
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool sucess, string messageDelete) = await EmployeeService.Ins.DeleteEmployee(SelectedItem.EMP_ID);
                    if (sucess)
                    {
                        Messenger.Default.Send(new EmployeeUpdatedMessage(SelectedItem.EMP_ID, SelectedItem.EMP_NAME)); //thông báo để update bên view ca làm việc
                        EmpList.Remove(SelectedItem);
                        SelectedItem = null;
                        EditEmp = null;
                        OnPropertyChanged(nameof(Count));
                        MessageBoxCustom.Show(MessageBoxCustom.Success, messageDelete);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                    }
                }

            });
            ExportExcelCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await ExportToExcel(p);
            });
        }
        private async Task ApplyFilterAndSearch(string searchText, string filterRole)
        {
            if (EmployeeService.Ins == null)
            {
                return;
            }

            var allEmployees = await EmployeeService.Ins.GetAllEmp();
            if (allEmployees == null || !allEmployees.Any())
            {
                return;
            }

            // Cập nhật tổng ca làm của mỗi nhân viên
            foreach (var employee in allEmployees)
            {
                if (employee == null)
                {
                    continue;
                }
                employee.EMP_TotalShifts = await Task.Run(() => EmployeeService.Ins.GetEmployeeTotalShifts(employee.EMP_ID));
            }

            filterRole = filterRole?.ToLower() ?? string.Empty;
            searchText = searchText?.ToLower() ?? string.Empty;

            // Lọc và tìm kiếm
            EmpList = new ObservableCollection<EmployeeDTO>(allEmployees.FindAll(x =>
                (string.IsNullOrEmpty(filterRole) || filterRole == "tất cả" ||
                 (x.EMP_ROLE?.ToLower().Contains(filterRole) ?? false)) &&
                (string.IsNullOrEmpty(searchText) ||
                 ($"nv{x.EMP_ID:D3}".ToLower().Contains(searchText)) ||
                 (x.EMP_NAME?.ToLower().Contains(searchText) ?? false) ||
                 (x.EMP_EMAIL?.ToLower().Contains(searchText) ?? false) ||
                 (x.EMP_PHONE?.ToLower().Contains(searchText) ?? false) ||
                 x.EMP_ID.ToString().Contains(searchText)
                )
            ));
        }

        private Task ExportToExcel(object o)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Chọn nơi để lưu file",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = "EmployeesExport.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Nhân viên");

                        // Thêm tiêu đề chính
                        worksheet.Cells["A1:K2"].Merge = true; // Merge từ A1 đến K2
                        worksheet.Cells["A1"].Value = "Danh sách nhân viên";
                        worksheet.Cells["A1"].Style.Font.Size = 16; // Tăng cỡ chữ
                        worksheet.Cells["A1"].Style.Font.Bold = true; // In đậm
                        worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        // Thêm tiêu đề cột
                        string[] headers = new string[] { "Mã nhân viên", "Họ tên", "Ngày sinh", "Giới tính", "CCCD", "SĐT", "Email",
                                                        "Ngày bắt đầu", "Chức vụ", "Tổng ca làm trong tuần", "Lương tuần" };

                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cells[3, i + 1].Value = headers[i];
                            worksheet.Cells[3, i + 1].Style.Font.Bold = true; // In đậm tiêu đề
                            worksheet.Cells[3, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[3, i + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        }

                        // Thêm dữ liệu
                        for (int i = 0; i < EmpList.Count; i++)
                        {
                            worksheet.Cells[i + 4, 1].Value = EmpList[i].EMP_ID;
                            worksheet.Cells[i + 4, 2].Value = EmpList[i].EMP_NAME;
                            worksheet.Cells[i + 4, 3].Value = EmpList[i].EMP_BIRTHDAY?.ToString("dd/MM/yyyy") ?? "";
                            worksheet.Cells[i + 4, 4].Value = EmpList[i].EMP_GENDER;
                            worksheet.Cells[i + 4, 5].Value = EmpList[i].EMP_CCCD;
                            worksheet.Cells[i + 4, 6].Value = EmpList[i].EMP_PHONE;
                            worksheet.Cells[i + 4, 7].Value = EmpList[i].EMP_EMAIL;
                            worksheet.Cells[i + 4, 8].Value = EmpList[i].EMP_STARTDATE?.ToString("dd/MM/yyyy") ?? "";
                            worksheet.Cells[i + 4, 9].Value = EmpList[i].EMP_ROLE;
                            worksheet.Cells[i + 4, 10].Value = EmpList[i].EMP_TotalShifts;
                            worksheet.Cells[i + 4, 11].Value = EmpList[i].EMP_SALARY;
                        }

                        // Auto fit cho tất cả các cột
                        worksheet.Cells.AutoFitColumns();

                        // Vẽ border cho tất cả các ô chứa dữ liệu
                        var totalRows = EmpList.Count + 3; // Bao gồm header và dữ liệu
                        var totalColumns = headers.Length;
                        var dataRange = worksheet.Cells[3, 1, totalRows, totalColumns];
                        dataRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        // Lưu file Excel
                        File.WriteAllBytes(filePath, package.GetAsByteArray());
                    }
                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Xuất file excel thành công");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xuất file excel thất bại");
            }

            return Task.CompletedTask;
        }
        private async Task LoadEmpListAsync()
        {
            EmployeeService.Ins.UpdateEmployeeSalaries();

            var employees = await Task.Run(() => EmployeeService.Ins.GetAllEmp());
            //cập nhật tổng số ca làm việc của mỗi nhân viên
            foreach (var employee in employees)
            {
                employee.EMP_TotalShifts = await Task.Run(() => EmployeeService.Ins.GetEmployeeTotalShifts(employee.EMP_ID));
            }

            EmpList = new ObservableCollection<EmployeeDTO>(employees);

            SearchText = SearchEmp?.Text;
            await ApplyFilterAndSearch(SearchText, SelectedRole);
        }
        #region methods
        void resetData()
        {
            Name = null;
            Email = null;
            Phone = null;
            Cccd = null;
            Username = null;
            Password = null;
            Role = null;
            Gender = null;
            Status = null;
            Salary = 0;
        }
        #endregion

    }
}
