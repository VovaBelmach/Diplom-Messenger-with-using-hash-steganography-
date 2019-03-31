using LibForEncrypt;
using Messender.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Messender.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void Save()
        {
            db.SaveChanges();
        }

        public string GetUserToId()
        {
            var itemUser = HttpContext.Current.User.Identity.GetUserId();

            var friendFirst = db.FriendShips.FirstOrDefault( x => x.IdFirstUser == itemUser );
            if (friendFirst == null)
            {
                friendFirst = db.FriendShips.FirstOrDefault( x => x.IdSecondUser == itemUser );
                return friendFirst.IdFirstUser;
            }
            else
            {
                return friendFirst.IdSecondUser;
            }
        }

        public void SendMessage( string message )
        {
            var id = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.First( X => X.Id == id );
            var userName = message.Substring( 0, message.IndexOf( ": " ) );
            var hash = message.Substring( message.IndexOf( ": " ) + 2 );
            var byteImages = db.SecretImages.First( X => X.Id == hash ).Images;
            var listImages = new List<string>();
            byteImages.ForEach( x =>
            {
                listImages.Add( x.UrlToImage  );
            } );
            var newMessage = new HashSteganography().GetMessage( listImages );
            Clients.Client( user.ConnectionIdentifier ).addMessage(userName + ": " + newMessage, listImages.ToArray() );
        }

        public void SendChatMessage( string toUserID, string seller, string message )
        {
            var thisUser = HttpContext.Current.User.Identity.GetUserId();
            var userId = GetUserToId();

            var id = int.Parse( toUserID );
            var friendship = db.FriendShips.First( x => x.IdFriendShip == id );
            if (friendship == null)
            {
                Clients.Caller.showErrorMessage( "Could not find that user." );
            }
            else
            {
                var fromUser = db.Users.First( x => x.Id == thisUser );
                var toUser = db.Users.First( x => x.Id == userId ); 

                var secret = new SecretImages
                {
                    Id = Guid.NewGuid().ToString(),
                    Seller = fromUser,
                    Reciever = toUser
                };
                int i = 100000000;
                var images = new HashSteganography().HideInfo(message).Select(x=> new Images
                {
                    Id = secret.Id + i++,
                    UrlToImage = x
                } );
                secret.Images = images.ToList();
                db.SecretImages.Add( secret );
                db.SaveChanges();

                Clients.Client( fromUser.ConnectionIdentifier ).addChatMessage(seller + ": " + message );
                Clients.Client( toUser.ConnectionIdentifier ).addChatMessage( seller + ": " + secret.Id );
            }
        }

        public override Task OnConnected()
        {
            var name = Context.User.Identity.Name;
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.First( x => x.UserName == name );
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = name
                    };
                    db.Users.Add( user );
                }
                user.ConnectionIdentifier = Context.ConnectionId;
                db.SaveChanges();
            }
            return base.OnConnected();
        }
    }
}

