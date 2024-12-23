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
    public class ExportInfoService
    {
        public ExportInfoService() { }
        private static ExportInfoService _ins;

        public static ExportInfoService Ins
        {
            get
            {
                if (_ins == null) _ins = new ExportInfoService();
                return _ins;
            }
            set { _ins = value; }
        }

        public async Task<List<ExportInfoDTO>> GetAllExportInfos()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var exportinfoList = (from e in context.EXPORT_INFO
                                          where e.IS_DELETED == false
                                          select new ExportInfoDTO
                                          {
                                              ExpId = e.EXP_ID,
                                              IngId = e.ING_ID,
                                              Quantity = e.QUANTITY,
                                              IngName = e.INGREDIENT.ING_NAME,
                                              INGREDIENT = e.INGREDIENT,
                                              PriceItem = e.PRICE_ITEM,
                                              IsDeleted = e.IS_DELETED
                                          }).ToListAsync();
                    return await exportinfoList;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }

        public async Task<List<ExportInfoDTO>> GetExportInfoByExportID(int exportID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var list = (from e in context.EXPORT_INFO
                                where e.IS_DELETED == false && e.EXP_ID == exportID
                                select new ExportInfoDTO
                                {
                                    ExpId = e.EXP_ID,
                                    IngId = e.ING_ID,
                                    Quantity = e.QUANTITY,
                                    IngName = e.INGREDIENT.ING_NAME,
                                    INGREDIENT = e.INGREDIENT,
                                    PriceItem = e.PRICE_ITEM,
                                    IsDeleted = e.IS_DELETED
                                }).ToListAsync();
                    return await list;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }

        public async Task<bool> AddNewExportInfo(ExportInfoDTO export)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var newExport = new EXPORT_INFO
                    {
                        EXP_ID = export.ExpId,
                        ING_ID = export.IngId,
                        QUANTITY = export.Quantity,
                        PRICE_ITEM = export.PriceItem,
                        INGREDIENT = export.INGREDIENT,
                        IS_DELETED = false
                    };
                    context.EXPORT_INFO.Add(newExport);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.InnerException.Message);
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return false;
            }
        }

        public async Task<bool> DeleteExportInfo(int id)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var exportinfo = await context.EXPORT_INFO.FindAsync(id);
                    exportinfo.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return false;
            }
        }

        public async Task<bool> UpdateExportInfo(ExportInfoDTO export)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var exportinfo = await context.EXPORT_INFO.FindAsync(export.ExpId, export.IngId);
                    exportinfo.QUANTITY = export.Quantity;
                    exportinfo.PRICE_ITEM = export.PriceItem;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return false;
            }
        }

    }
}
