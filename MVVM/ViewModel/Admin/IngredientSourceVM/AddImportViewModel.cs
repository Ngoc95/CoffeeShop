using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.MVVM.View.Admin.IngredientSourceManagement;
using QuanLiCoffeeShop.MVVM.View.Message;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin.IngredientSourceVM
{
    public class AddImportViewModel : ObservableObject
    {
        private ObservableCollection<SupplierDTO> _suppliers;

        public ObservableCollection<SupplierDTO> Suppliers
        {
            get { return _suppliers; }
            set { _suppliers = value; OnPropertyChanged(nameof(Suppliers)); }
        }

        private SupplierDTO _selectedSupplier;

        public SupplierDTO SelectedSupplier
        {
            get { return _selectedSupplier; }
            set { _selectedSupplier = value; OnPropertyChanged(nameof(SelectedSupplier)); }
        }

        private ObservableCollection<ImportInfoCardViewModel> _importInfo;
        public ObservableCollection<ImportInfoCardViewModel> ImportInfo
        {
            get
            {
                if (_importInfo == null)
                {
                    _importInfo = new ObservableCollection<ImportInfoCardViewModel>();
                    _importInfo.Add(new ImportInfoCardViewModel(this));
                }
                return _importInfo;
            }
            set { }
        }

        public ICommand FirstLoadCommand { get; set; }
        public ICommand AddCardCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand OpenAddSupCommand { get; set; }
        public ImportViewModel ParentViewModel { get; set; }
        public AddImportViewModel()
        {
            ImportInfo = new ObservableCollection<ImportInfoCardViewModel>();

            FirstLoadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LoadSupplier();
            });

            OpenAddSupCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddSupplierWindow wd = new AddSupplierWindow();
                wd.ShowDialog();
                LoadSupplier();
            });

            AddCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (SelectedSupplier == null || !ImportInfo.Any())
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Vui lòng chọn nhà cung cấp và thêm ít nhất một thông tin nhập hàng.");
                    return;
                }

                HashSet<int> seenIngIds = new HashSet<int>();

                foreach (var info in ImportInfo)
                {
                    if (info.SelectedIngredient == null || info.Quantity <= 0 || !decimal.TryParse(info.Cost, out _))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Thông tin nhập hàng không hợp lệ. Vui lòng kiểm tra lại.");
                        return;
                    }
                    if (!seenIngIds.Add(info.SelectedIngredient.ID))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Nguyên liệu của các thẻ không được trùng lặp.");
                        return;
                    }
                }

                try
                {

                    decimal totalCost = ImportInfo.Sum(info => info.Quantity * decimal.Parse(info.Cost));

                    ImportDTO newImport = new ImportDTO
                    {
                        SupId = SelectedSupplier.ID,
                        EmpId = MainViewModel.currentEmp.EMP_ID,
                        ImpDate = DateTime.Now,
                        TotalCost = totalCost,
                    };

                    var (isImportAdded, importMessage) = await ImportService.Ins.AddNewImport(newImport);

                    if (!isImportAdded)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, importMessage);
                        return;
                    }
                    var newImportId = (await ImportService.Ins.GetAllImports()).Last().ImpId;

                    foreach (var item in ImportInfo)
                    {
                        ImportInfoDTO newImportInfo = new ImportInfoDTO
                        {
                            ImpId = newImportId,
                            IngId = item.SelectedIngredient.ID,
                            Quantity = item.Quantity,
                            PriceItem = decimal.Parse(item.Cost),
                        };


                        bool isImportInfoAdded = await ImportInfoService.Ins.AddImportInfo(newImportInfo);
                        if (!isImportInfoAdded)
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, "Thêm thông tin nhập hàng thất bại.");
                            return;
                        }
                    }
                    if (isImportAdded)
                    {
                        ParentViewModel.Imports = new ObservableCollection<ImportDTO>(await ImportService.Ins.GetAllImports());
                    }

                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Thêm phiếu nhập thành công!");

                    p.DialogResult = true;
                    ImportInfo.Clear();
                    SelectedSupplier = null;
                    p?.Close();
                }
                catch 
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                }
            });


            AddCardCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ImportInfo.Add(new ImportInfoCardViewModel(this));
            });

            CancelCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                ImportInfo.Clear();
                p.Close();
            });
        }

        private async void LoadSupplier()
        {
            Suppliers = new ObservableCollection<SupplierDTO>(await SupplierService.Ins.GetAllSuppliers());
        }
    }
}