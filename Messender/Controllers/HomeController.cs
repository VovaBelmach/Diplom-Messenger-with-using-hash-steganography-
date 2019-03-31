using LibForEncrypt;
using Messender.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Messender.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]       
        public ActionResult Index()
        {
            //new HashSteganography().GenerateImages();
            //new HashSteganography().InitDb();
            var idUser = User.Identity.GetUserId();
            var id = db.Users.FirstOrDefault(p => p.Id == idUser);
            return View(id);
        }
    }
}