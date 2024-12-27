using Microsoft.Win32;
using OfficeOpenXml;
using QuanLiCoffeeShop.Core;
using QuanLiCoffeeShop.DTOs;
using QuanLiCoffeeShop.MVVM.Model;
using QuanLiCoffeeShop.MVVM.Model.Services;
using QuanLiCoffeeShop.MVVM.View.Admin.MenuManagement;
using QuanLiCoffeeShop.MVVM.View.Message;
using QuanLiCoffeeShop.MVVM.View.ProductCard;
using QuanLiCoffeeShop.MVVM.ViewModel.Staff;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
//using static System.Net.Mime.MediaTypeNames;

namespace QuanLiCoffeeShop.MVVM.ViewModel.Admin
{
    class MenuViewModel : ObservableObject
    {
        #region Declare

        private ObservableCollection<ProductDTO> CoreProductList;
        private ObservableCollection<ProductDTO> _productList;

        public ObservableCollection<ProductDTO> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged(); }
        }

        //Use as Combobox ItemSource
        private List<string> _genPrdNameList;
        public List<string> GenPrdNameList
        {
            get => _genPrdNameList;
            set { _genPrdNameList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<GenreProductDTO> _GenreProductList;
        public ObservableCollection<GenreProductDTO> GenreProductList
        {
            get { return _GenreProductList; }
            set { _GenreProductList = value; OnPropertyChanged(); }
        }

        private ProductDTO _SelectedItem;
        public ProductDTO SelectedItem 
        { 
            get=> _SelectedItem; 
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged();
            } 
        }

        private string _SelectedItemGenreName;
        public string SelectedItemGenreName { get => _SelectedItemGenreName; set { _SelectedItemGenreName = value; OnPropertyChanged(); } }

        private string _SearchText = "";
        public string SearchText { get => _SearchText; set { _SearchText= value; OnPropertyChanged(); } }

        private int _FilterGnereID = 0;
        public int FilterGnereID { get => _FilterGnereID; set { _FilterGnereID = value; OnPropertyChanged(); } }

        private int IDOfNextProduct = 0;
        #endregion

        #region Command
        public ICommand FirstLoadCM { get; set; }
        public ICommand OpenAddProWDCommand { get; set; }
        public ICommand OpenEditProWDCommand { get; set; }
        public ICommand PrintMenuCommand {  get; set; }
        public ICommand SearchMenuCommand { get; set; }
        public ICommand DeleteProductCommand {  get; set; }
        public ICommand BtnEditProductDataComand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand BtnImageCommand { get; set; }
        public ICommand BtnAddProductDataComand { get; set; }

        #endregion
        public MenuViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                if(CoreProductList == null)
                {
                    CoreProductList = new ObservableCollection<ProductDTO>(await ProductService.Ins.GetAllProduct());
                    ProductList = CoreProductList;
                }

                if(GenreProductList == null)
                    GenreProductList = new ObservableCollection<GenreProductDTO>(await GenreProService.Ins.GetAllGenre());
                if (GenreProductList != null)
                {
                    if (_genPrdNameList == null)
                    {                        
                        _genPrdNameList = new List<string>();
                        foreach (var item in GenreProductList)
                        {
                            _genPrdNameList.Add(item.GP_NAME);
                        }
                        GenreProductList.Insert(0, new GenreProductDTO(0, "Tất cả"));
                        
                    }
                }
                FilterGnereID = 0;
                SearchText = "";
                FilterProduct(FilterGnereID, SearchText);
                if (IDOfNextProduct == 0)
                    IDOfNextProduct = await ProductService.Ins.IDOfProduct() + 1;
            });

            OpenAddProWDCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                _SelectedItem = new ProductDTO();
                SelectedItemGenreName = "";
                SelectedItem.PRO_ID = IDOfNextProduct;
                AddProductWindow wd = new AddProductWindow();
                wd.ShowDialog();
            });

            OpenEditProWDCommand = new RelayCommand<ProductCard>((p) => { return true; }, (p) =>
            {
                _SelectedItem = new ProductDTO(p.DataContext as ProductDTO);
                SelectedItemGenreName = GetGenreName();
                EditProductWindow wd = new EditProductWindow();
                //wd.DataContext = _SelectedItem;
                wd.ShowDialog();
            });


            DeleteProductCommand = new RelayCommand<ProductCard>((p) => { return true; }, async (p) =>
            {
                _SelectedItem = p.DataContext as ProductDTO;
                SelectedItemGenreName = GetGenreName();
                DeleteMessage wd = new DeleteMessage();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool IsDeleted, string messageDelete) = await ProductService.Ins.DeletePrdList(SelectedItem.PRO_ID);
                    if (IsDeleted)
                    {
                        DeleteProductCoreList(SelectedItem);
                        FilterProduct(FilterGnereID, SearchText);
                        MessageBoxCustom.Show(MessageBoxCustom.Success, messageDelete);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                    }
                }
            });


            BtnEditProductDataComand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, async (p) =>
            {
                _SelectedItem.GP_ID = GetGenreID();
                (bool IsAdded, string messageAdd) = await ProductService.Ins.EditPrdList(_SelectedItem, _SelectedItem.PRO_ID);
                if (IsAdded)
                {
                    p.Close();
                    UpdateProductCoreList(SelectedItem);
                    FilterProduct(FilterGnereID, SearchText);
                    MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                }
            });

            FilterCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if(p != null && int.TryParse(p.ToString(), out int temp))
                    FilterGnereID = temp;
                FilterProduct(FilterGnereID, SearchText);
            });

            SearchMenuCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                FilterProduct(FilterGnereID, SearchText);   
            });


            BtnImageCommand = new RelayCommand<Image>((p) => { return true; }, (p) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.png;*.jpeg;*.gif|All Files|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // Tạo một BitmapImage từ đường 
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad; // Để tải ảnh ngay lập tức
                        bitmap.EndInit();
                        p.Source = bitmap;

                    }
                    catch 
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Tải ảnh thất bại");
                    }
                }
            });

            BtnAddProductDataComand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                _SelectedItem.GP_ID = GetGenreID();
                if(CanAddProduct(SelectedItem))
                {
                    (bool IsAdded, string messageAdd) = await ProductService.Ins.AddPrdList(SelectedItem);
                    if (IsAdded)
                    {
                        p.Close();
                        MessageBoxCustom.Show(MessageBoxCustom.Success, messageAdd);
                        AddProductCoreList(SelectedItem);
                        FilterProduct(FilterGnereID, SearchText);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                    }
                }
            });


            PrintMenuCommand = new RelayCommand<Window>((p) => { return true; },(p) =>
            {
                ExportMenuExcel();
            });
        }

        private bool CanAddProduct(ProductDTO selectedItem)
        {
            foreach(ProductDTO item in CoreProductList)
            {
                if(item.PRO_NAME == selectedItem.PRO_NAME)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Đã tồn tại tên sản phẩm này");
                    return false;
                }
            }
            return true;
        }

        private void ExportMenuExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Chọn nơi để lưu file",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = "MenuExport.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Menu");

                        worksheet.Cells["A1:F2"].Merge = true; 
                        worksheet.Cells["A1"].Value = "Danh sách sản phẩm";
                        worksheet.Cells["A1"].Style.Font.Size = 16; 
                        worksheet.Cells["A1"].Style.Font.Bold = true; 
                        worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        string[] headers = new string[] { "Mã sản phẩm", "Tên sản phẩm", "Loại sản phẩm", "Hình ảnh", "Mô tả", "Giá sản phẩm (VNĐ)"};

                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cells[3, i + 1].Value = headers[i];
                            worksheet.Cells[3, i + 1].Style.Font.Bold = true; // In đậm tiêu đề
                            worksheet.Cells[3, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[3, i + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        }
                        for (int i = 0; i < CoreProductList.Count; i++)
                        {
                            worksheet.Cells[i + 4, 1].Value = CoreProductList[i].PRO_ID;
                            worksheet.Cells[i + 4, 2].Value = CoreProductList[i].PRO_NAME;
                            worksheet.Cells[i + 4, 3].Value = GetGenrePrdName(CoreProductList[i].GP_ID);
                            byte[] imageBytes;
                            try
                            {
                                var uri = new Uri(CoreProductList[i].PRO_IMG);
                                var streamInfo = System.Windows.Application.GetResourceStream(uri);

                                using (var memoryStream = new MemoryStream())
                                {
                                    streamInfo.Stream.CopyTo(memoryStream);
                                    imageBytes = memoryStream.ToArray(); // Chuyển thành byte[]
                                }

                                // Thêm ảnh vào Excel
                                using (var imgStream = new MemoryStream(imageBytes))
                                {
                                    var excelImage = worksheet.Drawings.AddPicture("Image_" + i, imgStream);
                                    // Điều chỉnh kích thước ảnh sao cho vừa với ô
                                    excelImage.SetPosition(i + 3, 3, 3, 4); // Đặt ảnh vào cột 4, dòng i+4
                                    excelImage.SetSize(100, 100); // Điều chỉnh ảnh theo tỷ lệ
                                    worksheet.Row(i + 4).Height = 80;
                                }
                            }
                            catch
                            {

                            }
                            worksheet.Cells[i + 4, 5].Value = CoreProductList[i].PRO_DESCRIPTION;
                            worksheet.Cells[i + 4, 6].Value = CoreProductList[i].PRO_PRICE;
                        }
                        worksheet.Cells.AutoFitColumns();
                        worksheet.Column(4).Width = 15;



                        // Vẽ border cho tất cả các ô chứa dữ liệu
                        var totalRows = CoreProductList.Count + 3; // Bao gồm header và dữ liệu
                        var totalColumns = headers.Length;
                        var dataRange = worksheet.Cells[3, 1, totalRows, totalColumns];
                        dataRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        // Lưu file Excel
                        File.WriteAllBytes(filePath, package.GetAsByteArray());
                    }
                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Xuất file excel thành công");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xuất file excel thất bại");
            }
            //return Task.CompletedTask;
        }

        private string GetGenrePrdName(int? gP_ID)
        {
            foreach(GenreProductDTO i in GenreProductList)
            {
                if(i.GP_ID == gP_ID)
                    return i.GP_NAME;
            }
            return "";
        }

        private void AddProductCoreList(ProductDTO selectedItem)
        {
            if (selectedItem.PRO_IMG == null)
                selectedItem.PRO_IMG = "pack://application:,,,/Images/MenuAndError/UploadImg.jpg";
            CoreProductList.Add(selectedItem);
            ProductList = new ObservableCollection<ProductDTO>(CoreProductList);    
            IDOfNextProduct++;
        }

        private void UpdateProductCoreList(ProductDTO selectedItem)
        {
            foreach(ProductDTO product in CoreProductList)
            {
                if(product.PRO_ID == selectedItem.PRO_ID)
                {
                    product.PRO_NAME = selectedItem.PRO_NAME;
                    product.PRO_PRICE = selectedItem.PRO_PRICE;
                    product.GP_ID = selectedItem.GP_ID;
                    product.PRO_IMG = selectedItem.PRO_IMG;
                    product.PRO_DESCRIPTION = selectedItem.PRO_DESCRIPTION;
                    ProductList = new ObservableCollection<ProductDTO>(CoreProductList);
                    return;
                }
            }
        }

        private void DeleteProductCoreList(ProductDTO selectedItem)
        {
            for (int i = 0; i < CoreProductList.Count; i++)
            {
                if(CoreProductList[i].PRO_ID == selectedItem.PRO_ID)
                {
                    CoreProductList.RemoveAt(i);
                    ProductList = new ObservableCollection<ProductDTO>(CoreProductList);
                    return;
                }
            }
        }

        public string GetGenreName()
        {
            foreach(GenreProductDTO item in GenreProductList)
            {
                if(SelectedItem.GP_ID == item.GP_ID)
                {
                    return item.GP_NAME;
                }
            }
            return null;
        }

        public int GetGenreID()
        {
            foreach (GenreProductDTO item in GenreProductList)
            {
                if (SelectedItemGenreName == item.GP_NAME)
                {
                    return item.GP_ID;
                }
            }
            return 0;
        }

        private void FilterProduct(int filterGenID, string searchtxt)
        {
            ProductList = new ObservableCollection<ProductDTO>();
            if (filterGenID != 0 && searchtxt != "")
            {
                foreach (ProductDTO product in CoreProductList)
                {
                    if (product.PRO_NAME.ToLower().Contains(searchtxt.ToLower()) && product.GP_ID == filterGenID)
                    {
                        ProductList.Add(product);
                    }
                }
            }
            else if (searchtxt == "" && filterGenID != 0)
            {
                foreach (ProductDTO product in CoreProductList)
                {
                    if (product.GP_ID == filterGenID)
                        ProductList.Add(product);
                }
            }
            else if (filterGenID == 0 && searchtxt.Length != 0)
            {
                foreach (ProductDTO product in CoreProductList)
                {
                    if (product.PRO_NAME.ToLower().Contains(searchtxt.ToLower()))
                    {
                        ProductList.Add(product);
                    }
                }
            }
            else
            {
                ProductList = new ObservableCollection<ProductDTO>(CoreProductList);
            }
        }


    }
}
