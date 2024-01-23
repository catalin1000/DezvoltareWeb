using CatalinProiect2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalinProiect2.Models
{
    public class Drink
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele bauturii este obligatoriu")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Continutul bauturii este obligatoriu")]
        [MinLength(10, ErrorMessage = "Continut prea scurta")]
        [StringLength(300, ErrorMessage = "Continut prea lung")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Pretul bauturii este obligatoriu")]
        public float? Price { get; set; }

        public int? Rating { get; set; }

        public string? Photo { get; set; }

        public DateTime? Date { get; set; }


        [Required(ErrorMessage = "Categoria este obligatorie")]
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }

        public virtual ICollection<DrinkOrder>? DrinkOrders { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }
    }

}
