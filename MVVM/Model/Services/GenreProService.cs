using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.MVVM.Model.Services
{
    internal class GenreProService
    {
        public GenreProService() { }
        private static GenreProService _ins;

        public static GenreProService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new GenreProService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        //Get all GenreProduct

        public async Task<List<GenreProductDTO>> GetAllGenre()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var GenreProList = (from c in context.GENRE_PRODUCT
                                        select new GenreProductDTO
                                        {
                                            GP_ID = c.GP_ID,
                                            GP_NAME = c.GP_NAME,
                                        }).ToListAsync();
                    return await GenreProList;
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
