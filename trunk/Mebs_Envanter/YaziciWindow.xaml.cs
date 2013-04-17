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
using Mebs_Envanter.Hardware;
using System.Diagnostics;
using MEBS_Envanter.Repositories;
using MEBS_Envanter.DB;
using System.Data.SqlClient;
using System.Data;
using Mebs_Envanter.DB;
using MEBS_Envanter;
using System.ComponentModel;
using Mebs_Envanter.GeneralObjects;
using MEBS_Envanter.GeneralObjects;
using Mebs_Envanter.Repositories;
using Mebs_Envanter.PrintOperations;

namespace Mebs_Envanter
{
    /// <summary>
    /// Interaction logic for YaziciWindow.xaml
    /// </summary>
    public partial class YaziciWindow : Window
    {
        public YaziciWindow()
        {
            InitializeComponent();
            InitItems();

            Current_YaziciInfo = new YaziciInfo();            
            yaziciList.DataContext = new YaziciInfoRepository();
            RefreshPrinterList(null, true);
            SetContextForSearchFields();
        }

        private void InitItems()
        {
            yaziciUserControl1.Init();
            senetInfoControl1.Init();
            networkInfoControl1.Init();
        }
        private bool AddOrEditYaziciFunction(bool isEdit)
        {

            try
            {
                YaziciInfo infYazici = new YaziciInfo();

                YaziciInfo currentYazici = yaziciList.SelectedItem as YaziciInfo;

                if (currentYazici == null)
                {
                    Current_YaziciInfo = new YaziciInfo();
                }
                else
                {
                    Current_YaziciInfo = currentYazici;
                }
                //if (currentYazidi != null)
                {
                    AssignYaziciInfoByGui(Current_YaziciInfo, infYazici, isEdit);

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                    Mouse.OverrideCursor = Cursors.Wait;
                    IsBusy = true;
                    infYazici.isEdit = isEdit;
                    worker.RunWorkerAsync(infYazici);
                    return true;
                }
            }
            catch (Exception) { return false; }


        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            IsBusy = false;
            YaziciDbWorkInfo addInfo = e.Result as YaziciDbWorkInfo;
            if (addInfo == null)
            {

                InfoWindow.ShowMessage(this, "Hata");
                return;
            }

            YaziciInfoRepository yaziciRep = (yaziciList.DataContext as YaziciInfoRepository);

            if (addInfo.yazici.isEdit)
            {
                int index = yaziciRep.Yazicilar.IndexOf(Current_YaziciInfo);
                yaziciRep.Yazicilar[index] = addInfo.yazici;

                Current_YaziciInfo = addInfo.yazici;
                yaziciList.SelectedItem = Current_YaziciInfo;
                gridYaziciBilgileri.DataContext = Current_YaziciInfo;

            }
            else
            {
                yaziciRep.Yazicilar.Add(addInfo.yazici);
                yaziciList.SelectedItem = addInfo.yazici;
            }
        }

        internal class YaziciDbWorkInfo
        {

            public bool isSuccess = false;
            //public bool isEdit = false;
            public YaziciInfo yazici = null;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            YaziciInfo yaziciInfo = e.Argument as YaziciInfo;
            bool dbres2 = DBFunctions.InsertOrUpdateSenet(-1, yaziciInfo.SenetInfo, yaziciInfo.isEdit);
            bool dbresult = DBFunctions.InsertOrUpdateOemDevice(yaziciInfo, -1, yaziciInfo.isEdit);
            if (dbresult)
            {
                //bool dbresult2 = DBFunctions.InsertOrUpdateYazici(yaziciInfo, yaziciInfo.isEdit);
                YaziciDbWorkInfo addInfo = new YaziciDbWorkInfo();
                addInfo.yazici = yaziciInfo;
                addInfo.isSuccess = dbresult;
                //e.Result = dbresult;
                e.Result = addInfo;
            }
        }

