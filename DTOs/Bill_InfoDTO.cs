using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    internal class Bill_InfoDTO
    {
        public int BILL_ID { get; set; }
        public int PRO_ID { get; set; }
        public Nullable<int> QUANTITY { get; set; }
        public Nullable<decimal> PRICE_ITEM { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
    }
}
