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
using Mebs_Envanter;
using Mebs_Envanter.GUIHelpers;
using System.Windows.Controls.Primitives;

namespace Mebs_Envanter.HardwareUserControls
{
    /// <summary>
    /// Interaction logic for OemDevicesUserControl.xaml
    /// </summary>
    public partial class OemDevicesUserControl : UserControl
    {
        public OemDevicesUserControl()
        {
            InitializeComponent();
            //OemDevicesViewModel oemDevicesViewModel = new OemDevicesViewModel();
            //this.DataContext = oemDevicesViewModel;
           // DataContextChanged += new DependencyPropertyChangedEventHandler(OemDevicesUserControl_DataContextChanged);
        }

        void OemDevicesUserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IItemContainerGenerator generator = hardwareItemsControl.ItemContainerGenerator;
            GeneratorPosition position = generator.GeneratorPositionFromIndex(0);
            using (generator.StartAt(position, GeneratorDirection.Forward, true))
            {
                foreach (object o in hardwareItemsControl.Items)
                {
                    DependencyObject dp = generator.GenerateNext();
                    generator.PrepareItemContainer(dp);
                }
            }            
        }

        public void SetOemDevicesInfo(ComputerInfo inf) {

            //hardwareExtraItemsControl.UpdateLayout();
            // OEM Parçaların Bilgileri
            AssignOemDeviceInfoByGui(hardwareItemsControl, inf);
            AssignOemDeviceInfoByGui(hardwareExtraItemsControl, inf);        
        }
        
        private void AssignOemDeviceInfoByGui(ItemsControl control, ComputerInfo freshComputerInfo)
        {
            foreach (var item in control.ItemsSource)
            {
                OemDeviceViewModel oemDevViewModelOfGui = item as OemDeviceViewModel;
                OEMDevice devOem = freshComputerInfo.Get_OemDevice((oemDevViewModelOfGui.DevOem).DeviceType);
                devOem.shouldUpdate = false;
                DependencyObject dp = control.ItemContainerGenerator.ContainerFromItem(item) as DependencyObject;         
                
                if (dp != null)
                {
                    DeviceEntryControl devControl = VisualHelperWPF.FindVisualChildByType<DeviceEntryControl>(dp);                                        
                    if (devOem != null)
                    {
                        if (devControl != null)
                        {
                            devControl.SetOemDeviceProperties(devOem);
                            devOem.shouldUpdate = true;
                        }
                        else {
                            
                        }
                    }
                }
                else
                {
                    freshComputerInfo.OemDevicesVModel.AssignOemDevice(oemDevViewModelOfGui.DevOem);
                    // Arayüzde hardwareItemsControl için ContentPresenter'lar oluşmamış olabilir.
                    // ComputerInfo'daki OemDevice Değişkenleri olduğu gibi kalsın.
                }
            }
        }
    }
}
