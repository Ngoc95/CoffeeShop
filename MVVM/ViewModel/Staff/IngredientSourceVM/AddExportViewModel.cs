using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff.IngredientSourceVM
{
    public class AddExportViewModel : ObservableObject
    {

        private ObservableCollection<ExportInfoCardViewModel> _exportInfo;

        public ObservableCollection<ExportInfoCardViewModel> ExportInfo
        {
            get
            {
                if (_exportInfo == null)
                {
                    _exportInfo = new ObservableCollection<ExportInfoCardViewModel>();
                    _exportInfo.Add(new ExportInfoCardViewModel(this));
                }
                return _exportInfo;
            }
            set { }
        }
        public ICommand AddCardCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ExportViewModel ParentViewModel { get; set; }

        public AddExportViewModel()
        {
            ExportInfo = new ObservableCollection<ExportInfoCardViewModel>();

            AddCardCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ExportInfo.Add(new ExportInfoCardViewModel(this));
            });

            CancelCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                ExportInfo.Clear();
                p.Close();
            });

            AddCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (!ExportInfo.Any())
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Vui lòng thêm ít nhất một thông tin xuất hàng.");
                    return;
                }

                HashSet<int> seenIngIds = new HashSet<int>();
                foreach (var info in ExportInfo)
                {
                    if (info.SelectedIngredient == null || info.Quantity <= 0)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Thông tin xuất hàng không hợp lệ. Vui lòng kiểm tra lại.");
                        return;
                    }
                    if (!seenIngIds.Add(info.SelectedIngredient.ID))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Nguyên liệu của các thẻ không được trùng lặp.");
                        return;
                    }
                    if(info.Quantity > info.SelectedIngredient.Quantity)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, $"Số lượng xuất của {info.SelectedIngredient.Name} vượt quá số lượng tồn kho ({info.SelectedIngredient.Quantity}).");
                        return;
                    }
                }

                try
                {
                    ExportDTO newExport = new ExportDTO
                    {
                        EmpId = MainViewModel.currentEmp.EMP_ID,
                        Quantity = ExportInfo.Sum(i => i.Quantity),
                        ExpDate = DateTime.Now,
                        EmpName = MainViewModel.currentEmp.EMP_NAME,
                    };

                    var (isExportAdded, exportMessage) = await ExportService.Ins.AddExport(newExport);

                    if (!isExportAdded)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, exportMessage);
                        return;
                    }
                    var newExportId = (await ExportService.Ins.GetAllExports()).Last().ExpId;

                    foreach (var item in ExportInfo)
                    {
                        ExportInfoDTO newExportInfo = new ExportInfoDTO
                        {
                            ExpId = newExportId,
                            IngId = item.SelectedIngredient.ID,
                            Quantity = item.Quantity,
                            PriceItem = 0
                        };

                        bool isExportInfoAdded = await ExportInfoService.Ins.AddNewExportInfo(newExportInfo);
                        if (!isExportInfoAdded)
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, "Thêm phiếu xuất thất bại.");
                            return;
                        }
                    }
                    if (isExportAdded)
                    {
                        ParentViewModel.Exports = new ObservableCollection<ExportDTO>(await ExportService.Ins.GetAllExports());
                    }

                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Thêm phiếu xuất thành công!");

                    p.DialogResult = true;
                    ExportInfo.Clear();
                    p?.Close();
                }
                catch 
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                }
            });
        }
    }
}
