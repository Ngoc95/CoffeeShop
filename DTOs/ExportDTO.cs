using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class ExportDTO
    {
        public int ExpId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public Nullable<System.DateTime> ExpDate { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public string EmpName { get; set; }
        public int Quantity { get; set; }
    }
}