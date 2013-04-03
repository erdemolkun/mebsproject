using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MEBS_Envanter.Repositories
{
    public class ComputerInfoRepository:MebsBaseObject
    {
        private ObservableCollection<ComputerInfo> computers = new ObservableCollection<ComputerInfo>();
        public ObservableCollection<ComputerInfo> Computers
        {
            get { return computers; }
        }



        internal ComputerInfoRepository getSearchRepository(String searchText)
        {

            if (!String.IsNullOrEmpty(searchText) && searchText.Length > 0)
            {
                ComputerInfoRepository repNew = new ComputerInfoRepository();
                foreach (ComputerInfo item in this.Computers)
                {
                    if (item.Pc_adi.ToLower().Contains(searchText.ToLower()) ||
                        item.Senet.Alan_kisi_isim.ToLower().Contains(searchText.ToLower()) ||
                        item.Senet.Alan_kisi_komutanlik.Komutanlik_ismi.ToLower().Contains(searchText.ToLower()) ||
                        item.Senet.Veren_kisi_isim.ToLower().Contains(searchText.ToLower())
                        )
                    {
                        repNew.Computers.Add(item);
                    }
                }
                return repNew;
            }
            else
            {
                return this;
            }
        }
    }
}
