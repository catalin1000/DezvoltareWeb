using CatalinProiect2.Models;
using CatalinProiect2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


// PASUL 3 - useri si roluri

namespace CatalinProiect2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<DrinkBookmark> DrinkBookmarks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // definirea relatiei many-to-many dintre Article si Bookmark

            base.OnModelCreating(modelBuilder);

            // definire primary key compus
            modelBuilder.Entity<DrinkBookmark>()
                .HasKey(ab => new { ab.Id, ab.DrinkId, ab.BookmarkId });


            // definire relatii cu modelele Bookmark si Article (FK)

            modelBuilder.Entity<DrinkBookmark>()
                .HasOne(ab => ab.Drink)
                .WithMany(ab => ab.DrinkBookmarks)
                .HasForeignKey(ab => ab.DrinkId);

            modelBuilder.Entity<DrinkBookmark>()
                .HasOne(ab => ab.Bookmark)
                .WithMany(ab => ab.DrinkBookmarks)
                .HasForeignKey(ab => ab.BookmarkId);
        }
    }
}