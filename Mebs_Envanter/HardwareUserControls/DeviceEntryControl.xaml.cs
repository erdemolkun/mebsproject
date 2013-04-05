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

namespace MEBS_Envanter
{
    /// <summary>
    /// Interaction logic for DeviceEntryControl.xaml
    /// </summary>
    public partial class DeviceEntryControl : UserControl
    {
        public DeviceEntryControl()
        {
            InitializeComponent();
        }
        public void SetOemDeviceProperties(OEMDevice device) {

            device.SerialNumber = serialNumberTextBox.Text.ToString();
            device.DeviceInfo = deviceInfoTextBox.Text.ToString();
            device.Adet = numericTextBoxAdet.Value;
        }
    }
}
