using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PortalSpolecznosciowy.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Płeć")]
        public string Sex { get; set; }
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string FullName { get; set; }

        [Display(Name = "Avatar")]
        public string Image { get; set; }

        public virtual ICollection<Friend> Friend { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Like> Like { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("PortalSpolecznosciowyConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Friend> Friend { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Like> Like { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);

            //one-to-many user-friends
            modelBuilder.Entity<Friend>()
                        .HasRequired<ApplicationUser>(s => s.User)
                        .WithMany(s => s.Friend)
                        .HasForeignKey(s => s.UserId);

            //one-to-many user-posts
            modelBuilder.Entity<Post>()
            .HasRequired<ApplicationUser>(s => s.User)
            .WithMany(s => s.Post)
            .HasForeignKey(s => s.UserId);

            //one-to-many user-comments
            modelBuilder.Entity<Comment>()
            .HasRequired<ApplicationUser>(s => s.User)
            .WithMany(s => s.Comment)
            .HasForeignKey(s => s.UserId)
            .WillCascadeOnDelete(false);

            //one-to-many user-likes
            modelBuilder.Entity<Like>()
            .HasRequired<ApplicationUser>(s => s.User)
            .WithMany(s => s.Like)
            .HasForeignKey(s => s.UserId)
            .WillCascadeOnDelete(false);
        }

    }
}