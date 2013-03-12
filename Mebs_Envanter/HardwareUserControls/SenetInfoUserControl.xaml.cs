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
using MEBS_Envanter.GeneralObjects;
using MEBS_Envanter;

namespace Mebs_Envanter.HardwareUserControls
{
    /// <summary>
    /// Interaction logic for SenetInfoUserControl.xaml
    /// </summary>
    public partial class SenetInfoUserControl : UserControl
    {
        public SenetInfoUserControl()
        {
            InitializeComponent();
        }

        private void senetBirlikCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo_senet = sender as ComboBox;
            KisimRepository kisim_rep = new KisimRepository();
            kisim_rep.FillKisimlar((combo_senet.SelectedItem as Birlik));
            senetKisimCombo.ItemsSource = kisim_rep.Kisimlar;
            KisimRepository.INSTANCE = kisim_rep;
        }

        public void Init() {

            // Birlikler arayüze atanıyor
            BirlikRepository Birlik_Repository = new BirlikRepository();
            Birlik_Repository.FillBirlikler();
            senetBirlikCombo.ItemsSource = Birlik_Repository.Birlikler;
            BirlikRepository.INSTANCE = Birlik_Repository;
        }

        public void SetSenetInfo(SenetInfo inf) {
            
            // Senet Bilgileri
            if (senetRutbelerCombo.SelectedItem != null)
            {
                inf.Alan_kisi_rutbe = senetRutbelerCombo.SelectedItem.ToString();
            }
            inf.Alan_kisi_isim = senetAlamIsimTextBox.Text.Trim().ToString();
            inf.Veren_kisi_isim = senetVerenKisiTextBox.Text.Trim().ToString();
            if (senetBirlikCombo.SelectedItem != null)
            {
                inf.Alan_kisi_birlik = (senetBirlikCombo.SelectedItem as Birlik);
            }
            if (senetKisimCombo.SelectedItem != null)
            {
                inf.Alan_kisi_kisim = (senetKisimCombo.SelectedItem as Kisim);
            }
        }
    }
}
