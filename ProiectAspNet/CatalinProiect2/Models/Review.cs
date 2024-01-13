using System.ComponentModel.DataAnnotations;

namespace CatalinProiect2.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Continutul comentariului este obligatoriu")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Numarul de stele este obligatoriu")]
        public int? Rating { get; set; }

        public DateTime Date { get; set; }

        // un comentariu apartine unui articol
        public int? DrinkId { get; set; }

        // un comentariu este postat de catre un user
        public string? UserId { get; set; }

        // PASUL 6 - useri si roluri
        public virtual ApplicationUser? User { get; set; }

        public virtual Drink? Drink { get; set; }
    }

}
