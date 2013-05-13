using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.Hardware
{

    public static class IndividualDeviceTypes
    {
        public static int PRINTER = 1;
        public static int SCANNER = 2;
        public static int PROJECTION = 4;

        public static int ConvertToDeviceType(int extraDeviceType)
        {

            if (extraDeviceType == PRINTER)
            {
                return (int)DeviceTypes.PRINTER;
            }
            if (extraDeviceType == SCANNER)
            {
                return (int)DeviceTypes.SCANNER;
            }
            if (extraDeviceType == PROJECTION)
            {
                return (int)DeviceTypes.PROJECTION;
            }
            return -1;
        }
    }

    public class IndividualDevice
    {
       

        public static List<IndividualDevice> Devices = null;
        static IndividualDevice()
        {
            Devices = new List<IndividualDevice>();
            Devices.Add(new IndividualDevice(IndividualDeviceTypes.PRINTER));
            Devices.Add(new IndividualDevice(IndividualDeviceTypes.SCANNER));
            Devices.Add(new IndividualDevice(IndividualDeviceTypes.PROJECTION));
        }

        public override string ToString()
        {

            return DeviceTypeNameHelper.GET_DEV_NAME((DeviceTypes)IndividualDeviceTypes.ConvertToDeviceType(ExtraDeviceType));

        }
        public int ExtraDeviceType;
        public IndividualDevice(int devType)
        {
            this.ExtraDeviceType = devType;
        }
    }
}
