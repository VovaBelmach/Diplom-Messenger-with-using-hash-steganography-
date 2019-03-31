using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForEncrypt
{
    class Converter
    {
        public static string ToString( byte[] hash )
        {
            StringBuilder sb = new StringBuilder();
            hash.ToList().ForEach( x =>
            {
                sb.Append( x.ToString( "x2" ).ToLower() );
            } );
            return sb.ToString();
        }

        public static string ToHex( char symbol )
        {
            var str = ((int) symbol).ToString("X");
            return str.Length == 3 ? str : '0' + str;
        }

        public static byte[] ToByte( Color color )
        {
            return new []
            {
                color.A,
                color.R,
                color.G,
                color.B
            };
        }

        public static byte[] ToBytes( Bitmap img )
        {
            MemoryStream memoryStream = new MemoryStream();
            Bitmap image = new Bitmap(img);
            image.Save( memoryStream, ImageFormat.Bmp );
            return memoryStream.ToArray();
        }

        public static List<Bitmap> ToImages(List<string> imageNames)
        {
            var images = new List<Bitmap>();
            Enumerable.Range(0, imageNames.Count).ToList().ForEach(x =>
            {
                images.Add(new Bitmap(imageNames[x]));
            });
            return images;
        }
    }
}
