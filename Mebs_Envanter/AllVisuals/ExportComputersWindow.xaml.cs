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
using Mebs_Envanter.Repositories;
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
        int export_Format = ExportOptions.EXCEL;
        public ExportComputersWindow(Object exportContent, int export_Format)
        {
            InitializeComponent();
            this.exportContent = exportContent;
            this.export_Format = export_Format;
        }

        private ExportOptions GetOptions()
        {

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
                !options.ExportNetworkInfo && !options.ExportOemDevicesInfo && !options.ExportSenetInfo)
            {

                MessageBox.Show("En az bir öğe seçiniz !!!");
                return;
            }

            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            if (export_Format == ExportOptions.EXCEL)
            {
                sfd.FileName = "results.xls";
                sfd.Filter = "Excel File (.xls)|*.xls";
            }
            else
            {

                sfd.FileName = "results.html";
                sfd.Filter = "Excel File (.html)|*.html";
            }
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            Thread th = new Thread(new ParameterizedThreadStart(delegate
            {
                ComputerInfoRepository computerInfoRep = exportContent as ComputerInfoRepository;
                if (computerInfoRep != null)
                {
                    bool isSuccess = true;
                    try
                    {
                        ExportHelper exportHelper = new ExportHelper();
                        foreach (var item in computerInfoRep.Collection)
                        {
                            item.Fetch();
                        }

                        DataTable table = exportHelper.GetAsDataTable(computerInfoRep.Collection, options);
                        // export helper needs a dataset in case you want to save multiple worksheets
                        DataSet ds = new DataSet();
                        ds.Tables.Add(table);
                        FileExportHelper h = null;
                        if (export_Format == ExportOptions.EXCEL)
                        {
                            if (!sfd.FileName.EndsWith("xls"))
                            {
                                sfd.FileName += ".xls";
                            }
                            h = new ExcelXMLExportHelper();
                        }
                        else
                        {
                            if (!sfd.FileName.EndsWith("html"))
                            {
                                sfd.FileName += ".html";
                            }
                            h = new HTMLHelper();
                        }
                        if (h != null)
                        {
                            h.Export(ds, sfd.FileName);
                        }
                    }
                    catch (Exception)
                    {
                        isSuccess = false;

                    }
                    Dispatcher.Invoke(DispatcherPriority.DataBind, new Action(delegate
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                        IsEnabled = true;
                        if (isSuccess)
                        {
                            MessageBox.Show("Dosya Başarılıyla aktarıldı.");
                        }
                        else
                        {
                            MessageBox.Show("Hata Oluştu");
                        }
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
