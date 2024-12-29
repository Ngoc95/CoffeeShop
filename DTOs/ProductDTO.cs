using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class ProductDTO
    {
        public int PRO_ID { get; set; }
        public string PRO_NAME { get; set; }
        public Nullable<int> GP_ID { get; set; }
        public string PRO_IMG { get; set; }
        public string PRO_DESCRIPTION { get; set; }
        public Nullable<decimal> PRO_PRICE { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
        public int QUANTITY { get; set; }

        public ProductDTO() { }
        public ProductDTO(ProductDTO temp) 
        {
            PRO_ID = temp.PRO_ID;
            PRO_NAME = temp.PRO_NAME;
            GP_ID = temp.GP_ID;
            PRO_IMG = temp.PRO_IMG;
            PRO_DESCRIPTION = temp.PRO_DESCRIPTION;
            PRO_PRICE = temp.PRO_PRICE;
            IS_DELETED = temp.IS_DELETED;
        }
    }
}
