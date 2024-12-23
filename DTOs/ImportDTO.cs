using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class ImportDTO
    {
        public int ImpId { get; set; }
        public Nullable<int> SupId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public Nullable<DateTime> ImpDate { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public Nullable<bool> IsDeleted { get; set; }


        public string SupplierName { get; set; }
        public string EmployeeName { get; set; }
        public int Quantity { get; set; }
    }
}
