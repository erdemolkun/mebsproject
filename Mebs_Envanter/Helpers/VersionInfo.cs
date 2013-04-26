using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Mebs_Envanter
{
    internal class VersionInfo
    {
        public static String versiyonStr = "";
        static VersionInfo() {


            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.ProductVersion;

            //String msg = "MEBS Bölük Komutanlığı \nBilgisayar Envanter Kayıt Programı\n\n\n";
            //msg += "\tVersiyon : " + fvi.ProductBuildPart + "." + fvi.ProductPrivatePart;

            versiyonStr = "Versiyon : " + fvi.ProductBuildPart + "." + fvi.ProductPrivatePart;
        }
    }
}
