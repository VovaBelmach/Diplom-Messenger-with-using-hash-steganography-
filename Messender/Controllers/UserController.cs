using Messender.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Messender.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DetailsUserInformation()
        {
            var idUser = User.Identity.GetUserId();
            var itemUser = db.Users.FirstOrDefault(p => p.Id == idUser);

            return PartialView("DetailsUserInformation", itemUser);
        }

        [AllowAnonymous]
        public ActionResult DetailsUserInfo(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            var itemUser = db.Users.FirstOrDefault(s => s.Id == user.Id.ToString());
            ViewBag.Id = User.Identity.GetUserId();

            return PartialView(itemUser);
        }

        [AllowAnonymous]
        public ActionResult EditUserInformation()
        {
            var idUser = User.Identity.GetUserId();
            var itemUser = db.Users.FirstOrDefault(p => p.Id == idUser);

            return PartialView("EditUserInformation", itemUser);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserInformation(ApplicationUser itemUser)
        {
            ApplicationUser user = db.Users.Find(itemUser.Id);

            if (ModelState.IsValid)
            {
                user.FirstName = itemUser.FirstName;
                user.LastName = itemUser.LastName;
                user.UserName = itemUser.UserName;
                user.DateOfBirth = itemUser.DateOfBirth;
                user.Address = itemUser.Address;
                user.Sity = itemUser.Sity;
                user.State = itemUser.State;
                user.County = itemUser.County;
                user.Gender = itemUser.Gender;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult EditUserImage()
        {
            var idUser = User.Identity.GetUserId();
            var itemUser = db.Users.FirstOrDefault(p => p.Id == idUser);

            return PartialView("EditUserImage", itemUser);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserImage(ApplicationUser itemUser, HttpPostedFileBase uploadImage)
        {
            ApplicationUser user = db.Users.Find(itemUser.Id);

            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }

                user.Image = imageData;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}