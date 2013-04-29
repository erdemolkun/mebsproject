using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter
{
    public static class ExtraDeviceTypes
    {
        public static int PRINTER = 1;
        public static int SCANNER = 2;
        public static int PROJECTION = 4;

        public static int ConvertToDeviceType(int extraDeviceType) {

            if (extraDeviceType == PRINTER) {
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
    public class StaticFields
    {
        /// <summary>
        /// Rütbelerin değiştirilme yeri. 
        /// Rütbeler sabit olduğu için kod tarafından yazılması tercih edildi. 
        /// Gerekli görüldüğü yerde rütbelerin yazıları değiştirilebilir veya rütbe eklenip çıkartılabilir.
        /// </summary>
        public static String[] rutbeler = {"","Uzm. Çvş.","Astsb. Çvş.", "Kd. Çvş.", "Üçvş.", "Kd. Üçvş.", "Bçvş.", "Kd. Bçvş." ,"Astğm.",
                                              "Tğm.","Ütğm.","Yzb.","Bnb.","Yb.","Alb.","Tuğg.","Tümg.","Korg.","Org."};
    }
}
