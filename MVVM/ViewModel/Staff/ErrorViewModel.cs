using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.View.Staff.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shell;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Staff
{
    class ErrorViewModel : ObservableObject
    {
        #region Error Information

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        #endregion
        public static List<ErrorDTO> erList;
        private ObservableCollection<ErrorDTO> _errorList;
        public ObservableCollection<ErrorDTO> ErrorList
        {
            get { return _errorList; }
            set
            {
                _errorList = value;
                OnPropertyChanged(nameof(ErrorList));
            }
        }
        private ErrorDTO _selectedItem;
        public ErrorDTO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    EditError = new ErrorDTO
                    {
                        ER_ID = _selectedItem.ER_ID,
                        ER_NAME = _selectedItem.ER_NAME,
                        ER_STATUS = _selectedItem.ER_STATUS,
                        ER_DESCRIPTION = _selectedItem.ER_DESCRIPTION,
                    };
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        public ObservableCollection<string> StatusList { get; set; } = new ObservableCollection<string>
        {
            "Chưa khắc phục",
            "Đã khắc phục"
        };
        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); 
                if (FilterErrorCM.CanExecute(null))
                    FilterErrorCM.Execute(null);
            }
        }
        private ErrorDTO _editError;
        public ErrorDTO EditError
        {
            get => _editError;
            set
            {
                _editError = value;
                OnPropertyChanged(nameof(EditError));
            }
        }
        public ICommand FirstLoadCM { get; set; }
        public ICommand SearchErrorCM { get; set; }
        public ICommand FilterErrorCM { get; set; }
        public ICommand AddErrorWdCM { get; set; }
        public ICommand AddErrorListCM { get; set; }
       
        public ErrorViewModel()
        {
            FirstLoadCM = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                ErrorList = new ObservableCollection<ErrorDTO>(await Task.Run(() => ErrorService.Ins.GetAllError()));
                if (ErrorList != null)
                    erList = new List<ErrorDTO>(ErrorList);
            });
            SearchErrorCM = new RelayCommand<TextBox>((p) => { return true; }, async (p) =>
            {
                string searchText = p.Text?.ToLower() ?? string.Empty;
                string filterStatus = SelectedStatus?.ToLower() ?? string.Empty;
                if (p == null || string.IsNullOrWhiteSpace(p.Text))
                {
                    if (string.IsNullOrWhiteSpace(filterStatus))
                        ErrorList = new ObservableCollection<ErrorDTO>(await ErrorService.Ins.GetAllError());
                    else
                    {
                        ErrorList = new ObservableCollection<ErrorDTO>((await ErrorService.Ins.GetAllError()).FindAll(x =>
                                                   (string.IsNullOrEmpty(filterStatus) ||
                                                   (x.ER_STATUS?.ToLower().Contains(filterStatus) ?? false))));
                    }
                    return;
                }

                // Tìm kiếm dựa trên ID, tên và filter của combobox
                ErrorList = new ObservableCollection<ErrorDTO>(
                     (await ErrorService.Ins.GetAllError()).FindAll(x =>
                        (string.IsNullOrEmpty(filterStatus) ||
                            (x.ER_STATUS?.ToLower().Contains(filterStatus) ?? false)) &&
                        (string.IsNullOrEmpty(searchText) ||
                            ($"er{x.ER_ID:D3}".ToLower().Contains(searchText)) ||
                            (x.ER_NAME?.ToLower().Contains(searchText) ?? false) ||
                            (!string.IsNullOrEmpty(x.ER_DESCRIPTION) && x.ER_DESCRIPTION.ToLower().Contains(searchText)) ||
                            x.ER_ID.ToString().Contains(searchText))
                    ));

            });

            FilterErrorCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrWhiteSpace(SelectedStatus))
                {
                    ErrorList = new ObservableCollection<ErrorDTO>(await ErrorService.Ins.GetAllError());
                    return;
                }

                string searchText = SelectedStatus.ToLower();

                ErrorList = new ObservableCollection<ErrorDTO>(
                    (await ErrorService.Ins.GetAllError()).FindAll(x =>
                        (x.ER_STATUS?.ToLower().Contains(searchText) ?? false)
                    ));
            });

            AddErrorWdCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddErrorWindow wd = new AddErrorWindow();
                wd.ShowDialog();
            });

            AddErrorListCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(Name))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa nhập tên sự cố");
                    return;
                }
                ERROR newErr = new ERROR
                {
                    ER_NAME = this.Name,
                    ER_STATUS = "Chưa khắc phục",
                    ER_DESCRIPTION = this.Description,
                    IS_DELETED = false,
                };
                (bool IsAdded, string messageAdd) = await ErrorService.Ins.AddNewError(newErr);
                if (IsAdded)
                {
                    p.Close();
                    resetData();
                    ErrorList = new ObservableCollection<ErrorDTO>(await ErrorService.Ins.GetAllError());
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }
            });
        }
        #region methods
        void resetData()
        {
            Name = null;
            Description = null;
        }
        #endregion


    }
}

