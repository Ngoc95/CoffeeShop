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
    public class CustomerService
    {
        public CustomerService() { }
        private static CustomerService _ins;

        public static CustomerService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new CustomerService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<List<CustomerDTO>> GetAllCus()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var cusList = (from s in context.CUSTOMERs
                                   where s.IS_DELETED == false
                                   select new CustomerDTO
                                   {
                                       ID = s.CUS_ID,
                                       Name = s.CUS_NAME,
                                       Email = s.CUS_EMAIL,
                                       Phone = s.CUS_PHONE,
                                       Gender = s.CUS_GENDER,
                                       Point = s.CUS_POINT,
                                       IsDeleted = s.IS_DELETED,
                                   }).ToListAsync();
                    return await cusList;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }

        }

        public async Task<(bool, string)> AddNewCus(CUSTOMER newCus)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    if (string.IsNullOrEmpty(newCus.CUS_NAME) || string.IsNullOrEmpty(newCus.CUS_EMAIL) || string.IsNullOrEmpty(newCus.CUS_PHONE) || string.IsNullOrEmpty(newCus.CUS_GENDER))
                        return (false, "Bạn nhập thiếu thông tin");
                    bool IsEmailExist = await context.CUSTOMERs.AnyAsync(p => p.CUS_EMAIL == newCus.CUS_EMAIL);
                    bool IsPhoneExist = await context.CUSTOMERs.AnyAsync(p => p.CUS_PHONE == newCus.CUS_PHONE);

                    var cus = await context.CUSTOMERs.Where(p => p.CUS_EMAIL == newCus.CUS_EMAIL || p.CUS_PHONE == newCus.CUS_PHONE).FirstOrDefaultAsync();
                    if (cus != null)
                    {
                        if (cus.IS_DELETED == true)
                        {
                            cus.CUS_NAME = newCus.CUS_NAME;
                            cus.CUS_GENDER = newCus.CUS_GENDER;
                            cus.CUS_PHONE = newCus.CUS_PHONE;
                            cus.CUS_EMAIL = newCus.CUS_EMAIL;
                            cus.CUS_POINT = newCus.CUS_POINT;
                            cus.IS_DELETED = false;
                            await context.SaveChangesAsync();
                            return (true, "Khôi phục tài khoản thành công");

                        }
                        else
                        {
                            if (IsEmailExist)
                            {
                                return (false, "Đã tồn tại email này");
                            }
                            if (IsPhoneExist)
                            {
                                return (false, "Đã tồn tại số điện thoại này");
                            }
                        }
                    }
                    context.CUSTOMERs.Add(newCus);
                    await context.SaveChangesAsync();
                    return (true, "Đăng kí khách hàng thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi");
            }
        }

        public async Task<(bool, string)> EditCusList(CUSTOMER newCus, int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var cus = await context.CUSTOMERs.Where(p => p.CUS_ID == ID).FirstOrDefaultAsync();
                    if (cus == null) return (false, "Không tìm thấy ID");
                    cus.CUS_NAME = newCus.CUS_NAME;
                    cus.CUS_GENDER = newCus.CUS_GENDER;
                    cus.CUS_PHONE = newCus.CUS_PHONE;
                    cus.CUS_EMAIL = newCus.CUS_EMAIL;
                    cus.CUS_POINT = newCus.CUS_POINT;
                    await context.SaveChangesAsync();
                    return (true, "Chỉnh sửa thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi chỉnh sửa khách hàng");
            }
        }
        public async Task<(bool, string)> DeleteCustomer(int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var cus = await context.CUSTOMERs.Where(p => p.CUS_ID == ID).FirstOrDefaultAsync();
                    if (cus.IS_DELETED == true) return (false, "Đã xóa khách hàng này rồi");
                    cus.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch
            {
                return (false, "Xảy ra lỗi khi xóa khách hàng");
            }

        }
        // tìm theo email
        public async Task<(CUSTOMER, bool, string)> findCusbyEmail(string email)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var cus = await context.CUSTOMERs.Where(p => p.CUS_EMAIL == email).FirstOrDefaultAsync();
                    if (cus == null)
                    {
                        return (null, false, "Không tìm thấy khách hàng này");
                    }
                    else
                    {
                        return (cus, true, "Tìm thấy khách hàng");
                    }
                }
            }
            catch
            {
                return (null, false, "Xảy ra lỗi");
            }
        }
        // tìm theo sđt
        public async Task<(CUSTOMER, bool, string)> findCusbyPhone(string phone)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var cus = await context.CUSTOMERs.Where(p => p.CUS_PHONE == phone).FirstOrDefaultAsync();
                    if (cus == null)
                    {
                        return (null, false, "Không tìm thấy khách hàng này");
                    }
                    else
                    {
                        return (cus, true, "Tìm thấy khách hàng");
                    }
                }
            }
            catch
            {
                return (null, false, "Xảy ra lỗi");
            }

        }

        public async Task<CustomerDTO> FindCustomerbyID(int ID)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var cus = await context.CUSTOMERs.Where(p => p.CUS_ID == ID).FirstOrDefaultAsync();
                    CustomerDTO customer = new CustomerDTO()
                    {
                        ID = cus.CUS_ID,
                        Name = cus.CUS_NAME,
                        Email = cus.CUS_EMAIL,
                        Phone = cus.CUS_PHONE,
                    };
                    return customer;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
