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
    public class ExportDetailViewModel : ObservableObject
    {
        private ExportDTO _exportDetail;

        public ExportDTO ExportDetail
        {
            get { return _exportDetail; }
            set { _exportDetail = value; OnPropertyChanged(nameof(ExportDetail)); }
        }

        private ObservableCollection<ExportInfoDTO> _exportInfos;
        public ObservableCollection<ExportInfoDTO> ExportInfos
        {
            get { return _exportInfos; }
            set { _exportInfos = value; OnPropertyChanged(nameof(ExportInfos)); }
        }

        public ICommand LoadedCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ExportDetailViewModel()
        {
            LoadedCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                ExportInfos = new ObservableCollection<ExportInfoDTO>(await ExportInfoService.Ins.GetExportInfoByExportID(ExportDetail.ExpId));
            });

            SearchCommand = new RelayCommand<TextBox>(null, async (p) =>
            {
                if (p == null || string.IsNullOrEmpty(p.Text))
                {
                    ExportInfos = new ObservableCollection<ExportInfoDTO>(await ExportInfoService.Ins.GetExportInfoByExportID(ExportDetail.ExpId));
                    return;
                }
                string searchText = p.Text.ToLower();

                ExportInfos = new ObservableCollection<ExportInfoDTO>(
                    (await ExportInfoService.Ins.GetExportInfoByExportID(ExportDetail.ExpId)).FindAll(
                        x => x.IngName.ToLower().Contains(searchText.ToLower()) ||
                        $"nl{x.IngId:D3}".ToLower().Contains(searchText)
                    ));
            });
        }
    }
}
