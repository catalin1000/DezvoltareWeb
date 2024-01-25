using CatalinProiect2.Models;
using System.ComponentModel.DataAnnotations;

namespace CatalinProiect2.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele autorului")]
        public string? AuthorName { get; set; }
        public virtual ICollection<Book>? Books { get; set; }

        public virtual ICollection<Writting>? Writtings { get; set; }

    }
}
