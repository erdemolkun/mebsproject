using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Drawing2D;
using Mebs_Envanter.Hardware;
using System.Windows.Forms;


namespace Mebs_Envanter.PrintOperations
{
    internal class SystemPrint
    {

        #region Variables

        System.Drawing.Font fontMedium = new Font("Tahoma", 12,
                                                     FontStyle.Regular);
        System.Drawing.Font fontSmall = new Font("Tahoma", 11,
                                                 FontStyle.Regular);
        System.Drawing.Font fontMediumPlus = new Font("Tahoma", 13,
                                                 FontStyle.Regular);

        private object toPrintObject = null;

        #endregion

        public SystemPrint(object toPrintObject)
        {
            this.toPrintObject = toPrintObject;
        }

        void printOemDeviceProperties(GridPrinter p2, Graphics g, OemDeviceViewModel dev1, int row, String isim, Font fntString11)
        {
            OEMDevice dev = dev1.DevOem;
            String name = isim;
            String ozellikler = dev.ToString();

            PointF x = p2.getlocation(row, 1);
            PointF xrowarti1 = p2.getlocation(row + 1, 1);
            int a = (int)((x.Y + xrowarti1.Y) / 2);

            PointF x1 = p2.getlocation(row, 2);
            PointF x1rowarti12 = p2.getlocation(row + 1, 2);
            int b = (int)((x1.Y + x1rowarti12.Y) / 2);

            PointF x0 = p2.getlocation(row, 0);
            PointF x2rowarti1 = p2.getlocation(row + 1, 0);
            int c = (int)((x1.Y + x2rowarti1.Y) / 2);

            PointF x3 = p2.getlocation(row, 3);
            PointF x4 = p2.getlocation(row, 4);


            p2.printString(g, row, 1, fntString11, name, false, true);
            g.DrawString(ozellikler, fntString11,
                               Brushes.Black, x1.X, b - 6);

            g.DrawString(dev.SerialNumber, fntString11,
                               Brushes.Black, x0.X, c - 6);


            if (dev.DeviceType == DeviceTypes.MONITOR)
            {
                if (dev.DeviceInfo.Trim() == "" && dev.SerialNumber.Trim() == "" && dev.Marka.MarkaName.Trim() == "")
                {
                    p2.printString(g, row, 4, fntString11, "YOK", true, true);
                    p2.printString(g, row, 3, fntString11, "", true, true);
                }
                else
                {
                    p2.printString(g, row, 4, fntString11, dev.Adet.ToString(), true, true);
                    p2.printString(g, row, 3, fntString11, "EA", true, true);
                }
            }
            else
            {
                if (dev.DeviceInfo.Trim() == "" && dev.SerialNumber.Trim() == "")
                {
                    p2.printString(g, row, 4, fntString11, "YOK", true, true);
                    p2.printString(g, row, 3, fntString11, "", true, true);
                }
                else
                {
                    p2.printString(g, row, 4, fntString11, dev.Adet.ToString(), true, true);
                    p2.printString(g, row, 3, fntString11, "EA", true, true);
                }
            }
        }


        void PD_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (toPrintObject == null)
            {


                MessageBox.Show("NULL");
            }
            if (toPrintObject is ComputerInfo)
            {


                PrintComputer(toPrintObject as ComputerInfo, e);
            }
            else if (toPrintObject is YaziciInfo)
            {

                PrintYazici(toPrintObject as YaziciInfo, e);
            }
        }

