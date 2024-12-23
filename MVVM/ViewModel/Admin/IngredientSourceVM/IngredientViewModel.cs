using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
using System.Windows.Controls;
using QuanLiCoffeeShop.MVVM.View.Admin.IngredientSourceManagement;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin.IngredientSourceVM
{
    public class IngredientViewModel : ObservableObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _unit;

        public string Unit
        {
            get { return _unit; }
            set { _unit = value; OnPropertyChanged(nameof(Unit)); }
        }

        //for datagrid
        private ObservableCollection<IngredientDTO> _ingredients;
        public ObservableCollection<IngredientDTO> Ingredients
        {
            get { return _ingredients; }
            set { _ingredients = value; OnPropertyChanged(nameof(Ingredients)); }
        }

        private IngredientDTO _selectedItem;
        public IngredientDTO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    EditIngredient = new IngredientDTO
                    {
                        ID = _selectedItem.ID,
                        Name = _selectedItem.Name,
                        Unit = _selectedItem.Unit,
                        Quantity = _selectedItem.Quantity,
                        IsDeleted = _selectedItem.IsDeleted
                    };
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private IngredientDTO _editIngredient;
        public IngredientDTO EditIngredient
        {
            get { return _editIngredient; }
            set { _editIngredient = value; OnPropertyChanged(nameof(EditIngredient)); }
        }
        public ObservableCollection<string> StatusList { get; set; } = new ObservableCollection<string>
        {
            "Tất cả",
            "Còn hàng\n(SL > 10)",
            "Sắp hết\n(0 < SL <= 10)",
            "Đã hết"
        };
        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();

                CommandManager.InvalidateRequerySuggested();
                if (FilterCommand.CanExecute(null))
                    FilterCommand.Execute(null);
            }
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
        public ICommand FirstLoadCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OpenAddWindowCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand FilterCommand { get; set; }

        public IngredientViewModel()
        {
            FirstLoadCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                Ingredients = new ObservableCollection<IngredientDTO>(await IngredientService.Ins.GetAllIngredients());
            });

            SearchCommand = new RelayCommand<TextBox>((p) => { return true; }, async (p) =>
            {
                string searchText = p?.Text ?? string.Empty;
                await ApplyFilterAndSearch(searchText, SelectedStatus);
            });

            FilterCommand = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                await ApplyFilterAndSearch(SearchText, SelectedStatus);
            });

            EditCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(EditIngredient.Name) || string.IsNullOrEmpty(EditIngredient.Unit))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                    return;
                }

                IngredientDTO ingredient = new IngredientDTO
                {
                    ID = EditIngredient.ID,
                    Name = EditIngredient.Name,
                    Unit = EditIngredient.Unit,
                    Quantity = EditIngredient.Quantity,
                    IsDeleted = EditIngredient.IsDeleted
                };

                (bool result, string message) = await IngredientService.Ins.EditIngredient(ingredient, SelectedItem.ID);
                if (result)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Success, message);
                    Ingredients = new ObservableCollection<IngredientDTO>(await IngredientService.Ins.GetAllIngredients());
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                }
            });

            DeleteCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (SelectedItem == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Vui lòng chọn nguyên liệu cần xóa");
                    return;
                }
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool result, string message) = await IngredientService.Ins.DeleteIngredient(SelectedItem.ID);
                    if (result)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Success, message);
                        Ingredients = new ObservableCollection<IngredientDTO>(await IngredientService.Ins.GetAllIngredients());
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                    }
                }
            });

            OpenAddWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddIngredientWindow addIngredient = new AddIngredientWindow();
                addIngredient.ShowDialog();
            });

            AddCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Unit))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                    return;
                }

                IngredientDTO ingredient = new IngredientDTO
                {
                    Name = Name,
                    Unit = Unit,
                    Quantity = 0,
                    IsDeleted = false
                };

                (bool result, string message) = await IngredientService.Ins.AddNewIngredient(ingredient);
                if (result)
                {
                    p.Close();
                    resetData();
                    Ingredients = new ObservableCollection<IngredientDTO>(await IngredientService.Ins.GetAllIngredients());
                    MessageBoxCustom.Show(MessageBoxCustom.Success, message);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                }
            });
        }
        private async Task ApplyFilterAndSearch(string searchText, string filterStatus)
        {
            if (IngredientService.Ins == null)
                return;
            filterStatus = filterStatus?.ToLower().Replace("\n", "").Trim();
            searchText = searchText?.ToLower() ?? string.Empty;
            var allIng = await IngredientService.Ins.GetAllIngredients() ?? new List<IngredientDTO>();

            // Nếu cả filter và search text đều trống
            if (string.IsNullOrWhiteSpace(searchText) && string.IsNullOrWhiteSpace(filterStatus))
            {
                Ingredients = new ObservableCollection<IngredientDTO>(allIng);
                return;
            }
            // Lọc và tìm kiếm
            Ingredients = new ObservableCollection<IngredientDTO>(allIng.FindAll(x =>
            {
                // Lọc theo trạng thái
                bool matchesStatus = false;
                if (filterStatus == "tất cả")
                {
                    matchesStatus = true;
                }
                else if (filterStatus == "còn hàng(sl > 10)")
                {
                    matchesStatus = x.Quantity > 10;
                }
                else if (filterStatus == "sắp hết(0 < sl <= 10)")
                {
                    matchesStatus = (x.Quantity > 0 && x.Quantity <= 10);
                }
                else if (filterStatus == "đã hết")
                {
                    matchesStatus = x.Quantity == 0;
                }

                // Tìm kiếm theo searchText
                bool matchesSearchText = string.IsNullOrEmpty(searchText) ||
                                          ($"nl{x.ID:D3}".ToLower().Contains(searchText)) ||
                                          (x.Name?.ToLower().Contains(searchText) ?? false);

                return matchesStatus && matchesSearchText;
            }));
        }


        private void resetData()
        {
            Name = null;
            Unit = null;
        }
    }
}

