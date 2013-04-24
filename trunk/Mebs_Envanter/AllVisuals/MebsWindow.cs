using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Threading;
using Mebs_Envanter.DB;
using System.Threading;

namespace Mebs_Envanter.AllVisuals
{
    public delegate void DBProviderInitializedHandler();
    public class MebsWindow : Window
    {

        public event DBProviderInitializedHandler OnDbInitialized;


        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

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
                        IsBusy = false;
                        GlobalDataAccess.Set_Current_SQL_Connection(conSql);
                        if (OnDbInitialized != null)
                        {
                            OnDbInitialized();
                        }
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


        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(MebsWindow), new UIPropertyMetadata(false));
    }
}
