using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class EmployeeShiftDTO
    {
        public int EMP_ID { get; set; }
        public int SHIFT_ID { get; set; }
        public Nullable<System.DateTime> WORK_DATE { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }

        public virtual EMPLOYEE EMPLOYEE { get; set; }
        public virtual WORK_SHIFT WORK_SHIFT { get; set; }
    }
}
