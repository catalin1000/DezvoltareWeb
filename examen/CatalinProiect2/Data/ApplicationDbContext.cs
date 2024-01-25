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
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publishing> Publishings { get; set; }
        public DbSet<Writting> Writtings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // definirea relatiei many-to-many dintre Book si Autor

            base.OnModelCreating(modelBuilder);

            // definire primary key compus
            modelBuilder.Entity<Writting>()
                .HasKey(ab => new { ab.Id, ab.BookId, ab.AuthorId });


            // definire relatii cu modelele Book si Writting (FK)

            modelBuilder.Entity<Writting>()
                .HasOne(ab => ab.Book)
                .WithMany(ab => ab.Writtings)
                .HasForeignKey(ab => ab.BookId);

            modelBuilder.Entity<Writting>()
                .HasOne(ab => ab.Author)
                .WithMany(ab => ab.Writtings)
                .HasForeignKey(ab => ab.AuthorId);
        }
    }
}