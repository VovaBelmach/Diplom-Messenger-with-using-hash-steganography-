using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Messender.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:d'th 'MMMM' 'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Sity { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(50)]
        public string County { get; set; }
        [StringLength(20)]
        public string Gender { get; set; }
        public byte[] Image { get; set; }

        public string ConnectionIdentifier { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Messenger16", throwIfV1Schema: false)
        {
        }

        public DbSet<FriendShip> FriendShips { get; set; }
        public DbSet<SecretImages> SecretImages { get; set; }
        public DbSet<Images> Images { get; set; }
        

        public static ApplicationDbContext Create()
        {
            Database.SetInitializer( new CreateDatabaseIfNotExists<ApplicationDbContext>() /*new DropCreateDatabaseIfModelChanges<ApplicationDbContext>()*/ );
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FriendShip>()
                .HasRequired<ApplicationUser>(fs => fs.FirstUser)
                .WithMany()
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<FriendShip>()
                .HasRequired(fs => fs.SecondUser)
                .WithMany()
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Message>()
               .HasRequired(ch => ch.FriendShip)
               .WithMany()
               .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}