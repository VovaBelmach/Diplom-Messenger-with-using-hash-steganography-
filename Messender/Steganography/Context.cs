using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace LibForEncrypt
{
    public class Context : DbContext
    {
        public Context() : base("HashSteganography_Image_Test")
        {
        }

        //public Context() : base("HashSteganography")
        //{
        //}

        public DbSet<Model> Models { get; set; }
    }

    public class Model
    {
        [Key]
        public string Name { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
    }
}
