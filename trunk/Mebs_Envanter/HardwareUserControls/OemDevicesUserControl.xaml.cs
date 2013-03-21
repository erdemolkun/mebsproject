using System;
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
using Mebs_Envanter.GUIHelpers;

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
        }

        public void SetOemDevicesInfo(ComputerInfo inf) {
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
