using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class ShiftScheduleDTO
    {
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? StartTime { get; set; }       
        public DateTime? EndTime { get; set; }
        public Dictionary<int, List<string>> EmployeeNamesByDay { get; set; } = new Dictionary<int, List<string>>();


    }
}
