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

        // Get All Bill
        public async Task<List<BillDTO>> GetAllBill()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var billList = (from c in context.BILLs
                                    where c.IS_DELETED == false
                                    select new BillDTO
                                    {
                                        BILL_ID = c.BILL_ID,
                                        CUS_ID = c.CUS_ID,
                                        EMP_ID = c.EMP_ID,
                                        CREATE_AT = c.CREATE_AT,
                                        TOTAL_COST = c.TOTAL_COST,
                                        DISCOUNT = c.DISCOUNT,
                                        SUBTOTAL = c.SUBTOTAL,
                                        CUSTOMER = c.CUSTOMER,
                                        EMPLOYEE = c.EMPLOYEE,
                                        BillInfo = (from x in c.BILL_INFO
                                                    where x.IS_DELETED == false
                                                    select new Bill_InfoDTO
                                                    {
                                                        BILL_ID = x.BILL_ID,
                                                        PRICE_ITEM = x.PRICE_ITEM,
                                                        PRO_ID = x.PRO_ID,
                                                        QUANTITY = x.QUANTITY,
                                                        BILL = x.BILL,
                                                        PRODUCT = x.PRODUCT,
                                                    }).ToList(),
                                        IS_DELETED = c.IS_DELETED
                                    }).OrderByDescending(m => m.CREATE_AT).ToListAsync();

                    return await billList;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Lỗi xảy ra!");
                return null;
            }


        }

        //GET BILL BY DATE
        public async Task<int> getBillByDate(DateTime date)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var billTotal = await context.BILLs.Where(p => p.CREATE_AT.Value.Day == date.Day
                                                           && p.CREATE_AT.Value.Month == date.Month
                                                           && p.CREATE_AT.Value.Year == date.Year
                                                           && p.IS_DELETED == false).ToListAsync();
                    int totalPrice = 0;
                    foreach (var bill in billTotal)
                    {
                        totalPrice = totalPrice + (int)bill.TOTAL_COST;
                    }
                    return totalPrice;

                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Lỗi xảy ra!");
                return -1;
            }
        }

        //Delete bill
        public async Task<(bool, string)> DeleteBill(BillDTO Bill)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var bill = await context.BILLs.Where(p => p.BILL_ID == Bill.BILL_ID).FirstOrDefaultAsync();
                    if (bill.IS_DELETED == false) bill.IS_DELETED = true;
                    foreach (var b in Bill.BillInfo)
                    {
                        var billInfo = await context.BILL_INFO.Where(p => p.BILL_ID == b.BILL_ID && p.PRO_ID == b.PRO_ID).FirstOrDefaultAsync();
                        if (billInfo.IS_DELETED == false) billInfo.IS_DELETED = true;
                    }
                    await context.SaveChangesAsync();
                    return (true, "Xoa thanh cong");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Lỗi xảy ra!");
                return (false, null);

            }

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
                        CUSTOMER = newBill.CUSTOMER,
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
                            CUSTOMER = x.CUSTOMER,
                            BillInfo = (from b in x.BILL_INFO
                                        where b.IS_DELETED == false
                                        select new Bill_InfoDTO
                                        {
                                            BILL_ID = b.BILL_ID,
                                            PRICE_ITEM = b.PRICE_ITEM,
                                            PRO_ID = b.PRO_ID,
                                            QUANTITY = b.QUANTITY,
                                            BILL = b.BILL,
                                            PRODUCT = b.PRODUCT,
                                        }).ToList(),
                            TOTAL_COST = x.TOTAL_COST,
                            CREATE_AT = x.CREATE_AT,
                            IS_DELETED = x.IS_DELETED,
                        }).OrderByDescending(m => m.CREATE_AT).ToListAsync();
                    return bills;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<(int vanglai, int dadk)> BillToday()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var today = DateTime.Now.Date;

                    int Vanglai = await context.BILLs.CountAsync(t =>
                        t.CUS_ID == null && DbFunctions.TruncateTime(t.CREATE_AT) == today);

                    int dadk = await context.BILLs.CountAsync(t =>
                        t.CUS_ID != null && DbFunctions.TruncateTime(t.CREATE_AT) == today);

                    return (Vanglai, dadk);
                }
            }
            catch
            {
                return (0, 0);
            }
        }

        internal async Task<List<BillDTO>> GetBillByEmp(int eMP_ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    return await context.BILLs.Where(g => g.EMP_ID == eMP_ID && g.IS_DELETED == false)
                        .Select(g => new BillDTO
                        {
                            BILL_ID = g.BILL_ID,
                            CUS_ID = g.CUS_ID,
                            CREATE_AT = g.CREATE_AT,
                            TOTAL_COST = g.TOTAL_COST,
                            SUBTOTAL = g.SUBTOTAL,
                            DISCOUNT = g.DISCOUNT,
                            IS_DELETED = g.IS_DELETED,
                        })
                        .ToListAsync();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
