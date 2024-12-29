using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.MVVM.Model.Services
{
    public class ThongKeService
    {
        private ThongKeService() { }
        private static ThongKeService _ins;
        public static ThongKeService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ThongKeService();
                }
                return _ins;
            }
            private set => _ins = value;
        }
        internal async Task<List<ProductDTO>> GetTop10SalerBetween(DateTime from, DateTime to)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var prodStatistic = await context.BILL_INFO
                        .Where(b => b.BILL.CREATE_AT >= from && b.BILL.CREATE_AT <= to && b.IS_DELETED == false)
                        .GroupBy(pBill => pBill.PRO_ID)
                        .Select(gr => new
                        {
                            IDProduct = gr.Key,
                            Revenue = gr.Sum(pBill => (decimal?)pBill.PRICE_ITEM) ?? 0,
                            Quantity = gr.Sum(pBill => (int?)pBill.QUANTITY) ?? 0 // Tính tổng số lượng trực tiếp
                        })
                        .OrderByDescending(m => m.Quantity) // Sắp xếp theo số lượng
                        .Take(10)
                        .Join(
                            context.PRODUCTs,
                            statis => statis.IDProduct,
                            prod => prod.PRO_ID,
                            (statis, prod) => new ProductDTO
                            {
                                PRO_ID = prod.PRO_ID,
                                PRO_NAME = prod.PRO_NAME,
                                PRO_PRICE = statis.Revenue,
                                PRO_DESCRIPTION = prod.PRO_DESCRIPTION,
                                PRO_IMG = prod.PRO_IMG,
                                GP_ID = prod.GP_ID,
                                IS_DELETED = prod.IS_DELETED,
                                QUANTITY = statis.Quantity // Lấy số lượng từ kết quả nhóm
                            }
                        )
                        .ToListAsync();

                    return prodStatistic;
                }
            }
            catch (Exception ex)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, $"Xảy ra lỗi: {ex.Message}");
                return null;
            }
        }
    }
}
