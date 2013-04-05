﻿using System;
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

namespace Mebs_Envanter
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow(Window owner)
        {
            InitializeComponent();
            this.Owner = owner;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {

            if (e.Key == Key.Escape || e.Key == Key.Enter)
            {

                Close();
            }
            base.OnKeyDown(e);
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowMessage(String msg) {

            infoTxtBlock.Text = msg;
            ShowDialog();
        }

        public static void ShowMessage(Window owner,String msg) {
            InfoWindow w = new InfoWindow(owner);
            w.ShowMessage(msg);            
        }

        public static MessageBoxResult AskQuestion(String msg, String header)
        {

            MessageBoxResult result1 = MessageBox.Show(msg, header,
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            return result1;
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

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}