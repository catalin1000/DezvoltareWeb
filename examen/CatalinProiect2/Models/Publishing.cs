using System.ComponentModel.DataAnnotations;

namespace CatalinProiect2.Models
{
    public class Publishing
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; } // cheie externa(al cui uitlizator e)
        public virtual ApplicationUser? User { get; set; }


        // o editura are mai multi autori
        public virtual ICollection<Author>? Authors { get; set; }
    }
}
