using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using QuanLiCoffeeShop.MVVM.View.Login;
using QuanLiCoffeeShop.MVVM.View.Admin;
using QuanLiCoffeeShop.MVVM.View.Staff;
using QuanLiCoffeeShop.MVVM.Model;
using System.Data.Entity;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.ViewModel.Admin;
using QuanLiCoffeeShop.MVVM.ViewModel.Staff;
using QuanLiCoffeeShop.Helpers;
using QuanLiCoffeeShop.MVVM.Model.Services;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Login
{
    class LoginViewModel : ObservableObject
    {
        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _forgotEmail;

        public string ForgotEmail
        {
            get { return _forgotEmail; }
            set { _forgotEmail = value; OnPropertyChanged(); }
        }
        private bool IsLogin = false;

        public ICommand LoginCommand { get; set; }
        public ICommand ForgotPasswordCM { get; set; }
        public ICommand SendCM { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    return false;
                }
                return true;
            },
            async (p) =>
            {
                if (IsLogin == false)
                {
                    IsLogin = true;
                    await Login(p);
                    IsLogin = false;
                }
            });
            ForgotPasswordCM = new RelayCommand<TextBlock>((p) => { return true; }, (p) =>
            {
                ForgotPasswordWindow wd = new ForgotPasswordWindow();
                wd.ShowDialog();
            });
            SendCM = new RelayCommand<Window>((p) => 
            { if (string.IsNullOrEmpty(ForgotEmail)) 
                    return false;
                return true; 
            }, async (p) =>
            {
                string newPass = PasswordHelper.randomCode();
                (bool updateSuccess, string message, string username) = await EmployeeService.Ins.UpdatePassword(ForgotEmail, newPass);
                if (!updateSuccess)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                }
                else
                {
                    await LoginService.Ins.sendEmail(ForgotEmail, newPass, username);
                }
            });
        }
        async Task Login(Window p)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    string password = PasswordHelper.MD5Hash(Password);
                    EMPLOYEE emp = await context.EMPLOYEEs.Where(x => x.EMP_USERNAME == Username && x.EMP_PASSWORD == password && x.IS_DELETED == false).FirstOrDefaultAsync();
                    if (emp != null)
                    {
                        p.Visibility = Visibility.Collapsed;
                        EmployeeDTO curEmp = new EmployeeDTO
                        {
                            EMP_ID = emp.EMP_ID,
                            EMP_EMAIL = emp.EMP_EMAIL,
                            EMP_BIRTHDAY = emp.EMP_BIRTHDAY,
                            EMPLOYEE_SHIFT = emp.EMPLOYEE_SHIFT,
                            EMP_CCCD = emp.EMP_CCCD,
                            EMP_GENDER = emp.EMP_GENDER,
                            EMP_NAME = emp.EMP_NAME,
                            EMP_PASSWORD = password,
                            EMP_PHONE = emp.EMP_PHONE,
                            EMP_ROLE = emp.EMP_ROLE,
                            EMP_SALARY = emp.EMP_SALARY,
                            EMP_STARTDATE = emp.EMP_STARTDATE,
                            EMP_STATUS = emp.EMP_STATUS,
                            EMP_USERNAME = emp.EMP_USERNAME,
                            EMP_TotalShifts = await Task.Run(() => EmployeeService.Ins.GetEmployeeTotalShifts(emp.EMP_ID)),
                            IS_DELETED = emp.IS_DELETED,
                        };
                        if (emp.EMP_ROLE == "Quản lý")
                        {
                            Admin.MainViewModel.currentEmp = curEmp;
                            MainAdminWindow ad = new MainAdminWindow
                            {
                                Owner = p
                            };
                            ad.Show();

                        }
                        else
                        {
                            Staff.MainViewModel.currentEmp = curEmp;
                            MainStaffWindow st = new MainStaffWindow
                            {
                                Owner = p
                            };
                            st.Show();
                        }
                        
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Sai tài khoản hoặc mật khẩu, vui lòng nhập lại!");
                    }
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra khi đăng nhập");
            }
        }

    }
}
