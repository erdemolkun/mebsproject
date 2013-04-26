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
using Mebs_Envanter.Hardware;
using Mebs_Envanter.GeneralObjects;
using Mebs_Envanter.Repositories;

namespace Mebs_Envanter.HardwareUserControls
{
    /// <summary>
    /// Interaction logic for GeneralInfoIndividualDeviceUserControl.xaml
    /// </summary>
    public partial class GeneralInfoIndividualDeviceUserControl : UserControl
    {
        public GeneralInfoIndividualDeviceUserControl()
        {
            InitializeComponent();
        }
        public void SetGeneralInfo(IndividualDeviceInfo inf)
        {
            inf.Model = yaziciModeliTextBox.Text.Trim().ToString();
            inf.Marka = markalarCombo.SelectedItem as Marka;
            inf.Tempest = tempestCombo.SelectedItem as Tempest;
            inf.SerialNumber = serialTextBox.Text.Trim().ToString();
        }

        public void Init()
        {
            // Markalar arayüze atanıyor
            MarkaRepository Marka_Repository = new MarkaRepository();
            Marka_Repository.FillMarkalar(false);
            markalarCombo.ItemsSource = Marka_Repository.Markalar;
            MarkaRepository.INSTANCE = Marka_Repository;

            TempestRepository Rep_Tempest = new TempestRepository();
            Rep_Tempest.FillSeviyeler(false);
            tempestCombo.ItemsSource = Rep_Tempest.TempestSeviyeler;
            TempestRepository.INSTANCE = Rep_Tempest;
        }
    }
}
