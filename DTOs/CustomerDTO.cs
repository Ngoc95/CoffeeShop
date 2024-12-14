using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    public class CustomerDTO
    {

        public CustomerDTO() { }
        public CustomerDTO(CustomerDTO a) 
        {
            ID = a.ID;
            Name = a.Name;
            Email = a.Email;
            Phone = a.Phone;
            Gender = a.Gender;
            Point = a.Point;
            IsDeleted = a.IsDeleted;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public Nullable<decimal> Point { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
