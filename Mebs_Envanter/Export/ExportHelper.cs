using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MEBS_Envanter;

namespace Mebs_Envanter.Export
{
    internal class ExportHelper
    {
        public DataTable GetAsDataTable(IEnumerable<ComputerInfo> comps)
        {
            if (comps == null) return null;

            DataTable dt = new DataTable();

            // Genel Bilgiler
            addColumnToDatatable(ref dt, "Bilgisayar Adı", "System.String", ""); //1
            addColumnToDatatable(ref dt, "Marka", "System.String", "");//2
            addColumnToDatatable(ref dt, "Model", "System.String", "");//3
            addColumnToDatatable(ref dt, "Stok No", "System.String", "");//4
            addColumnToDatatable(ref dt, "Parça No", "System.String", "");//5
            addColumnToDatatable(ref dt, "Tempest Seviyesi", "System.String", "");//6
            addColumnToDatatable(ref dt, "Seri Numarası", "System.String", "");//7
            addColumnToDatatable(ref dt, "Eklenme Tarihi", "System.DateTime", DateTime.Now);//8
            // Genel Bilgiler

            // Ağ Bilgileri
            addColumnToDatatable(ref dt, "Mac Adresi", "System.String", "");//9
            addColumnToDatatable(ref dt, "Bağlı Olduğu Ağ", "System.String", "");//10
            // Ağ Bilgileri


            // Senet Bilgileri
            addColumnToDatatable(ref dt, "Alan Kişi Rütbesi", "System.String", "");//11
            addColumnToDatatable(ref dt, "Alan Kişi Komutanlık", "System.String", "");//12
            addColumnToDatatable(ref dt, "Alan Kişi Birliği", "System.String", "");//13
            addColumnToDatatable(ref dt, "Alan Kişi Kısım", "System.String", "");//14
            addColumnToDatatable(ref dt, "Alan Kişi İsim", "System.String", "");//15
            addColumnToDatatable(ref dt, "Teslim Eden İsim", "System.String", "");//16
            // Senet Bilgileri


            // Monitor Bilgileri

            addColumnToDatatable(ref dt, "Monitor Markası", "System.String", ""); //17
            addColumnToDatatable(ref dt, "Monitor Tipi", "System.String", ""); //18
            addColumnToDatatable(ref dt, "Monitor Stok No", "System.String", ""); //19
            addColumnToDatatable(ref dt, "Monitor Seri No", "System.String", ""); //20
            addColumnToDatatable(ref dt, "Monitor Parça No", "System.String", ""); //21
            addColumnToDatatable(ref dt, "Monitor Tempest Seviyesi", "System.String", ""); //22
            addColumnToDatatable(ref dt, "Monitor Boyutu", "System.Double", 0); //23
            // Monitor Bilgileri


            foreach (ComputerInfo compInfo in comps)
            {
                foreach (var oemDeviceModel in compInfo.OemDevicesVModel.OemDevicesAll)
                {
                    addColumnToDatatable(ref dt, oemDeviceModel.ParcaTipiIsmi + " Seri Numarası", "System.String", "");
                    addColumnToDatatable(ref dt, oemDeviceModel.ParcaTipiIsmi + " Parça Bilgisi", "System.String", "");
                }
                break;
            }


            foreach (ComputerInfo compInfo in comps)
            {
                List<Object> list1 = new List<Object>();

                // Genel Bilgiler
                list1.Add(compInfo.Pc_adi);// 1
                //2
                if (compInfo.Marka != null && compInfo.Marka.MarkaID > 0)
                {
                    list1.Add(compInfo.Marka.MarkaName);
                }
                else
                {
                    list1.Add("");
                }
                //2

                list1.Add(compInfo.Model);//3
                list1.Add(compInfo.PcStokNo);//4
                list1.Add(compInfo.DeviceNo);//5

                //6
                if (compInfo.Tempest != null && compInfo.Tempest.Id > 0)
                {
                    list1.Add(compInfo.Tempest);
                }
                else { list1.Add(""); }
                //6

                list1.Add(compInfo.SerialNumber);//7

                //8
                if (compInfo.EklenmeTarihi.HasValue)
                    list1.Add(compInfo.EklenmeTarihi.Value);
                else list1.Add(DateTime.Now);
                //8
                // Genel Bilgiler

                // Ağ Bilgileri
                //9 //10
                if (compInfo.NetworkInfo != null)
                {
                    list1.Add(compInfo.NetworkInfo.MacAddressString);
                    if (compInfo.NetworkInfo.BagliAg != null || compInfo.NetworkInfo.BagliAg.Ag_id > 0)
                    {
                        list1.Add(compInfo.NetworkInfo.BagliAg.Ag_adi);
                    }
                    else
                    {
                        list1.Add("");
                    }
                }
                else
                {
                    list1.Add("");
                    list1.Add("");
                }
                //9 //10
                // Ağ Bilgileri


                // Senet Bilgileri
                //11 //16
                list1.Add(compInfo.Senet.Alan_kisi_rutbe);
                list1.Add((compInfo.Senet.Alan_kisi_komutanlik));
                list1.Add((compInfo.Senet.Alan_kisi_birlik));
                list1.Add((compInfo.Senet.Alan_kisi_kisim));
                list1.Add((compInfo.Senet.Alan_kisi_isim));
                list1.Add((compInfo.Senet.Veren_kisi_isim));
                //11 //16
                // Senet Bilgileri


                // Monitor Bilgileri
                //17 //23
                list1.Add(compInfo.MonitorInfo.Marka);
                if ((int)compInfo.MonitorInfo.MonType > 0)
                {
                    list1.Add(compInfo.MonitorInfo.MonType);
                }
                else
                {
                    list1.Add("");
                }
                list1.Add(compInfo.MonitorInfo.StokNo);
                list1.Add(compInfo.MonitorInfo.SerialNumber);
                list1.Add(compInfo.MonitorInfo.Parca_no);
                list1.Add(compInfo.MonitorInfo.Tempest);
                list1.Add(compInfo.MonitorInfo.MonSize.MonitorLength);
                //17 //23
                // Monitor Bilgileri

                foreach (var oemDeviceModel in compInfo.OemDevicesVModel.OemDevicesAll)
                {
                    list1.Add(oemDeviceModel.DevOem.SerialNumber);
                    list1.Add(oemDeviceModel.DevOem.DeviceInfo);
                }

                dt.Rows.Add(list1.ToArray());
            }
            return dt;
        }
        // Convenience function to add a column to a DataTable
        // Excellent when you need to add extra fields to database tables
        // or when you need to do in-memory merges, computing, deletion, etc
        private void addColumnToDatatable(
            ref DataTable table, // reference to the actual datatable
            string fieldName,    // field name you want to add
            string type,         // Data type just as c# se
            object defaultValue)
        {
            DataColumn auxColumn = new DataColumn(fieldName, System.Type.GetType(type));
            auxColumn.DefaultValue = defaultValue;
            table.Columns.Add(auxColumn);
        }
    }
}
