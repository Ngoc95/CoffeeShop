using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.IngredientSourceManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin.IngredientSourceVM
{
    public class ImportInfoCardViewModel : ObservableObject
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

        private string _cost;
        public string Cost
        {
            get { return _cost; }
            set { _cost = value; OnPropertyChanged(nameof(Cost)); }
        }
        public ICommand OpenAddIngCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public AddImportViewModel ParentViewModel { get; set; }
        public ImportInfoCardViewModel(AddImportViewModel parentViewModel)
        {
            
            LoadIngredient();
            ParentViewModel = parentViewModel;

            OpenAddIngCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddIngredientWindow wd = new AddIngredientWindow();
                wd.ShowDialog();
                LoadIngredient();
            });

            DeleteCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ParentViewModel?.ImportInfo.Remove(this);
            });
        }

        private async void LoadIngredient()
        {
            Ingredients = new ObservableCollection<IngredientDTO>(await IngredientService.Ins.GetAllIngredients());
        }
    }
}
