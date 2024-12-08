using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    internal class ReservationDTO
    {
        public int RES_ID { get; set; }
        public int CUS_ID { get; set; }
        public int TABLE_ID { get; set; }
        public System.DateTime RES_DATE { get; set; }
        public System.DateTime RES_TIME { get; set; }
        public int NUM_OF_PEOPLE { get; set; }
        public string RES_STATUS { get; set; }
        public string SPECIAL_REQUEST { get; set; }
        public Nullable<System.DateTime> CREATE_AT { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
    }
}
