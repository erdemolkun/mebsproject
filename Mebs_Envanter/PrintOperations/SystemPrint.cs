using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter;
using System.Drawing.Printing;

namespace Mebs_Envanter.PrintOperations
{
    internal class SystemPrint
    {
        ComputerInfo computerInfo = null;
        public SystemPrint(ComputerInfo _computerInfo) {

            this.computerInfo = _computerInfo;
           
        }

        void PD_PrintPage(object sender, PrintPageEventArgs e)
        {

        }


        public void Print(bool isPreview){
            PrintDocument PD = new PrintDocument();
            PD.PrintPage += new PrintPageEventHandler(PD_PrintPage);
            System.Windows.Forms.PrintPreviewDialog pdlg = new System.Windows.Forms.PrintPreviewDialog();
            pdlg.Document = PD;


            pdlg.SetDesktopBounds(0, 0, (int)System.Windows.SystemParameters.PrimaryScreenWidth, (int)System.Windows.SystemParameters.PrimaryScreenHeight);
            pdlg.PrintPreviewControl.Zoom = 1.1;
           
            if (isPreview)
                pdlg.ShowDialog();
            else
                PD.Print();
        }
    }
}
