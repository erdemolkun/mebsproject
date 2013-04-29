using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.Base
{
    public class MebsBaseDBObject : MebsBaseObject
    {
        private int id = -1;

        public virtual int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }
    }
}
