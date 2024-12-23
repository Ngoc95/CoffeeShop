using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.ViewModel.Admin.IngredientSourceVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff.IngredientSourceVM
{
    public class ExportInfoCardViewModel : ObservableObject
    {
        private ObservableCollection<IngredientDTO> _ingredients;
        public ObservableCollection<IngredientDTO> Ingredients
        {
            get { return _ingredients; }
            set { _ingredients = value; OnPropertyChanged(nameof(Ingredients)); }
        }

        private IngredientDTO _selectedIngredient;
        public IngredientDTO SelectedIngredient
        {
            get { return _selectedIngredient; }
            set { _selectedIngredient = value; OnPropertyChanged(nameof(SelectedIngredient)); }
        }

        private int _quantity = 1;
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); }
        }
        public ICommand DeleteCommand { get; set; }
        public AddExportViewModel ParentViewModel { get; set; }

        public ExportInfoCardViewModel(AddExportViewModel parentViewModel)
        {
            LoadIngredient();
            ParentViewModel = parentViewModel;

            DeleteCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ParentViewModel?.ExportInfo.Remove(this);
            });
        }

        private async void LoadIngredient()
        {
            Ingredients = new ObservableCollection<IngredientDTO>(await IngredientService.Ins.GetAllIngredients());
        }
    }
}
