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
                    RefreshComputerList();
                    pcList_SelectionChanged(pcList, null);
                    IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Bağlantı Sağlanamadı. Çıkıyorum");
                    Environment.Exit(0);
                }
            }));
        }

        private void RefreshComputerList()
        {
            Stopwatch w = Stopwatch.StartNew();

            ComputerInfoRepository repositoryNew = new ComputerInfoRepository();
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();

            //String commandText = "Select TOP 1 * From tbl_bilgisayar pc order by bilgisayar_id Desc";
            String commandText = "Select * From tbl_bilgisayar pc order by bilgisayar_id Asc";
            SqlCommand cmd = new SqlCommand(commandText, cnn);

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
                    tempComputer.Get_HardwareInfos();
                    tempComputer.Get_SenetInfos();
                }
                catch (Exception)
                {
                }
                repositoryNew.Computers.Add(tempComputer);

            }
            pcList.DataContext = repositoryNew;
            pcList.SelectedIndex = repositoryNew.Computers.Count - 1;

            long x = w.ElapsedMilliseconds;
        }

        private void Assign_ComputerInfo_By_GUI(ComputerInfo computerInfo, bool isEdit)
        {
            generalInfoUserControl1.SetGeneralInfo(computerInfo);
            networkUserControl1.SetNetworkInfo(computerInfo.NetworkInfo);

            monitorUserControl1.SetMonitorInfo(computerInfo.MonitorInfo);
            senetInfoUserControl1.SetSenetInfo(computerInfo.Senet);

            // OEM Parçaların Bilgileri            
            oemDeviceUserControl1.SetOemDevicesInfo(computerInfo);

            if (isEdit)
            {

                computerInfo.Id = Current_Computer_Info.Id;
                computerInfo.MonitorInfo.Id = Current_Computer_Info.MonitorInfo.Id;
                computerInfo.MonitorInfo.Mon_id = Current_Computer_Info.MonitorInfo.Mon_id;
                computerInfo.Senet.Id = Current_Computer_Info.Senet.Id;
                foreach (OEMDevice item in Current_Computer_Info.GetOemDevices())
                {
                    computerInfo.Get_OemDevice(item.DeviceType).Id = item.Id;
                }
            }

        }

        private bool AddOrEditPCFunction(bool isEdit)
        {
            try
            {
                ComputerInfo freshComputerInfo = new ComputerInfo();
                Assign_ComputerInfo_By_GUI(freshComputerInfo, isEdit);
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
                RefreshComputerList();
            }
            else {
                MessageBox.Show("Hata");
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ComputerInfo freshComputerInfo = e.Argument as ComputerInfo;
            bool dbresult = DBFunctions.InsertOrUpdateComputerInfo(freshComputerInfo, freshComputerInfo.IsEdit);
            e.Result = dbresult;
        }

        private void changeCurrentPCContext(ComputerInfo infComp)
        {
            Current_Computer_Info = infComp;
            pcEnvanterTabControl.DataContext = Current_Computer_Info;
            generalInfoTab.DataContext = Current_Computer_Info;
            networkTab.DataContext = Current_Computer_Info.NetworkInfo;
            monitorTab.DataContext = Current_Computer_Info.MonitorInfo;
            senetTab.DataContext = Current_Computer_Info.Senet;
            hardwareTab.DataContext = Current_Computer_Info;
            oemDeviceUserControl1.DataContext = Current_Computer_Info.OemDevicesVModel;
        }

        private void setGUIDataContextForInitialization()
        {
            monitorUserControl1.Init();
            generalInfoUserControl1.Init();
            senetInfoUserControl1.Init();
            networkUserControl1.Init();
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
            changeCurrentPCContext(infComp);
        }

        private void pcAddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditPCFunction(false);            
        }

        private void pcEditBtn_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditPCFunction(true);           
        }

        private void pcDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            DBFunctions.DeletePC(Current_Computer_Info);
            RefreshComputerList();
        }
        private void refreshListBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshComputerList();
            //changeCurrentPCContext((pcList.DataContext as ComputerInfoRepository).Computers[0]);
        }

        #endregion
    }
}
