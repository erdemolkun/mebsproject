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
using MEBS_Envanter;
using Mebs_Envanter.GeneralObjects;
using Mebs_Envanter.Repositories;
using MEBS_Envanter.Repositories;

namespace Mebs_Envanter.HardwareUserControls
{
    /// <summary>
    /// Interaction logic for YaziciUserControl.xaml
    /// </summary>
    public partial class YaziciUserControl : UserControl
    {
        public YaziciUserControl()
        {
            InitializeComponent();
        }

        public void SetYaziciInfo(YaziciInfo inf)
        {
            inf.YaziciModeli = yaziciModeliTextBox.Text.Trim().ToString();
            inf.Marka = yaziciMarkalarCombo.SelectedItem as Marka;
            inf.Tempest = yaziciTempestCombo.SelectedItem as Tempest;
            inf.SerialNumber = yaziciSerialTextBox.Text.Trim().ToString();
        }

        public void Init()
        {

            // Markalar arayüze atanıyor
            MarkaRepository Marka_Repository = new MarkaRepository();
            Marka_Repository.FillMarkalar(false);
            //monitorMarkalarCombo.DataContext = Marka_Repository;
            yaziciMarkalarCombo.ItemsSource = Marka_Repository.Markalar;
            MarkaRepository.INSTANCE = Marka_Repository;


            TempestRepository Rep_Tempest = new TempestRepository();
            Rep_Tempest.FillSeviyeler(false);
            yaziciTempestCombo.ItemsSource = Rep_Tempest.TempestSeviyeler;
            TempestRepository.INSTANCE = Rep_Tempest;

        }

    }
}
