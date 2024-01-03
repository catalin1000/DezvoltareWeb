using CatalinProiect2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;


// PASUL 1 - useri si roluri 

namespace CatalinProiect2.Models
{
    public class ApplicationUser : IdentityUser
    {
        // un user poate posta mai multe comentarii
        public virtual ICollection<Review>? Reviews { get; set; }

        // un user poate sa adauge mai multe bauturi
        public virtual ICollection<Drink>? Drinks { get; set; }


        // un user poate sa creeze mai multe colectii
        public virtual ICollection<Bookmark>? Bookmarks { get; set; }

        // atribute suplimentare adaugate pentru user
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        // variabila in care vom retine rolurile existente in baza de date
        // pentru popularea unui dropdown list
        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }

    }
}
