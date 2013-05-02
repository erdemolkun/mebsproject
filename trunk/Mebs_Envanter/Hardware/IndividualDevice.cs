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
            Devices.Add(new IndividualDevice(ExtraDeviceTypes.PROJECTION));
        }

        public override string ToString()
        {

            return DeviceTypeNameHelper.GET_DEV_NAME((DeviceTypes)ExtraDeviceTypes.ConvertToDeviceType(ExtraDeviceType));

        }
        public int ExtraDeviceType;
        public IndividualDevice(int devType)
        {
            this.ExtraDeviceType = devType;
        }
    }
}
