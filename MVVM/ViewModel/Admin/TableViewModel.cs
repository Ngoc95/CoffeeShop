using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.TableManagament;
using QuanLiCoffeeShop.MVVM.View.Admin.TableManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class TableViewModel : ObservableObject
    {
        private string _CbbSelectedValue;
        public string CbbSelectedValue
        {
            get { return _CbbSelectedValue; }
            set 
            {
                if (_CbbSelectedValue != value)
                {
                    _CbbSelectedValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _FilterGnereID;
        public int FilterGnereID
        {
            get { return _FilterGnereID; }
            set { _FilterGnereID = value; OnPropertyChanged(); }
        }

        private ObservableCollection<GenreTableDTO> _GenreTableList;
        public ObservableCollection<GenreTableDTO> GenreTableList 
        {
            get { return _GenreTableList; }
            set
            {
                _GenreTableList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TableDTO> _TableList;
        public ObservableCollection<TableDTO> TableList
        {
            get { return _TableList; }
            set
            {
                _TableList = value;
                OnPropertyChanged();
            }
        }

        //private GenreTableDTO _SelectedGenreTable;
        //public GenreTableDTO SelectedGenreTable
        //{
        //    get { return _SelectedGenreTable; }
        //    set 
        //    {
        //        _SelectedGenreTable = value;
        //        OnPropertyChanged();
        //    }
        //}




        #region Command
        public ICommand FirstLoadCM { get; set; }
        public ICommand OpenEditTableWDCommand { get; set; }
        public ICommand OpenDeleteTableWDCommand { get; set; }
        public ICommand FilterCommand { get; set; }

        #endregion

        public TableViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                _GenreTableList = new ObservableCollection<GenreTableDTO>(await GenreTableService.Ins.GetAllGenre());
                _GenreTableList.Insert(0, new GenreTableDTO() { GT_ID = 0, GT_NAME = "Tất cả" });
                if (_GenreTableList != null)
                {
                    GenreTableList = _GenreTableList;
                }

                _TableList = new ObservableCollection<TableDTO>(await TableService.Ins.GetAllTable());
                if (_TableList != null)
                {
                    TableList = _TableList;
                }

                _FilterGnereID = 0;
                _CbbSelectedValue = "Tất cả";
            });

            OpenEditTableWDCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                Window wd = new EditTableWindow();
                wd.ShowDialog();
            });

            OpenDeleteTableWDCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                Window wd = new AddTableWindow();
                wd.ShowDialog();
            });

            FilterCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (p != null && int.TryParse(p.ToString(), out int temp))
                    FilterGnereID = temp;
                if (_CbbSelectedValue == "Tất cả") _CbbSelectedValue = null;
                if (FilterGnereID == 0 && _CbbSelectedValue == null)
                    return;

                TableList = new ObservableCollection<TableDTO>(await TableService.Ins.FilterTableList(_FilterGnereID, _CbbSelectedValue));

            });
        }
    }
}
