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

        public async Task<(bool, string)> DeleteTableList(int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var Prd = await context.C_TABLE.Where(p => p.TB_ID == ID).FirstOrDefaultAsync();
                    Prd.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi chỉnh sửa sản phẩm");
            }
        }

        public async Task<(bool, string)> AddTableList(TableDTO newTable)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    if (newTable.GT_ID == null || newTable.TB_STATUS == null || newTable.TB_STATUS == "")
                    {
                        return (false, "Điền chưa đủ thông tin");
                    }
                    C_TABLE Table = new C_TABLE()
                    {
                        TB_STATUS = newTable.TB_STATUS,
                        GT_ID = newTable.GT_ID,
                        IS_DELETED = false

                    };
                    if (Table == null) return (false, "Thêm thất bại");
                    context.C_TABLE.Add(Table);
                    await context.SaveChangesAsync();
                    return (true, "Thêm sản phẩm thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi thêm sản phẩm");
            }
        }

        public async Task<int> NumOfTable()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    return await Task.Run(() => context.C_TABLE.Count());
                }
            }
            catch (Exception ex)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, $"Xảy ra lỗi: {ex.Message}");
                return 0;
            }
        }

        
        public async Task<bool> UpdateATable(TableDTO temp)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var table = await context.C_TABLE.FirstOrDefaultAsync(c => c.IS_DELETED == false && c.TB_ID == temp.TB_ID);

                    if (table != null)
                    {
                        table.TB_STATUS = temp.TB_STATUS;
                        await context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Không tìm thấy bàn cần cập nhật.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, $"Xảy ra lỗi: {ex.Message}");
                return false;
            }
        }

    }
}
