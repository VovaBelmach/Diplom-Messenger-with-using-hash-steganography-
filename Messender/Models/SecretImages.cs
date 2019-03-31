using System.Collections.Generic;

namespace Messender.Models
{
    public class SecretImages
    {
        public string Id { get; set; }
        public virtual List<Images> Images { get; set; }
        public virtual ApplicationUser Seller { get; set; }
        public virtual ApplicationUser Reciever { get; set; }
    }
}