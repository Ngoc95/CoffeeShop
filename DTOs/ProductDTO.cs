using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    internal class ProductDTO
    {
        public int PRO_ID { get; set; }
        public string PRO_NAME { get; set; }
        public Nullable<int> GP_ID { get; set; }
        public string PRO_IMG { get; set; }
        public string PRO_DESCRIPTION { get; set; }
        public Nullable<decimal> PRO_PRICE { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }

        //public GenreProductDTO GenreProduct {  get; set; }
        //public string GENRE_NAME {  get; set; }    
        //public int GENRE_ID {  get; set; }     

    }
}
