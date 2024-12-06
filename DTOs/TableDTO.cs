using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class TableDTO
    {

        public TableDTO(TableDTO tableDTO)
        {
            TB_ID = tableDTO.TB_ID;
            GT_ID = tableDTO.GT_ID;
            TB_STATUS = tableDTO.TB_STATUS;
            IS_DELETED = tableDTO.IS_DELETED;
        }
        public TableDTO(){ }
        public int TB_ID { get; set; }
        public Nullable<int> GT_ID { get; set; }
        public string TB_STATUS { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
    }
}
