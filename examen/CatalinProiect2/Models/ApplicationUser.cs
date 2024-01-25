using CatalinProiect2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalinProiect2.Models
{
    public class ApplicationUser : IdentityUser
    {

        public virtual ICollection<Book>? Books { get; set; }

        public virtual ICollection<Publishing>? Publishings { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }


    }
}
