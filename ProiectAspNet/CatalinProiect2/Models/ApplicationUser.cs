using CatalinProiect2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalinProiect2.Models
{
    public class ApplicationUser : IdentityUser
    {

        public virtual ICollection<Drink>? Drinks { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }

    }
}