        void PrintComputer(ComputerInfo computerInfo, PrintPageEventArgs e)
        {

            /*Geçici mutemet senedi*/
            List<int> xPercantages = new List<int>();
            xPercantages.Add(10);

            List<int> yPercantages = new List<int>();
            yPercantages.Add(20);

            GridPrinter p0 = new GridPrinter(new Rectangle(100, 40, 999, 30), yPercantages.Count, xPercantages.Count, xPercantages, yPercantages);
            p0.Paint(e.Graphics);


            PointF gecici_mutemet_senedi_Loc = p0.getlocation(0, 0);

            PointF x_gecici_rowarti1 = p0.getlocation(1, 0);
            int b = (int)((gecici_mutemet_senedi_Loc.Y + x_gecici_rowarti1.Y) / 2);

            p0.printString(e.Graphics, 0, 0, fontMediumPlus, "GEÇİCİ MUTEMET SENEDİ", true, true);


            /*  
              malzeme veren isim kısmı
             */

            List<int> xPercantages4 = new List<int>();
            xPercantages4.Add(45);
            xPercantages4.Add(30);
            xPercantages4.Add(30);


            List<int> yPercantages4 = new List<int>();
            yPercantages4.Add(20);


            GridPrinter p4 = new GridPrinter(new Rectangle(100, 70, 1000, 50), yPercantages4.Count, xPercantages4.Count, xPercantages4, yPercantages4);
            p4.Paint(e.Graphics);

            PointF senedi_dosyalayacak = p4.getlocation(0, 0);
            e.Graphics.DrawString("Senedi Dosyalayacak Olan(Sorumlu Sb.)", fontMedium,
                                   Brushes.Black, senedi_dosyalayacak.X, senedi_dosyalayacak.Y);

            PointF malzemeyi_veren = p4.getlocation(0, 1);
            e.Graphics.DrawString("Malzemeyi Veren:", fontMedium,
                                   Brushes.Black, malzemeyi_veren.X, malzemeyi_veren.Y);

            SizeF yaziBoyutu_Malzeme_veren = e.Graphics.MeasureString("Malzemeyi Veren:", fontMedium);


            PointF malzemeyi_veren_data = p4.getlocation(0, 1);
            e.Graphics.DrawString(computerInfo.Senet.Veren_kisi_isim, fontSmall,
                                   Brushes.Black, malzemeyi_veren.X, malzemeyi_veren.Y + yaziBoyutu_Malzeme_veren.Height);



            PointF iade_edecegi_tarih = p4.getlocation(0, 2);
            e.Graphics.DrawString("İade Edeceği Tarih", fontMedium,
                                   Brushes.Black, iade_edecegi_tarih.X, iade_edecegi_tarih.Y);

            /*  
              isim veya evsaf kısmı
             */
            List<int> xPercantages3 = new List<int>();
            xPercantages3.Add(20);
            xPercantages3.Add(55);
            xPercantages3.Add(10);
            xPercantages3.Add(11);
            xPercantages3.Add(9);

            List<int> yPercantages3 = new List<int>();
            yPercantages3.Add(20);

            GridPrinter p3 = new GridPrinter(new Rectangle(100, 120, 1000, 40), yPercantages3.Count, xPercantages3.Count, xPercantages3, yPercantages3);
            p3.Paint(e.Graphics);

            PointF seri_no_Loc = p3.getlocation(0, 0);
            p3.printString(e.Graphics, 0, 0, fontMedium, "Seri Numarası", true, true);

            p3.printString(e.Graphics, 0, 1, fontMedium, "İSİM VEYA EVSAFI", true, true);
            p3.printString(e.Graphics, 0, 2, fontMedium, "Birimi", true, true);
            p3.printString(e.Graphics, 0, 3, fontMedium, "Verilen Miktar", true, true);
            p3.printString(e.Graphics, 0, 4, fontMedium, "Fiyatı", true, true);

            /*  
             bilgisayar özelliklerinin yazıldığı gridler
            */

            List<int> xPercantages2 = new List<int>();
            xPercantages2.Add(20);
            xPercantages2.Add(15);
            xPercantages2.Add(40);
            xPercantages2.Add(10);
            xPercantages2.Add(11);
            xPercantages2.Add(9);

            List<int> yPercantages2 = new List<int>();
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);

            GridPrinter p2 = new GridPrinter(new Rectangle(100, 160, 1000, 500), yPercantages2.Count, xPercantages2.Count, xPercantages2, yPercantages2);
            p2.Paint(e.Graphics);

