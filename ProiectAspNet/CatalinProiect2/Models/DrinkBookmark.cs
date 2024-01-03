using CatalinProiect2.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalinProiect2.Models
{
 
    public class DrinkBookmark
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // cheie primara compusa (Id, DrinkId, BookmarkId)
        public int Id { get; set; }
        public int? DrinkId { get; set; }
        public int? BookmarkId { get; set; }

        public virtual Drink? Drink { get; set; }
        public virtual Bookmark? Bookmark { get; set; }

        public DateTime BookmarkDate { get; set; }
    }
}
