using QuanLiCoffeeShop.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLiCoffeeShop.Core;
using System.Windows.Controls;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.View.Admin.IngredientSourceManagement;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin.IngredientSourceVM
{
    public class SupplierViewModel : ObservableObject
    {
        private string _supplierName;
        public string SupplierName
        {
            get { return _supplierName; }
            set { _supplierName = value; OnPropertyChanged(nameof(SupplierName)); }
        }

        private string _supplierPhone;
        public string SupplierPhone
        {
            get { return _supplierPhone; }
            set { _supplierPhone = value; OnPropertyChanged(nameof(SupplierPhone)); }
        }

        private string _supplierAddress;
        public string SupplierAddress
        {
            get { return _supplierAddress; }
            set { _supplierAddress = value; OnPropertyChanged(nameof(SupplierAddress)); }
        }

        //for datagrid
        private ObservableCollection<SupplierDTO> _suppliers;
        public ObservableCollection<SupplierDTO> Suppliers
        {
            get { return _suppliers; }
            set { _suppliers = value; OnPropertyChanged(nameof(Suppliers)); }
        }

        private SupplierDTO _selectedItem;

        public SupplierDTO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    EditSupplier = new SupplierDTO
                    {
                        ID = _selectedItem.ID,
                        Name = _selectedItem.Name,
                        Phone = _selectedItem.Phone,
                        Address = _selectedItem.Address,
                        IsDeleted = _selectedItem.IsDeleted
                    };
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private SupplierDTO _editSupplier;

        public SupplierDTO EditSupplier
        {
            get { return _editSupplier; }
            set { _editSupplier = value; OnPropertyChanged(nameof(EditSupplier)); }
        }
        public ICommand FirstLoadCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OpenAddWindowCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public SupplierViewModel()
        {
            FirstLoadCommand = new RelayCommand<object>(null, async o =>
            {
                Suppliers = new ObservableCollection<SupplierDTO>(await SupplierService.Ins.GetAllSuppliers());
            });

            SearchCommand = new RelayCommand<TextBox>(p => { return true; }, async p =>
            {
                if (p == null || string.IsNullOrWhiteSpace(p.Text))
                {
                    Suppliers = new ObservableCollection<SupplierDTO>(await SupplierService.Ins.GetAllSuppliers());
                    return;
                }
                string searchText = p.Text.ToLower();

                Suppliers = new ObservableCollection<SupplierDTO>(
                 (await SupplierService.Ins.GetAllSuppliers()).FindAll(x =>
                        ($"ncc{x.ID:D3}".ToLower().Contains(searchText)) ||
                        x.Name.ToLower().Contains(searchText) ||
                        x.Phone.ToLower().Contains(searchText) ||
                        x.Address.ToLower().Contains(searchText)
                    ));
            });

            EditCommand = new RelayCommand<object>(null, async o =>
            {
                if (string.IsNullOrEmpty(EditSupplier.Name) || string.IsNullOrEmpty(EditSupplier.Phone) || string.IsNullOrEmpty(EditSupplier.Address))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                    return;
                }

                SupplierDTO supplier = new SupplierDTO
                {
                    ID = EditSupplier.ID,
                    Name = EditSupplier.Name,
                    Phone = EditSupplier.Phone,
                    Address = EditSupplier.Address,
                    IsDeleted = EditSupplier.IsDeleted
                };
                (bool result, string message) = await SupplierService.Ins.EditSupplier(supplier, SelectedItem.ID);
                if (result)
                {
                    Suppliers = new ObservableCollection<SupplierDTO>(await SupplierService.Ins.GetAllSuppliers());
                    MessageBoxCustom.Show(MessageBoxCustom.Success, message);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                }
                SelectedItem = Suppliers.FirstOrDefault(x => x.ID == EditSupplier.ID);
            });

            DeleteCommand = new RelayCommand<object>(null, async o =>
            {
                if (SelectedItem == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa chọn nhà cung cấp để xóa");
                    return;
                }
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool result, string message) = await SupplierService.Ins.DeleteSupplier(SelectedItem.ID);
                    if (result)
                    {
                        Suppliers = new ObservableCollection<SupplierDTO>(await SupplierService.Ins.GetAllSuppliers());
                        MessageBoxCustom.Show(MessageBoxCustom.Success, message);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                    }
                }
            });

            OpenAddWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddSupplierWindow wd = new AddSupplierWindow();
                wd.ShowDialog();
            });

            AddCommand = new RelayCommand<Window>(p => { return true; }, async p =>
            {
                if (string.IsNullOrEmpty(SupplierName) || string.IsNullOrEmpty(SupplierPhone) || string.IsNullOrEmpty(SupplierAddress))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                    return;
                }
                else
                {
                    SupplierDTO supplier = new SupplierDTO
                    {
                        Name = SupplierName,
                        Phone = SupplierPhone,
                        Address = SupplierAddress,
                        IsDeleted = false
                    };
                    (bool result, string message) = await SupplierService.Ins.AddNewSupplier(supplier);
                    if (result)
                    {
                        p.Close();
                        resetData();
                        Suppliers = new ObservableCollection<SupplierDTO>(await SupplierService.Ins.GetAllSuppliers());
                        MessageBoxCustom.Show(MessageBoxCustom.Success, message);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, message);
                    }
                }
            });
        }

        #region methods
        void resetData()
        {
            SupplierName = null;
            SupplierPhone = null;
            SupplierAddress = null;
        }
        #endregion
    }
}
