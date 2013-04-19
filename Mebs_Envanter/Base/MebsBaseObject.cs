using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
namespace Mebs_Envanter
{

    public class MebsBaseObject :INotifyPropertyChanged
    {

        /// <summary>
        /// Handles PropertyChangedCallback
        /// </summary>
        /// <param name="target">Target Object</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        protected static void ValueInvalidated(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            //MebsBaseObject bs = target as MebsBaseObject;
            //if (bs != null)
            //{
            //    bs.OnPropertyChanged(e.Property.Name);
            //}
        }

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Property Changed Event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises Property Changed event
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
    
}
