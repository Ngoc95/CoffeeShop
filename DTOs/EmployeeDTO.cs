using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class EmployeeDTO
    {
        public int EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_PHONE { get; set; }
        public string EMP_CCCD { get; set; }
        public Nullable<System.DateTime> EMP_STARTDATE { get; set; }
        public Nullable<System.DateTime> EMP_BIRTHDAY { get; set; }
        public string EMP_USERNAME { get; set; }
        public string EMP_PASSWORD { get; set; }
        public string EMP_EMAIL { get; set; }
        public string EMP_GENDER { get; set; }
        public string EMP_STATUS { get; set; }
        public Nullable<decimal> EMP_SALARY { get; set; }
        public string EMP_ROLE { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
        public virtual ICollection<BILL> BILLs { get; set; }
        public virtual ICollection<EMPLOYEE_SHIFT> EMPLOYEE_SHIFT { get; set; }

        public int EMP_TotalShifts { get; set; }
    }
}
