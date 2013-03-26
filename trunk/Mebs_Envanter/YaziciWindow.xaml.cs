﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mebs_Envanter.Hardware;

namespace Mebs_Envanter
{
    /// <summary>
    /// Interaction logic for YaziciWindow.xaml
    /// </summary>
    public partial class YaziciWindow : Window
    {
        public YaziciWindow()
        {
            InitializeComponent();
            LoadItems();

            YaziciInfo x = new YaziciInfo();
            x.YaziciModeli = "asdz1";
            x.NetworkInfo.IpAddress = "asdz2";
            x.SenetInfo.Alan_kisi_isim = "asdz3";

            gridYaziciBilgileri.DataContext
                 = x;

        }

        private void LoadItems() {

            yaziciUserControl1.Init();
            senetInfoControl1.Init();
            networkInfoControl1.Init();


            
        }
        private bool AddOrEditPCFunction(bool isEdit)
        {
            YaziciInfo infYazici = new YaziciInfo();

            YaziciInfo currentYazidi = yaziciList.SelectedItem as YaziciInfo;
            if (currentYazidi != null) {
                


            
            }
            return true;
        
        }

        private void AssignYaziciInfo(YaziciInfo current,YaziciInfo toAssign,bool isEdit) { 
        

            

            
        }


        private void yaziciList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void yaziciAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void yaziciEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void yaziciDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refreshListBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
