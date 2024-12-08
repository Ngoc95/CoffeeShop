﻿using QuanLiCoffeeShop.DTOs;
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
                                               RES_DATE = c.RES_DATE,
                                               RES_TIME = c.RES_TIME,
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
    }
}
