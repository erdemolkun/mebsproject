using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Mebs_Envanter.Repositories
{
    public class BaseRepository<T>
    {
        public BaseRepository() { }
        private ObservableCollection<T> collection = new ObservableCollection<T>();
        public ObservableCollection<T> Collection
        {
            get { return collection; }
        }

        public virtual void Fill(bool isForSearch) { }
    }
}
