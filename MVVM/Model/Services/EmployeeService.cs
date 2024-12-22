using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.Helpers;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shell;

namespace QuanLiCoffeeShop.MVVM.Model.Services
{
    public class EmployeeService
    {
        public EmployeeService() { }
        private static EmployeeService _ins;

        public static EmployeeService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new EmployeeService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<List<EmployeeDTO>> GetAllEmp()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var empList = (from s in context.EMPLOYEEs
                                   where s.IS_DELETED == false
                                   select new EmployeeDTO
                                   {
                                       EMP_ID = s.EMP_ID,
                                       EMP_NAME = s.EMP_NAME,
                                       EMP_GENDER = s.EMP_GENDER,
                                       EMP_CCCD = s.EMP_CCCD,
                                       EMP_EMAIL = s.EMP_EMAIL,
                                       EMP_PHONE = s.EMP_PHONE,
                                       EMP_BIRTHDAY = s.EMP_BIRTHDAY,
                                       EMP_USERNAME = s.EMP_USERNAME,
                                       EMP_PASSWORD = s.EMP_PASSWORD,
                                       EMP_ROLE = s.EMP_ROLE,
                                       EMP_SALARY = s.EMP_SALARY,
                                       EMP_STARTDATE = s.EMP_STARTDATE,
                                       EMP_STATUS = s.EMP_STATUS,
                                       EMPLOYEE_SHIFT = s.EMPLOYEE_SHIFT,
                                       IS_DELETED = s.IS_DELETED,
                                   }).ToListAsync();
                    return await empList;
                }
            }
            catch
            {
                return null;
            }

        }

        public async Task<(bool, string)> AddNewEmp(EMPLOYEE newEmp)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    if (string.IsNullOrEmpty(newEmp.EMP_CCCD) || string.IsNullOrEmpty(newEmp.EMP_EMAIL) || string.IsNullOrEmpty(newEmp.EMP_NAME) ||
                        string.IsNullOrEmpty(newEmp.EMP_PHONE) || string.IsNullOrEmpty(newEmp.EMP_ROLE) || 
                        string.IsNullOrEmpty(newEmp.EMP_USERNAME) || string.IsNullOrEmpty(newEmp.EMP_GENDER))
                        return (false, "Bạn nhập thiếu thông tin");
                    if (DateTime.Compare((DateTime)newEmp.EMP_BIRTHDAY, new DateTime(1900, 1, 1)) < 0 || DateTime.Compare((DateTime)newEmp.EMP_BIRTHDAY, DateTime.Now) > 0)
                    {
                        return (false, "Ngày sinh không hợp lệ");
                    }

                    if (DateTime.Compare((DateTime)newEmp.EMP_STARTDATE, new DateTime(1900, 1, 1)) < 0 || DateTime.Compare((DateTime)newEmp.EMP_STARTDATE, DateTime.Now) > 0)
                    {
                        return (false, "Ngày bắt đầu không hợp lệ");
                    }

                    if (((DateTime)newEmp.EMP_STARTDATE).Year - ((DateTime)newEmp.EMP_BIRTHDAY).Year < 16)
                    {
                        return (false, "Đảm bảo nhân viên vào làm trên 16 tuổi");
                    }

                    bool IsCccdExist = await context.EMPLOYEEs.AnyAsync(p => p.EMP_CCCD == newEmp.EMP_CCCD);
                    bool IsEmailExist = await context.EMPLOYEEs.AnyAsync(p => p.EMP_EMAIL == newEmp.EMP_EMAIL);
                    bool IsPhoneExist = await context.EMPLOYEEs.AnyAsync(p => p.EMP_PHONE == newEmp.EMP_PHONE);
                    bool IsExistUsername = await context.EMPLOYEEs.AnyAsync(p => p.EMP_USERNAME == newEmp.EMP_USERNAME);

                    var emp = await context.EMPLOYEEs.Where(p => p.EMP_CCCD == newEmp.EMP_CCCD || p.EMP_EMAIL == newEmp.EMP_EMAIL || p.EMP_PHONE == newEmp.EMP_PHONE || p.EMP_USERNAME == newEmp.EMP_USERNAME).FirstOrDefaultAsync();
                    if (emp != null)
                    {
                        if (emp.IS_DELETED == true)
                        {
                            emp.EMP_NAME = newEmp.EMP_NAME;
                            emp.EMP_GENDER = newEmp.EMP_GENDER;
                            emp.EMP_CCCD = newEmp.EMP_CCCD;
                            emp.EMP_EMAIL = newEmp.EMP_EMAIL;
                            emp.EMP_PHONE = newEmp.EMP_PHONE;
                            emp.EMP_BIRTHDAY = newEmp.EMP_BIRTHDAY;
                            emp.EMP_USERNAME = newEmp.EMP_USERNAME;
                            emp.EMP_PASSWORD = newEmp.EMP_PASSWORD;
                            emp.EMP_ROLE = newEmp.EMP_ROLE;
                            emp.EMP_SALARY = newEmp.EMP_SALARY;
                            emp.EMP_STARTDATE = newEmp.EMP_STARTDATE;
                            emp.EMP_STATUS = newEmp.EMP_STATUS;
                            emp.IS_DELETED = false;
                            await context.SaveChangesAsync();
                            return (true, "Khôi phục tài khoản thành công");

                        }
                        else
                        {
                            if (IsCccdExist)
                            {
                                return (false, "Đã tồn tại CCCD này");
                            }
                            if (IsEmailExist)
                            {
                                return (false, "Đã tồn tại email này");
                            }
                            if (IsPhoneExist)
                            {
                                return (false, "Đã tồn tại số điện thoại này");
                            }
                            if (IsExistUsername)
                            {
                                return (false, "Tài khoản đã tồn tại");
                            }
                        }
                    }
                    context.EMPLOYEEs.Add(newEmp);
                    await context.SaveChangesAsync();
                    return (true, "Thêm nhân viên thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi");
            }
        }

        public async Task<(bool, string)> EditEmpList(EMPLOYEE newEmp, int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    if (string.IsNullOrEmpty(newEmp.EMP_CCCD) || string.IsNullOrEmpty(newEmp.EMP_EMAIL) || string.IsNullOrEmpty(newEmp.EMP_NAME) ||
                        string.IsNullOrEmpty(newEmp.EMP_PHONE) || string.IsNullOrEmpty(newEmp.EMP_ROLE) ||
                        string.IsNullOrEmpty(newEmp.EMP_USERNAME) || string.IsNullOrEmpty(newEmp.EMP_GENDER))
                        return (false, "Bạn nhập thiếu thông tin");
                    if (DateTime.Compare((DateTime)newEmp.EMP_BIRTHDAY, new DateTime(1900, 1, 1)) < 0 || DateTime.Compare((DateTime)newEmp.EMP_BIRTHDAY, DateTime.Now) > 0)
                    {
                        return (false, "Ngày sinh không hợp lệ");
                    }

                    if (DateTime.Compare((DateTime)newEmp.EMP_STARTDATE, new DateTime(1900, 1, 1)) < 0 || DateTime.Compare((DateTime)newEmp.EMP_STARTDATE, DateTime.Now) > 0)
                    {
                        return (false, "Ngày bắt đầu không hợp lệ");
                    }

                    if (((DateTime)newEmp.EMP_STARTDATE).Year - ((DateTime)newEmp.EMP_BIRTHDAY).Year < 16)
                    {
                        return (false, "Đảm bảo nhân viên vào làm trên 16 tuổi");
                    }

                    bool IsExistUsername = await context.EMPLOYEEs.AnyAsync(p => p.EMP_ID != newEmp.EMP_ID && p.EMP_USERNAME == newEmp.EMP_USERNAME && p.IS_DELETED == false);
                    if (IsExistUsername)
                    {
                        return (false, "Tài khoản đã tồn tại");
                    }
                    var emp = await context.EMPLOYEEs.Where(p => p.EMP_ID == ID).FirstOrDefaultAsync();
                    if (emp == null) return (false, "Không tìm thấy ID");
                    emp.EMP_NAME = newEmp.EMP_NAME;
                    emp.EMP_GENDER = newEmp.EMP_GENDER;
                    emp.EMP_CCCD = newEmp.EMP_CCCD;
                    emp.EMP_EMAIL = newEmp.EMP_EMAIL;
                    emp.EMP_PHONE = newEmp.EMP_PHONE;
                    emp.EMP_BIRTHDAY = newEmp.EMP_BIRTHDAY;
                    emp.EMP_USERNAME = newEmp.EMP_USERNAME;
                    emp.EMP_PASSWORD = newEmp.EMP_PASSWORD;
                    emp.EMP_ROLE = newEmp.EMP_ROLE;
                    emp.EMP_SALARY = newEmp.EMP_SALARY;
                    emp.EMP_STARTDATE = newEmp.EMP_STARTDATE;
                    emp.EMP_STATUS = newEmp.EMP_STATUS;
                    await context.SaveChangesAsync();
                    return (true, "Chỉnh sửa thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi chỉnh sửa nhân viên");
            }
        }
        public async Task<(bool, string)> DeleteEmployee(int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var emp = await context.EMPLOYEEs.Where(p => p.EMP_ID == ID).FirstOrDefaultAsync();
                    if (emp.IS_DELETED == true) return (false, "Đã xóa nhân viên này rồi");
                    emp.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi xóa nhân viên");
            }

        }
        public int GetEmployeeTotalShifts(int empId)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var employee = context.EMPLOYEEs
                                       .Include(e => e.EMPLOYEE_SHIFT) // Bao gồm ca làm việc của nhân viên
                                       .FirstOrDefault(e => e.EMP_ID == empId);

                    if (employee == null)
                    {
                        throw new Exception("Không tìm thấy nhân viên nào");
                    }

                    // Lọc ra các ca làm việc chưa bị xóa
                    var validShifts = employee.EMPLOYEE_SHIFT.Where(e => !(e.IS_DELETED ?? false)).ToList();

                    return validShifts.Count; // Đếm số ca làm việc hợp lệ
                }
            }
            catch
            {
                return 0;
            }
        }
        public async Task<EmployeeDTO> GetEmployeeById(int empId)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var emp = await context.EMPLOYEEs
                        .Where(s => s.IS_DELETED == false && s.EMP_ID == empId)
                        .FirstOrDefaultAsync(); 

                    if (emp == null)
                    {
                        return null;  
                    }

                    // Nếu tìm thấy, chuyển đổi sang EmployeeDTO
                    return new EmployeeDTO
                    {
                        EMP_ID = emp.EMP_ID,
                        EMP_NAME = emp.EMP_NAME,
                        EMP_GENDER = emp.EMP_GENDER,
                        EMP_CCCD = emp.EMP_CCCD,
                        EMP_EMAIL = emp.EMP_EMAIL,
                        EMP_PHONE = emp.EMP_PHONE,
                        EMP_BIRTHDAY = emp.EMP_BIRTHDAY,
                        EMP_USERNAME = emp.EMP_USERNAME,
                        EMP_PASSWORD = emp.EMP_PASSWORD,
                        EMP_ROLE = emp.EMP_ROLE,
                        EMP_SALARY = emp.EMP_SALARY,
                        EMP_STARTDATE = emp.EMP_STARTDATE,
                        EMP_STATUS = emp.EMP_STATUS,
                        EMPLOYEE_SHIFT = emp.EMPLOYEE_SHIFT,
                        IS_DELETED = emp.IS_DELETED,
                    };
                }
            }
            catch
            {
                return null;
            }
        }
        public void UpdateEmployeeSalaries()
        {
            using (var context = new CoffeeShopDBEntities())
            {
                var employees = context.EMPLOYEEs
                    .Where(e => e.IS_DELETED == false)
                    .ToList();

                foreach (var employee in employees)
                {
                    var totalSalary = context.EMPLOYEE_SHIFT
                        .Where(es => es.EMP_ID == employee.EMP_ID
                                     && es.IS_DELETED == false
                                     && es.WORK_SHIFT != null)
                        .Select(es => es.WORK_SHIFT.WAGE)
                        .ToList()
                        .Sum(wage => wage ?? 0); // Kiểm tra null sau khi dữ liệu đã ở bộ nhớ


                    employee.EMP_SALARY = totalSalary;
                }

                context.SaveChanges();
            }
        }
        public async Task<(bool, string, string)> UpdatePassword(string email, string newPass)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var emp = await context.EMPLOYEEs.Where(p => p.EMP_EMAIL == email).FirstOrDefaultAsync();
                    if (emp != null && emp.IS_DELETED == false)
                    {
                        emp.EMP_PASSWORD = PasswordHelper.MD5Hash(newPass);
                    }
                    else
                    {
                        return (false, "Không tồn tại email này", null);
                    }
                    await context.SaveChangesAsync();
                    return (true, "Update mật khẩu thành công", emp.EMP_USERNAME);
                }
            }
            catch 
            {
                return (false, "Xảy ra lỗi", null);
            }
        }

        // tìm theo email
        //public async Task<(EMPLOYEE, bool, string)> findEmpbyEmail(string email)
        //{
        //    try
        //    {
        //        using (var context = new CoffeeShopDBEntities())
        //        {
        //            var emp = await context.EMPLOYEEs.Where(p => p.EMP_EMAIL == email).FirstOrDefaultAsync();
        //            if (emp == null)
        //            {
        //                return (null, false, "Không tìm thấy nhân viên này");
        //            }
        //            else
        //            {
        //                return (emp, true, "Tìm thấy nhân viên");
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return (null, false, "Xảy ra lỗi");
        //    }
        //}
        //// tìm theo sđt
        //public async Task<(EMPLOYEE, bool, string)> findEmpbyPhone(string phone)
        //{
        //    try
        //    {
        //        using (var context = new CoffeeShopDBEntities())
        //        {
        //            var emp = await context.EMPLOYEEs.Where(p => p.EMP_PHONE == phone).FirstOrDefaultAsync();
        //            if (emp == null)
        //            {
        //                return (null, false, "Không tìm thấy nhân viên này");
        //            }
        //            else
        //            {
        //                return (emp, true, "Tìm thấy nhân viên");
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return (null, false, "Xảy ra lỗi");
        //    }

        //}

    }
}
