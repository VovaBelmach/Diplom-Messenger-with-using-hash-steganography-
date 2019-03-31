using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Messender.Models
{
    public class FriendShip
    {
        [Key]
        public int IdFriendShip { get; set; }

        [ForeignKey("FirstUser")]
        public string IdFirstUser { get; set; }
        [ForeignKey("SecondUser")]
        public string IdSecondUser { get; set; }

        public bool Access { get; set; }

        public virtual ApplicationUser FirstUser { get; set; }
        public virtual ApplicationUser SecondUser { get; set; }
    }
}