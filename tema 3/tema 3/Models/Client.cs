using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tema3.Models.Base;

namespace Tema3.Models
{
    public class Client : BaseEntity
    {
        public int clientId { get; set; }
        public string clientName { get; set; }
        public DateTime subscribtionDate { get; set; }

        public List<Movie> purchaseHistory { get; set; }
    }
}