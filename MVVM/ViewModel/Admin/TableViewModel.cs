using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class TableViewModel : ObservableObject
    {
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



        #region Command
        public ICommand FirstLoadCM { get; set; }

        #endregion

        public TableViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                _GenreTableList = new ObservableCollection<GenreTableDTO>(await GenreTableService.Ins.GetAllGenre());
                _GenreTableList.Insert(0, new GenreTableDTO() { GT_ID = 0, GT_NAME = "Tất cả"} );
                if(_GenreTableList != null)
                {
                    GenreTableList = _GenreTableList;
                }

                _TableList = new ObservableCollection<TableDTO>(await TableService.Ins.GetAllTable());
                if(_TableList != null)
                {
                    TableList = _TableList;
                }
            });
        }
    }

    //public class MyDataTemplateSelector : DataTemplateSelector
    //{

    //}
}
