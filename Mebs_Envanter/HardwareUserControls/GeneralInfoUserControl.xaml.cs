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
using Mebs_Envanter.GeneralObjects;

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

            TempestRepository Rep_Tempest = new TempestRepository();
            Rep_Tempest.FillSeviyeler();
            genelBilgilerTempestCombo.ItemsSource = Rep_Tempest.TempestSeviyeler;
            TempestRepository.INSTANCE = Rep_Tempest;
        
        }
        public void SetGeneralInfo(ComputerInfo inf) {

            inf.Pc_adi = pcAdiTextBox.Text.Trim().ToString();
            inf.Model = pcModelTextBox.Text.Trim().ToString();
            inf.PcStokNo = pcStokNoTextBox.Text.Trim().ToString();
            inf.DeviceNo = pcParcaNoTextBox.Text.Trim().ToString();
            inf.SerialNumber = pcSerialNumberTextBox.Text.Trim().ToString();
            inf.Marka = genelBilgilerMarkalarCombo.SelectedItem as Marka;
            inf.Tempest = genelBilgilerTempestCombo.SelectedItem as Tempest;            
            String not = new TextRange(notlarRichTxtBox.Document.ContentStart, notlarRichTxtBox.Document.ContentEnd).Text;
            inf.Notlar = not;            
        }
    }
}
