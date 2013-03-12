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
    /// Interaction logic for MonitorInfoUserControl.xaml
    /// </summary>
    public partial class MonitorInfoUserControl : UserControl
    {
        public MonitorInfoUserControl()
        {
            InitializeComponent();
        }
        public void SetMonitorInfo(Monitor inf)
        {
            // Monitor Bilgileri
            inf.Marka = monitorMarkalarCombo.SelectedItem as Marka;
            inf.Tempest = monitorTempestCombo.SelectedItem as Tempest;
            inf.SerialNumber = monitorSerialTextBox.Text.Trim().ToString();
            inf.StokNo = monitorStokNoTextBox.Text.Trim().ToString();
            inf.Parca_no = monitorParcaNoTextBox.Text.Trim().ToString();
            inf.MonType = (MonitorTypes)monitorTiplerCombo.SelectedItem;
        }
        public void Init() {

            // Markalar arayüze atanıyor
            MarkaRepository Marka_Repository = new MarkaRepository();
            Marka_Repository.FillMarkalar();            
            //monitorMarkalarCombo.DataContext = Marka_Repository;
            monitorMarkalarCombo.ItemsSource = Marka_Repository.Markalar;
            MarkaRepository.INSTANCE = Marka_Repository;


            TempestRepository Rep_Tempest = new TempestRepository();
            Rep_Tempest.FillSeviyeler();
            monitorTempestCombo.ItemsSource = Rep_Tempest.TempestSeviyeler;
            TempestRepository.INSTANCE = Rep_Tempest;

        }
    }
}
