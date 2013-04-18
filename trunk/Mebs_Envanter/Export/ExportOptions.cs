using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.Export
{
    internal class ExportOptions
    {
        public static int HTML=1;
        public static int EXCEL = 2;


        public bool ExportGeneralInfo = true;
        public bool ExportSenetInfo = true;
        public bool ExportNetworkInfo = true;
        public bool ExportMonitorInfo = true;
        public bool ExportOemDevicesInfo = true;

    }
}
