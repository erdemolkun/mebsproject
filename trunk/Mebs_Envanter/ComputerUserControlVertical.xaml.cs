using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mebs_Envanter;

namespace Mebs_Envanter
{
    /// <summary>
    /// Interaction logic for ComputerUserControlVertical.xaml
    /// </summary>
    public partial class ComputerUserControlVertical : Window
    {
        public ComputerUserControlVertical()
        {
            InitializeComponent();
            computerInfoControl.Init();            
            DataContextChanged += new DependencyPropertyChangedEventHandler(ComputerUserControlVertical_DataContextChanged);    
        }

        void ComputerUserControlVertical_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ComputerInfo)
            {
                computerInfoControl.SetDataContext(DataContext as ComputerInfo);
            }
            Title = (DataContext as ComputerInfo).Pc_adi +" No'lu Bilgisayar Özellikleri";
        }

    }
}
