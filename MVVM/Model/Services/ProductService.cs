﻿using QuanLiCoffeeShop.DTOs;
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
                                           GP_ID = c.GP_ID,
                                           PRO_DESCRIPTION = c.PRO_DESCRIPTION,
                                           PRO_IMG = c.PRO_IMG == null ? "pack://application:,,,/Images/MenuAndError/UploadImg.jpg" : c.PRO_IMG,
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

        public async Task<(bool, string)> EditPrdList(ProductDTO newPrd, int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var Prd = await context.PRODUCTs.Where(p => p.PRO_ID == ID).FirstOrDefaultAsync();
                    if (Prd == null) return (false, "Không tìm thấy ID");
                    Prd.PRO_NAME = newPrd.PRO_NAME;
                    Prd.PRO_PRICE = newPrd.PRO_PRICE;
                    Prd.GP_ID = newPrd.GP_ID;
                    Prd.PRO_IMG = newPrd.PRO_IMG;
                    Prd.PRO_DESCRIPTION = newPrd.PRO_DESCRIPTION;
                    Prd.IS_DELETED = newPrd.IS_DELETED;
                    await context.SaveChangesAsync();
                    return (true, "Chỉnh sửa thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi chỉnh sửa sản phẩm");
            }
        }
    }
}
