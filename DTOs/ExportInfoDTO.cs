using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class ExportInfoDTO
    {
        public int ExpId { get; set; }
        public int IngId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> PriceItem { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public string IngName { get; set; }
        public virtual INGREDIENT INGREDIENT { get; set; }
    }
}