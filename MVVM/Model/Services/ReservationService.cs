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
    internal class ReservationService
    {
        public ReservationService() { }
        private static ReservationService _ins;

        public static ReservationService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new ReservationService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }
        public async Task<List<ReservationDTO>> GetAllReservation()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var reservationList = (from c in context.RESERVATIONs
                                           where c.IS_DELETED == false
                                           select new ReservationDTO
                                           {
                                               RES_ID = c.RES_ID,
                                               CUS_ID = c.CUS_ID,
                                               TABLE_ID = c.TABLE_ID,
                                               RES_DATETIME = c.RES_DATETIME,
                                               NUM_OF_PEOPLE = c.NUM_OF_PEOPLE,
                                               RES_STATUS = c.RES_STATUS,
                                               SPECIAL_REQUEST = c.SPECIAL_REQUEST,
                                               CREATE_AT = c.CREATE_AT,
                                               IS_DELETED = c.IS_DELETED,
                                           }).ToListAsync();
                    return await reservationList;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }

        public async Task<bool> UpdateReservation(ReservationDTO reservation)
        {
            try
            {
                if(reservation.RES_STATUS == "Khách chưa nhận bàn")
                {
                    if (reservation.RES_DATETIME.Date < DateTime.Now.Date)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error,"Ngày đặt bàn không phù hợp");
                        return false;
                    }
                    if (reservation.RES_DATETIME.Date == DateTime.Now.Date && reservation.RES_DATETIME.TimeOfDay < DateTime.Now.TimeOfDay)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày đặt bàn không phù hợp");
                        return false;
                    }

                    if (reservation.RES_DATETIME.TimeOfDay < new TimeSpan(7, 0, 0) || reservation.RES_DATETIME.TimeOfDay > new TimeSpan(20,0,0))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Giờ đặt bàn từ 7h đến 20h");
                        return false;
                    }
                }

                using (var context = new CoffeeShopDBEntities())
                {
                    var res = await context.RESERVATIONs.FirstOrDefaultAsync(c => c.IS_DELETED == false && c.RES_ID == reservation.RES_ID); 
                    if (res != null)
                    {
                        res.CUS_ID = reservation.RES_ID;
                        res.RES_STATUS = reservation.RES_STATUS;
                        res.TABLE_ID = reservation.TABLE_ID;
                        res.RES_DATETIME = reservation.RES_DATETIME;
                        res.NUM_OF_PEOPLE = reservation.NUM_OF_PEOPLE;
                        res.SPECIAL_REQUEST = reservation.SPECIAL_REQUEST;
                        await context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                        return false;
                    }

                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return false;
            }
        }

        public int GetNextResID()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    return context.RESERVATIONs.Count() + 1;
                }
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> AddNewReservation(ReservationDTO reservation)
        {
            try
            {
                if (!(await TableService.Ins.TableID_Exsist(reservation.TABLE_ID)))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Mã bàn không chính xác");
                    return false;
                }
                if (reservation.CUS_ID == 0 || reservation.TABLE_ID == 0 || reservation.NUM_OF_PEOPLE <= 0)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Thông tin nhập vào chưa chính xác");
                    return false;
                }
                if (reservation.RES_DATETIME.Date < DateTime.Now.Date)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày đặt bàn không phù hợp");
                    return false;
                }
                if (reservation.RES_DATETIME.Date == DateTime.Now.Date && reservation.RES_DATETIME.TimeOfDay < DateTime.Now.TimeOfDay)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày đặt bàn không phù hợp");
                    return false;
                }

                if (reservation.RES_DATETIME.TimeOfDay < new TimeSpan(7, 0, 0) || reservation.RES_DATETIME.TimeOfDay > new TimeSpan(20, 0, 0))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Giờ đặt bàn từ 7h đến 20h");
                    return false;
                }
                using (var context = new CoffeeShopDBEntities())
                {
                    RESERVATION newRes = new RESERVATION()
                    {
                        CUS_ID = reservation.CUS_ID,
                        RES_STATUS = "Khách chưa nhận bàn",
                        RES_DATETIME = reservation.RES_DATETIME,
                        TABLE_ID = reservation.TABLE_ID,
                        NUM_OF_PEOPLE = reservation.NUM_OF_PEOPLE,
                        SPECIAL_REQUEST = reservation.SPECIAL_REQUEST,
                        IS_DELETED = false,
                        CREATE_AT = DateTime.Now,
                    };

                    context.RESERVATIONs.Add(newRes);
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
