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

namespace Mebs_Envanter.Animations
{
    /// <summary>
    /// Interaction logic for LoadingAnimation.xaml
    /// </summary>
    public partial class LoadingAnimation : UserControl
    {
        public LoadingAnimation()
        {
            InitializeComponent();
        }

        public String LoadingText
        {
            get { return (String)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadingText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingTextProperty =
            DependencyProperty.Register("LoadingText", typeof(String), typeof(LoadingAnimation), new UIPropertyMetadata("Loading..."));
    }
}
