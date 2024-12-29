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
    public class SupplierService
    {
        public SupplierService() { }

        private static SupplierService _ins;
        public static SupplierService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new SupplierService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<List<SupplierDTO>> GetAllSuppliers()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var supplierList = (from s in context.SUPPLIERs
                                        where s.IS_DELETED == false
                                        select new SupplierDTO
                                        {
                                            ID = s.SUP_ID,
                                            Name = s.SUP_NAME,
                                            Phone = s.SUP_PHONE,
                                            Address = s.SUP_ADDR,
                                            IsDeleted = s.IS_DELETED,
                                        }).ToListAsync();
                    return await supplierList;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }

        public async Task<(bool, string)> AddNewSupplier(SupplierDTO Supplier)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    // Check if the supplier already exists, even if it's deleted
                    var existingSupplier = await context.SUPPLIERs
                        .FirstOrDefaultAsync(s => s.SUP_NAME == Supplier.Name);

                    if (existingSupplier != null)
                    {
                        // If the supplier is deleted, restore it
                        if (existingSupplier.IS_DELETED == true)
                        {
                            existingSupplier.IS_DELETED = false; // Restore the supplier
                            existingSupplier.SUP_PHONE = Supplier.Phone; // Update phone number if needed
                            existingSupplier.SUP_ADDR = Supplier.Address; // Update address if needed
                            await context.SaveChangesAsync();
                            return (true, "Nhà cung cấp đã được khôi phục");
                        }

                        // If the supplier exists and is not deleted, inform the user
                        return (false, "Nhà cung cấp đã tồn tại");
                    }

                    // If not found, add the new supplier
                    SUPPLIER newSupplier = new SUPPLIER
                    {
                        SUP_NAME = Supplier.Name,
                        SUP_PHONE = Supplier.Phone,
                        SUP_ADDR = Supplier.Address,
                        IS_DELETED = false,
                    };

                    context.SUPPLIERs.Add(newSupplier);
                    await context.SaveChangesAsync();
                    return (true, "Thêm nhà cung cấp thành công");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return (false, "Thêm nhà cung cấp thất bại");
            }
        }


        public async Task<(bool, string)> EditSupplier(SupplierDTO Supplier, int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var supplier = await context.SUPPLIERs.Where(p => p.SUP_ID == ID).FirstOrDefaultAsync();
                    if (supplier == null)
                        return (false, "Nhà cung cấp không tồn tại");
                    supplier.SUP_NAME = Supplier.Name;
                    supplier.SUP_PHONE = Supplier.Phone;
                    supplier.SUP_ADDR = Supplier.Address;
                    await context.SaveChangesAsync();
                    return (true, "Sửa nhà cung cấp thành công");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return (false, "Sửa nhà cung cấp thất bại");
            }
        }

        public async Task<(bool, string)> DeleteSupplier(int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var supplier = await context.SUPPLIERs.Where(p => p.SUP_ID == ID).FirstOrDefaultAsync();
                    if (supplier == null)
                        return (false, "Nhà cung cấp không tồn tại");
                    supplier.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa nhà cung cấp thành công");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return (false, "Xóa nhà cung cấp thất bại");
            }
        }

        internal async Task<string> GetSupplierGeneral()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    int n = await context.SUPPLIERs.CountAsync(t => t.IS_DELETED == false);
                    return n.ToString();
                }
            }
            catch
            {
                return "0";
            }
        }
    }
}
