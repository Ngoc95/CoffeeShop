using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiCoffeeShop.MVVM.Model.Services
{
    public class IngredientService
    {
        public IngredientService() { }
        private static IngredientService _ins;

        public static IngredientService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new IngredientService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<List<IngredientDTO>> GetAllIngredients()
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var ingredientList = (from s in context.INGREDIENTs
                                          where s.IS_DELETED == false
                                          select new IngredientDTO
                                          {
                                              ID = s.ING_ID,
                                              Name = s.ING_NAME,
                                              Unit = s.ING_UNIT,
                                              Quantity = (context.IMPORT_INFO.Where(i => i.ING_ID == s.ING_ID && i.IS_DELETED == false).Sum(i => (int?)i.QUANTITY) ?? 0)
                                               - (context.EXPORT_INFO.Where(e => e.ING_ID == s.ING_ID && e.IS_DELETED == false).Sum(e => (int?)e.QUANTITY) ?? 0),
                                              IsDeleted = s.IS_DELETED,
                                          }).ToListAsync();
                    return await ingredientList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }

        public async Task<(bool, string)> AddNewIngredient(IngredientDTO Ingredient)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    // Check if the ingredient already exists and is deleted
                    var existingIngredient = await context.INGREDIENTs
                        .FirstOrDefaultAsync(i => i.ING_NAME == Ingredient.Name);

                    if (existingIngredient != null)
                    {
                        // If the ingredient is deleted, restore it
                        if (existingIngredient.IS_DELETED == true)
                        {
                            existingIngredient.IS_DELETED = false; // Restore the ingredient
                            existingIngredient.ING_UNIT = Ingredient.Unit; // Update unit if needed
                            await context.SaveChangesAsync();
                            return (true, "Nguyên liệu đã được khôi phục");
                        }

                        // If the ingredient exists and is not deleted, inform the user
                        return (false, "Nguyên liệu đã tồn tại");
                    }

                    // If not found, add the new ingredient
                    var newIngredient = new INGREDIENT
                    {
                        ING_NAME = Ingredient.Name,
                        ING_UNIT = Ingredient.Unit,
                        IS_DELETED = false
                    };

                    context.INGREDIENTs.Add(newIngredient);
                    await context.SaveChangesAsync();
                    return (true, "Thêm nguyên liệu thành công");
                }
            }
            catch
            {
                return (false, "Thêm nguyên liệu thất bại");
            }
        }


        public async Task<(bool, string)> EditIngredient(IngredientDTO ingredient, int id)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var ingredientEdit = await context.INGREDIENTs.Where(p => p.ING_ID == id).FirstOrDefaultAsync();
                    if (ingredientEdit == null)
                        return (false, "Nguyên liệu không tồn tại");
                    ingredientEdit.ING_NAME = ingredient.Name;
                    ingredientEdit.ING_UNIT = ingredient.Unit;
                    await context.SaveChangesAsync();
                    return (true, "Sửa nguyên liệu thành công");
                }
            }
            catch
            {
                return (false, "Sửa nguyên liệu thất bại");
            }
        }

        public async Task<(bool, string)> DeleteIngredient(int id)
        {
            try
            {
                using (var context = new CoffeeShopDBEntities())
                {
                    var ingredient = await context.INGREDIENTs.Where(p => p.ING_ID == id).FirstOrDefaultAsync();
                    if (ingredient == null)
                        return (false, "Nguyên liệu không tồn tại");
                    ingredient.IS_DELETED = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa nguyên liệu thành công");
                }
            }
            catch
            {
                return (false, "Xóa nguyên liệu thất bại");
            }
        }
    }
}
