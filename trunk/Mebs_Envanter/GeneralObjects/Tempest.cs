using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEBS_Envanter;

namespace Mebs_Envanter.GeneralObjects
{
    public class Tempest : MebsBaseObject
    {

        public override string ToString()
        {
            return TempestName;
        }


        public Tempest() { }

        public Tempest(int _id, String name) {

            Id = _id;
            TempestName = name;
        }

        private int id = -1;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }


        private String tempestName = "";

        public String TempestName
        {
            get { return tempestName; }
            set { tempestName = value; OnPropertyChanged("TempestName"); }
        }
    }
}
