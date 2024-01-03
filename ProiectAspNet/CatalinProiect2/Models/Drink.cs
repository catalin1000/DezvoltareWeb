using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalinProiect2.Models
{
    public class Drink
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(100, ErrorMessage = "Titlul nu poate avea mai mult de 100 de caractere")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Continutul articolului este obligatoriu")]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Categoria este obligatorie")]


        // o bautra este adaugata de catre un user (manager/patronu)
        public string? UserId { get; set; }

        // PASUL 6 - useri si roluri
        public virtual ApplicationUser? User { get; set; }


        // un articol are asociata o categorie
        public int? CategoryId { get; set; } // cheie externa

        public virtual Category? Category { get; set; } // tabelul la care face referire cheia externa

        // un articol poate avea o colectie de comentarii
        public virtual ICollection<Review>? Reviews { get; set; }


        [NotMapped] // sa nu se creeze in baza de date
        public IEnumerable<SelectListItem>? Categ { get; set; }

        // relatia many-to-many dintre Drink si Bookmark
        public virtual ICollection<DrinkBookmark>? DrinkBookmarks { get; set; }
    }

}


