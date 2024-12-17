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
    /// Interaction logic for CusForReservationWindow.xaml
    /// </summary>
    public partial class CusForReservationWindow : Window
    {
        public CusForReservationWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }
    }

}