        private void AssignYaziciInfoByGui(YaziciInfo current, YaziciInfo toAssign, bool isEdit)
        {

            yaziciUserControl1.SetYaziciInfo(toAssign);
            senetInfoControl1.SetSenetInfo(toAssign.SenetInfo);
            networkInfoControl1.SetNetworkInfo(toAssign.NetworkInfo);

            if (isEdit)
            {
                toAssign.Id = current.Id;
                toAssign.Yaz_id = current.Yaz_id;
                toAssign.SenetInfo.Id = current.SenetInfo.Id;
            }
        }

        private void SetContextForSearchFields()
        {
            KomutanlikRepository Rep_Komutanllik = new KomutanlikRepository();
            Rep_Komutanllik.FillKomutanliklar(true);
            searchGridKomutanliklarCombo.ItemsSource = Rep_Komutanllik.Komutanliklar;

            BagliAgRepository rep_bagli_ag = new BagliAgRepository();
            rep_bagli_ag.Fill_Aglar(true);
            searchGridAglarCombo.ItemsSource = rep_bagli_ag.BagliAglar;

            TempestRepository tempest_rep = new TempestRepository();
            tempest_rep.FillSeviyeler(true);
            searchGridTempestCombo.ItemsSource = tempest_rep.TempestSeviyeler;

            MarkaRepository marka_rep = new MarkaRepository();
            marka_rep.FillMarkalar(true);
            searchGridMarkalarCombo.ItemsSource = marka_rep.Markalar;


        }

        private void RefreshPrinterList(SortedList<String, object> parameterList, bool selectLast)
        {

            Stopwatch w = Stopwatch.StartNew();
            YaziciInfoRepository repositoryNew = new YaziciInfoRepository();
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();

            //String commandText = "Select TOP 10 * From tbl_yazici pc order by yazici_id Desc";
            String commandText = "p_yazici_arama";
            SqlCommand cmd = new SqlCommand(commandText, cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameterList != null)
            {
                foreach (var item in parameterList)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }
            }

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            bool res = GlobalDataAccess.Open_SQL_Connection(cnn);
            try
            {
                adp.Fill(dt);
                foreach (DataRow rowPC in dt.Rows)
                {
                    try
                    {
                        int parca_id = DBValueHelpers.GetInt32(rowPC["parca_id"].ToString(), -1);
                        List<OEMDevice> devs = OEMDevice.GetOemDevices(cnn, false, -1, parca_id);
                        YaziciInfo tempYazici = null;
                        foreach (var item in devs)
                        {
                            if (item is YaziciInfo)
                            {
                                tempYazici = item as YaziciInfo;
                            }
                        }

                        tempYazici.SetGeneralFields(rowPC);
                        repositoryNew.Yazicilar.Add(tempYazici);
                    }
                    catch (Exception) { }

                }
                yaziciList.DataContext = repositoryNew;
                if (selectLast)
                {
                    yaziciList.SelectedIndex = repositoryNew.Yazicilar.Count - 1;
                }
                else
                {
                    yaziciList.SelectedIndex = -1;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
            }

            long x = w.ElapsedMilliseconds;
            Console.WriteLine("Yazıcı listesi " + x + " milisaniye içinde yenilendi");
        }

        private void yaziciList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListBox list = sender as ListBox;
            YaziciInfo infYazici = null;
            if (list.SelectedItem != null)
            {
                infYazici = list.SelectedItem as YaziciInfo;
            }
            else
            {
                infYazici = new YaziciInfo();
            }
            list.ScrollIntoView(infYazici);
            Current_YaziciInfo = infYazici;
            gridYaziciBilgileri.DataContext = Current_YaziciInfo;
        }

        private YaziciInfo Current_YaziciInfo = new YaziciInfo();

