using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.DTOs
{
    internal class GenreProductDTO
    {

        public int GP_ID { get; set; }
        public string GP_NAME { get; set; }

        public GenreProductDTO(int id, string name)
        {
            GP_ID = id;
            GP_NAME = name;
        }

        public GenreProductDTO(){}
    }
}
