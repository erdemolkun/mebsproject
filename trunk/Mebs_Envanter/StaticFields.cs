using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter
{
    public enum ExtraDeviceTypes
    {
        PRINTER = 1,
        PROJECTOR = 2,
        SCANNER = 3
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
