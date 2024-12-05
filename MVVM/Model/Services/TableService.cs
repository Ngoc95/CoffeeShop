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

        public async Task<List<TableDTO>> FilterTableList(int GenreID, string Status)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    if(GenreID != 0 && Status != null)
                    {
                        var tableList = (from c in context.C_TABLE
                                         where (c.IS_DELETED == false && c.TB_STATUS == Status && c.GT_ID == GenreID)
                                         select new TableDTO
                                         {
                                             TB_ID = c.TB_ID,
                                             TB_STATUS = c.TB_STATUS,
                                             GT_ID = c.GENRE_TABLE.GT_ID,
                                             IS_DELETED = c.IS_DELETED,
                                         }).ToListAsync();
                        return await tableList;
                    }
                    else if(GenreID != 0)
                    {
                        var tableList = (from c in context.C_TABLE
                                         where (c.GT_ID == GenreID && c.IS_DELETED == false)
                                         select new TableDTO
                                         {
                                             TB_ID = c.TB_ID,
                                             TB_STATUS = c.TB_STATUS,
                                             GT_ID = c.GENRE_TABLE.GT_ID,
                                             IS_DELETED = c.IS_DELETED,
                                         }).ToListAsync();
                        return await tableList;
                    }
                    else if (Status != null)
                    {
                        var tableList = (from c in context.C_TABLE
                                         where (c.TB_STATUS == Status && c.IS_DELETED == false)
                                         select new TableDTO
                                         {
                                             TB_ID = c.TB_ID,
                                             TB_STATUS = c.TB_STATUS,
                                             GT_ID = c.GENRE_TABLE.GT_ID,
                                             IS_DELETED = c.IS_DELETED,
                                         }).ToListAsync();
                        return await tableList;
                    }
                    return null;
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