        private void yaziciAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditYaziciFunction(false);
        }

        private void yaziciEdit_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditYaziciFunction(true);
        }

        private void yaziciDelete_Click(object sender, RoutedEventArgs e)
        {
            bool isSuccess = DBFunctions.DeleteYazici(Current_YaziciInfo);
            if (isSuccess)
            {
                YaziciInfoRepository currentInfoRep = (yaziciList.DataContext as YaziciInfoRepository);
                if (currentInfoRep != null)
                {
                    currentInfoRep.Yazicilar.Remove(Current_YaziciInfo);
                }
            }
        }

        private void refreshListBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshPrinterList(null, true);
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(YaziciWindow), new UIPropertyMetadata(false));

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            searchGridAglarCombo.SelectedIndex = -1;
            searchGridalanKisiIsimTxtBox.Text = "";
            searchGridBirliklerCombo.SelectedIndex = -1;
            searchGridMarkalarCombo.SelectedIndex = -1;
            searchGridModelTxtBox.Text = "";


            searchGridTempestCombo.SelectedIndex = -1;
            searchGridSerialNumberTxtBox.Text = "";

            searchGridKomutanliklarCombo.SelectedIndex = -1;
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshPrinterList(GetParameterListForSearch(), false);
        }
        private SortedList<String, object> GetParameterListForSearch()
        {
            //parca_no

            SortedList<String, object> list = new SortedList<string, object>();
            if (searchGridKomutanliklarCombo.SelectedItem != null)
            {
                if ((searchGridKomutanliklarCombo.SelectedItem as Komutanlik).Komutanlik_id > 0)
                {
                    list.Add("@komutanlik_id", (searchGridKomutanliklarCombo.SelectedItem as Komutanlik).Komutanlik_id);
                }
            }
            if (searchGridBirliklerCombo.SelectedItem != null)
            {
                if ((searchGridBirliklerCombo.SelectedItem as Birlik).Birlik_id > 0)
                {
                    list.Add("@birlik_id", (searchGridBirliklerCombo.SelectedItem as Birlik).Birlik_id);
                }
            }
            if (searchGridAglarCombo.SelectedItem != null)
            {
                if ((searchGridAglarCombo.SelectedItem as BagliAg).Ag_id > 0)
                {
                    list.Add("@bagli_ag_id", (searchGridAglarCombo.SelectedItem as BagliAg).Ag_id);
                }
            }
            if (searchGridTempestCombo.SelectedItem != null)
            {
                if ((searchGridTempestCombo.SelectedItem as Tempest).Id > 0)
                {
                    list.Add("@tempest_id", (searchGridTempestCombo.SelectedItem as Tempest).Id);
                }
            }
            if (searchGridMarkalarCombo.SelectedItem != null)
            {
                if ((searchGridMarkalarCombo.SelectedItem as Marka).MarkaID > 0)
                {
                    list.Add("@marka_id", (searchGridMarkalarCombo.SelectedItem as Marka).MarkaID);
                }
            }

            String alan_kisi_isim = searchGridalanKisiIsimTxtBox.Text.Trim().ToString();
            if (!String.IsNullOrEmpty(alan_kisi_isim))
            {
                list.Add("@alan_kisi_isim", alan_kisi_isim);
            }

            String pcModel = searchGridModelTxtBox.Text.Trim().ToString();
            if (!String.IsNullOrEmpty(pcModel))
            {
                list.Add("@yazici_modeli", pcModel);
            }

            String serial = searchGridSerialNumberTxtBox.Text.Trim().ToString();
            if (!String.IsNullOrEmpty(serial))
            {
                list.Add("@seri_no", serial);
            }
            return list;
        }

        private void searchGridKomutanliklarCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo_senet = sender as ComboBox;
            BirlikRepository birlik_rep = new BirlikRepository();
            birlik_rep.FillBirlikler((combo_senet.SelectedItem as Komutanlik),true);
            searchGridBirliklerCombo.ItemsSource = birlik_rep.Birlikler;
            BirlikRepository.INSTANCE = birlik_rep;
        }

        private void hakkindaMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow.ShowAbout(this);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void printSenetPreview_Click(object sender, RoutedEventArgs e)
        {
            if (yaziciList.SelectedItem != null)
            {
                SystemPrint printFunc = new SystemPrint(yaziciList.SelectedItem as ComputerInfo);
                printFunc.Print(true);
            }
        }

        private void printSenet_Click(object sender, RoutedEventArgs e)
        {
            if (yaziciList.SelectedItem != null)
            {
                SystemPrint printFunc = new SystemPrint(yaziciList.SelectedItem as ComputerInfo);
                printFunc.Print(false);
            }
        }
    }
}
