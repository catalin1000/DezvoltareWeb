using CatalinProiect2.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalinProiect2.Models
{
    public class DrinkOrder
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? DrinkId { get; set; }
        public int? OrderId { get; set; }


        public virtual Drink? Drink { get; set; }
        public virtual Order? Order { get; set; }

    }
}
