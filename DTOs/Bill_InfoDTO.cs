using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.View.ProductCard;
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
        public string PRO_Name { get; set; }
        public Nullable<decimal> PRICE_ITEM { get; set; }
        public Nullable<decimal> Total_PRICE_ITEM { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
        public BILL BILL { get; set; }
        public PRODUCT PRODUCT { get; set; }
        public Bill_InfoDTO() { }
        public Bill_InfoDTO(ProductDTO product, int NumOfPrd, int IdOfBill)
        {
            BILL_ID = IdOfBill;
            PRO_ID = product.PRO_ID;
            QUANTITY = NumOfPrd;
            PRICE_ITEM = product.PRO_PRICE;
            IS_DELETED = false;

            PRO_Name = product.PRO_NAME;
            Total_PRICE_ITEM = QUANTITY * PRICE_ITEM;
        }
    }
}
