using QuanLiCoffeeShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff.IngredientSourceVM
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
        public ICommand SwitchToExportCommand { get; set; }
        public IngredientSourceViewModel()
        {
            CurrentView = new IngredientViewModel();
            SwitchToIngredientSourceCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new IngredientViewModel(); });
            SwitchToExportCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new ExportViewModel(); });
        }
    }
}
