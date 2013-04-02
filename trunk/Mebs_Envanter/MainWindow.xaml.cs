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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MEBS_Envanter.DB;
using System.Data.SqlClient;
using MEBS_Envanter.GeneralObjects;
using System.Data;
using System.Threading;
using System.Windows.Threading;
using System.Collections;
using Mebs_Envanter.GUIHelpers;
using Mebs_Envanter;
using System.Diagnostics;
using System.ComponentModel;
using Mebs_Envanter.GeneralObjects;
using System.Drawing.Printing;
using Mebs_Envanter.PrintOperations;
using MEBS_Envanter.Repositories;
using Mebs_Envanter.Repositories;


namespace MEBS_Envanter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Thread thSqlInit = new Thread(StartSqlInit);
            thSqlInit.IsBackground = true;
            thSqlInit.Start();            
            IsBusy = true;

        }

        private void StartSqlInit()
        {
            SqlConnection conSql = DBFunctions.proviceConnection();
            Dispatcher.Invoke(DispatcherPriority.DataBind, new Action(delegate()
            {
                try
                {
                    if (conSql != null)
                    {
                        GlobalDataAccess.Set_Current_SQL_Connection(conSql);
                        Current_Computer_Info = new ComputerInfo();
                        setGUIDataContextForInitialization();
                        RefreshComputerList(null, true);
                        pcList_SelectionChanged(pcList, null);
                        IsBusy = false;
                        pcList.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Bağlantı Sağlanamadı. Çıkıyorum");
                        IsBusy = false;
                        //Environment.Exit(0);
                    }
                }
                catch (Exception ex) {

                    LoggerMebs.WriteToFile(ex.Message.ToString());
                }
            }));
        }

        private void SetContextForSearchFields()
        {
            KomutanlikRepository Rep_Komutanllik = new KomutanlikRepository();
            Rep_Komutanllik.FillKomutanliklar();
            searchGridKomutanliklarCombo.ItemsSource = Rep_Komutanllik.Komutanliklar;

            BagliAgRepository rep_bagli_ag = new BagliAgRepository();
            rep_bagli_ag.Fill_Aglar();
            searchGridAglarCombo.ItemsSource = rep_bagli_ag.BagliAglar;

            TempestRepository tempest_rep = new TempestRepository();
            tempest_rep.FillSeviyeler();
            searchGridTempestCombo.ItemsSource = tempest_rep.TempestSeviyeler;

            MarkaRepository marka_rep = new MarkaRepository();
            marka_rep.FillMarkalar();
            searchGridMarkalarCombo.ItemsSource = marka_rep.Markalar;

            MarkaRepository marka_rep2 = new MarkaRepository();
            marka_rep2.FillMarkalar();
            searchGridMonitorMarkalar.ItemsSource = marka_rep2.Markalar;

        }

        private void ShowError(String msg)
        {

            InfoWindow w = new InfoWindow(this);
            w.ShowMessage(msg);
        }

        private bool AddOrEditPCFunction(bool isEdit)
        {
            try
            {
                ComputerInfo freshComputerInfo = new ComputerInfo();
                pcEnvanterControl.Assign_ComputerInfo_By_GUI(Current_Computer_Info, freshComputerInfo, isEdit);

                object count = DBFunctions.ExecuteToFetchSingleItem("Select Count(*) as Count from tbl_bilgisayar where Pc_adi like '" +
                    freshComputerInfo.Pc_adi.Trim().ToString() + "'", "Count");

                if (String.IsNullOrEmpty(freshComputerInfo.Pc_adi.Trim().ToString())) {

                    ShowError("Lütfen Bilgisayar ismini giriniz !!!");
                    return false;
                }

                if (count != null && Convert.ToInt32(count) > 0)
                {
                    if (isEdit && Current_Computer_Info.Pc_adi.Equals(freshComputerInfo.Pc_adi))
                    {

                    }
                    else
                    {
                        ShowError("Aynı isimli bilgisayar mevcut !!!");
                        return false;
                    }
                }                

                freshComputerInfo.IsEdit = isEdit;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                Mouse.OverrideCursor = Cursors.Wait;
                //this.IsEnabled = false;
                IsBusy = true;

                worker.RunWorkerAsync(freshComputerInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;           
            IsBusy = false;
            ComputerDbWorkInfo addInfo = e.Result as ComputerDbWorkInfo;
            ComputerInfoRepository computerRep = (pcList.DataContext as ComputerInfoRepository);
            if (addInfo.computer.IsEdit)
            {
                int index = computerRep.Computers.IndexOf(Current_Computer_Info);
                computerRep.Computers[index] = addInfo.computer;
                /*int index = 0;
                foreach (var itemx in computerRep.Computers)
                {
                    if (itemx.Id == Current_Computer_Info.Id) {
                        computerRep.Computers[index] = addInfo.computer;
                        break;
                    }
                    index++;
                }*/


                /*var item = computerRep.Computers.FirstOrDefault(i => i.Id == Current_Computer_Info.Id);
                if (item != null)
                {
                    item = (addInfo.computer);
                }*/
                //computerRep.Computers.Remove(Current_Computer_Info);
                //computerRep.Computers.Add(addInfo.computer);
                Current_Computer_Info = addInfo.computer;
                pcList.SelectedItem = Current_Computer_Info;
                pcEnvanterControl.SetDataContext(Current_Computer_Info);
                //pcList.SelectedIndex = -1;
                //pcList.SelectedItem = Current_Computer_Info;
            }
            else
            {
                computerRep.Computers.Insert(0, addInfo.computer);
                //computerRep.Computers.Add(addInfo.computer);
                pcList.SelectedItem = addInfo.computer;
            }
            /*if ((bool)e.Result)
            {
                RefreshComputerList(null,true);
            }
            else {
                MessageBox.Show("Hata Oluştu");
            }*/
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ComputerInfo freshComputerInfo = e.Argument as ComputerInfo;
            bool dbresult = DBFunctions.InsertOrUpdateComputerInfo(freshComputerInfo, freshComputerInfo.IsEdit);
            ComputerDbWorkInfo addInfo = new ComputerDbWorkInfo();
            addInfo.computer = freshComputerInfo;
            addInfo.isSuccess = dbresult;
            //e.Result = dbresult;
            e.Result = addInfo;
        }

        internal class ComputerDbWorkInfo
        {

            public bool isSuccess = false;
            //public bool isEdit = false;
            public ComputerInfo computer = null;
        }

        private void setGUIDataContextForInitialization()
        {
            pcEnvanterControl.Init();
            SetContextForSearchFields();
        }

        public ComputerInfo Current_Computer_Info
        {
            get { return (ComputerInfo)GetValue(Current_Computer_InfoProperty); }
            set { SetValue(Current_Computer_InfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Current_Computer_Info.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Current_Computer_InfoProperty =
            DependencyProperty.Register("Current_Computer_Info", typeof(ComputerInfo), typeof(MainWindow), new UIPropertyMetadata(null));

        #region Events

        private void pcList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            ComputerInfo infComp = null;
            if (list.SelectedItem != null)
            {
                infComp = list.SelectedItem as ComputerInfo;
            }
            else
            {
                infComp = new ComputerInfo();
            }
            list.ScrollIntoView(infComp);
            Current_Computer_Info = infComp;
            //pcEnvanterTabControl.DataContext = Current_Computer_Info;
            //pcEnvanterControl.DataContext = Current_Computer_Info;
            foreach (var item in list.SelectedItems)
            {
                ComputerInfo x = item as ComputerInfo;
                if (x != null) {
                    x.Fetch();
                }
            }
            //Current_Computer_Info.Fetch();
            pcEnvanterControl.SetDataContext(Current_Computer_Info);
            
            //changeCurrentPCContext(list.SelectedItem as ComputerInfo );
        }

        private void pcAddBtn_Click(object sender, RoutedEventArgs e)
        {
            PCAddCallerFunction(false);
        }

        private void pcEditBtn_Click(object sender, RoutedEventArgs e)
        {
            PCAddCallerFunction(true);
        }

        private void PCAddCallerFunction(bool isEdit)
        {

            if (AddOrEditPCFunction(isEdit))
            {

            }
            else
            {
               // MessageBox.Show("Hata Oluştu");
            }

        }

        private void pcDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result1 = System.Windows.Forms.MessageBox.Show("Silmek İstediğinize Emin misiniz?",
                    "Önemli Soru !!!",
                System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                System.Windows.Forms.MessageBoxIcon.Question);
            if (result1 != System.Windows.Forms.DialogResult.Yes) { return; }

            bool isSuccess = DBFunctions.DeletePC(Current_Computer_Info);
            if (isSuccess)
            {
                ComputerInfoRepository currentInfoRep = (pcList.DataContext as ComputerInfoRepository);
                if (currentInfoRep != null)
                {

                    currentInfoRep.Computers.Remove(Current_Computer_Info);
                }
                //RefreshComputerList(null,true);
            }
        }
        private void refreshListBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshComputerList(null, true);
            //changeCurrentPCContext((pcList.DataContext as ComputerInfoRepository).Computers[0]);
        }

        #endregion

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshComputerList(GetParameterListForSearch(), false);
        }

        private SortedList<String, object> GetParameterListForSearch()
        {
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
            if (searchGridMonitorTipler.SelectedItem != null)
            {
                try
                {
                    MonitorTypes monTipi = (MonitorTypes)searchGridMonitorTipler.SelectedItem;
                    list.Add("@monitor_tipi", (int)monTipi);
                }
                catch (Exception) { }
            }
            if (searchGridMonitorMarkalar.SelectedItem != null)
            {
                if ((searchGridMonitorMarkalar.SelectedItem as Marka).MarkaID > 0)
                {
                    list.Add("@mon_marka_id", (searchGridMonitorMarkalar.SelectedItem as Marka).MarkaID);
                }
            }
            String alan_kisi_isim = searchGridalanKisiIsimTxtBox.Text.Trim().ToString();
            if (!String.IsNullOrEmpty(alan_kisi_isim))
            {
                list.Add("@alan_kisi_isim", alan_kisi_isim);
            }
            String pcName = searchGridPcNameTxtBox.Text.Trim().ToString();
            if (!String.IsNullOrEmpty(pcName))
            {
                list.Add("@pc_adi", pcName);
            }
            String pcModel = searchGridModelTxtBox.Text.Trim().ToString();
            if (!String.IsNullOrEmpty(pcModel))
            {
                list.Add("@model", pcModel);
            }
            return list;
        }

        private void RefreshComputerList(SortedList<String, object> parameterList, bool selectLast)
        {
            try
            {
                Stopwatch w = Stopwatch.StartNew();
                ComputerInfoRepository repositoryNew = new ComputerInfoRepository();
                SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();

                //String commandText = "Select TOP 1 * From tbl_bilgisayar pc order by bilgisayar_id Desc";
                String commandText = "pc_genel_arama";
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
                    //adp.Fill(0, 2, dt);
                    //dataGridSample.ItemsSource = dt.DefaultView;
                    foreach (DataRow rowPC in dt.Rows)
                    {
                        ComputerInfo tempComputer = new ComputerInfo();
                        try
                        {
                            tempComputer.SetGeneralFields(rowPC);
                            //tempComputer.Fetch();
                            //tempComputer.Set_ComputerOemDevices(cnn);
                            //tempComputer.Senet.Set_SenetInfos(true,tempComputer.Id,-1);
                        }
                        catch (Exception) { }
                        repositoryNew.Computers.Add(tempComputer);
                    }
                    pcList.DataContext = repositoryNew;
                    current_In_MemoryList = repositoryNew;
                    if (selectLast)
                    {
                        pcList.SelectedIndex = repositoryNew.Computers.Count - 1;
                    }
                    else
                    {
                        pcList.SelectedIndex = -1;
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
                Console.WriteLine("Bilgisayar listesi " + x + " milisaniye içinde yenilendi");
            }
            catch (Exception ex) {
                LoggerMebs.WriteToFile("\nRefreshComputerList Hatası : \n" + ex.Message);
            }
        }
        ComputerInfoRepository current_In_MemoryList = null;

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            searchGridAglarCombo.SelectedIndex = -1;
            searchGridalanKisiIsimTxtBox.Text = "";
            searchGridBirliklerCombo.SelectedIndex = -1;
            searchGridMarkalarCombo.SelectedIndex = -1;
            searchGridModelTxtBox.Text = "";
            searchGridMonitorTipler.SelectedIndex = -1;
            searchGridPcNameTxtBox.Text = "";
            searchGridTempestCombo.SelectedIndex = -1;
            searchGridMonitorMarkalar.SelectedIndex = -1;
            searchGridKomutanliklarCombo.SelectedIndex = -1;
        }

        private void printSenet_Click(object sender, RoutedEventArgs e)
        {
            if (pcList.SelectedItem != null)
            {
                SystemPrint printFunc = new SystemPrint(pcList.SelectedItem as ComputerInfo);
                printFunc.Print(true);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow w = new InfoWindow(this);
            w.Owner = this;
            w.Show();
        }

        private void searchGridKomutanliklarCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo_senet = sender as ComboBox;
            BirlikRepository birlik_rep = new BirlikRepository();
            birlik_rep.FillBirlikler((combo_senet.SelectedItem as Komutanlik));
            searchGridBirliklerCombo.ItemsSource = birlik_rep.Birlikler;
            BirlikRepository.INSTANCE = birlik_rep;
        }

        private void pcList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
             ComputerUserControlVertical x = new ComputerUserControlVertical();
             x.DataContext = (pcList.SelectedItem as ComputerInfo);
             x.Show();
                
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(MainWindow), new UIPropertyMetadata(false));

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            YaziciWindow w = new YaziciWindow();
            w.ShowDialog();
        }


        private ComputerInfoRepository getSearchRepository(String searchText) {

            if (!String.IsNullOrEmpty(searchText) && searchText.Length > 0)
            {
                ComputerInfoRepository repNew = new ComputerInfoRepository();
                foreach (ComputerInfo item in current_In_MemoryList.Computers)
                {
                    if (item.Pc_adi.ToLower().Contains(searchText.ToLower()) ||
                        item.Senet.Alan_kisi_isim.ToLower().Contains(searchText.ToLower()) ||
                        item.Senet.Alan_kisi_komutanlik.Komutanlik_ismi.ToLower().Contains(searchText.ToLower()) ||
                        item.Senet.Veren_kisi_isim.ToLower().Contains(searchText.ToLower())
                        )
                    {
                        repNew.Computers.Add(item);
                    }
                }
                return repNew;
            }
            else {
                return current_In_MemoryList;
            }
        }

        private void quickSearchBtn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!quickSearchBtn.Text.Equals(quickSearchBtn.DisplayText))
            {                
                if (quickSearchBtn.IsKeyboardFocused) {

                    String txt = quickSearchBtn.Text;
                    ComputerInfoRepository newList = getSearchRepository(txt);
                    pcList.DataContext= newList;

                }
            }
        }        
    }
}
