using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Windows.Media.Imaging;

namespace GraphicsPractice
{
    public static class Helpers
    {
        public static BitmapImage ImageToBitmap(Image image)
        {
            // Possible memory leak?
            // Create memory stream
            MemoryStream ms = new MemoryStream();

            // Create encoder
            var encoder = new BmpEncoder
            {
                BitsPerPixel = BmpBitsPerPixel.Pixel32,
                SupportTransparency = true
            };

            // Save the image to stream
            image.Save(ms, encoder);

            // Create a bmp image
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            return bitmap;
        }

        public static Image<Rgba32> SetupCanvas(int width, int height)
        {
            return new Image<Rgba32>(width, height);
        }     
    }
}
