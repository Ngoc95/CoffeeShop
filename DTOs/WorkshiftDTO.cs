using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class WorkshiftDTO
    {
        public int SHIFT_ID { get; set; }
        public string SHIFT_NAME { get; set; }
        public Nullable<System.DateTime> START_TIME { get; set; }
        public Nullable<System.DateTime> END_TIME { get; set; }
        public Nullable<decimal> WAGE { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
        public virtual ICollection<EMPLOYEE_SHIFT> EMPLOYEE_SHIFT { get; set; }

    }
}
