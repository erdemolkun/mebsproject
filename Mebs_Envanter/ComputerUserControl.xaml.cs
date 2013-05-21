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
using Mebs_Envanter;

namespace Mebs_Envanter
{
    /// <summary>
    /// Interaction logic for ComputerUserControl.xaml
    /// </summary>
    public partial class ComputerUserControl : ComputerInfoUserControlBase
    {
        public ComputerUserControl()
        {
            InitializeComponent();
        }
        public override void SetFocus(int tabIndex)
        {
            pcEnvanterTabControl.SelectedIndex = 0;
        }

        private int GetCorrespondingTabItemIndex(int desiredIndex)
        {
            int currentIndex = 1;
            int ienumarableIndex = 1;
            foreach (TabItem item in pcEnvanterTabControl.Items)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    if (desiredIndex == currentIndex)
                    {
                        return ienumarableIndex;
                    }
                    currentIndex++;
                }
                ienumarableIndex++;
            }
            return -1;
        }

        public override void KeyEventResponder(KeyEventArgs e)
        {

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                try
                {
                    String str = e.Key.ToString();
                    if (str.StartsWith("D"))
                    {

                        str = str.Substring(1);
                        int index = Convert.ToInt32(str);
                        int newIndex = GetCorrespondingTabItemIndex(index);
                        if (newIndex > 0)
                        {
                            pcEnvanterTabControl.SelectedIndex = newIndex - 1;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            KeyEventResponder(e);
            base.OnKeyDown(e);
        }

        public override void Assign_ComputerInfo_By_GUI(ComputerInfo current_Computer, ComputerInfo toUpdateComputer, bool isEdit)
        {
            generalInfoUserControl1.SetGeneralInfo(toUpdateComputer, current_Computer, isEdit);
            networkUserControl1.SetNetworkInfo(toUpdateComputer.NetworkInfo);

            monitorUserControl1.SetMonitorInfo(toUpdateComputer.MonitorInfo);
            senetInfoUserControl1.SetSenetInfo(toUpdateComputer.Senet);

            // OEM Parçaların Bilgileri            
            oemDeviceUserControl1.SetOemDevicesInfo(toUpdateComputer);

            if (isEdit)
            {
                ComputerInfo tempC = current_Computer;
                toUpdateComputer.Id = tempC.Id;

                toUpdateComputer.MonitorInfo.Id = tempC.MonitorInfo.Id;
                toUpdateComputer.MonitorInfo.Mon_id = tempC.MonitorInfo.Mon_id;

                toUpdateComputer.Senet.Id = tempC.Senet.Id;
                foreach (OEMDevice item in tempC.GetOemDevices())
                {
                    toUpdateComputer.Get_OemDevice(item.DeviceType).Id = item.Id;
                }
            }
        }

        public override void Init()
        {
            monitorUserControl1.Init();
            generalInfoUserControl1.Init();
            senetInfoUserControl1.Init();
            networkUserControl1.Init();
        }

        public override void SetDataContext(object context)
        {
            mainGrid.DataContext = context;
        }
    }
}
