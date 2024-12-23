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
    public class ImportInfoService
    {
        private ImportInfoService() { }

        private static ImportInfoService _ins;
        public static ImportInfoService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ImportInfoService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<List<ImportInfoDTO>> GetAllImportInfos()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var importInfoList = (from i in context.IMPORT_INFO
                                          where i.IS_DELETED == false
                                          select new ImportInfoDTO
                                          {
                                              ImpId = i.IMP_ID,
                                              IngId = i.ING_ID,
                                              IngName = i.INGREDIENT.ING_NAME,
                                              Quantity = i.QUANTITY,
                                              PriceItem = i.PRICE_ITEM,
                                              INGREDIENT = i.INGREDIENT,
                                              TotalCost = (int)i.QUANTITY * (int)i.PRICE_ITEM,
                                              IsDeleted = i.IS_DELETED
                                          }).ToListAsync();
                    return await importInfoList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }

        public async Task<List<ImportInfoDTO>> GetImportInfosByImportID(int importId)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var importInfoList = (from i in context.IMPORT_INFO
                                          where i.IMP_ID == importId && i.IS_DELETED == false
                                          select new ImportInfoDTO
                                          {
                                              ImpId = i.IMP_ID,
                                              IngId = i.ING_ID,
                                              IngName = i.INGREDIENT.ING_NAME,
                                              Quantity = i.QUANTITY,
                                              PriceItem = i.PRICE_ITEM,
                                              INGREDIENT = i.INGREDIENT,
                                              TotalCost = (int)i.QUANTITY * (int)i.PRICE_ITEM,
                                              IsDeleted = i.IS_DELETED
                                          }).ToListAsync();
                    return await importInfoList;
                }
            }
            catch 
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }

        public async Task<bool> AddImportInfo(ImportInfoDTO importInfo)
        {
            try
            {
                using (var _context = new CoffeeShopDBEntities())
                {
                    var newImportInfo = new IMPORT_INFO
                    {
                        IMP_ID = importInfo.ImpId,
                        ING_ID = importInfo.IngId,
                        QUANTITY = importInfo.Quantity,
                        PRICE_ITEM = importInfo.PriceItem,
                        INGREDIENT = importInfo.INGREDIENT,
                        IS_DELETED = false
                    };
                    _context.IMPORT_INFO.Add(newImportInfo);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteImportInfo(int importInfoId)
        {

            try
            {
                using (var _context = new CoffeeShopDBEntities())
                {
                    var importInfo = _context.IMPORT_INFO.FirstOrDefault(x => x.IMP_ID == importInfoId);
                    if (importInfo == null) return false;

                    importInfo.IS_DELETED = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateImportInfo(ImportInfoDTO importInfo)
        {
            try
            {
                using (var _context = new CoffeeShopDBEntities())
                {
                    var existingImportInfo = _context.IMPORT_INFO.FirstOrDefault(x => x.IMP_ID == importInfo.ImpId);
                    if (existingImportInfo == null) return false;

                    existingImportInfo.IMP_ID = importInfo.ImpId;
                    existingImportInfo.ING_ID = importInfo.IngId;
                    existingImportInfo.QUANTITY = importInfo.Quantity;
                    existingImportInfo.PRICE_ITEM = importInfo.PriceItem;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

    }
}
