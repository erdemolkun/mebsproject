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
        public static void CreateImageFile()
        {
            return;

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