            int row = 1;
            foreach (OemDeviceViewModel item in computerInfo.OemDevicesVModel.OemDevicesAll)
            {
                String name = item.ParcaTipiIsmi;

                printOemDeviceProperties(p2, e.Graphics, item, row, name, fontSmall);
                row++;

            }
            OemDeviceViewModel x = new OemDeviceViewModel(computerInfo.MonitorInfo);
            printOemDeviceProperties(p2, e.Graphics, x, row, "Monitör", fontSmall);

            PointF pc_adi_Loc = p2.getlocation(0, 1);
            e.Graphics.DrawString("Bilgisayar", fontSmall,
                                   Brushes.Black, pc_adi_Loc.X, pc_adi_Loc.Y + 10);


            PointF pc_Adi_Loc1 = p2.getlocation(0, 2);
            e.Graphics.DrawString(computerInfo.ToString(), fontSmall,
                                   Brushes.Black, pc_Adi_Loc1.X, pc_Adi_Loc1.Y + 10);


            p2.printString(e.Graphics, 0, 3, fontSmall, "EA", true, true);

            p2.printString(e.Graphics, 0, 4, fontSmall, "1", true, true);

            /*  
             açıklma kısmı
            */

            List<int> xPercantages5 = new List<int>();
            xPercantages5.Add(105);


            List<int> yPercantages5 = new List<int>();
            yPercantages5.Add(20);



            GridPrinter p5 = new GridPrinter(new Rectangle(100, 656, 996, 40), yPercantages5.Count, xPercantages5.Count, xPercantages5, yPercantages5);
            p5.Paint(e.Graphics);


            SizeF yaziBoyutu = e.Graphics.MeasureString("AÇIKLAMALAR:", fontMedium);

            PointF aciklama_Loc = p5.getlocation(0, 0);
            e.Graphics.DrawString("AÇIKLAMALAR:", fontMedium,
                                   Brushes.Black, aciklama_Loc.X, aciklama_Loc.Y);

            e.Graphics.DrawString(computerInfo.Notlar, fontSmall,
                                     Brushes.Black, aciklama_Loc.X + yaziBoyutu.Width, aciklama_Loc.Y);

            /*  
             ad ,soyad, imza kısmı
            */

            List<int> xPercantages6 = new List<int>();
            xPercantages6.Add(20);
            xPercantages6.Add(55);
            xPercantages6.Add(10);
            xPercantages6.Add(20);


            List<int> yPercantages6 = new List<int>();
            yPercantages6.Add(10);
            yPercantages6.Add(10);

            GridPrinter p6 = new GridPrinter(new Rectangle(100, 697, 1000, 40), yPercantages6.Count, xPercantages6.Count, xPercantages6, yPercantages6);
            p6.Paint(e.Graphics);

            PointF tarih_Loc1 = p6.getlocation(0, 0);
            e.Graphics.DrawString("Tarih", fontMedium,
                                   Brushes.Black, tarih_Loc1.X, tarih_Loc1.Y);

            PointF tarih_Loc2 = p6.getlocation(1, 0);


            e.Graphics.DrawString(computerInfo.EklenmeTarihi.Value.ToShortDateString(), fontSmall,
                                  Brushes.Black, tarih_Loc2.X, tarih_Loc2.Y);

            PointF ad_soyad_rutbe_Loc1 = p6.getlocation(0, 1);
            e.Graphics.DrawString("Alanın ; Adı,Soyadı,Rütbesi,Sicil No. ve Görevi", fontMedium,
                                   Brushes.Black, ad_soyad_rutbe_Loc1.X, ad_soyad_rutbe_Loc1.Y);

            PointF ad_soyad_rutbe_Loc2 = p6.getlocation(1, 1);
            e.Graphics.DrawString(computerInfo.Senet.Alan_kisi_isim + ", " + computerInfo.Senet.Alan_kisi_rutbe, fontSmall,
                                   Brushes.Black, ad_soyad_rutbe_Loc2.X, ad_soyad_rutbe_Loc2.Y);


