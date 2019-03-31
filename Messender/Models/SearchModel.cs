using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Messender.Models
{
    public class SearchModel
    {
        public List<ApplicationUser> Username;
        public SelectList Firstname;
        public string SearchFirstame { get; set; }
    }
}