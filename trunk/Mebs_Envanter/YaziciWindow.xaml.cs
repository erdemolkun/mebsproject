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
using Mebs_Envanter.Repositories;
using Mebs_Envanter.DB;
using System.Data.SqlClient;
using System.Data;
using Mebs_Envanter;
using System.ComponentModel;
using Mebs_Envanter.GeneralObjects;
using Mebs_Envanter.PrintOperations;
using Mebs_Envanter.AllVisuals;
using Mebs_Envanter.Base;
using Mebs_Envanter.Helpers;
using System.Data.Common;

namespace Mebs_Envanter
{
    /// <summary>
    /// Interaction logic for YaziciWindow.xaml
    /// </summary>
    public partial class YaziciWindow : MebsWindow
    {
        public YaziciWindow()
        {
            InitializeComponent();
            this.Title = "Bağımsız Cihaz Envanter Kaydı    " + VersionInfo.versiyonStr;
            OnDbInitialized += new DBProviderInitializedHandler(YaziciWindow_OnDbInitialized);
        }

        private IndividualDeviceInfo GetNewDevice()
        {
            if (SelectedIndividual.ExtraDeviceType == ExtraDeviceTypes.PRINTER)
            {
                return new YaziciInfo();
            }
            else if (SelectedIndividual.ExtraDeviceType == ExtraDeviceTypes.PROJECTION)
            {
                return new ProjectionInfo();
            }
            else if (SelectedIndividual.ExtraDeviceType == ExtraDeviceTypes.SCANNER)
            {
                return new ScannerInfo();
            }
            return null;
        }

        void YaziciWindow_OnDbInitialized()
        {
            InitItems();
            Current_IndividualDeviceInfo = GetNewDevice();
            individualDevicesList.DataContext = new IndividualDeviceRepository<IndividualDeviceInfo>();
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
                IndividualDeviceInfo infIndividualDevice = GetNewDevice();
                IndividualDeviceInfo currentDevice = individualDevicesList.SelectedItem as IndividualDeviceInfo;
                if (currentDevice != null)
                    Current_IndividualDeviceInfo = currentDevice;

                AssignIndividualDeviceInfoByGui(Current_IndividualDeviceInfo, infIndividualDevice, isEdit);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                Mouse.OverrideCursor = Cursors.Wait;
                IsBusy = true;
                infIndividualDevice.isEdit = isEdit;
                worker.RunWorkerAsync(infIndividualDevice);
                return true;
            }
            catch (Exception) { return false; }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = null;
            IsBusy = false;
            IndividualDeviceDbWorkInfo addInfo = e.Result as IndividualDeviceDbWorkInfo;
            if (addInfo == null)
            {
                InfoWindow.ShowMessage(this, "Hata Oluştu");
                return;
            }

            IndividualDeviceRepository<IndividualDeviceInfo> yaziciRep =
                (individualDevicesList.DataContext as IndividualDeviceRepository<IndividualDeviceInfo>);
            if (addInfo.device.isEdit)
            {
                int index = yaziciRep.Collection.IndexOf(Current_IndividualDeviceInfo);
                yaziciRep.Collection[index] = addInfo.device;

                Current_IndividualDeviceInfo = addInfo.device;
                individualDevicesList.SelectedItem = Current_IndividualDeviceInfo;
                gridCihazBilgileri.DataContext = Current_IndividualDeviceInfo;
            }
            else
            {
                yaziciRep.Collection.Insert(0, addInfo.device);
                individualDevicesList.SelectedItem = addInfo.device;
            }
        }

        internal class IndividualDeviceDbWorkInfo
        {
            public bool isSuccess = false;
            public IndividualDeviceInfo device = null;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            IndividualDeviceInfo devInfo = e.Argument as IndividualDeviceInfo;
            bool senetInsertUpdate = DBFunctions.InsertOrUpdateSenet(devInfo.Senet, devInfo.isEdit);
            bool deviceInsertOrUpdate = DBFunctions.InsertOrUpdateOemDevice(devInfo, -1, devInfo.isEdit);
            if (deviceInsertOrUpdate)
            {
                IndividualDeviceDbWorkInfo addInfo = new IndividualDeviceDbWorkInfo();
                addInfo.device = devInfo;
                addInfo.isSuccess = deviceInsertOrUpdate;
                e.Result = addInfo;
            }
        }