            PointF imza_Loc1 = p6.getlocation(0, 2);
            e.Graphics.DrawString("İmzası", fontMedium,
                                   Brushes.Black, imza_Loc1.X, imza_Loc1.Y);

            PointF is_telefonu_Loc1 = p6.getlocation(0, 3);
            e.Graphics.DrawString("İş Telefonu", fontMedium,
                                   Brushes.Black, is_telefonu_Loc1.X, is_telefonu_Loc1.Y);



            //alt bilgi kısmı

            e.Graphics.DrawString("HKF : 1297", fontSmall, Brushes.Black, new PointF(100, 740));
            e.Graphics.DrawString("HKY 45-1", fontSmall, Brushes.Black, new PointF(1000, 740));

        }

        void PrintYazici(YaziciInfo yaziciInfo, PrintPageEventArgs e)
        {

            /*Geçici mutemet senedi*/
            List<int> xPercantages = new List<int>();
            xPercantages.Add(10);

            List<int> yPercantages = new List<int>();
            yPercantages.Add(20);

            GridPrinter p0 = new GridPrinter(new Rectangle(100, 40, 999, 30), yPercantages.Count, xPercantages.Count, xPercantages, yPercantages);
            p0.Paint(e.Graphics);

            PointF gecici_mutemet_senedi_Loc = p0.getlocation(0, 0);

            PointF x_gecici_rowarti1 = p0.getlocation(1, 0);
            int b = (int)((gecici_mutemet_senedi_Loc.Y + x_gecici_rowarti1.Y) / 2);

            p0.printString(e.Graphics, 0, 0, fontMediumPlus, "GEÇİCİ MUTEMET SENEDİ", true, true);

            /*  
              malzeme veren isim kısmı
             */

            List<int> xPercantages4 = new List<int>();
            xPercantages4.Add(45);
            xPercantages4.Add(30);
            xPercantages4.Add(30);


            List<int> yPercantages4 = new List<int>();
            yPercantages4.Add(20);



            GridPrinter p4 = new GridPrinter(new Rectangle(100, 70, 1000, 50), yPercantages4.Count, xPercantages4.Count, xPercantages4, yPercantages4);
            p4.Paint(e.Graphics);

            PointF senedi_dosyalayacak = p4.getlocation(0, 0);
            e.Graphics.DrawString("Senedi Dosyalayacak Olan(Sorumlu Sb.)", fontMedium,
                                   Brushes.Black, senedi_dosyalayacak.X, senedi_dosyalayacak.Y);


            PointF malzemeyi_veren = p4.getlocation(0, 1);
            e.Graphics.DrawString("Malzemeyi Veren:", fontMedium,
                                   Brushes.Black, malzemeyi_veren.X, malzemeyi_veren.Y);

            SizeF yaziBoyutu_Malzeme_veren = e.Graphics.MeasureString("Malzemeyi Veren:", fontMedium);


            PointF malzemeyi_veren_data = p4.getlocation(0, 1);
            e.Graphics.DrawString(yaziciInfo.SenetInfo.Veren_kisi_isim, fontSmall,
                                   Brushes.Black, malzemeyi_veren.X, malzemeyi_veren.Y + yaziBoyutu_Malzeme_veren.Height);



            PointF iade_edecegi_tarih = p4.getlocation(0, 2);
            e.Graphics.DrawString("İade Edeceği Tarih", fontMedium,
                                   Brushes.Black, iade_edecegi_tarih.X, iade_edecegi_tarih.Y);

            /*  
              isim veya evsaf kısmı
             */
            List<int> xPercantages3 = new List<int>();
            xPercantages3.Add(20);
            xPercantages3.Add(55);
            xPercantages3.Add(10);
            xPercantages3.Add(11);
            xPercantages3.Add(9);

            List<int> yPercantages3 = new List<int>();
            yPercantages3.Add(20);

            GridPrinter p3 = new GridPrinter(new Rectangle(100, 120, 1000, 40), yPercantages3.Count, xPercantages3.Count, xPercantages3, yPercantages3);
            p3.Paint(e.Graphics);

            PointF seri_no_Loc = p3.getlocation(0, 0);

            p3.printString(e.Graphics, 0, 0, fontMedium, "Seri Numarası", true, true);
            p3.printString(e.Graphics, 0, 1, fontMedium, "İSİM VEYA EVSAFI", true, true);
            p3.printString(e.Graphics, 0, 2, fontMedium, "Birimi", true, true);
            p3.printString(e.Graphics, 0, 3, fontMedium, "Verilen Miktar", true, true);
            p3.printString(e.Graphics, 0, 4, fontMedium, "Fiyatı", true, true);

            /*  
             bilgisayar özelliklerinin yazıldığı gridler
            */

            List<int> xPercantages2 = new List<int>();
            xPercantages2.Add(20);
            xPercantages2.Add(15);
            xPercantages2.Add(40);
            xPercantages2.Add(10);
            xPercantages2.Add(11);
            xPercantages2.Add(9);

            List<int> yPercantages2 = new List<int>();
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);
            yPercantages2.Add(10);

            GridPrinter p2 = new GridPrinter(new Rectangle(100, 160, 1000, 500), yPercantages2.Count, xPercantages2.Count, xPercantages2, yPercantages2);
            p2.Paint(e.Graphics);



            PointF yazici_seri_no_Loc = p2.getlocation(0, 0);
            e.Graphics.DrawString(yaziciInfo.SerialNumber, fontSmall,
                                   Brushes.Black, yazici_seri_no_Loc.X, yazici_seri_no_Loc.Y + 10);



            PointF pc_adi_Loc = p2.getlocation(0, 1);
            e.Graphics.DrawString("Yazıcı Markası", fontSmall,
                                   Brushes.Black, pc_adi_Loc.X, pc_adi_Loc.Y + 10);


            PointF pc_Adi_Loc1 = p2.getlocation(0, 2);
            e.Graphics.DrawString(yaziciInfo.Marka.ToString(), fontSmall,
                                   Brushes.Black, pc_Adi_Loc1.X, pc_Adi_Loc1.Y + 10);


            PointF yazici_model_Loc = p2.getlocation(1, 1);
            e.Graphics.DrawString("Yazıcı Modeli", fontSmall,
                                   Brushes.Black, yazici_model_Loc.X, yazici_model_Loc.Y + 10);


            PointF yazici_model_Loc1 = p2.getlocation(1, 2);
            e.Graphics.DrawString(yaziciInfo.YaziciModeli, fontSmall,
                                   Brushes.Black, yazici_model_Loc1.X, yazici_model_Loc1.Y + 10);



            p2.printString(e.Graphics, 0, 3, fontSmall, "EA", true, true);
            p2.printString(e.Graphics, 0, 4, fontSmall, "1", true, true);

            /*  
             açıklma kısmı
            */

            List<int> xPercantages5 = new List<int>();
            xPercantages5.Add(105);



            List<int> yPercantages5 = new List<int>();
            yPercantages5.Add(20);



            GridPrinter p5 = new GridPrinter(new Rectangle(100, 656, 996, 40), yPercantages5.Count, xPercantages5.Count, xPercantages5, yPercantages5);
            p5.Paint(e.Graphics);


            SizeF yaziBoyutu = e.Graphics.MeasureString("AÇIKLAMALAR:", fontMedium);

            PointF aciklama_Loc = p5.getlocation(0, 0);
            e.Graphics.DrawString("AÇIKLAMALAR:", fontMedium,
                                   Brushes.Black, aciklama_Loc.X, aciklama_Loc.Y);

            /*  
             ad ,soyad, imza kısmı
            */

            List<int> xPercantages6 = new List<int>();
            xPercantages6.Add(20);
            xPercantages6.Add(55);
            xPercantages6.Add(10);
            xPercantages6.Add(20);


            List<int> yPercantages6 = new List<int>();
            yPercantages6.Add(10);
            yPercantages6.Add(10);

            GridPrinter p6 = new GridPrinter(new Rectangle(100, 697, 1000, 40), yPercantages6.Count, xPercantages6.Count, xPercantages6, yPercantages6);
            p6.Paint(e.Graphics);


            PointF tarih_Loc1 = p6.getlocation(0, 0);
            e.Graphics.DrawString("Tarih", fontMedium,
                                   Brushes.Black, tarih_Loc1.X, tarih_Loc1.Y);

            PointF tarih_Loc2 = p6.getlocation(1, 0);


            PointF ad_soyad_rutbe_Loc1 = p6.getlocation(0, 1);
            e.Graphics.DrawString("Alanın ; Adı,Soyadı,Rütbesi,Sicil No. ve Görevi", fontMedium,
                                   Brushes.Black, ad_soyad_rutbe_Loc1.X, ad_soyad_rutbe_Loc1.Y);

            PointF ad_soyad_rutbe_Loc2 = p6.getlocation(1, 1);
            e.Graphics.DrawString(yaziciInfo.SenetInfo.Alan_kisi_isim + ", " + yaziciInfo.SenetInfo.Alan_kisi_rutbe, fontSmall,
                                   Brushes.Black, ad_soyad_rutbe_Loc2.X, ad_soyad_rutbe_Loc2.Y);


            PointF imza_Loc1 = p6.getlocation(0, 2);
            e.Graphics.DrawString("İmzası", fontMedium,
                                   Brushes.Black, imza_Loc1.X, imza_Loc1.Y);

            PointF is_telefonu_Loc1 = p6.getlocation(0, 3);
            e.Graphics.DrawString("İş Telefonu", fontMedium,
                                   Brushes.Black, is_telefonu_Loc1.X, is_telefonu_Loc1.Y);

        }

        public void Print(bool isPreview)
        {
            PrintDocument PD = new PrintDocument();
            PD.PrintPage += new PrintPageEventHandler(PD_PrintPage);
            PD.QueryPageSettings += new QueryPageSettingsEventHandler(PD_QueryPageSettings);
            PD.DefaultPageSettings.Landscape = true;
            System.Windows.Forms.PrintPreviewDialog pdlg = new System.Windows.Forms.PrintPreviewDialog();
            pdlg.Document = PD;

            pdlg.SetDesktopBounds(0, 0, (int)System.Windows.SystemParameters.PrimaryScreenWidth, (int)System.Windows.SystemParameters.PrimaryScreenHeight);
            pdlg.PrintPreviewControl.Zoom = 1.1;

            if (isPreview)
                pdlg.ShowDialog();
            else
                PD.Print();
        }

        void PD_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            //e.PageSettings.PaperSize.Kind = "custom";
            //e.PageSettings.PaperSize.Height= 300;
            //e.PageSettings.PaperSize.Width = 250;
            //throw new NotImplementedException();

            float width = e.PageSettings.PrintableArea.Width;
            float height = e.PageSettings.PrintableArea.Height;

            pageSize = new PointF(width, height);
        }

        PointF pageSize;
        public float getXByPercantage(float x)
        {
            return (pageSize.X * x) / 100f;
        }
        public float getYByPercantage(float y)
        {
            return (pageSize.Y * y) / 100f;
        }

        public RectangleF getPrintingRect(float xPerc, float yPerc, float widthPerc, float heightPerc)
        {
            float h = Math.Abs(getYByPercantage(heightPerc) - getYByPercantage(0));
            float w = Math.Abs(getXByPercantage(widthPerc) - getXByPercantage(0));
            float x = getXByPercantage(xPerc);
            float y = getYByPercantage(yPerc);
            return new RectangleF(x, y, w, h);
        }

        public Font fntString { get; set; }

    }
}
