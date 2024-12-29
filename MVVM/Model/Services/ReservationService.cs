using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
                                               RES_DATE  = c.RES_DATE,
                                               RES_TIME  = c.RES_TIME,
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
                using (var context = new CoffeeShopDBEntities())
                {
                    var res = await context.RESERVATIONs.FirstOrDefaultAsync(c => c.IS_DELETED == false && c.RES_ID == reservation.RES_ID); 
                    if (res != null)
                    {
                        res.RES_STATUS = reservation.RES_STATUS;
                        res.TABLE_ID = reservation.TABLE_ID;
                        res.RES_TIME = reservation.RES_TIME;
                        res.RES_DATE = reservation.RES_DATE;
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
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi khi truy cập database");
                return false;
            }
        }

        public async Task<bool> DeleteAReservation(ReservationDTO reservation)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var res = await context.RESERVATIONs.FirstOrDefaultAsync(c => c.RES_ID == reservation.RES_ID);
                    if (res != null)
                    {
                        res.IS_DELETED = true;
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
                MessageBoxCustom.Show(MessageBoxCustom.Error, $"Xảy ra lỗi khi truy cập DataBase");
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
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi khi truy cập DataBase");
                return 0;
            }
        }

        public async Task<(int, int)> GetResStatusToday()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    int pendingReservations = await context.RESERVATIONs.CountAsync(r =>
                        DbFunctions.TruncateTime(r.RES_DATE) == DateTime.Today && r.RES_STATUS == "Khách chưa nhận bàn");

                    int confirmedReservations = await context.RESERVATIONs.CountAsync(r =>
                        DbFunctions.TruncateTime(r.RES_DATE) == DateTime.Today && r.RES_STATUS == "Khách đã nhận bàn");
                    return (pendingReservations, confirmedReservations);
                }
            }
            catch
            {
                return (0,0);
            }
        }

        public async Task<bool> AddNewReservation(ReservationDTO reservation)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    RESERVATION newRes = new RESERVATION()
                    {
                        CUS_ID = reservation.CUS_ID,
                        RES_STATUS = "Khách chưa nhận bàn",
                        RES_DATE = reservation.RES_DATE,
                        RES_TIME = reservation.RES_TIME,
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
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                throw;
            }
        }

        internal async Task<string> ReservationGenaral()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    int n = await context.RESERVATIONs.CountAsync(t => t.IS_DELETED == false && t.RES_STATUS == "Khách chưa nhận bàn");
                    return n.ToString() + " (chưa nhận bàn)";
                }
            }
            catch
            {
                return "0";
            }
        }

        internal async Task<List<List<String>>> GetGenralResList()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    List<List<string>> res = new List<List<string>>();
                    var temp = await context.RESERVATIONs
                                    .Where(g => DbFunctions.TruncateTime(g.RES_DATE) == DateTime.Today && g.IS_DELETED == false)
                                    .ToListAsync();
                    foreach(var item in temp)
                    {
                        res.Add(new List<string>()
                        {
                            $"Mã bàn: {item.TABLE_ID:000}, " + $"Thời gian: {item.RES_TIME.ToString("HH:mm")}",
                            item.RES_STATUS + ((item.SPECIAL_REQUEST != null || item.SPECIAL_REQUEST.Length != 0 ) ? "\nCó yêu cầu đặc biệt" : ""),
                        });
                    }
                    return res;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
