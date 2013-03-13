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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MEBS_Envanter;
using MEBS_Envanter.GeneralObjects;

namespace Mebs_Envanter.HardwareUserControls
{
    /// <summary>
    /// Interaction logic for NetworkInfoUserControl.xaml
    /// </summary>
    public partial class NetworkInfoUserControl : UserControl
    {
        public NetworkInfoUserControl()
        {
            InitializeComponent();
        }
        public void SetNetworkInfo(NetworkInfo inf) {
            inf.MacAddressString = pcMacAddressTextBox.Text.Trim().ToString();
            inf.BagliAg = bagliAgCombo.SelectedItem as BagliAg;
            
        }
        public void Init() {


            // Bağlı Olduğu Ağlar arayüze atanıyor
            BagliAgRepository repoBagliAglar = new BagliAgRepository();
            repoBagliAglar.Fill_Aglar();
            //bagliAgCombo.DataContext = repoBagliAglar;
            bagliAgCombo.ItemsSource = repoBagliAglar.BagliAglar;
            BagliAgRepository.INSTANCE = repoBagliAglar;
        }
    }
}