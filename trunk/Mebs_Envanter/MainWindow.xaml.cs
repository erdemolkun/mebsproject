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
using Mebs_Envanter.DB;
using System.Data.SqlClient;
using Mebs_Envanter.GeneralObjects;
using System.Data;
using System.Threading;
using System.Windows.Threading;
using System.Collections;
using Mebs_Envanter.GUIHelpers;
using Mebs_Envanter;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Printing;
using Mebs_Envanter.PrintOperations;
using Mebs_Envanter.Repositories;
using System.Reflection;
using ReadWriteCsv;
using Mebs_Envanter.Export;


namespace Mebs_Envanter
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

            this.Title = "Bilgisayar Envanter Kaydı    " + VersionInfo.versiyonStr;
        }

        private ComputerInfo GetNewComputer()
        {
            return new ComputerInfo(x2 => pcDeleteBtn_Click(null, null));
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
                        Current_Computer_Info = GetNewComputer();
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
                catch (Exception ex)
                {
                    MessageBox.Show("Hata Oluştu. Çıkıyorum");
                    IsBusy = false;
                    LoggerMebs.WriteToFile(ex.Message.ToString());
                }
            }));
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

            MarkaRepository marka_rep2 = new MarkaRepository();
            marka_rep2.FillMarkalar(true);
            searchGridMonitorMarkalar.ItemsSource = marka_rep2.Markalar;

            MonitorSizesRepository mon_size_rep = new MonitorSizesRepository();
            mon_size_rep.FillSizes(true);
            searchGridMonitorBoyutlar.ItemsSource = mon_size_rep.Sizes;
        }

        private void ShowError(String msg)
        {
            InfoWindow.ShowMessage(this, msg);
        }

        private bool AddOrEditPCFunction(bool isEdit)
        {
            try
            {
                ComputerInfo freshComputerInfo = GetNewComputer();
                pcEnvanterControl.Assign_ComputerInfo_By_GUI(Current_Computer_Info, freshComputerInfo, isEdit);

                object count = DBFunctions.ExecuteToFetchSingleItem("Select Count(*) as Count from tbl_bilgisayar where Pc_adi like '" +
                    freshComputerInfo.Pc_adi.Trim().ToString() + "'", "Count");

                if (String.IsNullOrEmpty(freshComputerInfo.Pc_adi.Trim().ToString()))
                {
                    ShowError("Lütfen Bilgisayar ismini giriniz !!!");
                    return true;
                }

                if (count != null && Convert.ToInt32(count) > 0)
                {
                    if (isEdit && Current_Computer_Info.Pc_adi.Equals(freshComputerInfo.Pc_adi))
                    {
                    }
                    else
                    {
                        ShowError("Aynı isimli bilgisayar mevcut !!!");
                        return true;
                    }
                }

                if (
                    !string.IsNullOrEmpty(Current_Computer_Info.Pc_adi) &&
                    !Current_Computer_Info.Pc_adi.Equals(freshComputerInfo.Pc_adi)
                    && isEdit)
                {
                    MessageBoxResult x1 = MessageBox.Show("Bilgisayar İsmini Düzenlemek istiyor musunuz ? ",
                        "Dikkat !!!", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (!x1.Equals(MessageBoxResult.Yes))
                    {
                        return true;
                    }
                }

                freshComputerInfo.IsEdit = isEdit;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                Mouse.OverrideCursor = Cursors.Wait;
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
            if (addInfo.computer.IsEdit || Current_Computer_Info.Id < 0)
            {
                int index = computerRep.Computers.IndexOf(Current_Computer_Info);
                if (index < 0)
                {
                    computerRep.Computers.Insert(0, addInfo.computer);
                }
                else
                {
                    computerRep.Computers[index] = addInfo.computer;
                }
                Current_Computer_Info = addInfo.computer;
                pcList.SelectedItem = Current_Computer_Info;
                pcEnvanterControl.SetDataContext(Current_Computer_Info);
            }
            else
            {
                computerRep.Computers.Insert(0, addInfo.computer);
                //computerRep.Computers.Add(addInfo.computer);
                pcList.SelectedItem = addInfo.computer;
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ComputerInfo freshComputerInfo = e.Argument as ComputerInfo;
            bool dbresult = DBFunctions.InsertOrUpdateComputerInfo(freshComputerInfo, freshComputerInfo.IsEdit);
            ComputerDbWorkInfo addInfo = new ComputerDbWorkInfo();
            addInfo.computer = freshComputerInfo;
            addInfo.isSuccess = dbresult;
            e.Result = addInfo;
        }

        internal class ComputerDbWorkInfo
        {
            public bool isSuccess = false;
            public ComputerInfo computer = null;
        }

        private void setGUIDataContextForInitialization()
        {
            pcEnvanterControl.Init();
            SetContextForSearchFields();
        }

        #region Events

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pcEnvanterControl.KeyEventResponder(e);
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (Keyboard.IsKeyDown(Key.P))
                {
                    printSenetPreview_Click(null, null);
                }
                else if (Keyboard.IsKeyDown(Key.H))
                {

                    hakkindaMenuItem_Click(null, null);
                }
            }
            base.OnKeyDown(e);
        }

        private bool handleSelection = true;
        private void pcList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region Control Selection Change
            if (handleSelection && e != null)
            {
                ComputerInfo removed = null;
                ListBox lstBox = (ListBox)sender;
                foreach (var item in e.RemovedItems)
                {
                    removed = item as ComputerInfo;
                }
                if (removed != null && (lstBox.DataContext as ComputerInfoRepository).Computers.Contains(removed))
                {
                    if (removed.Id < 0)
                    {

                        MessageBoxResult result = InfoWindow.AskQuestion("Bilgisayarı kaydetmeden geçiş yapmak istiyor musunuz ?", "Uyarı !!");
                        if (result != MessageBoxResult.Yes)
                        {

                            handleSelection = false;
                            lstBox.SelectedItem = e.RemovedItems[0];
                            return;
                        }
                        else
                        {
                            (lstBox.DataContext as ComputerInfoRepository).Computers.Remove(removed);
                        }
                    }
                }
            }
            handleSelection = true;
            #endregion

            ListBox list = sender as ListBox;
            ComputerInfo infComp = null;
            if (list.SelectedItem != null)
            {
                infComp = list.SelectedItem as ComputerInfo;
            }
            else
            {
                infComp = GetNewComputer();
            }
            list.ScrollIntoView(infComp);
            Current_Computer_Info = infComp;

            foreach (var item in list.SelectedItems)
            {
                ComputerInfo computerItem = item as ComputerInfo;
                if (computerItem != null)
                {
                    computerItem.Fetch();
                }
            }
            pcEnvanterControl.SetDataContext(Current_Computer_Info);
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
            if (!AddOrEditPCFunction(isEdit))
            {
                InfoWindow.ShowMessage(this, "Ekleme Sırasında Hata Oluştu");
            }
        }

        private void pcDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (InfoWindow.AskQuestion("Silmek istediğinize emin misiniz?", "Dikkat !!!") != MessageBoxResult.Yes) { return; }

            bool isSuccess = DBFunctions.DeletePC(Current_Computer_Info);
            if (isSuccess)
            {
                ComputerInfoRepository currentInfoRep = (pcList.DataContext as ComputerInfoRepository);
                if (currentInfoRep != null)
                {
                    currentInfoRep.Computers.Remove(Current_Computer_Info);
                    SetSelectedItemAfterContextChange(true);
                }
            }
            else
            {
                InfoWindow.ShowMessage(this, "Silme Sırasında Hata Oluştu");
            }
        }


        #endregion

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshComputerList(GetParameterListForSearch(), true);
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

            if (searchGridMonitorBoyutlar.SelectedItem != null)
            {
                try
                {
                    MonitorSize monSize = searchGridMonitorBoyutlar.SelectedItem as MonitorSize;
                    if (monSize.Id > 0)
                    {
                        list.Add("@boyut_id", monSize.Id);
                    }
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

        private void SetSelectedItemAfterContextChange(bool selectLast)
        {
            ComputerInfoRepository repositoryNew = (pcList.DataContext as ComputerInfoRepository);
            if (selectLast && repositoryNew.Computers.Count > 0)
            {
                pcList.SelectedIndex = 0;//repositoryNew.Computers.Count - 1;
            }
            else
            {
                pcList.SelectedIndex = -1;
            }
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
                        ComputerInfo tempComputer = GetNewComputer();
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
                    quickSearchBtn.ClearText();
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
                Console.WriteLine("Bilgisayar listesi " + x + " milisaniye içinde yenilendi");
            }
            catch (Exception ex)
            {
                LoggerMebs.WriteToFile("\nRefreshComputerList Hatası : \n" + ex.Message);
            }
        }

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
                printFunc.Print(false);
            }
        }

        private void searchGridKomutanliklarCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo_senet = sender as ComboBox;
            BirlikRepository birlik_rep = new BirlikRepository();
            birlik_rep.FillBirlikler((combo_senet.SelectedItem as Komutanlik), true);
            searchGridBirliklerCombo.ItemsSource = birlik_rep.Birlikler;
            BirlikRepository.INSTANCE = birlik_rep;
        }

        private void pcList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /*ComputerUserControlVertical x = new ComputerUserControlVertical();
            x.DataContext = (pcList.SelectedItem as ComputerInfo);
            x.Show();*/
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            YaziciWindow w = new YaziciWindow();
            w.ShowDialog();
        }

        private void quickSearchBtn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!quickSearchBtn.Text.Equals(quickSearchBtn.DisplayText))
            {
                if (quickSearchBtn.IsKeyboardFocused)
                {
                    String txt = quickSearchBtn.Text;
                    ComputerInfoRepository newList = current_In_MemoryList.getSearchRepository(txt);
                    pcList.DataContext = newList;
                    SetSelectedItemAfterContextChange(true);
                }
            }
        }

        private void hakkindaMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow.ShowAbout(this);
        }

        private void printSenetPreview_Click(object sender, RoutedEventArgs e)
        {
            if (pcList.SelectedItem != null)
            {
                SystemPrint printFunc = new SystemPrint(pcList.SelectedItem as ComputerInfo);
                printFunc.Print(true);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SampleWindow x = new SampleWindow();
            x.ShowDialog();
        }

        private void CLOSE_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            pcDeleteBtn_Click(null, null);
        }

        #region Variables

        ComputerInfoRepository current_In_MemoryList = null;

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(MainWindow), new UIPropertyMetadata(false));


        public ComputerInfo Current_Computer_Info
        {
            get { return (ComputerInfo)GetValue(Current_Computer_InfoProperty); }
            set { SetValue(Current_Computer_InfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Current_Computer_Info.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Current_Computer_InfoProperty =
            DependencyProperty.Register("Current_Computer_Info", typeof(ComputerInfo), typeof(MainWindow), new UIPropertyMetadata(null));

        #endregion

        private void Export(ComputerInfoRepository rep, int export_Format)
        {

            if (rep != null && rep.Computers.Count > 0)
            {
                ExportComputersWindow exportWindow = new ExportComputersWindow(rep, export_Format);
                exportWindow.Owner = this;
                exportWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Aktarılacak Öğe Yok");
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Export(current_In_MemoryList, ExportOptions.EXCEL);
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            ComputerInfoRepository rep = new ComputerInfoRepository();
            rep.Computers.Add(pcList.SelectedItem as ComputerInfo);
            Export(rep, ExportOptions.EXCEL);
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {


            Export(current_In_MemoryList, ExportOptions.HTML);
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            ComputerInfoRepository rep = new ComputerInfoRepository();
            rep.Computers.Add(pcList.SelectedItem as ComputerInfo);
            Export(rep, ExportOptions.HTML);
        }

        private void addNewPcBtn_Click(object sender, RoutedEventArgs e)
        {
            ComputerInfoRepository currentInfoRep = (pcList.DataContext as ComputerInfoRepository);
            if (currentInfoRep != null)
            {
                currentInfoRep.Computers.Insert(0, GetNewComputer());
                pcEnvanterControl.SetFocus(0);
                SetSelectedItemAfterContextChange(true);
            }
            //RefreshComputerList(null, true);
        }
    }
}
