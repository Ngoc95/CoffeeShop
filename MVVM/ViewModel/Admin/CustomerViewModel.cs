using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.View.Admin.CustomerManagement;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class CustomerViewModel : ObservableObject
    {
        #region Customer Information

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private bool isMale;
        public bool IsMale
        {
            get => isMale;
            set
            {
                if (isMale != value)
                {
                    isMale = value;
                    OnPropertyChanged(nameof(IsMale));
                    UpdateGender();
                }
            }
        }
        private void UpdateGender()
        {
            Gender = IsMale ? "Nam" : "Nữ";
        }
        private string gender;
        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
        private decimal point;
        public decimal Point
        {
            get { return point; }
            set
            {
                point = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public static List<CustomerDTO> cusList;
        private ObservableCollection<CustomerDTO> _customerList;
        public ObservableCollection<CustomerDTO> CustomerList
        {
            get { return _customerList; }
            set
            {
                _customerList = value;
                OnPropertyChanged(nameof(CustomerList));
            }
        }
        private CustomerDTO _selectedItem;
        public CustomerDTO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    // Tạo bản sao của SelectedCustomer để chỉnh sửa
                    EditCustomer = new CustomerDTO
                    {
                        ID = _selectedItem.ID,
                        Name = _selectedItem.Name,
                        Email = _selectedItem.Email,
                        Phone = _selectedItem.Phone,
                        Gender = _selectedItem.Gender,
                        Point = _selectedItem.Point
                    };
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        private CustomerDTO _editCustomer;
        public CustomerDTO EditCustomer
        {
            get => _editCustomer;
            set
            {
                _editCustomer = value;
                OnPropertyChanged(nameof(EditCustomer));
            }
        }
        public ICommand FirstLoadCM { get; set; }
        public ICommand SearchCusCM { get; set; }
        public ICommand EditCusCM { get; }
        public ICommand AddCusWdCM { get; set; }
        public ICommand AddCusListCM { get; set; }
        public ICommand DeleteCusListCM { get; set; }
        public CustomerViewModel()
        {
            FirstLoadCM = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                CustomerList = new ObservableCollection<CustomerDTO>(await Task.Run(() => CustomerService.Ins.GetAllCus()));
                if (CustomerList != null)
                    cusList = new List<CustomerDTO>(CustomerList);
            });
            SearchCusCM = new RelayCommand<TextBox>((p) => { return true; }, async (p) =>
            {
                if (p == null || string.IsNullOrWhiteSpace(p.Text))
                {
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCus());
                    return;
                }
                string searchText = p.Text.ToLower();

                // Tìm kiếm dựa trên "KH" + ID hoặc các thông tin khác
                CustomerList = new ObservableCollection<CustomerDTO>(
                    cusList.FindAll(x =>
                        ($"kh{x.ID:D3}".ToLower().Contains(searchText)) ||
                        x.Name.ToLower().Contains(searchText) ||
                        x.Phone.ToLower().Contains(searchText) ||
                        x.Email.ToLower().Contains(searchText) ||
                        x.ID.ToString().Contains(searchText)
                    ));

            });

            EditCusCM = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                if (EditCustomer.Name == null || EditCustomer.Phone == null || EditCustomer.Email == null || EditCustomer.Name == "" || EditCustomer.Phone == "" || EditCustomer.Email == "")
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                    return;
                }
                CUSTOMER newCus = new CUSTOMER
                {
                    CUS_ID = SelectedItem.ID,
                    CUS_NAME = EditCustomer.Name,
                    CUS_GENDER = EditCustomer.Gender,
                    CUS_PHONE = EditCustomer.Phone,
                    CUS_EMAIL = EditCustomer.Email,
                    CUS_POINT = EditCustomer.Point,
                    IS_DELETED = false,
                };
                (bool success, string messageEdit) = await CustomerService.Ins.EditCusList(newCus, SelectedItem.ID);
                if (success)
                {
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCus());
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageEdit);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageEdit);
                }
            });

            AddCusWdCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddCustomerWindow wd = new AddCustomerWindow();
                wd.ShowDialog();
            });

            AddCusListCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                CUSTOMER newCus = new CUSTOMER
                {
                    CUS_NAME = this.Name,
                    CUS_GENDER = this.Gender,
                    CUS_PHONE = this.Phone,
                    CUS_EMAIL = this.Email,
                    CUS_POINT = 0,
                    IS_DELETED = false,
                };
                (bool IsAdded, string messageAdd) = await CustomerService.Ins.AddNewCus(newCus);
                if (IsAdded)
                {
                    p.Close();
                    resetData();
                    CustomerList = new ObservableCollection<CustomerDTO>(await CustomerService.Ins.GetAllCus());
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }
            });
            DeleteCusListCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if(SelectedItem == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa chọn khách hàng để xóa");
                    return;
                }
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool sucess, string messageDelete) = await CustomerService.Ins.DeleteCustomer(SelectedItem.ID);
                    if (sucess)
                    {
                        CustomerList.Remove(SelectedItem);
                        MessageBoxCustom.Show(MessageBoxCustom.Success, messageDelete);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                    }    
                }
            });
        }
        #region methods
        void resetData()
        {
            Name = null;
            Email = null;
            Phone = null;
        }
        #endregion

    }
}
