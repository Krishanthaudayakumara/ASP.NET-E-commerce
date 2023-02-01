using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OZQ_cart.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [JsonIgnore]
        public List<OrderItem> OrderItems { get; set; }

        public double TotalPrice { get; set; }
        public bool Completed { get; internal set; }
        public double Total { get; internal set; }
        public DateTime OrderDate { get; internal set; }
    }
}
