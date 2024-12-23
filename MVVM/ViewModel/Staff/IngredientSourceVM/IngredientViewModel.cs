using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff.IngredientSourceVM
{
    public class IngredientViewModel : ObservableObject
    {
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
                OnPropertyChanged(nameof(SelectedItem));
            }
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
    }
}