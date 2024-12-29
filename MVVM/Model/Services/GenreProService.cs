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
                                        where(c.IS_DELETED == false)
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

        internal async Task<int> IDOfGenPrd()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    return await context.GENRE_PRODUCT.CountAsync();
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return 0;
            }
        }

        internal async Task<bool> AddNewGen(GenreProductDTO temp)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    GENRE_PRODUCT item = new GENRE_PRODUCT()
                    {
                        GP_ID = temp.GP_ID,
                        GP_NAME = temp.GP_NAME,
                        IS_DELETED = false,
                    };
                    context.GENRE_PRODUCT.Add(item);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return false;
            }
        }

        internal async Task<bool> SaveChangeGen(GenreProductDTO temp)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var genprd = await context.GENRE_PRODUCT.FirstOrDefaultAsync(g => g.GP_ID == temp.GP_ID);
                    genprd.GP_NAME = temp.GP_NAME;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return false;
            }
        }

        internal async Task<bool> DeleteGenprd(GenreProductDTO selectedGenPrd)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var genprd = await context.GENRE_PRODUCT.FirstOrDefaultAsync(g => g.GP_ID == selectedGenPrd.GP_ID);
                    genprd.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return false;
            }
        }
    }
}
