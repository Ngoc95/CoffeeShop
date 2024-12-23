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
    internal class BillService
    {
        public BillService() { }
        private static BillService _ins;

        public static BillService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new BillService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<int> NumOfBill()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    return await Task.Run(() => context.BILLs.Count());
                }
            }
            catch 
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return 0;
            }
        }
        public async Task<(bool, string)> AddBill(BillDTO newBill)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    BILL bill = new BILL()
                    {
                        EMP_ID = newBill.EMP_ID,
                        CUS_ID = newBill.CUS_ID,
                        TOTAL_COST = newBill.TOTAL_COST,
                        EMPLOYEE = newBill.EMPLOYEE,
                        DISCOUNT = newBill.DISCOUNT,
                        SUBTOTAL = newBill.SUBTOTAL,
                        CREATE_AT = newBill.CREATE_AT,
                        IS_DELETED = false,
                    };
                    if (bill == null) return (false, "Thêm thất bại");
                    context.BILLs.Add(bill);
                    await context.SaveChangesAsync();
                    return (true, "Thêm hóa đơn thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi thêm hóa đơn");
            }
        }
        public async Task<List<BillDTO>> GetBillsByCustomerID(int cusID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var bills = await context.BILLs.Where(x => x.CUS_ID == cusID && x.IS_DELETED == false)
                        .Select(x => new BillDTO
                        {
                            BILL_ID = x.BILL_ID,
                            CUS_ID = x.CUS_ID,
                            EMP_ID = x.EMP_ID,
                            EMPLOYEE = x.EMPLOYEE,
                            SUBTOTAL = x.SUBTOTAL,
                            DISCOUNT = x.DISCOUNT,
                            TOTAL_COST = x.TOTAL_COST,
                            CREATE_AT = x.CREATE_AT,
                            IS_DELETED = x.IS_DELETED,
                        }).ToListAsync();
                    return bills;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
