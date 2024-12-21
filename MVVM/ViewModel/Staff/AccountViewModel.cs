using GalaSoft.MvvmLight.Messaging;
using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.Helpers;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Data.Entity;
using static QuanLiCoffeeShop.MVVM.ViewModel.Staff.WorkshiftViewModel;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff
{
    public class AccountViewModel : ObservableObject
    {
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

        private string oldPassword;
        public string OldPassword
        {
            get { return oldPassword; }
            set { oldPassword = value; }
        }

        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; }
        }

        private string newPasswordConfirm;
        public string NewPasswordConfirm
        {
            get { return newPasswordConfirm; }
            set { newPasswordConfirm = value; }
        }

        public ICommand EditInforCM { get; set; }
        public ICommand UpdatePasswordCM { get; set; }
        public event Action ResetPasswordRequested;

        public AccountViewModel()
        {
            EditEmp = new EmployeeDTO();
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                // Chỉ khởi tạo EditEmp với currentEmp tại runtime
                EditEmp = new EmployeeDTO
                {
                    EMP_ID = MainViewModel.currentEmp.EMP_ID,
                    EMP_NAME = MainViewModel.currentEmp.EMP_NAME,
                    EMP_GENDER = MainViewModel.currentEmp.EMP_GENDER,
                    EMP_EMAIL = MainViewModel.currentEmp.EMP_EMAIL,
                    EMP_PHONE = MainViewModel.currentEmp.EMP_PHONE,
                    EMP_STATUS = MainViewModel.currentEmp.EMP_STATUS,
                    EMP_USERNAME = MainViewModel.currentEmp.EMP_USERNAME,
                    EMP_PASSWORD = MainViewModel.currentEmp.EMP_PASSWORD,
                    EMP_STARTDATE = MainViewModel.currentEmp.EMP_STARTDATE,
                    EMPLOYEE_SHIFT = MainViewModel.currentEmp.EMPLOYEE_SHIFT,
                    EMP_CCCD = MainViewModel.currentEmp.EMP_CCCD,
                    EMP_BIRTHDAY = MainViewModel.currentEmp.EMP_BIRTHDAY,
                    EMP_ROLE = MainViewModel.currentEmp.EMP_ROLE,
                    EMP_TotalShifts = MainViewModel.currentEmp.EMP_TotalShifts,
                    EMP_SALARY = MainViewModel.currentEmp.EMP_SALARY
                };
            }
            EditInforCM = new RelayCommand<object>(null, async (p) =>
            {
                if (string.IsNullOrEmpty(EditEmp.EMP_NAME) || string.IsNullOrEmpty(EditEmp.EMP_PHONE) || string.IsNullOrEmpty(EditEmp.EMP_EMAIL) || string.IsNullOrEmpty(EditEmp.EMP_USERNAME))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                    return;
                }

                else
                {
                    if (EditEmp.EMP_USERNAME == MainViewModel.currentEmp.EMP_USERNAME && EditEmp.EMP_NAME == MainViewModel.currentEmp.EMP_NAME &&
                        EditEmp.EMP_EMAIL == MainViewModel.currentEmp.EMP_EMAIL && EditEmp.EMP_PHONE == MainViewModel.currentEmp.EMP_PHONE)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Không có gì mới để chỉnh sửa");
                        return;
                    }
                    EMPLOYEE newEmp = new EMPLOYEE
                    {
                        EMP_ID = EditEmp.EMP_ID,
                        EMP_NAME = EditEmp.EMP_NAME,
                        EMP_PHONE = EditEmp.EMP_PHONE,
                        EMP_EMAIL = EditEmp.EMP_EMAIL,
                        EMP_CCCD = EditEmp.EMP_CCCD,
                        EMP_BIRTHDAY = EditEmp.EMP_BIRTHDAY,
                        EMP_GENDER = EditEmp.EMP_GENDER,
                        EMP_PASSWORD = EditEmp.EMP_PASSWORD,
                        EMP_ROLE = EditEmp.EMP_ROLE,
                        EMP_SALARY = EditEmp.EMP_SALARY,
                        EMP_STARTDATE = EditEmp.EMP_STARTDATE,
                        EMP_STATUS = EditEmp.EMP_STATUS,
                        EMP_USERNAME = EditEmp.EMP_USERNAME,
                        IS_DELETED = false,
                    };
                    (bool success, string messageEdit) = await EmployeeService.Ins.EditEmpList(newEmp, EditEmp.EMP_ID);
                    if (success)
                    {
                        using (var context = new CoffeeShopDBEntities())
                        {
                            EMPLOYEE emp = await context.EMPLOYEEs.Where(x => x.EMP_ID == EditEmp.EMP_ID).FirstOrDefaultAsync();
                            EmployeeDTO curEmp = new EmployeeDTO
                            {
                                EMP_ID = emp.EMP_ID,
                                EMP_NAME = emp.EMP_NAME,
                                EMP_PHONE = emp.EMP_PHONE,
                                EMP_EMAIL = emp.EMP_EMAIL,
                                EMP_CCCD = emp.EMP_CCCD,
                                EMP_BIRTHDAY = emp.EMP_BIRTHDAY,
                                EMP_GENDER = emp.EMP_GENDER,
                                EMP_PASSWORD = emp.EMP_PASSWORD,
                                EMP_ROLE = emp.EMP_ROLE,
                                EMP_SALARY = emp.EMP_SALARY,
                                EMP_STARTDATE = emp.EMP_STARTDATE,
                                EMP_STATUS = emp.EMP_STATUS,
                                EMP_USERNAME = emp.EMP_USERNAME,
                                EMP_TotalShifts = await Task.Run(() => EmployeeService.Ins.GetEmployeeTotalShifts(emp.EMP_ID)),
                                IS_DELETED = emp.IS_DELETED,
                            };
                            MainViewModel.currentEmp = curEmp;
                            Messenger.Default.Send(new PropertyChangedMessage<string>(null, curEmp.EMP_NAME, nameof(MainViewModel.CurrentName))); //thông báo update CurrentName cho MainVM

                        }
                        Messenger.Default.Send(new EmployeeUpdatedMessage(newEmp.EMP_ID, newEmp.EMP_NAME)); //thông báo để update bên view ca làm việc
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Bạn đã chỉnh sửa thành công");
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra");
                    }
                }
            });

            UpdatePasswordCM = new RelayCommand<object>(null, async (p) =>
            {
                if (string.IsNullOrEmpty(OldPassword))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa nhập mật khẩu cũ");
                    return;
                }
                if (string.IsNullOrEmpty(NewPassword))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa nhập mật khẩu mới");
                    return;
                }
                if (string.IsNullOrEmpty(NewPasswordConfirm))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa nhập mật khẩu xác thực mới");
                    return;
                }
                if (NewPassword != NewPasswordConfirm)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Mật khẩu xác thực không đúng");
                    return;
                }
                if (PasswordHelper.MD5Hash(OldPassword) != MainViewModel.currentEmp.EMP_PASSWORD)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Mật khẩu cũ không đúng");
                    return;
                }

                EMPLOYEE newEmp = new EMPLOYEE
                {
                    EMP_ID = MainViewModel.currentEmp.EMP_ID,
                    EMP_NAME = MainViewModel.currentEmp.EMP_NAME,
                    EMP_PHONE = MainViewModel.currentEmp.EMP_PHONE,
                    EMP_EMAIL = MainViewModel.currentEmp.EMP_EMAIL,
                    EMP_CCCD = MainViewModel.currentEmp.EMP_CCCD,
                    EMP_BIRTHDAY = MainViewModel.currentEmp.EMP_BIRTHDAY,
                    EMP_GENDER = MainViewModel.currentEmp.EMP_GENDER,
                    EMP_PASSWORD = PasswordHelper.MD5Hash(NewPassword),
                    EMP_ROLE = MainViewModel.currentEmp.EMP_ROLE,
                    EMP_SALARY = MainViewModel.currentEmp.EMP_SALARY,
                    EMP_STARTDATE = MainViewModel.currentEmp.EMP_STARTDATE,
                    EMP_STATUS = MainViewModel.currentEmp.EMP_STATUS,
                    EMP_USERNAME = MainViewModel.currentEmp.EMP_USERNAME,
                    EMPLOYEE_SHIFT = MainViewModel.currentEmp.EMPLOYEE_SHIFT,
                    IS_DELETED = false,
                };
                (bool success, string messageEdit) = await EmployeeService.Ins.EditEmpList(newEmp, EditEmp.EMP_ID);
                if (success)
                {
                    using (var context = new CoffeeShopDBEntities())
                    {
                        EMPLOYEE emp = await context.EMPLOYEEs.Where(x => x.EMP_ID == EditEmp.EMP_ID).FirstOrDefaultAsync();
                        EmployeeDTO curEmp = new EmployeeDTO
                        {
                            EMP_ID = emp.EMP_ID,
                            EMP_NAME = emp.EMP_NAME,
                            EMP_PHONE = emp.EMP_PHONE,
                            EMP_EMAIL = emp.EMP_EMAIL,
                            EMP_CCCD = emp.EMP_CCCD,
                            EMP_BIRTHDAY = emp.EMP_BIRTHDAY,
                            EMP_GENDER = emp.EMP_GENDER,
                            EMP_PASSWORD = emp.EMP_PASSWORD,
                            EMP_ROLE = emp.EMP_ROLE,
                            EMP_SALARY = emp.EMP_SALARY,
                            EMP_STARTDATE = emp.EMP_STARTDATE,
                            EMP_STATUS = emp.EMP_STATUS,
                            EMP_USERNAME = emp.EMP_USERNAME,
                            EMP_TotalShifts = await Task.Run(() => EmployeeService.Ins.GetEmployeeTotalShifts(emp.EMP_ID)),
                            IS_DELETED = emp.IS_DELETED,
                        };
                        MainViewModel.currentEmp = curEmp;
                    }
                    ResetPasswordRequested?.Invoke();
                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Đổi mật khẩu thành công");
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                }
            });

        }
    }
}
