﻿using QuanLiCoffeeShop.DTOs;
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

    }
}
