using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
