using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Brushes = SixLabors.ImageSharp.Drawing.Processing.Brushes;


namespace GraphicsPractice.Labs._1
{
    /// <summary>
    /// Логика взаимодействия для Lab1Task1.xaml
    /// </summary>
    public partial class Lab1Task1 : Window
    {

        Image<Rgba32> image = null;

        public Lab1Task1()
        {
            InitializeComponent();
        }

        public void Draw()
        {
            if (ImageWrapper != null)
            {
                var width = (int)ImageWrapper.ActualWidth;
                var height = (int)ImageWrapper.ActualHeight;

                using (image = new Image<Rgba32>(width, height))
                {

                    // Draw triangle
                    image.Mutate((x) =>
                    {
                        // Define pens and inks
                        var sss = new Pen(Rgba32.ParseHex("#FFC0CB"), 10, new float[] { 2f, 6f, 8f });

                        var pinkPen = Pens.Solid(Rgba32.ParseHex("#FFC0CB"), 10);
                        var greenPen = Pens.Solid(Rgba32.ParseHex("#228B22"), 10);
                        var blackPen = Pens.Solid(Rgba32.ParseHex("#000000"), 10);
                        var pinkBrush = Brushes.Solid(Rgba32.ParseHex("#C71585"));


                        Image img = Image.Load("Resources\\cat.jpg");
                        img.Mutate((x) =>
                        {
                            x.Resize(
                                128,
                                (x.GetCurrentSize().Height / x.GetCurrentSize().Width) * 128);
                        });
                        var CatBrush = new ImageBrush(img);


                        // Draw a Line
                        x.DrawLines(pinkPen, new PointF[] {
                            new PointF(30, 30),
                            new PointF(100, 30)
                        });

                        // Draw a Triangle
                        x.FillPolygon(CatBrush, new PointF[] {
                            new PointF(50, 50),
                            new PointF(500, 30),
                            new PointF(555, 555),
                        });

                        // Draw a star x y numberofcorners innerradius outerradius
                        Star star = new Star(50, 50, 5, 50, 100);
                        var staroutline = Outliner.GenerateOutline(star, 2, JointStyle.Round);
                        x.Fill(CatBrush, star);

                        // Draw an arc
                        var arc = new Polygon(new CubicBezierLineSegment(new PointF[] {
                            new PointF(10, 400),
                            new PointF(30, 10),
                            new PointF(240, 30),
                            new PointF(300, 400)
                        }));
                        x.Draw(blackPen, arc);

                        // Draw some Ellipse
                        var sector = new EllipsePolygon(200, 200, 10, 20).Scale(5);
                        x.Draw(blackPen, sector);
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
    }
}
