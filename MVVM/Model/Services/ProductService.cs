using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Admin;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.MVVM.Model.Services
{
    internal class ProductService
    {
        public ProductService() { }
        private static ProductService _ins;

        public static ProductService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ProductService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }
        //Get all product
        public async Task<List<ProductDTO>> GetAllProduct()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var productList = (from c in context.PRODUCTs
                                       where c.IS_DELETED == false
                                       select new ProductDTO
                                       {
                                           PRO_ID = c.PRO_ID,
                                           PRO_NAME = c.PRO_NAME,
                                           PRO_PRICE = c.PRO_PRICE,
                                           GENRE_ID = (int)c.GP_ID,
                                           GENRE_NAME = c.GENRE_PRODUCT.GP_NAME,
                                           PRO_DESCRIPTION = c.PRO_DESCRIPTION,
                                           PRO_IMG = c.PRO_IMG == null ? "../../../Images/MenuAndError/UploadImg.jpg" : c.PRO_IMG,
                                           IS_DELETED = c.IS_DELETED,
                                       }).ToListAsync();
                    return await productList;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }
    }
}
