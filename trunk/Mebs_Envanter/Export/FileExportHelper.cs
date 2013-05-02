using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Mebs_Envanter.Export
{
    abstract class FileExportHelper
    {
        internal bool openAfterSave = false;
        public void OpenAfterSave(bool shouldOpen, String fileName)
        {
            if (shouldOpen)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(fileName));
                }
                catch
                    (Exception) { }
            }
        }

        public abstract void Export(System.Data.DataSet dsInput, string filename);
        

       
    }
}
