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
using MEBS_Envanter.Hardware;
using System.Threading;
using System.Windows.Threading;
using System.Collections;
using Mebs_Envanter.GUIHelpers;
using Mebs_Envanter;
using System.Diagnostics;
using System.ComponentModel;
using Mebs_Envanter.GeneralObjects;

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
            IsEnabled = false;
        }

        private void StartSqlInit()
        {
            SqlConnection conSql = DBFunctions.proviceConnection();
            Dispatcher.Invoke(DispatcherPriority.DataBind, new Action(delegate()
            {
                if (conSql != null)
                {
                    GlobalDataAccess.Set_Current_SQL_Connection(conSql);
                    Current_Computer_Info = new ComputerInfo();
                    setGUIDataContextForInitialization();
                    RefreshComputerList(null,true);
                    pcList_SelectionChanged(pcList, null);
                    IsEnabled = true;
                    pcList.Focus();
                }
                else
                {
                    MessageBox.Show("Bağlantı Sağlanamadı. Çıkıyorum");
                    Environment.Exit(0);
                }
            }));
        }
       
        private void SetContextForSearchFields() {

            // Birlikler arayüze atanıyor
            BirlikRepository Birlik_Repository = new BirlikRepository();
            Birlik_Repository.FillBirlikler();
            searchGridBirliklerCombo.ItemsSource = Birlik_Repository.Birlikler;

            BagliAgRepository rep_bagli_ag = new BagliAgRepository();
            rep_bagli_ag.Fill_Aglar();
            searchGridAglarCombo.ItemsSource = rep_bagli_ag.BagliAglar ;


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

        private bool AddOrEditPCFunction(bool isEdit)
        {
            try
            {
                ComputerInfo freshComputerInfo = new ComputerInfo();                
                pcEnvanterControl.Assign_ComputerInfo_By_GUI(Current_Computer_Info, freshComputerInfo, isEdit);
                freshComputerInfo.IsEdit = isEdit;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);                                
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                Mouse.OverrideCursor = Cursors.Wait;
                this.IsEnabled = false;
                
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
            this.IsEnabled = true;
            if ((bool)e.Result)
            {
                RefreshComputerList(null,true);
            }
            else {
                MessageBox.Show("Hata Oluştu");
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ComputerInfo freshComputerInfo = e.Argument as ComputerInfo;
            bool dbresult = DBFunctions.InsertOrUpdateComputerInfo(freshComputerInfo, freshComputerInfo.IsEdit);
            e.Result = dbresult;
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

        private void PCAddCallerFunction(bool isEdit) {

            if (AddOrEditPCFunction(isEdit))
            {

            }
            else {
                MessageBox.Show("Hata Oluştu");
            }
            
        }

        private void pcDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            DBFunctions.DeletePC(Current_Computer_Info);
            RefreshComputerList(null,true);
        }
        private void refreshListBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshComputerList(null,true);
            //changeCurrentPCContext((pcList.DataContext as ComputerInfoRepository).Computers[0]);
        }

        #endregion

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {

            RefreshComputerList(GetParameterListForSearch(),false);
        }

        private SortedList<String,object> GetParameterListForSearch(){

            SortedList<String, object> list = new SortedList<string, object>();
            if (searchGridBirliklerCombo.SelectedItem != null) {
                if ((searchGridBirliklerCombo.SelectedItem as Birlik).Birlik_id > 0) {

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
            if (searchGridMonitorTipler.SelectedItem != null) {

                try
                {
                    MonitorTypes monTipi = (MonitorTypes)searchGridMonitorTipler.SelectedItem;
                    list.Add("@monitor_tipi", (int)monTipi);
                }
                catch (Exception) {                 
                }

            }
            if (searchGridMonitorMarkalar.SelectedItem != null)
            {
                if ((searchGridMonitorMarkalar.SelectedItem as Marka).MarkaID > 0)
                {
                    list.Add("@mon_marka_id", (searchGridMonitorMarkalar.SelectedItem as Marka).MarkaID);
                }
            }                    
            list.Add("@alan_kisi_isim", searchGridalanKisiIsimTxtBox.Text.Trim());
            list.Add("@pc_adi", searchGridPcNameTxtBox.Text.Trim());
            list.Add("@model", searchGridModelTxtBox.Text.Trim());

            return list;
        }

        private void RefreshComputerList(SortedList<String,object> parameterList,bool selectLast) {
        
            Stopwatch w = Stopwatch.StartNew();

            ComputerInfoRepository repositoryNew = new ComputerInfoRepository();
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();

            //String commandText = "Select TOP 1 * From tbl_bilgisayar pc order by bilgisayar_id Desc";
            String commandText = "pc_genel_arama";
            SqlCommand cmd = new SqlCommand(commandText, cnn);
            cmd.CommandType= CommandType.StoredProcedure;

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
            }
            catch (Exception)
            {
            }
            finally
            {
                cnn.Close();
                cnn.Dispose();
            }
            foreach (DataRow rowPC in dt.Rows)
            {
                ComputerInfo tempComputer = new ComputerInfo();
                try
                {
                    tempComputer.SetGeneralFields(rowPC);
                    tempComputer.Set_HardwareInfos();
                    tempComputer.Set_SenetInfos();
                }
                catch (Exception){}
                repositoryNew.Computers.Add(tempComputer);
            }
            pcList.DataContext = repositoryNew;
            if (selectLast)
            {
                pcList.SelectedIndex = repositoryNew.Computers.Count - 1;
            }
            else {
                pcList.SelectedIndex = -1;
            }

            long x = w.ElapsedMilliseconds;
            Console.WriteLine("Bilgisayar listesi " + x + " milisaniye içinde yenilendi");
        }
    }
}
