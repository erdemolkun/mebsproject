using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter
{

    public enum DeviceTypes : int
    {
        NONE=0,
        PROCESSOR = 1,
        MAINBOARD = 2,
        MEMORY = 3,
        HDD_CAPACITY = 4,
        GRAPHICS_CARD = 5,
        FLOPPY = 6,
        OPTIC_DRIVE = 7,
        KEYBOARD_MOUSE = 8,
        MONITOR = 9,
        PRINTER=10
    }

    public enum MonitorTypes : int
    {

        CRT = 1,

        LCD = 2,

        LED = 3
    }

}