        private void AssignIndividualDeviceInfoByGui(IndividualDeviceInfo current, IndividualDeviceInfo toAssign, bool isEdit)
        {
            if (SelectedIndividual.ExtraDeviceType == ExtraDeviceTypes.PRINTER)
            {
                networkInfoControl1.SetNetworkInfo((toAssign as YaziciInfo).NetworkInfo);
            }
            yaziciUserControl1.SetGeneralInfo(toAssign);
            senetInfoControl1.SetSenetInfo(toAssign.Senet);

            if (isEdit)
            {
                toAssign.Id = current.Id;
                toAssign.Id_Dev = current.Id_Dev;
                toAssign.Senet.Id = current.Senet.Id;
            }
        }

        private void SetContextForSearchFields()
        {
            KomutanlikRepository Rep_Komutanllik = new KomutanlikRepository();
            Rep_Komutanllik.FillKomutanliklar(true);
            searchGridKomutanliklarCombo.ItemsSource = Rep_Komutanllik.Collection;

            BagliAgRepository rep_bagli_ag = new BagliAgRepository();
            rep_bagli_ag.Fill_Aglar(true);
            searchGridAglarCombo.ItemsSource = rep_bagli_ag.Collection;

            TempestRepository tempest_rep = new TempestRepository();
            tempest_rep.FillSeviyeler(true);
            searchGridTempestCombo.ItemsSource = tempest_rep.Collection;

            MarkaRepository marka_rep = new MarkaRepository();
            marka_rep.FillMarkalar(true);
            searchGridMarkalarCombo.ItemsSource = marka_rep.Collection;

            PrinterTypesRepository types_rep = new PrinterTypesRepository();
            types_rep.FillPrinterTypes(true);
            searchGridYaziciTiplerCombo.ItemsSource = types_rep.Collection;
        }

        private void RefreshPrinterList(SortedList<String, object> parameterList, bool selectLast)
        {
            Stopwatch w = Stopwatch.StartNew();
            IndividualDeviceRepository<IndividualDeviceInfo> repositoryNew = new IndividualDeviceRepository<IndividualDeviceInfo>();
            DbConnection cnn = GlobalDataAccess.Get_Fresh_Connection();

            //String commandText = "Select TOP 10 * From tbl_yazici pc order by yazici_id Desc";
            //String commandText = "p_yazici_arama";
            String commandText = "p_bagimsiz_cihaz_arama";
            SqlCommand cmd = DBCommonAccess.GetCommand(commandText, cnn) as SqlCommand;
            cmd.CommandType = CommandType.StoredProcedure;

           
            cmd.Parameters.AddWithValue("@parca_tipi", ExtraDeviceTypes.ConvertToDeviceType(SelectedIndividual.ExtraDeviceType));
            if (parameterList != null)
            {
                foreach (var item in parameterList)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }
            }

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            bool res = GlobalDataAccess.Open_DB_Connection(cnn);
            try
            {
                adp.Fill(dt);
                foreach (DataRow rowPC in dt.Rows)
                {
                    try
                    {                        
                        int parca_id = DBValueHelpers.GetInt32(rowPC["base_parca_id"].ToString(), -1);
                        List<OEMDevice> devs = OEMDevice.GetOemDevicesDB(cnn, false, -1, parca_id);
                        IndividualDeviceInfo tempDevice = null;
                        foreach (var item in devs)
                        {
                            if (item is IndividualDeviceInfo)
                            {
                                tempDevice = item as IndividualDeviceInfo;
                            }
                        }
                        if (tempDevice != null)
                        {
                            if (tempDevice is YaziciInfo)
                            {
                                (tempDevice as YaziciInfo).SetGeneralFieldsYazici(rowPC);
                            }
                            repositoryNew.Collection.Add(tempDevice);
                        }
                    }
                    catch (Exception) { }
                }
                individualDevicesList.DataContext = repositoryNew;
                SetSelectedItemAfterContextChange(selectLast);
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

        private void SetSelectedItemAfterContextChange(bool selectLast)
        {
            IndividualDeviceRepository<IndividualDeviceInfo> repositoryNew = (individualDevicesList.DataContext as IndividualDeviceRepository<IndividualDeviceInfo>);
            if (selectLast && repositoryNew.Collection.Count > 0)
            {
                individualDevicesList.SelectedIndex = 0;//repositoryNew.Computers.Count - 1;
            }
            else
            {
                individualDevicesList.SelectedIndex = -1;
            }
        }

        private void individualDevicesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            IndividualDeviceInfo infYazici = null;
            if (list.SelectedItem != null)
            {
                infYazici = list.SelectedItem as IndividualDeviceInfo;
            }
            else
            {
                infYazici = GetNewDevice();
            }
            list.ScrollIntoView(infYazici);
            Current_IndividualDeviceInfo = infYazici;

            foreach (var item in list.SelectedItems)
            {
                IndividualDeviceInfo devItem = item as IndividualDeviceInfo;
                if (devItem != null)
                {
                    devItem.Fetch();
                }
            }
            gridCihazBilgileri.DataContext = Current_IndividualDeviceInfo;
        }

