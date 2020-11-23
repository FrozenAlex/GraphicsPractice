using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsPractice.Common
{
    static class DrawingKit
    {
        public static SolidBrush whiteBrush = Brushes.Solid(Rgba32.ParseHex("#FFFF"));
        public static SolidBrush pinkBrush = Brushes.Solid(Rgba32.ParseHex("#C71585"));

        public static Pen pinkPen = Pens.Solid(Rgba32.ParseHex("#C0C0FF"), 2);
        public static Pen greenPen = Pens.Solid(Rgba32.ParseHex("#228B22"), 1);
        public static Pen blackPen = Pens.Solid(Rgba32.ParseHex("#000000"), 1);
        public static Pen blackTransPen = Pens.Solid(Rgba32.ParseHex("#00005050"), 1);
    }
}
