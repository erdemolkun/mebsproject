using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.Hardware
{
    public class IndividualDevice
    {
        public static List<IndividualDevice> Devices = null;
        static IndividualDevice()
        {
            Devices = new List<IndividualDevice>();
            Devices.Add(new IndividualDevice(ExtraDeviceTypes.PRINTER));
            Devices.Add(new IndividualDevice(ExtraDeviceTypes.SCANNER));
            Devices.Add(new IndividualDevice(ExtraDeviceTypes.PROJECTOR));
        }

        public override string ToString()
        {
            if (DeviceType == ExtraDeviceTypes.PRINTER)
            {
                return "Yazıcı";
            }
            else if (DeviceType == ExtraDeviceTypes.PROJECTOR)
            {
                return "Projektör";
            }
            else if (DeviceType == ExtraDeviceTypes.SCANNER)
            {
                return "Tarayıcı";
            }
            return "";

        }
        public int DeviceType;
        public IndividualDevice(int devType)
        {
            this.DeviceType = devType;
        }
    }
}
