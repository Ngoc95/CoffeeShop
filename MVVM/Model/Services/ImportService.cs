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
    public class ImportService
    {
        public ImportService() { }

        private static ImportService _ins;

        public static ImportService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ImportService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<List<ImportDTO>> GetAllImports()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var importList = (from i in context.IMPORTs
                                      where i.IS_DELETED == false
                                      select new ImportDTO
                                      {
                                          ImpId = i.IMP_ID,
                                          SupId = i.SUP_ID,
                                          EmpId = i.EMP_ID,
                                          SupplierName = i.SUPPLIER.SUP_NAME,
                                          EmployeeName = i.EMPLOYEE.EMP_NAME,
                                          Quantity = context.IMPORT_INFO.Where(x => x.IMP_ID == i.IMP_ID).Count(),
                                          ImpDate = i.IMP_DATE,
                                          TotalCost = i.TOTAL_COST,
                                          IsDeleted = i.IS_DELETED,
                                      }).ToListAsync();
                    return await importList;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }

        public async Task<(bool, string)> AddNewImport(ImportDTO Import)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var newImport = new IMPORT
                    {
                        SUP_ID = Import.SupId,
                        EMP_ID = Import.EmpId,
                        IMP_DATE = Import.ImpDate,
                        TOTAL_COST = Import.TotalCost,
                        IS_DELETED = false,
                    };
                    context.IMPORTs.Add(newImport);
                    await context.SaveChangesAsync();
                    return (true, "Thêm thành công");
                }
            }
            catch
            {
                return (false, "Thêm thất bại");
            }
        }

        public async Task<(bool, string)> EditImport(ImportDTO Import, int id)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var importEdit = await context.IMPORTs.Where(i => i.IMP_ID == id).FirstOrDefaultAsync();
                    var supid = await context.SUPPLIERs.Where(s => s.SUP_NAME == Import.SupplierName).Select(s => s.SUP_ID).FirstOrDefaultAsync();
                    var empid = await context.EMPLOYEEs.Where(e => e.EMP_NAME == Import.EmployeeName).Select(e => e.EMP_ID).FirstOrDefaultAsync();
                    if (importEdit == null)
                    {
                        return (false, "Không tìm thấy phiếu nhập");
                    }
                    importEdit.SUP_ID = supid;
                    importEdit.EMP_ID = empid;
                    await context.SaveChangesAsync();
                    return (true, "Sửa thành công");
                }
            }
            catch
            {
                return (false, "Sửa thất bại");
            }
        }

        public async Task<(bool, string)> DeleteImport(int id)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var import = await context.IMPORTs.FindAsync(id);
                    if (import == null)
                    {
                        return (false, "Không tìm thấy phiếu nhập");
                    }
                    var importInfoList = context.IMPORT_INFO.Where(x => x.IMP_ID == id).ToList();
                    foreach (var importInfo in importInfoList)
                    {
                        importInfo.IS_DELETED = true;
                    }
                    import.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch
            {
                return (false, "Xóa thất bại");
            }
        }


    }
}

