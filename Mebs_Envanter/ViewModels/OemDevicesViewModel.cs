using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter;
using System.Collections.ObjectModel;

namespace Mebs_Envanter
{
    public class OemDevicesViewModel : MebsBaseObject
    {
        private ObservableCollection<OemDeviceViewModel> oemDevices = new ObservableCollection<OemDeviceViewModel>();
        public ObservableCollection<OemDeviceViewModel> OemDevices
        {
            get { return oemDevices; }
        }

        private ObservableCollection<OemDeviceViewModel> oemDevicesExtra = new ObservableCollection<OemDeviceViewModel>();
        public ObservableCollection<OemDeviceViewModel> OemDevicesExtra
        {
            get { return oemDevicesExtra; }
        }

        public OemDevicesViewModel()
        {
            Init();
        }

        public void Init()
        {
            OemDevices.Add(new OemDeviceViewModel(new OEMDevice(DeviceTypes.PROCESSOR)));
            OemDevices.Add(new OemDeviceViewModel(new OEMDevice(DeviceTypes.MAINBOARD)));
            OemDevices.Add(new OemDeviceViewModel(new OEMDevice(DeviceTypes.MEMORY)));
            OemDevices.Add(new OemDeviceViewModel(new OEMDevice(DeviceTypes.HDD_CAPACITY)));
            OemDevices.Add(new OemDeviceViewModel(new OEMDevice(DeviceTypes.GRAPHICS_CARD)));

            OemDevicesExtra.Add(new OemDeviceViewModel(new OEMDevice(DeviceTypes.FLOPPY)));
            OemDevicesExtra.Add(new OemDeviceViewModel(new OEMDevice(DeviceTypes.OPTIC_DRIVE)));
            OemDevicesExtra.Add(new OemDeviceViewModel(new OEMDevice(DeviceTypes.KEYBOARD_MOUSE)));
        }

        public void AssignOemDevice(OEMDevice dev) {

            foreach (var item in OemDevices)
            {
                if (item.DevOem.DeviceType == dev.DeviceType) { item.DevOem = dev; return; }
            }
            foreach (var item in OemDevicesExtra)
            {
                if (item.DevOem.DeviceType == dev.DeviceType) { item.DevOem = dev; return; }
            }  
        }
    }
}
