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
using System.Threading;
using Mebs_Envanter.Export;
using MEBS_Envanter.Repositories;
using System.Data;
using System.Windows.Threading;

namespace Mebs_Envanter
{
    /// <summary>
    /// Interaction logic for ExportComputersWindow.xaml
    /// </summary>
    public partial class ExportComputersWindow : Window
    {
        Object exportContent;
        public ExportComputersWindow(Object exportContent)
        {
            InitializeComponent();
            this.exportContent = exportContent;
        }

        private ExportOptions GetOptions() {

            ExportOptions ops = new ExportOptions();
            ops.ExportGeneralInfo = generalInfoChkbx.IsChecked.Value;
            ops.ExportMonitorInfo = monitorChkbx.IsChecked.Value;
            ops.ExportNetworkInfo = networkInfoChkbx.IsChecked.Value;
            ops.ExportSenetInfo = senetInfoChkbx.IsChecked.Value;
            ops.ExportOemDevicesInfo = oemsChkbx.IsChecked.Value;
            
            return ops;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void exportBtn_Click(object sender, RoutedEventArgs e)
        {
            ExportOptions options = GetOptions();
            if (!options.ExportGeneralInfo && !options.ExportMonitorInfo &&
                !options.ExportNetworkInfo && !options.ExportOemDevicesInfo && !options.ExportSenetInfo) {

                    MessageBox.Show("En az bir öğe seçiniz !!!");
                    return;
            }

            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.FileName = "results.xls";
            sfd.Filter = "Excel File (.xls)|*.xls";
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            Thread th = new Thread(new ParameterizedThreadStart(delegate
            {

                ComputerInfoRepository computerInfoRep = exportContent as ComputerInfoRepository;
                if (computerInfoRep != null)
                {
                    ExportHelper exportHelper = new ExportHelper();
                    foreach (var item in computerInfoRep.Computers)
                    {
                        item.Fetch();
                    }

                    DataTable table = exportHelper.GetAsDataTable(computerInfoRep.Computers, options);
                    // export helper needs a dataset in case you want to save multiple worksheets
                    DataSet ds = new DataSet();
                    ds.Tables.Add(table);
                    if (!sfd.FileName.EndsWith("xls"))
                    {
                        sfd.FileName += ".xls";
                    }
                    ExcelXMLExportHelper.ToFormattedExcel(ds, sfd.FileName);
                    Dispatcher.Invoke(DispatcherPriority.DataBind, new Action(delegate
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                        IsEnabled = true;
                        MessageBox.Show("Dosya Başarılıyla aktarıldı.");
                        Close();
                    }));
                }

            }));
            th.IsBackground = true;
            th.Start();
            IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
            base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    this.DragMove();
                }
                catch { }
            }
            base.OnMouseDown(e);
        }
    }
}
