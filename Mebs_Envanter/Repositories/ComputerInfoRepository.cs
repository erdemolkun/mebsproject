using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Mebs_Envanter.Repositories
{
    public class ComputerInfoRepository : BaseRepository<ComputerInfo>
    {        
        internal ComputerInfoRepository getSearchRepository(String searchText)
        {
            ComputerInfoRepository repNew = new ComputerInfoRepository();
            if (!String.IsNullOrEmpty(searchText) && searchText.Length > 0)
            {
                String[] splitted = { searchText }; //searchText.Split(',');
                foreach (String itemSplittedStr in splitted)
                {
                    foreach (ComputerInfo item in this.Collection)
                    {
                        if (item.Pc_adi.ToLower().Contains(itemSplittedStr.ToLower()) ||
                            item.Senet.Alan_kisi_rutbe.ToLower().Contains(itemSplittedStr.ToLower()) ||
                            item.Senet.Alan_kisi_isim.ToLower().Contains(itemSplittedStr.ToLower()) ||
                            item.Senet.Alan_kisi_komutanlik.Komutanlik_ismi.ToLower().Contains(itemSplittedStr.ToLower()))
                        {
                            if (!repNew.Collection.Contains(item))
                            {
                                repNew.Collection.Add(item);
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
