using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Media.Effects;

namespace Mebs_Envanter
{

    public class TemplatedWindow : Window
    {        
        public override void OnApplyTemplate()
        {
            this.Background = Brushes.Transparent;

            Button closeButton = (Button)this.Template.FindName("PART_CLOSE_BTN", this);
            Button maxResButton = (Button)this.Template.FindName("PART_MAXRESTORE", this);
            FrameworkElement gridPanel = this.Template.FindName("PART_GRIDHEADER", this) as FrameworkElement;
            if (closeButton != null)
            {
                closeButton.Click += new RoutedEventHandler(closeButton_Click);
            }
            if (maxResButton != null)
            {
                maxResButton.Click += new RoutedEventHandler(maxResButton_Click);
            }
            if (gridPanel != null)
            {
                gridPanel.MouseDown += new MouseButtonEventHandler(gridPanel_MouseDown);

            }
            
            base.OnApplyTemplate();
        }

        void gridPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount == 2)
            {
                if (CanResize)
                {
                    //maxResButton_Click(null, null);
                }
            }
        }
       
        
        void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected void ChangeState() {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                Maximized = false;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                Maximized = true;
            }
        }
        void maxResButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeState();
        }


        public bool Maximized
        {
            get { return (bool)GetValue(MaximizedProperty); }
            set { SetValue(MaximizedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximized.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximizedProperty =
            DependencyProperty.Register("Maximized", typeof(bool), typeof(TemplatedWindow), new UIPropertyMetadata(false));


        public bool CanResize
        {
            get { return (bool)GetValue(CanResizeProperty); }
            set { SetValue(CanResizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanResize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanResizeProperty =
            DependencyProperty.Register("CanResize", typeof(bool), typeof(TemplatedWindow), new UIPropertyMetadata(true));


    }
}
