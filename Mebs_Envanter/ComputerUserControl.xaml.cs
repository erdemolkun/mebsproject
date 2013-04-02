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

namespace Mebs_Envanter
{
    /// <summary>
    /// Interaction logic for ComputerUserControl.xaml
    /// </summary>
    public partial class ComputerUserControl : UserControl
    {
        public ComputerUserControl()
        {
            InitializeComponent();
        }
        public void Assign_ComputerInfo_By_GUI(ComputerInfo current_Computer, ComputerInfo computerInfo, bool isEdit)
        {
            generalInfoUserControl1.SetGeneralInfo(computerInfo,current_Computer, isEdit);
            networkUserControl1.SetNetworkInfo(computerInfo.NetworkInfo);

            monitorUserControl1.SetMonitorInfo(computerInfo.MonitorInfo);
            senetInfoUserControl1.SetSenetInfo(computerInfo.Senet);

            yaziciUserControl1.SetYaziciInfo(computerInfo.YaziciInfo);

            // OEM Parçaların Bilgileri            
            oemDeviceUserControl1.SetOemDevicesInfo(computerInfo);

            if (isEdit)
            {
                ComputerInfo tempC = current_Computer;
                computerInfo.Id = tempC.Id;
                
                computerInfo.MonitorInfo.Id = tempC.MonitorInfo.Id;
                computerInfo.MonitorInfo.Mon_id = tempC.MonitorInfo.Mon_id;
                
                computerInfo.YaziciInfo.Id=tempC.YaziciInfo.Id;
                computerInfo.YaziciInfo.Yaz_id=tempC.YaziciInfo.Yaz_id;

                computerInfo.Senet.Id = tempC.Senet.Id;
                foreach (OEMDevice item in tempC.GetOemDevices())
                {
                    computerInfo.Get_OemDevice(item.DeviceType).Id = item.Id;
                }
            }
        }

        public void Init() {

            monitorUserControl1.Init();
            generalInfoUserControl1.Init();
            senetInfoUserControl1.Init();
            networkUserControl1.Init();
            yaziciUserControl1.Init();
        }

        public void SetDataContext(object context) {

            pcEnvanterTabControl.DataContext = context;
            //networkUserControl1.DataContext = (context as ComputerInfo).NetworkInfo;
        }
    }
}
