using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.IngredientSourceManagement;
using QuanLiCoffeeShop.MVVM.View.Message;
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
    public class ImportViewModel : ObservableObject
    {
        private ObservableCollection<ImportDTO> _imports;

        public ObservableCollection<ImportDTO> Imports
        {
            get { return _imports; }
            set { _imports = value; OnPropertyChanged(nameof(Imports)); }
        }

        private ImportDTO _selectedItem;
        public ImportDTO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    EditImport = new ImportDTO
                    {
                        ImpId = _selectedItem.ImpId,
                        SupId = _selectedItem.SupId,
                        EmpId = _selectedItem.EmpId,
                        SupplierName = _selectedItem.SupplierName,
                        EmployeeName = _selectedItem.EmployeeName,
                        Quantity = _selectedItem.Quantity,
                        ImpDate = _selectedItem.ImpDate,
                        TotalCost = _selectedItem.TotalCost,
                        IsDeleted = _selectedItem.IsDeleted
                    };

                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private ImportDTO _editImport;

        public ImportDTO EditImport
        {
            get { return _editImport; }
            set { _editImport = value; OnPropertyChanged(nameof(EditImport)); }
        }

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

        private ObservableCollection<EmployeeDTO> _employees;

        public ObservableCollection<EmployeeDTO> Employees
        {
            get { return _employees; }
            set { _employees = value; OnPropertyChanged(nameof(Employees)); }
        }

        private EmployeeDTO _selectedEmployee;
        public EmployeeDTO SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); }
        }

        public ICommand FirstLoadCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OpenAddWindowCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand OpenDetailCommand { get; set; }

        public ImportViewModel()
        {
            FirstLoadCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                Employees = new ObservableCollection<EmployeeDTO>(await EmployeeService.Ins.GetAllEmp());
                Suppliers = new ObservableCollection<SupplierDTO>(await SupplierService.Ins.GetAllSuppliers());
                Imports = new ObservableCollection<ImportDTO>(await ImportService.Ins.GetAllImports());
            });

            EditCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(EditImport.SupplierName) || string.IsNullOrEmpty(EditImport.EmployeeName))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                    return;
                }

                ImportDTO import = new ImportDTO
                {
                    ImpId = EditImport.ImpId,
                    SupId = EditImport.SupId,
                    EmpId = EditImport.EmpId,
                    SupplierName = EditImport.SupplierName,
                    EmployeeName = EditImport.EmployeeName,
                    Quantity = EditImport.Quantity,
                    ImpDate = EditImport.ImpDate,
                    TotalCost = EditImport.TotalCost,
                    IsDeleted = EditImport.IsDeleted
                };

                (bool result, string message) = await ImportService.Ins.EditImport(import, SelectedItem.ImpId);
                if (result)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Success, message);
                    Imports = new ObservableCollection<ImportDTO>(await ImportService.Ins.GetAllImports());
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
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Vui lòng chọn phiếu nhập cần xóa");
                    return;
                }
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool result, string message) = await ImportService.Ins.DeleteImport(SelectedItem.ImpId);
                    if (result)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Success, message);
                        Imports = new ObservableCollection<ImportDTO>(await ImportService.Ins.GetAllImports());
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                    }
                }
            });

            OpenAddWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddImportWindow wd = new AddImportWindow();
                var addImportViewModel = new AddImportViewModel
                {
                    ParentViewModel = this // Truyền tham chiếu của ImportViewModel
                };
                wd.DataContext = addImportViewModel;
                wd.ShowDialog();
            });

            OpenDetailCommand = new RelayCommand<object>(
                (p) => SelectedItem != null,
                p =>
                {
                    ImportDetailWindow wd = new ImportDetailWindow();
                    ImportDetailViewModel detailVM = new ImportDetailViewModel
                    {
                        ImportDetail = SelectedItem
                    };
                    wd.DataContext = detailVM;
                    wd.ShowDialog();
            });

            SearchCommand = new RelayCommand<TextBox>((p) => { return true; }, async (p) =>
            {
                if (p == null || string.IsNullOrWhiteSpace(p.Text))
                {
                    Imports = new ObservableCollection<ImportDTO>(await ImportService.Ins.GetAllImports());
                    return;
                }
                string searchText = p.Text.ToLower();

                Imports = new ObservableCollection<ImportDTO>(
                 (await ImportService.Ins.GetAllImports()).FindAll(x =>
                        $"imp{x.ImpId:D3}".ToString().Contains(searchText) ||
                        x.SupplierName.ToLower().Contains(searchText) ||
                        x.EmployeeName.ToLower().Contains(searchText) ||
                        x.ImpDate.ToString().Contains(searchText)
                ));
            });
        }
    }
}
