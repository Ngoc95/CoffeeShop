using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using QuanLiCoffeeShop.MVVM.View.Admin.ErrorManagement;
using QuanLiCoffeeShop.MVVM.Model;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
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
                    //tạo bản sao để edit
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
        public ICommand EditErrorCM { get; set; }
        public ICommand DeleteErrorCM { get; set; }

        public ErrorViewModel()
        {
            FirstLoadCM = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                ErrorList = new ObservableCollection<ErrorDTO>(await Task.Run(() => ErrorService.Ins.GetAllError()));
                if (ErrorList != null)
                    erList = new List<ErrorDTO>(ErrorList);
            });
            SearchErrorCM = new RelayCommand<TextBox>(p => true, async (p) =>
            {
                string searchText = p?.Text ?? string.Empty;
                await SearchAndFilterErrors(searchText, SelectedStatus);
            });


            FilterErrorCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                await FilterErrorList(SelectedStatus);
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
                    await FilterErrorList(SelectedStatus);
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }
            });

            EditErrorCM = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(EditError.ER_NAME))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                    return;
                }
                ERROR newErr = new ERROR
                {
                    ER_ID = SelectedItem.ER_ID,
                    ER_NAME = EditError.ER_NAME,
                    ER_DESCRIPTION = EditError.ER_DESCRIPTION,
                    ER_STATUS = EditError.ER_STATUS,
                    IS_DELETED = false,
                };
                (bool success, string messageEdit) = await ErrorService.Ins.EditErrorList(newErr, SelectedItem.ER_ID);
                if (success)
                {
                    ErrorList = new ObservableCollection<ErrorDTO>(await ErrorService.Ins.GetAllError());
                    await FilterErrorList(SelectedStatus);
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageEdit);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageEdit);
                }
            });

            DeleteErrorCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (SelectedItem == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa chọn sự cố để xóa");
                    return;
                }
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool sucess, string messageDelete) = await ErrorService.Ins.DeleteError(SelectedItem.ER_ID);
                    if (sucess)
                    {
                        ErrorList.Remove(SelectedItem);
                        MessageBoxCustom.Show(MessageBoxCustom.Success, messageDelete);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                    }
                }
            });
        }


        private async Task SearchAndFilterErrors(string searchText, string filterStatus)
        {
            searchText = searchText?.ToLower() ?? string.Empty;
            filterStatus = filterStatus?.ToLower() ?? string.Empty;

            var allErrors = await ErrorService.Ins.GetAllError();

            // Nếu cả filter và search text đều trống
            if (string.IsNullOrWhiteSpace(searchText) && string.IsNullOrWhiteSpace(filterStatus))
            {
                ErrorList = new ObservableCollection<ErrorDTO>(allErrors);
                return;
            }

            // Tìm kiếm và lọc
            ErrorList = new ObservableCollection<ErrorDTO>(
                allErrors.FindAll(x =>
                    (string.IsNullOrEmpty(filterStatus) ||
                        (x.ER_STATUS?.ToLower().Contains(filterStatus) ?? false)) &&
                    (string.IsNullOrEmpty(searchText) ||
                        ($"er{x.ER_ID:D3}".ToLower().Contains(searchText)) ||
                        (x.ER_NAME?.ToLower().Contains(searchText) ?? false) ||
                        (!string.IsNullOrEmpty(x.ER_DESCRIPTION) && x.ER_DESCRIPTION.ToLower().Contains(searchText)) ||
                        x.ER_ID.ToString().Contains(searchText))
                )
            );
        }

        private async Task FilterErrorList(string selectedStatus)
        {
            if (string.IsNullOrWhiteSpace(selectedStatus))
            {
                ErrorList = new ObservableCollection<ErrorDTO>(await ErrorService.Ins.GetAllError());
                return;
            }

            string searchText = selectedStatus.ToLower();

            ErrorList = new ObservableCollection<ErrorDTO>(
                (await ErrorService.Ins.GetAllError()).FindAll(x =>
                    (x.ER_STATUS?.ToLower().Contains(searchText) ?? false)
                ));
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

