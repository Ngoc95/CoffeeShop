using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.View.Staff.IngredientSourceManagement;
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
    public class ExportViewModel : ObservableObject
    {
        private ObservableCollection<ExportDTO> _exports;

        public ObservableCollection<ExportDTO> Exports
        {
            get { return _exports; }
            set { _exports = value; OnPropertyChanged(nameof(Exports)); }
        }
        private ExportDTO _selectedItem;

        public ExportDTO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    EditExport = new ExportDTO
                    {
                        ExpId = _selectedItem.ExpId,
                        EmpId = _selectedItem.EmpId,
                        EmpName = _selectedItem.EmpName,
                        Quantity = _selectedItem.Quantity,
                        ExpDate = _selectedItem.ExpDate,
                        TotalCost = _selectedItem.TotalCost,
                        IsDeleted = _selectedItem.IsDeleted
                    };
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private ExportDTO _editExport;

        public ExportDTO EditExport
        {
            get { return _editExport; }
            set { _editExport = value; OnPropertyChanged(nameof(EditExport)); }
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
        //public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OpenAddWindowCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand OpenDetailCommand { get; set; }

        public ExportViewModel()
        {
            FirstLoadCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                Exports = new ObservableCollection<ExportDTO>(await ExportService.Ins.GetAllExports());
                Employees = new ObservableCollection<EmployeeDTO>(await EmployeeService.Ins.GetAllEmp());
            });

            //EditCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            //{
            //    if (string.IsNullOrEmpty(EditExport.EmpName))
            //    {
            //        MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
            //        return;
            //    }

            //    ExportDTO export = new ExportDTO
            //    {
            //        ExpId = EditExport.ExpId,
            //        EmpId = EditExport.EmpId,
            //        EmpName = EditExport.EmpName,
            //        Quantity = EditExport.Quantity,
            //        ExpDate = EditExport.ExpDate,
            //        TotalCost = EditExport.TotalCost,
            //        IsDeleted = EditExport.IsDeleted
            //    };

            //    (bool result, string message) = await ExportService.Ins.EditExport(export, SelectedItem.ExpId);
            //    if (result)
            //    {
            //        MessageBoxCustom.Show(MessageBoxCustom.Success, message);
            //        Exports = new ObservableCollection<ExportDTO>(await ExportService.Ins.GetAllExports());
            //    }
            //    else
            //    {
            //        MessageBoxCustom.Show(MessageBoxCustom.Error, message);
            //    }
            //});

            DeleteCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (SelectedItem == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Vui lòng chọn một phiếu xuất hàng để xóa.");
                    return;
                }
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool result, string message) = await ExportService.Ins.DeleteExport(SelectedItem.ExpId);
                    if (result)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Success, message);
                        Exports = new ObservableCollection<ExportDTO>(await ExportService.Ins.GetAllExports());
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                    }
                }
            });

            OpenAddWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddExportWindow wd = new AddExportWindow();
                var addExportVM = new AddExportViewModel
                {
                    ParentViewModel = this
                };
                wd.DataContext = addExportVM;
                wd.ShowDialog();
            });

            OpenDetailCommand = new RelayCommand<object>(
                (p) => SelectedItem != null,
                p =>
                {
                    ExportDetailWindow wd = new ExportDetailWindow();
                    var detailVM = new ExportDetailViewModel
                    {
                        ExportDetail = SelectedItem
                    };
                    wd.DataContext = detailVM;
                    wd.ShowDialog();
                });

            SearchCommand = new RelayCommand<TextBox>((p) => { return true; }, async (p) =>
            {
                if (p == null || string.IsNullOrWhiteSpace(p.Text))
                {
                    Exports = new ObservableCollection<ExportDTO>(await ExportService.Ins.GetAllExports());
                    return;
                }
                string searchText = p.Text.ToLower();

                Exports = new ObservableCollection<ExportDTO>(
                    (await ExportService.Ins.GetAllExports()).FindAll(x =>
                    $"exp{x.ExpId:D3}".ToString().Contains(searchText) ||
                    x.EmpName.ToLower().Contains(searchText.ToLower()) ||
                    x.ExpDate.ToString().Contains(searchText)
                    ));
            });
        }

    }
}