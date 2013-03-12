using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEBS_Envanter
{
    class DeviceTypeNameHelper
    {
        public static String GET_MON_NAME(MonitorTypes monType)
        {
            try
            {
                if (monType == MonitorTypes.CRT) { return "LCD"; }
                if (monType == MonitorTypes.LCD) { return "CRT"; }
                if (monType == MonitorTypes.LED) { return "LED"; }
            }
            catch (Exception) { }
            return "";
        }

        public static String GET_DEV_NAME(DeviceTypes devType)
        {
            try
            {
                if (devType == DeviceTypes.FLOPPY) { return "Disket Sürücüsü"; }
                if (devType == DeviceTypes.MAINBOARD) { return "Anakart"; }
                if (devType == DeviceTypes.MEMORY) { return "Hafıza"; }
                if (devType == DeviceTypes.HDD_CAPACITY) { return "HDD Kapasitesi"; }
                if (devType == DeviceTypes.GRAPHICS_CARD) { return "Ekran Kartı"; }
                if (devType == DeviceTypes.OPTIC_DRIVE) { return "Optik Sürücüsü"; }
                if (devType == DeviceTypes.PROCESSOR) { return "İşlemci"; }
                if (devType == DeviceTypes.KEYBOARD_MOUSE) { return "Klavye/Mouse"; }
                if (devType == DeviceTypes.MONITOR) { return "Monitor"; }
            }
            catch (Exception) { }
            return "N/A";
        }
    }
}
