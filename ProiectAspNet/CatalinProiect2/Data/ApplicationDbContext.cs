using CatalinProiect2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


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
        public DbSet<Order> Orders { get; set; }
        public DbSet<DrinkOrder> DrinkOrders{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // definirea relatiei many-to-many dintre Article si Bookmark

            base.OnModelCreating(modelBuilder);

            // definire primary key compus
            modelBuilder.Entity<DrinkOrder>()
                .HasKey(ab => new { ab.Id, ab.DrinkId, ab.OrderId });


            // definire relatii cu modelele Bookmark si Article (FK)

            modelBuilder.Entity<DrinkOrder>()
                .HasOne(ab => ab.Drink)
                .WithMany(ab => ab.DrinkOrders)
                .HasForeignKey(ab => ab.DrinkId);

            modelBuilder.Entity<DrinkOrder>()
                .HasOne(ab => ab.Order)
                .WithMany(ab => ab.DrinkOrders)
                .HasForeignKey(ab => ab.OrderId);
        }
    }
}