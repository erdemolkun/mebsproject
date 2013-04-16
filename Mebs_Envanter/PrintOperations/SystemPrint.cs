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
        //ComputerInfo computerInfo = null;

        object toPrintObject = null;
        
        public SystemPrint(object _toPrintObject)
        {

            this.toPrintObject = _toPrintObject;

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

            //g.DrawString(name, fntString11,
            //                   Brushes.Black, x.X, a-6);


            p2.printString(g, row, 1, fntString11, name, false, true);
            g.DrawString(ozellikler, fntString11,
                               Brushes.Black, x1.X, b - 6);

            g.DrawString(dev.SerialNumber, fntString11,
                               Brushes.Black, x0.X, c - 6);

            //g.DrawString("EA", fntString11,
            //                 Brushes.Black, x3.X, x3.Y);

            

            //g.DrawString(dev.VerilenMiktar.ToString(), fntString11,
            //                   Brushes.Black, x4.X, x4.Y);
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
            //p2.printString(g, row, 4, fntString11, dev.VerilenMiktar.ToString(), true,true);


        }

        System.Drawing.Font fntString12 = new Font("Tahoma", 12,
                                                     FontStyle.Regular);
        System.Drawing.Font fntString11 = new Font("Tahoma", 11,
                                                 FontStyle.Regular);
        System.Drawing.Font fntString13 = new Font("Tahoma", 13,
                                                 FontStyle.Regular);
        void PD_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (toPrintObject == null) {


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


            //System.Drawing.Font fntString2 = new Font("Times New Roman", 12,
            //                                        FontStyle.Regular);
            //e.Graphics.DrawString("GEÇİCİ MUTEMET SENEDİ" , fntString11,
            //                       Brushes.Black,400, 55);

            PointF gecici_mutemet_senedi_Loc = p0.getlocation(0, 0);

            PointF x_gecici_rowarti1 = p0.getlocation(1, 0);
            int b = (int)((gecici_mutemet_senedi_Loc.Y + x_gecici_rowarti1.Y) / 2);


            //PointF y_gecici_columnarti1 = p0.getlocation(0, 1);
            //int c = (int)((gecici_mutemet_senedi_Loc.X + y_gecici_columnarti1.X) / 2);

            //e.Graphics.DrawString("GEÇİCİ MUTEMET SENEDİ", fntString13,
            //                       Brushes.Black, c-100, b-10);


            p0.printString(e.Graphics, 0, 0, fntString13, "GEÇİCİ MUTEMET SENEDİ", true, true);


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
            e.Graphics.DrawString("Senedi Dosyalayacak Olan(Sorumlu Sb.)", fntString12,
                                   Brushes.Black, senedi_dosyalayacak.X, senedi_dosyalayacak.Y);

            //SizeF yaziBoyutu_senedi_dosyalayacak = e.Graphics.MeasureString("Senedi Dosyalayacak Olan(Sorumlu Sb.)", fntString12);


            //PointF senedi_dosyalayacak_data = p4.getlocation(0, 0);
            //e.Graphics.DrawString(computerInfo.Senet.Veren_kisi_isim, fntString11,
            //                       Brushes.Black, senedi_dosyalayacak.X, senedi_dosyalayacak.Y+yaziBoyutu_senedi_dosyalayacak.Height);


            PointF malzemeyi_veren = p4.getlocation(0, 1);
            e.Graphics.DrawString("Malzemeyi Veren:", fntString12,
                                   Brushes.Black, malzemeyi_veren.X, malzemeyi_veren.Y);


            SizeF yaziBoyutu_Malzeme_veren = e.Graphics.MeasureString("Malzemeyi Veren:", fntString12);


            PointF malzemeyi_veren_data = p4.getlocation(0, 1);
            e.Graphics.DrawString(computerInfo.Senet.Veren_kisi_isim, fntString11,
                                   Brushes.Black, malzemeyi_veren.X, malzemeyi_veren.Y + yaziBoyutu_Malzeme_veren.Height);



            PointF iade_edecegi_tarih = p4.getlocation(0, 2);
            e.Graphics.DrawString("İade Edeceği Tarih", fntString12,
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
            //e.Graphics.DrawString("Seri Numarası", fntString12,
            //                       Brushes.Black, seri_no_Loc.X, seri_no_Loc.Y);

            p3.printString(e.Graphics, 0, 0, fntString12, "Seri Numarası", true, true);

            //PointF isim_evsaf_Loc1 = p3.getlocation(0, 1);
            //e.Graphics.DrawString("İSİM VEYA EVSAFI", fntString12,
            //                       Brushes.Black, isim_evsaf_Loc1.X, isim_evsaf_Loc1.Y);

            p3.printString(e.Graphics, 0, 1, fntString12, "İSİM VEYA EVSAFI", true, true);

            //PointF birimi_Loc1 = p3.getlocation(0,2);
            //e.Graphics.DrawString("Birimi", fntString12,
            //                       Brushes.Black, birimi_Loc1.X, birimi_Loc1.Y);

            p3.printString(e.Graphics, 0, 2, fntString12, "Birimi", true, true);

            //PointF verilen_miktar_Loc1 = p3.getlocation(0, 3);
            //e.Graphics.DrawString("Verilen Miktar", fntString12,
            //                       Brushes.Black, verilen_miktar_Loc1.X, verilen_miktar_Loc1.Y);

            p3.printString(e.Graphics, 0, 3, fntString12, "Verilen Miktar", true, true);

            //PointF fiyati_Loc1 = p3.getlocation(0, 4);
            //e.Graphics.DrawString("Fiyatı", fntString12,
            //                       Brushes.Black, fiyati_Loc1.X, fiyati_Loc1.Y);

            p3.printString(e.Graphics, 0, 4, fntString12, "Fiyatı", true, true);

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
                //String ozellikler = item.DevOem.ToString();

                //PointF x = p2.getlocation(row, 1);
                //PointF x1 = p2.getlocation(row, 2);
                //PointF x0 = p2.getlocation(row, 0);
                //PointF x3 = p2.getlocation(row, 3);
                //PointF x4 = p2.getlocation(row, 4);

                //e.Graphics.DrawString(name, fntString11,
                //                   Brushes.Black, x.X, x.Y);

                //e.Graphics.DrawString(ozellikler, fntString11,
                //                   Brushes.Black, x1.X, x1.Y);

                //e.Graphics.DrawString(item.DevOem.SerialNumber, fntString11,
                //                   Brushes.Black, x0.X, x0.Y);

                //e.Graphics.DrawString("EA", fntString11,
                //                 Brushes.Black, x3.X, x3.Y);

                //e.Graphics.DrawString(item.DevOem.VerilenMiktar.ToString(), fntString11,
                //                   Brushes.Black, x4.X, x4.Y);


                printOemDeviceProperties(p2, e.Graphics, item, row, name, fntString11);
                row++;

            }
            OemDeviceViewModel x = new OemDeviceViewModel(computerInfo.MonitorInfo);
            printOemDeviceProperties(p2, e.Graphics, x, row, "Monitör", fntString11);




            PointF pc_adi_Loc = p2.getlocation(0, 1);
            e.Graphics.DrawString("Bilgisayar", fntString11,
                                   Brushes.Black, pc_adi_Loc.X, pc_adi_Loc.Y + 10);


            PointF pc_Adi_Loc1 = p2.getlocation(0, 2);
            e.Graphics.DrawString(computerInfo.Pc_adi.ToString(), fntString11,
                                   Brushes.Black, pc_Adi_Loc1.X, pc_Adi_Loc1.Y + 10);

            //PointF pc_adi_birimi = p2.getlocation(0, 3);
            //e.Graphics.DrawString("EA", fntString11,
            //                       Brushes.Black, pc_adi_birimi.X, pc_adi_birimi.Y + 10);
            p2.printString(e.Graphics, 0, 3, fntString11, "EA", true, true);

            //PointF pc_verilen_miktar = p2.getlocation(0, 4);
            //e.Graphics.DrawString("1", fntString11,
            //                       Brushes.Black, pc_verilen_miktar.X, pc_verilen_miktar.Y + 10);


            p2.printString(e.Graphics, 0, 4, fntString11, "1", true, true);

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


            SizeF yaziBoyutu = e.Graphics.MeasureString("AÇIKLAMALAR:", fntString12);

            PointF aciklama_Loc = p5.getlocation(0, 0);
            e.Graphics.DrawString("AÇIKLAMALAR:", fntString12,
                                   Brushes.Black, aciklama_Loc.X, aciklama_Loc.Y);

            e.Graphics.DrawString(computerInfo.Notlar, fntString11,
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
            e.Graphics.DrawString("Tarih", fntString12,
                                   Brushes.Black, tarih_Loc1.X, tarih_Loc1.Y);

            PointF tarih_Loc2 = p6.getlocation(1, 0);


            e.Graphics.DrawString(computerInfo.EklenmeTarihi.Value.ToShortDateString(), fntString11,
                                  Brushes.Black, tarih_Loc2.X, tarih_Loc2.Y);

            //System.Drawing.Font fntString2 = new Font("Times New Roman", 12,
            //                                        FontStyle.Regular);
            //e.Graphics.DrawString(computerInfo.Senet.Alan_kisi_isim+", "+computerInfo.Senet.Alan_kisi_rutbe, fntString11,
            //                       Brushes.Black, 300, 717);


            PointF ad_soyad_rutbe_Loc1 = p6.getlocation(0, 1);
            e.Graphics.DrawString("Alanın ; Adı,Soyadı,Rütbesi,Sicil No. ve Görevi", fntString12,
                                   Brushes.Black, ad_soyad_rutbe_Loc1.X, ad_soyad_rutbe_Loc1.Y);

            PointF ad_soyad_rutbe_Loc2 = p6.getlocation(1, 1);
            e.Graphics.DrawString(computerInfo.Senet.Alan_kisi_isim + ", " + computerInfo.Senet.Alan_kisi_rutbe, fntString11,
                                   Brushes.Black, ad_soyad_rutbe_Loc2.X, ad_soyad_rutbe_Loc2.Y);

            //e.Graphics.DrawString("Alanın ; Adı,Soyadı,Rütbesi,Sicil No. ve Görevi", fntString12,
            //                       Brushes.Black, 300, 697);

            PointF imza_Loc1 = p6.getlocation(0, 2);
            e.Graphics.DrawString("İmzası", fntString12,
                                   Brushes.Black, imza_Loc1.X, imza_Loc1.Y);

            PointF is_telefonu_Loc1 = p6.getlocation(0, 3);
            e.Graphics.DrawString("İş Telefonu", fntString12,
                                   Brushes.Black, is_telefonu_Loc1.X, is_telefonu_Loc1.Y);



            //alt bilgi kısmı

            e.Graphics.DrawString("HKF : 1297", fntString11, Brushes.Black, new PointF(100, 740));
            e.Graphics.DrawString("HKF : 1297", fntString11, Brushes.Black, new PointF(1000, 740));

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


           


            p0.printString(e.Graphics, 0, 0, fntString13, "GEÇİCİ MUTEMET SENEDİ", true, true);


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
            e.Graphics.DrawString("Senedi Dosyalayacak Olan(Sorumlu Sb.)", fntString12,
                                   Brushes.Black, senedi_dosyalayacak.X, senedi_dosyalayacak.Y);

           
            PointF malzemeyi_veren = p4.getlocation(0, 1);
            e.Graphics.DrawString("Malzemeyi Veren:", fntString12,
                                   Brushes.Black, malzemeyi_veren.X, malzemeyi_veren.Y);


            SizeF yaziBoyutu_Malzeme_veren = e.Graphics.MeasureString("Malzemeyi Veren:", fntString12);


            PointF malzemeyi_veren_data = p4.getlocation(0, 1);
            e.Graphics.DrawString(yaziciInfo.SenetInfo.Veren_kisi_isim, fntString11,
                                   Brushes.Black, malzemeyi_veren.X, malzemeyi_veren.Y + yaziBoyutu_Malzeme_veren.Height);



            PointF iade_edecegi_tarih = p4.getlocation(0, 2);
            e.Graphics.DrawString("İade Edeceği Tarih", fntString12,
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
            

            p3.printString(e.Graphics, 0, 0, fntString12, "Seri Numarası", true, true);

            

            p3.printString(e.Graphics, 0, 1, fntString12, "İSİM VEYA EVSAFI", true, true);

           

            p3.printString(e.Graphics, 0, 2, fntString12, "Birimi", true, true);

           

            p3.printString(e.Graphics, 0, 3, fntString12, "Verilen Miktar", true, true);

            

            p3.printString(e.Graphics, 0, 4, fntString12, "Fiyatı", true, true);

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

            //int row = 1;
            //foreach (OemDeviceViewModel item in computerInfo.OemDevicesVModel.OemDevicesAll)
            //{
            //    String name = item.ParcaTipiIsmi;
                

            //    printOemDeviceProperties(p2, e.Graphics, item, row, name, fntString11);
            //    row++;

            //}
            //OemDeviceViewModel x = new OemDeviceViewModel(computerInfo.MonitorInfo);
            //printOemDeviceProperties(p2, e.Graphics, x, row, "Monitör", fntString11);

            PointF yazici_seri_no_Loc = p2.getlocation(0, 0);
            e.Graphics.DrawString(yaziciInfo.SerialNumber, fntString11,
                                   Brushes.Black, yazici_seri_no_Loc.X, yazici_seri_no_Loc.Y + 10);


       
            PointF pc_adi_Loc = p2.getlocation(0, 1);
            e.Graphics.DrawString("Yazıcı Markası", fntString11,
                                   Brushes.Black, pc_adi_Loc.X, pc_adi_Loc.Y + 10);


            PointF pc_Adi_Loc1 = p2.getlocation(0, 2);
            e.Graphics.DrawString(yaziciInfo.Marka.ToString(), fntString11,
                                   Brushes.Black, pc_Adi_Loc1.X, pc_Adi_Loc1.Y + 10);


            PointF yazici_model_Loc = p2.getlocation(1, 1);
            e.Graphics.DrawString("Yazıcı Modeli", fntString11,
                                   Brushes.Black, yazici_model_Loc.X, yazici_model_Loc.Y + 10);


            PointF yazici_model_Loc1 = p2.getlocation(1, 2);
            e.Graphics.DrawString(yaziciInfo.YaziciModeli, fntString11,
                                   Brushes.Black, yazici_model_Loc1.X, yazici_model_Loc1.Y + 10);


            
            p2.printString(e.Graphics, 0, 3, fntString11, "EA", true, true);

          

            p2.printString(e.Graphics, 0, 4, fntString11, "1", true, true);

            /*  
             açıklma kısmı
            */

            List<int> xPercantages5 = new List<int>();
            xPercantages5.Add(105);
           


            List<int> yPercantages5 = new List<int>();
            yPercantages5.Add(20);



            GridPrinter p5 = new GridPrinter(new Rectangle(100, 656, 996, 40), yPercantages5.Count, xPercantages5.Count, xPercantages5, yPercantages5);
            p5.Paint(e.Graphics);


            SizeF yaziBoyutu = e.Graphics.MeasureString("AÇIKLAMALAR:", fntString12);

            PointF aciklama_Loc = p5.getlocation(0, 0);
            e.Graphics.DrawString("AÇIKLAMALAR:", fntString12,
                                   Brushes.Black, aciklama_Loc.X, aciklama_Loc.Y);

            //e.Graphics.DrawString(computerInfo.Notlar, fntString11,
            //                         Brushes.Black, aciklama_Loc.X + yaziBoyutu.Width, aciklama_Loc.Y);

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
            e.Graphics.DrawString("Tarih", fntString12,
                                   Brushes.Black, tarih_Loc1.X, tarih_Loc1.Y);

            PointF tarih_Loc2 = p6.getlocation(1, 0);


            //e.Graphics.DrawString(computerInfo.EklenmeTarihi.Value.ToShortDateString(), fntString11,
            //                      Brushes.Black, tarih_Loc2.X, tarih_Loc2.Y);

           


            PointF ad_soyad_rutbe_Loc1 = p6.getlocation(0, 1);
            e.Graphics.DrawString("Alanın ; Adı,Soyadı,Rütbesi,Sicil No. ve Görevi", fntString12,
                                   Brushes.Black, ad_soyad_rutbe_Loc1.X, ad_soyad_rutbe_Loc1.Y);

            PointF ad_soyad_rutbe_Loc2 = p6.getlocation(1, 1);
            e.Graphics.DrawString(yaziciInfo.SenetInfo.Alan_kisi_isim + ", " + yaziciInfo.SenetInfo.Alan_kisi_rutbe, fntString11,
                                   Brushes.Black, ad_soyad_rutbe_Loc2.X, ad_soyad_rutbe_Loc2.Y);

          
            PointF imza_Loc1 = p6.getlocation(0, 2);
            e.Graphics.DrawString("İmzası", fntString12,
                                   Brushes.Black, imza_Loc1.X, imza_Loc1.Y);

            PointF is_telefonu_Loc1 = p6.getlocation(0, 3);
            e.Graphics.DrawString("İş Telefonu", fntString12,
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


    public void values(bool iscomputer)
     {
        ComputerInfo cinfo= new ComputerInfo();
        YaziciInfo yinfo= new YaziciInfo();
        string malzemeyi_veren;
        string tarih;

        if (iscomputer)
        {
            malzemeyi_veren = cinfo.Senet.Veren_kisi_isim;
            tarih = cinfo.EklenmeTarihi.ToString() ;
        }
        else
        {
            malzemeyi_veren = yinfo.SenetInfo.Veren_kisi_isim;
            
        }
     }


}
    
}
