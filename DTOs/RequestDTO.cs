using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class RequestDTO
    {
        public int REQ_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public string REQ_TYPE { get; set; }
        public System.DateTime REQ_DATE { get; set; }
        public string REQ_STATUS { get; set; }
        public string EMP_COMMENT { get; set; }
        public string APPROVER_COMMENT { get; set; }
        public Nullable<int> APPROVED_BY { get; set; }
        public Nullable<System.DateTime> APPROVED_DATE { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
        public virtual EMPLOYEE EMPLOYEE { get; set; }
    }
}
