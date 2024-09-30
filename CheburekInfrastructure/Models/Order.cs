using CheburekInfrastructure.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace CheburekInfrastructure.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public Cart? Cart { get; set; }
        public string PaymentMethod { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Delivery { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

    }


}



