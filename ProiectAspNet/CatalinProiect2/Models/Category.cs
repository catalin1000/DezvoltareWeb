using System.ComponentModel.DataAnnotations;

namespace CatalinProiect2.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatoriu")]
        public string CategoryName { get; set; }

        public virtual ICollection<Drink>? Drinks { get; set; }
    }

}
