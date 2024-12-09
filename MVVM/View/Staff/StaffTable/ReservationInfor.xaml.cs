﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLiCoffeeShop.MVVM.View.Staff.StaffTable
{
    /// <summary>
    /// Interaction logic for ReservationInfor.xaml
    /// </summary>
    public partial class ReservationInfor : Window
    {
        public ReservationInfor()
        {
            InitializeComponent();
            Loaded += ReservationInfor_Loaded;
        }

        private void ReservationInfor_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime a = new DateTime(int.Parse(YearRes.Text), int.Parse(MonthRes.Text), int.Parse(DayRes.Text));
            if (a.Date < DateTime.Now.Date)
            {
                CheckInBtn.Visibility = Visibility.Hidden;
                Savebtn.Visibility = Visibility.Hidden;
                TxtNumPP.IsEnabled = false;
                TxtTB_id.IsEnabled = false;
                TxtTimeRes.IsEnabled = false;
                DayRes.IsEnabled = false;
                MonthRes.IsEnabled = false;
                YearRes.IsEnabled = false;
                TxtReqRes.IsEnabled = false;
            }
            else if (a.Date > DateTime.Now.Date)
            {
                CheckInBtn.Visibility = Visibility.Hidden;
                Savebtn.Visibility = Visibility.Visible;
            }
            else
            {
                if (status.Text == "Khách đã nhận bàn")
                {
                    CheckInBtn.Content = "Hủy trạng thái đã xác nhận";
                }
                else
                {
                    CheckInBtn.Content = "CheckIn";
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
            
        }
    }
}
