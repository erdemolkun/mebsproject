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
using Mebs_Envanter;
using Mebs_Envanter.GeneralObjects;
using Mebs_Envanter.Repositories;
using System.Text.RegularExpressions;
using Mebs_Envanter.DB;

namespace Mebs_Envanter.HardwareUserControls
{
    /// <summary>
    /// Interaction logic for MonitorInfoUserControl.xaml
    /// </summary>
    public partial class MonitorInfoUserControl : UserControl
    {
        public MonitorInfoUserControl()
        {
            InitializeComponent();
        }
        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //IsEditable true olursa çalışır.
            TextBox TxtBox = monitorBoyutlarCombo.Template.FindName("PART_EditableTextBox", monitorBoyutlarCombo) as TextBox;
            if (TxtBox != null)
            {
                
                TxtBox.PreviewTextInput += new TextCompositionEventHandler(txt_PreviewTextInput);

            }
        }
        double MAX_SIZE = 100;
        void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            String previewText = e.Text;
            TextBox txtBox = sender as TextBox;
            bool doesOverflow = false;
            try
            {
                String textBoxText = txtBox.Text;
                textBoxText = textBoxText.Remove(txtBox.SelectionStart, txtBox.SelectionLength);
                String newText = textBoxText.Insert(txtBox.SelectionStart, previewText);
                double size = Convert.ToDouble(newText);
                if (size > MAX_SIZE)
                {
                    doesOverflow = true;
                }
            }
            catch (Exception) { 
            
            }
            //Regex pattern = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            Regex pattern = new Regex(@"^[0-9]*(?:\,[0-9]*)?$");
            e.Handled = (pattern.IsMatch(previewText) && !doesOverflow) == false;

        }
        public void SetMonitorInfo(Monitor inf)
        {
            // Monitor Bilgileri
            inf.Marka = monitorMarkalarCombo.SelectedItem as Marka;
            inf.Tempest = monitorTempestCombo.SelectedItem as Tempest;
            inf.SerialNumber = monitorSerialTextBox.Text.Trim().ToString();
            inf.StokNo = monitorStokNoTextBox.Text.Trim().ToString();
            inf.Parca_no = monitorParcaNoTextBox.Text.Trim().ToString();
            if (monitorTiplerCombo.SelectedItem != null)
            {
                try
                {
                    inf.MonType = (MonitorTypes)monitorTiplerCombo.SelectedItem;
                }
                catch (Exception)
                {

                }
            }
            if (monitorBoyutlarCombo.SelectedItem != null)
            {
                inf.MonSize = monitorBoyutlarCombo.SelectedItem as MonitorSize;
            }
            else if (monitorBoyutlarCombo.IsEditable &&  !String.IsNullOrEmpty(monitorBoyutlarCombo.Text.Trim()))
            {

                try
                {
                    double size = Convert.ToDouble(monitorBoyutlarCombo.Text);
                    if (size > 0)
                    {
                        int newId = DBFunctions.InsertMonitorSize(size);
                        MonitorSize sizeNew = new MonitorSize(newId, (float)size);
                        //(monitorBoyutlarCombo.ItemsSource as MonitorSizesRepository).Sizes.Add(sizeNew);
                        MonitorSizesRepository.INSTANCE.Collection.Add(sizeNew);
                        MonitorSizesRepository.INSTANCE.Collection.Sort(p => p.MonitorLength);
                        inf.MonSize = sizeNew;
                        
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        public void Init()
        {

            // Markalar arayüze atanıyor
            MarkaRepository Marka_Repository = new MarkaRepository();
            Marka_Repository.Fetch_Markalar(false);
            monitorMarkalarCombo.ItemsSource = Marka_Repository.Collection;
            MarkaRepository.INSTANCE = Marka_Repository;


            TempestRepository Rep_Tempest = new TempestRepository();
            Rep_Tempest.Fetch_Seviyeler(false);
            monitorTempestCombo.ItemsSource = Rep_Tempest.Collection;
            TempestRepository.INSTANCE = Rep_Tempest;


            MonitorSizesRepository Size_Rep = new MonitorSizesRepository();
            Size_Rep.Fetch_Sizes(false);
            monitorBoyutlarCombo.ItemsSource = Size_Rep.Collection;
            MonitorSizesRepository.INSTANCE = Size_Rep;
        }


    }
}
