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
    public class ErrorService
    {
        public ErrorService() { }
        private static ErrorService _ins;

        public static ErrorService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ErrorService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<List<ErrorDTO>> GetAllError()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var erList = (from s in context.ERRORs
                                   where s.IS_DELETED == false
                                   select new ErrorDTO
                                   {
                                       ER_ID = s.ER_ID,
                                       ER_NAME = s.ER_NAME,
                                       ER_STATUS = s.ER_STATUS,
                                       ER_DESCRIPTION = s.ER_DESCRIPTION,
                                       IS_DELETED = s.IS_DELETED,
                                   }).ToListAsync();
                    return await erList;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }

        }

        public async Task<(bool, string)> AddNewError(ERROR newError)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    context.ERRORs.Add(newError);
                    await context.SaveChangesAsync();
                    return (true, "Báo cáo sự cố thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi");
            }
        }

        public async Task<(bool, string)> EditErrorList(ERROR newError, int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var error = await context.ERRORs.Where(p => p.ER_ID == ID).FirstOrDefaultAsync();
                    if (error == null) return (false, "Không tìm thấy ID");
                    error.ER_NAME = newError.ER_NAME;
                    error.ER_STATUS = newError.ER_STATUS;
                    error.ER_DESCRIPTION = newError.ER_DESCRIPTION;
                    await context.SaveChangesAsync();
                    return (true, "Chỉnh sửa thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi chỉnh sửa sự cố");
            }
        }
        public async Task<(bool, string)> DeleteError(int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var error = await context.ERRORs.Where(p => p.ER_ID == ID).FirstOrDefaultAsync();
                    if (error.IS_DELETED == true) return (false, "Đã xóa sự cố này rồi");
                    error.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi xóa sự cố");
            }

        }

        public async Task<int> NumOfUnSolve()
        {
            try
            {
                using (var c = new CoffeeShopDBEntities())
                {
                    return await c.ERRORs.CountAsync(e => e.ER_STATUS == "Chưa khắc phục");
                }
            }
            catch
            {
                return 0;
            }
        }

    }
}
