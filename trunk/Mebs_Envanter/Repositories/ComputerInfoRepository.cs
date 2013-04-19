using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MEBS_Envanter.Repositories
{
    public class ComputerInfoRepository : MebsBaseObject
    {
        private ObservableCollection<ComputerInfo> computers = new ObservableCollection<ComputerInfo>();
        public ObservableCollection<ComputerInfo> Computers
        {
            get { return computers; }
        }



        internal ComputerInfoRepository getSearchRepository(String searchText)
        {


            ComputerInfoRepository repNew = new ComputerInfoRepository();
            if (!String.IsNullOrEmpty(searchText) && searchText.Length > 0)
            {
                String[] splitted = { searchText }; //searchText.Split(',');
                foreach (String itemSplittedStr in splitted)
                {
                    foreach (ComputerInfo item in this.Computers)
                    {
                        if (item.Pc_adi.ToLower().Contains(itemSplittedStr.ToLower()) ||
                            item.Senet.Alan_kisi_rutbe.ToLower().Contains(itemSplittedStr.ToLower()) ||
                            item.Senet.Alan_kisi_isim.ToLower().Contains(itemSplittedStr.ToLower()) ||
                            item.Senet.Alan_kisi_komutanlik.Komutanlik_ismi.ToLower().Contains(itemSplittedStr.ToLower()) ||
                            item.Senet.Veren_kisi_isim.ToLower().Contains(itemSplittedStr.ToLower()))
                        {
                            if (!repNew.Computers.Contains(item))
                            {
                                repNew.Computers.Add(item);
                            }
                        }
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
