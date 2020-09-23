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

namespace GraphicsPractice.Labs._1
{
    /// <summary>
    /// Логика взаимодействия для Lab1Task2.xaml
    /// </summary>
    public partial class Lab1Task2 : Window
    {
        Image<Rgba32> image = null;

        float scale = 50;
        PointF offset;

        public Lab1Task2()
        {
            InitializeComponent();
        }

        // From coord space to screen space
        public float XToScreen(float x)
        {
            return x*scale + offset.X;
        }
        public float YToScreen(float y)
        {
            return (y*scale*-1) + offset.Y;
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

        public void Draw()
        {
            if (ImageWrapper != null)
            {
                // Get size of the image
                var width = (int)ImageWrapper.ActualWidth;
                var height = (int)ImageWrapper.ActualHeight;

                using (image = new Image<Rgba32>(width, height))
                {
                    // Draw triangle
                    image.Mutate((ctx) =>
                    {
                        offset = new PointF(width/2, height/2);

                        // Define pens and inks
                        var pinkPen = Pens.Solid(Rgba32.ParseHex("#FFC0CB"), 1);
                        var greenPen = Pens.Solid(Rgba32.ParseHex("#228B22"), 1);
                        var blackPen = Pens.Solid(Rgba32.ParseHex("#000000"), 1);
                        var blackTransPen = Pens.Solid(Rgba32.ParseHex("#00005050"), 1);
                        var pinkBrush = Brushes.Solid(Rgba32.ParseHex("#C71585"));
     
                        var points = new List<PointF>();

                        for (int i=1; i< width; i++)
                        {
                            var x = ScreenToX(i);
                            var y = MathF.Pow(x, 2) * MathF.Exp(x);
                            
                            // Draw to screen
                            PointF point = new PointF(i, YToScreen(y));
                            
                            // Eliminate out of bounds exception
                            if (
                                !float.IsInfinity(y) &&
                                MathF.Abs(y) < height*10 
                            )
                            {
                                points.Add(point);
                            }
                        }

                        DrawAxis(ctx, width, height);

                        ctx.DrawLines(blackPen, points.ToArray());
                    });

                    // Set the source
                    ImageControl.Source = Helpers.ImageToBitmap(image);
                }
            }
        }

        private void DrawScene(object sender, RoutedEventArgs e)
        {
            Draw();
        }

        public void DrawAxis(IImageProcessingContext ctx, float width, float height)
        {
            var font = SixLabors.Fonts.SystemFonts.CreateFont("Arial", 20, SixLabors.Fonts.FontStyle.Regular);

            var blackPen = Pens.Solid(Rgba32.ParseHex("#000000"), 1);
            var blackTransPen = Pens.Solid(Rgba32.ParseHex("#00005050"), 1);

            // Scale the cell size up until it's sane
            var cellSize = 1;
            while (cellSize * scale < 50)
            {
                cellSize++;
            }

            // Draw other thing 
            for (int i = 1; i < width; i++)
            {
                var x = ScreenToX(i);

                // Draws every thing
                if (x % cellSize == 0)
                {
                    ctx.DrawLines(blackTransPen, new PointF[] { new PointF(i, 0), new PointF(i, height) });
                    ctx.DrawText(x.ToString(), font, new Rgba32(22, 33, 45, 66), new PointF(i, YToScreen(0)));
                }
            }

            // Draw other thing 
            for (int i = 1; i < height; i++)
            {
                var y = ScreenToY(i);

                if (y % cellSize == 0)
                {
                    ctx.DrawLines(blackTransPen, new PointF[] { new PointF(0, i), new PointF(width, i) });

                    // skip drawing 0 twice
                    if (y != 0)
                    {
                        ctx.DrawText(y.ToString(), font, new Rgba32(22, 33, 45, 66), new PointF(XToScreen(0), i));
                    }
                }
            }

            // Draw x line
            for (int i = 1; i < width; i++)
            {
                var x = ScreenToX(i);

                if (x == 0)
                {
                    ctx.DrawLines(blackPen, new PointF[] { new PointF(i, 0), new PointF(i, height) });
                }
            }

            // Draw other thing 
            for (int i = 1; i < height; i++)
            {
                var y = ScreenToY(i);

                if (y == 0)
                {
                    ctx.DrawLines(blackPen, new PointF[] { new PointF(0, i), new PointF(width, i) });
                }
            }

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
        }
    }
}
