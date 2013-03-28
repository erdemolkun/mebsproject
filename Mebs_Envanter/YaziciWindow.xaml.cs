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
            LoadItems();

            YaziciInfo x = new YaziciInfo();
            x.YaziciModeli = "asdz1";
            x.NetworkInfo.IpAddress = "asdz2";
            x.SenetInfo.Alan_kisi_isim = "asdz3";


            Current_YaziciInfo = x;
            gridYaziciBilgileri.DataContext = Current_YaziciInfo;

            RefreshPrinterList(null, true);

        }

        private void LoadItems() {
            yaziciUserControl1.Init();
            senetInfoControl1.Init();
            networkInfoControl1.Init();            
        }
        private bool AddOrEditPCFunction(bool isEdit)
        {

            try
            {
                YaziciInfo infYazici = new YaziciInfo();

                YaziciInfo currentYazidi = yaziciList.SelectedItem as YaziciInfo;
                if (currentYazidi != null)
                {
                    AssignYaziciInfoByGui(currentYazidi, infYazici, isEdit);

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
            return true;
        
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            IsBusy = false;
            YaziciDbWorkInfo addInfo = e.Result as YaziciDbWorkInfo;
            YaziciInfoRepository yaziciRep = (yaziciList.DataContext as YaziciInfoRepository);

            if (addInfo.yazici.isEdit)
            {
                int index = yaziciRep.Yazicilar.IndexOf(Current_YaziciInfo);
                yaziciRep.Yazicilar[index] = addInfo.yazici;

                Current_YaziciInfo = addInfo.yazici;
                yaziciList.SelectedItem = Current_YaziciInfo;
                gridYaziciBilgileri.DataContext=Current_YaziciInfo;
                
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
            bool dbresult = DBFunctions.InsertOrUpdateOemDevice(yaziciInfo,-1,yaziciInfo.isEdit);
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

        private void AssignYaziciInfoByGui(YaziciInfo current,YaziciInfo toAssign,bool isEdit) {

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

        private void RefreshPrinterList(SortedList<String, object> parameterList, bool selectLast)
        {

            Stopwatch w = Stopwatch.StartNew();
            YaziciInfoRepository repositoryNew = new YaziciInfoRepository();
            SqlConnection cnn = GlobalDataAccess.Get_Fresh_SQL_Connection();

            String commandText = "Select TOP 10 * From tbl_yazici pc order by yazici_id Desc";
            //String commandText = "pc_genel_arama";
            SqlCommand cmd = new SqlCommand(commandText, cnn);
            //cmd.CommandType = CommandType.StoredProcedure;

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
                //dataGridSample.ItemsSource = dt.DefaultView;
                foreach (DataRow rowPC in dt.Rows)
                {
                    int parca_id = DBValueHelpers.GetInt32(rowPC["parca_id"].ToString(), -1);
                    List<OEMDevice> xx= OEMDevice.GetOemDevices(cnn, false, -1, parca_id);
                    YaziciInfo tempYazici = null;
                    foreach (var item in xx)
                    {
                        if (item is YaziciInfo) {
                            tempYazici = item as YaziciInfo;
                        }
                    }
                    //YaziciInfo tempYazici = new YaziciInfo();
                    try
                    {
                        tempYazici.SetGeneralFields(rowPC);
                        //tempComputer.Set_HardwareInfos(cnn);
                        //tempComputer.Set_SenetInfos();
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
            //pcEnvanterTabControl.DataContext = Current_Computer_Info;
            //pcEnvanterControl.DataContext = Current_Computer_Info;
            Current_YaziciInfo = infYazici;
            gridYaziciBilgileri.DataContext = Current_YaziciInfo;
            //changeCurrentPCContext(list.SelectedItem as ComputerInfo );

        }

        private YaziciInfo Current_YaziciInfo = new YaziciInfo();

        private void yaziciAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditPCFunction(false);
        }

        private void yaziciEdit_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditPCFunction(true);
        }

        private void yaziciDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refreshListBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(YaziciWindow), new UIPropertyMetadata(false));
    }
}
