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
    internal class TableService
    {
        public TableService() { }
        private static TableService _ins;

        public static TableService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new TableService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }
        public async Task<List<TableDTO>> GetAllTable()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var tableList = (from c in context.C_TABLE
                                       where c.IS_DELETED == false
                                       select new TableDTO
                                       {
                                           TB_ID = c.TB_ID,
                                           TB_STATUS = c.TB_STATUS,
                                           GT_ID = c.GENRE_TABLE.GT_ID,
                                           IS_DELETED = c.IS_DELETED,
                                       }).ToListAsync();
                    return await tableList;
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
