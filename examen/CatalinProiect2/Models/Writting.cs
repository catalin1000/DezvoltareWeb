using CatalinProiect2.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalinProiect2.Models
{
    public class Writting
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? BookId { get; set; }
        public int? AuthorId { get; set; }


        public virtual Book? Book { get; set; }
        public virtual Author? Author { get; set; }

    }
}
