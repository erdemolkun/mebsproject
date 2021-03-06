﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mebs_Envanter.Hardware
{
    /// <summary>
    /// Yazıcı , Projeksiyon , Tarayıcı gibi bilgisayara bağımlı olmayan cihazların alt sınıfı.
    /// </summary>
    public class IndividualDeviceInfo : OEMDevice
    {
        public bool isEdit = false;
        public bool PropertiesFetched = false;
        public virtual void Fetch()
        {
            if (!PropertiesFetched)
            {                
                Senet.Set_SenetInfosDB();
                PropertiesFetched = true;
            }
        }

        public IndividualDeviceInfo()
        {

        }
        //public IndividualDeviceInfo(DeviceTypes type)
        //    : base(type)
        //{

        //}

        private int id_Dev = -1;
        public int Id_Dev
        {
            get { return id_Dev; }
            set { id_Dev = value; OnPropertyChanged("Id_Dev"); }
        }

    }
}
