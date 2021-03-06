﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mebs_Envanter;
using Mebs_Envanter.Repositories;
using Mebs_Envanter.Base;

namespace Mebs_Envanter.GeneralObjects
{
    public class Tempest : MebsBaseDBObject
    {
        public override string ToString()
        {
            return TempestName;
        }
        public Tempest() { }

        public Tempest(int _id, String name) {

            TempestName = name;
            Id = _id;            
        }

        private int id = -1;
        public override int Id
        {
            get { return id; }
            set { 
                id = value;
                if (TempestRepository.INSTANCE != null)
                {
                    foreach (Tempest item in TempestRepository.INSTANCE.Collection)
                    {
                        if (value == item.Id)
                        {
                            if (value < 0) break;
                            TempestName = item.TempestName;
                            break;
                        }
                    }
                }
            }
        }


        private String tempestName = "";
        public String TempestName
        {
            get { return tempestName; }
            set { tempestName = value; OnPropertyChanged("TempestName"); }
        }
    }
}
