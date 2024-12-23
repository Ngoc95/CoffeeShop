using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiCoffeeShop.MVVM.Model.Services
{
    public class ExportService
    {
        public ExportService() { }
        private static ExportService _in;

        public static ExportService Ins
        {
            get
            {
                if (_in == null) _in = new ExportService();
                return _in;
            }
            set { _in = value; }
        }

        public async Task<List<ExportDTO>> GetAllExports()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var exportlist = (from e in context.EXPORTs
                                      where e.IS_DELETED == false
                                      select new ExportDTO
                                      {
                                          ExpId = e.EXP_ID,
                                          EmpId = e.EMP_ID,
                                          ExpDate = e.EXP_DATE,
                                          TotalCost = e.TOTAL_COST,
                                          EmpName = e.EMPLOYEE.EMP_NAME,
                                          Quantity = context.EXPORT_INFO.Where(x => x.EXP_ID == e.EXP_ID).Count(),
                                          IsDeleted = e.IS_DELETED
                                      }).ToListAsync();
                    return await exportlist;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }

        public async Task<(bool, string)> AddExport(ExportDTO export)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var newExport = new EXPORT
                    {
                        EMP_ID = export.EmpId,
                        EXP_DATE = export.ExpDate,
                        TOTAL_COST = export.TotalCost,
                        IS_DELETED = false
                    };
                    context.EXPORTs.Add(newExport);
                    await context.SaveChangesAsync();
                    return (true, "Thêm thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return (false, "Thêm thất bại");
            }
        }

        public async Task<(bool, string)> DeleteExport(int id)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var export = await context.EXPORTs.FindAsync(id);
                    if (export == null)
                    {
                        return (false, "Không tìm thấy phiếu nhập");
                    }
                    var exportinfoList = context.EXPORT_INFO.Where(e => e.EXP_ID == id).ToList();
                    foreach (var exportinfo in exportinfoList)
                    {
                        exportinfo.IS_DELETED = true;
                    }
                    export.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return (true, "Xoá thành công");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return (false, "Xóa thất bại");
            }
        }

        public async Task<(bool, string)> EditExport(ExportDTO export, int id)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var exportEdit = await context.EXPORTs.Where(e => e.EXP_ID == id).FirstOrDefaultAsync();
                    var empid = await context.EMPLOYEEs.Where(e => e.EMP_NAME == export.EmpName).Select(e => e.EMP_ID).FirstOrDefaultAsync();
                    if (exportEdit == null)
                    {
                        return (false, "Không tìm thấy phiếu nhập");
                    }
                    exportEdit.EMP_ID = empid;
                    await context.SaveChangesAsync();
                    return (true, "Sửa thành công");
                }
            }
            catch
            {
                return (false, "Sửa thất bại");
            }
        }

    }
}