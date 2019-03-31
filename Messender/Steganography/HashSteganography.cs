using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibForEncrypt
{
    public class HashSteganography
    {
        private static Context con;
        private readonly Random rand;
        private const string pathToImages = @"F:\Diplom\Image_Image\";
        private readonly MD5CryptoServiceProvider hasher;

        public HashSteganography()
        {
            con = new Context();
            rand = new Random();
            hasher = new MD5CryptoServiceProvider();
        }

        public List<string> HideInfo( string message )
        {
            var images = new List<string>();
            message.ToList().ForEach( x =>
            {
                var hex = Converter.ToHex( x );
                var hashes = con.Models.Where( d => d.Hash == hex ).ToList();
                images.Add( hashes[rand.Next( 0, hashes.Count )].Name );
            } );
            return images;
        }

        public string GetMessage( List<string> bmp )
        {
            var str = new StringBuilder();
            bmp.ForEach( x =>
            {
                var bytes = hasher.ComputeHash( Converter.ToBytes( new Bitmap(x) ) );
                var result = Converter.ToString( bytes ).Substring( 0, 3 );
                str.Append( (char)Convert.ToInt32( result, 16 ) );
            });
            return str.ToString();
        }

        public void InitDb()
        {
            var models = con.Models.ToList();

            Directory.GetFiles( pathToImages ).ToList().ForEach( x =>
            {
                byte[] bt = Converter.ToBytes( new Bitmap( x ));
                var hash = Converter.ToString(hasher.ComputeHash(bt)).Substring( 0, 3 );
                if (models.Count( y => y.Hash.Substring( 0, 3 ) == hash ) < 3)
                {
                    models.Add(new Model
                    {
                        Hash = hash,
                        Name = x
                    });
                   con.Models.Add( new Model
                    {
                        Hash = hash,
                        Name = x
                    } );
                }
            } );
            con.SaveChanges();
        }

        public void GenerateImages()
        {
            Directory.CreateDirectory( pathToImages );
            Enumerable.Range( 0, 255 ).ToList().ForEach( x =>
            {
                Parallel.For( 0, 255, y =>
                {
                    var bitmap = new Bitmap( 1, 1 );
                    bitmap.SetPixel( 0, 0, Color.FromArgb( 255, x, y, 255 ) );
                    var a = bitmap.GetPixel( 0, 0 );
                    bitmap.Save( pathToImages +"255," +x + "," + y + ",255.bmp" );
                } );
            } );
        }
    }
}
