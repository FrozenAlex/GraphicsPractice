using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Brushes = SixLabors.ImageSharp.Drawing.Processing.Brushes;
using System.Numerics;
using SixLabors.Fonts;
using SystemFonts = SixLabors.Fonts.SystemFonts;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using GraphicsPractice.Common;

namespace GraphicsPractice.Labs._2
{
    /// <summary>
    /// Логика взаимодействия для Lab2Task3.xaml
    /// </summary>
    public partial class Lab2Task3 : Window
    {
        // Brushes 
        SolidBrush whiteBrush = Brushes.Solid(Rgba32.ParseHex("#FFFF"));
        SolidBrush pinkBrush = Brushes.Solid(Rgba32.ParseHex("#C71585"));

        Pen pinkPen = Pens.Solid(Rgba32.ParseHex("#C0C0FF"), 2);
        Pen greenPen = Pens.Solid(Rgba32.ParseHex("#228B22"), 1);
        Pen blackPen = Pens.Solid(Rgba32.ParseHex("#000000"), 1);
        Pen blackTransPen = Pens.Solid(Rgba32.ParseHex("#00005050"), 1);

        Image<Rgba32> image = null;

        DispatcherTimer dispatcherTimer;

        float scale = 10;
        float a = 5;
        PointF offset;
        int width, height;

        

        List<Line2D> lines = new List<Line2D>();
        public Lab2Task3()
        {
            InitializeComponent();

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(nextFrame);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 32);
            dispatcherTimer.Start();
        }

        public void nextFrame(object objectInfo, EventArgs e)
        {
            CalculatePoints();
            DrawScene(this, null);
        }

        // Sets up a scene, gets called only on refresh and other things to redo things
        private void SetupScene()
        {
            if (image != null)
            {
                // Spawn lines
                lines.Add(new Line2D(new PointF(-50, -50), new PointF(-100, -100)));
                lines.Add(new Line2D(new PointF(50, 50), new PointF(100, 100)));
                lines.Add(new Line2D(new PointF(-50, 50), new PointF(-100, 100)));
                lines.Add(new Line2D(new PointF(50, -50), new PointF(100, -100)));
            }
        }

        private void SetupCanvas()
        {
            if (ImageWrapper != null)
            {
                // Get size of the image
                width = (int)ImageWrapper.ActualWidth;
                height = (int)ImageWrapper.ActualHeight;

                image = new Image<Rgba32>(width, height);
            }
        }

        private void DrawScene(object sender, RoutedEventArgs e)
        {
            if (image != null)
            {
                // Draw triangle
                image.Mutate((ctx) =>
                {
                    ctx.Clear(whiteBrush);
                    var points = new List<PointF>();

                    ///
                    var displayLines = lines.ConvertAll<Line2D>((line) =>
                    {
                        // Transform origin
                        return new Line2D(
                            new PointF(line.start.X + width / 2, line.start.Y * -1 + height / 2),
                            new PointF(line.end.X + width / 2, line.end.Y * -1 + height / 2)
                        );
                    });
                    displayLines.ForEach((line) =>
                    {
                        ctx.DrawLines(blackPen, new PointF[] { line.start, line.end });
                    });

                    // X
                    ctx.DrawLines(greenPen, new PointF[] {
                        new PointF( 0, height/2),
                        new PointF(width, height / 2),
                    });

                    // Y
                    ctx.DrawLines(greenPen, new PointF[] {
                        new PointF( width/2, 0),
                        new PointF(width/2, height)
                    });

                });

                ImageControl.Source = Helpers.ImageToBitmap(image);
            }
        }

        public void CalculatePoints()
        {
            float angle = 0.1f;
            lines.ForEach((line) =>
            {
                var center = new PointF(line.end.X, line.end.Y);
                float cos = MathF.Cos(angle);
                float sin = MathF.Sin(angle);


                // Move the points to the "origin"
                var tx = line.start.X - center.X;
                var ty = line.start.Y - center.Y;
                line.start = new PointF(
                    (tx*cos-ty*sin) + center.X,
                    (ty*cos + tx*sin) + center.Y
                );

                // Move the points to the "origin"
                tx = line.end.X - center.X;
                ty = line.end.Y - center.Y;
                line.end = new PointF(
                    (tx * cos - ty * sin) + center.X,
                    (ty * cos + tx * sin) + center.Y
                );




                ///  line.start = new PointF(

                //   line.start.X * cos - line.start.Y * sin - center.X * (cos - 1) + (center.Y * sin),
                //    line.start.X * sin + line.start.Y * cos - (center.X * (sin) - center.Y * (cos - 1))
                //);
                //line.end = new PointF(
                //    line.end.X * cos - line.end.Y * sin - center.X * (cos - 1) + (center.Y * sin),
                //    line.end.X * sin + line.end.Y * cos - (center.X * (sin) - center.Y * (cos - 1))
                //);
            });
        }

        // From coord space to screen space
        public float XToScreen(float x)
        {
            return x * scale + offset.X;
        }
        public float YToScreen(float y)
        {
            return (y * scale * -1) + offset.Y;
        }

        // From screen space to coord space
        public float ScreenToX(float x)
        {
            return (x - offset.X) / scale;
        }
        public float ScreenToY(float y)
        {
            return (y - offset.Y) / scale * -1;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        private void Window_LayoutUpdated(object sender, EventArgs e)
        {
            SetupCanvas();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetupCanvas();
            SetupScene();
            DrawScene(sender, e);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
        }
    }
}
