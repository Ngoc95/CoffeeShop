using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    internal class BillDTO
    {
        public int BILL_ID { get; set; }
        public Nullable<int> CUS_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<decimal> SUBTOTAL { get; set; }
        public Nullable<decimal> DISCOUNT { get; set; }
        public Nullable<decimal> TOTAL_COST { get; set; }
        public Nullable<System.DateTime> CREATE_AT { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
        public virtual EMPLOYEE EMPLOYEE { get; set; }
    }
}
