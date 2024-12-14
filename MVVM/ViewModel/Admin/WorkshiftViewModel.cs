using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using OfficeOpenXml;
using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.Helpers;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.CaLamViecManagement;
using QuanLiCoffeeShop.MVVM.View.Admin.EmployeeManagement;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shell;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
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

        //lịch làm việc
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
        public ObservableCollection<string> ShiftList { get; set; } = new ObservableCollection<string>
        {
            "Ca sáng", "Ca chiều", "Ca tối",
        };
        private string selectedShiftName;
        public string SelectedShiftName
        {
            get => selectedShiftName;
            set
            {
                selectedShiftName = value;
                OnPropertyChanged(nameof(SelectedShiftName));
                ShiftNameToID(SelectedShiftName);
            }
        }
        private int selectedShiftId;
        public int SelectedShiftId
        {
            get => selectedShiftId;
            set
            {
                selectedShiftId = value;
                OnPropertyChanged(nameof(SelectedShiftId));
                LoadEmpInShiftCM.Execute(null);
                LoadEmpNotInShiftCM.Execute(null);
            }
        }
        public void ShiftNameToID(string shiftName)
        {
            if (shiftName == "Ca sáng")
            {
                SelectedShiftId = 1;
            }
            if (shiftName == "Ca chiều")
            {
                SelectedShiftId = 2;
            }
            if (shiftName == "Ca tối")
            {
                SelectedShiftId = 3;
            }
        }

        public ObservableCollection<string> WorkDayList { get; set; } = new ObservableCollection<string>
        {
            "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật",
        };
        private string selectedDayName;
        public string SelectedDayName
        {
            get => selectedDayName;
            set
            {
                selectedDayName = value;
                OnPropertyChanged(nameof(SelectedDayName));
                DayNameToInt(SelectedDayName);
            }
        }
        
        public void DayNameToInt(string dayName)
        {
            if (dayName == "Thứ 2")
            {
                SelectedWorkDay = 1;
            }
            if (dayName == "Thứ 3")
            {
                SelectedWorkDay = 2;
            }
            if (dayName == "Thứ 4")
            {
                SelectedWorkDay = 3;
            }
            if (dayName == "Thứ 5")
            {
                SelectedWorkDay = 4;
            }
            if (dayName == "Thứ 6")
            {
                SelectedWorkDay = 5;
            }
            if (dayName == "Thứ 7")
            {
                SelectedWorkDay = 6;
            }
            if (dayName == "Chủ nhật")
            {
                SelectedWorkDay = 7;
            }
        }
        private int selectedWorkDay;
        public int SelectedWorkDay
        {
            get => selectedWorkDay;
            set
            {
                selectedWorkDay = value;
                OnPropertyChanged(nameof(SelectedWorkDay));
                LoadEmpInShiftCM.Execute(null);
                LoadEmpNotInShiftCM.Execute(null);
            }
        }

        public ObservableCollection<EmployeeDTO> EmployeesInShift { get; set; } = new ObservableCollection<EmployeeDTO>();

        private ObservableCollection<EmployeeDTO> _selectedRemoveEmployees = new ObservableCollection<EmployeeDTO>();
        public ObservableCollection<EmployeeDTO> SelectedRemoveEmployees
        {
            get => _selectedRemoveEmployees;
            set
            {
                _selectedRemoveEmployees = value;
                OnPropertyChanged(nameof(SelectedRemoveEmployees));
            }
        }
        public ObservableCollection<EmployeeDTO> EmployeesNotInShift { get; set; } = new ObservableCollection<EmployeeDTO>();

        private ObservableCollection<EmployeeDTO> _selectedEmployees = new ObservableCollection<EmployeeDTO>();
        public ObservableCollection<EmployeeDTO> SelectedEmployeesNotInShift
        {
            get => _selectedEmployees;
            set
            {
                _selectedEmployees = value;
                OnPropertyChanged(nameof(SelectedEmployeesNotInShift));
            }
        }

        #region WorkShift Information 
        //ca sáng
        private int _startHourCS;
        public int StartHourCS
        {
            get => _startHourCS;
            set
            {
                _startHourCS = value;
                OnPropertyChanged(nameof(StartHourCS));
            }
        }

        private int _startMinuteCS;
        public int StartMinuteCS
        {
            get => _startMinuteCS;
            set
            {
                _startMinuteCS = value;
                OnPropertyChanged(nameof(StartMinuteCS));
            }
        }

        private int _endHourCS;
        public int EndHourCS
        {
            get => _endHourCS;
            set
            {
                _endHourCS = value;
                OnPropertyChanged(nameof(EndHourCS));
            }
        }

        private int _endMinuteCS;
        public int EndMinuteCS
        {
            get => _endMinuteCS;
            set
            {
                _endMinuteCS = value;
                OnPropertyChanged(nameof(EndMinuteCS));
            }
        }

        private decimal _wageCS;
        public decimal WageCS
        {
            get => _wageCS;
            set
            {
                _wageCS = value;
                OnPropertyChanged(nameof(WageCS));
            }
        }

        //ca chiều
        private int _startHourCC;
        public int StartHourCC
        {
            get => _startHourCC;
            set
            {
                _startHourCC = value;
                OnPropertyChanged(nameof(StartHourCC));
            }
        }

        private int _startMinuteCC;
        public int StartMinuteCC
        {
            get => _startMinuteCC;
            set
            {
                _startMinuteCC = value;
                OnPropertyChanged(nameof(StartMinuteCC));
            }
        }

        private int _endHourCC;
        public int EndHourCC
        {
            get => _endHourCC;
            set
            {
                _endHourCC = value;
                OnPropertyChanged(nameof(EndHourCC));
            }
        }

        private int _endMinuteCC;
        public int EndMinuteCC
        {
            get => _endMinuteCC;
            set
            {
                _endMinuteCC = value;
                OnPropertyChanged(nameof(EndMinuteCC));
            }
        }

        private decimal _wageCC;
        public decimal WageCC
        {
            get => _wageCC;
            set
            {
                _wageCC = value;
                OnPropertyChanged(nameof(WageCC));
            }
        }

        //ca tối
        private int _startHourCT;
        public int StartHourCT
        {
            get => _startHourCT;
            set
            {
                _startHourCT = value;
                OnPropertyChanged(nameof(StartHourCT));
            }
        }

        private int _startMinuteCT;
        public int StartMinuteCT
        {
            get => _startMinuteCT;
            set
            {
                _startMinuteCT = value;
                OnPropertyChanged(nameof(StartMinuteCT));
            }
        }

        private int _endHourCT;
        public int EndHourCT
        {
            get => _endHourCT;
            set
            {
                _endHourCT = value;
                OnPropertyChanged(nameof(EndHourCT));
            }
        }

        private int _endMinuteCT;
        public int EndMinuteCT
        {
            get => _endMinuteCT;
            set
            {
                _endMinuteCT = value;
                OnPropertyChanged(nameof(EndMinuteCT));
            }
        }

        private decimal _wageCT;
        public decimal WageCT
        {
            get => _wageCT;
            set
            {
                _wageCT = value;
                OnPropertyChanged(nameof(WageCT));
            }
        }
        #endregion         //điều chỉnh ca làm việc

        #region Request Information
        private ObservableCollection<RequestDTO> _reqList;
        public ObservableCollection<RequestDTO> ReqList
        {
            get { return _reqList; }
            set
            {
                _reqList = value;
                OnPropertyChanged(nameof(ReqList));
            }
        }
        private RequestDTO _selectedItem;
        public RequestDTO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    //tạo bản sao để edit
                    EditReq = new RequestDTO
                    {
                        REQ_ID = _selectedItem.REQ_ID,
                        REQ_DATE = _selectedItem.REQ_DATE,
                        REQ_STATUS = _selectedItem.REQ_STATUS,
                        REQ_TYPE = _selectedItem.REQ_TYPE,
                        EMPLOYEE = _selectedItem.EMPLOYEE,
                        EMP_COMMENT = _selectedItem.EMP_COMMENT,
                        EMP_ID = _selectedItem.EMP_ID,
                        APPROVED_BY = _selectedItem.APPROVED_BY,
                        APPROVED_DATE = _selectedItem.APPROVED_DATE,
                        APPROVER_COMMENT = _selectedItem.APPROVER_COMMENT,
                    };
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        private RequestDTO _editReq;
        public RequestDTO EditReq
        {
            get => _editReq;
            set
            {
                _editReq = value;
                OnPropertyChanged(nameof(EditReq));
            }
        }
        public ObservableCollection<string> StatusList { get; set; } = new ObservableCollection<string>
        {
            "Tất cả",
            "Chờ duyệt",
            "Đã duyệt",
            "Từ chối"
        };
        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();

                CommandManager.InvalidateRequerySuggested();
                if (FilterReqStatusCM.CanExecute(null))
                    FilterReqStatusCM.Execute(null);
            }
        }

        #endregion
        public ICommand EditEmpWdCM { get; set; }
        public ICommand AddEmpToShiftCM { get; set; }
        public ICommand RemoveEmpFromShiftCM { get; set; }
        public ICommand LoadEmpInShiftCM { get; set; }
        public ICommand LoadEmpNotInShiftCM { get; set; }
        public ICommand SaveEmpShiftToDatabaseCM { get; set; }

        public ICommand EditShiftWdCM { get; set; }
        public ICommand LoadShiftCM { get; set; }
        public ICommand SaveShiftChangesCM { get; set; }

        public ICommand ExportExcelCM { get; set; }

        public ICommand FirstLoadRequestCM { get; set; }
        public ICommand FilterReqStatusCM { get; set; }
        public ICommand OpenReqWdCM { get; set; }
        public ICommand EditReqCM { get; set; }
        public ICommand DeleteReqCM { get; set; }

        public WorkshiftViewModel()
        {
            LoadData();
            FirstLoadRequestCM = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                await ApplyFilter(SelectedStatus);
            });
            FilterReqStatusCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                await ApplyFilter(SelectedStatus);
            });
            OpenReqWdCM = new RelayCommand<object>((p) => { return SelectedItem != null; }, (p) =>
            {
                DonXinNghiWindow wd = new DonXinNghiWindow();
                wd.ShowDialog();
            });
            EditReqCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(EditReq.APPROVER_COMMENT) || string.IsNullOrEmpty(EditReq.REQ_STATUS))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu thông tin");
                    return;
                }
                REQUEST newReq = new REQUEST
                {
                    REQ_ID = SelectedItem.REQ_ID,
                    REQ_STATUS = EditReq.REQ_STATUS,
                    REQ_DATE = EditReq.REQ_DATE,
                    REQ_TYPE = EditReq.REQ_TYPE,
                    APPROVED_BY = MainViewModel.currentEmp.EMP_ID,
                    APPROVED_DATE = DateTime.Now,
                    APPROVER_COMMENT = EditReq.APPROVER_COMMENT,
                    EMPLOYEE = EditReq.EMPLOYEE,
                    EMP_COMMENT = EditReq.EMP_COMMENT,
                    EMP_ID = EditReq.EMP_ID,
                    IS_DELETED = false,
                };
                (bool success, string messageEdit) = await RequestService.Ins.EditRequest(newReq, SelectedItem.REQ_ID);
                if (success)
                {
                    await ApplyFilter(SelectedStatus);
                    await sendEmail(newReq.EMPLOYEE.EMP_EMAIL, newReq);
                    p.Close();
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageEdit);
                }
            });
            DeleteReqCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (SelectedItem == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa chọn sự cố để xóa");
                    return;
                }
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool sucess, string messageDelete) = await RequestService.Ins.DeleteRequest(SelectedItem.REQ_ID);
                    if (sucess)
                    {
                        ReqList.Remove(SelectedItem);
                        MessageBoxCustom.Show(MessageBoxCustom.Success, messageDelete);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                    }
                }
            });


            //thông báo loaddata lại khi chỉnh sửa tên NV
            Messenger.Default.Register<EmployeeUpdatedMessage>(this, async (msg) =>
            {
                LoadData();
                await ApplyFilter(SelectedStatus);
            });

            EditEmpWdCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                EditShiftWindow wd = new EditShiftWindow();
                wd.ShowDialog();
            });

            AddEmpToShiftCM = new RelayCommand<object>((p) => SelectedEmployeesNotInShift?.Count > 0, (p) =>
            {
                foreach (var employee in SelectedEmployeesNotInShift.ToList())
                {
                    EmployeesNotInShift.Remove(employee);
                    EmployeesInShift.Add(employee);
                }

                // Sắp xếp 
                var sortedEmployeesInShift = EmployeesInShift.OrderBy(emp => emp.EMP_ID).ToList();
                EmployeesInShift.Clear();
                foreach (var employee in sortedEmployeesInShift)
                {
                    EmployeesInShift.Add(employee);
                }
                var sortedEmployeesNotInShift = EmployeesNotInShift.OrderBy(emp => emp.EMP_ID).ToList();
                EmployeesNotInShift.Clear();
                foreach (var employee in sortedEmployeesNotInShift)
                {
                    EmployeesNotInShift.Add(employee);
                }
            });
            RemoveEmpFromShiftCM = new RelayCommand<object>((p) => SelectedRemoveEmployees?.Count > 0, (p) =>
            {
                foreach (var employee in SelectedRemoveEmployees.ToList())
                {
                    EmployeesInShift.Remove(employee);
                    EmployeesNotInShift.Add(employee);
                }

                // Sắp xếp 
                var sortedEmployeesInShift = EmployeesInShift.OrderBy(emp => emp.EMP_ID).ToList();
                EmployeesInShift.Clear();
                foreach (var employee in sortedEmployeesInShift)
                {
                    EmployeesInShift.Add(employee);
                }
                var sortedEmployeesNotInShift = EmployeesNotInShift.OrderBy(emp => emp.EMP_ID).ToList();
                EmployeesNotInShift.Clear();
                foreach (var employee in sortedEmployeesNotInShift)
                {
                    EmployeesNotInShift.Add(employee);
                }
            });


            LoadEmpInShiftCM = new RelayCommand<object>((p) => SelectedShiftId > 0 && SelectedWorkDay > 0, async (p) =>
            {
                if (SelectedShiftId > 0 && SelectedWorkDay > 0)
                {
                    // Tìm ca làm việc tương ứng
                    var selectedShift = Schedules.FirstOrDefault(s => s.ShiftName == SelectedShiftName.ToString());

                    if (selectedShift != null && selectedShift.EmployeeNamesByDay.ContainsKey(SelectedWorkDay))
                    {
                        var employeeList = selectedShift.EmployeeNamesByDay[SelectedWorkDay];
                        EmployeesInShift.Clear();

                        foreach (var empInfo in employeeList)
                        {
                            // Tách phần "NVxxx" từ chuỗi "NVxxx - Tên Nhân Viên"
                            var empIdString = empInfo.Split('-')[0].Trim();  // Lấy "NVxxx"
                            var empId = int.Parse(empIdString.Substring(2)); // Lấy phần "xxx" sau "NV"

                            var employee = await EmployeeService.Ins.GetEmployeeById(empId);
                            if (employee != null)
                            {
                                EmployeesInShift.Add(employee);
                            }
                        }
                    }
                    else
                    {
                        EmployeesInShift.Clear();
                    }
                }
                else return;
            });

            LoadEmpNotInShiftCM = new RelayCommand<object>((p) => SelectedShiftId > 0 && SelectedWorkDay > 0, async (p) =>
            {
                if (SelectedShiftId > 0 && SelectedWorkDay > 0)
                {
                    // Tìm ca làm việc tương ứng
                    var selectedShift = Schedules.FirstOrDefault(s => s.ShiftName == SelectedShiftName.ToString());

                    if (selectedShift != null)
                    {
                        var employeesInShift = selectedShift.EmployeeNamesByDay.ContainsKey(SelectedWorkDay)
                            ? selectedShift.EmployeeNamesByDay[SelectedWorkDay].Select(empInfo =>
                            {
                            var empIdString = empInfo.Split('-')[0].Trim(); // Lấy "NVxxx"
                                return int.Parse(empIdString.Substring(2));     // Lấy phần "xxx" sau "NV"
                            }).ToList()
                            : new List<int>();

                        // Lấy danh sách tất cả nhân viên
                        var allEmployees = await EmployeeService.Ins.GetAllEmp();

                        // Loại trừ các nhân viên đã ở trong ca
                        EmployeesNotInShift.Clear();
                        foreach (var employee in allEmployees)
                        {
                            if (!employeesInShift.Contains(employee.EMP_ID))
                            {
                                EmployeesNotInShift.Add(employee);
                            }
                        }
                    }
                    else
                    {
                        EmployeesNotInShift.Clear();
                    }
                }
                else return;
            });

            SaveEmpShiftToDatabaseCM = new RelayCommand<object>((p) => SelectedShiftId > 0 && SelectedWorkDay > 0, async (p) =>
            {
                try
                {
                    var employeeIds = EmployeesInShift.Select(emp => emp.EMP_ID).ToList();

                    if (employeeIds.Count > 0)
                    {
                        (bool success, string message) = await WorkshiftService.Ins.AddEmpToShift(employeeIds, SelectedShiftId, SelectedWorkDay);

                        if (success)
                        {
                            var dbEmployees = await WorkshiftService.Ins.GetEmployeesInShift(SelectedShiftId, SelectedWorkDay);

                            // Tìm các nhân viên cần xóa (có trong DB nhưng không có trong EmployeesInShift)
                            var employeesToRemove = dbEmployees.Where(dbEmp => !employeeIds.Contains(dbEmp.EMP_ID)).ToList();

                            foreach (var employee in employeesToRemove)
                            {
                                await WorkshiftService.Ins.RemoveEmpFromShift(employee.EMP_ID, SelectedShiftId, SelectedWorkDay);
                            }

                            MessageBoxCustom.Show(MessageBoxCustom.Success, "Cập nhật ca làm thành công!");
                            EmployeeService.Ins.UpdateEmployeeSalaries();
                            LoadData(); // Tải lại dữ liệu
                        }
                        else
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                        }
                    }
                    else
                    {
                        // Nếu EmployeesInShift trống, xóa toàn bộ nhân viên trong ca làm từ DB
                        var dbEmployees = await WorkshiftService.Ins.GetEmployeesInShift(SelectedShiftId, SelectedWorkDay);

                        foreach (var employee in dbEmployees)
                        {
                            await WorkshiftService.Ins.RemoveEmpFromShift(employee.EMP_ID, SelectedShiftId, SelectedWorkDay);
                        }

                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Đã xóa tất cả nhân viên khỏi ca làm!");
                        EmployeeService.Ins.UpdateEmployeeSalaries();
                        LoadData(); 
                    }
                }
                catch
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                }
            });

            EditShiftWdCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                DieuChinhCaLamViecWindow wd = new DieuChinhCaLamViecWindow();
                wd.ShowDialog();
            });

            LoadShiftCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                try
                {
                    using (var context = new CoffeeShopDBEntities())
                    {
                        var shiftCS = context.WORK_SHIFT.FirstOrDefault(s => s.SHIFT_NAME == "Ca sáng" && s.IS_DELETED == false);
                        if (shiftCS != null)
                        {
                            StartHourCS = shiftCS.START_TIME?.Hour ?? 0;
                            StartMinuteCS = shiftCS.START_TIME?.Minute ?? 0;
                            EndHourCS = shiftCS.END_TIME?.Hour ?? 0;
                            EndMinuteCS = shiftCS.END_TIME?.Minute ?? 0;
                            WageCS = shiftCS.WAGE ?? 0;
                        }
                        var shiftCC = context.WORK_SHIFT.FirstOrDefault(s => s.SHIFT_NAME == "Ca chiều" && s.IS_DELETED == false);
                        if (shiftCC != null)
                        {
                            StartHourCC = shiftCC.START_TIME?.Hour ?? 0;
                            StartMinuteCC = shiftCC.START_TIME?.Minute ?? 0;
                            EndHourCC = shiftCC.END_TIME?.Hour ?? 0;
                            EndMinuteCC = shiftCC.END_TIME?.Minute ?? 0;
                            WageCC = shiftCC.WAGE ?? 0;
                        }

                        var shiftCT = context.WORK_SHIFT.FirstOrDefault(s => s.SHIFT_NAME == "Ca tối" && s.IS_DELETED == false);
                        if (shiftCT != null)
                        {
                            StartHourCT = shiftCT.START_TIME?.Hour ?? 0;
                            StartMinuteCT = shiftCT.START_TIME?.Minute ?? 0;
                            EndHourCT = shiftCT.END_TIME?.Hour ?? 0;
                            EndMinuteCT = shiftCT.END_TIME?.Minute ?? 0;
                            WageCT = shiftCT.WAGE ?? 0;
                        }
                    }
                }
                catch
                {
                    return;
                }
            });

            SaveShiftChangesCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                try
                {
                    using (var context = new CoffeeShopDBEntities())
                    {
                        var shiftCS = context.WORK_SHIFT.FirstOrDefault(s => s.SHIFT_NAME == "Ca sáng" && s.IS_DELETED == false);
                        if (shiftCS != null)
                        {
                            shiftCS.START_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, StartHourCS, StartMinuteCS, 0);
                            shiftCS.END_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, EndHourCS, EndMinuteCS, 0);
                            shiftCS.WAGE = WageCS;
                            context.SaveChanges();
                        }

                        var shiftCC = context.WORK_SHIFT.FirstOrDefault(s => s.SHIFT_NAME == "Ca chiều" && s.IS_DELETED == false);
                        if (shiftCC != null)
                        {
                            shiftCC.START_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, StartHourCC, StartMinuteCC, 0);
                            shiftCC.END_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, EndHourCC, EndMinuteCC, 0);
                            shiftCC.WAGE = WageCC;
                            context.SaveChanges();
                        }

                        var shiftCT = context.WORK_SHIFT.FirstOrDefault(s => s.SHIFT_NAME == "Ca tối" && s.IS_DELETED == false);
                        if (shiftCT != null)
                        {
                            shiftCT.START_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, StartHourCT, StartMinuteCT, 0);
                            shiftCT.END_TIME = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, EndHourCT, EndMinuteCT, 0);
                            shiftCT.WAGE = WageCT;
                            context.SaveChanges();
                        }
                        EmployeeService.Ins.UpdateEmployeeSalaries();
                        LoadData(); //load lại trang
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Cập nhật ca làm thành công");
                    }
                }
                catch
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                }
            });

            ExportExcelCM = new RelayCommand<object>((p) => { return Schedules != null && Schedules.Any(); }, (p) =>
            {

                var dialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Chọn nơi lưu file Excel",
                    FileName = "ShiftSchedule.xlsx"
                };

                if (dialog.ShowDialog() == true)
                {
                    ExportScheduleToExcel(dialog.FileName, Schedules);
                }
            });
        }
        private void ExportScheduleToExcel(string filePath, ObservableCollection<ShiftScheduleDTO> schedules)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var worksheet = excel.Workbook.Worksheets.Add("Lịch làm việc");

                    // Thêm tiêu đề chính
                    worksheet.Cells["A1:H2"].Merge = true; // Merge từ A1 đến H2
                    worksheet.Cells["A1"].Value = "Lịch làm việc";
                    worksheet.Cells["A1"].Style.Font.Size = 16; // Tăng cỡ chữ
                    worksheet.Cells["A1"].Style.Font.Bold = true; // In đậm
                    worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    //Thêm tiêu đề cột
                    string[] headers = new string[] { "Ca làm việc", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật" };

                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[3, i + 1].Value = headers[i];
                        worksheet.Cells[3, i + 1].Style.Font.Bold = true; // In đậm tiêu đề
                        worksheet.Cells[3, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[3, i + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    }

                    // Ghi dữ liệu
                    int row = 4; // Bắt đầu từ dòng 4

                    foreach (var schedule in schedules)
                    {
                        // Cột 1: Tên ca làm việc + thời gian
                        worksheet.Cells[row, 1].Value = $"{schedule.DisplayName}";
                        worksheet.Cells[row, 1].Style.WrapText = true; // Cho phép xuống dòng trong ô

                            // Các cột: Thứ 2 -> Chủ nhật
                            for (int day = 2; day <= 8; day++) // Từ cột 2 (Thứ 2) đến cột 8 (Chủ nhật)
                            {
                                if (schedule.EmployeeNamesByDay.ContainsKey(day - 1)) // day - 1 tương ứng với Thứ 2 = 2
                                {
                                    var employeeList = schedule.EmployeeNamesByDay[day - 1];
                                    worksheet.Cells[row, day].Value = string.Join("\n", employeeList); // Xuống dòng
                                    worksheet.Cells[row, day].Style.WrapText = true; // Cho phép xuống dòng trong ô
                                }
                            }

                            row++;
                    }

                    // Auto-fit các cột
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Điều chỉnh chiều rộng cột tối thiểu
                    worksheet.Column(1).Width = Math.Max(worksheet.Column(1).Width, 17); //chiều rộng cột ca làm tối thiểu 17
                    
                    for(int i = 0; i < 3;  i++) //căn giữa và in đậm cho ca làm
                    {
                        worksheet.Cells[i + 4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[i + 4, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[i + 4, 1].Style.Font.Bold = true;
                    }

                    for (int col = 2; col <= worksheet.Dimension.End.Column; col++)
                    {
                        worksheet.Column(col).Width = Math.Max(worksheet.Column(col).Width, 30); // Đặt chiều rộng tối thiểu là 30
                    }

                    // Lưu file Excel
                    FileInfo fileInfo = new FileInfo(filePath);
                    excel.SaveAs(fileInfo);

                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Xuất file excel thành công!");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xuất file excel thất bại");
            }
        }
        #region Method
        private void LoadData()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var query = context.EMPLOYEE_SHIFT
                        .Include(es => es.EMPLOYEE)
                        .Include(es => es.WORK_SHIFT)
                        .Where(es => (es.IS_DELETED ?? false) == false &&
                                     (es.EMPLOYEE.IS_DELETED ?? false) == false &&
                                     (es.WORK_SHIFT.IS_DELETED ?? false) == false)
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
                                .Select(es => $"NV{es.EMPLOYEE.EMP_ID.ToString("D3")} - {es.EMPLOYEE.EMP_NAME}")
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
        private async Task ApplyFilter(string filterStatus)
        {
            filterStatus = filterStatus?.ToLower() ?? string.Empty;
            var allReq = await RequestService.Ins.GetAllRequest();

            if (string.IsNullOrWhiteSpace(filterStatus))
            {
                ReqList = new ObservableCollection<RequestDTO>(allReq);
                return;
            }
            // Lọc 
            ReqList = new ObservableCollection<RequestDTO>(allReq.FindAll(x =>
                (string.IsNullOrEmpty(filterStatus) || filterStatus == "tất cả" ||
                 (x.REQ_STATUS?.ToLower().Contains(filterStatus) ?? false))
            ));
        }
        private async Task sendEmail(string email, REQUEST rEQUEST)
        {
            try
            {
                var approver = await EmployeeService.Ins.GetEmployeeById(rEQUEST.APPROVED_BY ?? 0);
                string approver_name = approver.EMP_NAME;

                string body = "Mã đơn xin phép: " + $"REQ{rEQUEST.REQ_ID.ToString("D3")}" + "\nNgày gửi đơn: " + rEQUEST.REQ_DATE
                    + "\nNgày phê duyệt: " + rEQUEST.APPROVED_DATE + "\nNgười phê duyệt: " + approver_name
                    + "\nTrạng thái: " + rEQUEST.REQ_STATUS + "\nPhản hồi: " + rEQUEST.APPROVER_COMMENT;

                var smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.Port = 587;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("coffeeshophello@gmail.com", "klciusqavysaiorf");

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.From = new MailAddress("coffeeshophello@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Coffee Shop - Phản hồi đơn xin phép";
                mail.IsBodyHtml = false;
                mail.Body = body;
                await smtpClient.SendMailAsync(mail);
                MessageBoxCustom.Show(MessageBoxCustom.Success, "Đã gửi email thành công");
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra, vui lòng nhập lại!");
            }
        }
        #endregion
    }
}
