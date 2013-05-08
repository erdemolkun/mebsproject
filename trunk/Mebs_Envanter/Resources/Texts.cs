using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.Resources
{
    public class Texts : MebsBaseObject
    {
        public static Texts INSTANCE = new Texts();

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
    }
}
