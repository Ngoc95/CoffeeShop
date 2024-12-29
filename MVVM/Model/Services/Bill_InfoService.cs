using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shell;

namespace QuanLiCoffeeShop.MVVM.Model.Services
{
    internal class Bill_InfoService
    {
        public Bill_InfoService() { }
        private static Bill_InfoService _ins;

        public static Bill_InfoService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new Bill_InfoService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<bool> AddBillInfor(ObservableCollection<Bill_InfoDTO> newBillInforList)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    foreach (var item in newBillInforList)
                    {
                        BILL_INFO bill = new BILL_INFO()
                        {
                            BILL_ID = item.BILL_ID,
                            PRO_ID = item.PRO_ID,
                            QUANTITY = item.QUANTITY,
                            PRICE_ITEM = item.PRICE_ITEM,
                            IS_DELETED = false,
                        };
                        if (bill == null)
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi khi thêm vào Database");
                            return false;
                        }
                        context.BILL_INFO.Add(bill);
                    }
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi khi truy cập database");
                return false;
            }
        }

        public List<Tuple<string, int>> GetTopSeller()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    List<Tuple<string, int>> list;
                    var templist = context.BILL_INFO
                        .GroupBy(b => b.PRO_ID)
                        .Select(g => new
                        {
                            ProductId = g.Key,
                            TotalQuantity = g.Sum(binfo => binfo.QUANTITY) ?? 0
                        })
                        .OrderByDescending(x => x.TotalQuantity)
                        .Take(5)
                        .Join(context.PRODUCTs,
                              bi => bi.ProductId,
                              p => p.PRO_ID,
                              (bi, p) => new
                              {
                                  p.PRO_NAME, 
                                  bi.TotalQuantity 
                              })
                        .ToList();
                    list = templist
                            .Select(x => new Tuple<string, int>(x.PRO_NAME, x.TotalQuantity))
                            .ToList();
                    return list;
                }
            }
            catch (Exception ex) 
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, ex.Message);
                return null;
            }
        }
    }
}
