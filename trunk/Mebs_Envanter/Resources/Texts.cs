using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.Resources
{
    public class Texts : MebsBaseObject
    {
        public static Texts INSTANCE = new Texts();

        public void ChangeLanguage() {

            
        }

        private String deleteStr = "Sil";
        public String DeleteStr
        {
            get { return deleteStr; }
            set { deleteStr = value; OnPropertyChanged("DeleteStr"); }
        }

        private String aboutStr = "Hakkında";
        public String AboutStr
        {
            get { return aboutStr; }
            set { aboutStr = value; OnPropertyChanged("AboutStr"); }
        }

        private String notePreviewStr = "Senet Önizleme";
        public String NotePreviewStr
        {
            get { return notePreviewStr; }
            set { notePreviewStr = value; OnPropertyChanged("NotePreviewStr"); }
        }

        private String notePrintStr = "Senet Yazdır";

        public String NotePrintStr
        {
            get { return notePrintStr; }
            set { notePrintStr = value; OnPropertyChanged("NotePrintStr"); }
        }

        private String exitStr = "Çıkış";

        public String ExitStr
        {
            get { return exitStr; }
            set { exitStr = value; OnPropertyChanged("ExitStr"); }
        }

        private String printOptionsStr = "Yazdırma Seçenekleri";

        public String PrintOptionsStr
        {
            get { return printOptionsStr; }
            set { printOptionsStr = value; OnPropertyChanged("PrintOptionsStr"); }
        }

        private String markaStr = "Marka";

        public String MarkaStr
        {
            get { return markaStr; }
            set { markaStr = value; OnPropertyChanged("MarkaStr"); }
        }

        private String tempestLevelStr = "Tempest Seviyesi";

        public String TempestLevelStr
        {
            get { return tempestLevelStr; }
            set { tempestLevelStr = value; OnPropertyChanged("TempestLevelStr"); }
        }


        private String serialNumberStr = "Seri No";

        public String SerialNumberStr
        {
            get { return serialNumberStr; }
            set { serialNumberStr = value; OnPropertyChanged("SerialNumberStr"); }
        }


        private String modelStr = "Model";

        public String ModelStr
        {
            get { return modelStr; }
            set { modelStr = value; OnPropertyChanged("ModelStr"); }
        }


        private String parcaNoStr = "Parça No";

        public String ParcaNoStr
        {
            get { return parcaNoStr; }
            set { parcaNoStr = value; OnPropertyChanged("ParcaNoStr"); }
        }


        private String monitorStr = "Monitör";

        public String MonitorStr
        {
            get { return monitorStr; }
            set { monitorStr = value; OnPropertyChanged("MonitorStr"); }
        }

        private String minimizeStr = "Simge Durumuna Küçült";

        public String MinimizeStr
        {
            get { return minimizeStr; }
            set { minimizeStr = value; OnPropertyChanged("MinimizeStr"); }
        }
    }
}
