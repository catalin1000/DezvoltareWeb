using System.ComponentModel.DataAnnotations;

namespace CatalinProiect2.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string? Content { get; set; }
        public DateTime? Date { get; set; }


        [Required(ErrorMessage = "Numarul de stele este obligatoriu")]
        public int? Rating { get; set; }


        public int? DrinkId { get; set; }
        public virtual Drink? Drink { get; set; }

        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
