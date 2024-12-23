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

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin.IngredientSourceVM
{
    public class ImportDetailViewModel : ObservableObject
    {
        private ImportDTO _importDetail;

        public ImportDTO ImportDetail
        {
            get { return _importDetail; }
            set { _importDetail = value; OnPropertyChanged(nameof(ImportDetail)); }
        }

        private ObservableCollection<ImportInfoDTO> _importInfos;
        public ObservableCollection<ImportInfoDTO> ImportInfos
        {
            get { return _importInfos; }
            set { _importInfos = value; OnPropertyChanged(nameof(ImportInfos)); }
        }

        public ICommand LoadedCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ImportDetailViewModel()
        {
            LoadedCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                ImportInfos = new ObservableCollection<ImportInfoDTO>(await ImportInfoService.Ins.GetImportInfosByImportID(ImportDetail.ImpId));
            });

            SearchCommand = new RelayCommand<TextBox>(null, async (p) =>
            {
                if (p == null || string.IsNullOrEmpty(p.Text))
                {
                    ImportInfos = new ObservableCollection<ImportInfoDTO>(await ImportInfoService.Ins.GetImportInfosByImportID(ImportDetail.ImpId));
                    return;
                }
                string searchText = p.Text.ToLower();

                ImportInfos = new ObservableCollection<ImportInfoDTO>(
                    (await ImportInfoService.Ins.GetImportInfosByImportID(ImportDetail.ImpId)).FindAll(x => x.IngName.ToLower().Contains(searchText.ToLower())));
            });
        }
    }
}
