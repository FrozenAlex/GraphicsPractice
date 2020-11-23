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

namespace GraphicsPractice.Labs._1
{
    /// <summary>
    /// Логика взаимодействия для Lab1Task3.xaml
    /// </summary>
    public partial class Lab1Task3 : Window
    {

        Image<Rgba32> image = null;

        float scale = 10;
        float a = 5;
        PointF offset;

        public Lab1Task3()
        {
            InitializeComponent();
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

        private void DrawScene(object sender, RoutedEventArgs e)
        {
            Draw();
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
                        offset = new PointF(width / 2, height / 2);

                        // Define pens and inks
                        var pinkPen = Pens.Solid(Rgba32.ParseHex("#C0C0FF"), 2);
                        var greenPen = Pens.Solid(Rgba32.ParseHex("#228B22"), 1);
                        var blackPen = Pens.Solid(Rgba32.ParseHex("#000000"), 1);
                        var blackTransPen = Pens.Solid(Rgba32.ParseHex("#00005050"), 1);
                        var pinkBrush = Brushes.Solid(Rgba32.ParseHex("#C71585"));

                        var points = new List<PointF>();

                        try
                        {
                            // Get parameter a
                            float a = float.Parse(ParamA.Text);
                            // Quality and scale
                            float quality = (float)ParamB.Value;
                            scale = (float)ScaleSlider.Value;

                            if (AxisCheckbox.IsChecked == true)
                            {
                                DrawAxis(ctx, width, height);
                            }


                            for (float i = -MathF.PI; i < MathF.PI; i += quality)
                            {
                                // x = a(t^2 - 1) / (t^2 + 1), y = at(t^2 - 1) / (t^2 + 1), t- infinite, а > 0
                                // t = i
                                var t = i;

                                // Find coords
                                var x = (a * (MathF.Pow(t, 2) - 1)) / (MathF.Pow(t, 2) + 1);
                                var y = a * t * (MathF.Pow(t, 2) - 1) / (MathF.Pow(t, 2) + 1);

                                // Convert to screen space
                                x = XToScreen(x);
                                y = YToScreen(y);

                                // Draw on the screen
                                PointF point = new PointF(x, y);

                                // Eliminate out of bounds exception
                                if (
                                    !float.IsInfinity(y) &&
                                    MathF.Abs(y) < height &&
                                    !float.IsInfinity(x) &&
                                    MathF.Abs(x) < width
                                )
                                {
                                    points.Add(point);
                                }
                            }

                            ctx.DrawLines(pinkPen, points.ToArray());
                        }
                        catch (Exception err)
                        {

                        }
                    });

                    // Set the source
                    ImageControl.Source = Helpers.ImageToBitmap(image);
                }
            }
        }

        public void DrawAxis(IImageProcessingContext ctx, float width, float height)
        {
            var font = SystemFonts.CreateFont("Arial", 20, SixLabors.Fonts.FontStyle.Regular);

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
    }
}
