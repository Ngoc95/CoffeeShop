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
    internal class GenreTableService
    {
        public GenreTableService() { }
        private static GenreTableService _ins;

        public static GenreTableService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new GenreTableService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        //Get all GenreProduct

        public async Task<List<GenreTableDTO>> GetAllGenre()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var GenreTableList = (from c in context.GENRE_TABLE
                                        select new GenreTableDTO
                                        {
                                            GT_ID = c.GT_ID,
                                            GT_NAME = c.GT_NAME
                                        }).ToListAsync();
                    return await GenreTableList;
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
