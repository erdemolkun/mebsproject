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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mebs_Envanter
{
    /// <summary>
    /// Interaction logic for ComputerUserControlV.xaml
    /// </summary>
    public partial class ComputerUserControlV : ComputerInfoUserControlBase
    {
        public ComputerUserControlV()
        {
            InitializeComponent();
        }
        public override void Init()
        {
            monitorUserControl1.Init();
            generalInfoUserControl1.Init();
            senetInfoUserControl1.Init();
            networkUserControl1.Init();
        }
        public override void SetDataContext(object context)
        {
            mainGrid.DataContext = context;
        }
    }
}
