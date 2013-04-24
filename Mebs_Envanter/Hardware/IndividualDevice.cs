using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.Hardware
{
    internal class IndividualDevice
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
            if (devType == ExtraDeviceTypes.PRINTER)
            {
                return "Yazıcı";
            }
            else if (devType == ExtraDeviceTypes.PROJECTOR)
            {
                return "Projektör";
            }
            else if (devType == ExtraDeviceTypes.SCANNER)
            {
                return "Tarayıcı";
            }
            return "";

        }
        private ExtraDeviceTypes devType;
        public IndividualDevice(ExtraDeviceTypes devType)
        {

            this.devType = devType;
        }
    }
}
