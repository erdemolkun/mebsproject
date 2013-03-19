using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.DB
{
    public static class DBValueHelpers
    {
        public static Int32 GetInt32(object s, int defaultValue) {
            if (s == null || String.IsNullOrEmpty(s.ToString()))
            {
                return defaultValue;
            }
            else {
                return Convert.ToInt32(s);
            }
            return defaultValue;
        }
    }
}
