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
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 32);  // 15 FPS
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

          

            Point3D temp = new Point3D(point.X, point.Y, point.Z);
            temp.Y = -temp.Y; // Fix Y
            temp.rotate(0.5f,2f,0.2f);


            // Simple transform ignoring the 3d part
            return new PointF(temp.X*50+ (float)(width/2), temp.Y*50 + (float)(height/2));
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
            lines.ForEach((line) =>
            {
                Point3D temp = line.start;
                line.start = temp.rotate(0, 0.1f, 0.1f);
                
                temp = line.end;
                line.end = temp.rotate(0, 0.1f, 0.1f);
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
                ctx.Clear(new Color(Rgba32.ParseHex("#FFFFFF")));
                // Put your image render code here
                lines.ForEach((line) =>
                {
                    ctx.DrawLines(DrawingKit.blackPen, new PointF[] { ToScreenSpace(line.start), ToScreenSpace(line.end) });
                });


                /// Z
                ctx.DrawLines(DrawingKit.bluePen, new PointF[] {
                   ToScreenSpace(new Point3D(0,0,-1)),  ToScreenSpace(new Point3D(0,0,1))
                });
                /// X
                ctx.DrawLines(DrawingKit.pinkPen, new PointF[] {
                   ToScreenSpace(new Point3D(1,0,0)),  ToScreenSpace(new Point3D(-1,0,0))
                });
                /// Y
                ctx.DrawLines(DrawingKit.greenPen, new PointF[] {
                   ToScreenSpace(new Point3D(0,1,0)),  ToScreenSpace(new Point3D(0,-1,0))
                });
            });

            return image;
        }
    }
}
