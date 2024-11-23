using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class ErrorDTO
    {
        public int ER_ID { get; set; }
        public string ER_NAME { get; set; }
        public string ER_STATUS { get; set; }
        public string ER_DESCRIPTION { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
    }
}
