using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace Mebs_Envanter
{
    class Unnecessary
    {

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    try
        //    {
        //        pcList.ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);
        //        pcList.ItemContainerGenerator.ItemsChanged += new System.Windows.Controls.Primitives.ItemsChangedEventHandler(ItemContainerGenerator_ItemsChanged);
        //        DataTemplate template1 = base.FindResource("computerInfoListboxDataTemplate") as DataTemplate;
        //        //Button btnDeleteItem = pcList.ItemTemplate.FindName("CLOSE_BUTTON", pcList) as Button;
        //    }
        //    catch (Exception) { 


        //    }

        //}

        //void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        //{

        //}

        //void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        //{

        //    //MessageBox.Show("Index : "+e.Position.Index.ToString()+"Offset : "+e.Position.Offset.ToString());
        //    //// Iterate through Books


        //    foreach (var item in pcList.Items){

        //        // Get the ListBoxItem around the Book
        //        ListBoxItem listBoxItem =
        //            this.pcList.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;


        //        DependencyObject dp = pcList.ItemContainerGenerator.ContainerFromItem(item) as DependencyObject;        

        //        // Get the ContentPresenter
        //        ContentPresenter presenter = listBoxItem.FindVisualChild<ContentPresenter>();

        //        // Get the Template instance
        //        DataTemplate template = presenter.ContentTemplate;

        //        // Find the CheckBox within the Template
        //        CheckBox checkBox = template.FindName("CLOSE_BUTTON", presenter) as CheckBox;
        //        checkBox.IsEnabled = !checkBox.IsEnabled;
        //    }
        //}

        //public override void OnApplyTemplate(){

        //    base.OnApplyTemplate();
        //    DataTemplate template1 = base.FindResource("computerInfoListboxDataTemplate") as DataTemplate;


        //    //ContentPresenter myContentPresenter = WPFVisualHelpers.FindVisualChild<ContentPresenter>(this);


        //    try
        //    {
        //        ItemsPresenter itemsPresenter = GetVisualChild<ItemsPresenter>(pcList);
        //        StackPanel itemsPanelStackPanel = GetVisualChild<StackPanel>(itemsPresenter);

        //        var container = pcList.ItemContainerGenerator.ContainerFromIndex(0) as FrameworkElement;
        //        Button btnDeleteItem = pcList.ItemTemplate.FindName("CLOSE_BUTTON", pcList) as Button;

        //    }
        //    catch (Exception)
        //    {

        //    }
        //}


        public static void CreateImageFile()
        {
            

            Grid workGrid;
            TextBlock workTextBlock;
            RenderTargetBitmap bitmap;
            PngBitmapEncoder encoder;
            Rect textBlockBounds;
            GeneralTransform transform;

            workGrid = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            workTextBlock = new TextBlock()
            {
                //Text = "Lorem ipsum dolor sit amet, consectetur adipisicing" + Environment.NewLine + "elit, sed do eiusmod tempor",
                Text="38590366718",
                //FontFamily = new FontFamily("Verdana"),
                FontFamily = new FontFamily("IDAutomationHC39M"),
                
                FontSize = 36,
                TextAlignment = TextAlignment.Center,
                RenderTransformOrigin = new Point(0.5, 0.5),
                //LayoutTransform = new RotateTransform(-45)
            };

            workGrid.Children.Add(workTextBlock);

            /*
             * We now must measure and arrange the controls we just created to fill in the details (like
             * ActualWidth and ActualHeight before we call TransformToVisual() and TransformBounds()
             */
            workGrid.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            workGrid.Arrange(new Rect(0, 0, workGrid.DesiredSize.Width, workGrid.DesiredSize.Height));

            transform = workTextBlock.TransformToVisual(workGrid);
            textBlockBounds = transform.TransformBounds(new Rect(0, 0, workTextBlock.ActualWidth, workTextBlock.ActualHeight));

            /*
             * Now, create the bitmap that will be used to save the image. We will make the image the 
             * height and width we need at 96DPI and 32-bit RGBA (so the background will be transparent).
             */
            bitmap = new RenderTargetBitmap((int)textBlockBounds.Width, (int)textBlockBounds.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(workGrid);

            encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using (FileStream file = new FileStream("TextImage.png", FileMode.Create))
                encoder.Save(file);
        }
    }
}
