using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter.Repositories;
using Mebs_Envanter.Base;

namespace Mebs_Envanter
{
    public class PrinterType : MebsBaseDBObject
    {
        public override string ToString()
        {
            if (Id > 0)
            {
                return TypeName.ToUpper().ToString();// + " ID : " + MarkaID; ;
            }
            else
            {
                return TypeName;
            }
        }
        public PrinterType(int marka_id, String markaName)
        {
            this.TypeName = markaName;
            this.Id = marka_id;
        }

        private String _typeName;
        public String TypeName
        {
            get { return _typeName; }
            set { _typeName = value; OnPropertyChanged("TypeName"); }
        }


        private int _markaID = -1;
        /// <summary>
        /// Markanın db'deki id'si
        /// </summary>
        public override int Id
        {
            get { return _markaID; }
            set
            {
                _markaID = value;
                if (PrinterTypesRepository.INSTANCE != null)
                {
                    foreach (PrinterType item in PrinterTypesRepository.INSTANCE.Collection)
                    {
                        if (value == item.Id)
                        {
                            if (value < 0) break;
                            TypeName = item.TypeName;
                            break;
                        }
                    }
                }
                else if (value > 0)
                {

                }
                OnPropertyChanged("Id");
            }
        }

    }
}
