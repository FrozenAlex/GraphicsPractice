using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Numerics;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using GraphicsPractice.Common;
using System.Threading.Tasks;

namespace GraphicsPractice.Labs._3
{
    /// <summary>
    /// Логика взаимодействия для Lab3Task1.xaml
    /// Параллелепипед Перенос одновременно по осям XY, XZ, YZ.
    /// </summary>
    public partial class Lab3Task1 : Window
    {
        Boolean pageIsActive = true;
        DateTime time = DateTime.Now;


        public Lab3Task1()
        {
            InitializeComponent();

            AnimationLoop();
        }

        /// <summary>
        /// Render loop
        /// </summary>
        /// <returns></returns>
        async Task AnimationLoop()
        {
            while (pageIsActive)
            {
                DateTime time2 = DateTime.Now;
                float deltaTime = (time2.Ticks - time.Ticks) / 10000000f;
                time = time2;

                FixedUpdate(deltaTime);
                canvasView.InvalidateVisual();
                await Task.Delay(TimeSpan.FromSeconds(1.0 / 60));
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// Creates initial things like the image
        /// </summary>
        public void Start()
        {
            InitObjects();
        }

        /// <summary>
        /// Resizes the image and rerenders if the size is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvasView.InvalidateMeasure();
        }

        //******************** Custom code *******************//

        // Variables for the scene
        List<Line3D> lines = new List<Line3D>();


        /// <summary>
        /// Transforms a point from virtual space to screen space
        /// </summary>
        /// <param name="point">Point to transform</param>
        /// <returns>Transformed point</returns>
        private SkiaSharp.SKPoint ToScreenSpace(Point3D point)
        {
            var size = canvasView.RenderSize;
            int width = (int)size.Width;
            int height = (int)size.Height;

            Point3D temp = new Point3D(point.X, point.Y, point.Z);
            temp.Y = -temp.Y; // Fix Y
            temp.rotate(0.5f,2f,0.2f);

            // Simple transform ignoring the 3d part
            return new SkiaSharp.SKPoint(temp.X*50+ (float)(width/2), temp.Y*50 + (float)(height/2));
        }

        /// <summary>
        /// Creates all objects seen on the scene
        /// </summary>
        public void InitObjects()
        {
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

            canvasView.InvalidateVisual();
        }

        /// <summary>
        /// Calculates new positions of points
        /// </summary>
        public void FixedUpdate(float deltaTime)
        {
            lines.ForEach((line) =>
            {
                
                Point3D temp = line.start;
                line.start = temp.rotate(0, 0f*deltaTime, 1f*deltaTime);
                
                temp = line.end;
                line.end = temp.rotate(0, 0f*deltaTime, 1f*deltaTime);
            });
        }

        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear();

            lines.ForEach((line) => {
                canvas.DrawLine(
                    ToScreenSpace(line.start),
                    ToScreenSpace(line.end),
                    SkiaKit.blackPaint);
                }
            );

            // X
            canvas.DrawLine(
                ToScreenSpace(new Point3D(2, 0, 0)),
                ToScreenSpace(new Point3D(-2, 0, 0)),
                SkiaKit.redPaint
            );


            // Y
            canvas.DrawLine(
                ToScreenSpace(new Point3D(0, 2, 0)),
                ToScreenSpace(new Point3D(0, -2, 0)),
                SkiaKit.greenPaint
            );

            // Z
            canvas.DrawLine(
                ToScreenSpace(new Point3D(0, 0, 2)),
                ToScreenSpace(new Point3D(0, 0, -2)),
                SkiaKit.greenPaint
            ); ;
        }
    }
}
