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
using Mebs_Envanter.GeneralObjects;
using MEBS_Envanter.Repositories;

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

        public void Init()
        {
            KomutanlikRepository Komutanlik_Repository = new KomutanlikRepository();
            Komutanlik_Repository.FillKomutanliklar(false);
            senetKomutanlikCombo.ItemsSource = Komutanlik_Repository.Komutanliklar;
            KomutanlikRepository.INSTANCE = Komutanlik_Repository;

            foreach (Komutanlik item in Komutanlik_Repository.Komutanliklar)
            {
                BirlikRepository birlik_rep = new BirlikRepository();
                birlik_rep.FillBirlikler(item, false);
                item.Birlik_Repository = birlik_rep;

                foreach (Birlik itemBirlik in birlik_rep.Birlikler)
                {
                    KisimRepository kisim_rep = new KisimRepository();
                    kisim_rep.FillKisimlar(itemBirlik);
                    itemBirlik.Kisim_Repository = kisim_rep;
                }
            }
        }

        public void SetSenetInfo(SenetInfo inf)
        {
            // Senet Bilgileri
            if (senetRutbelerCombo.SelectedItem != null)
            {
                inf.Alan_kisi_rutbe = senetRutbelerCombo.SelectedItem.ToString();
            }
            inf.Alan_kisi_isim = senetAlamIsimTextBox.Text.Trim().ToString();
            inf.Veren_kisi_isim = senetVerenKisiTextBox.Text.Trim().ToString();
            if (senetKomutanlikCombo.SelectedItem != null)
            {
                inf.Alan_kisi_komutanlik = (senetKomutanlikCombo.SelectedItem as Komutanlik);
            }

            if (senetBirlikCombo.SelectedItem != null)
            {
                inf.Alan_kisi_birlik = (senetBirlikCombo.SelectedItem as Birlik);
            }

            if (senetKisimCombo.SelectedItem != null)
            {
                inf.Alan_kisi_kisim = (senetKisimCombo.SelectedItem as Kisim);
            }
            else if (!String.IsNullOrEmpty(senetKisimCombo.Text.Trim()))
            {
                inf.Alan_kisi_kisim = new Kisim(-1, senetKisimCombo.Text.Trim());
                inf.Alan_kisi_birlik.Kisim_Repository.Kisimlar.Add(inf.Alan_kisi_kisim);
            }
        }

        private void senetKomutanlikCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            ComboBox combo = sender as ComboBox;
            Komutanlik current_Komutanlik = (combo.SelectedItem as Komutanlik);
            BirlikRepository birlik_rep = null;
            if (current_Komutanlik != null && current_Komutanlik.Birlik_Repository != null)
            {
                birlik_rep = current_Komutanlik.Birlik_Repository;
            }
            else
            {
                birlik_rep = new BirlikRepository();
                birlik_rep.FillBirlikler(current_Komutanlik, false);
            }

            if (current_Komutanlik != null && current_Komutanlik.Birlik_Repository == null)
                current_Komutanlik.Birlik_Repository = birlik_rep;

            senetBirlikCombo.ItemsSource = birlik_rep.Birlikler;
            BirlikRepository.INSTANCE = birlik_rep;
        }

        private void senetBirlikCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            Birlik current_Birlik = (combo.SelectedItem as Birlik);

            KisimRepository kisim_rep = new KisimRepository();
            if (current_Birlik != null && current_Birlik.Kisim_Repository != null)
            {
                kisim_rep = current_Birlik.Kisim_Repository;
            }
            else
            {
                kisim_rep.FillKisimlar(current_Birlik);
            }
            if (current_Birlik != null && current_Birlik.Kisim_Repository == null)
                current_Birlik.Kisim_Repository = kisim_rep;

            senetKisimCombo.ItemsSource = kisim_rep.Kisimlar;
            KisimRepository.INSTANCE = kisim_rep;
        }
    }
}
