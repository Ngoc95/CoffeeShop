using QuanLiCoffeeShop.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static QuanLiCoffeeShop.MVVM.ViewModel.Admin.WorkshiftViewModel;

namespace QuanLiCoffeeShop.MVVM.Model.Services
{
    public class WorkshiftService
    {
        public WorkshiftService() { }
        private static WorkshiftService _ins;

        public static WorkshiftService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new WorkshiftService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }
        public async Task<(bool, string)> AddEmpToShift(List<int> employeeIds, int shiftId, int workDay)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    foreach (var employeeId in employeeIds)
                    {
                        // Kiểm tra nếu nhân viên đã có ca làm việc vào ngày đó
                        var existingRecord = context.EMPLOYEE_SHIFT
                            .FirstOrDefault(es => es.EMP_ID == employeeId && es.SHIFT_ID == shiftId && es.WORK_DAY == workDay);

                        if (existingRecord == null)
                        {
                            var newRecord = new EMPLOYEE_SHIFT
                            {
                                EMP_ID = employeeId,
                                SHIFT_ID = shiftId,
                                WORK_DAY = (byte)workDay,
                                IS_DELETED = false
                            };

                            context.EMPLOYEE_SHIFT.Add(newRecord);
                        }
                    }

                    await context.SaveChangesAsync();
                    return (true, "Thêm nhân viên vào ca làm việc thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi thêm nhân viên vào ca làm việc");
            }
        }
        public async Task<List<EmployeeDTO>> GetEmployeesInShift(int shiftId, int workDay)
        {
            try
            {
                using (var db = new CoffeeShopDBEntities())
                {
                    return await db.EMPLOYEE_SHIFT
                                   .Where(emp => emp.SHIFT_ID == shiftId && emp.WORK_DAY == workDay)
                                   .Select(emp => new EmployeeDTO
                                   {
                                       EMP_ID = emp.EMP_ID,         // ID nhân viên
                                       EMP_NAME = emp.EMPLOYEE.EMP_NAME,
                                       EMP_GENDER = emp.EMPLOYEE.EMP_GENDER,
                                       EMP_CCCD = emp.EMPLOYEE.EMP_CCCD,
                                       EMP_EMAIL = emp.EMPLOYEE.EMP_EMAIL,
                                       EMP_PHONE = emp.EMPLOYEE.EMP_PHONE,
                                       EMP_BIRTHDAY = emp.EMPLOYEE.EMP_BIRTHDAY,
                                       EMP_USERNAME = emp.EMPLOYEE.EMP_USERNAME,
                                       EMP_PASSWORD = emp.EMPLOYEE.EMP_PASSWORD,
                                       EMP_ROLE = emp.EMPLOYEE.EMP_ROLE,
                                       EMP_SALARY = emp.EMPLOYEE.EMP_SALARY,
                                       EMP_STARTDATE = emp.EMPLOYEE.EMP_STARTDATE,
                                       EMP_STATUS = emp.EMPLOYEE.EMP_STATUS,
                                       EMPLOYEE_SHIFT = emp.EMPLOYEE.EMPLOYEE_SHIFT,
                                       IS_DELETED = emp.EMPLOYEE.IS_DELETED,
                                   })
                                   .ToListAsync();
                }
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> RemoveEmpFromShift(int employeeId, int shiftId, int workDay)
        {
            using (var db = new CoffeeShopDBEntities())
            {
                try
                {
                    // Tìm ShiftAssignment tương ứng
                    var assignment = await db.EMPLOYEE_SHIFT
                                             .FirstOrDefaultAsync(sa => sa.EMP_ID == employeeId
                                                                        && sa.SHIFT_ID == shiftId
                                                                        && sa.WORK_DAY == workDay);
                    if (assignment != null)
                    {
                        // Xóa bản ghi
                        db.EMPLOYEE_SHIFT.Remove(assignment);
                        await db.SaveChangesAsync();
                        return true;
                    }
                    return false; // Không tìm thấy bản ghi
                }
                catch
                {
                    return false;
                }
            }
        }



    }
}
