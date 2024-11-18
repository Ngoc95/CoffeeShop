using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff
{
    class CustomerViewModel : ObservableObject
    {
        #region Customer Information
        private string editName;
        public string EditName
        {
            get { return editName; }
            set { editName = value; }
        }

        private string editPhone;
        public string EditPhone
        {
            get { return editPhone; }
            set { editPhone = value; }
        }

        private string editEmail;
        public string EditEmail
        {
            get { return editEmail; }
            set { editEmail = value; }
        }

        private string editGender;
        public string EditGender
        {
            get { return editGender; }
            set
            {
                editGender = value;
                OnPropertyChanged(nameof(EditGender));
            }
        }
        private decimal editPoint;
        public decimal EditPoint
        {
            get { return editPoint; }
            set
            {
                editPoint = value;
                OnPropertyChanged(nameof(EditPoint));
            }
        }
        #endregion

        public static List<CustomerDTO> cusList;
        private ObservableCollection<CustomerDTO> _customerList;
        public ObservableCollection<CustomerDTO> CustomerList
        {
            get { return _customerList; }
            set { _customerList = value; OnPropertyChanged(); }
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
                        Points = _selectedItem.Points
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

        public ICommand EditCustomerCM { get; }
        public CustomerViewModel()
        {
            CustomerList = new ObservableCollection<CustomerDTO>
            {
                new CustomerDTO { ID = "KH001", Name = "Nguyễn Văn A", Email = "vana@gmail.com", Phone = "0901234567", Gender = "Nam", Points = 120 },
                new CustomerDTO { ID = "KH002", Name = "Trần Thị B", Email = "thib@gmail.com", Phone = "0912345678", Gender = "Nữ", Points = 95 },
                new CustomerDTO { ID = "KH003", Name = "Lê Văn C", Email = "vanc@gmail.com", Phone = "0923456789", Gender = "Nam", Points = 150 },
                new CustomerDTO { ID = "KH004", Name = "Phạm Thị D", Email = "thid@gmail.com", Phone = "0934567890", Gender = "Nữ", Points = 110 },
            };
            EditCustomerCM = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                if (EditName == null || EditPhone == null || EditEmail == null || EditName == "" || EditPhone == "" || EditEmail == "" )
                {
          //          MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                }

                if (SelectedItem != null && EditCustomer != null)
                {
                    SelectedItem.Name = EditCustomer.Name;
                    SelectedItem.Email = EditCustomer.Email;
                    SelectedItem.Phone = EditCustomer.Phone;
                    SelectedItem.Gender = EditCustomer.Gender;
                    SelectedItem.Points = EditCustomer.Points;


                    // Tạo lại ObservableCollection để kích hoạt cập nhật
                    CustomerList = new ObservableCollection<CustomerDTO>(CustomerList);
                    OnPropertyChanged(nameof(CustomerList));
                }
            });
        }
    }
}
