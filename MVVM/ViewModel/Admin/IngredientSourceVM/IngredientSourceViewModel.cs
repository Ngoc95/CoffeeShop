using QuanLiCoffeeShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin.IngredientSourceVM
{
    public class IngredientSourceViewModel : ObservableObject
    {
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public ICommand SwitchToIngredientSourceCommand { get; set; }
        public ICommand SwitchToImportCommand { get; set; }
        public ICommand SwitchToSupplierCommand { get; set; }
        public IngredientSourceViewModel()
        {
            CurrentView = new IngredientViewModel();
            SwitchToImportCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new ImportViewModel(); });
            SwitchToIngredientSourceCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new IngredientViewModel(); });
            SwitchToSupplierCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new SupplierViewModel(); });
        }
    }
}
