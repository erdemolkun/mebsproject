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
using MEBS_Envanter;

namespace Mebs_Envanter.HardwareUserControls
{
    /// <summary>
    /// Interaction logic for GeneralInfoUserControl.xaml
    /// </summary>
    public partial class GeneralInfoUserControl : UserControl
    {
        public GeneralInfoUserControl()
        {
            InitializeComponent();
        }
        public void Init() {

            genelBilgilerMarkalarCombo.ItemsSource = MarkaRepository.INSTANCE.Markalar;
        
        }
        public void SetGeneralInfo(ComputerInfo inf) {

            inf.Pc_adi = pcAdiTextBox.Text.Trim().ToString();
            inf.Model = pcModelTextBox.Text.Trim().ToString();
            inf.PcStokNo = pcStokNoTextBox.Text.Trim().ToString();
            inf.DeviceNo = pcParcaNoTextBox.Text.Trim().ToString();
            inf.SerialNumber = pcSerialNumberTextBox.Text.Trim().ToString();
            inf.Marka = genelBilgilerMarkalarCombo.SelectedItem as Marka;
        
        }
    }
}
