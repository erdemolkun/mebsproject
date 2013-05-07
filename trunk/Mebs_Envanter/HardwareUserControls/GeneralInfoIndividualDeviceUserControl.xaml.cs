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

            if (SelectedIndividual.ExtraDeviceType == ExtraDeviceTypes.PRINTER) {

                (inf as YaziciInfo).YaziciTipi = printerTypesCombo.SelectedItem as PrinterType;
            }
        }

        public void Init()
        {
            // Markalar arayüze atanıyor
            MarkaRepository Marka_Repository = new MarkaRepository();
            Marka_Repository.Fetch_Markalar(false);
            markalarCombo.ItemsSource = Marka_Repository.Collection;
            MarkaRepository.INSTANCE = Marka_Repository;

            TempestRepository Rep_Tempest = new TempestRepository();
            Rep_Tempest.Fetch_Seviyeler(false);
            tempestCombo.ItemsSource = Rep_Tempest.Collection;
            TempestRepository.INSTANCE = Rep_Tempest;

            PrinterTypesRepository Rep_Types = new PrinterTypesRepository();
            Rep_Types.Fetch_PrinterTypes(false);
            printerTypesCombo.ItemsSource = Rep_Types.Collection;
            PrinterTypesRepository.INSTANCE = Rep_Types;
        }

        public IndividualDevice SelectedIndividual
        {
            get { return (IndividualDevice)GetValue(SelectedIndividualProperty); }
            set { SetValue(SelectedIndividualProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndividual.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndividualProperty =
            DependencyProperty.Register("SelectedIndividual", typeof(IndividualDevice), typeof(GeneralInfoIndividualDeviceUserControl), new UIPropertyMetadata(null));
    }
}
