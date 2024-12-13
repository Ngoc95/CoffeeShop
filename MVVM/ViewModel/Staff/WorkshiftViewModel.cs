using GalaSoft.MvvmLight.Messaging;
using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.CaLamViecManagement;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.View.Staff.StaffCaLamViec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff
{
    public class WorkshiftViewModel : ObservableObject
    {
        //thông báo khi bên nhân viên update thông tin
        public class EmployeeUpdatedMessage
        {
            public int EmployeeId { get; set; }
            public string NewName { get; set; }

            public EmployeeUpdatedMessage(int employeeId, string newName)
            {
                EmployeeId = employeeId;
                NewName = newName;
            }
        }

        private ObservableCollection<ShiftScheduleDTO> _schedules;
        public ObservableCollection<ShiftScheduleDTO> Schedules
        {
            get => _schedules;
            set
            {
                _schedules = value;
                OnPropertyChanged(nameof(Schedules));
            }
        }
        private string _selectedRequestType;
        public string SelectedRequestType
        {
            get => _selectedRequestType;
            set { _selectedRequestType = value; OnPropertyChanged(); }
        }

        private string _employeeComment;
        public string EmployeeComment
        {
            get => _employeeComment;
            set { _employeeComment = value; OnPropertyChanged(); }
        }
        public ICommand RequestWdCM { get; set; }
        public ICommand SendRequestCM { get; set; }
        public WorkshiftViewModel()
        {
            LoadData();

            // Lắng nghe thông báo cập nhật
            Messenger.Default.Register<EmployeeUpdatedMessage>(this, (msg) =>
            {
                if (msg.EmployeeId == MainViewModel.currentEmp.EMP_ID) // Kiểm tra nếu là nhân viên hiện tại
                {
                    MainViewModel.currentEmp.EMP_NAME = msg.NewName; // Cập nhật tên trong currentEmp
                    LoadData(); // Tải lại dữ liệu
                }
            });
            RequestWdCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                XinNghiPhepOrDoiCaWindow wd = new XinNghiPhepOrDoiCaWindow();
                wd.ShowDialog();
            });
            SendRequestCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (MainViewModel.currentEmp == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Không thể gửi yêu cầu vì chưa đăng nhập");
                    return;
                }

                if (string.IsNullOrEmpty(SelectedRequestType) || string.IsNullOrEmpty(EmployeeComment))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn nhập thiếu thông tin");
                    return;
                }

                var requestDto = new RequestDTO
                {
                    EMP_ID = MainViewModel.currentEmp.EMP_ID,
                    REQ_TYPE = SelectedRequestType,
                    EMP_COMMENT = EmployeeComment,
                };

                (bool isAdded, string message) = await RequestService.Ins.AddRequest(requestDto);

                if (isAdded)
                {
                    p.Close();
                    MessageBoxCustom.Show(MessageBoxCustom.Success, message);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                }
            });

        }
        public void LoadData()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    if (MainViewModel.currentEmp == null) // Kiểm tra nếu không có nhân viên đăng nhập
                        return;

                    int currentEmpId = MainViewModel.currentEmp.EMP_ID; // Lấy ID nhân viên hiện tại

                    var query = context.EMPLOYEE_SHIFT
                        .Include("EMPLOYEE") 
                        .Include("WORK_SHIFT")
                        .Where(es => (es.IS_DELETED ?? false) == false &&
                                     (es.EMPLOYEE.IS_DELETED ?? false) == false &&
                                     (es.WORK_SHIFT.IS_DELETED ?? false) == false &&
                                     (es.EMP_ID == currentEmpId))
                        .OrderBy(es => es.WORK_SHIFT.SHIFT_ID)
                        .ToList();


                    var groupedData = query.GroupBy(es => new
                    {
                        es.WORK_SHIFT.SHIFT_NAME,
                        es.WORK_SHIFT.START_TIME,
                        es.WORK_SHIFT.END_TIME
                    })
                    .Select(group => new ShiftScheduleDTO
                    {
                        ShiftName = group.Key.SHIFT_NAME,
                        DisplayName = $"{group.Key.SHIFT_NAME} \n({group.Key.START_TIME:HH:mm} - {group.Key.END_TIME:HH:mm})", // Hiển thị tên + thời gian
                        EmployeeNamesByDay = group
                            .GroupBy(es => (int)es.WORK_DAY)
                            .ToDictionary(
                                g => g.Key,
                                g => g
                                .OrderBy(es => es.EMPLOYEE.EMP_ID)
                                .Select(es => "X")
                                //    .Select(es => $"NV{es.EMPLOYEE.EMP_ID:D3} - {es.EMPLOYEE.EMP_NAME}")
                                .ToList()
                            )
                    }).ToList();

                    Schedules = new ObservableCollection<ShiftScheduleDTO>(groupedData);
                }
            }
            catch
            {
                return;
            }
        }
        private async Task SubmitRequest()
        {
            if (MainViewModel.currentEmp == null)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Không thể gửi yêu cầu vì chưa đăng nhập");
                return;
            }

            var requestDto = new RequestDTO
            {
                EMP_ID = MainViewModel.currentEmp.EMP_ID,
                REQ_TYPE = SelectedRequestType, 
                EMP_COMMENT = EmployeeComment
            };

            (bool isAdded, string message) = await RequestService.Ins.AddRequest(requestDto);

            if (isAdded)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Success, message);
            }
            else
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, message);
            }
        }

    }
}
