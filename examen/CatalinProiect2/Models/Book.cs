using CatalinProiect2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalinProiect2.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele cartii")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Cartea are un pret")]
        public float? Price { get; set; }

        [Required(ErrorMessage = "Categoria este obligatorie")]
        public int? AuthorId { get; set; }
        public virtual Author? Author { get; set; }

        public virtual ICollection<Writting>? Writtings { get; set; }

        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }


    }

}
