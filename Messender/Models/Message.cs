using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Messender.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMessage { get; set; }

        [ForeignKey("FriendShip")]
        public int IdFrientShip { get; set; }

        public virtual ApplicationUser FromUser { get; set; }

        public virtual ApplicationUser ToUser { get; set; }

        public string TextMessage { get; set; }

        public List<string> Urls { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:d'th 'MMMM' 'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataSendMessage { get; set; }

        public FriendShip FriendShip { get; set; }
    }
}
 