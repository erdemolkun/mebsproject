using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Mebs_Envanter.PrintOperations
{
    internal class SystemPrint
    {
        ComputerInfo computerInfo = null;
        public SystemPrint(ComputerInfo _computerInfo) {

            this.computerInfo = _computerInfo;
            
        }
        string isim = "Erdem OLKUN" + "Gökhan ÖNEN";


       

      

        void PD_PrintPage(object sender, PrintPageEventArgs e)
        {

            System.Drawing.Font fntString1 = new Font("Times New Roman", 12,
                                                     FontStyle.Regular);
            //int Row_sayisi; int col_sayisi; int x_nok; int y_nok; float en; float yukseklik;
            //for (int i = 1; i <= Row_sayisi; i++)
            //{
               
            //        e.Graphics.DrawRectangle(Pens.Black, (x_nok-4,y_nok-4,en-4,yukseklik-4,
            //        percantageRect.Top, percantageRect.Width, percantageRect.Height);                
            //}
            List<int> xPercantages = new List<int>();
            xPercantages.Add(10);
            xPercantages.Add(80);
            xPercantages.Add(10);
            List<int> yPercantages = new List<int>();
            yPercantages.Add(10);
            yPercantages.Add(80);
            yPercantages.Add(10);
            GridPrinter p = new GridPrinter(new Rectangle(300, 300, 500, 500), yPercantages.Count, xPercantages.Count, xPercantages, yPercantages);
            //p.Paint(e.Graphics);

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

            GridPrinter p2 = new GridPrinter(new Rectangle(100, 160, 1000, 500), yPercantages2.Count,xPercantages2.Count, xPercantages2, yPercantages2);
            p2.Paint(e.Graphics);

            int row = 1;
            foreach (OemDeviceViewModel item in computerInfo.OemDevicesVModel.OemDevicesAll)
            {
                String name = item.ParcaTipiIsmi;
                String ozellikler = item.DevOem.DeviceInfo;

                PointF x = p2.getlocation(row ,1);
                PointF x1 = p2.getlocation(row, 2);
                PointF x0 = p2.getlocation(row, 0);
                e.Graphics.DrawString(name, fntString1,
                                   Brushes.Black, x.X, x.Y);

                e.Graphics.DrawString(ozellikler, fntString1,
                                   Brushes.Black, x1.X, x1.Y);

                e.Graphics.DrawString(item.DevOem.SerialNumber, fntString1,
                                   Brushes.Black, x0.X, x0.Y);
                row++;

            }


            PointF pc_adi_Loc = p2.getlocation(0, 1);
            e.Graphics.DrawString("Bilgisayar Adı:", fntString1,
                                   Brushes.Black, pc_adi_Loc.X, pc_adi_Loc.Y);

            PointF pc_Adi_Loc1 = p2.getlocation(0, 2);
            e.Graphics.DrawString(computerInfo.Pc_adi.ToString(), fntString1,
                                   Brushes.Black, pc_Adi_Loc1.X, pc_Adi_Loc1.Y);

           

     
          
        
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


            PointF seri_no_Loc = p3.getlocation(0,0);
            e.Graphics.DrawString("Seri Numarası", fntString1,
                                   Brushes.Black, seri_no_Loc.X, seri_no_Loc.Y);


            PointF isim_evsaf_Loc1 = p3.getlocation(0, 1);
            e.Graphics.DrawString("İSİM VEYA EVSAFI", fntString1,
                                   Brushes.Black, isim_evsaf_Loc1.X, isim_evsaf_Loc1.Y);

            PointF birimi_Loc1 = p3.getlocation(0,2);
            e.Graphics.DrawString("Birimi", fntString1,
                                   Brushes.Black, birimi_Loc1.X, birimi_Loc1.Y);

            PointF verilen_miktar_Loc1 = p3.getlocation(0, 3);
            e.Graphics.DrawString("Verilen Miktar", fntString1,
                                   Brushes.Black, verilen_miktar_Loc1.X, verilen_miktar_Loc1.Y);


            PointF fiyati_Loc1 = p3.getlocation(0, 4);
            e.Graphics.DrawString("Fiyatı", fntString1,
                                   Brushes.Black, fiyati_Loc1.X, fiyati_Loc1.Y);

            /*  
              malzeme veren isim kısmı
             */

            List<int> xPercantages4 = new List<int>();
            xPercantages4.Add(45);
            xPercantages4.Add(30);
            xPercantages4.Add(30);
          

            List<int> yPercantages4 = new List<int>();
            yPercantages4.Add(20);



            GridPrinter p4 = new GridPrinter(new Rectangle(100, 80, 1000, 40), yPercantages4.Count, xPercantages4.Count, xPercantages4, yPercantages4);
            p4.Paint(e.Graphics);


            /*  
             açıklma kısmı
            */

            List<int> xPercantages5 = new List<int>();
            xPercantages5.Add(105);
            //xPercantages5.Add(30);
            //xPercantages5.Add(30);


            List<int> yPercantages5 = new List<int>();
            yPercantages5.Add(20);



            GridPrinter p5 = new GridPrinter(new Rectangle(100, 656, 996, 40), yPercantages5.Count, xPercantages5.Count, xPercantages5, yPercantages5);
            p5.Paint(e.Graphics);

            PointF aciklama_Loc = p5.getlocation(0, 0);
            e.Graphics.DrawString("AÇIKLAMALAR:", fntString1,
                                   Brushes.Black, aciklama_Loc.X, aciklama_Loc.Y);

            /*  
             ad ,soyad, imza kısmı
            */

            List<int> xPercantages6 = new List<int>();
            xPercantages6.Add(20);
            xPercantages6.Add(15);
            xPercantages6.Add(40);
            xPercantages6.Add(10);
            xPercantages6.Add(11);
            xPercantages6.Add(9);

            List<int> yPercantages6 = new List<int>();
            yPercantages6.Add(10);
            yPercantages6.Add(10);



            
            GridPrinter p6 = new GridPrinter(new Rectangle(100, 697, 1000, 40), yPercantages6.Count, xPercantages6.Count, xPercantages6, yPercantages6);
            p6.Paint(e.Graphics);

           
            
            e.Graphics.DrawString(computerInfo.EklenmeTarihi.ToString(), fntString1,
                                   Brushes.Black, 100, 717);

            System.Drawing.Font fntString2 = new Font("Times New Roman", 12,
                                                    FontStyle.Regular);
            e.Graphics.DrawString(computerInfo.Senet.Alan_kisi_isim+", "+computerInfo.Senet.Alan_kisi_rutbe, fntString1,
                                   Brushes.Black, 300, 717); 


            return;
            
            //pc_adi = computerInfo.Pc_adi;
            //pc_marka = computerInfo.Marka.ToString();
            //e.PageSettings.PaperSize = new PaperSize("Custom", 800, 300);

            RectangleF percantageRect =  getPrintingRect(4, 4, 120,60);
            RectangleF ic_cerceve = getPrintingRect(10, 10, 105, 50);
            //e.Graphics.DrawPath(Pens.Black, getPrintingRect(200, 200, 200, 200));
            //hastane bilğilerinin etrafını çiz

            GraphicsPath path = RoundedRectangle.Create((int)percantageRect.Left,
                (int)percantageRect.Top, (int)percantageRect.Width, (int)percantageRect.Height);
            //e.Graphics.DrawPath(Pens.Black, path);
            e.Graphics.DrawRectangle(Pens.Black, percantageRect.Left,
                percantageRect.Top, percantageRect.Width, percantageRect.Height);

            GraphicsPath path1 = RoundedRectangle.Create((int)ic_cerceve.Left,
                (int)ic_cerceve.Top, (int)ic_cerceve.Width, (int)ic_cerceve.Height);
            e.Graphics.DrawPath(Pens.Black, path1);

            e.Graphics.DrawLine(new Pen(Color.Black, 2), 60, 90, 700, 90);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), 60, 93, 700, 93);

            string strDisplay = "MEBS";
            System.Drawing.Font fntString = new Font("Times New Roman", 28,
                                                     FontStyle.Bold);
            e.Graphics.DrawString(strDisplay, fntString,
                                  Brushes.Black, 280, 100);

            strDisplay = "BİLGİSAYAR YAZDIRMA EKRANI";
            fntString = new System.Drawing.Font("Times New Roman", 18,
                                                FontStyle.Bold);
            e.Graphics.DrawString(strDisplay, fntString,
                                  Brushes.Black, 320, 150);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), 60, 184, 700, 184);
            e.Graphics.DrawLine(new Pen(Color.Black, 2), 60, 187, 700, 187);

            e.Graphics.DrawLine(new Pen(Color.Black, 2), 100, 250, 680, 250);

            fntString = new System.Drawing.Font("Times New Roman", 10,
                                                FontStyle.Bold);
            e.Graphics.DrawString("Bilgisayar Adı:", fntString,
                                  Brushes.Black, 100, 260);
            fntString = new System.Drawing.Font("Times New Roman", 10,
                                                FontStyle.Regular);
            e.Graphics.DrawString(computerInfo.Pc_adi, fntString,
                                  Brushes.Black, 260, 260); ;

            e.Graphics.DrawLine(new Pen(Color.Black, 1), 100, 280, 680, 280);

            fntString = new Font("Times New Roman", 10, FontStyle.Bold);
            e.Graphics.DrawString("Markası:", fntString,
                Brushes.Black, 100, 290);
            fntString = new Font("Times New Roman", 10, FontStyle.Regular);
            e.Graphics.DrawString(computerInfo.Marka.ToString(), fntString,
                Brushes.Black, 260, 290); ;

            e.Graphics.DrawLine(new Pen(Color.Black, 2), 100, 310, 680, 310);

            e.Graphics.DrawLine(new Pen(Color.Black, 2), 100, 340, 680, 340);

            fntString = new Font("Times New Roman", 10, FontStyle.Bold);
            e.Graphics.DrawString("Bilgisayar Modeli:", fntString, Brushes.Black, 100, 350);
            fntString = new Font("Times New Roman", 10, FontStyle.Regular);

            e.Graphics.DrawString(computerInfo.Model, fntString, Brushes.Black, 260, 350);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), 100, 370, 680, 370);

            fntString = new Font("Times New Roman", 10, FontStyle.Bold);
            e.Graphics.DrawString("Eklenme Tarihi:", fntString,
                Brushes.Black, 100, 380);
            fntString = new Font("Times New Roman", 10, FontStyle.Regular);
            e.Graphics.DrawString(computerInfo.EklenmeTarihi.ToString("YYYY-dd-mm"), fntString,
                                  Brushes.Black,
                                  new RectangleF(260, 380, 420, 380));
            e.Graphics.DrawString("%", fntString, Brushes.Black, 300, 380);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), 100, 400, 680, 400);

            fntString = new Font("Times New Roman", 10, FontStyle.Bold);
            e.Graphics.DrawString("Periods:", fntString,
                                  Brushes.Black, 100, 410);
            fntString = new Font("Times New Roman", 10, FontStyle.Regular);
            e.Graphics.DrawString(isim, fntString,
                                  Brushes.Black,
                                  new RectangleF(260, 410, 400, 410));
            e.Graphics.DrawString("Months", fntString, Brushes.Black, 300, 410);

            e.Graphics.DrawLine(new Pen(Color.Black, 2), 100, 430, 680, 430);

            e.Graphics.DrawString("Interest Earned:", fntString,
                                  Brushes.Black, 100, 440);
            fntString = new Font("Times New Roman", 10, FontStyle.Regular);
            e.Graphics.DrawString(isim, fntString,
                                  Brushes.Black,
                                  new RectangleF(260, 440, 420, 440));

            e.Graphics.DrawLine(new Pen(Color.Black, 1), 100, 460, 680, 460);

            e.Graphics.DrawString("Future Value:", fntString,
                                  Brushes.Black, 100, 470);
            fntString = new Font("Times New Roman", 10, FontStyle.Regular);
            e.Graphics.DrawString(isim, fntString,
                                  Brushes.Black,
                                  new RectangleF(260, 470, 420, 470));

            e.Graphics.DrawLine(new Pen(Color.Black, 2), 100, 500, 680, 500);

            
        }


       

        public void Print(bool isPreview){
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
