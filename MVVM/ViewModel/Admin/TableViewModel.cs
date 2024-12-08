using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.TableManagament;
using QuanLiCoffeeShop.MVVM.View.Admin.TableManagement;
using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class TableViewModel : ObservableObject
    {
        private int _CbbSelectedIndex = 0;
        public int CbbSelectedIndex
        {
            get { return _CbbSelectedIndex; }
            set 
            {
                if (_CbbSelectedIndex != value)
                {
                    _CbbSelectedIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _FilterGnereID = 0;
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

        private TableDTO _SelectedTable;
        public TableDTO SelectedTable
        {
            get { return _SelectedTable; }
            set { _SelectedTable = value; OnPropertyChanged(); }
        }

        private List<string> _GenreTableNameList = new List<string>();
        public List<string> GenreTableNameList
        {
            get { return _GenreTableNameList; }
            set { _GenreTableNameList = value; OnPropertyChanged(); }
        }

        private string _SelectedTableGenreName;
        public string SelectedTableGenreName
        {
            get { return _SelectedTableGenreName; }
            set { _SelectedTableGenreName = value; OnPropertyChanged(); }
        }

        private List<string> _TableStatusList;
        public List<string> TableStatusList
        {
            get { return _TableStatusList; }
            set { _TableStatusList = value; OnPropertyChanged(); }
        }

        private int _IDOfNextTable = 0;
        public int IDOfNextTable
        {
            get { return _IDOfNextTable; }
            set
            {
                if (_IDOfNextTable != value)
                {
                    _IDOfNextTable = value;
                    OnPropertyChanged();
                }
            }
        }



        #region Command
        public ICommand FirstLoadCM { get; set; }
        public ICommand OpenEditTableWDCommand { get; set; }
        public ICommand OpenDeleteTableWDCommand { get; set; }
        public ICommand OpenAddWDCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand btnCloseTableCommand { get; set; }
        public ICommand btnEditTableCommand { get; set; }
        public ICommand btnAddTableCommand { get; set; }

        #endregion

        public TableViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                if(GenreTableList == null)
                {
                    GenreTableList = new ObservableCollection<GenreTableDTO>(await GenreTableService.Ins.GetAllGenre());
                    foreach(var item in GenreTableList)
                    {
                        GenreTableNameList.Add(item.GT_NAME);
                    }
                    GenreTableList.Insert(0, new GenreTableDTO() { GT_ID = 0, GT_NAME = "Tất cả" });
                }

                if (_TableList == null)
                {
                    TableList = new ObservableCollection<TableDTO>(await TableService.Ins.GetAllTable());
                }
                if(TableStatusList == null)
                    TableStatusList = new List<string>() { "Còn trống", "Đang bận", "Đang sửa chữa" };
            });

            OpenEditTableWDCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                _SelectedTable = new TableDTO(p.DataContext as TableDTO);
                SelectedTableGenreName = GetGenreSelectedTableGenreName();
                Window wd = new EditTableWindow();
                wd.ShowDialog();
            });

            OpenAddWDCommand = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                IDOfNextTable = await TableService.Ins.NumOfTable() + 1;
                _SelectedTable = new TableDTO();
                SelectedTableGenreName = null;
                Window wd = new AddTableWindow();
                wd.ShowDialog();
            });

            OpenDeleteTableWDCommand = new RelayCommand<UserControl>((p) => { return true; }, async (p) =>
            {
                SelectedTable = p.DataContext as TableDTO;
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool IsDeleted, string messageDelete) = await TableService.Ins.DeleteTableList(SelectedTable.TB_ID);
                    if (IsDeleted)
                    {
                        string text;
                        if (_CbbSelectedIndex == 0) text = null;
                        else if (_CbbSelectedIndex == 1) text = "Còn trống";
                        else if (_CbbSelectedIndex == 2) text = "Đang bận";
                        else text = "Đang sửa chữa";
                        TableList = new ObservableCollection<TableDTO>(await TableService.Ins.FilterTableList(FilterGnereID, text));
                        MessageBoxCustom.Show(MessageBoxCustom.Success, messageDelete);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                    }
                }
            });

            FilterCommand = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (p != null && int.TryParse(p.ToString(), out int temp))
                    FilterGnereID = temp;
                string text;
                if (_CbbSelectedIndex == 0) text = null;
                else if (_CbbSelectedIndex == 1) text = "Còn trống";
                else if (_CbbSelectedIndex == 2) text = "Đang bận";
                else text = "Đang sửa chữa";

                if (FilterGnereID == 0 && text == null)
                {
                    TableList = new ObservableCollection<TableDTO>(await TableService.Ins.GetAllTable());
                    return;
                }

                TableList = new ObservableCollection<TableDTO>(await TableService.Ins.FilterTableList(_FilterGnereID, text));

            });

            btnCloseTableCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
                SelectedTable = null; //co the bug
            });

            btnEditTableCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                SelectedTable.GT_ID = GetGenreSelectedTableGenreID();
                (bool IsAdded, string messageAdd) = await TableService.Ins.EditTableList(SelectedTable);
                if (IsAdded)
                {
                    p.Close();
                    string text;
                    if (_CbbSelectedIndex == 0) text = null;
                    else if (_CbbSelectedIndex == 1) text = "Còn trống";
                    else if (_CbbSelectedIndex == 2) text = "Đang bận";
                    else text = "Đang sửa chữa";
                    TableList = new ObservableCollection<TableDTO>(await TableService.Ins.FilterTableList(FilterGnereID, text));
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }

            });

            btnAddTableCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                SelectedTable.GT_ID = GetGenreSelectedTableGenreID();
                (bool IsAdded, string messageAdd) = await TableService.Ins.AddTableList(SelectedTable);
                if (IsAdded)
                {
                    p.Close();
                    string text;
                    if (_CbbSelectedIndex == 0) text = null;
                    else if (_CbbSelectedIndex == 1) text = "Còn trống";
                    else if (_CbbSelectedIndex == 2) text = "Đang bận";
                    else text = "Đang sửa chữa";
                    TableList = new ObservableCollection<TableDTO>(await TableService.Ins.FilterTableList(FilterGnereID, text));
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }

            });
        }


        string GetGenreSelectedTableGenreName()
        {
            foreach(var item in GenreTableList)
            {
                if(item.GT_ID == SelectedTable.GT_ID)
                {
                    return item.GT_NAME;
                }
            }
            return null;
        }

        int GetGenreSelectedTableGenreID()
        {
            foreach(var item in GenreTableList)
            {
                if(SelectedTableGenreName ==  item.GT_NAME)
                    return item.GT_ID;
            }
            return 0;
        }

    }
}
