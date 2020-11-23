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

namespace GraphicsPractice.Labs._3
{
    /// <summary>
    /// Логика взаимодействия для Lab3Task1.xaml
    /// Параллелепипед Перенос одновременно по осям XY, XZ, YZ.
    /// </summary>
    public partial class Lab3Task1 : Window
    {
        // Generic stuff ie boilerplate
        Image<Rgba32> image = null;
        DispatcherTimer dispatcherTimer;
    
        public Lab3Task1()
        {
            InitializeComponent();
        }
       
        public void nextFrame(object objectInfo, EventArgs e)
        {
            FixedUpdate();
            DrawScene(this, null);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
        }

        /// <summary>
        /// Creates initial things like the image
        /// </summary>
        public void Start()
        {
            image = Helpers.SetupCanvas((int)ImageWrapper.ActualWidth, (int)ImageWrapper.ActualHeight);
            InitObjects();
            DrawScene(this, null);

            // Start timer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(nextFrame);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 64);  // 15 FPS
            dispatcherTimer.Start();
        }

        private void DrawScene(object sender, RoutedEventArgs e)
        {
            // Rendering code
            if (image != null)
            {
                image = RenderToImage(image, image.Width, image.Height);
                ImageControl.Source = Helpers.ImageToBitmap(image);
            }
        }

        /// <summary>
        /// Resizes the image and rerenders if the size is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            image = Helpers.SetupCanvas((int)ImageWrapper.ActualWidth, (int)ImageWrapper.ActualHeight);
            DrawScene(this, null);
        }

        //******************** Custom code *******************//

        // Variables for the scene
        List<Line3D> lines = new List<Line3D>();


        /// <summary>
        /// Transforms a point from virtual space to screen space
        /// </summary>
        /// <param name="point">Point to transform</param>
        /// <returns>Transformed point</returns>
        private PointF ToScreenSpace(Point3D point)
        {
            int width = image.Width;
            int height = image.Height;

            float angle = 1f;

            // Simple transform ignoring the 3d part
            return new PointF(point.X*100+ width/2, point.Y*100 + height/2);
        }

        /// <summary>
        /// Creates all objects seen on the scene
        /// </summary>
        public void InitObjects()
        {
            image = Helpers.SetupCanvas((int)ImageWrapper.ActualWidth, (int)ImageWrapper.ActualHeight);
            FixedUpdate();
            DrawScene(this, null);

            lines.AddRange(new List<Line3D> { 
                // Some figure
                // Top
                new Line3D(new Point3D(1, -1, 1), new Point3D(1, 1, 1)),
                new Line3D(new Point3D(1, 1, 1), new Point3D(4, 1, 1)),
                new Line3D(new Point3D(4, 1, 1), new Point3D(4, -1, 1)),
                new Line3D(new Point3D(4, -1, 1), new Point3D(1, -1, 1)),

                // Mid
                new Line3D(new Point3D(1, -1, 1), new Point3D(1, -1, -1)),
                new Line3D(new Point3D(1, 1, 1),  new Point3D(1, 1, -1)),
                new Line3D(new Point3D(4, 1, 1),  new Point3D(4, 1, -1)),
                new Line3D(new Point3D(4, -1, 1), new Point3D(4, -1, -1)),

                // Bottom
                new Line3D(new Point3D(1, -1, -1), new Point3D(1, 1, -1)),
                new Line3D(new Point3D(1, 1, -1), new Point3D(4, 1, -1)),
                new Line3D(new Point3D(4, 1, -1), new Point3D(4, -1, -1)),
                new Line3D(new Point3D(4, -1, -1), new Point3D(1, -1, -1)),
            });
        }

        /// <summary>
        /// Calculates new positions of points
        /// </summary>
        public void FixedUpdate()
        {
            float angle = 0.1f;
            lines.ForEach((line) =>
            {
                var center = new PointF(line.end.X, line.end.Y);
                float cos = MathF.Cos(angle);
                float sin = MathF.Sin(angle);
            });
        }

        /// <summary>
        /// Shape rendering code
        /// </summary>
        /// <param name="image">Image object</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <returns></returns>
        public Image<Rgba32> RenderToImage(Image<Rgba32> image, int width, int height)
        {

            image.Mutate((ctx)=>
            {
                // Put your image render code here
                lines.ForEach((line) =>
                {
                    ctx.DrawLines(DrawingKit.blackPen, new PointF[] { ToScreenSpace(line.start), ToScreenSpace(line.end) });
                });

            });

            return image;
        }
    }
}