        private IndividualDeviceInfo Current_IndividualDeviceInfo = null;

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
            if (InfoWindow.AskQuestion("Silmek istediğinize emin misiniz?", "Dikkat !!!") != MessageBoxResult.Yes) { return; }
            bool isSuccess = DBFunctions.DeleteIndividualDevice(Current_IndividualDeviceInfo);
            if (isSuccess)
            {
                IndividualDeviceRepository<IndividualDeviceInfo> currentInfoRep = (individualDevicesList.DataContext as IndividualDeviceRepository<IndividualDeviceInfo>);
                if (currentInfoRep != null)
                {
                    currentInfoRep.Collection.Remove(Current_IndividualDeviceInfo);
                    SetSelectedItemAfterContextChange(true);
                }
            }
        }

        private void refreshListBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshPrinterList(null, true);
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            searchGridAglarCombo.SelectedIndex = -1;
            searchGridalanKisiIsimTxtBox.Text = "";
            searchGridBirliklerCombo.SelectedIndex = -1;
            searchGridMarkalarCombo.SelectedIndex = -1;
            searchGridModelTxtBox.Text = "";
            searchGridYaziciTiplerCombo.SelectedIndex = -1;
            searchGridTempestCombo.SelectedIndex = -1;
            searchGridSerialNumberTxtBox.Text = "";
            searchGridKomutanliklarCombo.SelectedIndex = -1;
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshPrinterList(GetParameterListForSearch(), true);
        }

        private SortedList<String, object> GetParameterListForSearch()
        {
            //parca_no
            SortedList<String, object> list = new SortedList<string, object>();
            SearchHelper.AddToListFromFrameworkElement(searchGridKomutanliklarCombo, list, "@komutanlik_id");
            SearchHelper.AddToListFromFrameworkElement(searchGridBirliklerCombo, list, "@birlik_id");
            SearchHelper.AddToListFromFrameworkElement(searchGridAglarCombo, list, "@bagli_ag_id");
            SearchHelper.AddToListFromFrameworkElement(searchGridTempestCombo, list, "@tempest_id");
            SearchHelper.AddToListFromFrameworkElement(searchGridMarkalarCombo, list, "@marka_id");
            SearchHelper.AddToListFromFrameworkElement(searchGridYaziciTiplerCombo, list, "@yazici_tip_id");

            SearchHelper.AddToListFromFrameworkElement(searchGridalanKisiIsimTxtBox, list, "@alan_kisi_isim");
            SearchHelper.AddToListFromFrameworkElement(searchGridModelTxtBox, list, "@model");
            SearchHelper.AddToListFromFrameworkElement(searchGridSerialNumberTxtBox, list, "@seri_no");            
            return list;
        }

        private void searchGridKomutanliklarCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo_senet = sender as ComboBox;
            BirlikRepository birlik_rep = new BirlikRepository();
            birlik_rep.FillBirlikler((combo_senet.SelectedItem as Komutanlik), true);
            searchGridBirliklerCombo.ItemsSource = birlik_rep.Collection;
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
            if (individualDevicesList.SelectedItem != null)
            {
                SystemPrint printFunc = new SystemPrint(individualDevicesList.SelectedItem);
                printFunc.Print(true);
            }
        }

        private void printSenet_Click(object sender, RoutedEventArgs e)
        {
            if (individualDevicesList.SelectedItem != null)
            {
                SystemPrint printFunc = new SystemPrint(individualDevicesList.SelectedItem);
                printFunc.Print(false);
            }
        }

        public IndividualDevice SelectedIndividual
        {
            get { return (IndividualDevice)GetValue(SelectedIndividualProperty); }
            set { SetValue(SelectedIndividualProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndividual.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndividualProperty =
            DependencyProperty.Register("SelectedIndividual", typeof(IndividualDevice), typeof(YaziciWindow), new UIPropertyMetadata(null));

        private void individualDevicesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedIndividual = (sender as ComboBox).SelectedItem as IndividualDevice;
            if (IsLoaded)
            {
                RefreshPrinterList(null, true);
            }
        }
    }
}
