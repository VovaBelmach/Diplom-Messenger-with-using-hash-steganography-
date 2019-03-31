using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Messender.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using LibForEncrypt;
using System.Threading.Tasks;

namespace Messender.Controllers
{
    public class FriendController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Notification()
        {
            var id = User.Identity.GetUserId();
            List<FriendShip> notifiction = db.FriendShips.Where(x => x.IdSecondUser == id && x.Access == false).ToList();
            return PartialView(notifiction);
        }

        public ActionResult FriendList()
        {
            var idUser = User.Identity.GetUserId();

            IEnumerable<FriendShip> friendShip = db.FriendShips.Where(x => x.IdFirstUser == idUser || x.IdSecondUser == idUser);
            ViewBag.id = idUser;

            return View(friendShip.ToList());
        }

        [AllowAnonymous]
        public ActionResult DetailsFriendInfo(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            var itemUser = db.Users.FirstOrDefault(s => s.Id == user.Id.ToString());
            ViewBag.Id = User.Identity.GetUserId();

            return PartialView(itemUser);
        }

        public ActionResult SendRequest(string friendFirst, string friendSecond)
        {
            var idUser = User.Identity.GetUserId();
            if (friendSecond == idUser)
            {
                return RedirectToAction("Index", "Home");
            }


            else if (db.FriendShips.Count(x => x.IdFirstUser == friendFirst && x.IdSecondUser == friendSecond ||
                                               x.IdFirstUser == friendSecond && x.IdSecondUser == friendFirst) == 0)
            {
                FriendShip frienship = new FriendShip();
                frienship.FirstUser = db.Users.Find(friendFirst);
                frienship.SecondUser = db.Users.Find(friendSecond);
                frienship.Access = false;
                db.FriendShips.Add(frienship);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Add(int id)
        {
            db.FriendShips.First(x => x.IdFriendShip == id).Access = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult DeleteFriend(int id)
        {
            FriendShip friendShip = db.FriendShips.Find(id);
            if (friendShip == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = friendShip.IdFriendShip;
            return PartialView();
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FriendShip friendShip = db.FriendShips.Find(id);
            if (friendShip == null)
            {
                return HttpNotFound();
            }
            db.FriendShips.Remove(friendShip);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DetailsFriendInformation()
        {
            var idUser = User.Identity.GetUserId();
            var itemUser = db.Users.FirstOrDefault(p => p.Id == idUser);

            return PartialView("DetailsFriendInformation", itemUser);
        }

        public async Task<ActionResult> SearchUserName(string searchFirstame, string name)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                IQueryable<string> firstnameQuery = from m in context.Users
                                                    orderby m.FirstName
                                                    select m.FirstName;

                var result = from m in context.Users select m;

                if (!String.IsNullOrEmpty(name))
                {
                    result = result.Where(s => s.UserName.Contains(name));
                }

                if (!String.IsNullOrEmpty(searchFirstame))
                {
                    result = result.Where(x => x.FirstName.Contains(searchFirstame));
                }

                var search = new SearchModel();
                search.Firstname = new SelectList(await firstnameQuery.Distinct().ToListAsync());
                search.Username = await result.ToListAsync();

                return View(await result.ToListAsync());
            }
        }

        public int GetFollowRequestsForUser()
        {
            var id = User.Identity.GetUserId();
            var result = db.FriendShips.Where(x => x.IdSecondUser == id && x.Access == false).Count();
            return result;
        }

        public ActionResult OpenChat(string ID)
        {
            //определение текущего пользователя
            var id = User.Identity.GetUserId();
            //поиск username пользователя и передача в chatHub
            ApplicationUser app1 = db.Users.First(x => x.Id == id);
            ViewBag.Seller = app1.UserName;

            //кому отправляет
            ApplicationUser app = db.Users.FirstOrDefault(x => x.Id == ID);
            //поиск id дружбы и передача в chatHub
            FriendShip fr = db.FriendShips.First(x => x.IdFirstUser == id && x.IdSecondUser == ID || x.IdFirstUser == ID && x.IdSecondUser == id);
            ViewBag.Id = fr.IdFriendShip;
            //return View(model: app.FirstName + " " + app.LastName);

            //отображение сообщений
            var listSecretMessages = db.SecretImages.Where(x => x.Reciever.Id == app1.Id && x.Seller.Id == app.Id).ToList();
            var listMessage = new List<Message>();
            listSecretMessages.ForEach(x =>
            {
                var images = new List<string>();
                if (x.Images != null)
                {
                    x.Images.ForEach(y =>
                    {
                        images.Add(y.UrlToImage);
                    });

                    listMessage.Add(
                        new Message
                        {
                            FromUser = x.Seller,
                            ToUser = x.Reciever,
                            TextMessage = new HashSteganography().GetMessage(images),
                            Urls = images
                        });
                    db.SecretImages.Remove(x);
                }
            });
            db.SaveChanges();
            var model = new OpenChatModel
            {
                Id = app.Id,
                UserName = app.UserName,
                Messages = listMessage,
                FirstName = app.FirstName,
                LastName = app.LastName,
                Image = app.Image,
                Image1 = app1.Image
            };
            return PartialView(model);
        }
    }
}