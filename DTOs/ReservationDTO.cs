using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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

        public ReservationDTO() { }
        public ReservationDTO(ReservationDTO a) 
        {
            CREATE_AT = a.CREATE_AT;
            RES_ID = a.RES_ID;
            RES_DATE = a.RES_DATE;
            RES_TIME = a.RES_TIME;
            CUS_ID = a.CUS_ID;
            TABLE_ID = a.TABLE_ID;
            NUM_OF_PEOPLE = a.NUM_OF_PEOPLE;
            RES_STATUS = a.RES_STATUS;
            SPECIAL_REQUEST = a.SPECIAL_REQUEST;
            IS_DELETED = a.IS_DELETED;
        }

        public bool IsEqual(ReservationDTO other)
        {
            if(this.RES_TIME == other.RES_TIME && this.RES_DATE == other.RES_DATE && this.NUM_OF_PEOPLE == other.NUM_OF_PEOPLE
                && this.TABLE_ID == other.TABLE_ID
                && this.SPECIAL_REQUEST == other.SPECIAL_REQUEST)
                return true;
            return false;
        }
    }
}
