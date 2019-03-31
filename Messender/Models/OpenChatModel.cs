using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messender.Models
{
    public class OpenChatModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<Message> Messages { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public byte[] Image { get; set; }
        public byte[] Image1 { get; set; }
    }
}