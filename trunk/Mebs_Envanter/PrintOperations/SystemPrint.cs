using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter;
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

        String geciciMutemetStr = "GEÇİCİ MUTEMET SENEDİ";

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

        void PD_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (toPrintObject == null)
            {
                MessageBox.Show("NULL");
            }
            if (toPrintObject is ComputerInfo)
            {
                PrintAnyObject(toPrintObject as ComputerInfo, e.Graphics);
                //PrintComputer(toPrintObject as ComputerInfo, e);
            }
            else if (toPrintObject is IndividualDeviceInfo)
            {
                //PrintYazici(toPrintObject as YaziciInfo, e);
                PrintAnyObject(toPrintObject as IndividualDeviceInfo, e.Graphics);
            }
        }

        void printOemDeviceProperties(GridPrinter p2, Graphics g, OEMDevice dev, int row, String isim, Font fntString11)
        {
            //OEMDevice dev = dev1.DevOem;
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


            p2.printString(g, row, 1, fntString11, name, true, false);
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
      
        void PrintAnyObject(MebsBaseObject mebsObject, Graphics g)
        {
            int outSideOffsetX = 32;
            int outSizeOffsetY = 40;
            int mainGridWidth = (int)(pageSize.X - 2 * outSideOffsetX); // landscape olduğu için
            int currentY = outSizeOffsetY;

            #region Geçici Mutemet Senedi

            /*Geçici mutemet senedi*/
            List<int> xPercantages = new List<int>();
            xPercantages.Add(10);

            List<int> yPercantages = new List<int>();
            yPercantages.Add(20);

            int geciciMutemetHeight = 30;
            GridPrinter geciciMutemetGrid = new GridPrinter(new Rectangle(outSideOffsetX, outSizeOffsetY, mainGridWidth, geciciMutemetHeight),
                xPercantages, yPercantages);
            currentY += geciciMutemetHeight;

            geciciMutemetGrid.PaintGrid(g);
            geciciMutemetGrid.printString(g, 0, 0, fontMediumPlus, geciciMutemetStr, true, true);
            /*Geçici mutemet senedi*/

            #endregion

            #region 2. Kısım

            List<int> xPercantages4 = new List<int>();
            xPercantages4.Add(45);
            xPercantages4.Add(30);
            xPercantages4.Add(30);

            List<int> yPercantages4 = new List<int>();
            yPercantages4.Add(20);

            int malzemeVerenHeight = 50;
            GridPrinter p4 = new GridPrinter(new Rectangle(outSideOffsetX, currentY, mainGridWidth, malzemeVerenHeight), xPercantages4, yPercantages4);
            currentY += malzemeVerenHeight;

            p4.PaintGrid(g);
            p4.printString(g, 0, 0, fontMedium, "Senedi Dosyalayacak Olan(Sorumlu Sb.)", false, false);

            p4.printString(g, 0, 1, fontMedium, "Malzemeyi Veren:", false, false);

            SizeF yaziBoyutu_Malzeme_veren = g.MeasureString("Malzemeyi Veren:", fontMedium);

            PointF malzemeyi_veren_Point = p4.getlocation(0, 1);
            if (mebsObject is ISenetInfo)
            {
                g.DrawString((mebsObject as ISenetInfo).Senet.Veren_kisi_isim, fontSmall,
                                       Brushes.Black, malzemeyi_veren_Point.X, malzemeyi_veren_Point.Y + yaziBoyutu_Malzeme_veren.Height);
            }

            p4.printString(g, 0, 2, fontMedium, "İade Edeceği Tarih", false, false);
            #endregion

            #region İsim veya Evsaf Kısmı

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

            int isimEvsafHeight = 40;
            GridPrinter p3 = new GridPrinter(new Rectangle(outSideOffsetX, currentY, mainGridWidth, isimEvsafHeight), xPercantages3, yPercantages3);
            currentY += isimEvsafHeight;

            p3.PaintGrid(g);

            PointF seri_no_Loc = p3.getlocation(0, 0);
            p3.printString(g, 0, 0, fontMedium, "Seri Numarası", true, true);
            p3.printString(g, 0, 1, fontMedium, "İSİM VEYA EVSAFI", true, true);
            p3.printString(g, 0, 2, fontMedium, "Birimi", true, true);
            p3.printString(g, 0, 3, fontMedium, "Verilen Miktar", true, true);
            p3.printString(g, 0, 4, fontMedium, "Fiyatı", true, true);

            #endregion

            #region Parça Özelliklerinin yazıldığı kısım

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

            int ozelliklerGridHeight = 500;
            GridPrinter p2 = new GridPrinter(new Rectangle(outSideOffsetX, currentY, mainGridWidth, ozelliklerGridHeight), xPercantages2, yPercantages2);
            p2.PaintGrid(g);
            currentY += ozelliklerGridHeight;

            #region SubPart ComputerInfo için

            if (mebsObject is ComputerInfo)
            {
                ComputerInfo computerInfo = mebsObject as ComputerInfo;
                int row = 1;
                foreach (OemDeviceViewModel item in computerInfo.OemDevicesVModel.OemDevicesAll)
                {
                    String name = item.ParcaTipiIsmi;
                    printOemDeviceProperties(p2, g, item.DevOem, row, name, fontSmall);
                    row++;
                }
                OemDeviceViewModel x = new OemDeviceViewModel(computerInfo.MonitorInfo);
                printOemDeviceProperties(p2, g, x.DevOem, row, "Monitör", fontSmall);

                p2.printString(g, 0, 1, fontSmall, "Bilgisayar", true, false);
                p2.printString(g, 0, 2, fontSmall, computerInfo.ToString(), true, false);
                p2.printString(g, 0, 3, fontSmall, "EA", true, true);
                p2.printString(g, 0, 4, fontSmall, "1", true, true);
            }
            #endregion

            #region Subpart YaziciInfo İçin
            if (mebsObject is IndividualDeviceInfo)
            {
                IndividualDeviceInfo yaziciInfo = mebsObject as IndividualDeviceInfo;
                p2.printString(g, 0, 0, fontSmall, yaziciInfo.SerialNumber, true, false);
                p2.printString(g, 0, 1, fontSmall, "Markası", true, false);
                p2.printString(g, 0, 2, fontSmall, yaziciInfo.Marka.ToString(), true, false);
                p2.printString(g, 1, 1, fontSmall, "Modeli", true, false);
                p2.printString(g, 1, 2, fontSmall, yaziciInfo.Model, true, false);
            }
            #endregion

            #endregion

            #region Açıklama Kısmı

            /*  
             açıklma kısmı
            */

            List<int> xPercantages5 = new List<int>();
            xPercantages5.Add(105);

            List<int> yPercantages5 = new List<int>();
            yPercantages5.Add(20);

            int aciklamaHeight = 40;
            GridPrinter p5 = new GridPrinter(new Rectangle(outSideOffsetX, currentY, mainGridWidth, aciklamaHeight), xPercantages5, yPercantages5);
            p5.PaintGrid(g);
            currentY += aciklamaHeight;

                 
            p5.printString(g, 0, 0, fontMedium, "AÇIKLAMALAR:", false, false);

            if (mebsObject is ComputerInfo)
            {
                SizeF yaziBoyutu = g.MeasureString("AÇIKLAMALAR:", fontMedium);
                PointF aciklama_Loc = p5.getlocation(0, 0);      
                g.DrawString((mebsObject as ComputerInfo).Notlar, fontSmall,
                                         Brushes.Black, aciklama_Loc.X + yaziBoyutu.Width, aciklama_Loc.Y);
            }

            #endregion

            #region Ad Soyad İmza Kısmı

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

            int adSoyadGridHeight = 40;
            GridPrinter p6 = new GridPrinter(new Rectangle(outSideOffsetX, currentY, mainGridWidth, adSoyadGridHeight), xPercantages6, yPercantages6);
            p6.PaintGrid(g);
            currentY += adSoyadGridHeight;

            p6.printString(g, 0, 0, fontMedium, "Tarih", false, false);

            if (mebsObject is ComputerInfo)
            {
                PointF tarih_Loc2 = p6.getlocation(1, 0);
                g.DrawString((mebsObject as ComputerInfo).EklenmeTarihi.Value.ToShortDateString(), fontSmall,
                                      Brushes.Black, tarih_Loc2.X, tarih_Loc2.Y);
            }


            PointF ad_soyad_rutbe_Loc1 = p6.getlocation(0, 1);
            g.DrawString("Alanın ; Adı,Soyadı,Rütbesi,Sicil No. ve Görevi", fontMedium,
                                   Brushes.Black, ad_soyad_rutbe_Loc1.X, ad_soyad_rutbe_Loc1.Y);

            if (mebsObject is ISenetInfo)
            {
                PointF ad_soyad_rutbe_Loc2 = p6.getlocation(1, 1);
                g.DrawString((mebsObject as ISenetInfo).Senet.Alan_kisi_isim + ", " + (mebsObject as ISenetInfo).Senet.Alan_kisi_rutbe, fontSmall,
                                       Brushes.Black, ad_soyad_rutbe_Loc2.X, ad_soyad_rutbe_Loc2.Y);
            }

            PointF imza_Loc1 = p6.getlocation(0, 2);
            g.DrawString("İmzası", fontMedium,
                                   Brushes.Black, imza_Loc1.X, imza_Loc1.Y);

            PointF is_telefonu_Loc1 = p6.getlocation(0, 3);
            g.DrawString("İş Telefonu", fontMedium,
                                   Brushes.Black, is_telefonu_Loc1.X, is_telefonu_Loc1.Y);

            //alt bilgi kısmı

            g.DrawString("HKF : 1297", fontSmall, Brushes.Black, new PointF(outSideOffsetX, currentY));
            g.DrawString("HKY 45-1", fontSmall, Brushes.Black, new PointF(1000, currentY));

            #endregion
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

            if (e.PageSettings.Landscape)
            {
                pageSize = new PointF(height, width);
            }
            else
            {
                pageSize = new PointF(width, height);
            }
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
    }
}
